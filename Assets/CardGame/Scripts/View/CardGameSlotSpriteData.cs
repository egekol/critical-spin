using System;
using CardGame.Model;
using UnityEngine;

namespace CardGame.View
{
    [Serializable]
    public class CardGameSlotSpriteData
    {
        public ZoneType ZoneType;
        public Sprite SlotBase;
        public Sprite SlotIndicator;
    }

}