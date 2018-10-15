using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : MonoBehaviour {

	public ParticleSystem explosion;
    public float timeToDeath;
	private bool hasTriggered = false;
    public float speed;
    public Rigidbody rb;

    private void Start()
    {
        rb.velocity = transform.forward * speed;
    }

    void OnTriggerEnter(Collider other) {
        //add player kill here
		StartCoroutine(destroySpike(other));
	}

	private IEnumerator destroySpike(Collider other) {
		if(!hasTriggered) {
			hasTriggered = true;
			for(int i = 0; i < 2; i++)
				Destroy(gameObject.transform.GetChild(i).transform.gameObject);
            GetComponent<MeshRenderer>().enabled = false;
			explosion.Play();
			yield return new WaitForSeconds(timeToDeath);
			explosion.Stop();
			Destroy(gameObject);			
		}
	}

}
