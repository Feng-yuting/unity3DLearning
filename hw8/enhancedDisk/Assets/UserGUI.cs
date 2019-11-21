using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyGame;

public class UserGUI : MonoBehaviour {
    private IUserAction action;     
    GUIStyle LabelStyle1;
    GUIStyle LabelStyle2;
    GUIStyle ButtonStyle;
	GUIStyle ScrollerStyle;
    private int timeLeft = 60;
    public int score = 0;
    public static float time = 0;

    public int round = 1;
    public int CoolTimes = 3; 
    public int game = 0; // 0=playing 1=lose 2=win 3=cooling
    public int num = 0; // number of disks
    public int mode = 0; // 0=normal 1=physics
	
    public float health = 0.0f;
	private float resulthealth;
	public bool flag = false;
	
	GameObject canvas;
	public Canvas bloodC;
	public Slider bloodS;
    
    void Start () {
        action = (IUserAction)Director.getInstance().sceneCtrl;

        LabelStyle1 = new GUIStyle();
        LabelStyle1.fontSize = 20;
        LabelStyle1.alignment = TextAnchor.MiddleCenter;

        LabelStyle2 = new GUIStyle();
        LabelStyle2.fontSize = 30;
        LabelStyle2.alignment = TextAnchor.MiddleCenter;

        ButtonStyle = new GUIStyle("Button");
        ButtonStyle.fontSize = 20;
		
		/*
		canvas = GameObject.Find("bloodC");
		Debug.Log(canvas);
		canvas.SetActive(false);
		bloodC = transform.Find("bloodC").GetComponent<Canvas>();
		bloodS = bloodC.transform.Find("bloodS").GetComponent<Slider>();
		Debug.Log(bloodS.value);
		*/
		
		
    }

    void FixedUpdate() {
        time += Time.deltaTime;
        if (time < 1)
            return;
        time = 0;

        if (game == 3) {
            if (CoolTimes > 1) 
				CoolTimes--;
            else 
				game = 0;
        }
        else if (game == 0) {
            timeLeft--;
        }
    }

    public void Restart() {
        timeLeft = 60;
        round = 1;
        CoolTimes = 3;
        game = 3;
        num = 0;
        score = 0;
        mode = 0;
		
		health = 0.0f;
		flag = false;
    }

    public void Begin() {
        Restart();
    }

    void OnGUI () {
        string str = mode == 0 ? "Normal" : "Physics";
        GUI.Label(new Rect(Screen.width / 2 - 30, 10, 100, 50), "Mode: " + str, LabelStyle1);
        if (GUI.Button(new Rect(20, 20, 100, 50), "Switch", ButtonStyle)) {            
            action.SwitchMode();
            mode = 1 - mode;
        }
        if (game == 0) { // playing
			//canvas.SetActive(true);
			//Debug.Log(canvas);
			
            GUI.Label(new Rect(Screen.width / 2 - 180, Screen.height / 2 - 160, 100, 50), "Round: " + round, LabelStyle1);
            GUI.Label(new Rect(Screen.width / 2 - 30, Screen.height / 2 - 160, 100, 50), "Time: " + timeLeft, LabelStyle1);
            //GUI.Label(new Rect(Screen.width / 2 + 120, Screen.height / 2 - 160, 100, 50), "Score: " + score, LabelStyle1);
			if(flag){
				//Debug.Log("here in lerp");
				resulthealth = (float)score/(float)(round * 5);
				Debug.Log(resulthealth);
				flag = false;
			}
			
			health = Mathf.Lerp(health, resulthealth, 0.05f);
			GUI.color = Color.red;
			GUI.HorizontalScrollbar(new Rect(Screen.width / 2 + 120, Screen.height / 2 - 145, 100, 50), 0.0f, health, 0.0f, 1.0f);
			//bloodS.value = health;
			
            if (timeLeft == 0) game = 1;
        }
        else if (game == 1) { // lose
			//canvas.SetActive(false);
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), "Gameover!", LabelStyle2);
            if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2, 140, 70), "Restart", ButtonStyle))
            {
                game = 0;
                action.Restart();
            }
        }
        else if (game == 2) {// win 
			//canvas.SetActive(false);
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), "You win!", LabelStyle2);
            if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2, 140, 70), "Restart", ButtonStyle)) {
                game = 0;
                action.Restart();
            }
        }
        else if (game == 3) { // cooling
			//canvas.SetActive(false);
            GUI.Label(new Rect(Screen.width / 2 - 30, Screen.height / 2, 100, 50), CoolTimes.ToString(), LabelStyle2);
        }
    }

}
