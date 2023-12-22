using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FlagController : MonoBehaviour
{
    private CoreBilding _coreBilding;

    void Start()
    {
        _coreBilding = null;
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.TryGetComponent(out CoreBilding coreBilding) && Input.GetKeyDown(KeyCode.Mouse0))
            {
                _coreBilding = coreBilding;
                _coreBilding.CreateFlag();
            }

            if (hit.transform.gameObject.TryGetComponent(out Map map) && Input.GetKeyDown(KeyCode.Mouse0))
            {

                if (_coreBilding == null)
                {
                    return;
                }

                _coreBilding.SetFlagPosition(hit.point);
            }
        }
    }
}
