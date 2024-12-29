
namespace RucksackAlgorithm
{
    public class RucksackItem
    {
        public int Id { get; private set; }
        public int Value { get; private set; }
        public int Weight { get; private set; }

        public RucksackItem(int id,int value, int weight)
        {
            Id = id;
            Value = value;
            Weight = weight;
        }

        public static RucksackItem CreateRandomItem(int id)
        {
            return new RucksackItem(
                id: id,
                value: RandomNumberGenerator.GetRandomNumber(2, 10),
                weight: RandomNumberGenerator.GetRandomNumber(1, 5)
            );
        }
    }
}
