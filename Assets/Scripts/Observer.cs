using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Observer : MonoBehaviour
{
    [SerializeField] private Spawner _spawner;
    private Queue<Resource> _resources;

    private void Awake()
    {
        _resources = new Queue<Resource>();
    }

    private void OnEnable()
    {
        _spawner.ResourceSpawned += AddResource;
    }

    private void OnDisable()
    {
        _spawner.ResourceSpawned -= AddResource;
    }

    private void AddResource(Resource resource)
    {
        _resources.Enqueue(resource);
    }

    public bool ResourcesFull()
    {
        if (_resources.Count > 0)
        {
            return true;
        }

        return false;
    }

    public Queue<Resource> GetQ()
    {
        Queue<Resource> resources = new Queue<Resource>();

        foreach (var resource in _resources)
        {
            resources.Enqueue(resource);
        }

        _resources = new Queue<Resource>();
        return resources;
    }
}
