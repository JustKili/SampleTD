using UnityEngine;
using System.Collections;

public class DetectCol : MonoBehaviour {

	bool isColliding = false;
	public bool IsColliding {
		get {return isColliding;}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider otherCol){
		//Debug.Log ("Colliding");
		isColliding = true;
		this.GetComponent<MeshRenderer> ().material.color = new Color (255, 0, 0);
	}
		
	void OnTriggerExit(Collider otherCol){
		//Debug.Log ("Not Colliding anymore");
		isColliding = false;
		this.GetComponent<MeshRenderer> ().material.color = new Color (0, 255, 0);
	}
}
