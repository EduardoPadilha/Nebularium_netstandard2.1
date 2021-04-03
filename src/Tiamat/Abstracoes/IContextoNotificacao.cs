using Nebularium.Tiamat.Validacoes;
using System.Collections.Generic;

namespace Nebularium.Tiamat.Abstracoes
{
    public interface IContextoNotificacao
    {
        Dictionary<string, string> ObterNotificacoes();
        void AddNotificacao(string chave, string mensagem);
        bool TemNotificacao { get; }

        void AddErros(List<ErroValidacao> erros);
        void AddErros(List<ValidacaoSimples> erros);
    }
}
