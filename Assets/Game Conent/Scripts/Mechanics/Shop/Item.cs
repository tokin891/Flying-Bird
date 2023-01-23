using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Shop.Item
{
    [CreateAssetMenu(fileName ="New Item", menuName ="Shop/Item")]
    public class Item : ScriptableObject
    {
        // States
        [Header("Details")]
        public int ID;
        public string ObjectName;
        public string ObjectDesriptions;
        public TypePower _objectPower = new TypePower();

        [Header("Visual Details")]
        public Sprite ObjectSprite;
    }

    public enum TypePower
    {
        SlowTime10sec,
        DobuleLife,
        TripleLife,
        SpeedUp10sec
    }
}
