using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFloorTrigger : MonoBehaviour {

    private PlayerScript playerScript;

	// Use this for initialization
	void Start () {
        playerScript = transform.parent.GetComponent<PlayerScript>();
	}
	
    void OnTriggerStay(Collider collided) {
        playerScript.canJump = true;
    }

    void OnTriggerExit(Collider collided) {
        playerScript.canJump = false;
    }
}
