using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour {

	public GameObject selectedTower;
	ScoreManager sm;

	// Use this for initialization
	void Start () {
		sm = GameObject.FindObjectOfType<ScoreManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DamageUpgrade(){
		Tower tempTow = selectedTower.GetComponent<Tower> ();
		if (tempTow.upgradePrice <= sm.Sweets) {
			sm.Sweets -= tempTow.upgradePrice;
			tempTow.upgradePrice *= 1.15f;
			tempTow.upgradeDamage += 0.2f * tempTow.damage;
			ChangeVisuals (tempTow);
		} else {
			Debug.Log ("You dont have enough moneyZ to upgrade");
		}
	}

	public void SpeedUpgrade(){
		Tower tempTow = selectedTower.GetComponent<Tower> ();
		if (tempTow.upgradePrice <= sm.Sweets) {
			sm.Sweets -= tempTow.upgradePrice;
			tempTow.upgradePrice *= 1.25f;
			tempTow.fireCooldown *= 1 - tempTow.cdReduction;
			ChangeVisuals (tempTow);
		} else {
			Debug.Log ("You dont have enough moneyZ to upgrade");
		}
	}

	void ChangeVisuals(Tower targetTower){
		selectedTower.transform.Find ("TowerCanvas").Find ("MySpeed").GetComponent<Text> ().text = "Speed: \n" + (targetTower.fireCooldown) 
			+"\n Price: "+targetTower.upgradePrice;
		selectedTower.transform.Find ("TowerCanvas").Find ("MyDmg").GetComponent<Text> ().text = "Damage: \n" + ((float)(targetTower.upgradeDamage + targetTower.damage))
			+"\n Price: "+targetTower.upgradePrice;
	}
}
