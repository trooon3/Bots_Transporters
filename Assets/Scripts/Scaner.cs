using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaner : MonoBehaviour
{

    [SerializeField] private float _currentHitDistance;
    [SerializeField] private GameObject _findobject;
    [SerializeField] LayerMask _layerMask;

    private float _radius = 0.5f;
    private float _rotateSpeed = 180f;
    private float _maxDistance = 20f;

    private Vector3 _direction;
    private Vector3 _origin;
    private Queue<Resource> _resources = new Queue<Resource>();

    private void Update()
    {
        transform.Rotate(new Vector3(0, _rotateSpeed * Time.deltaTime));
        Scan();
    }

    public Queue<Resource> GetTargets()
    {
        Queue<Resource> resources = new Queue<Resource>();

        foreach (var resource in _resources)
        {
            resources.Enqueue(resource);
        }

        _resources.Clear();
        return resources;
    }

    public void Scan()
    {
        RaycastHit hit;
        _origin = transform.position;
        _direction = transform.forward;

        if (Physics.SphereCast(_origin, _radius, _direction, out hit, _maxDistance, _layerMask, QueryTriggerInteraction.UseGlobal))
        {
            _currentHitDistance = hit.distance;
            _findobject = hit.transform.gameObject;

            if (_findobject.TryGetComponent(out Resource resource))
            {
                _resources.Enqueue(resource);
            }

        }
        else
        {
            _currentHitDistance = _maxDistance;
            _findobject = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_origin, _origin + _direction * _currentHitDistance);
        Gizmos.DrawWireSphere(_origin + _direction * _currentHitDistance, _radius);
    }
}
