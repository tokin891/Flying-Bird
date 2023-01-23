using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Shop.Selector
{
    using Item;

    public class Selector : MonoBehaviour
    {
        [Header("Add Item To Selector")]
        [SerializeField] Item[] addTheseItems;
        [SerializeField] ShopItem prefabItem;

        private List<ShopItem> CurrentItem = new List<ShopItem>();

        private void Awake()
        {
            RefreshValueShop();
        }

        public void RefreshValueShop()
        {
            if(CurrentItem.Count > 0)
            {
                // Clear
                for (int i = 0; i < CurrentItem.Count; i++)
                {
                    Destroy(CurrentItem[i].gameObject);
                }
            }
            // Continue Refresh
            // Add Value
            for (int i = 0; i < addTheseItems.Length; i++)
            {
                GameObject itemS = new GameObject("Shop ID " + addTheseItems[i].ID.ToString());
                itemS.transform.SetParent(transform);
                ShopItem sh = itemS.AddComponent<ShopItem>();
                itemS.AddComponent<UnityEngine.UI.Image>();
                sh.SetupInfo(addTheseItems[i]);
                CurrentItem.Add(sh);
            }
        }
    }
}
