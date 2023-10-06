using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WaitUntilBallStopsState : CameraState
{

    private readonly Transform m_CamTransform;
    private readonly Transform m_BallTransform;
    private readonly Rigidbody m_BallRb;

    public WaitUntilBallStopsState(PlayerCameraController pcc, Transform ballTF) : base(pcc)
    {
        m_CamTransform = pcc.transform;
        m_BallTransform = ballTF;
        m_BallRb = m_BallTransform.GetComponent<Rigidbody>();
    }

    public override void OnEnter()
    {
        //
        m_Context.ShotPowerUI.Hide();
        m_Context.ShotAngleUI.Hide();

        m_CamTransform.position = Vector3.up * 40f;
        m_CamTransform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

    public override void OnExit()
    {
    }

    public override void OnLookIA(InputAction.CallbackContext ctx)
    {
    }

    public override void OnMainButtonIA(InputAction.CallbackContext ctx)
    {
    }

    public override void OnSecondaryButtonIA(InputAction.CallbackContext ctx)
    {
    }

    protected override void EarlyUpdate()
    {
        
    }

    protected override void MidUpdate()
    {
        if (m_BallRb.velocity.magnitude <= 0.05f && m_BallRb.velocity.magnitude != 0f)
        {
            // Advance Ball
            m_Context.MatchManager.AdvanceBallController();
            m_Context.ChangeState(new ShotLineupState(m_Context, m_Context.MatchManager.CurrBallController.transform));
        }
    }

    protected override void LateUpdate()
    {
    }
}
