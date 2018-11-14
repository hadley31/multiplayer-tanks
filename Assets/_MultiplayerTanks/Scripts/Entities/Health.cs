using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Entity))]
public class Health : EntityBase
{
    [SerializeField]
    private int m_MaxHealth;

    [SerializeField]
    private bool m_GodMode;

    public IntUnityEvent onHealthChanged;
    public UnityEvent onDie;

    protected int m_health;

    public virtual int Max
    {
        get { return m_MaxHealth; }
        set { m_MaxHealth = value; }
    }


    public virtual int Value
    {
        get
        {
            return m_health;
        }
        private set
        {
            m_health = Mathf.Clamp(value, 0, m_MaxHealth);
            onHealthChanged.Invoke(m_health);
            if (m_health <= 0)
            {
                Die();
            }
        }
    }


    public virtual bool GodMode
    {
        get { return m_GodMode; }
        private set { m_GodMode = value; }
    }


    protected virtual void Start()
    {
        SetValueToMax();
    }


    public virtual void SetValue(int value)
    {
        if (NetworkManager.IsMasterClient == false)
        {
            return;
        }

        photonView.RPC("SetValueRPC", PhotonTargets.AllBuffered, value);
    }


    [PunRPC]
    protected virtual void SetValueRPC(int value)
    {
        Value = value;
    }


    public virtual void Decrease(int amount)
    {
        SetValue(Value - amount);
    }


    public virtual void SetValueToMax()
    {
        SetValue(m_MaxHealth);
    }


    public virtual void SetMaxValue(int maxValue, bool setValueToMax = false)
    {
        if (NetworkManager.IsMasterClient == false)
        {
            return;
        }

        photonView.RPC("SetMaxValueRPC", PhotonTargets.All, maxValue, setValueToMax);
    }


    [PunRPC]
    protected virtual void SetMaxValueRPC(int maxValue, bool setValueToMax = false)
    {
        Max = maxValue;

        if (setValueToMax)
        {
            Value = Max;
        }
    }


    protected virtual void Die()
    {
        onDie.Invoke();
    }


    public virtual void Destroy()
    {
        Destroy(gameObject);
    }
}