using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida;
using MSTech.GestaoEscolar.BLL;
using System.Web.Http.Description;

namespace GestaoAcademica.WebApi.Controllers.SyncDiarioClasse
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ApiAssociaEscolaController : ApiController
    {
        /// <summary>
        /// Associa o tablet com uma escola através da tabela SYS_EquipamentoUnidadeAdministrativa
        /// do banco DiarioClasse.
        /// </summary>
        /// <param name="buscaEscolaEntradaDTO">Objeto com parâmetros de entrada</param>
        /// <returns></returns>
        
        public AssociaEscolaSaidaDTO GetAll([FromUri] AssociaEscolaEntradaDTO filtroEntrada)
        {
            try
            {
                return ApiBO.AssociaEscola(filtroEntrada);
            }
            catch
            {
                return null;
            }
        }

    }
}
