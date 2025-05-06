namespace SistemaAluguel.DTOs
{
    public class PagamentoDTO
    {
        public int Id {get; set;}
        public int ContratoId {get; set;}
        public DateTime MesAnoReferencia {get; set;}
        public DateTime DataVencimento {get; set;}
        public DateOnly? DataPagamento {get; set;}
        public decimal Valor {get; set;}
        public string Status { get; set; } = string.Empty;

    }
}