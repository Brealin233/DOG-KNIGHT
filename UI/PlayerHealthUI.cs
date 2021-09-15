using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    Text levelText;
    Image healthSlider;
    Image expSlider;

    private void Awake()
    {
        levelText = transform.GetChild(2).GetComponent<Text>();
        healthSlider = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        expSlider = transform.GetChild(1).GetChild(0).GetComponent<Image>();
    }

    private void Update()
    {
        levelText.text = "Level " + GameManager.Instance.playStats.characterDate.currentLevel.ToString("00");
        UpDateHealth();
        UpDateExp();
    }

    void UpDateHealth()
    {
        float sliderPersent = (float)GameManager.Instance.playStats.CurrentHealth / GameManager.Instance.playStats.MaxHealth;
        healthSlider.fillAmount = sliderPersent;
    }

    void UpDateExp()
    {
        float sliderPersent = (float)GameManager.Instance.playStats.characterDate.currentExp / GameManager.Instance.playStats.characterDate.baseExp; 
        expSlider.fillAmount = sliderPersent;
    }
}