using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShotAngleUIController : MonoBehaviour
{
    private const float BALLANGLEIMAGE_RADIUS = 30f; 
    
    private CanvasGroup m_CanvasGroup;
    private Image m_BallBGImage, m_BallAngleImage;
    private void Awake()
    {
        m_CanvasGroup = GetComponent<CanvasGroup>();
    }

    public void ShowAngle(Vector2 ballAngle)
    {
        // Takes a normalized vector and positions the BallAngleImage accordingly
        m_BallAngleImage.transform.localPosition = m_BallAngleImage.transform.localPosition + ((Vector3) ballAngle * BALLANGLEIMAGE_RADIUS);
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
