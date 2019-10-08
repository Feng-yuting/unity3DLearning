using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame;

public class Emit : SSAction
{
    public FirstController sceneControler = (FirstController)Director.getInstance().sceneCtrl;
    public Vector3 target;     
    public float speed;     
    private float distanceToTarget;    
    float startX;
    float targetX;
    float targetY;

    public override void Start() {
        speed = sceneControler.user.round * 5;
        GameObject.GetComponent<DiskController>().speed = speed;

        startX = 6 - Random.value * 12;

        if (Random.value > 0.5) {
            targetX = 36 - Random.value * 36;
            targetY = 25 - Random.value * 25;
        }
        else {
            targetX = -36 + Random.value * 36;
            targetY = -25 + Random.value * 25;
        }

        this.Transform.position = new Vector3(startX, 0, 0);
        target = new Vector3(targetX, targetY, 30);
        //Debug.Log(target);
        distanceToTarget = Vector3.Distance(this.Transform.position, target);
    }

    public static Emit GetSSAction() {
        Emit action = ScriptableObject.CreateInstance<Emit>();
        return action;
    }

    public override void Update() {
        Vector3 targetPos = target;
        if(!GameObject.activeSelf){
            this.destroy = true;
            return;
        }

        //facing the target
        GameObject.transform.LookAt(targetPos);

        //calculate the starting angel  
        float angle = Mathf.Min(1, Vector3.Distance(GameObject.transform.position, targetPos) / distanceToTarget) * 45;
        GameObject.transform.rotation = GameObject.transform.rotation * Quaternion.Euler(Mathf.Clamp(-angle, -42, 42), 0, 0);
        float currentDist = Vector3.Distance(GameObject.transform.position, target);
        //Debug.Log("****************************");
        //Debug.Log(startX);
        //Debug.Log(target);
        //Debug.Log("****************************");
        GameObject.transform.Translate(Vector3.forward * Mathf.Min(speed * Time.deltaTime, currentDist));
        if (this.Transform.position == target) {
            sceneControler.scoreRecorder.miss();
            Debug.Log("here in miss!!");
            GameObject.SetActive(false);
            GameObject.transform.position = new Vector3(startX, 0, 0);
            sceneControler.factory.freeDisk(GameObject);
            this.destroy = true;
            this.Callback.ActionDone(this);
        }
    }
}