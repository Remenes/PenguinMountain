using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBlock : ActionBlock {

    private ParticleSystem particles;

	// Use this for initialization
	void Start () {
        particles = GetComponentInChildren<ParticleSystem>();
	}
    /*	
	// Update is called once per frame
	void Update () {
		
	} */

    public float pausePlayerTime = .25f;
    public float bounceSpeed = 10f;
    public bool cancelXVel = false;
    public bool cancelYVel = false;
    public bool cancelZVel = false;

    protected override void performAction(Collider collided) {
        if (collided.gameObject.tag == "Player")
            Bounce(collided.gameObject);
    }

    protected void Bounce(GameObject player) {
        playParticleEffects();

        Rigidbody playerRB = player.GetComponent<Rigidbody>();
        Vector3 curVel = playerRB.velocity;
        

        Vector3 newVel = new Vector3(!cancelXVel ? curVel.x : 0, !cancelYVel ? curVel.y : 0, !cancelZVel ? curVel.z : 0);
        newVel += ((transform.rotation * Vector3.up) * bounceSpeed);
        
        playerRB.velocity = newVel;

        PlayerScript playerScript = player.GetComponent<PlayerScript>();

        if (playerScript)
            StartCoroutine(playerScript.PauseMovementAxis(pausePlayerTime));
    }

    private void playParticleEffects() {
        particles.Play();

    }  
        
}
