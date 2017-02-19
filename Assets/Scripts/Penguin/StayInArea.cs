using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayInArea : MonoBehaviour {

    public GameObject areaToStayIn;
    private BoxCollider areaCollider;
    private Vector3 areaPosition;
    private Vector3 areaBound;

    private Rigidbody penguinRB;
    private Vector3 newPosition;

    public float moveSpeed;

    public bool goToPosition = false;
    private Vector3 moveOffset;

    private AnimControl anim;

    private Vector3 zeroedYNewPos = Vector3.zero;
    private Vector3 zeroedYPosition;

    // Use this for initialization
    void Start () {
        if (!areaToStayIn) {
            enabled = false;
            return;
        }
        areaCollider = areaToStayIn.GetComponent<BoxCollider>();
        penguinRB = GetComponent<Rigidbody>();
        newPosition = Vector3.zero;
        areaPosition = areaToStayIn.transform.position;
        areaBound = areaCollider.bounds.extents;
        anim = GetComponent<AnimControl>();

        StartCoroutine(resetPosition());
    }
	
	// Update is called once per frame
	void Update () {
        /*If object not in area Collider, generate new position inside and walk towards it
        Don't walk towards if reached close enough, and just wait till outside again*/
        //if (anim.inAir && !goToPosition)
        //    return;

        //zeroedYPosition = transform.position;
        //zeroedYPosition.y = 0;

        //if (!inBoundsXZ(transform.position) && !goToPosition && !anim.inAir) {
        //    goToPosition = true;
        //    anim.hori = 1;

        //    Vector3 offsetPosition = new Vector3(randPlusMinusNum(areaBound.x), 0, randPlusMinusNum(areaBound.z));
        //    newPosition = areaPosition + offsetPosition;
        //    zeroedYNewPos = newPosition;
        //    zeroedYNewPos.y = 0;

        //    //moveOffset = ((zeroedYNewPos - zeroedYPosition).normalized * moveSpeed * Time.deltaTime);



        //}
        //else if (goToPosition) {

        //    if (Vector3.Distance(zeroedYPosition, zeroedYNewPos) > 1f) {
        //        moveOffset = ((zeroedYNewPos - zeroedYPosition).normalized * moveSpeed * Time.deltaTime);
        //        //print(zeroedYNewPos + " Current: " + zeroedYPosition + " | moveOffset " + moveOffset);
        //        penguinRB.MovePosition(transform.position + moveOffset);
        //        anim.rotatePenguin(moveOffset);
        //    }
        //    else {
        //        goToPosition = false;
        //        anim.hori = 0;
        //    }
        //}
    }

    private float randPlusMinusNum(float number) {
        return Random.Range(-number, number);
    }

    private IEnumerator resetPosition() {
        while (true) {
            if (anim.inAir && !goToPosition) {
                yield return 0;
                continue;
            }

            zeroedYPosition = transform.position;
            zeroedYPosition.y = 0;

            if (!inBoundsXZ(transform.position) && !goToPosition && !anim.inAir) {

                yield return new WaitForSeconds(2.5f);

                if (anim.inAir)
                    continue;

                goToPosition = true;
                anim.hori = 1;

                Vector3 offsetPosition = new Vector3(randPlusMinusNum(areaBound.x), 0, randPlusMinusNum(areaBound.z));
                newPosition = areaPosition + offsetPosition;
                zeroedYNewPos = newPosition;
                zeroedYNewPos.y = 0;

                //moveOffset = ((zeroedYNewPos - zeroedYPosition).normalized * moveSpeed * Time.deltaTime);



            }
            else if (goToPosition) {

                if (Vector3.Distance(zeroedYPosition, zeroedYNewPos) > 1f) {
                    moveOffset = ((zeroedYNewPos - zeroedYPosition).normalized * moveSpeed * Time.deltaTime);
                    //print(zeroedYNewPos + " Current: " + zeroedYPosition + " | moveOffset " + moveOffset);
                    penguinRB.MovePosition(transform.position + moveOffset);
                    anim.rotatePenguin(moveOffset);
                }
                else {
                    goToPosition = false;
                    anim.hori = 0;
                }
            }
            yield return 0;
        }
    }


    private bool inBoundsXZ(Vector3 position) {
        //print(position.x + " | " + areaPosition.x + " | " + areaBound.x + " | " + between(position.x, areaPosition.x, areaBound.x));
        //print(position.z + " | " + areaPosition.z + " | " + areaBound.z + " | " + between(position.z, areaPosition.z, areaBound.z));
        return between(position.x, areaPosition.x, areaBound.x) && between(position.z, areaPosition.z, areaBound.z);
    }

    private bool between(float position, float areaPosition, float areaBounds) {
        return position > areaPosition - areaBounds && position < areaPosition + areaBounds;
    }

    
}
