using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[InitializeOnLoad]
public class HCM_Hierarchy {

	static HCM_Data M_HCMSettings = null;

	static HCM_Hierarchy (){
		EditorApplication.hierarchyWindowItemOnGUI += hierarchyWindowOnGUI;
	}

	static void hierarchyWindowOnGUI (int InstanceID, Rect SelectionRect){

		if(M_HCMSettings == null){
        	M_HCMSettings = (HCM_Data)AssetDatabase.LoadAssetAtPath("Assets/Editor/HCM/HCM_Settings.asset", typeof(HCM_Data));			
		}

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

			for (int i = 0; i < M_HCMSettings.TagColors.Count; i++){
				Color GOColor = M_HCMSettings.TagColors[i].Color;
				if (GO.tag == M_HCMSettings.TagColors[i].Tag){
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
}