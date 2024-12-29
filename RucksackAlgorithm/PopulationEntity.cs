namespace RucksackAlgorithm
{
    public class PopulationEntity
    {
        public int[] ItemsSelected { get; private set; }
        public int TotalWeight { get; private set; } = 0;
        public int Fitness { get; private set; } = 0;

        public PopulationEntity(int maxItems)
        {
            ItemsSelected = new int[maxItems];
        }

        public void InsertItemWithChance(int id, RucksackItem? item)
        {
            //with the chance of 50% the item will be set to null
            item = RandomNumberGenerator.GetChance(50) == 0 ? null : item;

            ItemsSelected[id] = item != null ? 1 : 0;
            Fitness += item?.Value ?? 0;
            TotalWeight += item?.Weight ?? 0;
        }

        public void InsertItem(int id, RucksackItem item)
        {
            ItemsSelected[id] = 1;
            Fitness += item.Value;
            TotalWeight += item.Weight;
        }

        public void RemoveItem(int id, RucksackItem item)
        {
            ItemsSelected[id] = 0;
            Fitness -= item.Value;
            TotalWeight -= item.Weight;
        }
    }
}
