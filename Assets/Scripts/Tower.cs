using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tower : MonoBehaviour {

	Canvas towerCan;
	Button upgradeButDMG;
	Button upgradeButSpeed;

	Transform turretTrans;
	public float range = 10f;

	public GameObject bulletPrefab;
	public int charges;

	public float fireCooldown = .8f;
	public float cdReduction= 0.05f;
	float fireCooldownLeft = 0f;

	public float damage = 1f;
	public float upgradeDamage = 0f;
	public float upgradePrice;
	//add another one: DmgPerUpgrade ?
	public float radius = 0;

	public float price = 25f;
	public float Price{ 
		get{ return price; } 
		set{ price = value; }
	}
	public bool isAOE;
	public bool canPoison;

	//BUILDINGTIME
	EventSystem _eventSystem;
	public string inOfficialName;

	// Use this for initialization
	void Start () {
		towerCan = this.transform.FindChild ("TowerCanvas").GetComponent<Canvas> ();
		turretTrans = transform.FindChild ("Turret");
		upgradeButDMG = towerCan.transform.Find ("UpgradeBut").GetComponent<Button>();
		upgradeButDMG.onClick.AddListener(() => { GameObject.FindObjectOfType<UpgradeManager> ().DamageUpgrade();});
		upgradeButSpeed = towerCan.transform.Find ("UpgradeButSpeed").GetComponent<Button>();
		upgradeButSpeed.onClick.AddListener(() => { GameObject.FindObjectOfType<UpgradeManager> ().SpeedUpgrade();});
		// see above: remember that u can do this with EVERY button u'll add
		_eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
		upgradePrice = price * 0.75f;
		towerCan.transform.FindChild("MyDmg").GetComponent<Text>().text = "Damage: \n" + (upgradeDamage + damage) +"\n Price: "+upgradePrice;
		towerCan.transform.FindChild("MySpeed").GetComponent<Text>().text = "Speed: \n" + (fireCooldown) +"\n Price: "+upgradePrice;
	}
	
	// Update is called once per frame
	void Update () {
		//TODO: Optimize this
		EnemySc[] enemies = GameObject.FindObjectsOfType<EnemySc>();

		EnemySc nearestEnemy = null;
		float dist = Mathf.Infinity;

		foreach(EnemySc e in enemies){
			float d = Vector3.Distance (this.transform.position, e.transform.position);
			if (nearestEnemy == null || d < dist) {
				nearestEnemy = e;
				dist = d;
			}
		}

		if (nearestEnemy == null) {
			//Debug.Log ("No enemies?");
			return;
		}

		Vector3 dir = nearestEnemy.transform.position - this.transform.position;

		Quaternion lookRot = Quaternion.LookRotation (dir);

		turretTrans.rotation = Quaternion.Euler(0, lookRot.eulerAngles.y, 0);

		fireCooldownLeft -= Time.deltaTime;
		if (fireCooldownLeft <= 0 && dir.magnitude <= range) {
			fireCooldownLeft = fireCooldown;
			if (isAOE) {
				DealAOEDamage ();
			} else {
				ShootAt (nearestEnemy);
			}
		}
	}

	void ShootAt (EnemySc e){
		//TODO: Fire out of canons tipp
		GameObject bulletGO = (GameObject)Instantiate (bulletPrefab, this.transform.position, this.transform.rotation);

		Bullet b = bulletGO.GetComponent<Bullet> ();
		b.target = e.transform;
		b.Damage = damage+upgradeDamage;
		b.Radius = radius;
		b.charges = charges;
		b.canPoison = canPoison;
	}

	void DealAOEDamage(){
		Collider[] cols = Physics.OverlapSphere (transform.position, range);

		foreach (Collider c in cols) {
			EnemySc e = c.GetComponent<EnemySc> ();
			if (e != null) {
				e.TakeDamage (damage+upgradeDamage);
			}
		}
	}

	void OnMouseUp(){
		//UpgradeManager
		ScoreManager sm = GameObject.FindObjectOfType<ScoreManager> ();
		UpgradeManager um = GameObject.FindObjectOfType<UpgradeManager> ();
		if (_eventSystem.IsPointerOverGameObject ()) {
			//we r over an UI element aka Upgrade Button, peace out
			return;
		} else {
			if (um.selectedTower == null) {
				um.selectedTower = this.gameObject;
				if (um.selectedTower != null) {
					towerCan.enabled = true;
					sm.canActive = true;
				}
			} else if (um.selectedTower == this.gameObject) {
				um.selectedTower = null;
				towerCan.enabled = false;
				sm.canActive = false;
			} else {
				//another towers UpgradeCan is active
				um.selectedTower.GetComponent<Tower> ().towerCan.enabled = false;
				um.selectedTower = this.gameObject;
				towerCan.enabled = true;
				sm.canActive = true;
			}
		}
		// we still need that 2D buttons block raycast for 3D object. Like click the button and have another towers UpgradeCan open.
	}
}
