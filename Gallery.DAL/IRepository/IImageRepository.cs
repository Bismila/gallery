using Gallery.DAL.Models;
using System;
using System.Collections.Generic;

namespace Gallery.DAL.IRepository
{
    public interface IImageRepository : IBaseRepository<Image>
    {
        IEnumerable<Image> GetAllImagesFromFriends(long Id);
        IEnumerable<Image> GetAllElementsFromUser(long userId);
    }
}
