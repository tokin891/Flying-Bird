using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Trap
{
    using Managers;

    public class SpawnBarrier : MonoBehaviour
    {
        public static SpawnBarrier _instance;
        [SerializeField] bool isBuiling = false;
        [SerializeField] bool spawnRandomPos = false;
        [SerializeField] Vector3 randPosMax;

        [SerializeField]
        protected float delaySpawn;

        [SerializeField]
        protected GameObject[] wall;

        [SerializeField]
        private Transform _spawnWall;

        private float nttf;
        private List<GameObject> _listWall = new List<GameObject>();

        private void Awake()
        {
            _instance = this;
        }
        private void Update()
        {          
            if (vGameManager._instance._statsManager != eStatsManager.Game)
                return;
            if(isBuiling == false)
                delaySpawn = vGameManager._instance.GetDelaySpawn();

            if(Time.time > nttf + delaySpawn)
            {
                nttf = Time.time;
                // Spawn

                SpawnBarrierFunction();
            }
        }       
        
        protected void SpawnBarrierFunction()
        {
            if(spawnRandomPos)
            {
                float rX = Random.Range(-randPosMax.x, randPosMax.x);
                float rY = Random.Range(-randPosMax.y, randPosMax.y);
                float rZ = Random.Range(-randPosMax.z, randPosMax.z);
                Vector3 randPos = new Vector3(rX, rY, rZ);

                GameObject wall_ = Instantiate(getWall(), _spawnWall.position + randPos, _spawnWall.rotation);
                _listWall.Add(wall_);
            }
            else
            {
                GameObject wall_ = Instantiate(getWall(), _spawnWall.position, _spawnWall.rotation);
                _listWall.Add(wall_);
            }    
        }        
        private GameObject getWall()
        {
            int randWall = Random.Range(0, wall.Length);

            return wall[randWall];
        }

        public void DestroyAllBarrier()
        {
            if (vGameManager._instance._statsManager != eStatsManager.Game)
            {
                if (_listWall.Count != 0)
                {
                    for (int i = 0; i < _listWall.Count; i++)
                    {
                        if (_listWall[i] != null)
                        {
                            Destroy(_listWall[i]);
                        }
                    }
                }
            }
        }
    }
}
