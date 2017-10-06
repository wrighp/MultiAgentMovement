using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody2D))]
public class AgentMovement : MonoBehaviour {

	public Color agentColor = Color.blue;
	public float indicatorLineLength = .5f;
	public float maxAcceleration;
	public float maxSpeed;
	public float acceleration; //Should be scaled between 0 and 1

	public float maxAngularAcceleration;
	public float maxAngularSpeed;
	public float angularAcceleration; //Scale between 0 and 1
	public Vector2 targetDirection = Vector2.zero;
	public Rigidbody2D rb;

	void Awake(){
		rb = GetComponent<Rigidbody2D> ();

	}
	void Start(){
		//Set absolute rotation to lock it
		float targetAngle = Mathf.Atan2 (targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis (targetAngle - 90f, Vector3.forward);
	}
	void Update(){
		PlayerDebug.DrawRay (transform.position, transform.up * indicatorLineLength, Color.white);
		PlayerDebug.DrawCircle(transform.position, GetComponent<CircleCollider2D>().radius,agentColor);
	}
	void FixedUpdate(){
		
		rb.AddForce(transform.up * Mathf.Min(maxAcceleration, acceleration * maxAcceleration));

		//This clamping is not actually accurate as the addForce will increase the speed for a frame after
		//If you disable force when speed is over max speed that creates hysteresis
		//A correct force reduction is more complicated
		rb.velocity = Vector2.ClampMagnitude (rb.velocity, maxSpeed);

		TurnToTarget ();
	
	}

	void TurnToTarget(){
		targetDirection.Normalize ();

		Vector2 forwardDirection = transform.up;

		//get relative angle to target angle
		Vector3 cross = Vector3.Cross(targetDirection, forwardDirection);
		float sign = Mathf.Sign(cross.z);
		float angle = Vector2.Angle (targetDirection, forwardDirection);

		//Clamps max acceleration and max speed
		rb.angularVelocity = Mathf.Clamp (rb.angularVelocity, -maxAngularSpeed, maxAngularSpeed);

		//Snap to rotation if within 1 degree of target or expected to overshoot target next frame
		if (Mathf.Abs (angle) < Mathf.Max (Mathf.Abs (rb.angularVelocity) * Time.fixedDeltaTime * 4f, 1f)) {
			rb.angularVelocity = 0;
			//Set absolute rotation to lock it
			float targetAngle = Mathf.Atan2 (targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis (targetAngle - 90f, Vector3.forward);
		}
		else {
			rb.AddTorque (-sign * angularAcceleration * maxAngularAcceleration, ForceMode2D.Force);
		}


	}
}
