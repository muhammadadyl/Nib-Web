using Nib.Career.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nib.Career.Core.Services
{
    public interface IService<T> where T : BaseEntity
    {
        Task<IList<T>> GetAsync();
    }
}