// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ProcurandoApartamento.Web.Rest.Utilities
{
    public static class ActionResultUtil
    {
        public static ActionResult WrapOrNotFoundAsDto<TDtoType>(object value, IMapper mapper)
        {
            if (value != null)
            {
                var resultAsDto = mapper.Map<TDtoType>(value);
                return new OkObjectResult(resultAsDto);
            }
            return new NotFoundResult();
        }


        public static ActionResult WrapOrNotFound(object value)
        {
            return value != null ? new OkObjectResult(value) : new NotFoundResult();
        }
    }
}
