using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class CarSelector : MonoBehaviour {

    public GameObject slowCar;
    public GameObject fastCar;
    public TextMeshProUGUI selectText;
    public TextMeshProUGUI infoText;

    public string sportsInfo = "A fast car but difficult to handle";
    public string muscleInfo = "An easy car to handle but not the fastest";

    public float liftAmount = 0.5f;

    bool active = false;
    bool fastCarSelected = true;

    // Update is called once per frame
    void Update () {
        if(active)
        {
            if (Input.GetButtonDown("Horizontal"))
            {
                SwitchSelection();
            }
            if (Input.GetButtonDown("Handbrake"))
            {
                ConfirmSelection();
            }
        }
	}

    public void setToActive()
    {
        active = true;
        Vector3 v = fastCar.transform.position;
        v.y += liftAmount;
        fastCar.transform.position = v;
        selectText.text = "Sports Car Selected";
        infoText.text = sportsInfo;

        selectText.gameObject.SetActive(true);
        infoText.gameObject.SetActive(true);
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

    void ConfirmSelection()
    {
        if(fastCarSelected)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }
    }
}
