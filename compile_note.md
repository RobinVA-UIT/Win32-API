
## 1. Compile flag

Because I'm writing C# on Linux, but utilizing Win32 APIs, I need to setup Windows 64-bit environment on the Clangd in Neovim.

Create a file called `compile_flags.txt` in the project folder and paste this:

```
-target

x86_64-w64-mingw32
```

* The target architecture is 64-bit
* w64(vendor): Windows 64-bit
* mingw32 (OS/environment): Minimalist GNU for Windows

## 2. .csproj file

Copy-paste:

```html
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework> <!-- I'm using .NET 10.0 -->
    <ImplicitUsings>disable</ImplicitUsings>
    <Nullable>disable</Nullable>
    
    <PublishSingleFile>true</PublishSingleFile>
  </PropertyGroup>
</Project>

```

## 3. Commands to run

1. Clean temp files and old builds (just in case you need to rebuild)

```bash
dotnet clean

rm -rf bin/ obj/
```

2. Compile

```bash
dotnet publish -c Release -r win-x64
```

3. Check file size
```bash
ls -lh bin/Release/net10.0/win-x64/publish/
```
