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

    /// <summary>
    /// Api utilizada para buscar a data e hora do servidor e sincronizar com o tablet.
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ApiBuscaDataHoraController : ApiController
    {
        public BuscaDataHoraDTO Get([FromUri] AssociaEscolaEntradaDTO filtroEntrada)
        {
            BuscaDataHoraDTO dataHora = new BuscaDataHoraDTO();

            dataHora.Status = 0;
            dataHora.Date = DateTime.Now.ToString(MSTech.GestaoEscolar.ObjetosSincronizacao.Util.Util.mascaraData);

            if (filtroEntrada != null && !string.IsNullOrEmpty(filtroEntrada.Uad_codigo))
            {
                var atualizou = ApiBO.AtualizaEquipamento(filtroEntrada);

                if (!atualizou)
                {
                    dataHora.Status = 1;
                    dataHora.StatusDescription = "Ocorreu um erro ao tentar atualizar o equipamento!";
                }
            }

            return dataHora;
        }
    }
}
