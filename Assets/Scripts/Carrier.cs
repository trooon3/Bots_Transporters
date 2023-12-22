using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(Bot))]
public class Carrier : MonoBehaviour
{
    private Bot _bot;
    private Mover _mover;
    private Vector3 _offset = new Vector3(0, 2, 0);

    public UnityAction ResourceGiven;

    private void Start()
    {
        _bot = GetComponent<Bot>();
        _mover = GetComponent<Mover>();
    }

    public void GetResource(Resource resource)
    {
        resource.SetActiveFalse();
        transform.DetachChildren();
        ResourceGiven.Invoke();
    }

    public void TakeResource(Resource resource)
    {
        resource.transform.SetParent(transform);
        resource.transform.position = transform.position + _offset;

        _mover.SetTarget(_bot.CoreBuilding.transform);
    }
}
