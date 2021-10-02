using Nebularium.Tiamat.Entidades;

namespace Nebularium.Tiamat.Filtros
{
    public class FiltroEntidade<TEntidade> : FiltroAbstrato<TEntidade> where TEntidade : Entidade, new()
    {
        public FiltroEntidade()
        {
            AdicionarRegra(c => !c.Metadado.DataDelecao.HasValue).SobCondicional(c => true);
        }
        public FiltroEntidade(string id) : this()
        {
            AdicionarRegra(c => c.Id == id);
        }
    }

    public class FiltroEntidadeAtivo<TEntidade> : FiltroEntidade<TEntidade> where TEntidade : Entidade, new()
    {
        public FiltroEntidadeAtivo() : base()
        {
            AdicionarRegra(c => c.Metadado.Ativo);
        }
        public FiltroEntidadeAtivo(string id) : this()
        {
            AdicionarRegra(c => c.Id == id);
        }
    }
}
