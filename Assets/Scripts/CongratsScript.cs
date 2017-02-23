using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CongratsScript : MonoBehaviour {

    public GameObject congratsMenu;

	// Use this for initialization
	void Start () {
		
	}
	
	//// Update is called once per frame
	//void Update () {
		
	//}

    public void ShowCongratsMenu(bool show) {
        if (show) {
            congratsMenu.SetActive(true);
            congratsMenu.GetComponent<Animator>().Play("CongratsText");
        } else {
            congratsMenu.SetActive(false);
        }

    }

    void OnTriggerEnter(Collider collided) {
        if (collided.tag == "Player") {
            ShowCongratsMenu(true);
        }
    }

}
