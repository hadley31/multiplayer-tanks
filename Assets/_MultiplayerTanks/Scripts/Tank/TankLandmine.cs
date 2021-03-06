﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankLandmine : TankBase
{
    [Header("Spawn Info")]
    public Landmine landminePrefab;

    [Header("Use Info")]
    public int maxLandmines = 2;
    public float useCooldown = 5;
    public float landmineRechargeCooldown = 20;

    [Header("Landmine Info")]
    public float fuse = 8;
    public int damage = 1000;
    public int health = 1;
    public float radius = 2;

    public int Landmines
    {
        get;
        private set;
    }

    private float m_lastUseTime;
    private float m_rechargeTimer;

    private void Start()
    {
        Landmines = maxLandmines;
    }

    private void Update()
    {
        if (Tank.IsLocal == false)
        {
            return;
        }

        if (Tank.IsAlive == false)
        {
            return;
        }

        m_rechargeTimer -= Time.deltaTime;

        if (Landmines < maxLandmines)
        {
            m_rechargeTimer -= Time.deltaTime;

            if (m_rechargeTimer < 0)
            {
                Landmines++;
                m_rechargeTimer = landmineRechargeCooldown;
            }
        }
    }

    public void Use()
    {
        if (photonView.isMine == false)
        {
            return;
        }

        if (Tank.IsAlive == false)
        {
            return;
        }

        if (Landmines <= 0 && m_rechargeTimer > 0)
        {
            return;
        }

        int id = LandmineManager.GetNextID();

        LandmineManager.Instance.SpawnNew(transform.position, fuse, damage, health, radius, photonView.viewID, id, PhotonNetwork.time);

        Landmines--;
        m_rechargeTimer = useCooldown;
    }
}
