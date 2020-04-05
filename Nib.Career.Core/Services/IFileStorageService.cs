using Nib.Career.Core.Entities;

namespace Nib.Career.Core.Services
{
    public interface IFileStorageService<T> : IService<T> where T : BaseEntity
    {
    }
}
