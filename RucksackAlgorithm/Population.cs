namespace RucksackAlgorithm
{
    public class Population
    {
        public List<PopulationEntity> PopulationEntities { get; private set; }
        public List<RucksackItem> RucksackItems { get; private set; }

        public int itteration = 0;
        public int maxPopulation = 600;
        public Population(List<RucksackItem> rucksack)
        {
            PopulationEntities = new List<PopulationEntity>();
            RucksackItems = rucksack;
        }

        public void CreatePopulationEntities(int count, int maxItems)
        {
            for (int i = 0; i < count; i++)
            {
                PopulationEntities.Add(new PopulationEntity(maxItems));
            }
        }

        public void AddItemsToInitialPopulation(List<RucksackItem> items)
        {
            //one entity gets one different item
            for (int i = 0; i < PopulationEntities.Count; i++)
            {
                PopulationEntities[i].InsertItem(i, items[i]);
            }

        }

        public PopulationEntity GetBestEntity()
        {
            return PopulationEntities.OrderByDescending(x => x.Fitness).First();
        }

        public PopulationEntity GetEntityForCrossover()
        {
            // Step 1: Sort entities by fitness, but we don't need to do this anymore for roulette selection.
            var sortedEntities = PopulationEntities;

            // Step 2: Calculate total fitness of the population
            int totalFitness = sortedEntities.Sum(entity => entity.Fitness);

            // Step 3: Generate a random number in the range [0, totalFitness)
            double randomValue = RandomNumberGenerator.GetRandomNumber(0, totalFitness);

            // Step 4: Select the entity based on the random value
            double cumulativeFitness = 0.0;
            foreach (var entity in sortedEntities)
            {
                cumulativeFitness += entity.Fitness;
                if (cumulativeFitness >= randomValue)
                {
                    return entity;
                }
            }

            // If we get here, something went wrong, but we can fallback to the first entity.
            return sortedEntities.First();
        }


        public void LocalOptimization(PopulationEntity entity)
        {
            var willHyperMutate = RandomNumberGenerator.GetChance(10);
            if (willHyperMutate == 1)
            {
                PerformHyperMutation(entity);
            }

        }

        public void PerformHyperMutation(PopulationEntity entity)
        {
            int mutationCount = entity.ItemsSelected.Length / 4;  // 25% of the genome size
            for (int i = 0; i < mutationCount; i++)
            {
                MutateEntity(entity);
            }

            if (entity.TotalWeight > 150)
            {
                RepairEntity(entity);
            }
        }

        public void RepairEntity(PopulationEntity entity)
        {
            // Create a list of selected items with their value-to-weight ratio
            var selectedItems = entity.ItemsSelected
                .Select((isSelected, index) => new { IsSelected = isSelected, Index = index })
                .Where(x => x.IsSelected == 1) // Only consider selected items
                .Select(x => new
                {
                    x.Index,
                    Item = RucksackItems[x.Index],
                    ValueToWeightRatio = (double)RucksackItems[x.Index].Value / RucksackItems[x.Index].Weight
                })
                .OrderBy(x => x.ValueToWeightRatio) // Sort by value-to-weight ratio (ascending)
                .ToList();


            foreach (var item in selectedItems)
            {
                if (entity.TotalWeight <= 150)
                {
                    break;
                }

                entity.RemoveItem(item.Index, item.Item);
            }
        }

        public void MutateEntity(PopulationEntity entity)
        {
            var firstRandomIndex = RandomNumberGenerator.GetRandomNumber(0, entity.ItemsSelected.Length - 1);
            var secondRandomIndex = RandomNumberGenerator.GetRandomNumber(0, entity.ItemsSelected.Length - 1);


            if (firstRandomIndex == secondRandomIndex)
            {
                return;
            }

            var firstItem = RucksackItems[firstRandomIndex];
            var secondItem = RucksackItems[secondRandomIndex];

            if (entity.ItemsSelected[firstRandomIndex] == 1 && entity.ItemsSelected[secondRandomIndex] == 0)
            {
                entity.RemoveItem(firstRandomIndex, firstItem);
                entity.InsertItem(secondRandomIndex, secondItem);
            }
            else if (entity.ItemsSelected[firstRandomIndex] == 0 && entity.ItemsSelected[secondRandomIndex] == 1)
            {
                entity.RemoveItem(secondRandomIndex, secondItem);
                entity.InsertItem(firstRandomIndex, firstItem);
            }
        }


        public void Evolve()
        {
            while (itteration < 20000)
            {
                var bestEntity = GetBestEntity();


                var firstEntity = GetEntityForCrossover();
                var secondEntity = GetEntityForCrossover();

                var newEntity = new PopulationEntity(firstEntity.ItemsSelected.Length);

                for (int i = 0; i < firstEntity.ItemsSelected.Length; i++)
                {
                    var firstOrSecond = RandomNumberGenerator.GetChance(50);
                    if (firstOrSecond == 1 && firstEntity.ItemsSelected[i] == 1)
                    {
                        newEntity.InsertItem(i, RucksackItems[i]);
                    }
                    else if (firstOrSecond == 0 && secondEntity.ItemsSelected[i] == 1)
                    {
                        newEntity.InsertItem(i, RucksackItems[i]);
                    }

                }

                var chanceForMutation = RandomNumberGenerator.GetChance(10);
                if (chanceForMutation == 1)
                {
                    MutateEntity(newEntity);
                }

                LocalOptimization(newEntity);

                if (newEntity.TotalWeight <= 150 && newEntity.Fitness > 0)
                {
                    PopulationEntities.Add(newEntity);
                    if (PopulationEntities.Count > maxPopulation)
                    {
                        PopulationEntities.Remove(PopulationEntities.OrderBy(x => x.Fitness).First());
                    }
                }


                itteration++;
                if (itteration >= 1000 && itteration % 1000 == 0)
                {
                    Console.WriteLine($"Best entity: Fitness: {bestEntity.Fitness} Rucksack weight: {bestEntity.TotalWeight}. Iteration: {itteration}");
                }
            }
        }
    }
}
