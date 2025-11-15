[Setup]
AppName=JalaNota
AppVersion=1.0
DefaultDirName={pf}\JalaNota
DefaultGroupName=JalaNota
OutputDir=Output
OutputBaseFilename=JalaNota-Setup
Compression=lzma
SolidCompression=yes

[Files]
Source: "bin\Release\JalaNota.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\*.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\JalaNota.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "PrivateSettings.config"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\JalaNota"; Filename: "{app}\JalaNota.exe"
Name: "{commondesktop}\JalaNota"; Filename: "{app}\JalaNota.exe"

[Run]
Filename: "{app}\JalaNota.exe"; Description: "Jalankan JalaNota"; Flags: postinstall nowait skipifsilent
