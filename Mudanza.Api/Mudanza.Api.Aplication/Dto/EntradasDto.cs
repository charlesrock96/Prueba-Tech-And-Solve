using System;
using System.Collections.Generic;
using System.Text;

namespace Mudanza.Api.Aplication.Dto
{
    public class EntradasDto
    {
        public int NumeroDias  { get; set; }
        public List<ElementosDto> LstElementosPorDia { get; set; }
    }
}
