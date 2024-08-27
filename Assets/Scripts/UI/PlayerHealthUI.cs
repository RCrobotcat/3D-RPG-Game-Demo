using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerHealthUI : MonoBehaviour
{
    Text levelText;
    Image healthSlider;
    Image expSlider;

    void Awake()
    {
        levelText = transform.GetChild(2).GetComponent<Text>();
        healthSlider = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        expSlider = transform.GetChild(1).GetChild(0).GetComponent<Image>();
    }

    void Update()
    {
        levelText.text = "Level: " + GameManager.Instance.playerStatus.characterData.currentLevel.ToString("00");
        updateHealthBar();
        updateExpBar();
    }

    void updateHealthBar()
    {
        float fillPercent = (float)GameManager.Instance.playerStatus.currentHealth / GameManager.Instance.playerStatus.maxHealth;
        // healthSlider.fillAmount = fillPercent;
        healthSlider.DOFillAmount(fillPercent, 0.3f);
    }

    void updateExpBar()
    {
        float fillPercent = (float)GameManager.Instance.playerStatus.characterData.currentExp / GameManager.Instance.playerStatus.characterData.baseExp;
        // expSlider.fillAmount = fillPercent;
        expSlider.DOFillAmount(fillPercent, 0.5f);
    }
}
