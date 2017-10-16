using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class PlayerDebug : MonoBehaviour {

	public static PlayerDebug instance;
	public Material mat;
	public int defaultCircleSegments = 16;
	public bool renderOnPause = true;
	List<Vector3> verts;
	List<Color> colors;
	List<float> lineTimes;

	List<Vector3> circleCenters;
	List<float> circleRadii;
	List<Color> circleColors;
	List<int> circleSegments;
	List<float> circleTimes;

	bool paused = false;

	void Awake(){
		EditorApplication.playmodeStateChanged += OnStateChange;

		instance = this;
		verts = new List<Vector3>();
		colors = new List<Color>();
		lineTimes = new List<float>();

		circleCenters = new List<Vector3>();
		circleRadii = new List<float>();
		circleColors = new List<Color>();
		circleSegments = new List<int>();
		circleTimes = new List<float>();
	}

	private void OnStateChange(){
		paused = renderOnPause && Application.isEditor && EditorApplication.isPaused;
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		
	}

	public static void DrawCircle(Vector3 position, float radius, Color? color = null, float time = 0, int? segments = null){
		if(instance == null){
			return;
		}
		instance.AddCircle(position,radius,color,time,segments);
	}
	public void AddCircle(Vector3 position, float radius, Color? color = null, float time = 0, int? segments = null){
		color = color ?? Color.white;
		segments = segments ?? Math.Max(3, defaultCircleSegments);
		circleCenters.Add(position);
		circleRadii.Add(radius);
		circleColors.Add(color.Value);
		circleSegments.Add(segments.Value);
		circleTimes.Add (time);
	}

	public static void DrawLine(Vector3 v1, Vector3 v2, Color? color = null, float time = 0){
		if(instance == null){
			return;
		}
		instance.AddLine(v1,v2,color, time);
	}
	public static void DrawRay(Vector3 v1, Vector3 v2, Color? color = null, float time = 0){
		if(instance == null){
			return;
		}
		color = color ?? Color.white;
		instance.AddLine(v1,v1 + v2,color, time);
	}


	public void AddLine(Vector3 v1, Vector3 v2, Color? color = null, float time = 0){
		color = color ?? Color.white;
		verts.Add(v1);
		verts.Add(v2);
		colors.Add(color.Value);
		lineTimes.Add(time);
	}

	// LateUpdate is called once per frame
	void LateUpdate(){
		for (int i = 0, j = 0, colorsCount = colors.Count; i < colorsCount; i++, j += 2) {
			Debug.DrawLine(verts[j], verts[j + 1], colors[i], 0, false);
		}
	}


	void OnPostRender () {
		DrawLines();
		DrawCircles();
	}
	void DrawLines(){
		GL.PushMatrix();
		mat.SetPass(0);
		mat.color = Color.white;
		//GL.LoadOrtho();
		GL.Begin(GL.LINES);
		Color c = Color.white;

		for (int i = 0, j = 0, colorsCount = colors.Count; i < colorsCount; i++, j += 2) {
			if(c != colors[i]){
				GL.Color(colors[i]);
				c = colors[i];
			}
			GL.Vertex(verts [j]);
			GL.Vertex(verts [j + 1]);
		}

		GL.End();
		GL.PopMatrix();

		if(!paused){

			int count = lineTimes.Count;
			//Add line back in if time isn't up
			float t = Time.deltaTime;
			for (int i = 0; i < count; i++) {
				float d = lineTimes [i];
				if(d >= 0){
					lineTimes.Add(d - t);
					verts.Add(verts[i*2]);
					verts.Add(verts[i*2 + 1]);
					colors.Add(colors[i]);
				}
			}
   

			lineTimes.RemoveRange(0,count);
			verts.RemoveRange(0,count * 2);
			colors.RemoveRange(0,count);
		}
	}

	void DrawCircles(){
		

		GL.PushMatrix();
		mat.SetPass(0);
		mat.color = Color.white;

		const float totalRadians = Mathf.PI * 2f;


		for (int i = 0, circleCentersCount = circleCenters.Count; i < circleCentersCount; i++) {
			Vector3 center = circleCenters [i];
			float z = center.z;
			float radius = circleRadii[i];
			Color color = circleColors[i];

			GL.Begin(GL.TRIANGLE_STRIP);
			GL.Color(color);

			float x1 = center.x;
			float y1 = center.y;

			float angleInc = totalRadians / circleSegments[i];

			GL.Vertex3(x1,y1,z);
			for (float angle=0f;angle<totalRadians;angle += angleInc)
			{
				float x2 = x1 + Mathf.Sin(angle)*radius;
				float y2 = y1 + Mathf.Cos(angle)*radius;
				GL.Vertex3(x2,y2,z);
				GL.Vertex3(x1,y1,z);
			}
			GL.Vertex3( x1 + Mathf.Sin(0)*radius, y1 + Mathf.Cos(0)*radius,z);

			GL.End();
		}
		GL.PopMatrix();


		if(!paused){

			int count = circleTimes.Count;
			//Add line back in if time isn't up
			float t = Time.deltaTime;
			for (int i = 0; i < count; i++) {
				float d = circleTimes [i];
				if(d >= 0){
					circleTimes.Add(d - t);
					circleRadii.Add(circleRadii[i]);
					circleColors.Add(circleColors[i]);
					circleSegments.Add(circleSegments[i]);
					circleCenters.Add (circleCenters [i]);
				}
			}

			circleTimes.RemoveRange(0,count);
			circleRadii.RemoveRange(0,count);
			circleColors.RemoveRange(0,count);
			circleSegments.RemoveRange(0,count);
			circleCenters.RemoveRange (0, count);
		}

	}



}
