using System;
using System.Threading.Tasks;

namespace Nib.Career.Core.Helpers
{
    public interface ISimpleMemoryCache
    {
        Task<T> GetOrCreate<T>(object key, Func<Task<T>> item, int slidingExpiryInMin = 60, int absoluteExpiryInHours = 24);
    }
}