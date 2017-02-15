using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class Buttons : MonoBehaviour {
    private Camera cam;
    private ThirdPersonCamera camScript;

    void Start() {
        cam = Camera.main;
        camScript = cam.GetComponent<ThirdPersonCamera>();
    }

    public void GoToLevel(string levelName) {
        EditorSceneManager.LoadScene(levelName);
    }

    public void RegainFocus() {
        camScript.setPauseMenuActive(false);
    }

    
    

}
