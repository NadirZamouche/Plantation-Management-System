namespace Identity.Models.Repositories
{
    public class PlantRepository : IIdentityRepository<Plant>
    {
        List<Plant> plants;

        public PlantRepository()
        {
            plants = new List<Plant>()
            {
                new Plant{Id = 1, Name ="1", Weight=30, HarvestDate= new DateTime(2023, 9, 2), State=true, Verification= true, Remarks="", Greenhouse = new Greenhouse(), Alley = new Alley()},
                new Plant{Id = 2, Name ="2", Weight=30, HarvestDate= new DateTime(2023, 9, 2), State=true, Verification= false, Remarks="", Greenhouse = new Greenhouse(), Alley = new Alley()},
                new Plant{Id = 3, Name ="3", Weight=30, HarvestDate= new DateTime(2023, 9, 2), State=false, Verification= false, Remarks="", Greenhouse = new Greenhouse(), Alley = new Alley()},
                new Plant{Id = 4, Name ="5", Weight=30, HarvestDate= new DateTime(2023, 9, 2), State=false, Verification= true, Remarks="", Greenhouse = new Greenhouse(), Alley = new Alley()},
            };
        }

        public void Add(Plant entity)
        {
            entity.Id = plants.Max(b => b.Id) + 1;
            plants.Add(entity);
        }

        public void Delete(int id)
        {
            var plant = Find(id);

            plants.Remove(plant);
        }

        public Plant Find(int id)
        {
            var plant = plants.SingleOrDefault(b => b.Id == id);

            return plant;
        }

        public IList<Plant> List()
        {
            return plants;
        }

        public void Update(int id, Plant newPlant)
        {
            var plant = Find(id);
            plant.Name = newPlant.Name;
            plant.Weight = newPlant.Weight;
            plant.HarvestDate = newPlant.HarvestDate;
            plant.State = newPlant.State;
            plant.Verification = newPlant.Verification;
            plant.Remarks = newPlant.Remarks;
            plant.Greenhouse = newPlant.Greenhouse;
            plant.Alley = newPlant.Alley;
        }

        public IList<Plant> Search(string term)
        {
            throw new NotImplementedException();
        }
    }
}