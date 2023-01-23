using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Trap
{
    using Managers;

    public class Barrier : MonoBehaviour
    {
        [SerializeField] float _setSpeed;
        [SerializeField] bool isBuilding;

        private void Awake()
        {
            if(isBuilding == false)
                _setSpeed = vGameManager._instance.GetSpeedWall();
        }
        private void FixedUpdate()
        {
            if(vGameManager._instance._statsManager != eStatsManager.Dead)
            {
                if (isBuilding == false)
                {
                    transform.position += Vector3.back * vGameManager._instance.GetSpeedWall();
                }
                else
                    transform.position += Vector3.back * _setSpeed * vGameManager._instance.GetPercentagesWall();
            }
        }
    }
}