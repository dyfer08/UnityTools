using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Screenshoter))]
public class ScreenshoterEditor : Editor {
    
    public override void OnInspectorGUI(){
        DrawDefaultInspector();

        Screenshoter ScreenshoterScript = (Screenshoter)target;
        GUILayout.Space(20);
        if(GUILayout.Button("Take Screenshot")){
            ScreenshoterScript.TakeEditorScreenshot();
        }
        if(GUILayout.Button("Open Folder")){
            ScreenshoterScript.OpenFolder();
        }
        GUILayout.Space(10);
    }
}