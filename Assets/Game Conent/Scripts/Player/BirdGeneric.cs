using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game.Player
{
    namespace GenericBird
    {
        using EnumsBird;
        using Shop.Item;
        using Movement;

        public abstract class BirdGeneric : MonoBehaviour
        {
            [Header("Stats Bird")]
            public BirdDetails _vBirdDetails;
            public eBirdState _eBirdStats;

            private Item _currentItem;

            #region Function
            #region State
            public bool TryToJump()
            {
                if (!CanJump())
                    return false;

                // Do Jump

                GetComponent<Rigidbody>().AddForce(Vector3.up * _vBirdDetails.JumpForce *55, ForceMode.Acceleration);
                return true;
            }
            private bool CanJump()
            {
                if (_eBirdStats != eBirdState.Fly)
                    return false;

                return true;
            }

            public bool TryToUsePower()
            {
                if (!CanPower())
                    return false;

                UsePower(_currentItem);
                return true;
            }
            private bool CanPower()
            {
                if (_currentItem == null)
                    return false;

                return true;
            }
            #endregion

            public virtual void SetupCurrentItem(Item _item)
            {
                _currentItem = _item;
            }
            public void SwitchState(eBirdState _state)
            {
                _eBirdStats = _state;            
            }
            #endregion

            #region Abstract
            public abstract void UsePower(Item index);
            #endregion
        }

        [Serializable]
        public class BirdDetails
        {
            // Informatios
            public float JumpForce { get; set; } = 8f;
        }
    }
    namespace EnumsBird
    {
        public enum eBirdState
        {
            Idle,
            Fly,
            Dead
        }
    }
}