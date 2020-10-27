using Nebularium.Behemoth.Mongo;
using System;

namespace Nebularium.Tellurian.Mock
{
    [NomeColecao("PessoasTeste")]
    public class PessoaMProxy : EntidadeMProxy
    {
        public string NomeSobrenome { get; set; }
        public Genero Genero { get; set; }
        public DateTime Nascimento { get; set; }
        //public List<Endereco> Enderecos { get; set; }
    }
}
