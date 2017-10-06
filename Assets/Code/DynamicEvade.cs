using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AgentMovement))]
public class DynamicEvade : DynamicPursue {

	override protected void Awake(){
		base.Awake();
	}
	// Use this for initialization
	override protected void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	override protected void Update () {
		base.Update ();
		agent.targetDirection *= -1f;
	}
}
