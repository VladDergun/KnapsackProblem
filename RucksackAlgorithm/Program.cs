using RucksackAlgorithm;



const int ITEM_COUNT = 100;

var rucksack = new Rucksack();


var items = rucksack.CreateRuckSackItems(ITEM_COUNT);
var population = new Population(items);
population.CreatePopulationEntities(100, ITEM_COUNT);
population.AddItemsToInitialPopulation(items);

population.Evolve();








