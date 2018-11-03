//by Łukasz Nowak

//this script contains progress bar as well as winning and loosing logic
//the progress bar value is an int between 0 and 100, with the staring value of 50

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class GameState : MonoBehaviour {

    //reference to the slider on canvas
    public Slider ProgressBar;

    //  ( ͡° ͜ʖ ͡°) yep   this clip 
    public AudioSource icrievritim;

    //starting progress bar value
    [SerializeField]
    private int StartValue;

    //holds number the progress bar value is decreased by every second
    [SerializeField]
    private float LossperSecond;

    //bool that prevents firing gameloss logic every frame ;)
    private bool AlreadyQueued;

    // Use this for initialization
    void Start () {
        ProgressBar.value = StartValue;
        AlreadyQueued = false;
        StartCoroutine("DecreaseProgressBar"); // decreases progress bar value every second
    }
	
	// Update is called once per frame
	void Update () {


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

    //coroutine that decreases progress bar value by LossperSecond variable every second
    IEnumerator DecreaseProgressBar()
    {
        for(; ; )
        {
            ProgressBar.value -= LossperSecond;
            Debug.Log("Current value: " + ProgressBar.value);
            yield return new WaitForSeconds(1.0f);
        }        
    }
}
