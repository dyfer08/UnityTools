using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class Message {
	public int TypeID;
	public GameObject MessageObject;
}

public class DeviceConsole : MonoBehaviour{

	[SerializeField]
	GameObject Console = null;
	[SerializeField]
	RectTransform ConsoleContent = null;
	[SerializeField]
	GameObject MessagePrefab = null;
    [SerializeField]
    List<Toggle> ToggleTypeButtons = null;
    List<int> TypesCounter = new List<int>(){0, 0, 0};
	List<bool> ActiveLogTypes = new List<bool>(){true, true, true};
	bool ToggleConsole = false;
	List<Message> Messages = new List<Message>();
	// Console design
	[SerializeField]
    List<Sprite> TypeIcons = null;
	bool ColorToggle = true;
    Color DefaultColor = new Color32(194,194,194,255);
    Color AltColor = new Color32(201,201,201,255);

	void Start (){
		for(int i=0; i<ActiveLogTypes.Count; i++){
			if(PlayerPrefs.HasKey("DC_ShowType"+i)){
				ToggleTypeButtons[i].isOn = PlayerPrefs.GetString("DC_ShowType"+i) == "True";
			}
		}
        Application.logMessageReceived += LogMessage;

        if(UnityEngine.EventSystems.EventSystem.current == null){
        	GameObject NewEventSystem = new GameObject("EventSystem");
        	NewEventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
        	NewEventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        	Destroy(NewEventSystem.GetComponent<UnityEngine.EventSystems.BaseInput>());
        }
    }

	public void ToggleDeviceConsole(){
		ToggleConsole = !ToggleConsole;
		Console.SetActive(ToggleConsole);
	}

	public void ToggleLog(bool Value){
		ActiveLogTypes[0] = Value;
		PlayerPrefs.SetString("DC_ShowType0", Value.ToString());
		RearrangeList();
	}

	public void ToggleWarning(bool Value){
		ActiveLogTypes[1] = Value;
		PlayerPrefs.SetString("DC_ShowType1", Value.ToString());
		RearrangeList();
	}

	public void ToggleError(bool Value){
		ActiveLogTypes[2] = Value;
		PlayerPrefs.SetString("DC_ShowType2", Value.ToString());
		RearrangeList();
	}

	public void Clear(){
		TypesCounter[0] = TypesCounter[1] = TypesCounter[2] = 0;
		ToggleTypeButtons[0].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
		ToggleTypeButtons[1].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
		ToggleTypeButtons[2].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
		"0";
		for(int i=0; i<Messages.Count; i++){
			Destroy(Messages[i].MessageObject);
			Messages.Clear();
		}
	}

	void LogMessage(string Message, string StackTrace, LogType Type){
		
		Message NewMessage = new Message();
		GameObject MessageObject = Instantiate(MessagePrefab, transform);
		MessageObject.transform.SetParent(ConsoleContent);

		int TypeID = 0;

		switch(Type){
			case LogType.Log:
				TypeID = 0;
			break;

			case LogType.Warning:
				TypeID = 1;
			break;

			case LogType.Exception:
			case LogType.Assert:
			case LogType.Error:
				TypeID = 2;
			break;

			default :
				TypeID = 0;
			break;
		}

		NewMessage.TypeID = TypeID;

		MessageObject.transform.GetChild(0).GetComponent<Image>().sprite = TypeIcons[TypeID];
		TypesCounter[TypeID]++;
		if(TypesCounter[TypeID] > 999){
			TypesCounter[TypeID] = 999;
		}
		ToggleTypeButtons[TypeID].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = ""+TypesCounter[TypeID];

		MessageObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Message + "\n" + StackTrace;
		
		NewMessage.MessageObject = MessageObject;
		Messages.Add(NewMessage);
		
		if(ActiveLogTypes[TypeID]){
			ColorToggle = !ColorToggle;
    		if(!ColorToggle){
    			MessageObject.GetComponent<Image>().color = AltColor;
    		}
    		MessageObject.SetActive(true);
		}
    }

    void RearrangeList(){

    	ColorToggle = true;

    	for(int i=0; i<Messages.Count; i++){
    		if(ActiveLogTypes[Messages[i].TypeID]){
    			ColorToggle = !ColorToggle;
    			if(ColorToggle){
    				Messages[i].MessageObject.GetComponent<Image>().color = DefaultColor;
    			}else{
    				Messages[i].MessageObject.GetComponent<Image>().color = AltColor;
    			}
    			Messages[i].MessageObject.SetActive(true);
    		}else{
    			Messages[i].MessageObject.SetActive(false);
    		}
    	}
    }
}