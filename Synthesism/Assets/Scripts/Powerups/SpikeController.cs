using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : MonoBehaviour {

	public ParticleSystem explosion;

	void OnTriggerEnter(Collider other) {
		StartCoroutine(destroySpike(other));
	}

	private IEnumerator destroySpike(Collider other) {
		explosion.Play();
		yield return new WaitForSeconds(1);
		Destroy(gameObject);
		if(other.gameObject.tag=="Player") {
			other.gameObject.SetActive(false);
		}
	}
}
