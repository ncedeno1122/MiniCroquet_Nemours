using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShotAngleUIController : MonoBehaviour
{
    private const float BALLANGLEIMAGE_RADIUS = 20f; 
    
    private CanvasGroup m_CanvasGroup;
    private Image m_BallBGImage, m_BallAngleImage;
    private void Awake()
    {
        m_CanvasGroup = GetComponent<CanvasGroup>();
        m_BallBGImage = transform.GetChild(0).GetComponent<Image>();
        m_BallAngleImage = m_BallBGImage.transform.GetChild(0).GetComponent<Image>();
    }

    public void ShowAngle(Vector2 ballAngle)
    {
        // Takes a normalized vector and positions the BallAngleImage accordingly
        m_BallAngleImage.transform.localPosition = m_BallBGImage.transform.localPosition + ((Vector3) ballAngle * BALLANGLEIMAGE_RADIUS);
    }
    
    public void Show()
    {
        ResetImage();
        // TODO: Smoothed Animation to fade in?
        m_CanvasGroup.alpha = 1f;
    }

    public void Hide()
    {
        m_CanvasGroup.alpha = 0f;
    }

    public void ResetImage()
    {
        m_BallAngleImage.transform.localPosition = m_BallBGImage.transform.localPosition;
    }
}
