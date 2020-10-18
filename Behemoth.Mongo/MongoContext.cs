using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Nebularium.Tarrasque.Configuracoes;
using Nebularium.Tarrasque.Extensoes;
using System;
using System.Linq;

namespace Nebularium.Behemoth.Mongo
{
    public abstract class MongoContext : IMongoContext
    {
        private readonly MongoClient _cliente;
        protected IMongoDatabase _database { get; }
        protected IDbConfigs _mongoConfig { get; set; }

        protected MongoContext(IDbConfigs mongoConfig)
        {
            _mongoConfig = mongoConfig;
            try
            {
                var camelCaseConventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
                ConventionRegistry.Register("CamelCase", camelCaseConventionPack, type => true);

                _cliente = new MongoClient(_mongoConfig.ConnectionString);

                _database = _cliente.GetDatabase(_mongoConfig.DatabaseName);
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível se conectar com o servidor de banco.", ex);
            }
        }
        public IQueryable<T> ObterColecao<T>()
        {
            var nome = typeof(T).ObterAnotacao<NomeColecaoAttribute>()?.Nome;
            return _database.GetCollection<T>(nome.LimpoNuloBranco() ? nameof(T) : nome).AsQueryable<T>();
        }

        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
