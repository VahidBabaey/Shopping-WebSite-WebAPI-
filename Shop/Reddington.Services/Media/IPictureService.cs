using System.Threading.Tasks;
using Reddington.Services.DTOs;

namespace Reddington.Service.Media
{
    public interface IPictureService
    {
        Task<bool> CheckExists(int ID);
        Task<PictureDTO> RegisterBase64PictureAsync(PictureUploadBase64DTO pictureUploadDTO);
        Task<PictureDTO> RegisterPictureAsync(PictureUploadDTO image);
        Task<PictureDTO> SearchPictureByIdAsync(int id);
    }
}