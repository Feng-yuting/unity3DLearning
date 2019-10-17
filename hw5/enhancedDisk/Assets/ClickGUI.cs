using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame;

public class ClickGUI : MonoBehaviour {
    public DiskController diskCtrl;
    public IUserAction action;

	void Start () {
        action = (IUserAction)Director.getInstance().sceneCtrl;
	}
	
    private void OnMouseDown() {
        action.Hit(diskCtrl);
    }
}
