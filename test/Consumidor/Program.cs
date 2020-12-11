using Nebularium.Tarrasque.Gestores;
using Nebularium.Tellurian.Drone.Recursos;
using Nebularium.Weaver.Interfaces;
using System;

namespace Consumidor
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = new Program();
            Console.WriteLine("Consumidor startado");
            Console.WriteLine("Aguardando mensagnes");
            while (true)
            {
            }
        }

        public Program()
        {
            NebulariumFabrica.Inicializar();
            var fabrica = (NebulariumFabrica)NebulariumFabrica.Instancia;
            fabrica.ConfiguracaoesAdicionais(ServicosExtensao.ConfigurarBarramentoEvento);
        }
    }
}
