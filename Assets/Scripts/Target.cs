using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IShootable
{
    private Animator _animator;
    private bool wasShot = false;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Enable()
    {
        _animator.SetBool("IsEnabled", true);
    }

    public void GetShot()
    {
        _animator.SetBool("WasShot", true);
        wasShot = true;
    }

    public void Disable()
    {
        if(!wasShot)
        {
            _animator.SetBool("IsEnabled", false);
        }
    }

    private void ResetAnimatorBools()
    {
        wasShot = false;
        _animator.SetBool("WasShot", false);
        _animator.SetBool("IsEnabled", false);
    }
}
