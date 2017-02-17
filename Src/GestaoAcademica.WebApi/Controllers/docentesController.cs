using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;
using System.Web.Http.Description;

namespace GestaoAcademica.WebApi.Controllers
{
    public class docentesController : ApiController
    {

        public struct FiltrosDocente {
            public int esc_id{get;set;}
            public string ultimaModificacao{get;set;}
        }

        /// <summary>
        /// Descrição: retorna o registro de docente pelo id
        /// -- Utilização: URL_API/docentes/1
        /// -- Parâmetros: id do docente
        /// </summary>
        /// <param name="id">id do docente</param>
        /// <returns></returns>
        [HttpGet]
        public ACA_DocenteDTO Get(int id)
        {
            try
            {
                List<ACA_DocenteDTO> docente = ApiBO.SelecionarEscolasDocentePorId(id);

                if (docente != null && docente.Count > 0) return docente.FirstOrDefault();
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
        /// Descrição: Retorna uma listagem de docentes por turma.Quando não passar
        /// a data base apenas registros ativos serão retornados, caso contrario apenas registros
        /// criados ou alterados apos esta data serão retornados.
        /// --Utilização: URL_API/docentes?tur_id=1 ou URL_API/docentes?esc_id=1&dataBase=9999-99-99T99:99:99.999
        /// --Paramêtros: tur_id = id da turma
        /// </summary>
        /// <param name="tur_id"></param>
        /// <returns></returns>
        [HttpGet]
        public List<ACA_DocenteDTO> GetDocentesPorTurma(long tur_id)
        {
            return GetDocentesPorTurma(tur_id, null);
        }

        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public List<ACA_DocenteDTO> GetDocentesPorTurma(long tur_id, String dataBase)
        {
            try
            {
                DateTime data = string.IsNullOrEmpty(dataBase) ? new DateTime() : Convert.ToDateTime(dataBase);
                List<ACA_DocenteDTO> docentes = ApiBO.SelecionarDocentesPorTurma(tur_id, data);

                if (docentes != null && docentes.Count > 0)
                    return docentes;
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
        /// Descrição: Retorna uma listagem de docentes por escola e data base. Quando não passar
        /// a data base apenas registros ativos serão retornados, caso contrario apenas registros
        /// criados ou alterados apos esta data serão retornados.
        /// --Utilização: URL_API/docentes?esc_id=1 ou URL_API/docentes?esc_id=1&dataBase=9999-99-99T99:99:99.999
        /// --Paramêtros: esc_id = id da escola / dataBase = data base para retorno dos registros.
        /// </summary>
        /// <param name="esc_id"></param>
        /// <returns></returns>
        [HttpGet]
        public List<ACA_DocenteDTO> GetDocentesPorEscola(int esc_id)
        {
            return GetDocentesPorEscolaDataBase(esc_id, null);
        }

        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public List<ACA_DocenteDTO> GetDocentesPorEscolaDataBase(int esc_id, String dataBase)
        {
            try
            {
                DateTime data = string.IsNullOrEmpty(dataBase) ? new DateTime() : Convert.ToDateTime(dataBase);
                List<ACA_DocenteDTO> docentes = ApiBO.SelecionarDocentesPorEscola(esc_id, data);

                if (docentes != null && docentes.Count > 0)
                    return docentes;
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
        /// Descrição: Retorna uma listagem de docentes por vínculo de trabalho e data base. 
        /// --Utilização:
        /// --Paramêtros: psd_numeroCPF = cpf do docente / psd_numeroRG = rg / ent_id = entidade /  coc_matricula = matrícula do docente /coc_vinculoSede = true se o docente possuir vínculo
        /// </summary>
        /// <param name="psd_numeroCPF, psd_numeroRG, ent_id, coc_matricula, coc_vinculoSede"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public List<ACA_DocenteDTO> GetDocentesPorVinculoDeTrabalho(string psd_numeroCPF, string psd_numeroRG, Guid ent_id, string coc_matricula, bool coc_vinculoSede)
        {
            try
            {
                
                List<ACA_DocenteDTO> docentes = ApiBO.SelecionarDocentesPorVinculoDeTrabalho(psd_numeroCPF, psd_numeroRG, ent_id, coc_matricula, coc_vinculoSede);

                if (docentes != null && docentes.Count > 0)
                    return docentes;
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
        /// Descrição: Retorna uma listagem de usuários de docentes na mesma unidade ou abaixo da unidade do usuário/grupo informado.
        /// Só faz a pesquisa se o ID do sistema informado for o mesmo do gestão escolar
        /// --Utilização: URL_API/docentes?usu_id=6EF424DC-8EC3-E011-9B36-00155D033206&gru_id=6EF424DC-8EC3-E011-9B36-00155D033206&sis_id=102
        /// --Paramêtros: usu_id = id do usuário que está realizando a pesquisa / usu_id = id do grupo do usuário que está realizando a pesquisa / sis_id = id do sistema
        /// </summary>
        /// <param name="usu_id"></param>
        /// <param name="gru_id"></param>
        /// <param name="sis_id"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public List<sUsrDocente> GetDocentesPorUsuarioGrupo(Guid usu_id, Guid gru_id, int sis_id, string usu_login, string usu_email, string pes_nome)
        {
            try
            {
                List<sUsrDocente> lstUsrDocentes = new List<sUsrDocente>();
                IDictionary<string, ICFG_Configuracao> lstConfig = new Dictionary<string, ICFG_Configuracao>();
                CFG_ConfiguracaoBO.Consultar(eConfig.Academico, out lstConfig);
                if (lstConfig.Any() && lstConfig.ContainsKey("appSistemaID") && !string.IsNullOrEmpty(lstConfig["appSistemaID"].cfg_valor) &&
                    sis_id == Convert.ToInt32(lstConfig["appSistemaID"].cfg_valor))
                    lstUsrDocentes = ApiBO.SelecionarUsrDocentesPorUsuarioGrupo(usu_id, gru_id, sis_id, usu_login, usu_email, pes_nome);

                if (lstUsrDocentes != null && lstUsrDocentes.Count > 0)
                    return lstUsrDocentes;
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
