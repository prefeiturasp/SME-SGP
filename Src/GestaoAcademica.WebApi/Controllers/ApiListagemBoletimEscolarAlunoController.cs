using GestaoAcademica.WebApi.Authentication;
using GestaoAcademica.WebApi.Controllers.Base;
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
    public class ApiListagemBoletimEscolarAlunoController : BaseApiController
    {
        /// <summary>
        /// Retorna o boletim escolar do aluno
        /// </summary>
        /// <param name="filtros">Objeto com parâmetros de entrada</param>
        /// <returns></returns>
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public List<BuscaBoletimEscolarAlunoSaidaDTO> GetAll(long alu_id, int mtu_id)
        {
            try
            {
                BuscaBoletimEscolarAlunoEntradaDTO filtros = new BuscaBoletimEscolarAlunoEntradaDTO { alu_id = alu_id, mtu_id = mtu_id };
                return ApiBO.BuscaBoletimEscolarAluno(filtros);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);

                List<BuscaBoletimEscolarAlunoSaidaDTO> list = new List<BuscaBoletimEscolarAlunoSaidaDTO>();
                BuscaBoletimEscolarAlunoSaidaDTO buscaBoletimEscolarAlunoSaidaDTO = new BuscaBoletimEscolarAlunoSaidaDTO();
                buscaBoletimEscolarAlunoSaidaDTO.Status = 1;
                buscaBoletimEscolarAlunoSaidaDTO.StatusDescription = "Ocorreu um erro ao carregar dados.";
                return list;
            }
        }

        /// <summary>
        /// Retorna o boletim escolar dos alunos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [BasicAuthentication(false)]
        [JWTAuthenticationFilter()]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public List<BuscaBoletimEscolarDosAlunosSaidaDTO> GetBoletimEscolarDosAlunos(string alu_ids, string mtu_ids, int tpc_id)
        {
            try
            {
                BuscaBoletimEscolarDosAlunosEntradaDTO filtros = new BuscaBoletimEscolarDosAlunosEntradaDTO { alu_ids = alu_ids, mtu_ids = mtu_ids, tpc_id = tpc_id };
                return ApiBO.BuscaBoletimEscolarDosAlunos(filtros);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);

                List<BuscaBoletimEscolarDosAlunosSaidaDTO> list = new List<BuscaBoletimEscolarDosAlunosSaidaDTO>();
                BuscaBoletimEscolarDosAlunosSaidaDTO buscaBoletimEscolarDosAlunosSaidaDTO = new BuscaBoletimEscolarDosAlunosSaidaDTO();
                buscaBoletimEscolarDosAlunosSaidaDTO.Status = 1;
                buscaBoletimEscolarDosAlunosSaidaDTO.StatusDescription = "Ocorreu um erro ao carregar dados.";
                list.Add(buscaBoletimEscolarDosAlunosSaidaDTO);
                return list;
            }
        }

    }
}
