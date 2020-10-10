using System;
using System.Collections.Generic;
using System.Text;

namespace Mudanza.Api.Aplication.Dto
{
    public class ElementosDto
    {
        public int NumeroElementosPorDia { get; set; }
        public List<PesoDto> LstElementosACargar { get; set; }
    }
}
