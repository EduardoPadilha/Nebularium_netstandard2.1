using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tiamat.Filtros;
using System;
using System.Linq;

namespace Nebularium.Tellurian.Mock
{
    public class PessoaFiltro : FiltroAbstrato<Pessoa>
    {
        public string Cidade { get; set; }
        public int Cep { get; set; }
        public Genero? Genero { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public PessoaFiltro()
        {
            AdicionarRegra(c => c.NomeSobrenome.Contains(Criterio.NomeSobrenome))
                .SobCondicional(c => !Criterio.NomeSobrenome.LimpoNuloBranco());
            AdicionarRegra(c => c.Genero == Genero)
                .SobCondicional(c => Genero != null);
            AdicionarRegra(c => c.Enderecos != null && c.Enderecos.Any(e => e.Cidade.ToLower().Contains(Cidade.ToLower())))
                .SobCondicional(c => !Cidade.LimpoNuloBranco());
            AdicionarRegra(c => c.Enderecos != null && c.Enderecos.Any(e => e.Cep == Cep))
                .SobCondicional(c => Cep != 0);
            AdicionarRegra(c => c.Nascimento >= DataInicio && c.Nascimento <= DataFim)
                .SobCondicional(c => DataInicio.HasValue && DataInicio.Value != default && DataFim.HasValue && DataFim.Value != default);
        }
    }
}
