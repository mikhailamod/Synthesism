using UnityEngine;
using TMPro;
public class CarSelector : MonoBehaviour {

    public GameObject slowCar;
    public GameObject fastCar;
    public TextMeshProUGUI selectText;
    public TextMeshProUGUI infoText;

    public string sportsInfo = "A fast car but difficult to handle";
    public string muscleInfo = "An easy car to handle but not the fastest";

    public float liftAmount = 0.5f;

    bool fastCarSelected = true;

    private void Start()
    {
        Vector3 v = fastCar.transform.position;
        v.y += liftAmount;
        fastCar.transform.position = v;
        selectText.text = "Sports Car Selected";
        infoText.text = sportsInfo;
    }

    // Update is called once per frame
    void Update () {
        if(Input.GetButtonDown("Horizontal"))
        {
            SwitchSelection();
        }
	}

    void SwitchSelection()
    {
        if (fastCarSelected)
        {
            fastCarSelected = false;
            LiftCar(slowCar, fastCar);
            selectText.text = "Muscle Car Selected";
            infoText.text = muscleInfo;
        }
        else
        {
            fastCarSelected = true;
            LiftCar(fastCar, slowCar);
            selectText.text = "Sports Car Selected";
            infoText.text = sportsInfo;
        }
    }

    void LiftCar(GameObject selctedCar, GameObject oldCar)
    {
        Vector3 v = selctedCar.transform.position;
        Vector3 oldV = oldCar.transform.position;

        v.y += liftAmount;
        selctedCar.transform.position = v;

        oldV.y -= liftAmount;
        oldCar.transform.position = oldV;
    }
}
