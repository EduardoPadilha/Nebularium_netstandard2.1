using Nebularium.Tarrasque.Gestores;
using Nebularium.Tellurian.Drone.Entidades;
using Nebularium.Tellurian.Drone.Eventos;
using Nebularium.Tellurian.Drone.Recursos;
using Nebularium.Weaver.Interfaces;
using System;

namespace Publicador
{
    public class Program
    {
        static void Main(string[] args)
        {
            var p = new Program();
            Console.WriteLine("Publicador startado");
            var entrada = "";
            while (!entrada.ToLower().Equals("sair"))
            {
                Console.WriteLine("Digite um evento");
                entrada = Console.ReadLine();
                string pessoa;
                if (string.IsNullOrWhiteSpace(entrada))
                    pessoa = "Fulaninho de tal";
                else
                    pessoa = entrada;

                p.DispararEvento(pessoa);
            }
        }

        private readonly IBarramentoEvento barramento;

        public Program()
        {
            NebulariumFabrica.Inicializar();
            var fabrica = (NebulariumFabrica)NebulariumFabrica.Instancia;
            //fabrica.ConfiguracaoesAdicionais(ServicosExtensao.ConfigurarBarramentoEvento);
            barramento = GestorDependencia.Instancia.ObterInstancia<IBarramentoEvento>();
        }

        public void DispararEvento(string pessoa)
        {
            var evento = new CadastrarPessoaComandoEvento(new Pessoa
            {
                Genero = Genero.Masculino,
                Nascimento = new DateTime(1988, 06, 29),
                NomeSobrenome = pessoa
            });
            barramento.Publicar(evento);
        }

    }
}
