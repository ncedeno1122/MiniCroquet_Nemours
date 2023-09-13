using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraController : MonoBehaviour
{
    #region Fields

    private float m_LookSpeed;

    public PlayerInput PlayerInputInstance;
    public InputAction LookIA, MainButtonIA, SecondaryButtonIA;
    public Vector2 CurrentRotationEulers = Vector2.zero;

    public CameraState CamState { get; private set; }

    public ShotPowerUIController ShotPowerUI;

    #endregion
    
    #region UnityMethods

    private void Start()
    {
        // Get InputActions
        LookIA = PlayerInputInstance.actions["Look"];
        MainButtonIA = PlayerInputInstance.actions["MainButton"];
        SecondaryButtonIA = PlayerInputInstance.actions["SecondaryButton"];
        
        // Set Initial State
        // TODO: RID OF THIS
        CamState = new ShotLineupState(this, GameObject.Find("CroquetBall").transform);
    }

    private void Update()
    {
        CamState.OnUpdate();
    }
    
    // TODO: FixedUpdate loop in CameraStates?

    public void OnLookIA(InputAction.CallbackContext ctx)
    {
        CamState.OnLookIA(ctx);
    }
    
    public void OnMainButtonIA(InputAction.CallbackContext ctx)
    {
        CamState.OnMainButtonIA(ctx);
    }

    public void OnSecondaryButtonIA(InputAction.CallbackContext ctx)
    {
        CamState.OnSecondaryButtonIA(ctx);
    }
    
    #endregion
    
    #region Methods

    public void ChangeState(CameraState newState)
    {
        newState.OnExit();
        CamState = newState;
        newState.OnEnter();
    }
    
    #endregion
}
