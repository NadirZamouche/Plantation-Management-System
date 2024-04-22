using Identity.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Identity.Models.Repositories
{
    public class PlantDbRepository : IIdentityRepository<Plant>
    {
        ApplicationDbContext db;

        public PlantDbRepository(ApplicationDbContext _db)
        {
            db = _db;
        }

        public void Add(Plant entity)
        {
            db.Plants.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var plant = Find(id);
            db.Plants.Remove(plant);
            db.SaveChanges();
        }

        public Plant Find(int id)
        {
            var plant = db.Plants.Include(a => a.Greenhouse).Include(a => a.Alley).SingleOrDefault(b => b.Id == id);
            return plant;
        }

        public IList<Plant> List()
        {
            return db.Plants.Include(a => a.Greenhouse).Include(a => a.Alley).ToList();
        }

        public void Update(int id, Plant newPlant)
        {
            db.Plants.Update(newPlant);
            db.SaveChanges();
        }

        public IList<Plant> Search(string term)
        {
            // Convert the term string to a float
            float? termValue1 = null;
            if (float.TryParse(term, out float value1))
            {
                termValue1 = value1;
            }

            // Convert the term string to a DateTime
            DateTime termValue2 = DateTime.MinValue;
            if (DateTime.TryParseExact(term, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime value2))
            {
                termValue2 = value2;
            }

            // Convert the term string to a bool
            var uppterm = term.ToUpper(); // Convert the term string to uppercase
            bool? termValue3 = null; // Define termValue3 and initialize it to null
            bool? termValue4 = null; // Define termValue4 and initialize it to null
            // assign the termvalue3 & termvalue4 variables a value for each scenario for state & verification
            if (uppterm == "BON")
            {
                termValue3 = true;
            }
            else if (uppterm == "PAS BON")
            {
                termValue3 = false;
            }
            else if (uppterm == "VÉRIFIÉE")
            {
                termValue4 = true;
            }
            else if (uppterm == "PAS VÉRIFIÉE")
            {
                termValue4 = false;
            }

            var result = db.Plants.Include(a => a.Greenhouse).Include(a => a.Alley)
            .Where(b => b.Name.Contains(term)
                || b.Greenhouse.Name.Contains(term)
                || b.Alley.Name.Contains(term)
                || b.Weight.Equals(termValue1)
                || b.HarvestDate.Date.Equals(termValue2)
                || b.State.Equals(termValue3)
                || b.Verification.Equals(termValue4)
                || b.Remarks.Contains(term)).ToList();
            return result;
        }
    }
}