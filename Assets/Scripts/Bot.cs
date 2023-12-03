using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bot : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private Resource _target;

    private Mover _mover;
    private Ñarrier _carrier;

    public bool InWay { get; private set; }
    public Base Base => _base;
    public Ñarrier Ñarrier => _carrier;

    private void Awake()
    {
        _carrier = GetComponent<Ñarrier>();
        _mover = GetComponent<Mover>();
    }

    private void Start()
    {
        InWay = false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Resource resource))
        {
            if (_target != resource)
            {
                return;
            }

           _carrier.TakeResource(_target);
        }

        if (collider.TryGetComponent(out Base _base))
        {
            _carrier.GetResource(_target);
            ResetTarget();
        }
    }

    public void TakeResouscePoint(Resource resource)
    {
        _target = resource;
        _mover.SetTarget(resource);
        InWay = true;
    }

    private void ResetTarget()
    {
         InWay = false;
        _target = null;
    }

}
