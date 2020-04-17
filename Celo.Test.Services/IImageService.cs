using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Celo.Test.Data;
using Celo.Test.Data.Models;

namespace Celo.Test.Services
{
    public interface IImageService
    {
        /// <summary>
        /// Add an image.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public Task<Guid> AddImageAsync(Image image, Stream imageStream);

        /// <summary>
        /// Delete an image.
        /// </summary>
        public Task DeleteImageAsync(Guid id);

        /// <summary>
        /// Get images.
        /// </summary>
        public Task<PageResult<Image>> GetImagesAsync(ImageFilter filter);
    }
}
