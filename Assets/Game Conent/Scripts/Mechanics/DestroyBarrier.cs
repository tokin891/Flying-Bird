using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Trap
{
    public class DestroyBarrier : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Barrier>())
            {
                Destroy(other.gameObject);
            }
        }
    }
}
