namespace SistemaAluguel.DTOs
{
    public class ContratoDTO
    {
        public int Id { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public decimal ValorMensal { get; set; }
        public bool Ativo { get; set; }

        public string? NomeInquilino { get; set; } = string.Empty;
        public string? EnderecoImovel { get; set; } = string.Empty;
    }
}
