using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Resource _template;
    [SerializeField] private GameObject _container;
    private WaitForSeconds _delayBetweenSpawn = new WaitForSeconds(1f);
    private List<Resource> _resources = new List<Resource>();
    private int _maxResourceCount = 4;
    private Coroutine _coroutine;

    public UnityAction<Resource> Spawned;

    private void Start()
    {
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
        Vector3 spawnPoint = new Vector3(Random.Range(-20, 20), Random.Range(0, 1), Random.Range(-20, 20));
        var sortResources = _resources.SkipWhile(resource => resource.gameObject.activeSelf);
        Resource resourceToSpawn = sortResources.FirstOrDefault();

        if (resourceToSpawn == null)
        {
            return;
        }

        resourceToSpawn.transform.position = spawnPoint;
        resourceToSpawn.gameObject.SetActive(true);

        Spawned.Invoke(resourceToSpawn);
    }
}
