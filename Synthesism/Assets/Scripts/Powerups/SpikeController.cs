using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : MonoBehaviour {

	public ParticleSystem explosion;

	void OnTriggerEnter(Collider other) {
		StartCoroutine(destroySpike(other));
		explosion.Play();
	}

	private IEnumerator destroySpike(Collider other) {
		yield return new WaitForSeconds(1);
		Destroy(gameObject);
		if(other.gameObject.tag=="Player") {
			other.gameObject.SetActive(false);
		}
	}
}
