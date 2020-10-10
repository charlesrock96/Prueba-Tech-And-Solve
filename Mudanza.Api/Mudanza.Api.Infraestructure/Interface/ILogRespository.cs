using Mudanza.Api.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mudanza.Api.Infraestructure.Interface
{
    public interface ILogRespository
    {
        public Task<bool> InsertAsync(TblLog log);
    }
}
