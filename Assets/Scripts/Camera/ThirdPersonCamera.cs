using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

    private const float MAX_Y_ANGLE_IDLE = 50;
    private const float MIN_Y_ANGLE_IDLE = -20;
    private const float MAX_Y_ANGLE_MOVING = 15;
    private const float MIN_Y_ANGLE_MOVING = -10;

    private const float MAX_DISTANCE = 12f;
    private const float MIN_DISTANCE = 3f;
    private float currentDistance = 7f;

    public GameObject player;
    private PlayerScript playerScript;

    private float currentX = 20;
    private float currentY = 20;
    public float sensitivityX = 4f;
    public float sensitivityY = 4f;
    public float sensitivityScroll = 3f;

    private Camera cam;
    private Quaternion camRotation;

    public float camMoveTime = 10;
    private float moveTimer = 0;

    private Vector3 updatedPosition;

    private RaycastHit rayOut;

    private CursorLockMode cursorLockState;

    public GameObject pauseScreen;

	// Use this for initialization
	void Start () {
        cam = Camera.main;

        playerScript = player.GetComponent<PlayerScript>();

        cursorLockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        controlCursorLock();

        rotateCamera();
        
        moveCamera();
	}

    private void rotateCamera() {
        
    }

    private void moveCamera() {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        //float mouseWheel = Input.GetAxis("Mouse ScrollWheel");


        //print(mouseX + " | " + mouseY);

        if (mouseY != 0 || mouseX != 0) {
            //updatedposition = cam.transform.position;
            moveTimer = 0;
        }

        if (cursorLocked()) { // Can only move camera when cursor is locked to center... Cam can still slerp
            currentX += mouseX * sensitivityX;
            currentY = Mathf.Clamp(currentY + sensitivityY * mouseY, MIN_Y_ANGLE_IDLE, MAX_Y_ANGLE_IDLE);
            handleZoom();
        }

        currentY = Mathf.Clamp(currentY, MIN_Y_ANGLE_IDLE, MAX_Y_ANGLE_IDLE);
        
        camRotation = Quaternion.Euler(currentY, currentX, 0);

        float distanceAway = currentDistance;
        
        Vector3 direction = camRotation * new Vector3(0, 0, -1);

        if (Physics.Raycast(player.transform.position, direction, out rayOut, currentDistance ) ) {
            if (rayOut.collider.gameObject.tag == "Wall" || rayOut.collider.gameObject.tag == "Floor") {
                //distanceAway = Mathf.Clamp(rayOut.distance - .5f, MIN_DISTANCE, MAX_DISTANCE);
                distanceAway = rayOut.distance - .5f;
            }
        }

        Vector3 newCamPos = player.transform.position + (camRotation * new Vector3(0, 0, -distanceAway));
        

        //if (moveTimer < camMoveTime) {
        //    moveTimer += Time.deltaTime;
        //}
        //else {
        //    moveTimer = 0;
        //}

        //cam.transform.position = Vector3.Slerp(updatedPosition, newCamPos, moveTimer / camMoveTime);
        cam.transform.position = Vector3.Slerp(cam.transform.position, newCamPos, .06f);
        //cam.transform.position = newCamPos;
        

        cam.transform.LookAt(player.transform);
    }

    private void handleZoom() {
        float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
        float zoomAmount = 0;
        if (Input.GetKeyDown(KeyCode.P)) {
            zoomAmount = .5f;
        } else if (Input.GetKeyDown(KeyCode.O)) {
            zoomAmount = -.5f;
        }
        currentDistance = Mathf.Clamp(currentDistance - (mouseWheel * sensitivityScroll) - zoomAmount, MIN_DISTANCE, MAX_DISTANCE);
    }

    private void controlCursorLock() {
        if (Input.GetKeyDown(KeyCode.Escape))
            setPauseMenuActive(true);

        Cursor.lockState = cursorLockState;
    }
    

    private bool cursorLocked() {
        return cursorLockState == CursorLockMode.Locked;
    }

    public void setCursorLockState(bool lockMouse) {
        cursorLockState = lockMouse ? CursorLockMode.Locked : CursorLockMode.None;
    }

    public void setPauseMenuActive(bool active) {
        bool oppositeActive = !active;

        setCursorLockState(oppositeActive);
        pauseScreen.SetActive(active);
        //playerScript.setCanMove(oppositeActive, oppositeActive);
        playerScript.gamePaused = active;
    }
}
