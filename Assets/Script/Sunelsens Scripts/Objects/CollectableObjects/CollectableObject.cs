using System;
using UnityEngine;

public abstract class CollectableObject : MonoBehaviour, ICollectable
{
    [SerializeField] protected float _scorePerObject;

    public virtual void Collect()
    {
        Debug.Log($"{GetType().Name} is collected");
    }
}
