using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GestaoAcademica.WebApi.Controllers
{
    public class tipos_anotacoes_alunoController : ApiController
    {

        /// <summary>
        /// Descrição: retorna o registro de um tipo de anotação do aluno por id
        /// -- Utilização: URL_API/tipos_anotacoes_aluno/{id}
        /// -- Parâmetros: id: id do tipo de anotação
        /// </summary>
        /// <param name="id">id do tipo de anotação</param>
        /// <returns></returns>
        [HttpGet]
        public ACA_TipoAnotacaoAlunoDTO Get(int id)
        {
            try
            {
                List<ACA_TipoAnotacaoAlunoDTO> tipos = ApiBO.SelecionarTiposAnotacoesAlunoPorEntidade(id, new Guid(), new DateTime());

                if (tipos != null && tipos.Count > 0)
                    return tipos.FirstOrDefault();
            }
            catch (Exception e)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Erro: " + e.Message)
                });
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Descrição: retorna uma lista de tipos de anotações dos alunos ativos por entidade
        /// -- Utilização: URL_API/tipos_anotacoes_aluno?ent_id={ent_id}
        /// -- Parâmetros: ent_id: id da entidade
        /// </summary>
        /// <param name="ent_id">id da entidade</param>
        /// <returns></returns>
        [HttpGet]
        public List<ACA_TipoAnotacaoAlunoDTO> GetPorEntidade(Guid ent_id)
        {
            return GetPorEntidadeDataBase(ent_id, null);
        }

        /// <summary>
        /// Descrição: returna uma lista de tipos de anotações dos alunos por entidade
        /// criadas ou alteradas apos a dataBase.
        /// -- Utilização: URL_API/tipos_anotacoes_aluno?ent_id={ent_id}&dataBase={dataBase}
        /// -- Parâmetros: ent_id=id da entidade / dataBase = data base para a busca dos registros
        /// </summary>
        /// <param name="ent_id">id da entidade</param>
        /// <param name="dataBase">data base para busca dos registros</param>
        /// <returns></returns>
        [HttpGet]
        public List<ACA_TipoAnotacaoAlunoDTO> GetPorEntidadeDataBase(Guid ent_id, String dataBase)
        {
            try
            {
                DateTime data = string.IsNullOrEmpty(dataBase) ? new DateTime() : Convert.ToDateTime(dataBase);
                List<ACA_TipoAnotacaoAlunoDTO> tipos = ApiBO.SelecionarTiposAnotacoesAlunoPorEntidade(0, ent_id, data);

                if (tipos != null && tipos.Count > 0)
                    return tipos;
            }
            catch (Exception e)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Erro: " + e.Message)
                });
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

    }
}
