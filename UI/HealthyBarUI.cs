using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthyBarUI : MonoBehaviour
{
    public GameObject healthUIPerfab;
    public Transform barPoint;

    Image healthSlider;
    Transform UIBar;
    Transform cam;

    public bool alwaysVisible;
    public float visibleTime;
    private float timeLeft;

    CharacterStates characterStats;

    void Awake()
    {
        characterStats = GetComponent<CharacterStates>();
        characterStats.UpDateHealthBarOnAttack += UpDateHealthBar;
    }

    void OnEnable()
    {
        cam = Camera.main.transform;

        foreach(Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if(canvas.renderMode == RenderMode.WorldSpace)
            {
                UIBar = Instantiate(healthUIPerfab, canvas.transform).transform;
                healthSlider = UIBar.GetChild(0).GetComponent<Image>();
                UIBar.gameObject.SetActive(alwaysVisible);
            }
        }
    }

    private void UpDateHealthBar(int currentHealth, int maxHealth)
    {
        if (currentHealth <= 0)
            Destroy(UIBar.gameObject);

        UIBar.gameObject.SetActive(true);
        timeLeft = visibleTime;

        float currentPersent = (float)currentHealth / maxHealth;
        healthSlider.fillAmount = currentPersent;
    }

    void LateUpdate()
    {
        if(UIBar != null)
        {
            UIBar.position = barPoint.position;
            UIBar.forward = -cam.forward;

            if (timeLeft <= 0 && !alwaysVisible)
            {
                UIBar.gameObject.SetActive(false);
            }
            else
            {
                timeLeft -= Time.deltaTime;
            }
        }
    }
}
