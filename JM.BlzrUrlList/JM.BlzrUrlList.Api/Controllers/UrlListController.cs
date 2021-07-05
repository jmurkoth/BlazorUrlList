using JM.BlzrUrList.Core.Helper;
using JM.BlzrUrlList.Core.Repository;
using JM.BlzrUrlList.Exceptions;
using JM.BlzrUrlList.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JM.BlzrUrlList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlListController : ControllerBase
    {
        private readonly IUrlRepository _urlReposity;
        private readonly ILogger<UrlListController> _logger;
        private readonly IHttpContextAccessor _context;
        static readonly string[] SCOPE_NEEDED_FOR_READ = new string[] { "api_read" };
        static readonly string[] SCOPE_NEEDED_FOR_DELETE_WRITE = new string[] { "api_edit" };
        static readonly string[] APPROLE_FOR_DELETE_WRITE = new string[] { "urllist_editor" };

        public UrlListController(IUrlRepository urlReposity, ILogger<UrlListController> logger, IHttpContextAccessor context)
        {
            this._urlReposity = urlReposity;
            this._logger = logger;
            this._context = context;
        }

        // GET api/<UrlListController>/5
        [HttpGet("{urlId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(typeof(UrlList))]
        public async Task<ActionResult<UrlList>> Get(string urlId)
        {
            try
            {
                var matchingItem = await _urlReposity.Get(urlId.ToLower());
                return Ok(matchingItem);
            }
            catch (UrlListNotFoundException ex)
            {

                return NotFound(ex.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Getting Data");
            }
        }
        [Authorize]
        [HttpGet("users/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(typeof(UrlList))]
        public async Task<ActionResult<UrlList>> GetForUser(string userId)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(SCOPE_NEEDED_FOR_READ);
            //TODO:Verify that it is the user's before returning?
            try
            {
                var matchingItems = await _urlReposity.GetListForUser(userId);
                return Ok(matchingItems);
            }
            catch (UrlListNotFoundException ex)
            {

                return NotFound(ex.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Getting Data");
            }
        }

        [Authorize]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(typeof(UrlList))]
        public async Task<ActionResult> Put([FromBody] UrlList urlList)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(SCOPE_NEEDED_FOR_DELETE_WRITE);
            HttpContext.ValidateAppRole(APPROLE_FOR_DELETE_WRITE);
            if (urlList == null || !urlList.Validate())
            {
                return BadRequest();
            }
            if (!await ValidateCreateUser(urlList.UrlId))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "Error Deleting Data");
            }

            // Save the urlbundle
            urlList.SetUrlId();
            var savedList = await _urlReposity.Save(urlList);
            return Accepted($"/{savedList.UrlId}", savedList);

        }

        // POST api/<UrlListController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(typeof(UrlList))]
        public async Task<ActionResult> Post([FromBody] UrlList urlList)
        {

            // Save the urlbundle use post only for addition
            if (urlList != null && urlList.Validate() )
            {
                urlList.SetUrlId();
                var savedList = await _urlReposity.Save(urlList);
                return Created($"/{savedList.UrlId}", savedList);
            }
            return BadRequest();
        }

        // DELETE api/<UrlListController>/5
        [Authorize]
        [HttpDelete("{urlId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(typeof(UrlList))]
        public async Task<ActionResult> Delete(string urlId)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(SCOPE_NEEDED_FOR_DELETE_WRITE);
            HttpContext.ValidateAppRole(APPROLE_FOR_DELETE_WRITE);

            try
            {
                if(! await ValidateCreateUser(urlId))
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, "Error Deleting Data");    
                }
                var deletedItm= await _urlReposity.Delete(urlId);
                return Ok(deletedItm);
            }
            catch (UrlListNotFoundException ex)
            {

                return NotFound( ex.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Deleting Data");
            }
            
        }

        private async Task<bool> ValidateCreateUser(string urlId)
        {
            bool isUserMatch=true;
            var matchin= await _urlReposity.Get(urlId);
            var loggedinUserId=_context.HttpContext.User?.FindFirst(c=> c.Type== ClaimTypes.Upn)?.Value;
            if(matchin!=null && matchin?.UserId!= loggedinUserId)
            {
                _logger.LogError($"context user id is:{loggedinUserId}");
                 isUserMatch=false;
            }
            return isUserMatch;
        }
    }
}
