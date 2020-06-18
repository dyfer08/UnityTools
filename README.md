# Unity Tools
A collection of super simple and useful tools for Unity.


## HCM (Hierarchy Color Manager)
Assign colors to the gameobjects in your hierarchy window based on their tags.

![HCM in Unity](https://ferdinanddervieux.com/ImageHosting/HCM2.png)

**Download** :
- [HierarchyColorManager.unitypackage.zip](https://github.com/dyfer08/UnityTools/raw/master/Unity%20Tools/Assets/Unity%20Packages/HierarchyColorManager.unitypackage.zip)

**Future improvements** :
- Maybe remove the total number of colors. Seems sadly useless :p

---

## BVAU (Bundle Version Auto Updater) for iOS & Android
Auto increment the bundle version number after a build on iOS and Android. This prevent your app from being rejected by the store submission if the bundle version has not changed.

**Download** :
- [BundleVersionAutoUpdater.unitypackage.zip](https://github.com/dyfer08/UnityTools/raw/master/Unity%20Tools/Assets/Unity%20Packages/BundleVersionAutoUpdater.unitypackage.zip)

**Future improvements** :
- Maybe add support for more platforms if required.

---

## Device Console
Receive and show every Unity console message directly in your build. this is especially useful on mobile and AR projects.

![DeviceConsole in Unity](https://ferdinanddervieux.com/ImageHosting/DeviceConsole.png)*Device Console directly into the game view, ready to be exported in a build.*

**Download** :
- [DeviceConsole.unitypackage.zip](https://github.com/dyfer08/UnityTools/raw/master/Unity%20Tools/Assets/Unity%20Packages/DeviceConsole.unitypackage.zip)

**How to use** :
- Just drag and drop the DeviceConsole prefab in your scene. If you need to adjust the scale, modify the Scale Factor of the Canvas Scaler component.

**Future improvements** :
- Maybe limit the console messages to 2 lines and show the full content in a bottom window when selected (like Unity does in the Editor).

---

## Save Manager
An easy to use encrypted game save manager. Create, load, save game data on any platform. It's using a SHA256 encryption and warns you if the game save has been modified or the SHA key is missing so you can decide what to do. You can create as many game save as you want by passing a slot int.

**Download** :
- [SaveManager.unitypackage.zip](https://github.com/dyfer08/UnityTools/raw/master/Unity%20Tools/Assets/Unity%20Packages/SaveManager.unitypackage.zip)

**How to use** :
- Just drag and drop the SaveManager prefab in your scene. It is a DontDestroyOnLoad singleton object.
- Create or load an existing game save in a specific slot. If the slot is empty, it creates a new game save in this slot.
**You need to create or load a game save to be able to use most of the SaveManager functions( eg : GetData, SetData, SaveGame ).**
  ```csharp
  SaveManager.LoadOrCreateGameSave(int Slot);
  ```
  When loading a game save, the SaveManager checks it's integrity with a SHA256 encryption key. If the game save has been modified or the SHA256 key is missing, it triggers a Debug.LogWarning and you can decide to ignore it or react as you wish.

- Get the list of all game saves in the Application.persistentDataPath folder. It returns a list of int, each int being a slot containing a game save.
  ```csharp
  List<int> GetListOfExistingGameSaves();
  ```
- Set or update game save data. Pass a key and a value. It automatically creates new keys and update existing ones.
  ```csharp
  SaveManager.SetData(string DataKey, string DataValue);
  ```
- Get a save data value. Return the requested value as a string. If the key doesn't exist it returns the value "No data".
  ```csharp
  string SaveManager.GetData(string DataKey);
  ```
- Save the game. Write the game save data to the active slot.
  ```csharp
  SaveManager.SaveGame();
  ```
  To save the current game save in a different slot you can pass an int. From now on this new slot will be the active one.
  ```csharp
  SaveManager.SaveGame(int Slot);
  ```  
- Erase the current game save. Replace the data of the active slot with empty data.
  ```csharp
  SaveManager.EraseGameSave();
  ```
- Show the list of all the data contained by the current game save in the console.
  ```csharp
  SaveManager.DebugGameSave();
  ```
- By default every game save created contains a "SaveTime" key. this key is updated everytime you save the game. It returns the System.DateTime.Now formated as a string : "yyMMddHHmmss".

---

## Screenshoter
Screenshot is a straightforward tool to take screenshots in builds and Editor (both in playmode or not). You can specify a file format(RAW, PNG, JPG), camera, resolution and set a super size upscale for high resolution captures. Tested on OSX and Android. To use in Android you need to enable access to externl files in the Unity Player settings.

![Screenshoter in Unity](https://ferdinanddervieux.com/ImageHosting/Screenshoter2.png)*Screenshoter tool ui.*

**Download** :
- [Screenshoter.unitypackage.zip](https://github.com/dyfer08/UnityTools/raw/master/Unity%20Tools/Assets/Unity%20Packages/Screenshoter.unitypackage.zip)

**How to use** :
- Just drag and drop the Screenshoter prefab in your scene. It is a DontDestroyOnLoad singleton object.

  
**In Editor mode**
- You can use Screenshoter in both play and edit mode.
- Tweak the optional settings.
- Click on "Take Screenshot" to capture an image.
- Click on "Show Folder" to open the folder containing your screenshots.

  
**In build**
- Take a screenshot in build.
  ```csharp
  Screenshother.TakeScreenshot();
  ```
  You can pass an int to set the supersize scale.
  ```csharp
  Screenshother.TakeScreenshot(int SuperSize);
  ```
  You can pass a Vector2 to set a specific resolution.
  ```csharp
  Screenshother.TakeScreenshot(Vector2 Resolution);
  ```
  And you can pass both.
  ```csharp
  Screenshother.TakeScreenshot(int SuperSize, Vector2 Resolution);
  ```

---

## CustomShaderUGUI
I have been using shader graph a lot these days and my materials in inspector get quite complex sometimes with a ton of otions stacking there. So I created a simple script to hel design and organise shader settings. You can add headers, lines and spaces to improve readability. I also changed the default texture layout to the Unity small one.

![CustomShaderUGUI in Unity](https://ferdinanddervieux.com/ImageHosting/CustomShaderUGUI.png)

**Download** :
- [CustomShaderUGUI.unitypackage.zip](https://github.com/dyfer08/UnityTools/raw/master/Unity%20Tools/Assets/Unity%20Packages/CustomShaderUGUI.unitypackage.zip)

**How to use** :<br>
<br>
**Setup**
- This script requires shader graph 7.4+
- In your shader graph, click on the little cog in the top right corner of your Master Node
- Check Override ShaderGUI and in the ShaderGUI field write "CustomShaderGUI"

![CustomShaderUGUI setup](https://ferdinanddervieux.com/ImageHosting/CustomShaderUGUISetup.png)

**Use**
- To add a line, create a Vector1 property and call it "[Line]". The property will be ignored and a white line separator will appear in inspector.
- To add a header, create a Vector1 property and call it "[Header(Any title you want)]" where "Any title you want" is your text. The property will be ignored and a header will appear in inspector.
- To add a space, create a Vector1 property and call it "[Space(999)]" where "999" is your space in pixels. The property will be ignored and a space will appear in inspector.

**No more properties**
- Alternatively for Header and Space tags you can avoid to create a Vector1 property and add them directly in an existing property name. So for example if you have a Texture property called "Main texture" and you want to insert a header above, you can rename it "[Header(Any title you want)]Main texture".
