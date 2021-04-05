using JM.BlzrUrlList.Exceptions;
using JM.BlzrUrlList.Models;
using JM.BlzrUrlList.Models.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public UrlList Get(string urlId)
        {
            return _urlReposity.Get(urlId);
        }

        // POST api/<UrlListController>
        [HttpPost]
        public void Post([FromBody] UrlList urlList)
        {

            // Save the urlbundle
            _urlReposity.Save(urlList);
        }

        // DELETE api/<UrlListController>/5
        [HttpDelete("{urlId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Delete(string urlId)
        {
            try
            {
                _urlReposity.Delete(urlId);
                return Ok();
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
