; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppVersion "2.0"
#define MyAppName "PuzzleMadness" + MyAppVersion
#define MyAppPublisher "Nachiten"
#define MyAppURL "https://www.youtube.com/channel/UCIaYoKyvFB6ubQSYFQ6EISg"
#define MyAppExeName "Puzzle Madness Launcher.exe"
#define MyAppAssocName MyAppName
#define MyAppAssocExt ".myp"
#define MyAppAssocKey StringChange(MyAppAssocName, " ", "") + MyAppAssocExt
#define PathProyecto "D:\Programas Unity\Puzzle-Madness" ; PATH EN PC ESCRITORIO
; #define PathProyecto "C:\Programas Unity\Puzzle-Madness" ; PATH EN NOTEBOOK

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
; VERSION 1.0
; AppId={{8DC430C4-C06C-4E87-87A6-B2A41EB848A2}}

; VERSION 1.1
; AppId={{B436E5F6-6A44-410D-8A19-7DDDD4B1928A}}

; VERSION 2.0
AppId={{E6853E16-7154-4578-A10F-8C61FDD46E77}}
 
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
ChangesAssociations=yes
DisableProgramGroupPage=yes
; Remove the following line to run in administrative install mode (install for all users.)
PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=commandline
OutputDir=D:\Programas Unity\Puzzle-Madness\Builds\Installers
OutputBaseFilename="PuzzleMadness{#MyAppVersion}Installer"
SetupIconFile=D:\Programas Unity\Puzzle-Madness\Assets\Icon.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern


[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked



[Files]
Source: "{#PathProyecto}\Builds\Windows\PuzzleMadness{#MyAppVersion}\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#PathProyecto}\Builds\Windows\PuzzleMadness{#MyAppVersion}\MonoBleedingEdge\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "{#PathProyecto}\Builds\Windows\PuzzleMadness{#MyAppVersion}\Puzzle Madness Launcher_Data\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "{#PathProyecto}\Builds\Windows\PuzzleMadness{#MyAppVersion}\UnityCrashHandler64.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#PathProyecto}\Builds\Windows\PuzzleMadness{#MyAppVersion}\UnityPlayer.dll"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Registry]
Root: HKA; Subkey: "Software\Classes\{#MyAppAssocExt}\OpenWithProgids"; ValueType: string; ValueName: "{#MyAppAssocKey}"; ValueData: ""; Flags: uninsdeletevalue
Root: HKA; Subkey: "Software\Classes\{#MyAppAssocKey}"; ValueType: string; ValueName: ""; ValueData: "{#MyAppAssocName}"; Flags: uninsdeletekey
Root: HKA; Subkey: "Software\Classes\{#MyAppAssocKey}\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\{#MyAppExeName},0"
Root: HKA; Subkey: "Software\Classes\{#MyAppAssocKey}\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#MyAppExeName}"" ""%1"""
Root: HKA; Subkey: "Software\Classes\Applications\{#MyAppExeName}\SupportedTypes"; ValueType: string; ValueName: ".myp"; ValueData: ""

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent
