using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public GameObject levelSelect;

    public GameObject settingsMenu;

    public Toggle soundToggle;

    public GameObject menuBackgroundSound;

    public GameObject[] lockedLevels;


    // Start is called before the first frame update
    void Start()
    {
        //levelSelect.SetActive(false);
        settingsMenu.SetActive(false);
        for(int i=2; i<=PlayerPrefs.GetInt("Completed Level", 1)+2; i++)
        {
            lockedLevels[i-2].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(soundToggle.isOn)
        {
            PlayerPrefs.SetInt("Sound", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Sound", 0);
        }

        if(PlayerPrefs.GetInt("Sound", 0) == 0)
        {
            menuBackgroundSound.GetComponent<AudioSource>().mute = true;
        }
        else
        {
            menuBackgroundSound.GetComponent<AudioSource>().mute = false;
        }
    }

    public void SettingsClick()
    {
        levelSelect.SetActive(false);
        settingsMenu.SetActive(true);
    }
    
    public void LeaveAccount()
    {
        PlayerPrefs.DeleteKey("Account");
        SceneManager.LoadScene(0);
    }
    
    public void Close()
    {
        levelSelect.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void NewGame()
    {
        levelSelect.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void Level1()
    {
        PlayerPrefs.SetInt("Current Level", 1);
        SceneManager.LoadScene(1);
    }

    public void Level2()
    {
        PlayerPrefs.SetInt("Current Level", 2);
        SceneManager.LoadScene(2);
    }

    public void Level3()
    {
        PlayerPrefs.SetInt("Current Level", 3);
        SceneManager.LoadScene(3);
    }

    public void Level4()
    {
        PlayerPrefs.SetInt("Current Level", 4);
        SceneManager.LoadScene(4);
    }

    public void Level5()
    {
        PlayerPrefs.SetInt("Current Level", 5);
        SceneManager.LoadScene(5);
    }

    public void Level6()
    {
        PlayerPrefs.SetInt("Current Level", 6);
        SceneManager.LoadScene(6);
    }

    public void Level7()
    {
        PlayerPrefs.SetInt("Current Level", 7);
        SceneManager.LoadScene(7);
    }

    public void Level8()
    {
        PlayerPrefs.SetInt("Current Level", 8);
        SceneManager.LoadScene(8);
    }

    public void Level9()
    {
        PlayerPrefs.SetInt("Current Level", 9);
        SceneManager.LoadScene(9);
    }

    public void Level10()
    {
        PlayerPrefs.SetInt("Current Level", 10);
        SceneManager.LoadScene(10);
    }

    public void Level11()
    {
        PlayerPrefs.SetInt("Current Level", 11);
        SceneManager.LoadScene(11);
    }

    public void Level12()
    {
        PlayerPrefs.SetInt("Current Level", 12);
        SceneManager.LoadScene(12);
    }

    public void Level13()
    {
        PlayerPrefs.SetInt("Current Level", 13);
        SceneManager.LoadScene(13);
    }

    public void Level14()
    {
        PlayerPrefs.SetInt("Current Level", 14);
        SceneManager.LoadScene(14);
    }

    public void Level15()
    {
        PlayerPrefs.SetInt("Current Level", 15);
        SceneManager.LoadScene(15);
    }

    public void Level16()
    {
        PlayerPrefs.SetInt("Current Level", 16);
        SceneManager.LoadScene(16);
    }

    public void Level17()
    {
        PlayerPrefs.SetInt("Current Level", 17);
        SceneManager.LoadScene(17);
    }

    public void Level18()
    {
        PlayerPrefs.SetInt("Current Level", 18);
        SceneManager.LoadScene(18);
    }

    public void Level19()
    {
        PlayerPrefs.SetInt("Current Level", 19);
        SceneManager.LoadScene(19);
    }

    public void Level20()
    {
        PlayerPrefs.SetInt("Current Level", 20);
        SceneManager.LoadScene(20);
    }


    public void Level21()
    {
        PlayerPrefs.SetInt("Current Level", 21);
        SceneManager.LoadScene(21);
    }

    public void Level22()
    {
        PlayerPrefs.SetInt("Current Level", 22);
        SceneManager.LoadScene(22);
    }

    public void Level23()
    {
        PlayerPrefs.SetInt("Current Level", 23);
        SceneManager.LoadScene(23);
    }

    public void Level24()
    {
        PlayerPrefs.SetInt("Current Level", 24);
        SceneManager.LoadScene(24);
    }

    public void Level25()
    {
        PlayerPrefs.SetInt("Current Level", 25);
        SceneManager.LoadScene(25);
    }

    public void Level26()
    {
        PlayerPrefs.SetInt("Current Level", 26);
        SceneManager.LoadScene(26);
    }

    public void Level27()
    {
        PlayerPrefs.SetInt("Current Level", 27);
        SceneManager.LoadScene(27);
    }

    public void Level28()
    {
        PlayerPrefs.SetInt("Current Level", 28);
        SceneManager.LoadScene(28);
    }

    public void Level29()
    {
        PlayerPrefs.SetInt("Current Level", 29);
        SceneManager.LoadScene(29);
    }

    public void Level30()
    {
        PlayerPrefs.SetInt("Current Level", 30);
        SceneManager.LoadScene(30);
    }

}
