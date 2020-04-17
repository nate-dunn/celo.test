using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Celo.Test.Data.Models;
using Celo.Test.Services;
using Celo.Test.Services.Exceptions;
using Celo.Test.Services.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Celo.Test.Api.Controllers
{
    [Route("api/images")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IImageService _imageService;
        private readonly ILogger _logger;

        /// <summary>
        /// Instantiates the images controller with the service object and a logger injected by DI.
        /// </summary>
        public ImagesController(
            IUserService userService, 
            IImageService imageService,
            ILogger<UsersController> logger) : base()
        {
            _userService = userService;
            _imageService = imageService;
            _logger = logger;
        }

        // GET: /api/images/{id}
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetImage([FromRoute]Guid id)
        {
            try
            {
                var images = await _imageService.GetImagesAsync(new ImageFilter
                {
                    Id = id
                });

                var image = images.Results.FirstOrDefault();
                if (image == null)
                {
                    return NotFound();
                }

                return Ok(image);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting image.");
                // Should be a 500 error
                throw;
            }
        }

        // GET: /api/images?filter.userid=12345&filter.isthumbnail=true
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetImages([FromQuery]ImageFilter filter)
        {
            try
            {
                var images = await _imageService.GetImagesAsync(filter);
                return Ok(images);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting images.");
                // Should be a 500 error
                throw;
            }
        }

        // PUT: /api/users/{id}
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute]Guid id, [FromBody]User model)
        {
            try
            {
                // TODO: Basic validation done here but expect more validation at lower levels in real life
                if (model != null && ModelState.IsValid)
                {
                    model.Id = id;
                    await _userService.UpdateUserAsync(model);
                    return Ok();
                }
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating user.");
                // Should be a 500 error
                throw;
            }

            return BadRequest(ModelState);
        }

        // DELETE: /api/images/{id}
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteImage([FromRoute]Guid id)
        {
            try
            {
                await _imageService.DeleteImageAsync(id);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting image.");
                // Should be a 500 error
                throw;
            }
        }
    }
}