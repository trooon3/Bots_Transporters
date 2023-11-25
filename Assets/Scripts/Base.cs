using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class Base : MonoBehaviour
{
    [SerializeField] private Spawner _spawner;
    [SerializeField] private List<Bot> _bots = new List<Bot>();
    private Scaner _scaner;
    private List<Resource> _targets;
    private int _resourceCount;
    public int ResursCont => _resourceCount;

    private void Start()
    {
        _scaner = new Scaner();
        _targets = new List<Resource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TryFindResources();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            TrySendBot();
        }
    }

    private void OnEnable()
    {
        foreach (var bot in _bots)
        {
            bot.ResourceGiven += IncreaceResourceCount;
        }
    }

    private void OnDisable()
    {
        foreach (var bot in _bots)
        {
            bot.ResourceGiven -= IncreaceResourceCount;
        }
    }

    private void TryFindResources()
    {
        _targets = _scaner.Scan();
    }

    private void TrySendBot()
    {
        var bot = _bots.SkipWhile(bot => bot.InWay).FirstOrDefault();
        var target = _targets.SkipWhile(resource => resource.IsReserved).FirstOrDefault();

        if (bot == null|| target == null)
        {
            return;
        }

        target.IsReserved = true;
        bot.TakeResouscePoint(target);
    }

    private void IncreaceResourceCount()
    {
        _resourceCount++;
    }
}
