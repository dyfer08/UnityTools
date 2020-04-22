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
- Maybe limit the console messages to 2 lines and show the full reslut in a bottom window when selected (like Unity does in the Editor).

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
  When a game save is loaded, the SaveManager checks it's integrity with a SHA256 encryption key. If the game save has been modified or the SHA256 key is missing, it triggers a Debug.LogWarning and you can decide to ignore it or react as you wish.

- Get the list of all game saves in the Application.persistentDataPath folder. It returns a list of int, each int being a slot containing a game save.
  ```csharp
  List<int>  GetListOfExistingGameSaves();
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
