using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBarUI : MonoBehaviour
{
    public GameObject HealthBarUIPrefab;
    public Transform HealthBarPos;

    public bool alwaysVisible;

    public float visibleTime;
    private float visibleTimer;

    Image HealthSlider;
    Transform UIbar;
    Transform cam;

    CharacterStatus currentStatus;

    private void Awake()
    {
        currentStatus = GetComponent<CharacterStatus>();
        currentStatus.updateHealthBarOnAttack += UpdateHealthBar;
    }

    private void OnEnable()
    {
        cam = Camera.main.transform;

        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                UIbar = Instantiate(HealthBarUIPrefab, canvas.transform).transform;
                HealthSlider = UIbar.GetChild(0).GetComponent<Image>();
                UIbar.gameObject.SetActive(alwaysVisible);
            }
        }
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (currentHealth <= 0)
            Destroy(UIbar.gameObject);

        UIbar.gameObject.SetActive(true);
        visibleTimer = visibleTime;

        // update health bar
        float sliderPercent = (float)currentHealth / maxHealth;
        // HealthSlider.fillAmount = sliderPercent;
        HealthSlider.DOFillAmount(sliderPercent, 0.3f);
    }

    // Update is called once per frame, after Update
    private void LateUpdate()
    {
        if (UIbar != null)
        {
            UIbar.position = HealthBarPos.position;
            UIbar.forward = -cam.forward;

            if (visibleTimer <= 0 && !alwaysVisible)
                UIbar.gameObject.SetActive(false);
            else
                visibleTimer -= Time.deltaTime;
        }
    }
}
