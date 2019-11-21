using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame;

public class DiskFactory : MonoBehaviour {
    private static DiskFactory _instance;
    public FirstController sceneControler { get; set; }
    GameObject diskPrefab;
    public DiskController diskData;
    public List<GameObject> used;
    public List<GameObject> free;
    // Use this for initialization

    public static DiskFactory getInstance() {
        return _instance;
    }

    private void Awake() {
        if (_instance == null) {
            _instance = Singleton<DiskFactory>.Instance;
            _instance.used = new List<GameObject>();
            _instance.free = new List<GameObject>();
            diskPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/disk"), new Vector3(40, 0, 0), Quaternion.identity);
        }
    }


    public void Start() {
        sceneControler = (FirstController)Director.getInstance().sceneCtrl;
        sceneControler.factory = _instance;      
    }

    public GameObject getDisk(int round) { // 0=playing 1=lose 2=win 3=cooling
        if (sceneControler.scoreRecorder.Score >= round * 5) {
            if (sceneControler.user.round < 3) {
                sceneControler.user.round++;
				sceneControler.user.score = 0;
				sceneControler.user.flag = true;
                sceneControler.user.num = 0;
                sceneControler.scoreRecorder.Score = 0;
            }
            else {
                sceneControler.user.game = 2; // 赢了
                return null;
            }
        }
        else {
            if (sceneControler.user.num >= 10) {
                sceneControler.user.game = 1; // 输了
                return null;
            }            
        }

        GameObject newDisk;
        RoundController diskOfCurrentRound = new RoundController(sceneControler.user.round);        
        if (free.Count == 0) {// if no free disk, then create a new disk
            newDisk = GameObject.Instantiate(diskPrefab) as GameObject;
            newDisk.AddComponent<ClickGUI>();
            diskData = newDisk.AddComponent<DiskController>();
        }
        else {// else let the first free disk be the newDisk
            newDisk = free[0];
            free.Remove(free[0]);
            newDisk.SetActive(true);
        }
        diskData = newDisk.GetComponent<DiskController>();
        diskData.color = diskOfCurrentRound.color;
        //Debug.Log(diskData);

        newDisk.transform.localScale = diskOfCurrentRound.scale * diskPrefab.transform.localScale;
        newDisk.GetComponent<Renderer>().material.color = diskData.color;
        
        used.Add(newDisk);
        return newDisk;
    }

    public void freeDisk(GameObject disk1) {
        used.Remove(disk1);
        disk1.SetActive(false);
		disk1.GetComponent<DiskController>().hit = false;
        free.Add(disk1);
        return;
    }

    public void Restart() {
        used.Clear();
        free.Clear();
    }
}
