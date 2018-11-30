using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class TankHealthVisual : MonoBehaviour
{
    private Slider m_Slider;

    public float Value
    {
        get;
        private set;
    }

    private void Awake()
    {
        m_Slider = GetComponent<Slider>();
    }

    private void Update()
    {
        Tank tank = SpectatorCamera.Instance?.MainTarget.GetComponent<Tank>();

        if (tank != null)
        {
            float newValue = tank.Movement.Boost / tank.Movement.maxBoost;
            if (newValue != Value)
            {
                m_Slider.value = newValue;
                Value = newValue;
            }
        }
    }
}
