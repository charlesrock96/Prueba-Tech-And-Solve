using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using Mudanza.Api.Infraestructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mudanza.Api.Test3.BaseTest
{
    public class ClaseBaseTest
    {
        protected DbContextMudanza ConstruirContext(string nombreDB)
        {
            var opciones = new DbContextOptionsBuilder<DbContextMudanza>()
                .UseInMemoryDatabase(nombreDB).Options;

            return new DbContextMudanza(opciones);
        }

        protected IFormFile ContruirIFormFile()
        {
            var fileMock = new Mock<IFormFile>();
            //Seteamos el archivo mock usando un memory stream 
            var content = "5\n4\n30\n30\n1\n1\n3\n20\n20\n20\n11\n1\n2\n3\n4\n5\n6\n7\n8\n9\n10\n11\n6\n9\n19\n29\n39\n49\n59\n10\n32\n56\n76\n8\n44\n60\n47\n85\n71\n91";
            var fileName = "lazy_loading_example_input.txt";
            var contentType = "text/plain";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            fileMock.Setup(_ => _.ContentType).Returns(contentType);

            return fileMock.Object;
        }
    }
}
