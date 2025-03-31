using System.Collections.Generic;
using CardGame.Model;
using UnityEngine;

namespace CardGame.View
{
    public interface ICardGameSceneView
    {
        void SetSpinSlotView(CardGameZoneModel zoneModelList);
        // void SetSpriteCache(IRewardViewIconSpriteCache cache);
    }

    public class CardGameSceneView : MonoBehaviour, ICardGameSceneView
    {
        [SerializeField] private CardGameSpinView _cardGameSpinView;
        
        public void SetSpinSlotView(CardGameZoneModel zoneModelList)
        {
            _cardGameSpinView.SetSpinSlots(zoneModelList);

        }

        // public void SetSpriteCache(IRewardViewIconSpriteCache cache)
        // {
        //     _cardGameSpinView.SetSpriteCache();
        // }
    }
}