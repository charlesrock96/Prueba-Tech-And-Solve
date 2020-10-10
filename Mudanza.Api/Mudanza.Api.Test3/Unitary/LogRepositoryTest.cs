using Mudanza.Api.Domain.Entity;
using Mudanza.Api.Infraestructure.Repository;
using Mudanza.Api.Test3.BaseTest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Mudanza.Api.Test3.Unitary
{
    public class LogRepositoryTest : ClaseBaseTest
    {
        [Fact]
        public async Task InsertAsync()
        {
            // Arrange
            var nombreDb = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombreDb);

            var log = new LogRespository(context);
            var datos = new TblLog {
                Cedula = "1065",
                Fecha = DateTime.Now,
                Traza = "Case #1: 2\nCase #2: 1\nCase #3: 2\nCase #4: 3\nCase #5: 8\n"
            };
            
            // Act
            var resultado = await log.InsertAsync(datos);

            // Assert
            Assert.True(resultado);
        }
    }
}
