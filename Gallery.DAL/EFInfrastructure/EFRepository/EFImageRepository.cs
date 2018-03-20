using Gallery.DAL.IRepository;
using Gallery.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.DAL.EFInfrastructure.EFRepository
{
    public class EFImageRepository : IImageRepository
    {
        private readonly DbContext _context;
        private readonly IDbSet<Image> _imageSet;
        private readonly IDbSet<User> _userSet;
        private readonly IDbSet<Friend> _friendSet;

        public EFImageRepository(DbContext _context)
        {
            this._context = _context;
            _imageSet = _context.Set<Image>();
            _userSet = _context.Set<User>();
            _friendSet = _context.Set<Friend>();
        }

        public void Create(Image element)
        {
            var image = new Image()
            {
                Name = element.Name,
                ImageDate = element.ImageDate,
                PathImage = element.PathImage,
                UserId = element.UserId,
            };

            _imageSet.Add(image);

            _context.Entry(image).State = EntityState.Added;
            _context.SaveChanges();

        }

        public void Delete(long id)
        {
            var image = _imageSet.Find(id);

            _context.Entry(image).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public Image Get(long id)
        {
            return _imageSet.Find(id);
        }

        public IEnumerable<Image> GetAllElements()
        {
            return _imageSet.ToList();
        }

        public void Update(Image element)
        {
            var image = _imageSet.Find(element.Id);
            _context.Entry(image).CurrentValues.SetValues(element);
            _context.SaveChanges();
        }

        public IEnumerable<Image> GetAllElementsFromUser(long userId)
        {
            var allImg = _imageSet.ToList().Where(i => i.UserId == userId).ToList();
            return allImg;
        }

        public IEnumerable<Image> GetAllImagesFromFriends(long Id)
        {
            var allImages = _imageSet.Join(_friendSet,
                                                i => i.UserId,
                                                f => f.FriendId,
                                                (i, f) => new { i, f })
                                                .Join(_userSet,
                                                im => im.i.UserId,
                                                 u => u.Id,
                                                 (im, u) => new { im, u })
                                                 .Where(img => img.im.f.UserId == Id)
                                                 .Select(image => image.im.i).Take(12).ToList();

            return allImages;
        }


    }
}
