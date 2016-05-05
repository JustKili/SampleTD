using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	ScoreManager sm;
	BuildingManager bm;
	CamControl camController;
	public GameObject towerButPre;
	string[] towerTexts = new string[6];


	// Use this for initialization
	void Start () {
		towerTexts [0] = "Balista\nP:12";
		towerTexts [1] = "Morser\nAOE-Bullet\nP:25";
		towerTexts [2] = "SpinSword\nAOE\nP:25";
		towerTexts [3] = "Canon\nP:15";
		towerTexts [4] = "Magix Thunder\nP:20";
		towerTexts [5] = "Poison\nP:15";
		sm = GameObject.FindObjectOfType<ScoreManager> ();
		bm = GameObject.FindObjectOfType<BuildingManager> ();
		camController = GameObject.FindObjectOfType<CamControl> ();

		for (int i=0; i<bm.towers.Length; i++) {
			GameObject tempBut = Instantiate (towerButPre) as GameObject;
			tempBut.transform.SetParent(transform.FindChild("Towers"));
			tempBut.transform.FindChild("Text").GetComponent<Text> ().text = towerTexts[i];		//have a txt file to get the texts correct ?
			//tempBut.GetComponent<Button>().onClick.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.Off);
			int temp_i = i+1;
			tempBut.GetComponent<Button>().onClick.AddListener(delegate{bm.ChangeTower(temp_i);});	//only chosing the last tower on the list(why?)
			tempBut.transform.localScale = new Vector3(.8f,.8f,.8f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
