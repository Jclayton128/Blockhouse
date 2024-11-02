using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweepHandler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ActorController.Instance.SweepObject(collision.transform.root.gameObject);
    }
}
