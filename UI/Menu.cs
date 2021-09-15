using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class Menu : MonoBehaviour
{
    public GameObject menu;
    public AudioMixer audioMixer;

    public TextMeshProUGUI forest, rainForest, snowField;
    public Transform startPos;
    public Transform endPos,endPos_1;
    float time;

    public GameObject QuitPanel;
    public nashinanjue nashinanJue;

    public void OpenMenu()
    {
        menu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ClosedMenu()
    {
        menu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ClosedQuit()
    {
        Time.timeScale = 1f;

        nashinanJue.isEnd = false;
        
    }

    public void SetVolume(float value)
    {
        audioMixer.SetFloat("MainVolume",value);
    }

    private void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            forest.gameObject.SetActive(true);
            forest.transform.DOMove(endPos.position, 4f);
            forest.DOFade(0, 4f);
            Resume();
        }


        if (SceneManager.GetActiveScene().name == "RainScene")
        {
            rainForest.gameObject.SetActive(true);
            rainForest.transform.DOMove(endPos_1.position, 4f);
            rainForest.DOFade(0, 4f);
            Resume();
        }


        if (SceneManager.GetActiveScene().name == "SnowScene")
        {
            snowField.gameObject.SetActive(true);
            snowField.transform.DOMove(endPos_1.position, 4f);
            snowField.DOFade(0, 4f);
            Resume();
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainScene" && SceneManager.GetActiveScene().name == "RainScene"&& SceneManager.GetActiveScene().name == "SnowScene")
            time += Time.deltaTime;

        if (nashinanJue.isEnd == true)
        {
            SoundManager.instance.WinAudio();
            Time.timeScale = 0;
            QuitPanel.SetActive(true);
        }
    }

    public void Resume()
    {
        if (time >= 4)
        {
            forest.transform.DOMove(startPos.position, 0.01f);
            forest.DOFade(1, 0.01f);
            time = 0;
        }
    }

    public void Resumegame()
    {
        PlayerPrefs.DeleteAll();
        SceneController.Instance.TransitionToFirstLevel();
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }

}   
