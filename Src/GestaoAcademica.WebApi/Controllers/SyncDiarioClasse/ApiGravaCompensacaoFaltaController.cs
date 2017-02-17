using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace GestaoAcademica.WebApi.Controllers.SyncDiarioClasse
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ApiGravaCompensacaoFaltaController : ApiController
    {
        [System.Web.Http.AcceptVerbs("POST")]
        public SincronizacaoDiarioClasseSaidaDTO Post([FromUri] SincronizacaoDiarioClasseEntradaDTO filtroEntrada)
        {
            try
            {
                var jsonPacote = Request.Content.ReadAsStringAsync().Result;
                filtroEntrada.pro_pacote = jsonPacote.ToString();
                return ApiBO.SincronizaDiarioClasse(filtroEntrada, DCL_ProtocoloBO.eTipo.CompensacaoDeAula);
            }
            catch
            {
                return null;
            }
        }
    }
}
