using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemySc : MonoBehaviour {

	Canvas enemyCan;
	float curHP = 1;
	public float maxHP = 3.0f;
	float maxHpMulti = 1;
	public float MaxHpMulti{ get{ return maxHpMulti; } set { maxHpMulti = value; } }
	public float sweetsValue = 2;
	public float maxSpeed = 8;
	float curSpeed = 8;
	public int damage = 1;

	//buffs/debuffs

	// Shield/Armor values
	public float maxShield;
	float curShield;
	public float maxArmor;
	float curArmor;

	// HP/SHIELD/ARMOR bars
	public RectTransform hpBar;
	float cachedYHP;
	float minXValueHP;
	float maxXValueHP;

	public RectTransform armorBar;
	float cachedYArmor;
	float minXValueArmor;
	float maxXValueArmor;

	public RectTransform shieldBar;
	float cachedYShield;
	float minXValueShield;
	float maxXValueShield;
	//pathnode variables to find the path to go
	Transform targetPathNode;
	int pathNodeIndex;
	GameObject pathNodeParent;

	// Use this for initialization
	void Start () {
		//HPBar
		cachedYHP = hpBar.localPosition.y;
		maxXValueHP = hpBar.localPosition.x;
		minXValueHP = hpBar.localPosition.x - hpBar.rect.width;
		//ArmorBar
		cachedYArmor = armorBar.localPosition.y;
		maxXValueArmor = armorBar.localPosition.x;
		minXValueArmor = armorBar.localPosition.x - armorBar.rect.width;
		//ShieldBar
		cachedYShield = shieldBar.localPosition.y;
		maxXValueShield = shieldBar.localPosition.x;
		minXValueShield = shieldBar.localPosition.x - shieldBar.rect.width;
		//Stats etc
		curSpeed = maxSpeed;
		enemyCan = this.transform.FindChild ("EnemyCanvas").GetComponent<Canvas> ();
		pathNodeParent = GameObject.Find ("Path");
		curHP = maxHP;
		if (maxShield > 0) {
			curShield = maxShield;
		} else {
			shieldBar.gameObject.SetActive (false);
		}
		if (maxArmor > 0) {
			curArmor = maxArmor;
		} else {
			armorBar.gameObject.SetActive (false);
		}
	}
	// Call it once this reached it's targeted pathnode
	void GetNextPathNode(){
		try{
			targetPathNode = pathNodeParent.transform.GetChild (pathNodeIndex);
		}catch{
			ReachedGoal ();
			return;
		}
		pathNodeIndex++;
	}
	
	// Update is called once per frame
	void Update () {
		if (targetPathNode == null) {
			GetNextPathNode ();
			if (targetPathNode == null) {
				//No more path, go lose some life or something!
				ReachedGoal ();
				return;
			}
		}
			
		Vector3 dir = targetPathNode.position - this.transform.position;

		float distThisFrame = curSpeed * Time.deltaTime;

		if (dir.magnitude <= distThisFrame) {
			targetPathNode = null;
		} else {
			this.transform.Translate (dir.normalized * distThisFrame, Space.World);
			Quaternion targetRot = Quaternion.LookRotation (dir);
			this.transform.rotation = Quaternion.Lerp (this.transform.rotation, targetRot, Time.deltaTime * 10);
		}


	}

	void ReachedGoal(){
		GameObject.FindObjectOfType<ScoreManager> ().LoseLife (damage);
		Destroy (gameObject);
	}

	// shield->armor->hp
	public void TakeDamage(float damage){
		if (curShield > 0) {
			curShield -= damage;
			if (curShield < 0) {
				curArmor += curShield;
				curShield = 0;
				if (curArmor < 0) {
					curHP += curArmor;
					curArmor = 0;
				}
			}
		} else if(curArmor > 0){
			curArmor -= damage;
			if (curArmor < 0) {
				curHP += curArmor;
				curArmor = 0;
			}
		} else {
			curHP -= damage;
		}


		//set visuals HP/ARMOR/SHIELD
		if (curShield <= 0) {
			shieldBar.gameObject.SetActive (false);
			if (curArmor <= 0) {
				armorBar.gameObject.SetActive (false);
				if (curHP <= 0) {
					EnemySlain ();
				} else {
					float curXValueHP = VisualHP ();
					hpBar.localPosition = new Vector3 (curXValueHP, cachedYHP);
				}
			} else {
				float curXValueArmor = VisualArmor ();
				armorBar.localPosition = new Vector3 (curXValueArmor, cachedYArmor);
			}
		} else {
			float curXValueShield = VisualShield ();
			shieldBar.localPosition = new Vector3 (curXValueShield, cachedYShield);
		}
	}
		
	void EnemySlain(){
		//add monies etc
		GameObject.FindObjectOfType<ScoreManager>().Sweets += sweetsValue;
		Destroy (gameObject);
	}
	//methods to set visuals of shield/armor/hp bars
	float VisualHP(){
		return (curHP - 0) * (maxXValueHP - minXValueHP) / (maxHP - 0) + minXValueHP;
	}
	float VisualArmor(){
		return (curArmor - 0) * (maxXValueArmor - minXValueArmor) / (maxArmor - 0) + minXValueArmor;
	}
	float VisualShield(){
		return (curShield - 0) * (maxXValueShield - minXValueShield) / (maxShield - 0) + minXValueShield;
	}
}
