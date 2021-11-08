using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;

    private InputAction aimAction, fireAction, moveAction, timeStopAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        fireAction = playerInput.actions["Fire"];
        fireAction.ReadValue<bool>();
        aimAction = playerInput.actions["Aim"];
        aimAction.ReadValue<Vector2>();
        moveAction = playerInput.actions["Move"];
        moveAction.ReadValue<Vector2>();
        timeStopAction = playerInput.actions["TimeStop"];
        timeStopAction.ReadValue<bool>();
    }
}
