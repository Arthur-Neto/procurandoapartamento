// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProcurandoApartamento.Domain;
using ProcurandoApartamento.Domain.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProcurandoApartamento.Infrastructure.Data
{
    public class ApplicationDatabaseContext : DbContext
    {
        public DbSet<Apartamento> Apartamentos { get; set; }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _env;

        public ApplicationDatabaseContext(DbContextOptions<ApplicationDatabaseContext> options, IHttpContextAccessor httpContextAccessor, IHostingEnvironment env)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _env = env;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Rename AspNet default tables
            builder.Entity<Apartamento>().ToTable("Apartamento");

            if (_env.IsDevelopment())
            {
                builder.Entity<Apartamento>().HasData(
                    //Quadra 1
                    new Apartamento
                    {
                        Id = 1,
                        Quadra = 1,
                        ApartamentoDisponivel = true,
                        EstabelecimentoExiste = true,
                        Estabelecimento = "ACADEMIA",
                    },
                    new Apartamento
                    {
                        Id = 2,
                        Quadra = 1,
                        ApartamentoDisponivel = true,
                        EstabelecimentoExiste = false,
                        Estabelecimento = "ESCOLA",
                    },
                    new Apartamento
                    {
                        Id = 3,
                        Quadra = 1,
                        ApartamentoDisponivel = true,
                        EstabelecimentoExiste = true,
                        Estabelecimento = "MERCADO",
                    },
                    //Quadra 2
                    new Apartamento
                    {
                        Id = 4,
                        Quadra = 2,
                        ApartamentoDisponivel = true,
                        EstabelecimentoExiste = true,
                        Estabelecimento = "ACADEMIA",
                    },
                    new Apartamento
                    {
                        Id = 5,
                        Quadra = 2,
                        ApartamentoDisponivel = true,
                        EstabelecimentoExiste = false,
                        Estabelecimento = "ESCOLA",
                    },
                    new Apartamento
                    {
                        Id = 6,
                        Quadra = 2,
                        ApartamentoDisponivel = true,
                        EstabelecimentoExiste = false,
                        Estabelecimento = "MERCADO",
                    },
                    //Quadra 3
                    new Apartamento
                    {
                        Id = 7,
                        Quadra = 3,
                        ApartamentoDisponivel = true,
                        EstabelecimentoExiste = false,
                        Estabelecimento = "ACADEMIA",
                    },
                    new Apartamento
                    {
                        Id = 8,
                        Quadra = 3,
                        ApartamentoDisponivel = true,
                        EstabelecimentoExiste = false,
                        Estabelecimento = "ESCOLA",
                    },
                    new Apartamento
                    {
                        Id = 9,
                        Quadra = 3,
                        ApartamentoDisponivel = true,
                        EstabelecimentoExiste = false,
                        Estabelecimento = "MERCADO",
                    },
                    //Quadra 4
                    new Apartamento
                    {
                        Id = 10,
                        Quadra = 4,
                        ApartamentoDisponivel = true,
                        EstabelecimentoExiste = false,
                        Estabelecimento = "ACADEMIA",
                    },
                    new Apartamento
                    {
                        Id = 11,
                        Quadra = 4,
                        ApartamentoDisponivel = true,
                        EstabelecimentoExiste = true,
                        Estabelecimento = "ESCOLA",
                    },
                    new Apartamento
                    {
                        Id = 12,
                        Quadra = 4,
                        ApartamentoDisponivel = true,
                        EstabelecimentoExiste = false,
                        Estabelecimento = "MERCADO",
                    },
                    //Quadra 5
                    new Apartamento
                    {
                        Id = 13,
                        Quadra = 5,
                        ApartamentoDisponivel = true,
                        EstabelecimentoExiste = false,
                        Estabelecimento = "ACADEMIA",
                    },
                    new Apartamento
                    {
                        Id = 14,
                        Quadra = 5,
                        ApartamentoDisponivel = true,
                        EstabelecimentoExiste = false,
                        Estabelecimento = "ESCOLA",
                    },
                    new Apartamento
                    {
                        Id = 15,
                        Quadra = 5,
                        ApartamentoDisponivel = true,
                        EstabelecimentoExiste = true,
                        Estabelecimento = "MERCADO",
                    }
                );
            }
        }

        /// <summary>
        /// SaveChangesAsync with entities audit
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var entries = ChangeTracker
              .Entries()
              .Where(e => e.Entity is IAuditedEntityBase && (
                  e.State == EntityState.Added
                  || e.State == EntityState.Modified));

            string modifiedOrCreatedBy = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "System";

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    ((IAuditedEntityBase)entityEntry.Entity).CreatedDate = DateTime.Now;
                    ((IAuditedEntityBase)entityEntry.Entity).CreatedBy = modifiedOrCreatedBy;
                }
                else
                {
                    Entry((IAuditedEntityBase)entityEntry.Entity).Property(p => p.CreatedDate).IsModified = false;
                    Entry((IAuditedEntityBase)entityEntry.Entity).Property(p => p.CreatedBy).IsModified = false;
                }
              ((IAuditedEntityBase)entityEntry.Entity).LastModifiedDate = DateTime.Now;
                ((IAuditedEntityBase)entityEntry.Entity).LastModifiedBy = modifiedOrCreatedBy;
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
