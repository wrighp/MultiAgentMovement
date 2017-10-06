using UnityEngine;
using System.Linq;
using System;

[RequireComponent(typeof(MeshFilter))]
public class Collider2DMesh : MonoBehaviour {


	public void TriangulateMesh () {
		PolygonCollider2D collider = gameObject.GetComponent<PolygonCollider2D>();
		if(collider == null){
			return;
		}
		MeshFilter mf = GetComponent<MeshFilter>();

		Vector2[] points = collider.points;

		Triangulator tris = new Triangulator(points);

		Mesh mesh = new Mesh();

		//Lambda expression to convert the vector2 array to vector3 array
		mesh.vertices = Array.ConvertAll<Vector2,Vector3>(points, vert => new Vector3(vert.x,vert.y,0));
		mesh.triangles = tris.Triangulate();
		mf.mesh = mesh;

	}
}