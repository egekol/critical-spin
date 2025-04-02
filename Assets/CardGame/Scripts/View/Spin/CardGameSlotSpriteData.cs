using System;
using CardGame.Model.Spin;
using UnityEngine;

namespace CardGame.View.Spin
{
    [Serializable]
    public class CardGameSlotSpriteData
    {
        public ZoneType ZoneType;
        public Sprite SlotBase;
        public Sprite SlotIndicator;
    }
}