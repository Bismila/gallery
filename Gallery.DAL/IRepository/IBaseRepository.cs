using System.Collections.Generic;

namespace Gallery.DAL.IRepository
{

    public interface IBaseRepository<T>
    {
        void Create(T element);
        void Delete(long id);
        T Get(long id);
        IEnumerable<T> GetAllElements();
        void Update(T element);

    }
}
