// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProcurandoApartamento.Dto;
using System.Collections.Generic;

namespace ProcurandoApartamento.Controllers
{
    [Route("")]
    [ApiController]
    public class SwaggerController : ControllerBase
    {
        private readonly ILogger<SwaggerController> _log;

        public SwaggerController(ILogger<SwaggerController> log)
        {
            _log = log;
        }

        [HttpGet("swagger-resources")]
        public ActionResult<IEnumerable<SwaggerResourceDto>> GetSwaggerResources()
        {
            _log.LogDebug("REST request to get Swagger Resources");
            SwaggerResourceDto sr1 = new SwaggerResourceDto() { Name = "ProcurandoApartamento", Location = "/v2/api-docs" };
            List<SwaggerResourceDto> swaggerResources = new List<SwaggerResourceDto>
            {
                sr1
            };
            return Ok(swaggerResources);
        }
    }
}
