namespace RucksackAlgorithm
{
    public class Rucksack
    {
        public int TotalWeight  { get; private set; } = 150;

        public List<RucksackItem> CreateRuckSackItems(int count)
        {
            var items = new List<RucksackItem>();
            for (int i = 0; i < count; i++)
            {
                items.Add(RucksackItem.CreateRandomItem(i));
            }
            return items;
        }
    }
}
