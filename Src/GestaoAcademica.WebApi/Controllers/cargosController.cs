using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GestaoAcademica.WebApi.Controllers
{
	public class cargosController : ApiController
	{
		/// <summary>
		/// Busca os cargos por situação e por docente
		/// </summary>
		/// <param name="docente"></param>
		/// <param name="bloqueado"></param>
		/// <param name="ent_id">Ids da entidade</param>
		/// <returns></returns>
		[HttpGet]
		public List<RHU_CargoDTO> Get(bool docente, bool bloqueado, string ent_id, bool cargahoraria)
		{
			try
			{
				return ApiBO.SelecionaCargosPorDocenteSituacao(docente, bloqueado, ent_id, cargahoraria);
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