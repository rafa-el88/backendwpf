using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace DATA.Domain.Models
{
    public class User : BaseEntity
    {
        [Required(ErrorMessage = "Preenchimento do nome é obrigatório")]
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Telefone { get; set; }
    }
}
