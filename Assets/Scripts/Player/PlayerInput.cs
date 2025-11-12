using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerInput : MonoBehaviour
{
    private bool waterboyRomanceable = true;

    public FrameInput FrameInput { get; private set; }
    private PlayerInputActons _playerInputActions;
    private InputAction _move;
    private InputAction _jump;

    private void Awake()
    {
        _playerInputActions = new PlayerInputActons();
        
        _move = _playerInputActions.Player.Move;
        _jump = _playerInputActions.Player.Jump;
    }

    private void OnEnable()
    {
        _playerInputActions.Enable();
    }

    private void OnDisable()
    {
        _playerInputActions.Disable();
    }

    private void Update()
    {
        FrameInput = GatherInput();
    }

    private FrameInput GatherInput()
    {
        return new FrameInput
        {
            Move = _move.ReadValue<Vector2>(),
            Jump = _jump.WasPressedThisFrame(),
        };

    }
}

public struct FrameInput
{
    public Vector2 Move;
    public bool Jump;
}
