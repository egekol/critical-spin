using CardGame.Model.Spin;

namespace CardGame.EventBus
{
    public struct RewardGainedSignal
    {
        public readonly CardGameRewardModel Model;
        public readonly bool IsNewReward;

        public RewardGainedSignal(CardGameRewardModel model, bool isNewReward)
        {
            Model = model;
            IsNewReward = isNewReward;
        }
    }
}