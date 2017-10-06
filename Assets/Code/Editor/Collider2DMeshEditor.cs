using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Collider2DMesh))]
public class Collider2DMeshEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if(GUILayout.Button("Create Collider Mesh"))
		{
			Collider2DMesh cdMesh = (Collider2DMesh)target;
			Undo.RecordObject(cdMesh.GetComponent<MeshFilter>(), "Create Collider Mesh");
			cdMesh.TriangulateMesh();

		}
		if(GUILayout.Button("Delete Collider Mesh"))
		{
			Collider2DMesh cdMesh = (Collider2DMesh)target;
			MeshFilter mf = cdMesh.GetComponent<MeshFilter>();
			Undo.RecordObject(mf, "Delete Collider Mesh");
			mf.mesh = null;
		}
	}
}