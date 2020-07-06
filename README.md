# WslPathShellExt
Shell extension for copying file and directory pathes to clipboard in WSL Linux mount format.

Converts your Windows pathes to a format your WSL Linux understands:

```
"D:\ProjectsGitHub\WslPathShellExt\WslPathExtension.cs"
```
---will become--->
```
"/mnt/d/ProjectsGitHub/WslPathShellExt/WslPathExtension.cs"
```

## Installation

Clone the repo and build the DLL.

Afterwards, install the shell extension using the [ServerRegistrationManager](https://github.com/dwmkerr/sharpshell/releases) utility (look for _ServerRegistrationManager.zip_
 in the release assets!)

```
ServerRegistrationManager.exe install WslPathShellExt.dll -os64
```