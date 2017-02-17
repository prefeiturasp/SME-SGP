using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using MSTech.GestaoEscolar.ObjetosSincronizacao;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida;
using MSTech.GestaoEscolar.BLL;
using System.Web.Http.Description;

namespace GestaoAcademica.WebApi.Controllers.SyncDiarioClasse
{

    [ApiExplorerSettings(IgnoreApi = true)]
    public class ApiRetornaEscolaController : ApiController
    {
        
        /// <summary>
        /// Busca a escola referente a unidade administrativa passada
        /// </summary>
        /// <param name="filtroEntrada"></param>
        /// <returns></returns>
        public RetornaEscolaSaidaDTO GetAll([FromUri] RetornaEscolaEntradaDTO filtroEntrada)
        {
            try
            {
                return ApiBO.RetornaEscola(filtroEntrada);
            }
            catch
            {
                return null;
            }
        }

    }
}
