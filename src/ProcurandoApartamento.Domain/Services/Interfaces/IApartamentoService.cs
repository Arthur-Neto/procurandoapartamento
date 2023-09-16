// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using JHipsterNet.Core.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProcurandoApartamento.Domain.Services.Interfaces
{
    public interface IApartamentoService
    {
        Task<Apartamento> Save(Apartamento apartamento);

        Task<IPage<Apartamento>> FindAll(IPageable pageable);

        Task<Apartamento> FindOne(long id);

        Task Delete(long id);

        Task<Apartamento> EncontrarPorEstabelecimentos(List<string> estabelecimentos);
    }
}
