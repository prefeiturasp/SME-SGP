using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace GestaoAcademica.WebApi.Controllers
{
    public class cursosController : ApiController
    {
        /// <summary>
        /// Seleciona todos os cursos ativos.
        /// </summary>
        /// <param name="id">cur_id</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        public HttpResponseMessage Get(Int32 id)
        {
            try
            {
                CursoEntradaDTO filtro = new CursoEntradaDTO();
                filtro.cur_id = id;
                //filtro.cur_nome = nome;

                List<ACA_CursoDTO> dto = ApiBO.SelecionarCursos(filtro);

                return (dto == null || dto.Count.Equals(0)) ?
                    Request.CreateResponse(HttpStatusCode.NotFound) :
                    Request.CreateResponse(HttpStatusCode.OK, dto);

            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, string.Format("Erro: {0}", e.Message));
            }
        }

        /// <summary>
        /// Seleciona todos os cursos ativos vinculados a escola passada por parametro.
        /// </summary>
        /// <param name="esc_id">Id da escola</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        public HttpResponseMessage Get(long esc_id)
        {
            try
            {
                List<ACA_CursoDTO> dto = ApiBO.SelecionarCursosByEsc_id(esc_id);

                return (dto == null || dto.Count.Equals(0)) ?
                    Request.CreateResponse(HttpStatusCode.NotFound) :
                    Request.CreateResponse(HttpStatusCode.OK, dto);

            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, string.Format("Erro: {0}", e.Message));
            }
        }

        /// <summary>
        /// Persiste os dados do Json.
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
       // [HttpPost]
        //public HttpResponseMessage Post()
        //{
        //    try
        //    {
        //        string json = Request.Content.ReadAsStringAsync().Result;

        //        List<ACA_CursoDTO> dto = ApiBO.SalvarCurso(json);

        //        return (dto == null || dto.Count.Equals(0)) ?
        //            Request.CreateResponse(HttpStatusCode.NoContent) :
        //            Request.CreateResponse(HttpStatusCode.Created, dto);

        //    }
        //    catch (Exception e)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, string.Format("Erro: {0}", e.Message));
        //    }
        //}

    }
}