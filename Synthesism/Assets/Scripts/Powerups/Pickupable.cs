using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour {

    public int pickupID = 0;

    public float rotationSpeed = 2.0f; 

    private void Update()
    {
        transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" || other.tag == "AI")
        {
            other.GetComponent<ActivatePickup>().SetPowerUp(pickupID);
        }
        GameObject.FindGameObjectWithTag("Spawn Manager").GetComponent<SpawnManager>().SpawnNew(transform.position);
        Destroy(gameObject);
    }
}
