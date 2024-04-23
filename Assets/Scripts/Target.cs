using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IShootable
{
    [SerializeField] GameManager _gameManager;
    [SerializeField] private int _pointsValue;
    private Animator _animator;
    private bool wasShot = false;
    private bool isEnabled = false;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Enable()
    {
        _animator.SetBool("IsEnabled", true);
        isEnabled = true;
    }

    public void GetShot()
    {
        if(isEnabled)
        {
            _animator.SetBool("WasShot", true);
            wasShot = true;
            isEnabled = false;
            _gameManager.AddPoints(_pointsValue);
        }
    }

    public void Disable()
    {
        if(!wasShot)
        {
            _animator.SetBool("IsEnabled", false);
            isEnabled = false;
        }
    }

    private void ResetAnimatorBools()
    {
        wasShot = false;
        _animator.SetBool("WasShot", false);
        _animator.SetBool("IsEnabled", false);
    }
}
