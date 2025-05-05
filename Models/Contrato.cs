using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaAluguel.Models
{
    public class Contrato
    {
        public int Id { get; set; }
        public int ImovelId { get; set; }
        public int InquilinoId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public decimal ValorMensal { get; set; }
        public bool Ativo { get; set; }


        public Inquilino? Inquilino { get; set; }
        public Imovel? Imovel { get; set; }
    }
}