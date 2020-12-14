using System;

namespace Nebularium.Tiamat.Validacoes
{
    public class ErroValidacao
    {
        public string Mensagem { get; set; }
        public string NomePropriedade { get; set; }

        public override string ToString()
        {
            return $"{NomePropriedade} - {Mensagem}";
        }
    }
}
