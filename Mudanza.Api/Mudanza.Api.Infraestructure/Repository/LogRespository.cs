using Mudanza.Api.Domain.Entity;
using Mudanza.Api.Infraestructure.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Mudanza.Api.Infraestructure.Repository
{
    public class LogRespository : ILogRespository
    {
        private readonly DbContextMudanza _context;

        public LogRespository(DbContextMudanza context)
        {
            _context = context;
        }

        public async Task<bool> InsertAsync(TblLog log)
        {
            try
            {
                await _context.AddAsync(log);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
    }
}
