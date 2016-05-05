using UnityEngine;
using System.Collections;

public class Path : MonoBehaviour {



	// Use this for initialization
	void Start () {
		foreach (Transform child in transform) {
			child.GetComponent<MeshRenderer> ().enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
