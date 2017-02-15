using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionBlock : MonoBehaviour {
    /*
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
    */


    
    protected abstract void performAction(Collider collided);

    //void OnCollisionEnter(Collision collided) {
    //    performAction(collided);
    //}

    void OnTriggerEnter(Collider collided) {
        performAction(collided);
    }
}
