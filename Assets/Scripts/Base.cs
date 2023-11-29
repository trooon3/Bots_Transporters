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
    private Coroutine _resourceFinder;
    private WaitForSeconds _delayBetweenFindResourses;
    private int _resourceCount;
    private float _secondsBetweenFindResourses = 2f;

    public int ResursCont => _resourceCount;

    private void Start()
    {
        _delayBetweenFindResourses = new WaitForSeconds(_secondsBetweenFindResourses);
        _scaner = new Scaner();
        _targets = new List<Resource>();
       _resourceFinder = StartCoroutine(ResourceFinder());
    }

    private IEnumerator ResourceFinder()
    {
        while (true)
        {
            TryFindResources();
            TrySendBot();
            yield return _delayBetweenFindResourses;
        }
    }

    private void OnEnable()
    {
        foreach (var bot in _bots)
        {
            bot.Ñarrier.ResourceGiven += IncreaceResourceCount;
        }
    }

    private void OnDisable()
    {
        foreach (var bot in _bots)
        {
            bot.Ñarrier.ResourceGiven -= IncreaceResourceCount;
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
