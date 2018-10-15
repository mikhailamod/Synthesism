using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSpillController : MonoBehaviour {

    public float oilLife;

    private GameObject owner;

    private void Start()
    {
        StartCoroutine(destroyOil());
    }

    void OnTriggerEnter(Collider other) {
		if((other.gameObject.tag=="Player" || other.gameObject.tag == "AI") && other.gameObject != owner) {
			other.gameObject.GetComponent<ActivatePickup>().StartSlip();
		}
	}

    private IEnumerator destroyOil()
    {
        yield return new WaitForSeconds(oilLife);
        Destroy(gameObject);
    }

    public GameObject Owner
    {
        get { return owner; }
        set { owner = value; }
    }
}
