using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WicketGoalPanelController : MonoBehaviour
{
    private TextMeshProUGUI m_Text;
    public TextMeshProUGUI Text { get => m_Text; }

    private void Awake()
    {
        m_Text = GetComponentInChildren<TextMeshProUGUI>();
    }

    // + + + + | Functions | + + + + 


}
