using UnityEngine;
using System.Collections;

public class DamageOverTime : TimedEffect {

	public float damagePerTick;

	protected override void ApplyEffect () {
		target.TakeDamage(damagePerTick);
		//Debug.Log ("Working");
	}
}

