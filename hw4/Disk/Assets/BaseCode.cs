using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame {
    public class Director : System.Object {
        private static Director _instance;
        public ISceneController sceneCtrl { get; set; }

        public bool running { get; set; } //

        public static Director getInstance() {
            if (_instance == null) return _instance = new Director();
            else return _instance;
        }

        public int getFPS() {
            return Application.targetFrameRate;
        }

        public void setFPS(int fps) {
            Application.targetFrameRate = fps;
        }
    }

    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
        protected static T instance;
        public static T Instance {
            get {
                if (instance == null) {
                    instance = (T)FindObjectOfType(typeof(T));
                    if (instance == null) {
                        Debug.LogError ("An instance of " + typeof(T) + " is needed in the scene, but there is none."); 
                    }
                }
                return instance;
            }
        }
    }

    public interface ISceneController {
        void PlayDisk();
    }

    public interface IUserAction {
        void Begin();
        void Hit(DiskController diskCtrl);
        void Restart();
    }

    public interface ISSActionCallback {
        void ActionDone(SSAction source);
    }

    public class DiskController : MonoBehaviour {
        public float size;
        public Color color;
        public float speed;
        public bool hit = false;
        public SSAction action;
    }

    public class RoundController {
        int round = 0;
        public float scale;
        public Color color;
        public RoundController(int r) {
            round = r;
            scale = 5 - r;
            switch (r) {
                case 1:
                    color = Color.blue;
                    break;
                case 2:
                    color = Color.red;
                    break;
                case 3:
                    color = Color.yellow;
                    break;
            }            
        }
    }

    public class RecordController : MonoBehaviour {
        public int Score = 0;
        public FirstController sceneControler { get; set; }
        // Use this for initialization
        void Start() {
            sceneControler = (FirstController)Director.getInstance().sceneCtrl;
            sceneControler.scoreRecorder = this;
            Score = sceneControler.user.score;
        }

        public void add(DiskController dc) {
            if(dc.color == Color.blue)
                Score += 1;
            else if(dc.color == Color.red)
                Score += 2;
            else 
                Score += 3;
            sceneControler.user.score = Score;
            //Debug.Log(Score);
        }
        public void miss() {
            Score -= 1;
            sceneControler.user.score = Score;
            Debug.Log(Score);
        }
    }
}