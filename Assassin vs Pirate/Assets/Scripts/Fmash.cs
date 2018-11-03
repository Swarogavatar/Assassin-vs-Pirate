//by Łukasz Nowak

//script that contains only f mash logic

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GameState))]
public class Fmash : MonoBehaviour {

    public GameObject Button;
    //references to necessary stuff -> jest pozno, nie chce mi sie pisac porzadnych komentarzy :)
    public GameState gamestate;

    //serialized int that holds the number of times player has to press f to increase progress bar value
    [SerializeField]
    private int minPerSecond;

    //serialized int that holds max amount the progress bar value can be increased by mashing f in 1s
    [SerializeField]
    private int maxPerSecond;

    //holds number of times f was pressed this second - reset logic in FmashReset
    private int PressesThisSecond;

    // Use this for initialization
    void Start () {
        PressesThisSecond = 0;
        StartCoroutine("FmashReset");
	}

    // Update is called once per frame
    void Update() {

        //if player pressed f increase PressesThisSecond and clamp it. Also do stuff to the UI
        if (Input.GetKeyDown(KeyCode.F))
        {
            PressesThisSecond++;

            Mathf.Clamp(PressesThisSecond, 0, maxPerSecond);

            Button.GetComponent<Image>().color = Color.red;
        }
        //return button to normal state on button up
        if (Input.GetKeyUp(KeyCode.F))
        {
            Button.GetComponent<Image>().color = Color.white;
        }

	}

    //coroutine that handles reseting variables and calls increase progressbar value
    IEnumerator FmashReset()
    {
        for(; ; )
        {
           // Debug.Log("This second F was pressed " + PressesThisSecond + " times.");

            if(PressesThisSecond > minPerSecond)
            {
                gamestate.IncreaseFromFmash(PressesThisSecond);
            }
            PressesThisSecond = 0;
            yield return new WaitForSeconds(1.0f);
        }
    }
}
