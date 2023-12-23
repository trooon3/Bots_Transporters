using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(Carrier))]
public class Bot : MonoBehaviour
{
    private CoreBilding _coreBuilding;
    private Resource _target;
    private Spawner _spawner;
    private Transform _bulidPoit;
    private Mover _mover;
    private Carrier _carrier;

    public bool InWay { get; private set; }
    public CoreBilding CoreBuilding => _coreBuilding;
    public Carrier Carrier => _carrier;

    public UnityAction ResourceGet;

    private void Awake()
    {
        _carrier = GetComponent<Carrier>();
        _mover = GetComponent<Mover>();
    }

    private void OnEnable()
    {
        _carrier.ResourceGiven += IncreaseCoreBuilngCount;
    }

    private void OnDisable()
    {
        _carrier.ResourceGiven -= IncreaseCoreBuilngCount;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Resource resource))
        {
            if (_target == resource)
            {
                _carrier.TakeResource(_target);
            }
        }

        if (collider.TryGetComponent(out CoreBilding coreBuilding))
        {
            if (transform.childCount > 0)
            {
                _carrier.GetResource(_target);
                ResetTarget();
            }
        }

        if (collider.TryGetComponent(out Flag flag))
        {
            if (_bulidPoit == flag.transform)
            {
                SetCoreBuildig(CreateNewBuilding(flag.transform.position));
                InWay = false;
                flag.CoreBilding.IsTakenFlag = false;
                flag.gameObject.SetActive(false);
            }
        }
    }

    public void TakeResourcePoint(Resource resource)
    {
        _target = resource;
        _mover.SetTarget(_target.transform);
        InWay = true;
    }

    public void TakeBuildPoint(Transform buildPoint)
    {
        _bulidPoit = buildPoint;
        _mover.SetTarget(buildPoint);
        InWay = true;
    }

    public void SetCoreBuildig(CoreBilding coreBilding)
    {
        _coreBuilding = coreBilding;
    }

    private void ResetTarget()
    {
         InWay = false;
        _target = null;
    }

    private void IncreaseCoreBuilngCount()
    {
        ResourceGet?.Invoke();
        _coreBuilding.SetBotUnbuzzed(this);
    }

    private CoreBilding CreateNewBuilding(Vector3 position)
    {
        return _spawner.CreateCoreBuilding(this, position);
    }

    public void SetSpawner(Spawner spawner)
    {
        _spawner = spawner;
    }
}
