
namespace SistemaAluguel.DTOs
{
    public class ImovelDTO
    {
        public int Id { get; set; }
        public string? Endereco { get; set;}
        public string? Tipo {get; set;}
        public decimal ValorAluguel {get;set;}
        public bool Disponivel {get; set;}

    
    }
}