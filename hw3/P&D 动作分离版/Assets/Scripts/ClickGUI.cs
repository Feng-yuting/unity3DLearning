using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Mygame;

public class ClickGUI : MonoBehaviour {
	IUserAction action;
	MyCharacterController characterController;

	public void setController(MyCharacterController characterCtrl) {
		characterController = characterCtrl;
	}

	void Start() {
		action = SSDirector.getInstance ().currentSceneController as IUserAction;
	}

	void OnMouseDown() {
		if (gameObject.name != "boat") {
			action.characterIsClicked (characterController);
		}
	}
}
