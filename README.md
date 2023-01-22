# Visual Studio Report

<h2>Problem description</h2>

Visual Studio hangs on `Open Folder` when the `Test Explorer` window is open.

![alt text](https://github.com/deniskovalchuk/VS-Hangs-On-Open-Folder-Bug-Report/blob/b53219dd8e0d1200ce952e268eb7e198a2306b0a/Images/VisualStudioHangsOnOpenFolder.png)

It occurs when [IVsSccFolderProviderBinder.SccBindingsChanged](https://learn.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.shell.sccintegration.ivssccfolderproviderbinder.sccbindingschanged?view=visualstudiosdk-2022) event is called from [IVsSolutionEvents7.OnAfterOpenFolder](https://learn.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.shell.interop.ivssolutionevents7.onafteropenfolder?view=visualstudiosdk-2022#microsoft-visualstudio-shell-interop-ivssolutionevents7-onafteropenfolder(system-string)) method.

This repository contains a synthetic example for demonstration purposes, but the problem was originally reproduced on a real-world source control plug-in.

<h2>Steps to reproduce</h2>

1. Initial settings.
    - `Tools -> Options... -> Environment`.
        - `On startup, open: Empty environment`.
    - `View -> Test Explorer`.
        - Pin the window so that it appears the next time you start Visual Studio.
2. Download the project.
3. Open the solution in Visual Studio.
4. `Build -> Build Solution`.
5. Close Visual Studio.
6. Install `bin\Debug\VisualStudioReport.vsix`.
7. Open Visual Studio.
8. `Tools -> Options... -> Source Control`.
    - `Current source control plug-in: Test Source Control Provider`.
9. `File -> Open -> Folder...`.
10. Select any folder.
