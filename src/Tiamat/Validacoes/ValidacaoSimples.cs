using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tiamat.Abstracoes;
using System;
using System.Collections.Generic;

namespace Nebularium.Tiamat.Validacoes
{
    public class ValidacaoSimples
    {
        public ValidacaoSimples(string campo, string mensagem, Func<bool> validar, bool bloqueante = false, bool notificar = true)
        {
            Campo = campo;
            Mensagem = mensagem;
            Validar = validar;
            Bloqueante = bloqueante;
            Notificar = notificar;
        }

        public string Campo { get; }
        public string Mensagem { get; }
        public Func<bool> Validar { get; }
        public bool Notificar { get; set; }
        public bool Bloqueante { get; set; }
    }

    public class ValidadorSimples
    {
        private readonly List<ValidacaoSimples> validacaoes;
        public Action<List<ValidacaoSimples>> EventoFalhaValidacao { get; set; }
        public bool TemValidacao => validacaoes.AnySafe();

        public ValidadorSimples()
        {
            validacaoes = new List<ValidacaoSimples>();
        }

        public static ValidadorSimples Iniciar()
        {
            return new ValidadorSimples();
        }

        public ValidadorSimples Add(ValidacaoSimples validacao)
        {
            if (validacao != null)
                validacaoes.Add(validacao);
            return this;
        }

        public ValidadorSimples Add(string campo, string mensagem, Func<bool> validar, bool bloqueante = false, bool notificar = true)
        {
            if (!string.IsNullOrEmpty(campo) && !string.IsNullOrEmpty(mensagem) && validar != null)
                validacaoes.Add(new ValidacaoSimples(campo, mensagem, validar, bloqueante, notificar));
            return this;
        }

        public ValidadorSimples AddSemNotificacao(Func<bool> validar, bool bloqueante = false)
        {
            Add(string.Empty, string.Empty, validar, bloqueante, false);
            return this;
        }

        public bool Validar(IContextoNotificacao contextoNotificacao = null)
        {
            var valido = true;
            var erros = new List<ValidacaoSimples>();
            foreach (var validacao in validacaoes)
            {
                if (!validacao.Validar.Invoke())
                {
                    valido = false;
                    erros.Add(validacao);
                    if (contextoNotificacao != null && validacao.Notificar)
                        contextoNotificacao.AddNotificacao(validacao.Campo, validacao.Mensagem);
                    if (validacao.Bloqueante)
                        break;
                }
            }

            if (EventoFalhaValidacao != null && !valido)
                EventoFalhaValidacao.Invoke(erros);

            return valido;
        }
    }
}
