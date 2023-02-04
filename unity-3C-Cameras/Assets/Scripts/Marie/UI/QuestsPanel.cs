using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestsPanel : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void OpenClose()
    {
        _animator.SetTrigger("OpenClose");
    }

}
