using Nebularium.Tellurian.Drone.Entidades;
using Nebularium.Tiamat.Abstracoes;
using Nebularium.Tiamat.Entidades.Resultados;
using Nebularium.Tiamat.Excecoes;
using Nebularium.Tiamat.Features;
using System.Threading.Tasks;

namespace Nebularium.Tellurian.Drone.Features
{
    public interface ICadastrarPessoa
    {
        Task<CriacaoResultado> Executar(Pessoa inbound);
    }
    public class CadastrarPessoa : FeatureComandoValidando<Pessoa, Pessoa>, ICadastrarPessoa
    {
        public CadastrarPessoa(IContextoNotificacao notificacao,
            IValidador<Pessoa> validador,
            IComandoRepositorioBase<Pessoa> repositorio) :
            base(notificacao, validador, repositorio)
        {
        }

        public async Task<CriacaoResultado> Executar(Pessoa inbound)
        {
            if (!ValidarInbound(inbound)) return CriacaoResultado.Retorno(null);

            try
            {
                await repositorio.AdicionarAsync(inbound);
            }
            catch (UnicidadeException ex)
            {
                notificacao.AddNotificacao(ex.Message, "Pessoa com mesmo cpf já cadastrada");
                return CriacaoResultado.Retorno(null);
            }

            return CriacaoResultado.Retorno(inbound.Id);
        }
    }
}
