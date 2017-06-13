namespace GestaoAcademica.WebApi.Controllers
{
    using Authentication;
    using MSTech.GestaoEscolar.BLL;
    using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    /// <summary>
    /// Recurso parecer conclusivo.
    /// </summary>
    [BasicAuthentication]
    public class parecerConclusivoController : ApiController
    {
        /// <summary>
        /// Descrição: retorna parecer conclusivo do aluno pelo código EOL da turma e do aluno.
        /// -- Utilização: URL_API/parecerConclusivo?CodigoEOLTurma=99999&CodigoEOLAluno=99999
        /// -- Parâmetros: CodigoEOLTurma: Código EOL da turma
        /// --             CodigoEOLAluno: Código EOL do aluno
        /// </summary>
        /// <param name="CodigoEOLTurma">Código EOL da turma</param>
        /// <param name="CodigoEOLAluno">Código EOL do aluno</param>
        /// <returns>registro de aluno</returns>
        [HttpGet]
        public HttpResponseMessage GetResultadoAluno(int CodigoEOLTurma, string CodigoEOLAluno)
        {
            try
            {
                ParecerConclusivo parecer = ApiBO.SelecionaResultadoPorAlunoTurmaEOL(CodigoEOLTurma, CodigoEOLAluno);

                if (parecer != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, parecer);
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ErrorMessage { message = e.Message });
            }

            return Request.CreateResponse(HttpStatusCode.NoContent, new ErrorMessage { message = "204 NoContent" });
        }
    }
}
