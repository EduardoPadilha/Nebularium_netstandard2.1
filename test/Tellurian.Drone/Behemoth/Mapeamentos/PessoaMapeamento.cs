using Nebularium.Behemoth.Mongo.Configuracoes;
using Nebularium.Behemoth.Mongo.Mapeamento;
using Nebularium.Tellurian.Drone.Entidades;
using System;

namespace Nebularium.Tellurian.Drone.Behemoth.Mapeamentos
{
    [NomeColecao("PessoasTeste")]
    public class PessoaMapeamento : EntidadeMapeamento
    {
        public string NomeSobrenome { get; set; }
        public Genero Genero { get; set; }
        public DateTime Nascimento { get; set; }
        public Metadado Metadado { get; set; }
        //public List<Endereco> Enderecos { get; set; }
    }
}
