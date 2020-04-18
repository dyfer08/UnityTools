using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[InitializeOnLoad]
public class CustomHierarchy {

	static List<HierarchyTagAttribute> MyTags = new List<HierarchyTagAttribute>();
	static string AppName;

	static CustomHierarchy (){
		EditorApplication.hierarchyWindowItemOnGUI += hierarchyWindowOnGUI;
		AppName = GetProjectName();
		UpdateTags(EditorPrefs.GetInt(AppName + "TagQuantity"));
	}

	static void hierarchyWindowOnGUI (int InstanceID, Rect SelectionRect){

		Rect Rectangle = new Rect (SelectionRect);
		Rectangle.x = 0;
		Rectangle.width = 4;

		Object Obj = EditorUtility.InstanceIDToObject(InstanceID);
		GameObject GO = (GameObject)Obj as GameObject;

		if (GO != null){

			if (GO.activeInHierarchy){
				GUI.backgroundColor = new Color(.7f,.7f,.7f,1);
			}else{
				GUI.backgroundColor = new Color(.7f,.7f,.7f,.5f);
			}

			for (int i = 0; i < MyTags.Count; i++){
				Color GOColor = MyTags[i].TagOn;
				if (GO.tag == MyTags[i].Tag){
					if (GO.activeInHierarchy){
						GUI.backgroundColor = GOColor;
					}else{
						GUI.backgroundColor = new Color(GOColor.r, GOColor.g, GOColor.b, .5f);
					}
				}
			}
		}

		EditorGUI.DrawRect(Rectangle, GUI.backgroundColor);
		GUI.backgroundColor = new Color(1,1,1,1);
	}

	public static void UpdateTags(int Quantity){

		while (MyTags.Count < Quantity){
            MyTags.Add(new HierarchyTagAttribute("", new Color (0,0,0)));
        }

        while (MyTags.Count > Quantity){
            MyTags.Remove(MyTags[MyTags.Count-1]);
        }

		for (int i =0; i < Quantity; i++){
			string PrefID = AppName + "Tag" +i;
            MyTags[i].Tag = EditorPrefs.GetString(PrefID);
            MyTags[i].TagOn = new Color(EditorPrefs.GetFloat(PrefID + "CAR"), EditorPrefs.GetFloat(PrefID + "CAG"), EditorPrefs.GetFloat(PrefID + "CAB"), EditorPrefs.GetFloat(PrefID + "CAA"));
        }
	}

	static string GetProjectName(){
        string[] s = Application.dataPath.Split('/');
        string projectName = s[s.Length - 2];
        return projectName;
    }
}