namespace DoubTech.AnimalAI.Consumables
{
    public interface IConsumer
    {
        public bool CanConsume(IConsumable consumable);
    }
}