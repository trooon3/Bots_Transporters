using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Сarrier : MonoBehaviour
{
    private Bot _bot;
    private Mover _mover;
    private Vector3 _resouscePlace = new Vector3(0, 2, 0);

    public UnityAction ResourceGiven;

    private void Start()
    {
        _bot = GetComponent<Bot>();
        _mover = GetComponent<Mover>();
    }

    public void GetResource(Resource resource)
    {
        ResourceGiven.Invoke();
        transform.DetachChildren();
        resource.IsTaked = false;
        resource.IsReserved = false;
        resource.SetActiveFalse();
    }

    public void TakeResource(Resource resource)
    {
        resource.transform.SetParent(transform);
        resource.transform.position = transform.position + _resouscePlace;
        resource.IsTaked = true;

        _mover.SetTarget(_bot.Base);
    }
}
