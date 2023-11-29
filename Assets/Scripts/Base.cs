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
    private Queue<Resource> _targets;
    private Coroutine _resourceFinder;
    private WaitForSeconds _delayBetweenFindResourses;
    private int _resourceCount;
    private float _secondsBetweenFindResourses = 2f;

    public int ResursCont => _resourceCount;

    private void Start()
    {
        _delayBetweenFindResourses = new WaitForSeconds(_secondsBetweenFindResourses);
        _targets = new Queue<Resource>();
        _scaner = GetComponent<Scaner>();
       _resourceFinder = StartCoroutine(ResourceFinder());
    }

    private IEnumerator ResourceFinder()
    {
        while (true)
        {
            if (_targets.Count == 0)
            {
                yield return null;
            }

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
        var bot = _bots.Where(bot => bot.InWay == false).FirstOrDefault();

        if (bot == null)
        {
            return;
        }

        if (_targets.Count == 0)
        {
            TryFindResources();
        }

        var target = _targets.Dequeue();

        if (target.IsReserved == false)
        {
            bot.TakeResouscePoint(target);
            target.IsReserved = true;
        }
        else
        {
            return;
        }
    }

    private void IncreaceResourceCount()
    {
        _resourceCount++;
    }
}
