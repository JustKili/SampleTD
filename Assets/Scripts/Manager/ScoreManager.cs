using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	public int lives = 20;
	float sweets = 50;

	public float Sweets {
		get {return sweets;}
		set {sweets = value;}
	}

	public int waveCount = 1;

	Text scoreTxt;

	//preventing to build while upgrade screen is there fix that PLS later on it sucks liek this
	public bool canActive;
	//rlly fix this u fagg

	void Start(){
		scoreTxt = GameObject.Find ("ScoreTxt").GetComponent<Text> ();
	}

	public void LoseLife(int l){
		lives -= l;
		if (lives <= 0) {
			GameOver ();
		}
	}

	public void GameOver(){
		//GameOver :D
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	void Update(){
		//FIXME: only update when values are changed
		scoreTxt.text = "Sweets:\n" + (int)sweets + "\nLives: " + lives;
	}
}
