using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame;

public class PhysicsEmit : SSAction
{
    bool enableEmit = true;
    Vector3 force;
    float startX;
    float targetZ = 50;
    public FirstController sceneControler = (FirstController)Director.getInstance().sceneCtrl;
    
    public override void Start() {
        startX = 6 - Random.value * 12;
        this.Transform.position = new Vector3(startX, 0, 0);
		force = new Vector3(3 * Random.Range(-1, 1), 3 * Random.Range(0.5f, 5), 5 + 3 * sceneControler.user.round);
    }
	
    public static PhysicsEmit GetSSAction() {
        PhysicsEmit action = ScriptableObject.CreateInstance<PhysicsEmit>();
        return action;
    }
    
    public override void Update() {
        if (!this.destroy) {
            if (enableEmit) {
				GameObject.GetComponent<Rigidbody>().useGravity = true;
                GameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                GameObject.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
                enableEmit = false;
            }
        }
        if (this.Transform.position.z >= targetZ || GameObject.GetComponent<DiskController>().hit) {
            GameObject.SetActive(false);
			GameObject.transform.position = new Vector3(startX, 0, 0);
			sceneControler.factory.freeDisk(GameObject);
			this.destroy = true;
			this.Callback.ActionDone(this);
			GameObject.GetComponent<DiskController>().hit = false;
        }
    }
    
}