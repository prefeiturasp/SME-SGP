using GestaoAcademica.WebApi.Authentication;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace GestaoAcademica.WebApi.Controllers
{ 
    public class sondagemController : ApiController
    {
        /// <summary>
        /// Descrição: retorna os dados das sondagens que o aluno participou.
        /// </summary>
        /// <param name="filtros">Objeto com parâmetros de entrada: ano e id do aluno.</param>
        /// <returns>Objeto com os dados das sondagens.</returns>
        [HttpGet]
        [BasicAuthentication(false)]
        [JWTAuthenticationFilter()]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public SondagemSaidaDTO Get([FromUri] AnoAlunoEntradaDTO filtros)
        {
            try
            {
                return ApiBO.BuscaSondagemAluno(filtros);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);

                SondagemSaidaDTO saidaDTO = new SondagemSaidaDTO();
                saidaDTO.Status = 1;
                saidaDTO.StatusDescription = "Ocorreu um erro ao carregar dados.";
                saidaDTO.sondagens = new List<SondagemDTO>();
                return saidaDTO;
            }
        }
    }
}