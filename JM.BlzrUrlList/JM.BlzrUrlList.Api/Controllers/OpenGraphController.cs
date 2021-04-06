using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenGraphNet;
using JM.BlzrUrlList.Core.Repository;
using System.Net.Mime;

namespace JM.BlzrUrlList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class OpenGraphController : ControllerBase
    {
        private readonly IOpenGraphRepository _openGraphRepository;

        public OpenGraphController(IOpenGraphRepository openGraphRepository)
        {
            this._openGraphRepository = openGraphRepository;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> UriMetadata(string urlId)
        {
                var result = await _openGraphRepository.GetInfo(urlId);
                return Ok(result);
        }

    }
}
