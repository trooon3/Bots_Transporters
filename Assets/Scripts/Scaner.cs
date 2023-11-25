using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaner : MonoBehaviour
{
    private string _tag = "Resource";

    public List<Resource> Scan() 
    { 
        GameObject[] findOblects = GameObject.FindGameObjectsWithTag(_tag);

        List<Resource> resources = new List<Resource>();

        foreach (var findobject in findOblects)
        {
            if (findobject.TryGetComponent(out Resource resource))
            {
                resources.Add(resource);
                resource.IsReserved = false;
            }
        }

        return resources;
    }
}
