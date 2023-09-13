using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class CameraState
{
    #region Fields

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
