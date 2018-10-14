using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : MonoBehaviour {

	public ParticleSystem explosion;

	private bool hasTriggered = false;

	void OnTriggerEnter(Collider other) {
		StartCoroutine(destroySpike(other));
	}

	private IEnumerator destroySpike(Collider other) {
		if(!hasTriggered) {
			hasTriggered = true;
			for(int i = 0; i < 3; i++)
				Destroy(gameObject.transform.GetChild(i).transform.gameObject);

			explosion.Play();
			yield return new WaitForSeconds(0.4f);
			explosion.Stop();
			if(other.gameObject.tag=="Player") {
				other.gameObject.SetActive(false);
				yield return new WaitForSeconds(2.5f);
				other.gameObject.SetActive(true);
			}
			Destroy(gameObject);			
		}
	}

}
