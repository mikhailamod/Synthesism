using UnityEngine;

public class BoostController : MonoBehaviour
{
    public Vector3 forceDir;
    public float boostSpeed;

    void OnTriggerEnter(Collider other)
    {
        //other.transform.forward
        //(forceDir - transform.position).normalized
        other.gameObject.GetComponent<Rigidbody>().AddForce(other.transform.forward * boostSpeed, ForceMode.Impulse);
    }


}
