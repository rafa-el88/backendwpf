using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DATA.Domain.Models
{
    // Classe base para todos os modelos deste projeto teste (Apesar de ter somente o User neste caso)
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime ModificadoEm { get; set; }
    }
}
