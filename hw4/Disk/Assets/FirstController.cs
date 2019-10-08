using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyGame;
using UnityEngine.SceneManagement;

public class FirstController : MonoBehaviour, ISceneController, IUserAction {

    public ActionManager MyActionManager { get; set; }
    public DiskFactory factory { get; set; }
    public RecordController scoreRecorder;
    public UserGUI user;    
    
    void Awake() {
        Director diretor = Director.getInstance();
        diretor.sceneCtrl = this;                              
    }

    // Use this for initialization
    void Start() {
        Begin();
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    public void Begin() {
        MyActionManager = gameObject.AddComponent<ActionManager>() as ActionManager;
        scoreRecorder = gameObject.AddComponent<RecordController>();
        user = gameObject.AddComponent<UserGUI>();
        user.Begin();
    }

    public void Hit(DiskController diskCtrl) {   
        // 0=playing 1=lose 2=win 3=cooling     
        if (user.game == 0) {            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit)) {
                hit.collider.gameObject.SetActive(false);
                Debug.Log("Hit");
                factory.freeDisk(hit.collider.gameObject);
                hit.collider.gameObject.GetComponent<DiskController>().hit = true;
                scoreRecorder.add(hit.collider.gameObject.GetComponent<DiskController>());
            }

        }
    }

    public void PlayDisk() {
        MyActionManager.playDisk(user.round);
    }

    public void Restart() {
        SceneManager.LoadScene("scene");
    }

    public int Check() {
        return 0;
    }
}
