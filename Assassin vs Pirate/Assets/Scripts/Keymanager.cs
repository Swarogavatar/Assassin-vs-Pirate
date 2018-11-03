//by Łukasz Nowak

// script holds keyboard combos logic single button mash will be added in another script

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//i kno public everything is public and all but i don't have time to write accessors and stuff :)
[RequireComponent(typeof(GameState))]
public class Keymanager : MonoBehaviour {

    //GameObject (in that case UI boxes) that will show:
    public GameObject Display;                      //what to press
    public GameObject Determiner;                  //if the player succeded

    //reference to a script holding progress and state of the game
    public GameState gamestate;

    //bool used to check if a new combo can be initialised
    private bool OnCooldown;

    //used to determine if player pressed the correct combo
    private int Result;
    private enum PossibleOutcomes {correct, incorrect, toolate, undetermined }; // just to avoid magic numbers

    //this int will be used to generate button wombas to press, DOES NOT include constant F mash 
    private int ComboGen;

    //time to press the key combination
    [SerializeField]
    private float TimeForCombos;

    private float RemainingTimeforCombos;

    [SerializeField]
    private int CorrectIncrease;

    [SerializeField]
    private int OutOfTimeDecrease;

	// Use this for initialization
	void Start () {
        //trauma z PROE wiem, ze moge to zrobic przy deklaracji ale nawet jak napisalem ten komentarz to dostaje flashbackow
        ComboGen = 0;
        OnCooldown = false;
	}

   
	// Update is called once per frame
	void Update () {

		if(!OnCooldown)     //if a previous isn't finished
        {
            ComboGen = Random.Range(1, 3); // random combo to press

            OnCooldown = true;

            Result = (int)PossibleOutcomes.undetermined; // just to be sure :)

            // Display text matching the generated combo
            switch (ComboGen)
            {
                case 0:
                    break;
                case 1:
                    Display.GetComponent<Text>().text = "|a + h|";
                    
                    //reset timer
                    RemainingTimeforCombos = TimeForCombos;
                    break;

                case 2:
                    Display.GetComponent<Text>().text = "|Ctrl + l|";

                    //reset timer
                    RemainingTimeforCombos = TimeForCombos;
                    break;

                default:
                    Debug.Log("WHAT THE FUCK?! "); //dont ask
                    break;

            }

        }

        //Decrease remaining time
        if (OnCooldown && RemainingTimeforCombos > 0)
        {
            RemainingTimeforCombos -= Time.deltaTime;
        }

        //If the timer reached 0 and played didn't press anything - do too late logic
        if (Result == (int)PossibleOutcomes.undetermined && (RemainingTimeforCombos <= 0))
        {
            Result = (int)PossibleOutcomes.toolate;
            StartCoroutine("KeyPress");
        }

        //if a player pressed any key excluding f
        if (Input.anyKey && !Input.GetKeyDown(KeyCode.F) && !Input.GetKey(KeyCode.F))
        {
            //check if pressed keys match
            switch (ComboGen)
            {
                case 0:
                    break;
                case 1:
                    //if player press the right combo fire correct logic
                    //this really should be a function but fuck it, maybe tomorrow
                    if (Input.GetKey(KeyCode.A) && Input.GetKeyDown(KeyCode.H) 
                        || Input.GetKeyDown(KeyCode.A) && Input.GetKey(KeyCode.H))
                    {
                        StopCoroutine("KeyPress");
                        Result = (int)PossibleOutcomes.correct;
                        StartCoroutine("KeyPress");
                        Debug.Log("Correct! Key: Ctrl -" );


                    }
                    //incorrect logic
                    break;
                case 2:
                    //if player press the right combo fire correct logic
                    if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.L)
                      || Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKey(KeyCode.L))
                    {
                        StopCoroutine("KeyPress");
                        Result = (int)PossibleOutcomes.correct;
                        StartCoroutine("KeyPress");
                        Debug.Log("Correct");

                    }
                    //incorrect logic
                    break;
                default:
                    Debug.Log("y u do dis"); // this is fine
                    break;
            }

            
        }
    }

    //coroutine responsible for displaying according message based on player action
    //also progress bar value is affected form here
    IEnumerator KeyPress()
    {
        ComboGen = 0;
        switch(Result)
        {
            case (int)PossibleOutcomes.correct:
                Determiner.GetComponent<Text>().text = "Correct!";
                gamestate.IncreaseFromFmash(CorrectIncrease);
                break;

            case (int)PossibleOutcomes.incorrect:
                Determiner.GetComponent<Text>().text = "Incorrect!";
                gamestate.IncreaseFromFmash(-CorrectIncrease);
                break;

            case (int)PossibleOutcomes.toolate:
                Determiner.GetComponent<Text>().text = "Too late!";
                gamestate.IncreaseFromFmash(-OutOfTimeDecrease);
                break;
            case (int)PossibleOutcomes.undetermined:
                break;
            default:
                Debug.Log("Should have used if motherfucka");
                break;
        }
        yield return new WaitForSeconds(seconds: RemainingTimeforCombos + 2f);
        Display.GetComponent<Text>().text = "";
        Determiner.GetComponent<Text>().text = "";
        OnCooldown = false;
    }
}
