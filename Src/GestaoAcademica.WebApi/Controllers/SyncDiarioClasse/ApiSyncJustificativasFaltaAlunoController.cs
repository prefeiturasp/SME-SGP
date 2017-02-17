using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;
using MSTech.GestaoEscolar.BLL;
using System.Web.Http.Description;

namespace GestaoAcademica.WebApi.Controllers.SyncDiarioClasse
{

    [ApiExplorerSettings(IgnoreApi = true)]
    public class ApiSyncJustificativasFaltaAlunoController : ApiController
    {
        /// <summary>
        /// Recebe e sincroniza todas as informações necessárias referentes a justificativa falta dos alunos
        /// </summary>
        /// <param name="SincronizacaoJustificativasFaltaAlunoEntradaDTO">Objeto com parâmetros de entrada</param>
        /// <returns></returns>

        [System.Web.Http.AcceptVerbs("POST")]
        public SincronizacaoDiarioClasseSaidaDTO GetAll([FromUri] SincronizacaoDiarioClasseEntradaDTO filtroEntrada)
        {
            try
            {
                var jsonPacote = Request.Content.ReadAsStringAsync().Result;
                filtroEntrada.pro_pacote = jsonPacote.ToString();
                return ApiBO.SincronizaDiarioClasse(filtroEntrada, DCL_ProtocoloBO.eTipo.JustificativaFaltaAluno);
            }
            catch
            {
                return null;
            }
        }
    }
}
