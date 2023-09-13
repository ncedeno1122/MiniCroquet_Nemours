using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShotChargeState : CameraState
{
    private bool m_WasSelectPressed; // Used to determine 
    
    private float m_TimerHelper = 0f;
    private const float INITIAL_DEBOUNCE_TIME = 0.25f;

    private const float MAXIMUM_CHARGE_TIME = 2f;
        
    private readonly Transform m_CamTransform;
    private readonly Transform m_BallTransform;

    public ShotChargeState(PlayerCameraController pcc, Transform ballTF) : base(pcc)
    {
        m_CamTransform = pcc.transform;
        m_BallTransform = ballTF;
    }

    public override void OnLookIA(InputAction.CallbackContext ctx)
    {
        //
    }

    public override void OnMainButtonIA(InputAction.CallbackContext ctx)
    {
        //
    }
    
    public override void OnSecondaryButtonIA(InputAction.CallbackContext ctx)
    {
        //
    }

    public override void OnEnter()
    {
        //
        m_Context.ShotPowerUI.Show();
    }

    public override void OnExit()
    {
        //
        m_Context.ShotPowerUI.Hide();
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
            
            m_Context.ChangeState(new ShotLineupState(m_Context, m_BallTransform)); // TODO: Go to state to follow ball!
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
    }

    private float CalculatePower(float time)
    {
        return time <= INITIAL_DEBOUNCE_TIME ? 0f : Mathf.SmoothStep(50f, 500f, ((m_TimerHelper - INITIAL_DEBOUNCE_TIME) / MAXIMUM_CHARGE_TIME));
    }
}
