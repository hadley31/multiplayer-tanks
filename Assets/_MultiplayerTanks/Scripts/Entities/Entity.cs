using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void EntityDelegate(Entity e);

[System.Serializable]
public class Entity : EntityBase
{
    private EntityBase m_ComponentOfInterest;

    public bool Is<T>() where T : EntityBase
    {
        return GetComponent<T>() != null;
    }

    public T GetComponentOfType<T>() where T : EntityBase
    {
        if (m_ComponentOfInterest == null)
        {
            m_ComponentOfInterest = GetComponent<T>();
        }

        return m_ComponentOfInterest as T;
    }
}
