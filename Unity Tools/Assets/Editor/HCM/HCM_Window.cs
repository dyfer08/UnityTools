using UnityEditor;
using UnityEngine;

public class HCM_Window : EditorWindow{

    Vector2 scrollPos;
    HCM_Data HCMSettings = null;

    [MenuItem("Window/Hierarchy Color Manager")]
    public static void ShowWindow(){
        EditorWindow.GetWindow<HCM_Window>("Hierarchy Color Manager");
    }

    void Awake(){
        HCMSettings = (HCM_Data)AssetDatabase.LoadAssetAtPath("Assets/Editor/HCM/HCM_Settings.asset", typeof(HCM_Data));
    }
    
    void OnGUI(){

        Undo.RecordObject(HCMSettings, "Changed Settings");
        
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
        GUI.skin.label.margin.left = 10;

        GUIStyle MyButton = new GUIStyle (GUI.skin.button);
        MyButton.fixedWidth = 18;
        MyButton.fixedHeight = 18;
        MyButton.margin.left = 0;
        MyButton.margin.right = 2;
        MyButton.padding.top = 3;
        MyButton.padding.left = 7;

        GUILayout.Space(10); 

        EditorGUI.DrawRect(new Rect(0, 0, Screen.width, 1), new Color32(0,0,0,100));
        EditorGUI.DrawRect(new Rect(0, 1, Screen.width, 32), new Color32(0,0,0,25));
        EditorGUI.DrawRect(new Rect(0, 32, Screen.width, 1), new Color32(0,0,0,100));

        EditorGUILayout.BeginHorizontal ();
            GUILayout.FlexibleSpace();
            GUILayout.Label ("Hierarchy Color Manager Settings", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(16);

        EditorGUILayout.BeginHorizontal ();
            GUILayout.Label ("N°", GUI.skin.label, GUILayout.MaxWidth(16));
            GUILayout.Label ("Tag", GUI.skin.label );
            GUILayout.Label ("Color", GUI.skin.label );
        EditorGUILayout.EndHorizontal ();

        GUILayout.Space(6);

        for (int i =0; i < HCMSettings.TagColors.Count; i++){
            EditorGUILayout.BeginHorizontal ();
                GUILayout.Label (""+i, GUI.skin.label, GUILayout.MaxWidth(16));
                HCMSettings.TagColors[i].Tag = EditorGUILayout.TagField(HCMSettings.TagColors[i].Tag);
                HCMSettings.TagColors[i].Color = EditorGUILayout.ColorField(HCMSettings.TagColors[i].Color);
                if (GUILayout.Button("-", MyButton, GUILayout.MaxWidth(22))){
                    HCMSettings.TagColors.Remove(HCMSettings.TagColors[i]);
                }
            EditorGUILayout.EndHorizontal ();
        }

        GUILayout.Space(10);

        DrawUILine(new Color32(0,0,0,100), 1, -3);

        GUIStyle NewStyle = new GUIStyle();
        NewStyle.normal.background = Texture2D.whiteTexture;
        GUI.backgroundColor =  new Color32(0,0,0,25);

        EditorGUILayout.BeginHorizontal (NewStyle);
            GUILayout.FlexibleSpace();
            GUILayout.Label ("Number of colors : "+ HCMSettings.TagColors.Count, GUI.skin.label, GUILayout.Height(20));
            GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal ();

        GUI.backgroundColor = Color.white;

        DrawUILine(new Color32(0,0,0,100), 1, -4);

        GUILayout.Space(10);

        GUI.skin.button.fixedWidth = default;
        GUI.skin.button.margin.left = 28;
        GUI.skin.button.margin.right = 22;
        GUI.skin.button.fixedHeight = 20;

        if (GUILayout.Button("Add new color", GUI.skin.button)){
            HCMSettings.TagColors.Add(new HCM_Data.TagColor());
        }

        EditorGUILayout.EndScrollView();

        if (GUI.changed){
            EditorUtility.SetDirty(HCMSettings);
            AssetDatabase.SaveAssets();
        }

    }

    void DrawUILine(Color color, int thickness = 2, int padding = 10){
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding+thickness));
        r.height = thickness;
        r.y += padding/2;
        r.x -= 2;
        r.width += 6;
        EditorGUI.DrawRect(r, color);
    }
}