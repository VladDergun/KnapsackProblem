namespace RucksackAlgorithm
{
    public class RandomNumberGenerator
    {
        public static int GetRandomNumber(int min, int max)
        {
            return new System.Random().Next(min, max);
        }
     
        public static int GetChance(int chance)
        {
            return new System.Random().Next(1, 101) <= chance ? 1 : 0;
        }
    }
}
