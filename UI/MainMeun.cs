using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class MainMeun : MonoBehaviour
{
    Button newGameBtn;
    Button continueGameBtn;
    Button quitGameBtn;

    PlayableDirector director; 

    void Awake()
    {
        newGameBtn = transform.GetChild(1).GetComponent<Button>();
        continueGameBtn = transform.GetChild(2).GetComponent<Button>();
        quitGameBtn = transform.GetChild(3).GetComponent<Button>();

        newGameBtn.onClick.AddListener(PlayTimeLine);
        continueGameBtn.onClick.AddListener(ContinueGame);
        quitGameBtn.onClick.AddListener(QuitGame);

        director = FindObjectOfType<PlayableDirector>();
        director.stopped += NewGame;  
    }

    void PlayTimeLine()
    {
        director.Play();
    }

    void NewGame(PlayableDirector obj)
    {
        PlayerPrefs.DeleteAll();
        SceneController.Instance.TransitionToFirstLevel(); 
    }

    void ContinueGame()
    {
        SceneController.Instance.TransitionToLoadScene();
    }

    void QuitGame()
    {
        Application.Quit();
    }
}
