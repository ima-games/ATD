using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class UIController : MonoBehaviour {

	public UIPanel panel;

	private GTextField cashField;

    void Start() {
		cashField = panel.ui.GetChild("Cash") as GTextField;
	}
	
    void Update() {
		cashField.SetVar("cash", LogicManager.Cash.ToString()).FlushVars();
    }
}
