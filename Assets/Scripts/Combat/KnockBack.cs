using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class KnockBack : MonoBehaviour
{
    public Action onKnockBackStart;
    public Action onKnockBackEnd;

    [SerializeField] private float knockbackTime = 0.2f;

    private Vector3 hitDirection;
    private float knockbackThrust;

    private Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        onKnockBackStart += ApplyKnockbackForce;
        onKnockBackEnd += StopKnockRoutine;
    }

    private void OnDisable()
    {
        onKnockBackStart -= ApplyKnockbackForce;
        onKnockBackEnd -= StopKnockRoutine;
    }

    public void GetKnockedBack(Vector3 hitDirection, float knockbackThrust)
    {
        Debug.Log("git hit loser");

        hitDirection = hitDirection;
        knockbackThrust = knockbackThrust;

        onKnockBackStart?.Invoke();
    }

    private void ApplyKnockbackForce()
    {
        Vector3 difference = (transform.position - hitDirection).normalized * knockbackThrust * rigidbody.mass;
        rigidbody.AddForce(difference, ForceMode2D.Impulse);
        StartCoroutine(KnockRoutine());
    }

    private IEnumerator KnockRoutine()
    {
        yield return new WaitForSeconds(knockbackTime);
        onKnockBackEnd?.Invoke();
    }

    private void StopKnockRoutine()
    {
        rigidbody.linearVelocity = Vector2.zero;
    }
}
