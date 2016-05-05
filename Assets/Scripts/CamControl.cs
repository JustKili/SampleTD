using UnityEngine;
using System.Collections;

public class CamControl : MonoBehaviour {

	Camera myCam;
	GameObject yourTowers;
	float maxY = 40;
	float minY = 10;
	float scrollSpeed = 2.5f;
	float curYValue;
	public float camMoveSpeed = 20f;
	float canvasSize = 1750f;

	Vector3 mousePos;

	// Use this for initialization
	void Start () {
		yourTowers = GameObject.Find ("YourTowers");
		myCam = transform.GetComponent<Camera>();
		curYValue = this.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			curYValue -= scrollSpeed;
			if (curYValue < minY) {
				curYValue = minY;
			}
			this.transform.position = new Vector3 (this.transform.position.x, curYValue, this.transform.position.z);
			//Method to change size of Tower Canvases use the curYValue for it
			for (int i=0; i<yourTowers.transform.childCount;i++) {
				Transform tempChild = yourTowers.transform.GetChild (i).FindChild ("TowerCanvas");
				tempChild.transform.localScale = new Vector3(curYValue / canvasSize,curYValue / canvasSize,curYValue / canvasSize);
			}
		}
			
		if(Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			curYValue += scrollSpeed;
			if (curYValue > maxY) {
				curYValue = maxY;
			}
			this.transform.position = new Vector3 (this.transform.position.x, curYValue, this.transform.position.z);
			for (int i=0; i<yourTowers.transform.childCount;i++) {
				Transform tempChild = yourTowers.transform.GetChild (i).FindChild ("TowerCanvas");
				tempChild.transform.localScale = new Vector3(curYValue / canvasSize,curYValue / canvasSize,curYValue / canvasSize);
			}
		}

		//control with mouse
		if(Input.GetMouseButton(2))
		{
			float h = scrollSpeed * Input.GetAxis("Mouse X");
			float v = scrollSpeed * Input.GetAxis("Mouse Y");
			Camera.main.transform.Translate(-h, 0, -v, Space.World);
		}

		//control with arrows or wasd
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
			//move right
			this.transform.position = new Vector3(this.transform.position.x+camMoveSpeed*Time.deltaTime, this.transform.position.y, this.transform.position.z);
		}
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
			//move right
			this.transform.position = new Vector3(this.transform.position.x-camMoveSpeed*Time.deltaTime, this.transform.position.y, this.transform.position.z);
		}
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
			//move right
			this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z+camMoveSpeed*Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
			//move right
			this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z-camMoveSpeed*Time.deltaTime);
		}
	}
}
