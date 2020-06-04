using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if !UNITY_EDITOR && UNITY_WEBGL
using System.Runtime.InteropServices;
#endif

public class PointerLockUtility : MonoBehaviour
{
#if UNITY_WEBGL  && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void RequestPointerStateChange(bool locked);
#endif

    private static bool _pointerLocked = false;
    public bool PointerLocked { get { return _pointerLocked; } }

    private void OnPointerStateChange(string state)
    {
        Debug.Log($"OnPointerStateChange: {state}");

        switch (state)
        {
            case "normal":
                _pointerLocked = false;
                break;
            case "locked":
                _pointerLocked = true;
                break;
            default:
                Debug.LogError("Invalid pointer state received! State value \'" + state + "\' is not recognized.", gameObject);
                _pointerLocked = false;
                break;
        }
    }

#if UNITY_EDITOR
    private void SetLockState(bool locked) {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
        _pointerLocked = locked;
    }
#endif

    public void RequestPointerLock(bool locked)
    {
#if UNITY_EDITOR
        SetLockState(locked);
#elif UNITY_WEBGL 
        RequestPointerStateChange(locked);
#endif
    }
}
