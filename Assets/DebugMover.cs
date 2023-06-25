using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMover : MonoBehaviour
{
    //settings
    [SerializeField] float _moveSpeed = 3f;


    //state
    

    

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * _moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * _moveSpeed * Time.deltaTime;
        }
    }
}
