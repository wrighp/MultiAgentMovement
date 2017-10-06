using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Obstacles{

	private static List<GameObject> gameobjects;
	private static List<Rigidbody2D> rigidbodies;
	private static List<Transform> transforms;

	static Obstacles(){
		
	}

	static void BuildLists(){
		gameobjects = new List<GameObject>();
		rigidbodies = new List<Rigidbody2D>();
		transforms = new List<Transform>();

		foreach(AgentMovement agent in Object.FindObjectsOfType<AgentMovement>()){
			gameobjects.Add(agent.gameObject);
			rigidbodies.Add(agent.GetComponent<Rigidbody2D>());
			transforms.Add(agent.GetComponent<Transform>());
		}
	}

	public static List<GameObject> GetGameObjects(){
		if(gameobjects == null){
			BuildLists();
		}
		return new List<GameObject>(gameobjects);
	}

	public static List<Rigidbody2D> GetRigidbodies(){
		if(rigidbodies == null){
			BuildLists();
		}
		return new List<Rigidbody2D>(rigidbodies);
	}
		
	public static List<Transform> GetTransforms(){
		if(transforms == null){
			BuildLists();
		}
		return new List<Transform>(transforms);
	}

}
