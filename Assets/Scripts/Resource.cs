using UnityEngine;

public class Resource : MonoBehaviour
{
   public bool IsReserved { get; set; }
   
    public void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
