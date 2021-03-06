﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TankBase : EntityBase
{
    private Tank m_Tank;

    public Tank Tank
    {
        get
        {
            if (m_Tank == null)
            {
                m_Tank = GetComponent<Tank>();
            }

            return m_Tank;
        }
    }

    private TankInput m_Input;

    public TankInput TankInput
    {
        get
        {
            if (m_Input == null)
            {
                m_Input = GetComponent<TankInput>();
            }

            return m_Input;
        }
    }

    private TankShoot m_Shooting;

    public TankShoot Shooting
    {
        get
        {
            if (m_Shooting == null)
            {
                m_Shooting = GetComponent<TankShoot>();
            }

            return m_Shooting;
        }
    }

    private TankLandmine m_Landmine;

    public TankLandmine TankLandmine
    {
        get
        {
            if (m_Landmine == null)
            {
                m_Landmine = GetComponent<TankLandmine>();
            }

            return m_Landmine;
        }
    }

    private TankMovement m_Movement;

    public TankMovement Movement
    {
        get
        {
            if (m_Movement == null)
            {
                m_Movement = GetComponent<TankMovement>();
            }

            return m_Movement;
        }
    }

    private TankNametag m_Nametag;

    public TankNametag NameTag
    {
        get
        {
            if (m_Nametag == null)
            {
                m_Nametag = GetComponentInChildren<TankNametag>();
            }

            return m_Nametag;
        }
    }

    private TankVisuals m_Visuals;

    public TankVisuals Visuals
    {
        get
        {
            if (m_Visuals == null)
            {
                m_Visuals = GetComponent<TankVisuals>();
            }

            return m_Visuals;
        }
    }

    private NetworkTank m_NetworkTank;

    public NetworkTank NetworkTank
    {
        get
        {
            if (m_NetworkTank == null)
            {
                m_NetworkTank = GetComponent<NetworkTank>();
            }
            return m_NetworkTank;
        }
    }
}
