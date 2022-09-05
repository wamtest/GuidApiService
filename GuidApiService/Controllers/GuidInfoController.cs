using GuidApiService.Models;
using GuidApiService.Services;
using Microsoft.AspNetCore.Mvc;

namespace GuidApiService.Controllers
{
    /// <summary>
    /// The guid info controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GuidInfoController : ControllerBase
    {
        private readonly IGuidService _guidService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="guidService"></param>
        public GuidInfoController(IGuidService guidService)
        {
            _guidService = guidService;
        }

        /// <summary>
        /// Read: Returns the metadata associated with given guid
        /// </summary>
        /// <param name="guid"></param>
        /// <remarks>Read</remarks>
        [HttpGet("/guid/{guid}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GuidInfoOutput>> Read(string guid)
        {
            if (!_guidService.IsValidGuid(guid))
            {
                return Problem("Invalid input guid");
            }

            GuidInfoOutput guidInfoOutput = await _guidService.Get(guid);

            if (guidInfoOutput == null)
            {
                return NotFound();
            }

            return guidInfoOutput;
        }

        /// <summary>
        /// Create: Create new guid with given parameters
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="expire"></param>
        /// <param name="user"></param>
        /// <remarks>Create</remarks>
        [HttpPost("/guid/{guid}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GuidInfoOutput>> Create(string guid, long expire, string user)
        {
            if (!_guidService.IsValidGuid(guid))
            {
                return Problem("Invalid input guid");
            }
            if (!_guidService.IsValidUser(user))
            {
                return Problem("User name cant be empty");
            }
            if (!_guidService.IsExpiryValid(expire) || _guidService.IsExpiryInPast(expire))
            {
                return Problem("Invalid expiry");
            }

            GuidInfoOutput guidInfoOutput = await _guidService.Create(guid, expire, user);

            return CreatedAtAction("Create", guidInfoOutput);
        }

        /// <summary>
        /// Create: Create new guid
        /// </summary>
        /// <param name="user"></param>
        /// <remarks>Create</remarks>
        [HttpPost("/guid")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GuidInfoOutput>> Create(string user)
        {
            GuidInfoOutput guidInfoOutput = await _guidService.Create(user);

            return CreatedAtAction("Create", guidInfoOutput);
        }

        /// <summary>
        /// Update: Update metadata associated with given guid
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="expire"></param>
        /// <param name="user"></param>
        /// <remarks>Update</remarks>
        [HttpPut("/guid/{guid}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<GuidInfoOutput>> Update(string guid, long expire, string user)
        {
            if (!_guidService.IsValidGuid(guid))
            {
                return Problem("Invalid input guid");
            }
            if (_guidService.IsExpiryValid(expire) && _guidService.IsExpiryInPast(expire))
            {
                return Problem("Expiry cant be in the past");
            }
            if (!_guidService.IsValidUser(user) && !_guidService.IsExpiryValid(expire))
            {
                return Problem("At least expire or user name are required");
            }

            GuidInfoOutput guidInfoOutput = await _guidService.Update(guid, expire, user);

            return CreatedAtAction("Update", guidInfoOutput);
        }

        /// <summary>
        /// Delete: Delete guid and associated data
        /// </summary>
        /// <param name="guid"></param>
        [HttpDelete("guid/{guid}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(string guid)
        {
            if (!_guidService.IsValidGuid(guid))
            {
                return Problem("Invalid input guid");
            }

            var didRemove = await _guidService.Delete(guid);
            if (!didRemove)
            {
                return NotFound();
            }
            else
            {
                return NoContent();
            }
        }
    }
}
