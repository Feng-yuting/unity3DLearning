﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame;

public class PatrolControl : MonoBehaviour {
    private IAddAction addAction;
    private IGameStatusOp gameStatusOp;

    public int whichPatrol;
    public bool isCatching; //whether found hero

    private float CATCH_RADIUS = 3.0f;

    // Use this for initialization
    void Start () {
        addAction = (FirstControl)Director.getInstance().sceneCtrl as IAddAction;
        gameStatusOp = (FirstControl)Director.getInstance().sceneCtrl as IGameStatusOp;

        whichPatrol = getIndex();
        isCatching = false;
    }

    // Update is called once per frame
    void Update () {
		//Debug.Log("here1");
        if (Vector3.Distance(gameStatusOp.getHeroPosition().position, gameObject.transform.position) <= 10f)
        {   
            if (!isCatching)
            {
                isCatching = true;                
            }
            
            addAction.addDirectMovement(this.gameObject);
        }
        else
        {
            if (isCatching)
            {
                //stop catching 
                gameStatusOp.heroEscapeAndScore();
                isCatching = false;
				addAction.addRandomMovement(this.gameObject, false);     
				        
            }
            else
				addAction.addRandomMovement(this.gameObject, true);        
        }
    }

    public int getIndex()
    {
        //get the index of patrol
        string name = this.gameObject.name;
        return name[name.Length - 1] - '0';
    }

    void OnCollisionStay(Collision e)
    {
        if (e.gameObject.name.Contains("Patrol"))
        {
            isCatching = false;
			gameStatusOp.heroEscapeAndScore();
            addAction.addRandomMovement(this.gameObject, false);
        }

        if (e.gameObject.name.Contains("hero"))
        {
            gameStatusOp.patrolHitHeroAndGameover();
            Debug.Log("Game Over!");
        }
    }
}
