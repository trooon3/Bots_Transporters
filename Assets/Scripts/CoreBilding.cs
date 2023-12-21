using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Flag))]
public class CoreBilding : MonoBehaviour
{
    private List<Bot> _bots = new List<Bot>();
    private Spawner _spawner;
    private Observer _observer;
    private Queue<Resource> _targets;
    private Coroutine _resourceFinder;
    private WaitForSeconds _delayBetweenFindResourses;
    private int _resourceCount;
    private float _secondsBetweenFindResourses = 1f;
    public Flag Flag;

    public int ResourceCount => _resourceCount;
    public bool IsTakenFlag;

    private void Awake()
    {
        _delayBetweenFindResourses = new WaitForSeconds(_secondsBetweenFindResourses);
        _targets = new Queue<Resource>();
    }

    private void Start()
    {
        _observer = _spawner.GetObserver();
        _resourceFinder = StartCoroutine(ResourceFinder());
    }

    private IEnumerator ResourceFinder()
    {
        while (true)
        {
            if (_resourceCount >=5 && IsTakenFlag)
            {
                SendBotToBuild();
            }

            if (_resourceCount >= 3 && IsTakenFlag == false)
            {
                AddBot();
            }

            if (_targets.Count == 0)
            {
                TryFindResources();
            }
            
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
        if (_observer.ResourcesFull())
        {
            _targets = _observer.GetQ();
        }
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
            bot.TakeResourcePoint(target);
            target.IsReserved = true;
        }
        else
        {
            return;
        }
    }

    private void AddBot()
    {
        _resourceCount -= 3;
        var bot = _spawner.CreateBot();
        bot.SetCoreBuildig(this);
        _bots.Add(bot);
    }

    public void IncreaceResourceCount()
    {
        _resourceCount++;
    }

    private void SendBotToBuild()
    {
       var bot = _spawner.CreateBot();

        _resourceCount -= 5;
        bot.TakeBuildPoint(Flag.transform);
    }

    public void CreateFlag()
    {
        if (IsTakenFlag == false)
        {
            Flag = _spawner.CreateFlag();
            Flag.CoreBilding = this;
            IsTakenFlag = true;
        }
    }

    public void SetFlagPosition(Vector3 position)
    {
        if (IsTakenFlag)
        {
            Flag.transform.position = position;
        }
    }

    public void Initialize(Spawner spawner, Bot bot)
    {
        _bots.Add(bot);
        _spawner = spawner;
    }
}
