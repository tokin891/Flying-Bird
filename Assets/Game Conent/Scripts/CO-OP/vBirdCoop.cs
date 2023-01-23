using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player.Coop
{
    using Movement;

    public class vBirdCoop : MonoBehaviour
    {
        [HideInInspector]
        public BirdMovement birdMv;

        private void Awake()
        {
            birdMv = GetComponentInChildren<BirdMovement>();
        }
    }
}
