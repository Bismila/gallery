using Gallery.Api.Models;
using Gallery.BAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Gallery.BAL.DTO;
using Gallery.BAL.DTO.ImagesDto;

namespace Gallery.Api.Controllers
{
    public class ImageController : ApiController
    {
        private readonly IUserService userService;
        private readonly IImageService imageService;

        public ImageController(IUserService userService,
                              IImageService imageService)
        {
            this.userService = userService;
            this.imageService = imageService;
        }

        [Route("api/images/{idUser}")]
        [HttpGet]
        //[System.Web.Mvc.ValidateAntiForgeryToken]
        public IEnumerable<CreateUpdateImageDto> GetImagesForCurrUser(int idUser)
        {
            //long id = idUser;
            var images = imageService.GetAllElementsFromUser(Convert.ToInt64(idUser));
            return images;
        }

    }
}
