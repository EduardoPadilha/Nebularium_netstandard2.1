using Nebularium.Tiamat.Interfaces;
using System.Threading.Tasks;

namespace Nebularium.Tiamat.Servicos
{
    public abstract class ConsultaServico<TEntidade> : IConsultaServico<TEntidade> where TEntidade : IEntidade, new()
    {
        private readonly IConsultaRepositorio<TEntidade> repositorioConsulta;

        public ConsultaServico(IConsultaRepositorio<TEntidade> repositorioConsulta)
        {
            this.repositorioConsulta = repositorioConsulta;
        }
        public Task<TEntidade> Obter(string id)
        {
            return repositorioConsulta.Obter(id);
        }

        //public Task<IList<TEntidade>> Obter(TEntidade filtro)
        //{
        //    return repositorioConsulta.Obter(filtro);
        //}
    }
}
