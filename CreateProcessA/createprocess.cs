using System;
using System.Runtime.InteropServices;

class Win32
{
    //Import Kernel32.dll via P/Invoke
    [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
    public static extern bool CreateProcessA(
        string lpApplicationNName,
        string lpCommandLine,
        ref SECURITY_ATTRIBUTES lpProcessAttributes,
        ref SECURITY_ATTRIBUTES lpThreadAttributes,
        bool bInheritHandles,
        uint dwCreationFlags,
        IntPtr lpEnvironment,
        string lpCurrentDirectory,
        ref STARTUPINFO lpStartupInfo,

        out PROCESS_INFORMATION lpProcessInformation
    );

    [DllImport("kernel32.dll")]
    public static extern bool CloseHandle(
        IntPtr hObject //Handle
    );

}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct STARTUPINFO
{
    public uint cb; //size of the struct, in bytes
    public string lpReserved; //Reversed; must be NULL
    public string lpDesktop; //The name of the desktop (or both desktop and window station for this process)
    public string lpTitle; //Title name on the title bar of the console
    public uint dwX; // X offset of the upperleft corner    
    public uint dwY; //Y offset of the upperleft corner
    public uint dwXSize; //The width of the window
    public uint dwYSize; //The height of the window

    //===IF A NEW CONSOLE WINDOW IS CREATED IN A CONSOLE PROCESS=== 
    public uint dwXCountChars; //Screen buffer width in char columns (if dwFlags specifies STARTF_USECOUNTCHARS)
    public uint dwYCountChar; //Screen buffer height in char columns (if dwFlags specifies STARTF_USECOUNTCHARS)
    public uint dwFillAttribute; //Initial text and background colors (if dwFlags specifies STARTF_USEFILLATTRIBUTE)
                                 //<BACKGROUND/FOREGROUND>_<RED/GREEN/BLUE> 
                                 //===END===

    public uint dwFlags; //Determines whether certain STARTUPINFO members are used when creates a window
    public ushort wShowWindow; //Can be any of the values in nCmdShow parameter for ShowWindow func (STARTF_USESHOWWINDOW)
    public ushort cbReserved2; //Reserved for use by C Run-time (CRT); must be zero
    public IntPtr lpReserved2; //Reserved for use by CRT; must be NULL

    public IntPtr hStdInput;//Std input handle for process (STARTF_USESTDHANDLES), else, default is keyboard buffer
                            //Specifies a hotkey value (STARTF_USEHOTKEY) (not sure what this is)
    public IntPtr hStdOutput;//Std output handle for process (STARTF_USESTDHANDLES), else, def is console screen
    public IntPtr hStdError; //Std error handle (STARTF_USESTDHANDLES), else, console windows's buffer
}

[StructLayout(LayoutKind.Sequential)]
public struct PROCESS_INFORMATION
{
    public IntPtr hProcess; //Handle to the newly created process
    public IntPtr hThread; //Handle to the primary thread of the newly created process
    public uint dwProcessId; //PID
    public uint dwThreadId; //TID
}

[StructLayout(LayoutKind.Sequential)]
public struct SECURITY_ATTRIBUTES
{
    public uint nLength; //The size, in bytes, of this struct
    public IntPtr lpSecurityDescriptor; //ptr to a SECURITY_DESCRIPTOR struct that controls access to the obj
                                        //If NULL, the obj is assigned the def sec desc associated with the access token of
                                        //the calling process.
    public bool bInheritHandle; //If the new process inherits the handle when the new process is created
}

class Program
{
    static void Main()
    {
        STARTUPINFO si = new STARTUPINFO();

        si.cb = (uint)Marshal.SizeOf(si);

        PROCESS_INFORMATION pi = new PROCESS_INFORMATION();

        SECURITY_ATTRIBUTES pa = new SECURITY_ATTRIBUTES();
        pa.nLength = (uint)Marshal.SizeOf(pa);
        SECURITY_ATTRIBUTES ta = new SECURITY_ATTRIBUTES();
        ta.nLength = (uint)Marshal.SizeOf(ta);

        //Flag
        uint new_console_flag = 0x00000010;



        string targetApp = @"C:\Windows\System32\notepad.exe";

        bool success = Win32.CreateProcessA(
            targetApp,
            null,
            ref pa,
            ref ta,
            false,
            new_console_flag,
            IntPtr.Zero,
            null,
            ref si,
            out pi
        );


        if (success)
        {
            Console.WriteLine($"[+] Successful. PID: {pi.dwProcessId}. Process handle (IntPtr) = {pi.hProcess}");
            Win32.CloseHandle(pi.hProcess);
            Win32.CloseHandle(pi.hThread);
        }
        else
        {
            Console.WriteLine($"[+] Unsuccessful.");
        }
    }
}
