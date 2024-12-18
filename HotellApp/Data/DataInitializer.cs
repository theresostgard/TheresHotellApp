using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Data
{
    public class DataInitializer : IDataInitializer
    {
        public void MigrateAndSeed(ApplicationDbContext dbContext)
        {
            dbContext.Database.Migrate();
            //metoder för olika seeds...
            SeedGuests(dbContext);
            dbContext.SaveChanges();
        }

        private void SeedGuests(ApplicationDbContext dbContext)
        {
            //if (!dbContext.County.Any(c => c.Name == "Stockholms län"))
            //{
            //    dbContext.County.Add(new County
            //    {
            //        Name = "Stockholms län",
            //        ContactPerson = "Annie"
            //    });
            //}
            //if (!dbContext.County.Any(c => c.Name == "Uppsalas län"))
            //{
            //    dbContext.County.Add(new County
            //    {
            //        Name = "Uppsalas län",
            //        ContactPerson = "Brand"
            //    });
            //}
        }
    }
}
