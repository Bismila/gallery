using System.Collections.Generic;

namespace Gallery.BAL.Interfaces
{
    public interface IBaseService<T>
    {
        void Create(T element);
        void Delete(long id);
        T Get(long id);
        IEnumerable<T> GetAllElements();
        void Update(T element);
    }
}
