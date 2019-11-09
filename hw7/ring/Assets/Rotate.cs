using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {


    // Use this for initialization
	void Start () {
			transform.localScale = new Vector3(5, 5, 5);
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(Vector3.down * 30 * Time.deltaTime);
	}
}
