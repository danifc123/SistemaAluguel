using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaAluguel.Models
{
    public class Imovel
    {
        public int Id {get; set; }
        public string?  Endereco{get; set; }
        public string? Tipo{get; set; }
        public decimal ValorAluguel{get; set; }
        public bool Disponivel{get; set;}
    }
}