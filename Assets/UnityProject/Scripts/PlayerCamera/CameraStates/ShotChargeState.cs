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
    
    private Vector2 m_InitialMousePositionScreen = Vector2.zero;
        
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
        Vector2 v2Input = ctx.ReadValue<Vector2>();
        Debug.Log($"Mouse input is {v2Input}");
        // Update ShotAngleUI with m_InitialMousePositionScreen
        if (ctx.performed)
        {
            
            //m_InitialMousePositionScreen = (m_InitialMousePositionScreen + v2Input).normalized;
            m_Context.ShotAngleUI.ShowAngle(m_InitialMousePositionScreen);
        }
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
        
        m_InitialMousePositionScreen = Vector2.zero; // TODO: Currently redundant
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
        
        // Revert state if MainButtonIA is released prior to debounce time
        if (!m_Context.MainButtonIA.IsPressed() && m_WasSelectPressed && m_TimerHelper <= INITIAL_DEBOUNCE_TIME)
        {
            m_Context.ChangeState(new ShotLineupState(m_Context, m_BallTransform));
        }
        
        // Launch ball with certain amount of power if MainButtonIA is released AFTER debounce time
        // TODO: AnimationCurve + ScriptableObject for different club/mallet types' powers?
        if (!m_Context.MainButtonIA.IsPressed() && m_WasSelectPressed && m_TimerHelper > INITIAL_DEBOUNCE_TIME)
        {
            Debug.Log($"Shot launched with power of {CalculatePower(m_TimerHelper)}");
            
            Rigidbody rb = m_BallTransform.GetComponent<Rigidbody>();
            rb.AddForce(m_CamTransform.TransformDirection(Vector3.forward) * CalculatePower(m_TimerHelper));

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
        return time <= INITIAL_DEBOUNCE_TIME ? 0f : Mathf.SmoothStep(50f, 500f, ((m_TimerHelper - INITIAL_DEBOUNCE_TIME) / MAXIMUM_CHARGE_TIME));
    }
}
