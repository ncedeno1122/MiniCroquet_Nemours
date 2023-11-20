using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WaitUntilBallStopsState : CameraState
{
    private const float CAM_LERP_SCALAR = 0.125f;
    private const float VELOCITY_CHECK_TIME = 0.2f; // Check Velocity after ___ seconds!
    private float velocityCheckTimeHelper = 0f;
    private readonly Vector3 m_BirdsEyeOffset = Vector3.up * 10f;
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
        // Smoothstep to position above the ball
        m_CamTransform.position = Vector3.Lerp(m_CamTransform.position, m_BallTransform.position + m_BirdsEyeOffset, CAM_LERP_SCALAR);
        m_CamTransform.rotation = Quaternion.Lerp(m_CamTransform.rotation, Quaternion.Euler(Vector3.right * 90f), CAM_LERP_SCALAR); // TODO: Creates jittering, not quite a fan... Might need to just lock rotation...
    }

    protected override void MidUpdate()
    {
        if (velocityCheckTimeHelper >= VELOCITY_CHECK_TIME && m_BallRb.velocity.magnitude <= 0.05f && m_BallRb.velocity.magnitude != 0f)
        {
            // Advance Ball
            m_Context.MatchManager.AdvanceBallController();
            m_Context.ChangeState(new ShotLineupState(m_Context, m_Context.MatchManager.CurrBallController.transform));
        }
    }

    protected override void LateUpdate()
    {
        // Velocity Check
        if (velocityCheckTimeHelper < VELOCITY_CHECK_TIME)
        {
            velocityCheckTimeHelper += Time.deltaTime;
        }
    }
}
