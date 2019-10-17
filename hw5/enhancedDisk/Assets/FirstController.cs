using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyGame;
using UnityEngine.SceneManagement;

public class FirstController : MonoBehaviour, ISceneControl, IUserAction {

    public ActionManagerAdapter myAdapter;
    public DiskFactory factory { get; set; }
    public RecordController scoreRecorder;
    public UserGUI user;
    public static float time = 0;
	public int c = 0;
    
    void Awake() {
        Director diretor = Director.getInstance();
        diretor.sceneCtrl = this;                     
    }

    void Start() {
        Begin();
    }

    void FixedUpdate() {
        time += Time.deltaTime;
        if (time < 1)
            return;
        time = 0;
		//Debug.Log(time);
		
        if (user.round <= 3 && user.game == 0) { // still playing
            PlayDisk();
            user.num++;
			c++;
        }
    }

    public void LoadPrefabs() {}

    public void Begin() {
        LoadPrefabs();
        myAdapter = new ActionManagerAdapter(gameObject);
        scoreRecorder = gameObject.AddComponent<RecordController>();
        user = gameObject.AddComponent<UserGUI>();
        user.Begin();
    }

    public void Hit(DiskController diskCtrl) {        
        if (user.game == 0) {            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit)) {
                hit.collider.gameObject.SetActive(false);
                Debug.Log("Hit");
                hit.collider.gameObject.GetComponent<DiskController>().hit = true;
                scoreRecorder.add();
            }
        }
    }
	
    public void PlayDisk() {
        myAdapter.PlayDisk(user.round);
    }
	
    public void Restart() {
        SceneManager.LoadScene("scene");
    }
	
    public void SwitchMode() {
        myAdapter.SwitchActionMode();
    }
	
    public int Check() {
        return 0;
    }
}
