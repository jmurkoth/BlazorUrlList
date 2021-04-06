using JM.BlzrUrlList.Core.Repository;
using JM.BlzrUrlList.Exceptions;
using JM.BlzrUrlList.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JM.BlzrUrlList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlListController : ControllerBase
    {
        private readonly IUrlRepository _urlReposity;

        public UrlListController(IUrlRepository urlReposity)
        {
            this._urlReposity = urlReposity;
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
                var matchingItem = await _urlReposity.Get(urlId);
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

        // POST api/<UrlListController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(typeof(UrlList))]
        public async Task<ActionResult> Post([FromBody] UrlList urlList)
        {

            // Save the urlbundle
            var savedList= await _urlReposity.Save(urlList);
            return Created($"/{savedList.UrlId}", savedList);
        }

        // DELETE api/<UrlListController>/5
        [HttpDelete("{urlId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(typeof(UrlList))]
        public async Task<ActionResult> Delete(string urlId)
        {
            try
            {
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
    }
}
