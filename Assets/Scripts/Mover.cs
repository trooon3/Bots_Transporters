using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private GameObject _target;

    private void Update()
    {
        if (_target == null)
        {
            return;
        }

       transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);
    }

    public void SetTarget(Resource target)
    {
        _target = target.gameObject;
    }
    
    public void SetTarget(Base target)
    {
        _target = target.gameObject;
    }
}
