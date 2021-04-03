using Nebularium.Tiamat.Abstracoes;
using Nebularium.Tiamat.Validacoes;
using System.Collections.Generic;

namespace Nebularium.Tiamat.Notificacao
{
    public class ContextoNotificacao : IContextoNotificacao
    {
        private readonly Dictionary<string, string> notificacoes;

        public ContextoNotificacao()
        {
            notificacoes = new Dictionary<string, string>();
        }
        public bool TemNotificacao => (notificacoes?.Count ?? 0) > 0;

        public void AddNotificacao(string chave, string mensagem)
        {
            notificacoes.Add(chave, mensagem);
        }

        public Dictionary<string, string> ObterNotificacoes()
        {
            return notificacoes;
        }

        public void AddErros(List<ErroValidacao> erros)
        {
            foreach (var erro in erros)
                if (!notificacoes.ContainsKey(erro.NomePropriedade))
                    notificacoes.Add(erro.NomePropriedade, erro.Mensagem);
        }

        public void AddErros(List<ValidacaoSimples> erros)
        {
            foreach (var erro in erros)
                if (!notificacoes.ContainsKey(erro.Campo))
                    notificacoes.Add(erro.Campo, erro.Mensagem);
        }
    }
}
