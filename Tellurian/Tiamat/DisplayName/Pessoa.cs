using System.Collections.Generic;
using System.ComponentModel;

namespace Nebularium.Tellurian.Tiamat.DisplayName
{
    public class Pessoa
    {
        [DisplayName]
        public string Id { get; set; }
        public string NomeSobrenome { get; set; }
        [DisplayName]
        public Genero Genero { get; set; }
        [DisplayName]
        public List<Endereco> Enderecos { get; set; }

    }
    public class Endereco
    {
        [DisplayName]
        public string Numero { get; set; }
        [DisplayName]
        public string Complemento { get; set; }
        [DisplayName]
        public string Logradouro { get; set; }
        [DisplayName]
        public int Cep { get; set; }
        [DisplayName]
        public string Cidade { get; set; }
        [DisplayName]
        public string Estado { get; set; }
    }

    public enum Genero
    {
        Indefinido,
        Feminio,
        Masculino
    }
}
