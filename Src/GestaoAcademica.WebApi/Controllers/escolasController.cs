using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace GestaoAcademica.WebApi.Controllers
{
    public class escolasController : ApiController
    {
        /// <summary>
        /// Descrição: Retorna o registro de escola pelo id
        /// -- Utilização: URL_API/escolas/1
        /// -- Parâmetros: id=corresponde ao id da escola.
        /// </summary>
        /// <param name="id">esc_id</param>
        /// <returns></returns>
        [HttpGet]
        public ESC_EscolaDTO Get(int id)
        {
            try
            {
                List<ESC_EscolaDTO> escolas = ApiBO.SelecionarEscolasPorEntidade(id, null, Guid.Empty, new DateTime());
                if (escolas != null && escolas.Count > 0)
                    return escolas.FirstOrDefault();
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
        /// Descrição: Retorna o registro de escola pelo código
        /// -- Utilização: URL_API/escolas?esc_codigo=1111
        /// -- Parâmetros: esc_codigo = codigo da escola
        /// </summary>
        /// <param name="esc_codigo">codigo da escola</param>
        /// <returns></returns>
        [HttpGet]
        public ESC_EscolaDTO GetPorCodigo(string esc_codigo)
        {
            try
            {
                List<ESC_EscolaDTO> escolas = ApiBO.SelecionarEscolasPorEntidade(0, esc_codigo, Guid.Empty, new DateTime());
                if (escolas != null && escolas.Count > 0)
                    return escolas.FirstOrDefault();
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
        /// Descrição: lista todas as escolas vinculadas a entidade, passando a dataBase
        /// sera retornado uma lista com registros criados ou alterados apos esta data, caso contrario
        /// apenas registros ativos serão retornados.
        /// --Utilização: URL_API/escolas?esc_id=1 ou URL_API/escolas?esc_id=1&dataBase=9999-99-99T99:99:99.999
        /// --Paramêtros: esc_id = id da escola / dataBase = data base para seleção dos registros
        /// </summary>
        /// <param name="ent_id"></param>
        /// <returns></returns>
        [HttpGet]
        public List<ESC_EscolaDTO> GetPorEntidade(string ent_id)
        {
            return GetPorEntidadeDataBase(ent_id, null);
        }

        [HttpGet]
        public List<ESC_EscolaDTO> GetPorEntidadeDataBase(string ent_id, String dataBase)
        {
            try
            {
                DateTime data = string.IsNullOrEmpty(dataBase) ? new DateTime() : Convert.ToDateTime(dataBase);
                List<ESC_EscolaDTO> escolas = ApiBO.SelecionarEscolasPorEntidade(0, null, new Guid(ent_id), data);
                if (escolas != null && escolas.Count > 0)
                    return escolas;
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
        /// Retorna as escolas de acordo com a unidade administrativa superior.
        /// --Utilização: URL_API/escolas/GetPorUsuarioUASuperior?uad_idSuperior=2fc88528-6b2a-4d1d-9426-d672efb50d40&ent_id=6cf424dc-8ec3-e011-9b36-00155d033206&gru_id=b2b46864-65b6-e611-80e9-00155d000dfb&usu_id=6ef424dc-8ec3-e011-9b36-00155d033206
        /// --Paramêtros: uad_idSuperior = id da unidade administrativa superior / ent_id = id da entidade / gru_id = id do grupo do usuário que está realizando a pesquisa / usu_id = id do usuário que está realizando a pesquisa
        /// </summary>
        /// <param name="uad_idSuperior"></param>
        /// <param name="ent_id"></param>
        /// <param name="gru_id"></param>
        /// <param name="usu_id"></param>
        /// <returns></returns>
        [HttpGet]
        public List<sComboUAEscola> GetPorUsuarioUASuperior(Guid uad_idSuperior, Guid ent_id, Guid gru_id, Guid usu_id)
        {
            try
            {
                return ESC_UnidadeEscolaBO.GetSelectAtivosByUASuperior(uad_idSuperior, ent_id, gru_id, usu_id);
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
