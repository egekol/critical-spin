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
            _cardGameSpinView.SetSpinView(zoneModelList.ZoneType);
            _cardGameSpinView.SetSpinSlots(zoneModelList);
        }

    }
}