using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Resource _template;
    [SerializeField] private GameObject _container;

    private WaitForSeconds _delayBetweenSpawn;
    private List<Resource> _resources = new List<Resource>();

    private float _secondsBetweenSpawn = 1f;
    private int _maxResourceCount = 4;
    private int _maxXSpawnPosition = 20;
    private int _minXSpawnPosition = -20;
    private int _maxZSpawnPosition = 0;
    private int _minZSpawnPosition = -20;
    private int _maxYSpawnPosition = 1;
    private int _minYSpawnPosition = 0;

    private Coroutine _coroutine;

    private void Start()
    {
        _delayBetweenSpawn = new WaitForSeconds(_secondsBetweenSpawn);

        for (int i = 0; i < _maxResourceCount; i++)
        {
            Resource newRes = Instantiate(_template);
            _resources.Add(newRes);
            newRes.gameObject.SetActive(false);
        }

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
           (Random.Range(_minXSpawnPosition, _maxXSpawnPosition), 
            Random.Range(_minYSpawnPosition, _maxYSpawnPosition), 
            Random.Range(_minZSpawnPosition, _maxZSpawnPosition));
        var sortResources = _resources.SkipWhile(resource => resource.gameObject.activeSelf);
        Resource resourceToSpawn = sortResources.FirstOrDefault();

        if (resourceToSpawn == null)
        {
            return;
        }

        resourceToSpawn.transform.position = spawnPoint;
        resourceToSpawn.gameObject.SetActive(true);
    }
}
