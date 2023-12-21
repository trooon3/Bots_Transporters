using UnityEngine;

public class Mover : MonoBehaviour
{
    private float _speed;
    private Transform _target;

    private void Awake()
    {
        _speed = 10;
        _target = transform;
    }

    private void Update()
    {
        if (_target == null)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
}
