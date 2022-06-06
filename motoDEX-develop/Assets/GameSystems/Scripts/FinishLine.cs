using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishLine : MonoBehaviour
{

    public int place = 0;

    public Text placeText;

    public Text placeTime;

    public Text timeRunning;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Wheel")
        {
            GameObject.Find("Game Manager").GetComponent<GameManager>().gameFinished = true;
            place += 1;
            GameObject.Find("Game Manager").GetComponent<GameManager>().playerRanking = place;
            GameObject.Find("Game Manager").GetComponent<GameManager>().playerFinishTime = timeRunning.text;
             if(place < 4)
            {
                if(place == 1)
                {
                    placeText.text = place +"ST";
                }
                else if(place == 2)
                {
                    placeText.text = place +"ND";
                }
                else if(place == 3)
                {
                    placeText.text = place +"RD";
                }
                placeTime.text = timeRunning.text;
            }
        }
        else if(other.gameObject.tag == "AI Wheels")
        {
            place += 1;
            if(place < 4)
            {
                if(place == 1)
                {
                    placeText.text = place +"ST";
                }
                else if(place == 2)
                {
                    placeText.text = place +"ND";
                }
                else if(place == 3)
                {
                    placeText.text = place +"RD";
                }
                placeTime.text = timeRunning.text;
            }
        }
    }
}
