//by Łukasz Nowak

//this script contains progress bar as well as winning and loosing logic
//the progress bar value is an int between 0 and 100, with the staring value of 50

//changes made immediately afer enthering ez mode are in PlayerEnteredEasyMode()

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class GameState : MonoBehaviour {

    Camera camera;

    //reference to the slider on canvas
    public Slider ProgressBar;

    public Slider EzModeBar;
    //  ( ͡° ͜ʖ ͡°) yep   this clip 
    public AudioSource icrievritim;

    //references to other scripts
    public Keymanager keymanager;
    public Fmash fmash;

    private bool EnableEzMode;
    private bool EasyMode;

    [SerializeField]
    private float TimeForComboEzMode;
    //starting progress bar value
    [SerializeField]
    private int StartValue;

    //holds number the progress bar value is decreased by every second
    [SerializeField]
    private float LossPerSecond;

    //bool that prevents firing gameloss logic every frame ;)
    private bool AlreadyQueued;

    // Use this for initialization
    void Start () {
        ProgressBar.value = StartValue;
        EasyMode = false;
        AlreadyQueued = false;
        camera = Camera.main;
        StartCoroutine("DecreaseProgressBar"); // decreases progress bar value every second
        StartCoroutine("CheckForEzMode");
    }
	
	// Update is called once per frame
	void Update () {

        if(PlayerWantsEzMode())
        {
            EasyMode = true;
            PlayerEnteredEasyMode();
        }

        //losing logic
        if(ProgressBar.value <= 0 && !AlreadyQueued)
        {
            AlreadyQueued = true;
            Debug.Log("Ram pam pam you just lost an arm");
            StopAllCoroutines();
            icrievritim.Play(0);
            Debug.Log("Queue sound of silence");
        }

        //wining logic
        if(ProgressBar.value >= 100)
        {
            Debug.Log("Was it worth it?");
            StopAllCoroutines();
        }
	}

    //function that increased progress bar value by the given amount (can be negative)
    public void IncreaseFromFmash(int increase)
    {
        ProgressBar.value += increase;
        Mathf.Clamp(ProgressBar.value, 0, 100);
        Debug.Log("Value was increased by " + increase);
    }

    //checking if player pressed keys for ez mode - !!!!NEEDS TO BE CHANGED TO SMF ELSE BEFORE BUILD!!!!
    private bool PlayerWantsEzMode()
    {
        if (Input.GetKey(KeyCode.A))
        {
            //50% code
            Debug.Log("50%");
            if (Input.GetKey(KeyCode.Keypad9))
            {
                //100% code
                Debug.Log("Ez mode!!!");
                return true;
            }
  
        }
        return false;
    }

    private void PlayerEnteredEasyMode()
    {
        LossPerSecond = 2;
        keymanager.ChangeTimetoEzMode(TimeForComboEzMode);
        camera.backgroundColor = new Color(255, 0, 255);
    }

    //coroutine that decreases progress bar value by LossperSecond variable every second
    IEnumerator DecreaseProgressBar()
    {
        for(; ; )
        {
            ProgressBar.value -= LossPerSecond;
            //Debug.Log("Current value: " + ProgressBar.value);
            yield return new WaitForSeconds(1.0f);
        }        
    }

    IEnumerator CheckForEzMode()
    {
        for (; ; )
        {
            if (Input.GetKey(KeyCode.E))
            {
                //20% code
                Debug.Log("E");
                if (Input.GetKey(KeyCode.R))
                {
                    //40% code
                    Debug.Log("ER");
                    if (Input.GetKey(KeyCode.T))
                    {
                        //60% code
                        Debug.Log("ERT");
                        if (Input.GetKey(KeyCode.U))
                        {
                            //80% code
                            Debug.Log("ERTU");
                            if (Input.GetKeyDown(KeyCode.O))
                            {
                                //100%
                                Debug.Log("ERTUO");
                                Debug.Log("It is done");
                                EnableEzMode = true;
                            }
                        }
                    }
                }
            }
            yield return null;
        }
    }
}
