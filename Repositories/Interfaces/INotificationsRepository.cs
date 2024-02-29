using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface INotificationsRepository
    {
        Task<IEnumerable<T>> GetAsync<T>() where T : class;       
        Task<bool> SaveAsync<T>(T item) where T : class;
    }
}
