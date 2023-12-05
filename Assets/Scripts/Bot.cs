using UnityEngine;


public class Bot : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private Resource _target;

    private Mover _mover;
    private Carrier _carrier;

    public bool InWay { get; private set; }
    public Base Base => _base;
    public Carrier Ñarrier => _carrier;

    private void Awake()
    {
        _carrier = GetComponent<Carrier>();
        _mover = GetComponent<Mover>();
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

        if (collider.TryGetComponent(out Base coreBuilding))
        {
            _carrier.GetResource(_target);
            ResetTarget();
        }
    }

    public void TakeResouscePoint(Resource resource)
    {
        _target = resource;
        _mover.SetTarget(resource.transform.position);
        InWay = true;
    }

    private void ResetTarget()
    {
         InWay = false;
        _target = null;
    }

}
