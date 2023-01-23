using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace Game.Shop.Item
{
    public class ShopItem : MonoBehaviour
    {
        [Header("Visual Objects")]
        private Image _icon;
        private Item indexCurrent;
        private Button newButt;

        private void Awake()
        {
            newButt = gameObject.AddComponent<Button>();
        }
        private void Update()
        {
            newButt.onClick.AddListener(delegate () { Pressing(); });
        }
        public void SetupInfo(Item _index)
        {
            indexCurrent = _index;

            StartCoroutine(setupDetials(indexCurrent));
        }

        private IEnumerator setupDetials(Item ind)
        {
            _icon = GetComponent<Image>();
            if (_icon == null)
                yield return null;

            _icon.sprite = ind.ObjectSprite;
        }

        private void Pressing()
        {
            Shop._instance.StartPreparingBuy(indexCurrent);
        }
    }

}