using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeCheck : MonoBehaviour {

	public float maxAngle = 60f; //Total angle of forward cone check (not a half angle)
	public float maxDistance = 2f;
	private AgentMovement agent;

	void Awake(){
		agent = GetComponent<AgentMovement>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		GameObject avoidTarget = null;
		float lowestDistance = maxDistance;

		//Get closest target within cone to avoid
		foreach(GameObject go in Obstacles.GetGameObjects()){
			//Ignore checking against yourself
			if(go == gameObject){
				continue;
			}

			Vector2 vectorDirection = go.transform.position - transform.position;
			Vector2 forwardDirection = transform.up;
			//Dot product to find angle (function then converts to 0-180 degrees)
			float angle = Vector2.Angle (vectorDirection, forwardDirection);

			//If the doubled half angle is less than the cone check angle consider for avoidance
			if(angle * 2f <= maxAngle){
				//Evade from closest character or from average of characters
				//In this case we are evading from closest character
				float distance = Vector2.Distance(transform.position, go.transform.position);

				if(distance < lowestDistance){
					lowestDistance = distance;
					avoidTarget = go; 
				}
			}
		}

		//If there was no target, proceed with normal movement
		if(avoidTarget == null){
			return;
		}

		//Else do avoidance on target
		agent.targetDirection = AvoidDirection(transform, avoidTarget.transform.position);
		//PlayerDebug.DrawRay(transform.position,agent.targetDirection,new Color(1f,0,0f,1f));
	}

	static Vector2 AvoidDirection(Transform transform, Vector3 targetPosition){
		Vector2 vectorDirection = targetPosition - transform.position;
		Vector2 forwardDirection = transform.up;
		//get relative angle (aiming left or right of) to target angle with simple cross product
		Vector3 cross = Vector3.Cross(vectorDirection, forwardDirection);
		float sign = Mathf.Sign(cross.z); //Used for determining wether to swivel away left or right
		return (Quaternion.Euler(0,0,90f * sign) *transform.up).normalized;
	}

	void LateUpdate(){
		
		Vector3 halfConeRight = (Quaternion.Euler(0,0,maxAngle *.5f) *transform.up).normalized;
		Vector3 halfConeLeft = (Quaternion.Euler(0,0,maxAngle * -.5f) * transform.up).normalized;

		Vector3 p1 = transform.position + halfConeLeft * maxDistance;
		Vector3 p2 = transform.position + halfConeRight * maxDistance;

		PlayerDebug.DrawLine(transform.position, p1,new Color(0,0,1f,.25f));
		PlayerDebug.DrawLine(transform.position, p2,new Color(0,0,1f,.25f));
		//Makes a triangle, not technically the cone that is being used, but helps to visualize it
		//PlayerDebug.DrawLine(p1, p2,new Color(0,0,1f,.25f));

	}
}
