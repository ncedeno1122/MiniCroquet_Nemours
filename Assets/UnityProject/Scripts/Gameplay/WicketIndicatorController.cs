using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class WicketIndicatorController : MonoBehaviour
{
    [Range(0f, 40f)]
    public float LightRotationSpeed = 15f;

    public Light m_Spotlight;
    public Transform m_ArrowTF, m_LightTF;

    private IEnumerator MoveCRT;

    void Start()
    {
        m_Spotlight = GetComponentInChildren<Light>();
        m_LightTF = m_Spotlight.transform;
        foreach(Transform tf in transform)
        {
            m_ArrowTF = (tf != m_LightTF) ? tf : m_ArrowTF; 
        }
    }

    private void Update()
    {
        m_LightTF.Rotate(0f, 0f, LightRotationSpeed * Time.deltaTime);
    }

    // + + + + | Functions | + + + + 

    public void SetPosition(WicketGate currWicketGate)
    {
        // Set Position
        //transform.position = currWicketGate.CorrespondingController.transform.position;

        // Set Arrow Rotation
        //m_ArrowTF.rotation = Quaternion.Euler(0f, (currWicketGate.ReceivesFromFront? 180f : 0f), 0f);

        if (MoveCRT != null)
        {
            StopCoroutine(MoveCRT);
        }
        MoveCRT = MovePositionCRT(transform.position, currWicketGate.CorrespondingController.transform.position, 1f, currWicketGate.ReceivesFromFront);
        StartCoroutine(MoveCRT);
    }

    private IEnumerator MovePositionCRT(Vector3 from, Vector3 to, float timeSeconds, bool receivesFromFront)
    {
        for (float i = 0f; i < timeSeconds; i += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(from, to, i / timeSeconds);
            yield return new WaitForEndOfFrame();
        }
        transform.position = to;
        m_ArrowTF.rotation = Quaternion.Euler(0f, (receivesFromFront ? 180f : 0f), 0f);
    }
}
