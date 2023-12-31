using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A CameraState in which the player can line up a shot, then launch a given ball!
/// </summary>
public class ShotLineupState : CameraState
{
    private const float UPPER_LOOKLIMIT_DEG = 88f;  // 90f causes snaps with transform.LookAt()
    private const float LOWER_LOOKLIMIT_DEG = -15f;
    private const float CAM_ORBIT_SPEED = 25f;
    private float m_CamRadiusFromTarget = 10f;
    private bool m_IsSecondaryButtonHeld = false;
    private Vector2 m_MouseDeltaVec2 = Vector2.zero;
    private Vector2 m_CurrentRotationEulers = Vector2.zero;
    private readonly Transform m_CamTransform;
    private readonly Transform m_BallTransform;

    public ShotLineupState(PlayerCameraController pcc, Transform ballTF) : base(pcc)
    {
        m_CamTransform = pcc.transform;
        m_BallTransform = ballTF;
    }

    public override void OnLookIA(InputAction.CallbackContext ctx)
    {
        // Set MouseDeltaVec2
        if (ctx.performed & m_IsSecondaryButtonHeld)
        {
            m_MouseDeltaVec2 = ctx.ReadValue<Vector2>(); // TODO: Rid of field, use only as local var?
            m_Context.CurrentRotationEulers += m_MouseDeltaVec2 * (-1f * (CAM_ORBIT_SPEED * Time.deltaTime));

            // Clamp as Eulers
            m_Context.CurrentRotationEulers = new Vector2(
                Mathf.Repeat(m_Context.CurrentRotationEulers.x, 360f), // Clamp as Euler Angle
                Mathf.Clamp(m_Context.CurrentRotationEulers.y, LOWER_LOOKLIMIT_DEG, UPPER_LOOKLIMIT_DEG)); // Clamp to reduce Sin
            //Debug.Log($"CurrentRotationEulers are {m_Context.CurrentRotationEulers}");
            
        }
        else if (ctx.canceled)
        {
            m_MouseDeltaVec2 = Vector2.zero;
        }
    }

    public override void OnMainButtonIA(InputAction.CallbackContext ctx)
    {
        // Switch to ShotChargeState if Main Button is performed
        if (ctx.performed)
        {
            m_Context.ChangeState(new ShotChargeState(m_Context, m_BallTransform));
        }
    }
    
    public override void OnSecondaryButtonIA(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            m_IsSecondaryButtonHeld = true;
        }
        else if (ctx.canceled)
        {
            m_IsSecondaryButtonHeld = false;
        }
    }

    public override void OnEnter()
    {
        //
        m_Context.ShotPowerUI.Hide();
    }

    public override void OnExit()
    {
        //
    }

    protected override void EarlyUpdate()
    {
        //
    }

    protected override void MidUpdate()
    {
        //
    }

    protected override void LateUpdate()
    {
        float percentageLookingDown = Mathf.Abs(m_Context.CurrentRotationEulers.y) / UPPER_LOOKLIMIT_DEG;
        m_CamTransform.position = (m_BallTransform.position) +
                                  new Vector3(
                                      Mathf.Cos(m_Context.CurrentRotationEulers.x * Mathf.Deg2Rad) * Mathf.SmoothStep(m_CamRadiusFromTarget, m_CamRadiusFromTarget * 0.3f, percentageLookingDown),
                                      Mathf.Sin(m_Context.CurrentRotationEulers.y * Mathf.Deg2Rad) * Mathf.SmoothStep(0f, m_CamRadiusFromTarget, percentageLookingDown),
                                      Mathf.Sin(m_Context.CurrentRotationEulers.x * Mathf.Deg2Rad) * Mathf.SmoothStep(m_CamRadiusFromTarget, m_CamRadiusFromTarget * 0.3f, percentageLookingDown));
        m_CamTransform.LookAt(m_BallTransform);
    }
}
