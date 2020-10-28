using Nebularium.Behemoth.Mongo.Configuracoes;
using Nebularium.Behemoth.Mongo.Mapeamento;
using System;

namespace Nebularium.Tellurian.Mock
{
    [NomeColecao("PessoasTeste")]
    public class PessoaMProxy : EntidadeMapeamento
    {
        public string NomeSobrenome { get; set; }
        public Genero Genero { get; set; }
        public DateTime Nascimento { get; set; }
        //public List<Endereco> Enderecos { get; set; }
    }
}
