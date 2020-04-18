using UnityEngine;

public class HierarchyTagAttribute : PropertyAttribute{
	 public string Tag; 
	 public Color TagOn; 
	
	public HierarchyTagAttribute(string TagName, Color TagColor){
		this.Tag = TagName;
		this.TagOn = TagColor;
	}
}

