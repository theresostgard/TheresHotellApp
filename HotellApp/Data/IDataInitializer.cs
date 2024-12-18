namespace HotellApp.Data
{
    public interface IDataInitializer
    {
        void MigrateAndSeed(ApplicationDbContext dbContext);
    }
}