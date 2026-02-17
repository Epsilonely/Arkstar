# Tech Context

## Technologies Used
- **Language**: C# (.NET Framework 4.8)
- **Game Engine**: Unity (via RimWorld)
- **Modding Framework**: RimWorld Modding API
- **IDE**: Visual Studio
- **Platform**: Windows (Steam game mod)
- **Build Tool**: MSBuild

## Setup Requirements

### Development Environment
1. Visual Studio (.NET Framework 4.8 support)
2. RimWorld game installed (Steam)
3. RimWorld DLL reference paths:
   - Assembly-CSharp.dll
   - UnityEngine.dll
   - UnityEngine.CoreModule.dll
   - UnityEngine.IMGUIModule.dll

### Project Configuration
- **RimWorld install path**: `D:\SteamLibrary\steamapps\common\RimWorld\`
- **DLL references**: `RimWorldWin64_Data\Managed\` folder
- **Private = False**: RimWorld DLLs loaded by game, not copied

## Dependencies

### RimWorld API
- **Assembly-CSharp**: Core game logic
  - `Verse` namespace: Base game engine
  - `RimWorld` namespace: Game mechanics
- **HintPath**: `D:\SteamLibrary\steamapps\common\RimWorld\RimWorldWin64_Data\Managed\Assembly-CSharp.dll`

### Unity Engine
- **UnityEngine**: Basic Unity functionality
- **UnityEngine.CoreModule**: Core module (Transform, GameObject, etc.)
- **UnityEngine.IMGUIModule**: IMGUI system

### .NET References
- System, System.Core, System.Xml.Linq
- System.Data.DataSetExtensions, Microsoft.CSharp
- System.Data, System.Net.Http, System.Xml

## Development Environment
- **OS**: Windows 11 Home 10.0.26200
- **Target Framework**: .NET Framework 4.8
- **Assembly Name**: Arkstar
- **Output Type**: Library (DLL)
- **Namespace**: Arkstar

## Constraints

### RimWorld Modding
1. Can only use APIs exposed by RimWorld
2. Locked to RimWorld's Unity version
3. Must maintain save file compatibility
4. Minimize frame rate impact

### Technical
1. .NET 4.8 fixed (RimWorld's framework version)
2. DLL Private = False (don't copy game DLLs)
3. Avoid namespace conflicts with other mods

### Design
1. Minimum ricochet chance 10% (gameplay fun)
2. No duplicate target hits
3. Range limited to 7 tiles (balance)

## Build & Deploy

### Build Process
```bash
MSBuild Arkstar/Arkstar.csproj /p:Configuration=Debug
MSBuild Arkstar/Arkstar.csproj /p:Configuration=Release
```

### Output Paths
- **Debug**: `Arkstar/bin/Debug/Arkstar.dll`
- **Release**: `Arkstar/bin/Release/Arkstar.dll`

### Deployment
1. Copy `Arkstar.dll` to `D:\SteamLibrary\steamapps\common\RimWorld\Mods\Arkstar\Assemblies\`
2. Steam Workshop upload via Steam tools

### Testing
- RimWorld Developer Mode (F12 in-game)
- Log: `%USERPROFILE%\AppData\LocalLow\Ludeon Studios\RimWorld by Ludeon Studios\Player.log`
