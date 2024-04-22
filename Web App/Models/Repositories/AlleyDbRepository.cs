using Identity.Data;

namespace Identity.Models.Repositories
{
    public class AlleyDbRepository : IIdentityRepository<Alley>
    {
        ApplicationDbContext db;

        public AlleyDbRepository(ApplicationDbContext _db)
        {
            db = _db;
        }

        public void Add(Alley entity)
        {
            db.Alleys.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var alley = Find(id);
            db.Alleys.Remove(alley);
            db.SaveChanges();
        }

        public Alley Find(int id)
        {
            var alley = db.Alleys.SingleOrDefault(b => b.Id == id);
            return alley;
        }

        public IList<Alley> List()
        {
            return db.Alleys.ToList();
        }

        public void Update(int id, Alley newAlley)
        {
            db.Alleys.Update(newAlley);
            db.SaveChanges();
        }

        public IList<Alley> Search(string term)
        {
            var result = db.Alleys.Where(b => b.Name.Contains(term)).ToList();
            return result;
        }
    }
}
