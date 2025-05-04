using CardGame.Model.Spin;

namespace CardGame.EventBus.Spin
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