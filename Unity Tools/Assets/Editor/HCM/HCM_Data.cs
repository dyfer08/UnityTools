using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class HCM_Data : ScriptableObject {
	[System.Serializable]
	public class TagColor {
		public string Tag;
		public Color Color;
	}
	public List<TagColor> TagColors;
}
