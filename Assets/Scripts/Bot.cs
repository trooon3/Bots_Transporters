using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bot : MonoBehaviour
{
    public UnityAction ResourceGiven;
    [SerializeField] private Base _base;
    [SerializeField] private Resource _target;
    private Vector3 _resouscePlace = new Vector3(0, 2, 0);
    private Mover _mover;

    public bool InWay { get; private set; }

    private void Awake()
    {
        _mover = GetComponent<Mover>();
    }

    private void Start()
    {
        InWay = false;
    }

    private void Update()
    {
        if (_target != null && InWay == false)
        {
            TakeResourceInTarget(_target);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Base _base))
        {
            GetResource(_target);
        }

        if (collider.TryGetComponent(out Resource resource))
        {
            if (_target.IsTaked || resource.IsTaked || _target != resource)
            {
                return;
            }

            TakeResource(_target);
        }
    }

    public void TakeResouscePoint(Resource resource)
    {
        _target = resource;
    }

    private void TakeResourceInTarget(Resource resource)
    {
        InWay = true;

        _mover.SetTarget(resource);
    }

    private void GetResource(Resource resource)
    {
        ResourceGiven.Invoke();
        transform.DetachChildren();
        resource.IsTaked = false;
        resource.IsReserved = false;
        resource.SetActiveFalse();
        InWay = false;
        _target = null;
    }

    private void TakeResource(Resource resource)
    {
        resource.transform.SetParent(transform);
        resource.transform.position = transform.position + _resouscePlace;
        resource.IsTaked = true;

        _mover.SetTarget(_base);
    }
}
