using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Nebularium.Behemoth.Mongo.Abstracoes;
using Nebularium.Behemoth.Mongo.Configuracoes;
using Nebularium.Behemoth.Mongo.Serializadores;
using Nebularium.Tarrasque.Configuracoes;
using Nebularium.Tarrasque.Extensoes;
using System;

namespace Nebularium.Behemoth.Mongo.Contextos
{
    public abstract class MongoContextoBase : IMongoContexto
    {
        private readonly MongoClient cliente;
        protected IMongoDatabase database { get; }
        protected IDbConfiguracao mongoConfig { get; set; }

        public IMongoDatabase OberDataBase => database;

        protected MongoContextoBase(IDbConfiguracao mongoConfig)
        {
            this.mongoConfig = mongoConfig;
            try
            {
                BsonSerializer.RegisterSerializer(new DateTimeOffsetSupportingBsonDateTimeSerializer());
                var camelCaseConventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
                ConventionRegistry.Register("CamelCase", camelCaseConventionPack, type => true);

                cliente = new MongoClient(this.mongoConfig.ConnectionString);

                database = cliente.GetDatabase(this.mongoConfig.DatabaseName);
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível se conectar com o servidor de banco.", ex);
            }
        }
        public virtual IMongoCollection<T> ObterColecao<T>()
        {
            var nome = typeof(T).ObterAnotacao<NomeColecaoAttribute>()?.Nome;
            return database.GetCollection<T>(nome.LimpoNuloBranco() ? nameof(T) : nome);
        }

        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
