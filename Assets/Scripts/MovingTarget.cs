using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTarget : MonoBehaviour, IShootable
{
    
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
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(isEnabled)
        {
            _rb.MovePosition(_movementDirection * _speed);
        }
    }

    public void Enable()
    {
        isEnabled = true;
    }

    public void GetShot()
    {
        if(wasShot)
        {
            _animator.SetBool("WasShot", true);
            wasShot = true;
            _gameManager.AddPoints(_pointsValue);
        }
    }

    public void StopMoving()
    {
        isEnabled = false;
        _animator.SetBool("IsReset", true);
        ResetAnimatorBools();
    }

    private void ResetAnimatorBools()
    {
        wasShot = false;
        _animator.SetBool("WasShot", false);
        _animator.SetBool("IsReset", false);
    }
}
