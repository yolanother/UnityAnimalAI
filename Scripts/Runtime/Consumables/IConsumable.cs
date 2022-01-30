namespace DoubTech.AnimalAI.Consumables
{
    public interface IConsumable
    {
        public string ConsumableType { get; }
        bool IsConsumed { get; }
    }
}