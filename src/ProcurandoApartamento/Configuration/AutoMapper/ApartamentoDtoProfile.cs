// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using ProcurandoApartamento.Domain;
using ProcurandoApartamento.Dto;

namespace ProcurandoApartamento.Configuration.AutoMapper
{
    public class ApartamentoDtoProfile : Profile
    {
        public ApartamentoDtoProfile()
        {
            CreateMap<Apartamento, ApartamentoDto>()
                .ForMember(x => x.Quadra, opt => opt.MapFrom(s => s.Quadra));
        }
    }
}
