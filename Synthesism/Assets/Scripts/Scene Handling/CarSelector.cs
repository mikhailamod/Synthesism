using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

//This class handles the car selection of the player(s)
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
    int[] carSelections;//what each player has chosen. 0 = muscle car, 1 = fast car

    Button b;

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
            if (Input.GetButtonDown(ControllerInfo.HORIZONTAL_MOVES[0]) ||
                Input.GetButtonDown(ControllerInfo.HORIZONTAL_MOVES[1]))
            {
                SwitchSelection();
            }
            if (Input.GetButtonDown(ControllerInfo.HANDBRAKES[0]) ||
                Input.GetButtonDown(ControllerInfo.HANDBRAKES[1]))
            {
                ConfirmSelection();
            }
        }
	}

    //called by MenuController, this will initialize everything required by the Car Selection 'sub scene'
    //Note that the Car Selection 'sub scene' is within the same scene as the main menu, UI elements are
    //just set to disabled
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

        //default selection is the fast car
        Vector3 v = fastCar.transform.position;
        v.y += liftAmount;
        fastCar.transform.position = v;
        selectText.text = "Player " + (currentPlayer+1) + ": Sports Car Selected";
        infoText.text = sportsInfo;

        selectText.gameObject.SetActive(true);
        infoText.gameObject.SetActive(true);
    }

    //change the selection of the current player, and raise that car for visual feedback
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

    //Lift the selected car and lower the old car
    void LiftCar(GameObject selctedCar, GameObject oldCar)
    {
        Vector3 v = selctedCar.transform.position;
        Vector3 oldV = oldCar.transform.position;

        v.y += liftAmount;
        selctedCar.transform.position = v;

        oldV.y -= liftAmount;
        oldCar.transform.position = oldV;
    }

    //either allow player 2 to select a car, or save the selection of both players
    //and load next scene
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
