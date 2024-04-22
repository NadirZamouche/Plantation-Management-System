namespace Identity.Models.Repositories
{
    public class GreenhouseRepository : IIdentityRepository<Greenhouse>
    {
        List<Greenhouse> greenhouses;

        public GreenhouseRepository()
        {
            greenhouses = new List<Greenhouse>()
            {
                new Greenhouse{Id = 1, Name ="1"},
                new Greenhouse{Id = 2, Name ="2"},
                new Greenhouse{Id = 3, Name ="3"},
                new Greenhouse{Id = 4, Name ="4"},
            };
        }
        public void Add(Greenhouse entity)
        {
            entity.Id = greenhouses.Max(b => b.Id) + 1;
            greenhouses.Add(entity);
        }

        public void Delete(int id)
        {
            var greenhouse = Find(id);
            greenhouses.Remove(greenhouse);
        }

        public Greenhouse Find(int id)
        {
            var greenhouse = greenhouses.SingleOrDefault(b => b.Id == id);

            return greenhouse;
        }

        public IList<Greenhouse> List()
        {
            return greenhouses;
        }

        public void Update(int id, Greenhouse newGreenhouse)
        {
            var greenhouse = Find(id);
            greenhouse.Name = newGreenhouse.Name;
        }

        public IList<Greenhouse> Search(string term)
        {
            throw new NotImplementedException();
        }
    }
}
