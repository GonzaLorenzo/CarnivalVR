using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTarget : MonoBehaviour, IShootable
{
    private Vector3 startingPosition;
    [SerializeField] GameManager _gameManager;
    [SerializeField] private int _pointsValue;
    [SerializeField] private int _speed;
    [SerializeField] private Vector3 _movementDirection;
    private Rigidbody _rb;
    private Animator _animator;
    private bool wasShot = false;
    private bool isEnabled = false;

    void Start()
    {
        startingPosition = transform.position;
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if(isEnabled)
        {
            Vector3 direction = _movementDirection * _speed * Time.deltaTime;

            _rb.velocity = direction;
        }
    }

    public void Enable()
    {
        isEnabled = true;
    }

    public void GetShot()
    {
        if(!wasShot)
        {
            _animator.SetBool("WasShot", true);
            wasShot = true;
            _gameManager.AddPoints(_pointsValue);
        }
    }

    public void ResetTarget()
    {
        isEnabled = false;
        _animator.SetBool("IsReset", true);
        ResetAnimatorBools();
        transform.position = startingPosition;
    }

    private void ResetAnimatorBools()
    {
        wasShot = false;
        _animator.SetBool("WasShot", false);
        _animator.SetBool("IsReset", false);
    }
}
