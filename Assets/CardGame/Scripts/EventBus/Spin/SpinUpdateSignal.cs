using CardGame.Model.Spin;

namespace CardGame.EventBus
{
    public struct SpinUpdateSignal
    {
        public readonly CardGameZoneModel ZoneModel;

        public SpinUpdateSignal(CardGameZoneModel zoneModel)
        {
            ZoneModel = zoneModel;
        }
    }
}