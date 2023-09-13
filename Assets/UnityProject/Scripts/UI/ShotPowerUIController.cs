using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShotPowerUIController : MonoBehaviour
{
    private CanvasGroup m_CanvasGroup;
    private Image m_FilledPowerImage;

    [SerializeField] private Gradient m_FillGradient;

    private void Awake()
    {
        m_CanvasGroup = GetComponent<CanvasGroup>();
        m_FilledPowerImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
    }

    /// <summary>
    /// Sets the amount of FilledPowerImage
    /// </summary>
    /// <param name="normalizedValue"></param>
    public void SetFillAmount(float normalizedValue)
    {
        m_FilledPowerImage.fillAmount = normalizedValue;
        m_FilledPowerImage.color = m_FillGradient.Evaluate(normalizedValue);
    }

    public void Show()
    {
        // TODO: Smoothed Animation to fade in?
        m_CanvasGroup.alpha = 1f;
    }

    public void Hide()
    {
        m_CanvasGroup.alpha = 0f;
    }
}
