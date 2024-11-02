using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.X))
        {
            Debug.Log("Discarding Entire Hand");

        }
      
    }
}
