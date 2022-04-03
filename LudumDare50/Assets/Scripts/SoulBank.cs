using System;
using UnityEngine;


public class SoulBank : MonoBehaviour
{
    public int Balance = 0;
    public int SoulsToTransmute = 0;

    public static event Action<int> OnSoulBalanceChanged;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Deposit(1);
        }
    }

    public void Deposit(int _numSouls)
    {
        Balance += _numSouls;
        OnSoulBalanceChanged?.Invoke(Balance);
    }

    public void WithDraw(int _numSouls)
    {
        if(Balance < _numSouls)
        {
            return;
        }
        Balance -= _numSouls;
        OnSoulBalanceChanged?.Invoke(Balance);
    }

    public bool HasEnough(int _numSouls)
    {
        return Balance >= _numSouls;
    }

    public void DepositToTransmute(int _numSouls)
    {
        SoulsToTransmute += _numSouls;
    }

    public bool HasEnoughToTransmute(int _numSouls)
    {
        return SoulsToTransmute >= _numSouls;
    }

    public void WithDrawToTransmute(int _numSouls)
    {
        if (SoulsToTransmute < _numSouls)
        {
            return;
        }
        SoulsToTransmute -= _numSouls;
    }

    public void TransmuteSouls()
    {
        if(SoulsToTransmute == 0)
        {
            return;
        }

        int sToTransmute = SoulsToTransmute;
        for(int i = 0; i < sToTransmute; ++i)
        {
            InGameUI.Instance.PerformSoulTransmutation(() =>
            {
                WithDrawToTransmute(1);
                Deposit(1);
            }, UnityEngine.Random.Range(0.1f, 0.3f) * i);
        }
    }


}
