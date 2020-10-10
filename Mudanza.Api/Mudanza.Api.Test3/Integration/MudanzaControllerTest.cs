using Xunit;
using Moq;
using Mudanza.Api.Test3.BaseTest;
using System.Threading.Tasks;
using Mudanza.Api.Aplication.Dto;
using Mudanza.Api.Aplication.Interface;
using Mudanza.Api.Core.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Mudanza.Api.Test3.Integration
{
   
    public class MudanzaControllerTest: ClaseBaseTest
    {
        [Fact]
        public async Task EjecutarProcesoMudanzaTest()
        {
            // Arrange
            var archivo = ContruirIFormFile();
            var mudanza = new MudanzaDto { Cedula = "1065", Archivo = archivo };
            var ResultadoProceso = new ResultadoDto { Salida = "Case #1: 2\nCase #2: 1\nCase #3: 2\nCase #4: 3\nCase #5: 8\n" };

            var mockMudanzaAplication = new Mock<IMudanzaAplication>();
            mockMudanzaAplication.Setup(x => x.EjecutarProcesoMudanzaAsync(mudanza))
                .ReturnsAsync(ResultadoProceso);

            MudanzaController controllador = new MudanzaController(mockMudanzaAplication.Object);

            // Act
            var resultado = await controllador.PostAsync(mudanza);

            // Assert
            var okObjectResult = resultado as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);

            var presentations = okObjectResult.Value as ResultadoDto;
            Assert.Equal(presentations.Salida, ResultadoProceso.Salida);
        }

        [Fact]
        public async Task EjecutarProcesoMudanzaInvalidTest()
        {
            // Arrange
            var archivo = ContruirIFormFile();
            var mudanza = new MudanzaDto { Archivo = null };
            var ResultadoProceso = new ResultadoDto { Error = true };

            var mockMudanzaAplication = new Mock<IMudanzaAplication>();
            mockMudanzaAplication.Setup(x => x.EjecutarProcesoMudanzaAsync(mudanza))
                .ReturnsAsync(ResultadoProceso);

            MudanzaController controllador = new MudanzaController(mockMudanzaAplication.Object);

            // Act
            var resultado = await controllador.PostAsync(mudanza);

            // Assert
            var okObjectResult = resultado as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);

            var presentations = okObjectResult.Value as ResultadoDto;
            Assert.True(presentations.Error);
        }
    }
}
