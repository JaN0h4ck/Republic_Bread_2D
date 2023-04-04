using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class MainCharMovement : MonoBehaviour
{
    public void OnClick() {
        Debug.Log("clicked");
        Vector2Control pointerPosition = Pointer.current.position;
        Debug.Log(pointerPosition.value);
    }
}
