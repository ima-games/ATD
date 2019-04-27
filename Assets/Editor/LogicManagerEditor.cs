using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LogicManager))]
public class LogicManagerEditor : Editor {



	void OnEnable() {

	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
	}

}
