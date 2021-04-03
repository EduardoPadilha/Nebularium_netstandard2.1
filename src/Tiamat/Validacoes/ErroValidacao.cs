namespace Nebularium.Tiamat.Validacoes
{
    public class ErroValidacao
    {
        public string NomePropriedade { get; set; }
        public string Mensagem { get; set; }

        public override string ToString()
        {
            return $"{NomePropriedade} - {Mensagem}";
        }
    }
}
