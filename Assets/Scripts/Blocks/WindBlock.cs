using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBlock : ActionBlock {

    public float force = 500f;

	// Use this for initialization
	void Start () {
        
	}

    /*
    void Update() {

    }
    */

    protected override void performAction(Collider collided) {
        if (new List<string>() { "Player", "Penguin" }.Contains(collided.gameObject.tag) ) {
        Rigidbody playerRB = collided.GetComponent<Rigidbody>();
            //if (!playerRB)
            //    return;

            //playerRB.velocity = ((transform.rotation * Vector3.up) * force);
            playerRB.AddForce((transform.rotation * Vector3.up) * force);

        }
    }

    void OnTriggerStay(Collider collided) {
        performAction(collided);
    }

}
