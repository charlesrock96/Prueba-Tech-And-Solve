using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mudanza.Api.Domain.Entity
{
    public class TblLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Cedula { get; set; }
        public DateTime Fecha { get; set; }
        public string Traza { get; set; }
    }
}
