using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

public class SaveManager : MonoBehaviour {

	public static SaveManager Instance;
    static string FilePath;
    static int Slot;
    static Dictionary<string, string> GameSave = null;
    static Dictionary<string, string> EmptyGameSave = new Dictionary<string, string>(){{"SaveTime", ""}};

    void Awake(){
    	if(Instance == null){
    		Instance = this;
            DontDestroyOnLoad(gameObject);
            FilePath = Application.persistentDataPath+"/GameSave";
    	}else{
    		Destroy(gameObject);
    	}
    }

	public static void LoadOrCreateGameSave(int NewSlot){
		
		Slot = NewSlot;
		
		if (System.IO.File.Exists(FilePath+Slot)){
			byte[] GameFile = File.ReadAllBytes(FilePath+Slot);

			if(System.IO.File.Exists(Application.persistentDataPath+"/SHA256"+Slot+".txt")){
				StringBuilder SB = new StringBuilder();
				using (SHA256Managed sha256 = new SHA256Managed()){
					byte[] BytesSalt = System.Text.Encoding.UTF8.GetBytes(SystemInfo.deviceUniqueIdentifier);
					byte[] Hash = sha256.ComputeHash(GameFile.Concat(BytesSalt).ToArray());
					foreach (byte B in Hash){
						SB.Append(B.ToString("X2"));
					}
				}
				if(SB.ToString() != File.ReadAllText(Application.persistentDataPath+"/SHA256"+Slot+".txt")){
					Debug.LogWarning("Save file has been modified. Do something about it... or not.");
				}
			}else{
				Debug.LogWarning("SHA file is missing. Do something about it... or not.");
			}

			GameSave = FromBytes<Dictionary<string, string>>(GameFile);
        }else{
        	CreateGameSave();
        }
	}

    public static void EraseGameSave(){
    	
    	if(GameSave == null) return;

        CreateGameSave();

    }

    static void CreateGameSave(){
    	
    	byte[] GameFile = ToBytes(EmptyGameSave);
        File.WriteAllBytes(FilePath+Slot, GameFile);
        GameSave = new Dictionary<string, string>(EmptyGameSave);
        
        SaveGame();

    }

    public static string GetData(string DataKey){
    	
    	if(GameSave == null) return "No game save";

    	if (!GameSave.ContainsKey(DataKey)){
            return "No data";
        }else{
        	return GameSave[DataKey];
        }

    }

    public static void SetData(string DataKey, string DataValue){
    	
    	if(GameSave == null) return;

    	if (!GameSave.ContainsKey(DataKey)){
            GameSave.Add(DataKey, DataValue);
        }else{
        	GameSave[DataKey] = DataValue;
        }

    }

    public static void SaveGame(int NewSlot){

    	if(GameSave == null) return;

    	Slot = NewSlot;
    	SaveGame();

    }

	public static void SaveGame(){
		
		if(GameSave == null) return;
		
		GameSave["SaveTime"] = System.DateTime.Now.ToString("yyMMddHHmmss");
		byte[] GameFile = ToBytes(GameSave);
		File.WriteAllBytes(FilePath+Slot, GameFile);

		StringBuilder SB = new StringBuilder();
		using (SHA256Managed sha256 = new SHA256Managed()){
			byte[] BytesSalt = System.Text.Encoding.UTF8.GetBytes(SystemInfo.deviceUniqueIdentifier);
			byte[] Hash = sha256.ComputeHash(GameFile.Concat(BytesSalt).ToArray());
			foreach (byte B in Hash){
				SB.Append(B.ToString("X2"));
			}
		}

		File.WriteAllText(Application.persistentDataPath+"/SHA256"+Slot+".txt", SB.ToString());
	}

	static byte[] ToBytes(Dictionary<string, string> Data) {

		BinaryFormatter BF = new BinaryFormatter();
		MemoryStream MS = new MemoryStream();
		BF.Serialize(MS, Data); 
		MS.Flush();

		return MS.ToArray();
	}

	static T FromBytes<T>(byte[] Data){
        
        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter BF = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        System.IO.MemoryStream MS = new System.IO.MemoryStream(Data);
        
        return (T)BF.Deserialize(MS);
    }


    public static List<int> GetListOfExistingGameSaves(){

    	List<int> GameSaves = new List<int>();

    	foreach(string File in System.IO.Directory.GetFiles(Application.persistentDataPath)){

    		if(File.Contains("GameSave")){
    			int SlotNumber = int.Parse(File.Replace(FilePath,""));
    			GameSaves.Add(SlotNumber);
    		}
    	}

    	return GameSaves;
    }

	public static void DebugGameSaves(){
		
		if(GameSave == null) return;
		
		string DebugMessage = "<b>••• CURRENT GAME SAVE DATA : •••</b>";
		foreach (KeyValuePair<string, string> Data in GameSave){
			DebugMessage += string.Format("\n<b>{0}</b> : {1}", Data.Key, Data.Value);
        }

        DebugMessage += "\n<b>••• END OF GAME SAVE DATA •••</b>";
        Debug.Log(DebugMessage);
	}

}