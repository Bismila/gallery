using Gallery.BAL.DTO;
using Gallery.BAL.DTO.ImagesDto;
using System.Collections.Generic;

namespace Gallery.BAL.Interfaces
{
    public interface IImageService : IBaseService<CreateUpdateDto>
    {
        bool IsUniqName(string albumName);

        IEnumerable<CreateUpdateImageDto> GetAllElementsFromUser(long userId);

        IEnumerable<CreateUpdateImageDto> GetAllElementsFromFriendsAndUser(long userId);

        void Create(CreateUpdateImageDto element);

        void Update(CreateUpdateImageDto element);

        CreateUpdateImageDto GetOneImage(long id);

        CreateUpdateDto Get(long id, long currentUserId);
    }
}
