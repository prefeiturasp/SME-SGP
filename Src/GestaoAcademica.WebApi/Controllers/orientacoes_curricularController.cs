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
    public class orientacoes_curricularController : ApiController
    {
        /// <summary>
        /// Descrição: Retorna o registro de escola pelo id
        /// -- Utilização: URL_API/escolas/1
        /// -- Parâmetros: id=corresponde ao id da escola.
        /// </summary>
        /// <param name="id">esc_id</param>
        /// <returns></returns>
        [HttpGet]
        public ORC_OrientacaoCurricularDTO Get(long id)
        {
            try
            {
                ORC_OrientacaoCurricularDTO orientacao = ApiBO.SelecionaOrientacoesPorID(id);
                if (orientacao != null )
                    return orientacao;
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
        /// Busca as orientacoes curriculares pela turmadisciplina
        /// </summary>
        /// <param name="tud_id"></param>        
        /// <returns></returns>
        [HttpGet]
        public List<ORC_OrientacaoCurricularDTO> GetPorTurmaDisciplina(long tud_id)
        {
            return GetPorTurmaDisciplinaDataBaseOrientacaoSuperior(tud_id, null, 0);
        }


        /// <summary>
        /// Busca as orientacoes curriculares pela database e pela turmadisciplina
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="dataBase"></param>
        /// <returns></returns>
        [HttpGet]
        public List<ORC_OrientacaoCurricularDTO> GetPorTurmaDisciplinaDataBase(long tud_id, String dataBase)
        {
            return GetPorTurmaDisciplinaDataBaseOrientacaoSuperior(tud_id, dataBase, -1);
        }

        /// <summary>
        /// Busca as orientações curriculares pela turma disciplina e tipo periodo calendario
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="tpc_id">Tipo Periodo Calendario</param>
        /// <returns></returns>
        [HttpGet]
        public List<ORC_OrientacaoCurricularDTO> GetPorTurmaDisciplinaTipoPeriodoCalendario(long tud_id, long tpc_id)
        {
            return GetPorTurmaDisciplinaDataBaseOrientacaoSuperior(tud_id, null, -1, tpc_id);
        }

        /// <summary>
        /// Busca as orientacoes curriculares pela database, pela turmadisciplina , pela orientacao curricular superior e por Tipo Periodo calendario
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="dataBase"></param>
        /// <param name="ocr_idSuperior"></param>
        /// <param name="tpc_id">Tipo Periodo Calendario, Opcional</param>
        /// <returns></returns>
        [HttpGet]
        public List<ORC_OrientacaoCurricularDTO> GetPorTurmaDisciplinaDataBaseOrientacaoSuperior(long tud_id, String dataBase, long ocr_idSuperior, Nullable<long> tpc_id = null)
        {
            try
            {
                DateTime data = string.IsNullOrEmpty(dataBase) ? new DateTime() : Convert.ToDateTime(dataBase);
                List<ORC_OrientacaoCurricularDTO> orientacoes = ApiBO.SelecionaOrientacoesPorTurmaDisplinaDataBase(tud_id, data, ocr_idSuperior, tpc_id);
                if (orientacoes != null && orientacoes.Count > 0)
                    return orientacoes;
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
        /// Busca as orientacoes curriculares pela escola
        /// </summary>
        /// <param name="esc_id"></param>
        /// <returns></returns>
        [HttpGet]
        public List<object> GetPorEscolaDataBase(int esc_id)
        {
            return GetPorEntidadeEscolaDataBase(new Guid(), esc_id, "");
        }

        /// <summary>
        /// Busca as orientacoes curriculares pela entidade
        /// </summary>
        /// <param name="ent_id"></param>
        /// <returns></returns>
        [HttpGet]
        public List<object> GetPorEscolaDataBase(Guid ent_id)
        {
            return GetPorEntidadeEscolaDataBase(ent_id, 0, "");
        }

        /// <summary>
        /// Busca as orientacoes curriculares pela entidade e dataBase
        /// </summary>
        /// <param name="ent_id"></param>
        /// <param name="dataBase"></param>
        /// <returns></returns>
        [HttpGet]
        public List<object> GetPorEscolaDataBase(Guid ent_id, string dataBase)
        {
            return GetPorEntidadeEscolaDataBase(ent_id, 0, dataBase);
        }

        /// <summary>
        /// Busca as orientacoes curriculares pela database e escola
        /// </summary>
        /// <param name="esc_id"></param>
        /// <param name="dataBase"></param>
        /// <returns></returns>
        [HttpGet]
        public List<object> GetPorEscolaDataBase(int esc_id, string dataBase)
        {
            return GetPorEntidadeEscolaDataBase(new Guid(), esc_id, dataBase);
        }

        /// <summary>
        /// Busca as orientacoes curriculares pela database, entidade e escola
        /// </summary>
        /// <param name="ent_id"></param>
        /// <param name="esc_id"></param>
        /// <param name="dataBase"></param>
        /// <returns></returns>
        [HttpGet]
        public List<object> GetPorEntidadeEscolaDataBase(Guid ent_id, int esc_id, string dataBase)
        {
            try
            {
                DateTime data = string.IsNullOrEmpty(dataBase) ? new DateTime() : Convert.ToDateTime(dataBase);
                List<object> orientacoes = ApiBO.SelecionaOrientacoesPorEntidadeEscolaDataBase(ent_id, esc_id, data);
                if (orientacoes != null && orientacoes.Count > 0)
                    return orientacoes;
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
