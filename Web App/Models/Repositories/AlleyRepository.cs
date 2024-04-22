namespace Identity.Models.Repositories
{
    public class AlleyRepository : IIdentityRepository<Alley>
    {
        List<Alley> alleys;

        public AlleyRepository()
        {
            alleys = new List<Alley>()
            {
                new Alley{Id = 1, Name ="1"},
                new Alley{Id = 2, Name ="2"},
                new Alley{Id = 3, Name ="3"},
                new Alley{Id = 4, Name ="4"},
            };
        }

        public void Add(Alley entity)
        {
            entity.Id = alleys.Max(b => b.Id) + 1;
            alleys.Add(entity);
        }

        public void Delete(int id)
        {
            var alley = Find(id);

            alleys.Remove(alley);
        }

        public Alley Find(int id)
        {
            var alley = alleys.SingleOrDefault(b => b.Id == id);

            return alley;
        }

        public IList<Alley> List()
        {
            return alleys;
        }

        public void Update(int id, Alley newAlley)
        {
            var alley = Find(id);
            alley.Name = newAlley.Name;
        }

        public IList<Alley> Search(string term)
        {
            throw new NotImplementedException();
        }
    }
}