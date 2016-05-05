using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public float spawnTimer = 1f;
	float spawnTimerLeft;
	ScoreManager sc;

	//stats to change on enemies on higher levels

	[System.Serializable]
	public class WaveComponent{
		public GameObject enemyPrefab;		//well obv
		public int quantityToSpawn;		//how much enemies are we gonna spawn ?
		[System.NonSerialized]
		public int spawned = 0;		//how much enemies were spawnd allready
	}

	public WaveComponent[] waveComps;

	void Start(){
		sc = GameObject.FindObjectOfType<ScoreManager> () as ScoreManager;
	}

	void Update(){
		spawnTimerLeft -= Time.deltaTime;

		if (spawnTimerLeft <= 0) {
			spawnTimerLeft = spawnTimer;
			bool wasSpawned = false;
			//spawn enemy
			foreach(WaveComponent wc in waveComps){
				if (wc.spawned < wc.quantityToSpawn) {
					//Spawn it?
					GameObject enemyTemp = Instantiate(wc.enemyPrefab, this.transform.position, this.transform.rotation) as GameObject;
					enemyTemp.transform.SetParent (this.transform);
					//need to set maxHpMulti before Start() method from the enemy
					enemyTemp.GetComponent<EnemySc> ().maxHP *= (sc.waveCount /10.0f) +1.0f;
					wc.spawned++;

					wasSpawned = true;
					break;
				}
			}
			if (!wasSpawned && transform.childCount == 0) {
				//waves completed spawn next spawner
				if (transform.parent.childCount <= 1) {
					Destroy (gameObject);
					//You've completed this level!
					Debug.Log("GZ YOU've WON!");
				} else {
					sc.waveCount++;
					transform.parent.GetChild (1).gameObject.SetActive (true);
					Destroy (gameObject);
				}
			}
		}
	}
}