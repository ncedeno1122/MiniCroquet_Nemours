using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinnerAnnouncementPanelController : MonoBehaviour
{
    private float AnimationTime = 1.5f;
    private IEnumerator m_AnimationCRT;
    private CanvasGroup m_CanvasGroup;
    private TextMeshProUGUI m_WinnerText;

    private void Awake()
    {
        m_CanvasGroup = GetComponent<CanvasGroup>();
        m_WinnerText = GetComponentInChildren<TextMeshProUGUI>();

        Hide();
    }

    public void Hide()
    {
        m_CanvasGroup.alpha = 0f;
    }

    public void Show()
    {
        m_CanvasGroup.alpha = 1f;
    }

    public void ShowWinnerUI(string winnerName)
    {
        m_WinnerText.text = $"Winner: {winnerName}!";

        if (m_AnimationCRT != null)
        {
            StopCoroutine(m_AnimationCRT);
            m_AnimationCRT = null;
        }
        m_AnimationCRT = ScoreFadeInAnimation(AnimationTime);
        StartCoroutine(m_AnimationCRT);
    }

    private IEnumerator ScoreFadeInAnimation(float totalTime)
    {
        // Fade in
        for (float i = 0f; i < totalTime / 8f; i += Time.deltaTime)
        {
            m_CanvasGroup.alpha = i / (totalTime / 8f);
            yield return new WaitForEndOfFrame();
        }

        // Hold
        yield return new WaitForSeconds(totalTime - (totalTime / 4f));

        // Fade out
        for (float i = 0f; i < totalTime / 8f; i += Time.deltaTime)
        {
            m_CanvasGroup.alpha = 1f - (i / (totalTime / 8f));
            yield return new WaitForEndOfFrame();
        }
    }
}
