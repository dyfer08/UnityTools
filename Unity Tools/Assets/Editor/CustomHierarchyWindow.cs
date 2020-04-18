using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomHierarchyWindow : EditorWindow{

    string AppName;
    int TagQuantity;
    Vector2 scrollPos;
    List<HierarchyTagAttribute> MyTags = new List<HierarchyTagAttribute>();

    [MenuItem("Window/Hierarchy Color Manager")]
    public static void ShowWindow(){
        EditorWindow.GetWindow<CustomHierarchyWindow>("Hierarchy Color Manager");
    }

    void Awake(){
        AppName = GetProjectName();
        TagQuantity = EditorPrefs.GetInt(AppName+"TagQuantity");
        while (MyTags.Count < TagQuantity){
            MyTags.Add(new HierarchyTagAttribute("", new Color (0,0,0)));
        }
        OnFocus();
    }
    
    void OnGUI(){
        
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
        GUI.skin.label.margin.left = 10;

        GUILayout.Space(10);

        EditorGUI.DrawRect(new Rect(0, 0, Screen.width, 1), new Color32(125,125,125,255));
        EditorGUI.DrawRect(new Rect(0, 1, Screen.width, 32), new Color32(225,225,225,255));
        EditorGUI.DrawRect(new Rect(0, 32, Screen.width, 1), new Color32(125,125,125,255));

        EditorGUILayout.BeginHorizontal ();
            GUILayout.FlexibleSpace();
            GUILayout.Label ("Hierarchy Color Manager Settings", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal ();

        GUILayout.Space(16);

        EditorGUILayout.BeginHorizontal ();
            GUILayout.Label ("N°", GUI.skin.label, GUILayout.MaxWidth(16));
            GUILayout.Label ("Tag", GUI.skin.label );
            GUILayout.Label ("Color", GUI.skin.label );
        EditorGUILayout.EndHorizontal ();

        GUILayout.Space(6);

        for (int i =0; i < MyTags.Count; i++){
            EditorGUILayout.BeginHorizontal ();
                GUILayout.Label (""+i, GUI.skin.label, GUILayout.MaxWidth(16));
                MyTags[i].Tag = EditorGUILayout.TagField(MyTags[i].Tag);
                MyTags[i].TagOn = EditorGUILayout.ColorField(MyTags[i].TagOn);
            EditorGUILayout.EndHorizontal ();
        }

        GUILayout.Space(10);

        DrawUILine(new Color32(125,125,125,255), 1, -3);

        GUIStyle NewStyle = new GUIStyle();
        NewStyle.normal.background = Texture2D.whiteTexture;
        GUI.backgroundColor =  new Color32(225,225,225,255);

        EditorGUILayout.BeginHorizontal (NewStyle);
            GUILayout.FlexibleSpace();
            GUILayout.Label ("Number of colors : "+ TagQuantity, GUI.skin.label, GUILayout.Height(20));
            GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal ();

        GUI.backgroundColor = Color.white;

        DrawUILine(new Color32(125,125,125,255), 1, -4);

        GUILayout.Space(10);

        GUI.skin.button.margin.left = 28;
        GUI.skin.button.margin.right = 22;
        GUI.skin.button.fixedHeight = 20;

        if (GUILayout.Button("Remove last color", GUI.skin.button)){
            TagQuantity --;
            OnFocus();
        }

        if (GUILayout.Button("Add new color", GUI.skin.button)){
            TagQuantity ++;
            OnFocus();
        }

        EditorGUILayout.EndScrollView();

        if (GUI.changed){
            EditorPrefs.SetInt(AppName+"TagQuantity", TagQuantity);

            while (MyTags.Count < TagQuantity){
                MyTags.Add(new HierarchyTagAttribute("", new Color32 (0,0,0,255)));
            }

            while (MyTags.Count > TagQuantity){
                MyTags.Remove(MyTags[MyTags.Count-1]);
            }

            for (int i =0; i < MyTags.Count; i++){
                string PrefID = AppName + "Tag" +i;
                EditorPrefs.SetString(PrefID, MyTags[i].Tag);
                EditorPrefs.SetFloat(PrefID + "CAR", MyTags[i].TagOn.r);
                EditorPrefs.SetFloat(PrefID + "CAG", MyTags[i].TagOn.g);
                EditorPrefs.SetFloat(PrefID + "CAB", MyTags[i].TagOn.b);
                EditorPrefs.SetFloat(PrefID + "CAA", MyTags[i].TagOn.a);
            }

        	CustomHierarchy.UpdateTags(TagQuantity);
        }
    }

    void DrawUILine(Color color, int thickness = 2, int padding = 10){
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding+thickness));
        r.height = thickness;
        r.y+=padding/2;
        r.x-=2;
        r.width +=6;
        EditorGUI.DrawRect(r, color);
    }

    void OnFocus(){
        while (MyTags.Count < TagQuantity){
            MyTags.Add(new HierarchyTagAttribute("", new Color32 (0,0,0,255)));
        }

        while (MyTags.Count > TagQuantity){
            MyTags.Remove(MyTags[MyTags.Count-1]);
        }

        for (int i =0; i < MyTags.Count; i++){
            string PrefID = AppName + "Tag" +i;
            MyTags[i].Tag = EditorPrefs.GetString(PrefID);
            MyTags[i].TagOn = new Color(EditorPrefs.GetFloat(PrefID + "CAR"), EditorPrefs.GetFloat(PrefID + "CAG"), EditorPrefs.GetFloat(PrefID + "CAB"), EditorPrefs.GetFloat(PrefID + "CAA"));
        }

        CustomHierarchy.UpdateTags(TagQuantity);
    }

    string GetProjectName(){
        string[] s = Application.dataPath.Split('/');
        string projectName = s[s.Length - 2];
        return projectName;
    }
}