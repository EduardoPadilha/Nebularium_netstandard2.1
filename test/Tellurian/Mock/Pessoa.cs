using Nebularium.Tiamat.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Nebularium.Tellurian.Mock
{
    public class Pessoa : Entidade
    {
        [DisplayName]
        public string NomeSobrenome { get; set; }
        [DisplayName]
        public Genero Genero { get; set; }
        [DisplayName]
        public List<Endereco> Enderecos { get; set; }
        [DisplayName]
        public DateTime Nascimento { get; set; }

        public static List<Pessoa> Pessoas = new List<Pessoa>
        {
            new Pessoa
            {
                Id = "7592faa772d54e5387589908",
                NomeSobrenome = "Melissa Evelyn Almada",
                Genero = Genero.Feminio,
                Nascimento = new DateTime(1963,8,5),
                Enderecos = new List<Endereco>
                {
                    new Endereco
                    {
                        Cep = 77441026,
                        Logradouro = "Rua VS 12",
                        Numero = "813",
                        Cidade = "Gurupi",
                        Estado = "Tocantins"
                    }
                }
            },
            new Pessoa
            {
                Id = "941fe55c7e0a4cafb931767e",
                NomeSobrenome = "Juan Anderson Alves",
                Genero = Genero.Masculino,
                Nascimento = new DateTime(1998,6,14),
                Enderecos = new List<Endereco>
                {
                    new Endereco
                    {
                        Cep = 55038206,
                        Logradouro = "Rua Inácio Pereira Duque",
                        Numero = "442",
                        Cidade = "Caruaru",
                        Estado = "Pernambuco",
                    }
                }
            },
            new Pessoa
            {
                Id = "226acc2289174654911ac0c3",
                NomeSobrenome = "Renan Erick Fernandes",
                Genero = Genero.Masculino,
                Nascimento = new DateTime(1989,3,10),
                Enderecos = new List<Endereco>
                {
                    new Endereco
                    {
                        Cep = 69090690,
                        Logradouro = "Travessa Belterra",
                        Numero = "508",
                        Cidade = "Manaus",
                        Estado = "Amazonas",
                    }
                }
            },
            new Pessoa
            {
                Id = "1385e3ac0ef5441686794a76",
                NomeSobrenome = "Francisca Stefany Raimunda Pinto",
                Genero = Genero.Feminio,
                Nascimento = new DateTime(1946,6,17),
                Enderecos = new List<Endereco>
                {
                    new Endereco
                    {
                        Cep = 58419230,
                        Logradouro = "Rua Adalcina de Lucena",
                        Numero = "189",
                        Cidade = "Campina Grande",
                        Estado = "Paraíba",
                    }
                }
            }
        };
        public static List<Pessoa> PessoasValidacao = new List<Pessoa>
        {
            new Pessoa
            {
                Id = "7592faa772d54e5387589908",
                Genero = Genero.Feminio,
                Enderecos = new List<Endereco>
                {
                    new Endereco
                    {
                        Cep = 77441026,
                        Logradouro = "Rua VS 12",
                        Numero = "813",
                        Cidade = "Gurupi",
                        Estado = "Tocantins"
                    }
                }
            },
            new Pessoa
            {
                Id = "941fe55c7e0a4cafb931767e",
                NomeSobrenome = "Juan Anderson Alves",
                Genero = Genero.Masculino,
            },
            new Pessoa
            {
                Id = "226acc2289174654911ac0c3",
                NomeSobrenome = "Renan Erick Fernandes",
                Genero = Genero.Masculino,
                Enderecos = new List<Endereco>
                {
                    new Endereco
                    {
                        Cep = 69090690,
                        Numero = "508",
                        Cidade = "Manaus",
                    }
                }
            },
            new Pessoa
            {
                Id = "1385e3ac0ef5441686794a76",
                Genero = Genero.Feminio,
                Enderecos = new List<Endereco>
                {
                    new Endereco
                    {
                    }
                }
            }
        };
    }

    public static class PessoaExtensao
    {
        public static bool IguaisMenosPorId(this Pessoa esta, Pessoa outra)
        {
            return esta.Genero == outra.Genero && esta.Nascimento == outra.Nascimento && esta.NomeSobrenome == outra.NomeSobrenome;
        }

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
