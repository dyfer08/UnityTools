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
    static Dictionary<string, string> GameSave;
    static Dictionary<string, string> EmptyGameSave = new Dictionary<string, string>(){{"SaveTime", ""}};

    void Awake(){
    	if(Instance == null){
    		Instance = this;
            DontDestroyOnLoad(gameObject);
    	}else{
    		Destroy(gameObject);
    	}
    }

	public static void LoadGameSave(int Slot){
		FilePath = Application.persistentDataPath+"/GameSave"+Slot;
		if (System.IO.File.Exists(FilePath)){
			byte[] GameFile = File.ReadAllBytes(FilePath);

			StringBuilder SB = new StringBuilder();
			using (SHA256Managed sha256 = new SHA256Managed()){
				byte[] BytesSalt = System.Text.Encoding.UTF8.GetBytes(SystemInfo.deviceUniqueIdentifier);
				byte[] Hash = sha256.ComputeHash(GameFile.Concat(BytesSalt).ToArray());
				foreach (byte B in Hash){
					SB.Append(B.ToString("X2"));
				}
			}
			if(SB.ToString() != File.ReadAllText(Application.persistentDataPath+"/SHA256.txt")){
				Debug.LogWarning("Save file has been modified. Do something about it... or not.");
			}

			GameSave = FromBytes<Dictionary<string, string>>(GameFile);
        }else{
        	CreateSaveFile();
        }
	}

    public static void EraseSaveFile(){
        CreateSaveFile();
    }

    static void CreateSaveFile(){
    	byte[] GameFile = ToBytes(EmptyGameSave);
        File.WriteAllBytes(FilePath, GameFile);
        GameSave = new Dictionary<string, string>(EmptyGameSave);
        SaveGame();
    }

    public static string GetData(string DataKey){
    	if (!GameSave.ContainsKey(DataKey)){
            return "No data";
        }else{
        	return GameSave[DataKey];
        }
    }

    public static void UpdateData(string DataKey, string DataValue){
    	if (!GameSave.ContainsKey(DataKey)){
            GameSave.Add(DataKey, DataValue);
        }else{
        	GameSave[DataKey] = DataValue;
        }
    }

	public static void SaveGame(){
		GameSave["SaveTime"] = System.DateTime.Now.ToString("yyMMddHHmmss");
		byte[] GameFile = ToBytes(GameSave);
		File.WriteAllBytes(FilePath, GameFile);

		StringBuilder SB = new StringBuilder();
		using (SHA256Managed sha256 = new SHA256Managed()){
			byte[] BytesSalt = System.Text.Encoding.UTF8.GetBytes(SystemInfo.deviceUniqueIdentifier);
			byte[] Hash = sha256.ComputeHash(GameFile.Concat(BytesSalt).ToArray());
			foreach (byte B in Hash){
				SB.Append(B.ToString("X2"));
			}
		}
		File.WriteAllText(Application.persistentDataPath+"/SHA256.txt", SB.ToString());
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

	public static void DebugGameSave(){
		string DebugMessage = "<b>••• CURRENT GAME SAVE DATA : •••</b>";
		foreach (KeyValuePair<string, string> Data in GameSave){
			DebugMessage += string.Format("\n<b>{0}</b> : {1}", Data.Key, Data.Value);
        }
        DebugMessage += "\n<b>••• END OF GAME SAVE DATA •••</b>";
        Debug.Log(DebugMessage);
	}

}