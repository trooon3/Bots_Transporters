using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Resource _resourceTemplate;
    [SerializeField] private Bot _templateBot;
    [SerializeField] private CoreBilding _coreBuildingTemplate;
    [SerializeField] private GameObject _spawnBotPoint;
    [SerializeField] private GameObject _spawnCoreBuildingPoint;
    [SerializeField] private Flag _flagTemplate;
    [SerializeField] private Observer _observer;

    private WaitForSeconds _delayBetweenSpawn;
    private List<Resource> _resources = new List<Resource>();
    public UnityAction<Resource> ResourceSpawned;

    private float _secondsBetweenSpawn = 0.1f;
    private int _maxResourceCount = 16;
    private int _maxXSpawnPosition = 20;
    private int _minXSpawnPosition = -20;
    private int _maxZSpawnPosition = 0;
    private int _minZSpawnPosition = -20;
    private int _ySpawnPosition = 1;

    private Coroutine _coroutine;

    private void Start()
    {
        _delayBetweenSpawn = new WaitForSeconds(_secondsBetweenSpawn);

        for (int i = 0; i < _maxResourceCount; i++)
        {
            Resource newResource = Instantiate(_resourceTemplate);
            _resources.Add(newResource);
            newResource.gameObject.SetActive(false);
        }

        Bot botFirst = CreateBot();
        Bot botSecond = CreateBot();
        Bot botThird = CreateBot();

        CoreBilding coreBilding = CreateCoreBuilding(botFirst, _spawnCoreBuildingPoint.transform.position);
        coreBilding.Initialize(this, botSecond);
        coreBilding.Initialize(this, botThird);

        botFirst.SetCoreBuildig(coreBilding);
        botSecond.SetCoreBuildig(coreBilding);
        botThird.SetCoreBuildig(coreBilding);

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(SpawnResources());
    }

    private IEnumerator SpawnResources()
    {
        while (true)
        {
            Spawn();
            yield return _delayBetweenSpawn;
        }
    }

    private void Spawn()
    {
        Vector3 spawnPoint = new Vector3
           (Random.Range(_minXSpawnPosition, _maxXSpawnPosition), _ySpawnPosition, 
            Random.Range(_minZSpawnPosition, _maxZSpawnPosition));
        Resource resourceToSpawn = _resources.FirstOrDefault(resource => resource.gameObject.activeSelf == false);

        if (resourceToSpawn == null)
        {
            return;
        }

        ResourceSpawned?.Invoke(resourceToSpawn);
        resourceToSpawn.transform.position = spawnPoint;
        resourceToSpawn.gameObject.SetActive(true);
    }

    public Bot CreateBot()
    {
        Bot bot = Instantiate(_templateBot, _spawnBotPoint.transform.position, Quaternion.identity);
        bot.SetSpawner(this);
        return bot;
    }

    public CoreBilding CreateCoreBuilding(Bot bot, Vector3 buildPosition)
    {
        CoreBilding coreBilding = Instantiate(_coreBuildingTemplate, buildPosition, Quaternion.identity);
        coreBilding.Initialize(this, bot);
        return coreBilding;
    }

    public Flag CreateFlag()
    {
        Flag flag = Instantiate(_flagTemplate, transform.position + new Vector3(0,3,0), Quaternion.identity);
        return flag;
    }

    public Observer GetObserver()
    {
        return _observer;
    }
}
