using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class CustomShaderGUI : ShaderGUI {
    
    override public void OnGUI(MaterialEditor MaterialEditor, MaterialProperty[] Properties){

        foreach (MaterialProperty Property in Properties){

            string PropertyName = Property.displayName;
            string PropertyTag = "";
            int EndTagPos;

            if(PropertyName.Contains("[Space(")){
                EndTagPos = PropertyName.IndexOf(")]") + 2;
                PropertyTag = PropertyName.Substring(0, EndTagPos);
                PropertyTag = PropertyTag.Replace("[Space(", "");
                PropertyTag = PropertyTag.Replace(")]", "");

                GUILayout.Space(int.Parse(PropertyTag));
                PropertyName = PropertyName.Replace(PropertyName.Substring(0, EndTagPos), "");
            }

            if(PropertyName.Contains("[Header(")){
                EndTagPos = PropertyName.IndexOf(")]") + 2;
                PropertyTag = PropertyName.Substring(0, EndTagPos);
                PropertyTag = PropertyTag.Replace("[Header(", "");
                PropertyTag = PropertyTag.Replace(")]", "");

                GUILayout.Space(10);
                EditorStyles.boldLabel.fontSize = 13;
                GUILayout.Label(PropertyTag, EditorStyles.boldLabel);
                EditorStyles.boldLabel.fontSize = default;
                GUILayout.Space(10);
                PropertyName = PropertyName.Replace(PropertyName.Substring(0, EndTagPos), "");
            }

            if(PropertyName.Contains("[Line]")){
                DrawUILine( new Color (.9f,.9f,.9f));
                continue;
            }

            switch(Property.type){
                case MaterialProperty.PropType.Texture:
                    MaterialEditor.TexturePropertyMiniThumbnail(EditorGUILayout.GetControlRect() , Property, "   " + PropertyName, "Drag & drop texture");
                break;

                default:
                    MaterialEditor.ShaderProperty(Property, PropertyName);
                break;
            
            }
            GUILayout.Space(6);
        }
    }

    void DrawUILine(Color Color, int Thickness = 2, int Padding = 14){
        Rect R = EditorGUILayout.GetControlRect(GUILayout.Height(Padding + Thickness));
        R.height = Thickness;
        R.y += Padding/2;
        R.x -= 20;
        R.width += 26;
        EditorGUI.DrawRect(R, Color);
    }
}