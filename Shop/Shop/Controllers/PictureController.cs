using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reddington.Framework;
using Reddington.Service.Media;
using Reddington.Services.DTOs;


namespace Shop.Controllers
{
    public class PictureController : ReddingtonController
    {
        private readonly IPictureService _pictureService=null;

        public PictureController(IPictureService pictureService)
        {
            _pictureService = pictureService;
        }

        //[HttpGet("{id}")]
        //public IActionResult Get(int? id)
        //{
        //    var file = Path.Combine(Directory.GetCurrentDirectory(),
        //                            "wwwroot", "PImages", id + ".png");

        //    if (!System.IO.File.Exists(file))
        //        return NotFound();

        //    return PhysicalFile(file, "image/png");
        //}


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(int? id)
        {
            if (!await _pictureService.CheckExists(id.Value))
            {
                return NotFound();
            }
            var image = await _pictureService.SearchPictureByIdAsync(id.Value);

            return PhysicalFile(image.VirtualPath, image.MimeType);
        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> UploadAsync([FromForm]PictureUploadDTO image)
        {
            


            image.ContentType = image.File.ContentType;
            image.fileExtension = Path.GetExtension(image.File.FileName); 

            var pictureDTO = await _pictureService.RegisterPictureAsync(image);

            return CreatedAtAction("Get", new { id = pictureDTO.ID }, pictureDTO.ID);
        }




        [HttpPost("Base64")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> UploadBase64Async([FromForm]PictureUploadBase64DTO pictureUploadBase64DTO)
        {
            if (string.IsNullOrEmpty(pictureUploadBase64DTO.File))
                return BadRequest();

            var format = pictureUploadBase64DTO.File.Split(",")[0];

           try
            {
               var buffer= Convert.FromBase64String(pictureUploadBase64DTO.File.Split(",")[1]);
                MemoryStream memory = new MemoryStream(buffer);
                System.Drawing.Image.FromStream(memory);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }


            pictureUploadBase64DTO.File = pictureUploadBase64DTO.File.Split(",")[1];

            var ImageContentType = new List<string>
            {
                "image/jpg",
                "image/jpeg",
                "image/gif",
                "image/png",
            } as IReadOnlyCollection<string>;

            var ImageExtension = new List<string>
            {
                ".jpg",
                ".png",
                ".gif",
                ".jpeg",
            } as IReadOnlyCollection<string>;

            var contentType = ImageContentType.FirstOrDefault(p => format.Contains(p));

            if (contentType == null)
                return BadRequest();


            var fileExtension = ImageExtension.FirstOrDefault(p => contentType.Contains(p.Replace(".","")));

            if (!string.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();


            pictureUploadBase64DTO.ContentType = contentType;
            pictureUploadBase64DTO.fileExtension = fileExtension;


            var pictureDTO = await _pictureService.RegisterBase64PictureAsync(pictureUploadBase64DTO);


            return CreatedAtAction("Get", new { id = pictureDTO.ID }, pictureDTO.ID);
        }
    }
}