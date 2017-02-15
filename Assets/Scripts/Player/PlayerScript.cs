using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    public bool isPlayer = true;

    public bool gamePaused = false;

    private bool canMoveHori = true;
    private bool canMoveVert = true;
    public bool canJump = true;
    private bool justJumped = false;
    public bool inAir = false;

    private float hori;
    private float vert;

    private Rigidbody playerRB;
    public float speed = 5f;
    public float jumpSpeed = 5f;

    private Camera cam;

    private Vector3 savedMoveVector;
    private Animator playerAnim;

	// Use this for initialization
	void Start () {
        playerRB = GetComponent<Rigidbody>();
        cam = Camera.main;
        savedMoveVector = Vector3.zero;
        playerAnim = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update () {
        movePlayer();
        
	}

    private void movePlayer() {
        hori = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");
        
        Quaternion camRotation = cam.transform.rotation;

        //if (!justJumped) {
            hori = canMoveHori ? hori : 0;
            vert = canMoveVert ? vert : 0;
        //}

        if (gamePaused || !isPlayer) {
            hori = 0;
            vert = 0;
        }

        Vector3 direction = (camRotation * new Vector3(hori, 0, vert));
        direction.y = 0;

        //if (justJumped) {
        //    justJumped = false;
        //    playerRB.velocity = direction * speed;
        //}
        
        Vector3 newVector = (direction * speed * Time.deltaTime);

        jumpPlayer();

        playAnim();

        if (!justJumped) {
            if (!inAir)
                rotatePlayer(camRotation);
            else {
                transform.Rotate(new Vector3(randRotationInt(), randRotationInt(), randRotationInt()) * Time.deltaTime);
            }

            savedMoveVector = newVector;
            playerRB.MovePosition(transform.position + newVector);

            
        }
        else {
            transform.Rotate(new Vector3(180, 0, 0) * Time.deltaTime);
            playerRB.MovePosition(transform.position + savedMoveVector);
        }

        
        //playerRB.AddForce((Physics.gravity));
        
    }

    private int randRotationInt() {
        return Random.Range(50, 150);
    }

    public void jumpPlayer() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (canJump) {
                //canJump = false;
                playerRB.velocity = new Vector3(0, jumpSpeed, 0);
                print(playerRB.velocity);
                justJumped = true;
                StartCoroutine(PauseMovementAxis(1f));
                
            }
        }
    }

    private void rotatePlayer(Quaternion camRotation) {
        camRotation.x = 0;
        camRotation.z = 0;

        if (isMoving()) {
            transform.rotation = Quaternion.Slerp(transform.rotation, camRotation, .16f);
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

        justJumped = false;
    }
    
    private void playAnim() {
        float value = Mathf.Sqrt(hori * hori + vert * vert);
        if (inAir) {
            value = 1;
        }
        if (justJumped) {
            value = 0;
        }
        playerAnim.SetFloat("MoveSpeed", value);
    }

    //Collision Events

    void OnCollisionStay(Collision collided) {
        inAir = false;
    }

    void OnCollisionExit(Collision collided) {
        inAir = true;
    }
}
