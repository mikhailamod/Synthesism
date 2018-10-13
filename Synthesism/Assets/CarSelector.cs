using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class CarSelector : MonoBehaviour {

    public GameObject slowCar;
    public GameObject fastCar;
    public TextMeshProUGUI selectText;
    public TextMeshProUGUI infoText;

    Vector3 fastCarStartPos;
    Vector3 slowCarStartPos;

    public string sportsInfo = "A fast car but difficult to handle";
    public string muscleInfo = "An easy car to handle but not the fastest";

    public float liftAmount = 0.5f;

    bool active = false;
    int[] carSelections;

    int currentPlayer = 0;

    private void Start()
    {
        fastCarStartPos = fastCar.transform.position;
        slowCarStartPos = slowCar.transform.position;
    }

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

    public void setToActive(bool multiplayer)
    {
        if(multiplayer)
        {
            carSelections = new int[2];
            carSelections[0] = 1;
            carSelections[1] = 1;
        }
        else
        {
            carSelections = new int[1];
            carSelections[0] = 1;
        }
        active = true;
        Vector3 v = fastCar.transform.position;
        v.y += liftAmount;
        fastCar.transform.position = v;
        selectText.text = "Player " + (currentPlayer+1) + ": Sports Car Selected";
        infoText.text = sportsInfo;

        selectText.gameObject.SetActive(true);
        infoText.gameObject.SetActive(true);
    }

    void SwitchSelection()
    {
        if (carSelections[currentPlayer] == 1)
        {
            carSelections[currentPlayer] = 0;
            LiftCar(slowCar, fastCar);
            selectText.text = "Player " + (currentPlayer + 1) + ": Muscle Car Selected";
            infoText.text = muscleInfo;
        }
        else
        {
            carSelections[currentPlayer] = 1;
            LiftCar(fastCar, slowCar);
            selectText.text = "Player " + (currentPlayer + 1) + ": Sports Car Selected";
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
        //all players have chosen a car
        //tell the race manager which player chose which car 
        if (currentPlayer == carSelections.Length-1)
        {
            if(carSelections.Length == 1)//single player
            {
                PlayerPrefs.SetInt("P1_choice", carSelections[0]);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else//multiplayer
            {
                PlayerPrefs.SetInt("P1_choice", carSelections[0]);
                PlayerPrefs.SetInt("P2_choice", carSelections[0]);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
            }
        }
        else
        {
            //let player 2 choose
            currentPlayer++;
            slowCar.transform.position = slowCarStartPos;
            fastCar.transform.position = fastCarStartPos;

            Vector3 v = fastCar.transform.position;
            v.y += liftAmount;
            fastCar.transform.position = v;

            selectText.text = "Player " + (currentPlayer + 1) + ": Sports Car Selected";
            infoText.text = sportsInfo;
        }
    }
}
