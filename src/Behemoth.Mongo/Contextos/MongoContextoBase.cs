using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Nebularium.Behemoth.Mongo.Abstracoes;
using Nebularium.Behemoth.Mongo.Configuracoes;
using Nebularium.Behemoth.Mongo.Serializadores;
using Nebularium.Tarrasque.Abstracoes;
using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tiamat.Entidades;
using System;
using System.Security.Authentication;

namespace Nebularium.Behemoth.Mongo.Contextos
{
    public abstract class MongoContextoBase : IMongoContexto
    {
        private readonly MongoClient cliente;
        protected IMongoDatabase database { get; }
        protected IDbConfiguracao mongoConfig { get; set; }

        public IMongoDatabase OberDataBase => database;
        public IDbConfiguracao ObterConfiguracao => mongoConfig;

        protected MongoContextoBase(IDbConfiguracao mongoConfig)
        {
            this.mongoConfig = mongoConfig;
            try
            {
                BsonSerializer.RegisterSerializer(new DateTimeOffsetSupportingBsonDateTimeSerializer());
                var nebulariumConvensionPack = new ConventionPack
                {
                    new CamelCaseElementNameConvention(),
                    new IgnoreExtraElementsConvention(true)
                };
                ConventionRegistry.Register("NebulariumConvencao", nebulariumConvensionPack, type => true);

                if (UsarMapeamentoBsonClassMap)
                {
                    MapearEntidadesBase();
                    ConfigurarMapeamentoBsonClassMap();
                }

                var settings = MongoClientSettings.FromUrl(new MongoUrl(this.mongoConfig.StringConexao));

                if (ProcoloSsl.HasValue)
                    settings.SslSettings = new SslSettings() { EnabledSslProtocols = ProcoloSsl.Value };

                cliente = new MongoClient(settings);

                database = cliente.GetDatabase(this.mongoConfig.NomeBancoDados);
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível se conectar com o servidor de banco.", ex);
            }
        }
        public virtual IMongoCollection<T> ObterColecao<T>()
        {
            var nome = typeof(T).ObterAnotacao<NomeColecaoAttribute>()?.Nome;
            return ObterColecao<T>(nome.LimpoNuloBranco() ? typeof(T).Name : nome);
        }

        public virtual IMongoCollection<T> ObterColecao<T>(string nomeColecao)
        {
            return database.GetCollection<T>(nomeColecao);
        }
        public virtual SslProtocols? ProcoloSsl => null;
        public abstract bool UsarMapeamentoBsonClassMap { get; }
        public abstract void ConfigurarMapeamentoBsonClassMap();

        protected virtual void MapearEntidadesBase()
        {
            BsonClassMap.RegisterClassMap<Entidade>(cm =>
            {
                cm.AutoMap();
                cm
                .MapIdProperty(c => c.Id)
                .SetIdGenerator(StringObjectIdGenerator.Instance)
                .SetSerializer(new StringSerializer(BsonType.ObjectId));
            });
            BsonClassMap.RegisterClassMap<Metadado>();
        }

        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
