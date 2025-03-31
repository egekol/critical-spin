using System.Collections.Generic;
using CardGame.Model;
using UnityEngine;

namespace CardGame.View
{
    public interface ICardGameSceneView
    {
        void SetSpinSlotView(CardGameZoneModel zoneModelList);
    }

    public class CardGameSceneView : MonoBehaviour, ICardGameSceneView
    {
        [SerializeField] private CardGameSpinView _cardGameSpinView;
        
        public void SetSpinSlotView(CardGameZoneModel zoneModelList)
        {
            _cardGameSpinView.SetSpinSlots(zoneModelList);

        }
    }
}