using Identity.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Identity.Models.Repositories
{
    public class PlanningDbRepository : IIdentityRepository<Planning>
    {
        ApplicationDbContext db;

        public PlanningDbRepository(ApplicationDbContext _db)
        {
            db = _db;
        }

        public void Add(Planning entity)
        {
            db.Plannings.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var planning = Find(id);
            db.Plannings.Remove(planning);
            db.SaveChanges();
        }

        public Planning Find(int id)
        {
            var planning = db.Plannings.Include(a => a.Alley).Include(a => a.User).SingleOrDefault(b => b.Id == id);
            return planning;
        }

        public IList<Planning> List()
        {
            return db.Plannings.Include(a => a.Alley).Include(a => a.User).ToList();
        }

        public void Update(int id, Planning newPlanning)
        {
            db.Plannings.Update(newPlanning);
            db.SaveChanges();
        }

        public IList<Planning> Search(string term)
        {
            // Convert the term string to a DateTime
            DateTime termValue2 = DateTime.MinValue;
            if (DateTime.TryParseExact(term, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime value2))
            {
                termValue2 = value2;
            }

            var result = db.Plannings.Include(a => a.Alley).Include(a => a.User)
            .Where(b => b.Alley.Name.Contains(term)
                || b.User.FullName.Contains(term)
                || b.Order.Contains(term)
                || b.PlanDate.Date.Equals(termValue2)).ToList();
            return result;
        }
    }
}