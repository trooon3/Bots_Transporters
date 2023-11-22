using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class Base : MonoBehaviour
{
    [SerializeField] private Spawner _spawner;
    [SerializeField] private List<Bot> _bots = new List<Bot>();
    private Queue<Resource> _targets;
    private int _resourceCount;
    public int ResursCont => _resourceCount;

    private void Start()
    {
        _targets = new Queue<Resource>();
    }

    private void Update()
    {
        if (_targets.Count != 0 )
        {
            TrySendBot();
        }
    }

    private void OnEnable()
    {
        _spawner.Spawned += AddResourceToQueue;

        foreach (var bot in _bots)
        {
            bot.ResourceGiven += IncreaceResourceCount;
        }
    }

    private void OnDisable()
    {
        _spawner.Spawned -= AddResourceToQueue;

        foreach (var bot in _bots)
        {
            bot.ResourceGiven -= IncreaceResourceCount;
        }
    }

    private void AddResourceToQueue(Resource resource)
    {
        _targets.Enqueue(resource);
    }

    private void TrySendBot()
    {

        var bot = _bots.SkipWhile(bot => bot.InWay).FirstOrDefault();

        if (bot == null)
        {
            return;
        }

        var target = _targets.Dequeue();

        bot.TakeResouscePoint(target);
    }

    private void IncreaceResourceCount()
    {
        _resourceCount++;
    }
}
