using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;

namespace Repositories
{
    public class NotificationsRepository(ApgNotificationsDbContext context) : INotificationsRepository
    {
        private readonly ApgNotificationsDbContext context = context;

        public async Task<IEnumerable<T>> GetAsync<T>() where T : class
        {
            try
            {
                return await context.Set<T>().ToListAsync();
            }
            catch (Exception)
            {
                return new List<T>();
            }
            
        }

        public async Task<bool> SaveAsync<T>(T item) where T : class
        {
            try
            {
                context.Set<T>().Add(item);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;                
            }
        }
    }
}
