using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public float speed = 50f;
	public Transform target;

	public int charges = 0;
	public bool canPoison;
	public GameObject timedEffect;

	float damage = 1f;
	public float Damage{ 
		get{ return damage; } 
		set{ damage = value; }
	}

	float radius = 0f;
	public float Radius{ 
		get{ return radius; } 
		set{ radius = value; }
	}

	public GameObject soundObj;
	public GameObject particleSys;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (target == null) {
			Destroy (gameObject);
			return;
		}

		Vector3 dir = target.position - this.transform.localPosition;

		float distThisFrame = speed * Time.deltaTime;

		if (dir.magnitude <= distThisFrame) {
			DoBulletHit ();
		} else {
			this.transform.Translate (dir.normalized * distThisFrame, Space.World);
			Quaternion targetRot = Quaternion.LookRotation (dir);
			this.transform.rotation = Quaternion.Lerp (this.transform.rotation, targetRot, Time.deltaTime * 10);
		}
	}

	void DoBulletHit(){
		//Poison the enemy if tower can poison
		if (canPoison) {
			GameObject tempEffect = Instantiate (timedEffect, transform.position, transform.rotation) as GameObject;
			tempEffect.transform.SetParent (target.transform);
			tempEffect.GetComponent<DamageOverTime> ().target = this.target.GetComponent<EnemySc>();
			tempEffect.GetComponent<DamageOverTime> ().duration = 5;	//hardcoded change later on just to test with hard values
			tempEffect.GetComponent<DamageOverTime> ().startTime = 0;
			tempEffect.GetComponent<DamageOverTime> ().repeatTime = 1f;
			tempEffect.GetComponent<DamageOverTime> ().damagePerTick = this.damage;
			Instantiate (soundObj, transform.position, transform.rotation);
			if (charges > 0) {
				//search for nearest enemy
				ShootAtNextEnemy();
				charges--;
			} else {
				Destroy (gameObject);
			}
			return;
		}
		//Deal damage
		if (radius == 0) {
			target.GetComponent<EnemySc> ().TakeDamage (damage);
		} else {
			Collider[] cols = Physics.OverlapSphere (transform.position, radius);

			foreach (Collider c in cols) {
				EnemySc e = c.GetComponent<EnemySc> ();
				if (e != null) {
					e.GetComponent<EnemySc> ().TakeDamage (damage);
				}
			}
			Instantiate (particleSys, transform.position, transform.rotation);
		}
		//spawn soundobject
		Instantiate (soundObj, transform.position, transform.rotation);
		if (charges > 0) {
			//search for nearest enemy
			ShootAtNextEnemy();
			charges--;
		} else {
			Destroy (gameObject);
		}
	}

	void ShootAtNextEnemy(){
		EnemySc[] enemies = GameObject.FindObjectsOfType<EnemySc>();

		EnemySc nearestEnemy = null;
		float dist = Mathf.Infinity;

		foreach(EnemySc e in enemies){
			float d = Vector3.Distance (this.transform.position, e.transform.position);
			if (nearestEnemy == null || d < dist) {
				if (target.GetComponent<EnemySc>() != e) {
					nearestEnemy = e;
					dist = d;
				}
			}
		}
		if (nearestEnemy == null) {
			//Debug.Log ("No enemies?");
			return;
		} else {
			target = nearestEnemy.transform;
		}
	}
}
