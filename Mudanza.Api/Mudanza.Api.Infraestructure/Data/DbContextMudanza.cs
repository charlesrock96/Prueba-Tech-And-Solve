using Microsoft.EntityFrameworkCore;
using Mudanza.Api.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mudanza.Api.Infraestructure
{
    public class DbContextMudanza: DbContext
    {
        public DbContextMudanza(DbContextOptions<DbContextMudanza> options)
            : base(options) { }

        public DbSet<TblLog> TblLog { get; set; }
    }
}
