﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : MonoBehaviour {

	public ParticleSystem explosion;
    public float timeToDeath;
	private bool hasTriggered = false;
    public float speed;
    public Rigidbody rb;

    private GameObject owner;

    private void Start()
    {
        rb.velocity = transform.forward * speed;
    }

    void OnTriggerEnter(Collider other) {
        //add player kill here
        if(other.gameObject != owner && other.tag != "Boost" && other.tag != "Checkpoint")
        {
            StartCoroutine(destroySpike(other));
            if (other.tag == "Player" || other.tag == "AI")
            {
                Destroy(other.gameObject);
            }
            
        }
		    
	}

	private IEnumerator destroySpike(Collider other) {
		if(!hasTriggered) {
			hasTriggered = true;
            rb.velocity = Vector3.zero;
			for(int i = 0; i < 2; i++)
				Destroy(gameObject.transform.GetChild(i).transform.gameObject);
            GetComponent<MeshRenderer>().enabled = false;
			explosion.Play();
			yield return new WaitForSeconds(timeToDeath);
			explosion.Stop();
			Destroy(gameObject);			
		}
	}

    public GameObject Owner
    {
        get { return owner; }
        set { owner = value; }
    }

}
