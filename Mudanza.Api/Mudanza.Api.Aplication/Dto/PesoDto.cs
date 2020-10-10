using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Mudanza.Api.Aplication.Dto
{
    public class PesoDto : IComparable<PesoDto>
    {
        public PesoDto(int value) { Peso = value; }
        public int Peso { get; private set; }
        public int CompareTo(PesoDto other)
        {
            return Peso.CompareTo(other.Peso);
        }
    }
}
