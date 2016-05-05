using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour {

	public GameObject[] towers;
	public GameObject selectedTower;
	bool towerInstantiated = false;
	public GameObject colCube;
	GameObject tempTower;
	GameObject tempTowerToSpawn;

	EventSystem _eventSystem;

	//number of towers build
	public int balistatsBuild = 0;
	public int morsersBuild = 0;
	public int spinswordsBuild = 0;
	public int canonsBuild = 0;
	public int thundersBuild = 0;
	public int poisonsBuild = 0;

	// Use this for initialization
	void Start () {
		_eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		SelectSpawnPos ();
	}

	public void ChangeTower(int towerIndex){
		selectedTower = towers [towerIndex-1];	//dont care for indexing from 0 just name the towers right
	}

	void SpawnTower(){
		if (tempTower.GetComponent<DetectCol> ().IsColliding == false && !Input.GetKey(KeyCode.LeftShift)) {	//collider of tempTower has no collision
			towerInstantiated = false;
			if (selectedTower != null) {
				ScoreManager sm = GameObject.FindObjectOfType<ScoreManager> ();
				if (sm.Sweets >= selectedTower.GetComponent<Tower> ().Price *(1f+CheckTowersBuild())) {
					tempTowerToSpawn = Instantiate (selectedTower, tempTower.transform.position, tempTower.transform.rotation) as GameObject;
					tempTowerToSpawn.transform.SetParent (GameObject.Find ("YourTowers").transform);
					sm.Sweets -= selectedTower.GetComponent<Tower> ().Price *(1f+CheckTowersBuild());
					Debug.Log (selectedTower.GetComponent<Tower> ().Price *(1f + CheckTowersBuild ()));
					Destroy (tempTower);
					tempTower = null;
					CountTowers ();
				} else {
					Debug.Log ("Not enough Money to build that!");
					Destroy (tempTower);
					tempTower = null;
					//let the player still try to build another tower?!
				}
			}
			selectedTower = null;
		}else if(tempTower.GetComponent<DetectCol> ().IsColliding == false && Input.GetKey(KeyCode.LeftShift)){
			towerInstantiated = false;
			if (selectedTower != null) {
				ScoreManager sm = GameObject.FindObjectOfType<ScoreManager> ();
				if (sm.Sweets >= selectedTower.GetComponent<Tower> ().Price *(1f+CheckTowersBuild())) {
					tempTowerToSpawn = Instantiate (selectedTower, tempTower.transform.position, tempTower.transform.rotation) as GameObject;
					tempTowerToSpawn.transform.SetParent (GameObject.Find ("YourTowers").transform);
					sm.Sweets -= selectedTower.GetComponent<Tower> ().Price *(1f+CheckTowersBuild());
					//Debug.Log (selectedTower.GetComponent<Tower> ().Price *(1f+CheckTowersBuild ()));
					Destroy (tempTower);
					tempTower = null;
					CountTowers ();
					SelectSpawnPos ();
					tempTower = selectedTower;
				} else {
					Debug.Log ("Not enough Money to build that!");
					Destroy (tempTower);
					tempTower = null;
					//let the player still try to build another tower?!
				}
			}
		}else {
			Debug.Log ("You can't build here");
		}
	}

	void SelectSpawnPos(){
		if (selectedTower != null && !towerInstantiated && !Input.GetMouseButtonDown(0)) {
			tempTower = Instantiate(colCube);
			towerInstantiated = true;
		}else if (selectedTower != null && towerInstantiated  && !Input.GetMouseButtonDown(0)) {
			float xx = 0;
			float yy = 0;
			Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				xx = hit.point.x;
				yy = hit.point.z;
			} else {
				//Stay on last pos
				xx = tempTower.transform.position.x;
				yy = tempTower.transform.position.z;
			}
			tempTower.transform.position = new Vector3 (xx, 0.1f, yy);		//towerbase follow mouse
		}
		if (Input.GetMouseButtonDown(0) && tempTower != null) {
			if (_eventSystem.IsPointerOverGameObject ()) {
				//we r over an UI element aka Upgrade Button, peace out
				Debug.Log("We r over an UI element");
				return;
			}
			SpawnTower ();
		}
	}

	void CountTowers(){
		if (selectedTower.GetComponent<Tower>().inOfficialName == "balista") {
			balistatsBuild += 1;
		}else if (selectedTower.GetComponent<Tower>().inOfficialName == "morser") {
			morsersBuild += 1;
		}else if (selectedTower.GetComponent<Tower>().inOfficialName == "spinsword") {
			spinswordsBuild += 1;
		}else if (selectedTower.GetComponent<Tower>().inOfficialName == "canon") {
			canonsBuild += 1;
		}else if (selectedTower.GetComponent<Tower>().inOfficialName == "thunder") {
			thundersBuild += 1;
		}else if (selectedTower.GetComponent<Tower>().inOfficialName == "poison") {
			poisonsBuild += 1;
		}
	}

	//like this we get a 25% rise at the price for each build tower
	float CheckTowersBuild(){
		if (selectedTower.GetComponent<Tower> ().inOfficialName == "balista") {
			return balistatsBuild * 0.25f;
		} else if (selectedTower.GetComponent<Tower> ().inOfficialName == "morser") {
			return morsersBuild * 0.25f;
		} else if (selectedTower.GetComponent<Tower> ().inOfficialName == "spinsword") {
			return spinswordsBuild * 0.25f;
		} else if (selectedTower.GetComponent<Tower> ().inOfficialName == "canon") {
			return canonsBuild * 0.25f;
		} else if (selectedTower.GetComponent<Tower> ().inOfficialName == "thunder") {
			return thundersBuild * 0.25f;
		} else if (selectedTower.GetComponent<Tower> ().inOfficialName == "poison") {
			return poisonsBuild * 0.25f;
		} else {
			//wrong call ?
			return 0f;
		}
	}
}
