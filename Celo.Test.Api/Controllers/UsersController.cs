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
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IImageService _imageService;
        private readonly ILogger _logger;

        /// <summary>
        /// Instantiates the users controller with the service object and a logger injected by DI.
        /// </summary>
        public UsersController(
            IUserService userService, 
            IImageService imageService,
            ILogger<UsersController> logger) : base()
        {
            _userService = userService;
            _imageService = imageService;
            _logger = logger;
        }

        // GET: /api/users/{id}
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUser([FromRoute]Guid id)
        {
            try
            {
                var users = await _userService.GetUsersAsync(new UserFilter
                {
                    Id = id
                });

                var user = users.Results.FirstOrDefault();
                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting user.");
                // Should be a 500 error
                throw;
            }
        }

        // GET: /api/users?filter.namecontains=nathan%20dunn
        // GET: /api/users?filter.firstnameequals=nathan
        // GET: /api/users?filter.lastnameequals=dunn
        // GET: /api/users?filter.firstnameequals=nathan&lastnameequals=dunn
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetUsers([FromQuery]UserFilter filter)
        {
            try
            {
                var users = await _userService.GetUsersAsync(filter);
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting users.");
                // Should be a 500 error
                throw;
            }
        }

        // POST: /api/users
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddUser([FromBody]User model)
        {
            try
            {
                // TODO: Basic validation done here but expect more validation at lower levels in real life
                if (model != null && ModelState.IsValid)
                {
                    var id = await _userService.AddUserAsync(model);
                    return CreatedAtAction(nameof(GetUser), new { id }, new { id });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating user.");
                // Should be a 500 error
                throw;
            }

            return BadRequest(ModelState);
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

        // DELETE: /api/users/{id}
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute]Guid id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting user.");
                // Should be a 500 error
                throw;
            }
        }

        // GET: /api/users/{id}/profile-thumbs
        [HttpGet]
        [Route("{id}/profile-thumbs")]
        public async Task<IActionResult> GetUserProfileThumbnails([FromRoute]Guid id)
        {
            try
            {
                var results = await _imageService.GetImagesAsync(new ImageFilter
                {
                    UserId = id,
                    IsThumbnail = true
                });
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving user profiel thumbnails.");
                // Should be a 500 error
                throw;
            }
        }

        [HttpPost]
        [Route("{id}/profile-images")]
        public async Task<IActionResult> UploadImage([FromForm]IFormFile file, [FromRoute]Guid id, [FromForm]Image image)
        {
            try
            {
                using (var s = file.OpenReadStream())
                {
                    if (image != null)
                    {
                        image.UserId = id;
                    }

                    var imageId = await _imageService.AddImageAsync(image, s);
                    return CreatedAtAction(
                        nameof(ImagesController.GetImage),
                        nameof(ImagesController).Replace("Controller", ""),
                        new { id = imageId }, new { id = imageId });
                }
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.ValidationResult);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while uploading user profile images.");
                // Should be a 500 error
                throw;
            }
        }
    }
}