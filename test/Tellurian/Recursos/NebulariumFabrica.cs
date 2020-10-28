using Microsoft.Extensions.Configuration;
using Nebularium.Behemoth.Mongo.Contextos;
using Nebularium.Tarrasque.Configuracoes;
using Nebularium.Tarrasque.Gestores;
using Nebularium.Tarrasque.Interfaces;
using Nebularium.Tarrasque.Recursos;
using Nebularium.Tellurian.Behemoth.Repositorios;
using Nebularium.Tellurian.Mock;
using Nebularium.Tellurian.Mock.Interfaces;
using Nebularium.Tellurian.Mock.Repositorios;
using Nebularium.Tiamat.Interfaces;
using SimpleInjector;
using System;

namespace Nebularium.Tellurian.Recursos
{
    public class NebulariumFabrica : GestorDependencia<NebulariumFabrica>
    {
        public NebulariumFabrica()
        {
            Container = new Container();
            IConfiguration configuracao = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            GestorAutoMapper.Inicializar();
            Container.Register(() => GestorAutoMapper.Instancia.Mapper, Lifestyle.Singleton);

            Container.Register(() => configuracao, Lifestyle.Singleton);
            Container.Register<IGestorConfiguracao, GestorConfiguracaoPadrao>(Lifestyle.Singleton);

            Container.Register<IDisplayNameExtrator>(() => new DisplayNameExtratorPadrao(), Lifestyle.Singleton);
            Container.Register<IValidador<Pessoa>, PessoaValidador>(Lifestyle.Singleton);
            Container.Register<IValidador<Endereco>, EnderecoValidador>(Lifestyle.Singleton);

            Container.Register<IDbConfigs>(() => new DBConfig(configuracao), Lifestyle.Singleton);
            Container.Register<IMongoContext, TellurianContext>(Lifestyle.Singleton);
            Container.Register<IPessoaConsultaRepositorio, PessoaConsultaRepositorio>();
            Container.Register<IPessoaComandoRepositorio, PessoaComandoRepositorio>();

            Container.Verify();

        }
        internal protected Container Container { get; }
        public override object ObterInstancia(Type tipo)
        {
            return Container.GetInstance(tipo);
        }

        public override TInstancia ObterInstancia<TInstancia>()
        {
            return Container.GetInstance<TInstancia>();
        }
    }
}
