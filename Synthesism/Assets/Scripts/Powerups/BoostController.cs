using UnityEngine;

public class BoostController : MonoBehaviour
{
    public Vector3 forceDir;
    public float boostSpeed;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" && other.tag != "AI") return;
        //other.transform.forward
        //(forceDir - transform.position).normalized
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        if(rb != null)
            rb.AddForce(other.transform.forward * boostSpeed, ForceMode.Impulse);
    }


}
