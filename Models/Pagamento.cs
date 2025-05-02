using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaAluguel.Models
{
    public class Pagamento
    {
        public int Id { get; set; }
        public int ContratoId{ get; set; }
        public DateTime MesAnoReferencia{ get; set; }
        public DateTime DataVencimento{ get; set; }
        public decimal Valor{get; set; }
        public DateOnly? DataPagamento{get; set;}   //O PAGAMENTO PODE NAO TER SIDO EFETUADO POR ISSO O NULLABLE '?'
        public StatusPagamento StatusPagamento{ get; set; }
    }
}