using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player.Movement
{
    using Game.Player.EnumsBird;
    using Game.Shop.Item;
    using GenericBird;
    using Managers;

    [RequireComponent(typeof(Rigidbody))]
    public class BirdMovement : BirdGeneric
    {
        [Header("Code")]
        [SerializeField]
        BirdObjects _objects;

        [Header("Objects")]
        [SerializeField] Transform[] wings;
        [SerializeField] GameObject prefabAddCoin;

        public PlayerState currentState;

        private void Update()
        {
            _vInputs();

            switch (_eBirdStats)
            {
                case eBirdState.Idle:
                    GetComponent<Rigidbody>().useGravity = false;

                    break;
                case eBirdState.Fly:
                    GetComponent<Rigidbody>().useGravity = true;

                    break;
                case eBirdState.Dead:
                    GetComponent<Rigidbody>().useGravity = false;

                    break;
            }
        }
        public void _vInputs()
        {
            if (_eBirdStats != eBirdState.Fly)
                return;

            if (Input.GetKeyDown(vGameManager._instance._codes.GetPlayerCode(vGameManager._instance._codes._jumps, currentState.indexPlayer)))
            {
                if(TryToJump())
                {
                    _objects._jumpAD.Play();
                }
            }

            #region Tests
            if(Input.GetKeyDown(KeyCode.E))
            {
                TryToUsePower();
            }
            #endregion
        }
        public void SetupPlayer(PlayerState _state)
        {
            currentState = _state;

            Debug.Log(_state);
            Debug.Log(_state.indexPlayer);

            _objects._meshRender.material = vGameManager._instance._codes.GetMaterial(currentState.indexPlayer);
        }

        #region Override
        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.transform.CompareTag("Trap"))
                return;

            vGameManager._instance.LoseGame();
            
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Score"))
            {
                vGameManager._instance.AddScore();
            }
            if (other.CompareTag("coin"))
            {
                AddCoin();
                Destroy(other.gameObject);
            }
        }
        public override void UsePower(Item index)
        {
            vGameManager._instance.UsePower(index);
        }
        public override void SetupCurrentItem(Item _item)
        {
            // Add Some Effects

        }
        #endregion

        public void AddCoin()
        {
            Quaternion nQ = new Quaternion();
            nQ = Quaternion.Euler(0, -90, 0);
            GameObject insCoinPrefab = Instantiate(prefabAddCoin, transform.position, nQ);

            Destroy(insCoinPrefab, 8f);

            //ADD COIN
            vGameManager._instance.AddCoin();
        }
    }
        
    [System.Serializable]
    public class BirdObjects
    {
        public AudioSource _jumpAD;
        public SkinnedMeshRenderer _meshRender;
    }

    #region Player Class
    public abstract class PlayerState
    {
        public int indexPlayer;
    }

    public class PlayerOne : PlayerState { }
    public class PlayerTwo : PlayerState { }
    #endregion
}