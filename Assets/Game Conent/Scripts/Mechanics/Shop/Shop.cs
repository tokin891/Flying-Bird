using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Shop
{
    using Item;

    public class Shop : MonoBehaviour
    {
        public static Shop _instance;

        [Header("Visual")]
        [SerializeField] GameObject windowSetup;

        private Item.Item currentItem;

        private void Awake()
        {
            _instance = this;
        }
        public void StartPreparingBuy(Item.Item index)
        {
            currentItem = index;
            windowSetup.SetActive(true);
        }

        public void JustBuy()
        {
            // But Action
            windowSetup.SetActive(false);
            currentItem = null;
        }
        public void CancelBuy()
        {
            // Cancel Buying
            windowSetup.SetActive(false);
            currentItem = null;
        }
    }
}
