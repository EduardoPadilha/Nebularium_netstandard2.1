using Nebularium.Tiamat.Enumeracoes;
using System;

namespace Nebularium.Tiamat.Entidades
{
    public class Metadado
    {
        public Metadado()
        {
            DataCriacao = DateTimeOffset.UtcNow;
        }

        public bool Ativo { get; set; }
        public DateTimeOffset DataCriacao { get; set; }
        public DateTimeOffset? DataAtualizacao { get; set; }
        public DateTimeOffset? DataDelecao { get; set; }
        public virtual SituacaoEntidade Situacao
        {
            get
            {
                if (DataDelecao.HasValue) return SituacaoEntidade.Deletado;

                return Ativo ? SituacaoEntidade.Ativo : SituacaoEntidade.Inativo;
            }
        }
    }
}
