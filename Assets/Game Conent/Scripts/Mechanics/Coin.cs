using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] float smooth;
    private void FixedUpdate()
    {
        transform.position += Vector3.up * smooth;
    }
}
