using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject player;
    public float time = 0f;
    public Text timeText;

    public float timeInMIn = 0f;

    public float timeInSec = 0f;

    public float timeInMS = 0f;

    public bool speeding = false;

    string min, sec, millisec; 

    public float temp = 0f;

    public Slider tempSlide;

    public bool tempisOn = false;

    public bool tempIsFull = false;

    public bool speedButtonPressed = false;

    public bool gameStart = false;

    public bool timeStart = false;

    public bool gameFinished = false;

    public Text startTime;

    public float timeForStart = 3f;

    public GameObject pauseMenu;

    public GameObject gameOver;

    public GameObject nextLevelButton;

    public GameObject nextLevelButtonShadow;

    public int playerRanking;

    public string playerFinishTime;

    public Text playerRankingText;

    public Text playerFinishTimeText;

    public GameObject startingClip;

    public GameObject bikeMoving;

    public GameObject tempFull;

    public GameObject fallSound;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        timeText.text = "00:00";

        StartCoroutine("StarttheGame");
        Invoke("TimerStartAction", 4f);
        pauseMenu.SetActive(false);
        gameOver.SetActive(false);
        Time.timeScale = 1;
        Invoke("StartingTimerSound", 1f);
        startingClip.SetActive(false);
        bikeMoving.SetActive(false);
        tempFull.SetActive(false);
        fallSound.SetActive(false);
        
        if(PlayerPrefs.GetInt("Sound") == 1)
        {
            startingClip.GetComponent<AudioSource>().mute = false;
            bikeMoving.GetComponent<AudioSource>().mute = false;
            tempFull.GetComponent<AudioSource>().mute = false;
            fallSound.GetComponent<AudioSource>().mute = false;
        }
        else
        {
            startingClip.GetComponent<AudioSource>().mute = true;
            bikeMoving.GetComponent<AudioSource>().mute = true;
            tempFull.GetComponent<AudioSource>().mute = true;
            fallSound.GetComponent<AudioSource>().mute = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Timer();
        Temperature();
        PlayerMovement();
        StartLineTimer();
        SoundControl();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            speedButtonPressed = true;
        }
        else if(Input.GetKeyUp(KeyCode.Space))
        {
            speedButtonPressed = false;
        }

        // Up Down Arrow
        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)){
            if(Input.GetKeyDown(KeyCode.UpArrow)){
                TurnLeft();
            }
            if(Input.GetKeyDown(KeyCode.DownArrow)){
                TurnRight();
            }
        }
        else if(Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow)){
            Drive();
        }


        if(gameFinished)
        {
            GameComplete();
        }
        
    }

    public void SoundControl()
    {
        
        if(timeStart == false)
        {
        }
        else
        {
            if(tempIsFull && gameStart)
            {
                startingClip.SetActive(false);
                bikeMoving.SetActive(false);
                tempFull.SetActive(true);
                fallSound.SetActive(false);
            }
            else if(tempIsFull == false && speeding && GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().hasFallenDown == false && gameStart)
            {
                startingClip.SetActive(false);
                bikeMoving.SetActive(true);
                tempFull.SetActive(false);
                fallSound.SetActive(false);
                
            }
            else if(tempIsFull == false && speeding && GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().hasFallenDown == true && gameStart)
            {
                startingClip.SetActive(false);
                bikeMoving.SetActive(false);
                tempFull.SetActive(false);
                fallSound.SetActive(true);
            }
        }
    }

    public void StartingTimerSound()
    {
        startingClip.SetActive(true);
        bikeMoving.SetActive(false);
        tempFull.SetActive(false);
        fallSound.SetActive(false);
    }

    public void TimerStartAction()
    {
        timeStart = true;
    }

    public void StartLineTimer()
    {
        if(timeForStart > 0)
        {
            timeForStart -= Time.deltaTime;
            startTime.text = Mathf.Round(timeForStart) + "";
            
        }
        else
        {
            startTime.text = "";
        }
    }

    public void Timer()
    {

        if(timeStart)
        {
            time += Time.deltaTime;

            timeInMIn = Mathf.Round(time/60);

            timeInSec = Mathf.Round(time%60);

            timeInMS = Mathf.Round((time*1000)%100);       

            if(timeInMIn < 10)
            {
                min = "0" + timeInMIn;
            }
            else
            {
                min = timeInMIn + "";
            }
            
            if(timeInSec < 10)
            {
                sec = "0" + timeInSec;
            }
            else
            {
                sec = timeInSec + "";
            }

            //if(timeInMS < 50)
            {
                millisec = timeInMS +"";
            }

            //timeText.text = min + ":" + sec + ":" + millisec;
            timeText.text = min + ":" + sec ;
        }
    }

    public void Temperature()
    {
        if(GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>().gameStart)
        {
            tempSlide.value = temp;

            if(temp > 0.99f)
            {
                tempIsFull = true;
            }
            else
            {
                tempIsFull = false;
            }

            if(tempisOn)
            {
                if(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().hasFallenDown == false || !gameFinished)
                {
                    if(temp <= 1)
                    {
                        temp += 0.001f;
                    }
                }
            }
            else
            {   if(temp >= 0)
                {
                    temp -= 0.01f;
                }
            }
        }
    }

    public void PlayerMovement()
    {
        if(speedButtonPressed)
        {
            if(temp < 1.2f)
            {   
                if(tempIsFull)
                {
                    speeding = false;
                }
                else
                {
                    speeding = true;
                }
            }   

            tempisOn = true;
        }
        else
        {
            speeding = false;

             tempisOn = false;

             bikeMoving.SetActive(false);
             tempFull.SetActive(false);
        }
    }

    public void SpeedControl()
    {

        speedButtonPressed = true;
        
    }

    public void SpeedLose()
    {

        speedButtonPressed = false;
        
    }


    public IEnumerator StarttheGame()
    {
        yield return new WaitForSeconds(4f);
        gameStart = true;
    }

     public void TurnLeft()
    {
        
        if(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().speed > 0f)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().leftPressed  = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().rightPressed = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().animator.SetBool("turnLeft", true);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().animator.SetBool("turnRight", false);
        }
    }

    public void TurnRight()
    {
        
         if(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().speed > 0f)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().rightPressed = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().leftPressed = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().animator.SetBool("turnRight", true);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().animator.SetBool("turnLeft", false);
           
        }
    }

    public void Drive()
    {

        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().rightPressed = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().leftPressed = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().xDir = 0f;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().animator.SetBool("turnRight", false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().animator.SetBool("turnLeft", false);
    }

    public void GameComplete()
    {
        Invoke("GameCompleteCall", 3f);
    }

    public void GameCompleteCall()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(false);
        gameOver.SetActive(true);
        if(playerRanking == 1)
        {
            playerRankingText.text = playerRanking +"ST PLACE !!";
        }
        else if(playerRanking == 2)
        {
            playerRankingText.text = playerRanking +"ND PLACE !!";
        }
        else if(playerRanking == 3)
        {
            playerRankingText.text = playerRanking +"RD PLACE !!";
        }
        else
        {
            playerRankingText.text = playerRanking +"TH PLACE !!";
        }
        
        playerFinishTimeText.text = playerFinishTime;
        NextLevelButton();

    }

    public void Pause()
    {
        //GameObject.Find("Ad Control").GetComponent<Adcontrol>().ShowInterstitial();
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        gameOver.SetActive(false);

    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        gameOver.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        //GameObject.Find("Ad Control").GetComponent<Adcontrol>().ShowInterstitial();
        SceneManager.LoadScene(PlayerPrefs.GetInt("Current Level", 1));
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        //GameObject.Find("Ad Control").GetComponent<Adcontrol>().ShowInterstitial();
        SceneManager.LoadScene(0);
    }

    public void NextLevel()
    {
        //GameObject.Find("Ad Control").GetComponent<Adcontrol>().ShowInterstitial();
        if(PlayerPrefs.GetInt("Current Level", 0) < 30)
        {
            if(PlayerPrefs.GetInt("Current Level", 1) == PlayerPrefs.GetInt("Completed Level", 1))
            {
                PlayerPrefs.SetInt("Completed Level", PlayerPrefs.GetInt("Current Level", 1) + 1);
            }
            int curLevel = PlayerPrefs.GetInt("Current Level", 1) + 1;
            PlayerPrefs.SetInt("Current Level", curLevel);
            SceneManager.LoadScene(PlayerPrefs.GetInt("Current Level", 1));
            
        }
        else
        {
            gameOver.SetActive(true);
        }
    }

    public void NextLevelButton()
    {
        if(playerRanking > 3)
        {
            nextLevelButton.SetActive(false);
            nextLevelButtonShadow.SetActive(false);
        }
        else
        {
            nextLevelButton.SetActive(true);
            nextLevelButtonShadow.SetActive(true);
        }
    }
}
