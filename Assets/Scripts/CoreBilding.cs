using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Flag))]
public class CoreBilding : MonoBehaviour
{
    private Queue<Bot> _freeBots = new Queue<Bot>();
    private Queue<Bot> _busyBots = new Queue<Bot>();
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
        _busyBots = new Queue<Bot>();
        _delayBetweenFindResourses = new WaitForSeconds(_secondsBetweenFindResourses);
    }

    private void Start()
    {
        _observer = _spawner.GetObserver();
        _targets = _observer.GetQ();
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
        if (_freeBots.Count <= 0)
        {
            return;
        }

        var bot = _freeBots.Dequeue();
        _busyBots.Enqueue(bot);

        if (bot == null)
        {
            return;
        }

        if (_targets.Count == 0)
        {
            return;
        }

        var target = _targets.Dequeue();

        bot.TakeResourcePoint(target);
    }

    private void AddBot()
    {
        _resourceCount -= 3;
        var bot = _spawner.CreateBot();
        bot.SetCoreBuildig(this);
        _freeBots.Enqueue(bot);
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
        _freeBots.Enqueue(bot);
        _spawner = spawner;
    }

    public void SetBotUnbuzzed(Bot bot)
    {
        _freeBots.Enqueue(bot);
    }
}
