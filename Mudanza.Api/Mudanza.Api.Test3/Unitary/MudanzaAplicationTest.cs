using Microsoft.AspNetCore.Http;
using Moq;
using Mudanza.Api.Aplication.Dto;
using Mudanza.Api.Aplication.Main;
using Mudanza.Api.Infraestructure.Interface;
using Mudanza.Api.Test3.BaseTest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Mudanza.Api.Test3.Unitary
{
    public class MudanzaAplicationTest: ClaseBaseTest
    {
        IFormFile _archivo;
        Mock<ILogRespository> _mockLogRepository;
        MudanzaAplication _mudanza;
        String _traza;
        String[] _datos;

        public MudanzaAplicationTest()
        {
            _archivo = ContruirIFormFile();
            _mockLogRepository = new Mock<ILogRespository>();
            _mudanza = new MudanzaAplication(_mockLogRepository.Object);
            _traza = "Case #1: 2\nCase #2: 1\nCase #3: 2\nCase #4: 3\nCase #5: 8\n";
            _datos = ("5\n4\n30\n30\n1\n1\n3\n20\n20\n20\n11\n1\n2\n3\n4\n5\n6\n7\n8\n9\n10\n11\n6\n9\n19\n29\n39\n49\n59\n10\n32\n56\n76\n8\n44\n60\n47\n85\n71\n91").Split("\n");

        }

        [Fact]
        public void ProcesarDatosTest()
        {
            // Arrange            
             var estructura = _mudanza.LeerArregloDatos(_datos);
            
            // Act
            var resultado = _mudanza.ProcesarDatos(estructura);

            // Assert
            Assert.Equal(_traza, resultado);
        }

        [Fact]
        public void ProcesarDatosInvalidoTest()
        {   
            // Act
            var resultado = _mudanza.ProcesarDatos(null);

            // Assert
            Assert.Equal("Error al procesar los datos.", resultado);
        }

        [Fact]
        public async Task ValidarDatosAsyncTest()
        {
            // Arrange            
            var datos = new MudanzaDto { Cedula = "1065", Archivo = _archivo };

            // Act
            var resultado = await _mudanza.ValidarDatosAsync(datos);

            // Assert
            Assert.False(resultado.Error);
            Assert.Empty(resultado.ErrorMensaje);
        }

        [Fact]
        public async Task ValidarDatosErrorAsyncTest()
        {
            // Arrange
            var datos = new MudanzaDto { Cedula = "1065", Archivo = null };

            // Act
            var resultado = await _mudanza.ValidarDatosAsync(datos);

            // Assert
            Assert.True(resultado.Error);
            Assert.NotEmpty(resultado.ErrorMensaje);
        }

        [Fact]
        public async Task CargarArchivoAsyncTest()
        {           
            // Act
            var resultado = await _mudanza.CargarArchivoAsync(_archivo);

            // Assert
            Assert.Equal(_datos.Length, resultado.Length);
        }

        [Fact]
        public async Task ValidarDatosArchivoAsyncTest()
        {
            // Act
            var resultado = await _mudanza.ValidarDatosArchivoAsync(_archivo);

            // Assert
            Assert.Empty(resultado);
        }

        [Fact]
        public async Task ValidarDatosArchivoAsyncErrorTest()
        {
            // Act
            var resultado = await _mudanza.ValidarDatosArchivoAsync(null);

            // Assert
            Assert.NotEmpty(resultado);
        }
    }
}
