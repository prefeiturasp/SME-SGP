using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;

namespace GestaoAcademica.WebApi.Controllers.SyncDiarioClasse
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ApiGravaFotoAlunosController : ApiController
    {

        /// <summary>
        /// Recebe e grava uma lista de fotos de alunos.
        /// </summary>
        /// <param name="filtroEntrada">Objeto com os itens de entrada.</param>
        /// <returns>Protocolo</returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public SincronizacaoDiarioClasseSaidaDTO Post([FromUri] SincronizacaoDiarioClasseEntradaDTO filtroEntrada)
        {
            try
            {
                var jsonPacote = Request.Content.ReadAsStringAsync().Result;
                filtroEntrada.pro_pacote = jsonPacote.ToString();
                return ApiBO.SincronizaDiarioClasse(filtroEntrada, DCL_ProtocoloBO.eTipo.Foto);
            }
            catch
            {
                return null;
            }
        }
    }
}
