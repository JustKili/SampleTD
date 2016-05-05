using UnityEngine;
using System.Collections;

public class TowerSpot : MonoBehaviour {

	void OnMouseUp(){
		//Debug.Log ("SpotTest clickd");

		BuildingManager bm = GameObject.FindObjectOfType<BuildingManager> ();
		if (bm.selectedTower != null) {
			ScoreManager sm = GameObject.FindObjectOfType<ScoreManager> ();
			if (!sm.canActive) {
				if (sm.Sweets >= bm.selectedTower.GetComponent<Tower> ().Price) {
					GameObject tempTower = Instantiate (bm.selectedTower, transform.parent.position, transform.parent.rotation) as GameObject;
					tempTower.transform.SetParent (this.transform.parent.transform.parent);
					Destroy (transform.parent.gameObject);
					sm.Sweets -= bm.selectedTower.GetComponent<Tower> ().Price;
				} else {
					Debug.Log ("Not enough Money to build that!");
				}
			} else {
				Debug.Log ("You can't build while upgrading");
			}
		}
	}
}
