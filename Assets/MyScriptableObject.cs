using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Odin",menuName =("Odin"))]
public class MyScriptableObject : SerializedScriptableObject
{
	// This Dictionary will be serialized by Odin.
	public Dictionary<int, string> IntStringMap;
}
