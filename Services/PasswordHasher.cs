namespace SistemaAluguel.Services
{
    public class PasswordHasher
    {
       public string Hash(string senha)
       {
            return BCrypt.Net.BCrypt.HashPassword(senha);
       }

       public bool Verify(string senha, string hash )
       {
            return BCrypt.Net.BCrypt.Verify(senha, hash);

       } 
    }
}