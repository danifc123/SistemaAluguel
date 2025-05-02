using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaAluguel.Models
{
    public class Contrato
    {
        public int Id{get; set;}
        public int ImovelId{get; set;}
        public int InquilinoId{get; set;}
        public DateTime DataInicio{get; set;}
        public DateTime DataTermino{get; set;}
        public decimal ValorAcordado{get; set;}
        public bool StatusContrato{get; set;}


    }
}