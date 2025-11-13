using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static Action onJump;

    public static PlayerController Instance;

    [SerializeField] private Transform _feetTranform;
    [SerializeField] private Vector2 _groundCheck;
    [SerializeField] private LayerMask _groundlayer;
    [SerializeField] private float _jumpStrength = 7f;
    [SerializeField] private float _extraGravity = 700f;
    [SerializeField] private float _gravityDelay = 0.2f;
    [SerializeField] private float _coyoteTime = 0.5f;

    private float timeInAir;
    private float _coyoteTimer;
    private bool doubleJumpAvailable;

    private PlayerInput _playerInput;
    private FrameInput _frameInput;

    private bool _isGrounded = false;
    

    private Rigidbody2D _rigidBody;
    private Movement _movement;

    public void Awake() {
        if (Instance == null) { Instance = this; }

        _rigidBody = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _movement = GetComponent<Movement>();
    }

    private void OnEnable()
    {
        onJump += ApplyJumpForce;
    }
    private void OnDisable()
    {
        onJump -= ApplyJumpForce;
    }

    private void Update()
    {
        GatherInput();
        CoyoteTimer();
        HandleJump();
        HandleSpriteFlip();
        Movement();
        GravityDelay();
    }

    

    private void FixedUpdate()
    {
        extraGravity();
    }

    public bool IsFacingRight()
    {
        return transform.eulerAngles.y == 0;
    }

    private bool CheckGrounded()
    {
        Collider2D isGrounded = Physics2D.OverlapBox(_feetTranform.position, _groundCheck, 0f, _groundlayer);
        return isGrounded;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_feetTranform.position, _groundCheck);
    }

    private void GravityDelay()
    {
        if (!CheckGrounded())
        {
            timeInAir = Time.deltaTime;
        }
        else
        {
            timeInAir = 0f;
        }
    }

    private void extraGravity()
    {
        if ((timeInAir > _gravityDelay))
        {
            _rigidBody.AddForce(new Vector2(0f, -_extraGravity * Time.deltaTime));
        }
    }

    private void GatherInput()
    {
        //float moveX = Input.GetAxis("Horizontal");
        //_movement = new Vector2(moveX * _moveSpeed, _rigidBody.linearVelocity.y);

        _frameInput = _playerInput.FrameInput;
        
    }

    private void Movement()
    {
        _movement.SetCurrentDirection(_frameInput.Move.x);

    }


    private void HandleJump()
    {
        if(!_frameInput.Jump)
        {
            return;
        }

        if(doubleJumpAvailable)
        {
            
            onJump?.Invoke();
        }
        else if(_coyoteTime > 0f)
        {
            onJump?.Invoke();
        }
        else if (doubleJumpAvailable)
        {
            doubleJumpAvailable = false;
            onJump?.Invoke();
        }
    }
    
    private void CoyoteTimer()
    {
        if(CheckGrounded())
        {
            _coyoteTimer = _coyoteTime;
        }
        else
        {
            _coyoteTimer -= Time.deltaTime;
        }
    }

    private void ApplyJumpForce()
    {
        _rigidBody.linearVelocity = Vector2.zero;
        timeInAir = 0f;
        _coyoteTimer = 0f;
        _rigidBody.AddForce(Vector2.up * _jumpStrength, ForceMode2D.Impulse);
    }

    private void HandleSpriteFlip()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePosition.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
        }
        else
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    } 
}
