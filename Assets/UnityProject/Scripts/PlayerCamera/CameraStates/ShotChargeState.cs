using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShotChargeState : CameraState
{
    private bool m_WasSelectPressed;
    
    private float m_TimerHelper = 0f;
    private const float INITIAL_DEBOUNCE_TIME = 0.25f;
    private const float MAXIMUM_CHARGE_TIME = 2f;

    private const float SHOTANGLE_DELTA = 0.35f;
    private const float SHOTANGLELIMIT_LAUNCH = 60f;
    private const float SHOTPOWER_MIN = 50f;
    private const float SHOTPOWER_MAX = 650f;
    private Vector2 m_CurrentShotAngle = Vector2.zero;

        
    private readonly Transform m_CamTransform;
    private readonly Transform m_BallTransform;

    public ShotChargeState(PlayerCameraController pcc, Transform ballTF) : base(pcc)
    {
        m_CamTransform = pcc.transform;
        m_BallTransform = ballTF;
    }

    public override void OnMainButtonIA(InputAction.CallbackContext ctx)
    {
        //
    }

    public override void OnLookIA(InputAction.CallbackContext ctx)
    {
        //
        
        //Vector2 v2Input = ctx.ReadValue<Vector2>();
        //Debug.Log($"Mouse input is {v2Input}");
        // Update ShotAngleUI with m_InitialMousePositionScreen
        //if (ctx.performed && !ctx.canceled)
        //{
        //    m_InitialMousePositionScreen = (m_InitialMousePositionScreen + v2Input).normalized;
        //    //m_InitialMousePositionScreen = (m_InitialMousePositionScreen + v2Input).normalized;
        //    m_Context.ShotAngleUI.ShowAngle(m_InitialMousePositionScreen);
        //}
    }

    public override void OnSecondaryButtonIA(InputAction.CallbackContext ctx)
    {
        //
    }

    public override void OnEnter()
    {
        //
        m_Context.ShotPowerUI.Show();
        m_Context.ShotAngleUI.Show();
    }

    public override void OnExit()
    {
        //
        m_Context.ShotPowerUI.Hide();
        m_Context.ShotAngleUI.Hide();
    }

    protected override void EarlyUpdate()
    {
        // Poll for MainButtonIA Input
        if (m_Context.MainButtonIA.IsPressed() && !m_WasSelectPressed)
        {
            m_WasSelectPressed = true;
        }

        // Poll for ShotAngleUI
        //Debug.Log($"LookIA | type: {m_Context.LookIA.type} | inprogress: {m_Context.LookIA.inProgress } | phase: {m_Context.LookIA.phase}");
        if (m_Context.LookIA.inProgress && m_Context.LookIA.phase == InputActionPhase.Started)
        {
            //
            Vector2 lookInputV2 = m_Context.LookIA.ReadValue<Vector2>();
            m_CurrentShotAngle += lookInputV2 * (SHOTANGLE_DELTA * Time.deltaTime);
            m_CurrentShotAngle = (m_CurrentShotAngle.magnitude > 1f) ? m_CurrentShotAngle.normalized : m_CurrentShotAngle;
            //m_CurrentShotAngle = Vector2.ClampMagnitude(m_CurrentShotAngle, 1f);

            // BallController Gizmo Display
            //BallController ballController = m_BallTransform.GetComponent<BallController>();
            //ballController.xzSpin = m_CurrentShotAngle.x * SHOTANGLELIMIT_LAUNCH + 90f;
            //ballController.ySpin = m_CurrentShotAngle.y * SHOTANGLELIMIT_LAUNCH * -1f;

            // TODO: Keep track of current angle & add lookInputV2
            m_Context.ShotAngleUI.ShowAngle(m_CurrentShotAngle);
            //Debug.Log(m_CurrentShotAngle);
        }
        
        // Revert state if MainButtonIA is released prior to debounce time
        if (!m_Context.MainButtonIA.IsPressed() && m_WasSelectPressed && m_TimerHelper <= INITIAL_DEBOUNCE_TIME)
        {
            m_Context.ChangeState(new ShotLineupState(m_Context, m_BallTransform));
        }
        
        // Launch ball with certain amount of power if MainButtonIA is released AFTER debounce time
        // TODO: AnimationCurve + ScriptableObject for different club/mallet types' powers?
        if (!m_Context.MainButtonIA.IsPressed() && m_WasSelectPressed && m_TimerHelper > INITIAL_DEBOUNCE_TIME)
        {
            //Debug.Log($"Shot launched with power of {CalculatePower(m_TimerHelper)}");
            
            Rigidbody rb = m_BallTransform.GetComponent<Rigidbody>(); // TODO: Why not use BallController access here?

            Vector3 launchForce = m_CamTransform.TransformDirection(new Vector3(
                m_BallTransform.position.x + Mathf.Cos((m_CurrentShotAngle.x * SHOTANGLELIMIT_LAUNCH + 90f) * Mathf.Deg2Rad) * 1.25f,
                m_BallTransform.position.y + Mathf.Sin((m_CurrentShotAngle.y * SHOTANGLELIMIT_LAUNCH * -1f) * Mathf.Deg2Rad) * 1.25f,
                m_BallTransform.position.z + Mathf.Sin((m_CurrentShotAngle.x * SHOTANGLELIMIT_LAUNCH + 90f) * Mathf.Deg2Rad) * 1.25f) - m_BallTransform.position);

            rb.AddForce(launchForce * CalculatePower(m_TimerHelper));

            //m_Context.ChangeState(new ShotLineupState(m_Context, m_BallTransform)); // TODO: Go to state to follow ball!
            m_Context.ChangeState(new WaitUntilBallStopsState(m_Context, m_BallTransform));
        }
    }

    protected override void MidUpdate()
    {
        //
    }

    protected override void LateUpdate()
    {
        // Increment TimeHelper
        m_TimerHelper += Time.deltaTime;
        
        // Update ShotPowerUI
        m_Context.ShotPowerUI.SetFillAmount(((m_TimerHelper - INITIAL_DEBOUNCE_TIME) / MAXIMUM_CHARGE_TIME)); // TODO: Better helper function for this?

        // Update ShotAngleUI
        //m_Context.ShotAngleUI.ShowAngle(lookInputV2);

        // Orbit Camera around Target 
        float percentageLookingDown = Mathf.Abs(CurrentRotationEulers.y) / UPPER_LOOKLIMIT_DEG;
        m_CamTransform.position = (m_BallTransform.position) +
                                  new Vector3(
                                      Mathf.Cos(CurrentRotationEulers.x * Mathf.Deg2Rad) * Mathf.SmoothStep(m_CamRadiusFromTarget, m_CamRadiusFromTarget * 0.3f, percentageLookingDown),
                                      Mathf.Sin(CurrentRotationEulers.y * Mathf.Deg2Rad) * Mathf.SmoothStep(0f, m_CamRadiusFromTarget, percentageLookingDown),
                                      Mathf.Sin(CurrentRotationEulers.x * Mathf.Deg2Rad) * Mathf.SmoothStep(m_CamRadiusFromTarget, m_CamRadiusFromTarget * 0.3f, percentageLookingDown));
        m_CamTransform.LookAt(m_BallTransform);
    }

    private float CalculatePower(float time)
    {
        return time <= INITIAL_DEBOUNCE_TIME ? 0f : Mathf.SmoothStep(SHOTPOWER_MIN, SHOTPOWER_MAX, ((m_TimerHelper - INITIAL_DEBOUNCE_TIME) / MAXIMUM_CHARGE_TIME));
    }
}
