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
    public class ApiBuscaFotoAlunoController : ApiController
    {
        /// <summary>
        /// Busca a foto do respectivo aluno, redimenciona ela para o tamanho utilizado no tablet, transforma em Base64 envia a informacao
        /// </summary>
        /// <param name="filtroEntrada">Objeto com parâmetros de entrada</param>
        /// <returns></returns>

        public BuscaFotoAlunoSaidaDTO GetAll([FromUri] BuscaFotoAlunoEntradaDTO filtroEntrada)
        {
            try
            {
                return ApiBO.BuscaFotoAluno(filtroEntrada);
            }
            catch
            {
                return null;
            }
        }

    }
}
