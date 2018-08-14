
using UnityEngine;

public abstract class CarController : MonoBehaviour
{

    protected abstract void MoveVehicle();
    public abstract float getCurrentSpeed();
    public abstract float getRpm();
}
