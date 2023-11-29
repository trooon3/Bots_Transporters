using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaner : MonoBehaviour
{
    private string _tag = "Resource";

    public Queue<Resource> Scan() 
    { 
        GameObject[] findObjects = GameObject.FindGameObjectsWithTag(_tag);

        Queue<Resource> resources = new Queue<Resource>();

        foreach (var findObject in findObjects)
        {
            if (findObject.TryGetComponent(out Resource resource))
            {
                resources.Enqueue(resource);
            }
        }

        return resources;
    }
}
