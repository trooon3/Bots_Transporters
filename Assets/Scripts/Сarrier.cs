using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Сarrier : MonoBehaviour
{
    private Bot _bot;
    private Mover _mover;
    private Vector3 _offset = new Vector3(0, 2, 0);

    public event UnityAction ResourceGiven;

    private void Start()
    {
        _bot = GetComponent<Bot>();
        _mover = GetComponent<Mover>();
    }

    public void GetResource(Resource resource)
    {
        resource.IsReserved = false;
        resource.SetActiveFalse();
        transform.DetachChildren();
        ResourceGiven.Invoke();
    }

    public void TakeResource(Resource resource)
    {
        resource.transform.SetParent(transform);
        resource.transform.position = transform.position + _offset;

        _mover.SetTarget(_bot.Base);
    }
}
