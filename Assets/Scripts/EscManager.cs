using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EscManager : MonoBehaviour {

	public Button exitBut;
	public Button switchLvlBut;

	// Use this for initialization
	void Start () {
		//exitBut = GameObject.Find ("ExitBut").GetComponent<Button>() as Button;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (exitBut.gameObject.activeSelf) {
				exitBut.gameObject.SetActive(false);
				switchLvlBut.gameObject.SetActive(false);
			} else {
				exitBut.gameObject.SetActive(true);
				switchLvlBut.gameObject.SetActive(true);
			}
		}
	}

	public void ExitToWindows(){
		if(Application.isEditor){
			//Do nothing since it wont compile ? wtf
		}else{
			Application.Quit ();
		}
	}

	public void SwitchLevel(string levelName){
		SceneManager.LoadScene (levelName);
	}
}
