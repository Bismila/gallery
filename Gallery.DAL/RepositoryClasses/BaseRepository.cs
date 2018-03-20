using System.Data;

namespace Gallery.DAL.RepositoryClasses
{
    public abstract class BaseRepository
    {
        protected readonly IDbConnection connection;

        public BaseRepository(IDbConnection conn)
        {
            this.connection = conn;
        }
    }
}
