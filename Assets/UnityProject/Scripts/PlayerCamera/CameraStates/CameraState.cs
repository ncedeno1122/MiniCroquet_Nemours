using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class CameraState
{
    #region Fields

    public const float UPPER_LOOKLIMIT_DEG = 88f; // 90f causes snaps with transform.LookAt()
    public const float LOWER_LOOKLIMIT_DEG = -15f;
    public const float CAM_ORBIT_SPEED = 25f;
    protected float m_CamRadiusFromTarget = 10f;

    protected Vector2 CurrentRotationEulers
    {
        get => m_Context.CurrentRotationEulers;
        set => m_Context.CurrentRotationEulers = value;
    }

    protected PlayerCameraController m_Context;
    
    #endregion

    protected CameraState(PlayerCameraController pcc)
    {
        m_Context = pcc;
    }

    public abstract void OnLookIA(InputAction.CallbackContext ctx);
    public abstract void OnMainButtonIA(InputAction.CallbackContext ctx);
    public abstract void OnSecondaryButtonIA(InputAction.CallbackContext ctx);
    
    public abstract void OnEnter();
    public abstract void OnExit();
    
    public void OnUpdate()
    {
        EarlyUpdate();
        MidUpdate();
        LateUpdate();
    }
    protected abstract void EarlyUpdate();
    protected abstract void MidUpdate();
    protected abstract void LateUpdate();
}
