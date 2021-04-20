namespace Nebularium.Tiamat.Validacoes
{
    public class ErroValidacao
    {
        public ErroValidacao(string nomePropriedade, string mensagem)
        {
            NomePropriedade = nomePropriedade;
            Mensagem = mensagem;
        }

        public string NomePropriedade { get; set; }
        public string Mensagem { get; set; }

        public override string ToString()
        {
            return $"{NomePropriedade} - {Mensagem}";
        }
    }
}
