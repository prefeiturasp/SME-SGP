using MSTech.Business.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL;
using System.ComponentModel;
using System.Collections.Generic;
using System;
using MSTech.Data.Common;
using System.Web;
using System.Data;
using System.Linq;

namespace MSTech.GestaoEscolar.BLL
{
	/// <summary>
	/// Estrutura para CursoRelacionado usado para salvar CheckboxList.
	/// </summary>
	public struct CursoRelacionado 
	{
		public int cur_id;
		public int crr_id;
	}

	/// <summary>
	/// ACA_CursoRelacionado Business Object 
	/// </summary>
	public class ACA_CursoRelacionadoBO : BusinessBase<ACA_CursoRelacionadoDAO,ACA_CursoRelacionado>
	{
		/// <summary>
		/// Envia paramentros para o metodo [DAO]DeletarTodosRelacionado_Curso_Curriculo 
		/// </summary>
		/// <param name="cur_id">Id Curso</param>
		/// <param name="crr_id">Id Curriculo</param>
		/// <param name="banco">Transação</param>
		/// <returns>Boleano</returns>
		[DataObjectMethod(DataObjectMethodType.Delete, false)]
		public static bool DeletarTodosRelacionado_Curso_Curriculo
		(
			int cur_id
			, int crr_id
			, TalkDBTransaction banco
		)
		{
			ACA_CursoRelacionadoDAO dao = new ACA_CursoRelacionadoDAO {_Banco = banco};
			dao.DeletarTodosRelacionado_Curso_Curriculo(cur_id, crr_id);
			return true;
		}

		/// <summary>
		/// Envia parametros para metodo [DAO]SelecionaCursosRelacionados.
		/// </summary>
		/// <param name="cur_id">Id curso.</param>
		/// <param name="crr_id">Id curriculo.</param>
		/// <returns>Retorna List com dados dos cursos relacionados.</returns>
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public static List<String> SelecionaCursosRelacionados
		(
			int cur_id
			, int crr_id
		)
		{
			ACA_CursoRelacionadoDAO dao = new ACA_CursoRelacionadoDAO();
			return dao.SelecionaCursosRelacionados(cur_id, crr_id);
		}


		/// <summary>
		/// Retorna os cur_id e crr_id relacionados ao curso informado.
		/// </summary>
		/// <param name="cur_id">ID do curso</param>
		/// <param name="crr_id">ID do currículo do curso</param>
		/// <returns></returns>
		public static List<sComboCurso> GetSelectBy_Curso
		(
			int cur_id
			, int crr_id
			, int appMinutosCacheLongo = 0
			, TalkDBTransaction banco = null
		)
		{
			List<sComboCurso> dados = null;
			if (appMinutosCacheLongo > 0)
			{
				if (HttpContext.Current != null)
				{
					// Chave padrão do cache - nome do método + parâmetros.
					string chave = RetornaChaveCache_Seleciona_CursosRelacionados(cur_id, crr_id);
					object cache = HttpContext.Current.Cache[chave];

					if (cache == null)
					{
						ACA_CursoRelacionadoDAO dao = new ACA_CursoRelacionadoDAO();
						if (banco != null)
						{
							dao._Banco = banco;
						}

						DataTable dtDados = dao.GetSelectBy_Curso(cur_id, crr_id);
						dados = (from DataRow dr in dtDados.Rows
								 select new sComboCurso
								 {
									 cur_crr_id = dr["cur_crr_id"].ToString(),
									 cur_crr_nome = dr["cur_crr_nome"].ToString()
								 }).ToList();

						// Adiciona cache com validade do tempo informado na configuração.
						HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
					}
					else
					{
						dados = (List<sComboCurso>)cache;
					}
				}
			}

			if (dados == null)
			{
				// Se não carregou pelo cache, seleciona os dados do banco.
				ACA_CursoRelacionadoDAO dao = new ACA_CursoRelacionadoDAO();
				if (banco != null)
				{
					dao._Banco = banco;
				}
				DataTable dtDados = dao.GetSelectBy_Curso(cur_id, crr_id);
				dados = (from DataRow dr in dtDados.Rows
						 select new sComboCurso
						 {
							 cur_crr_id = dr["cur_crr_id"].ToString(),
							 cur_crr_nome = dr["cur_crr_nome"].ToString()
						 }).ToList();
			}

			return dados;
		}

		private static string RetornaChaveCache_Seleciona_CursosRelacionados(int cur_id, int crr_id)
		{
			return string.Format("Cache_Seleciona_CursosRelacionados_{0}_{1}", cur_id, crr_id);
		}
	}
}