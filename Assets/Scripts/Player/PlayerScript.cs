using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    public bool isPlayer = true;

    public bool gamePaused = false;

    private bool canMoveHori = true;
    private bool canMoveVert = true;
    //public bool canJump = true;
    //private bool justJumped = false;
    //public bool inAir = false;

    private float hori;
    private float vert;

    private Rigidbody playerRB;
    public float speed = 7f;
    public float jumpSpeed = 10.5f;

    private Camera cam;

    private Vector3 savedMoveVector;
    private AnimControl anim;

	// Use this for initialization
	void Start () {
        playerRB = GetComponent<Rigidbody>();
        cam = Camera.main;
        savedMoveVector = Vector3.zero;
        anim = GetComponent<AnimControl>();
	}

	// Update is called once per frame
	void Update () {
        movePlayer();
        
	}

    private void movePlayer() {
        hori = Input.GetAxisRaw("Horizontal");
        vert = Input.GetAxisRaw("Vertical");
        
        Quaternion camRotation = cam.transform.rotation;

        //if (!justJumped) {
            hori = canMoveHori ? hori : 0;
            vert = canMoveVert ? vert : 0;
        //}

        if (gamePaused || !isPlayer) {
            hori = 0;
            vert = 0;
        }

        anim.hori = hori;
        anim.vert = vert;

        Vector3 direction = (camRotation * new Vector3(hori, 0, vert));
        
        //direction.x = direction.x > 0 ? 1 : 0;
        //direction.z = direction.z > 0 ? 1 : 0;
        direction.y = 0;
        direction = direction.normalized;

        //if (justJumped) {
        //    justJumped = false;
        //    playerRB.velocity = direction * speed;
        //}
        Vector3 speedVector = direction * speed;

        //if (isMoving())
        //    speedVector = speedVector * speed / (Mathf.Sqrt(speedVector.x * speedVector.x + speedVector.z * speedVector.z));
        
        Vector3 newVector = (speedVector * Time.deltaTime);

        jumpPlayer();
        
        if (!anim.justJumped) {
            if (!anim.inAir)
                rotatePlayer(camRotation);

            savedMoveVector = newVector;
            playerRB.MovePosition(transform.position + newVector);

            
        }
        else {
            playerRB.MovePosition(transform.position + savedMoveVector);
        }

        
        //playerRB.AddForce((Physics.gravity));
        
    }

    private int randRotationInt() {
        return Random.Range(50, 150);
    }

    public void jumpPlayer() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (anim.canJump) {
                anim.canJump = false;
                playerRB.velocity = new Vector3(0, jumpSpeed, 0);
                
                anim.justJumped = true;
                StartCoroutine(PauseMovementAxis(1f));
                
            }
        }
    }

    private void rotatePlayer(Quaternion camRotation) {

        if (isMoving()) {
            //transform.rotation = Quaternion.Slerp(transform.rotation, camRotation, .16f);
            anim.rotatePenguin(camRotation);
        }
    }
    

    public bool isMoving() {
        return hori != 0 || vert != 0;
    }

    public bool canMove() {
        return canMoveHori && canMoveVert;
    }

    public void setCanMove(bool horiMove, bool vertMove) {
        canMoveHori = horiMove;
        canMoveVert = vertMove;
    }

    public IEnumerator PauseMovementAxis(float waitTime, bool horizontal = true, bool vertical = true) {
        canMoveHori = !horizontal;
        canMoveVert = !vertical;
        yield return new WaitForSeconds(waitTime);
        canMoveHori = true;
        canMoveVert = true;

        anim.justJumped = false;
    }
    
    
}
