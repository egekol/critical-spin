using CardGame.Model.Spin;

namespace CardGame.EventBus.Spin
{
    public struct SpinRestartSignal
    {
        public readonly CardGameZoneModel ZoneModel;

        public SpinRestartSignal(CardGameZoneModel zoneModel)
        {
            ZoneModel = zoneModel;
        }
    }
}