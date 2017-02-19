using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinFloorTrigger : MonoBehaviour {

    private AnimControl penguinScript;

	// Use this for initialization
	void Start () {
        penguinScript = transform.parent.GetComponent<AnimControl>();
	}
	
    void OnTriggerStay(Collider collided) {
        penguinScript.canJump = true;
    }

    void OnTriggerExit(Collider collided) {
        penguinScript.canJump = false;
    }
}
