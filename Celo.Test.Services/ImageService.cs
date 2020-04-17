using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celo.Test.Data.Models;
using Celo.Test.Data.Repositories;
using Celo.Test.Services.Exceptions;
using Celo.Test.Services.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Celo.Test.Services
{
    public class ImageService : IImageService
    {
        private readonly IUserRepository _userRepository;
        private readonly IImageRepository _imageRepository;
        public ImageService(
            IUserRepository userRepository,
            IImageRepository imageRepository)
        {
            _userRepository = userRepository;
            _imageRepository = imageRepository;
        }

        public async Task<Guid> AddImageAsync(Image image, Stream imageStream)
        {
            if (image == null)
            {
                throw new ArgumentException(nameof(image));
            }
            if (imageStream == null || imageStream.Length < 1)
            {
                throw new ValidationException("There is no image data.");
            }

            // Copy to byte array
            // Assume the original stream is handled by caller
            using (var ms = new MemoryStream())
            {
                await imageStream.CopyToAsync(ms);
                image.Blob = ms.ToArray();
            }

            image.Id = Guid.NewGuid();

            // Ensure the user exists
            var user = await _userRepository.Get().Where(i => i.Id == image.UserId).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new ValidationException("The user with the specified UserId does not exist.");
            }

            _imageRepository.Add(image);
            await _imageRepository.SaveAsync();

            return image.Id;
        }

        public async Task DeleteImageAsync(Guid id)
        {
            var existing = await _imageRepository.Get().Where(i => i.Id == id).FirstOrDefaultAsync();
            if (existing == null)
            {
                throw new NotFoundException();
            }

            _imageRepository.Delete(existing);
            await _imageRepository.SaveAsync();
        }

        public async Task<PageResult<Image>> GetImagesAsync(ImageFilter filter)
        {
            var query = _imageRepository.Get();
            if (filter?.Id != null)
            {
                query = query.Where(i => i.Id == filter.Id.Value);
            }
            // In real life you may want to enforce some sort of user filter to reduce breach of security break
            if (filter?.UserId != null)
            {
                query = query.Where(i => i.UserId == filter.UserId.Value);
            }
            if (filter?.IsThumbnail != null)
            {
                query = query.Where(i => i.IsThumbnail == filter.IsThumbnail.Value);
            }

            var result = await query
                .OrderBy(i => i.UserId)
                .ThenBy(i => i.IsThumbnail)
                .ThenBy(i => i.DisplayOrder)
                .GetPage(filter)
                .MaterializeAsync();
            return result;
        }
    }
}
