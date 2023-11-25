using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Resource : MonoBehaviour
{
    public bool IsTaked { get; set; }
    public bool IsReserved { get; set; } = false;

    public void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
