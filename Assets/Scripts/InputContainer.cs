using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputContainer : Utils.Singleton<InputContainer>
{
    public Unified_Input inputActions;

    private void Awake() {
        inputActions = new Unified_Input();
    }

    private void Start() {
        inputActions.Enable();
    }

    public void DisableActions() {
        inputActions.Disable();
    }

    public void EnableActions() {
        inputActions.Enable();
    }
}
