using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;

    private Rigidbody2D _rigidbody;

    private float _moveX;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void SetCurrentDirection(float currentDirection)
    {
        _moveX = currentDirection;
    }

    private void Move()
    {
        Vector2 movement = new Vector2(_moveX * _moveSpeed, _rigidbody.linearVelocity.y);
        _rigidbody.linearVelocity = movement;
    }
}
