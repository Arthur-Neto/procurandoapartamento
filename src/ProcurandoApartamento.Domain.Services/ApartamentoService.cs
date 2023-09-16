// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using JHipsterNet.Core.Pagination;
using LanguageExt;
using ProcurandoApartamento.Domain.Repositories.Interfaces;
using ProcurandoApartamento.Domain.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcurandoApartamento.Domain.Services
{
    public class ApartamentoService : IApartamentoService
    {
        protected readonly IApartamentoRepository _apartamentoRepository;

        public ApartamentoService(IApartamentoRepository apartamentoRepository)
        {
            _apartamentoRepository = apartamentoRepository;
        }

        public virtual async Task<Apartamento> Save(Apartamento apartamento)
        {
            await _apartamentoRepository.CreateOrUpdateAsync(apartamento);
            await _apartamentoRepository.SaveChangesAsync();
            return apartamento;
        }

        public virtual async Task<IPage<Apartamento>> FindAll(IPageable pageable)
        {
            var page = await _apartamentoRepository.QueryHelper()
                .GetPageAsync(pageable);
            return page;
        }

        public virtual async Task<Apartamento> FindOne(long id)
        {
            var result = await _apartamentoRepository.QueryHelper()
                .GetOneAsync(apartamento => apartamento.Id == id);
            return result;
        }

        public virtual async Task Delete(long id)
        {
            await _apartamentoRepository.DeleteByIdAsync(id);
            await _apartamentoRepository.SaveChangesAsync();
        }

        public virtual async Task<Apartamento> EncontrarPorEstabelecimentos(List<string> estabelecimentos)
        {
            var apartamentos = await _apartamentoRepository.QueryHelper().GetAllAsync();

            if (estabelecimentos.Any() == false)
            {
                return apartamentos.LastOrDefault();
            }

            var result = apartamentos
                .Where(ap => ap.EstabelecimentoExiste)
                .GroupBy(ap => ap.Quadra)
                .Filter(apartamentos =>
                {
                    return apartamentos.All(apartamento => estabelecimentos.Contains(apartamento.Estabelecimento));
                })
                .FirstOrDefault()
                .OrderByDescending(ap => ap.Quadra)
                .FirstOrDefault();

            return result;
        }
    }
}
