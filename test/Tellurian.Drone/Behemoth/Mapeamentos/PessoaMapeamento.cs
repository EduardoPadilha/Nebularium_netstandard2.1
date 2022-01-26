using Nebularium.Behemoth.Atributos;
using Nebularium.Behemoth.Mongo.Mapeamento;
using Nebularium.Tellurian.Drone.Entidades;
using System;
using System.Collections.Generic;

namespace Nebularium.Tellurian.Drone.Behemoth.Mapeamentos
{
    [Nome("PessoasTeste")]
    public class PessoaMapeamento : EntidadeMapeamento
    {
        public string NomeSobrenome { get; set; }
        public string Cpf { get; set; }
        public Genero Genero { get; set; }
        public DateTimeOffset Nascimento { get; set; }
        public List<EnderecoMapeamento> Enderecos { get; set; }
    }
    public class EnderecoMapeamento
    {
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Logradouro { get; set; }
        public int Cep { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
    }
}
