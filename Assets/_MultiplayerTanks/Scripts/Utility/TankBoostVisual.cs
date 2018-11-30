using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class TankBoostVisual : MonoBehaviour
{
    private Slider m_Slider;

    public float Value
    {
        get { return m_Slider.value; }
        set
        {
            if (m_Slider.value != value)
            {
                m_Slider.value = value;
            }
        }
    }

    private void Awake()
    {
        m_Slider = GetComponent<Slider>();
    }

    private void Update()
    {
        Tank tank = SpectatorCamera.Instance?.MainTarget?.GetComponent<Tank>();

        if (tank != null)
        {
            Value = tank.Movement.Boost / tank.Movement.maxBoost;
        }
    }
}
