using Identity.Data;

namespace Identity.Models.Repositories
{
    public class GreenhouseDbRepository : IIdentityRepository<Greenhouse>
    {
        ApplicationDbContext db;

        public GreenhouseDbRepository(ApplicationDbContext _db)
        {
            db = _db;
        }

        public void Add(Greenhouse entity)
        {
            db.Greenhouses.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var greenhouse = Find(id);

            db.Greenhouses.Remove(greenhouse);
            db.SaveChanges();
        }

        public Greenhouse Find(int id)
        {
            var greenhouse = db.Greenhouses.SingleOrDefault(b => b.Id == id);

            return greenhouse;
        }

        public IList<Greenhouse> List()
        {
            return db.Greenhouses.ToList();
        }

        public void Update(int id, Greenhouse newGreenhouse)
        {
            db.Greenhouses.Update(newGreenhouse);
            db.SaveChanges();
        }

        public IList<Greenhouse> Search(string term)
        {
            var result = db.Greenhouses.Where(b => b.Name.Contains(term)).ToList();
            return result;
        }
    }
}