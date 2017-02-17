using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace GestaoAcademica.WebApi.Controllers
{
	public class cargas_horariasController : ApiController
	{
		/// <summary>
		/// Busca os cargos por situação e por docente
		/// </summary>
		/// <param name="idsCargo">Ids de cargos de docente</param>
		/// <param name="ent_id">Ids da entidade</param>
		/// <returns></returns>
		[HttpGet]
		public List<RHU_CargaHorariaDTO> GetPorEntidadeEscolaDataBase(string idsCargo, string ent_id)
		{
			try
			{
				return ApiBO.SelecionaCargaHorariaPorCargoDocente(idsCargo, ent_id);
			}
			catch (Exception e)
			{
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
				{
					Content = new StringContent("Erro: " + e.Message)
				});
			}

			throw new HttpResponseException(HttpStatusCode.NotFound);
		}
	}
}