using Nebularium.Tarrasque.Gestores;
using Nebularium.Tellurian.Drone.Entidades;
using Nebularium.Tellurian.Drone.Eventos;
using Nebularium.Tellurian.Recursos;
using Nebularium.Weaver.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Nebularium.Tellurian.Weaver
{
    public class PessoaEventoTeste : TesteBase
    {
        private readonly IBarramentoEvento barramento;
        public PessoaEventoTeste(ITestOutputHelper saida) : base(saida)
        {
            barramento = GestorDependencia.Instancia.ObterInstancia<IBarramentoEvento>();
        }

        [Fact]
        public void CadastrarComando_teste()
        {
            //while (true)
            //{
                var evento = new CadastrarPessoaComandoEvento(new Pessoa
                {
                    Genero = Genero.Masculino,
                    Nascimento = new DateTime(1988, 06, 29),
                    NomeSobrenome = "Eduardo da Cruz Padilha"
                });
                barramento.Publicar(evento);
                //Thread.Sleep(1000);
            //}
        }
    }
}
