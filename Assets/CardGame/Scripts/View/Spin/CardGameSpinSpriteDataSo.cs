using System;
using CardGame.Model.Spin;
using UnityEngine;

namespace CardGame.View.Spin
{
    [CreateAssetMenu(fileName = "so_cardGame_spin_sprite_", menuName = "SO/CardGameSpinSpriteDataSo", order = 0)]
    public class CardGameSpinSpriteDataSo : ScriptableObject
    {
        public ZoneType ZoneType;
        public Sprite SlotBase;
        public Sprite SlotIndicator;
    }
}