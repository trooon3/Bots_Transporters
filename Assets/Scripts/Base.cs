using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Scaner))]
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
  

    private void OnEnable()
    {
        foreach (var bot in _bots)
        {
            bot.Carrier.ResourceGiven += IncreaceResourceCount;
        }
    }

    private void OnDisable()
    {
        foreach (var bot in _bots)
        {
            bot.Carrier.ResourceGiven -= IncreaceResourceCount;
        }
    }

    private IEnumerator ResourceFinder()
    {
        while (true)
        {
            TryFindResources();

            if (_targets.Count == 0)
            {
                yield return null;
            }

            for (int i = 0; i < _targets.Count; i++)
            {
              TrySendBot();
            }

            yield return _delayBetweenFindResourses;
        }
    }

    private void TryFindResources()
    {
        _targets = _scaner.GetTargets(); 
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
            return;
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
