using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody2D))]
public class AgentMovement : MonoBehaviour {

	[HideInInspector]
	public Rigidbody2D rb;

	public Color agentColor = Color.blue;
	public float indicatorLineLength = .5f;
	public float maxAcceleration = 1f;
	public float maxSpeed = 1f;
	public float acceleration = 1f; //Should be scaled between 0 and 1
	public float turnSpeed = 4f;
	public Vector2 targetDirection = Vector2.zero;

	public Vector3 aimingDirection; //Direction that agent is pointing at

	void Awake(){
		rb = GetComponent<Rigidbody2D> ();

	}
	void Start(){
		aimingDirection = targetDirection;
	}
	void Update(){
		targetDirection.Normalize();
		aimingDirection = Vector3.Lerp(aimingDirection,  targetDirection, Time.deltaTime * turnSpeed);
		aimingDirection.Normalize();
		//Set absolute rotation
		float targetAngle = Mathf.Atan2 (aimingDirection.y, aimingDirection.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis (targetAngle - 90f, Vector3.forward);

		PlayerDebug.DrawRay (transform.position, aimingDirection * indicatorLineLength, Color.white);
		PlayerDebug.DrawCircle(transform.position, GetComponent<CircleCollider2D>().radius,agentColor);
	}
	void FixedUpdate(){
		acceleration = Mathf.Clamp01(acceleration);
		//If force should always be in target direction
		//rb.AddForce(targetDirection * Mathf.Min(maxAcceleration, acceleration * maxAcceleration));

		//If force should always be forward based on aimingDirection
		rb.AddForce(aimingDirection * Mathf.Min(maxAcceleration, acceleration * maxAcceleration));


		//This clamping is not actually accurate as the addForce will increase the speed for a frame after
		//If you disable force when speed is over max speed that creates hysteresis
		//A correct force reduction is more complicated
		rb.velocity = Vector2.ClampMagnitude (rb.velocity, maxSpeed);
	
	}

}
