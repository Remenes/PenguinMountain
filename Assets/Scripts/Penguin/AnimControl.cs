using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimControl : MonoBehaviour {

    public float hori = 0;
    public float vert = 0;

    public bool canJump = true;
    public bool justJumped = false;
    public bool inAir = true;
    
    private Rigidbody penguinRB;
    
    private Vector3 savedMoveVector;

    private Animator penguinAnim;

    // Use this for initialization
    void Start() {
        penguinRB = GetComponent<Rigidbody>();

        savedMoveVector = Vector3.zero;
        penguinAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        animPenguin();

    }

    private void animPenguin() {
        
        //Vector3 direction = (camRotation * new Vector3(hori, 0, vert));
        //direction.y = 0;

        ////if (justJumped) {
        ////    justJumped = false;
        ////    penguinRB.velocity = direction * speed;
        ////}

        //Vector3 newVector = (direction * speed * Time.deltaTime);

        //jumpPenguin();

        playAnim();

        if (justJumped) {
            transform.Rotate(new Vector3(180, 0, 0) * Time.deltaTime);
        } else {
            
            if (inAir) {
                transform.Rotate(new Vector3(randRotationInt(), randRotationInt(), randRotationInt()) * Time.deltaTime);
            }
        }

        //    //savedMoveVector = newVector;
        //    //penguinRB.MovePosition(transform.position + newVector);
            
        //}
        //else {
        //    transform.Rotate(new Vector3(180, 0, 0) * Time.deltaTime);
        //    penguinRB.MovePosition(transform.position + savedMoveVector);
        //}


        //penguinRB.AddForce((Physics.gravity));

    }

    private int randRotationInt() {
        return Random.Range(50, 150);
    }

    //public void jumpPenguin() {
    //    if (Input.GetKeyDown(KeyCode.Space)) {
    //        if (canJump) {
    //            //canJump = false;
    //            penguinRB.velocity = new Vector3(0, jumpSpeed, 0);

    //            justJumped = true;
    //            StartCoroutine(PauseMovementAxis(1f));

    //        }
    //    }
    //}

    //private void rotatePenguin(Quaternion camRotation) {
    //    camRotation.x = 0;
    //    camRotation.z = 0;

    //    if (isMoving()) {
    //        transform.rotation = Quaternion.Slerp(transform.rotation, camRotation, .16f);
    //    }
    //}

    public void rotatePenguin(Vector3 direction) {
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        lookRotation.x = 0;
        lookRotation.z = 0;
        penguinRB.MoveRotation( Quaternion.Slerp(transform.rotation, lookRotation, .16f));
    }

    public void rotatePenguin(Quaternion rotation) {
        //Quaternion lockedRotation = rotation;
        //lockedRotation.x = 0;
        //lockedRotation.z = 0;
        //penguinRB.MoveRotation(Quaternion.Slerp(transform.rotation, lockedRotation, .16f));
        rotation.x = 0;
        rotation.z = 0;
        penguinRB.MoveRotation(Quaternion.Slerp(transform.rotation, rotation, .16f));
    }

    //public IEnumerator PauseMovementAxis(float waitTime, bool horizontal = true, bool vertical = true) {
    //    canMoveHori = !horizontal;
    //    canMoveVert = !vertical;
    //    yield return new WaitForSeconds(waitTime);
    //    canMoveHori = true;
    //    canMoveVert = true;

    //    justJumped = false;
    //}

    private void playAnim() {
        float value = Mathf.Sqrt(hori * hori + vert * vert);
        if (inAir) {
            value = 1;
        }
        if (justJumped) {
            value = 0;
        }
        penguinAnim.SetFloat("MoveSpeed", value);
    }

    //Collision Events

    void OnCollisionStay(Collision collided) {
        inAir = false;
    }

    void OnCollisionExit(Collision collided) {
        inAir = true;
    }
}
