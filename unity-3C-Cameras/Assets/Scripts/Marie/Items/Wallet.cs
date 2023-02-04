using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    public static Wallet Instance;
    [SerializeField] private int money;
    void Awake()
    {
        if (Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        MoneyUI.Instance.UpdateMoney();
    }


    public int GetMoney()
    {
        return money;
    }

    public void EarnMoney(int amount)
    {
        money += amount;
    }

    public void SpendMoney(int amount)
    {
        if(CanSpend(amount))
            money -= amount;
    }

    public bool CanSpend(int amount)
    {
        return money >= amount;
    }
}
