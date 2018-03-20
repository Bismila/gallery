using System.Collections.Generic;
using System.Linq;
using Gallery.DAL.Models;
using System.Data;
using Dapper;
using Gallery.DAL.IRepository;

namespace Gallery.DAL.RepositoryClasses
{
    public class ImageRepository : BaseRepository, IImageRepository
    {
        public ImageRepository(IDbConnection connection) : base(connection)
        {

        }

        public IEnumerable<Image> GetAllElements()
        {
            List<Image> imagesList = new List<Image>();
            imagesList = connection.Query<Image>("SELECT * FROM Images").ToList();
            return imagesList;
        }
      
        public void Create(Image image)
        {
            var sqlQuery = "INSERT INTO Images (Name, ImageDate, PathImage, UserId) VALUES (@Name, @ImageDate, @PathImage, @UserId); SELECT CAST(SCOPE_IDENTITY() as int)";
            connection.Execute(sqlQuery, image);
        }

        public void Update(Image image)
        {
            var sqlQuery = "UPDATE Images SET Name = @Name, ImageDate = @ImageDate, PathImage = @PathImage, UserId = @UserId WHERE Id = @Id";
            connection.Execute(sqlQuery, image);
        }
        public void Delete(long id)
        {
            var sqlQuery = "DELETE FROM Images WHERE Id = @id";
            connection.Execute(sqlQuery, new { id });
        }

        public Image Get(long id)
        {
            Image image = connection.Query<Image>("SELECT * FROM Images WHERE Id = @id", new { id }).Single(s => s.Id == id);
            return image;
        }      

        public bool ListEquals(List<Image> target, List<Image> source)
        {
            if (target.Count != source.Count)
                return false;
            else
            {
                for (int i = 0; i < target.Count; i++)
                {
                    if (!target[i].Id.Equals(source[i].Id))
                        return false;
                }
                return true;
            }
        }

        public IEnumerable<Image> GetAllElementsFromUser(long userId)
        {
            var sql = @"SELECT * 
                        FROM Images 
                        WHERE UserId = @userId 
                        order by ImageDate DESC";
            List<Image> imagesList = connection.Query<Image>(sql, new { userId }).ToList();
            return imagesList;
        }

        public IEnumerable<Image> GetAllImagesFromFriends(long Id)
        {
            var sql = @"select TOP 12 i.Id, i.UserId, i.ImageDate, u.Login, i.Name, i.PathImage, u.Id, u.Login
                        from Friends f
                        inner join Images i
                        on i.UserId = f.FriendId
                        inner join Users u
                        on u.Id = i.UserId
                        where f.UserId = @Id
                        order by i.ImageDate DESC";
            var images = connection.Query<Image, User, Image>(sql, (i, u) =>
            {
                i.User = u;
                return i;
            }, new { Id });

            return images;
        }
    }
}
