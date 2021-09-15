using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : Singleton<SaveManager>
{
    string sceneName = "";
    public string SceneName { get { return PlayerPrefs.GetString(sceneName); } }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneController.Instance.TransitionToEnter();
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            SavePlayerDate();
            InventoryManager.Instance.SaveDate();
            QuestManager.Instance.SaveQuestManager();
        }

        if(Input.GetKeyDown(KeyCode.X))
        {
            LoadPlayerDate();
            InventoryManager.Instance.LoadDate();
            QuestManager.Instance.LoadQuestManager();
        }
    }

    public void SavePlayerDate()
    {
        Save(GameManager.Instance.playStats.characterDate, GameManager.Instance.playStats.characterDate.name);
    }

    public void LoadPlayerDate()
    {
        Load(GameManager.Instance.playStats.characterDate, GameManager.Instance.playStats.characterDate.name);
    }

    public void Save(object date,string key)
    {
        var jsonDate = JsonUtility.ToJson(date,true);
        PlayerPrefs.SetString(key, jsonDate);
        PlayerPrefs.SetString(sceneName, SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
    }

    public void Load(object date,string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), date);
        }
    }
}
