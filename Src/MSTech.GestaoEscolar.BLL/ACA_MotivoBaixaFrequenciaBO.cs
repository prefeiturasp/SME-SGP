/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using System.ComponentModel;
    using System.Data;
    using System.Web;
    using System;
    using System.Collections.Generic;
	
	/// <summary>
	/// Description: ACA_MotivoBaixaFrequencia Business Object. 
	/// </summary>
	public class ACA_MotivoBaixaFrequenciaBO : BusinessBase<ACA_MotivoBaixaFrequenciaDAO, ACA_MotivoBaixaFrequencia>
	{
        #region Consultas

        /// <summary>
        /// Retorna todos os motivos de baixa frequência (area e seus itens)
        /// </summary>
        /// <returns>Lista os motivos</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionarAtivos(int appMinutosCacheCurto = 0)
        {
            ACA_MotivoBaixaFrequenciaDAO dao = new ACA_MotivoBaixaFrequenciaDAO();
            DataTable dados = null;
            if (appMinutosCacheCurto > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_MotivoBaixaFrequenciaAtivos();
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        dados = dao.SelecionarAtivos(out totalRecords);

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheCurto), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (DataTable)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                dados = dao.SelecionarAtivos(out totalRecords);
            }

            return dados;
        }

        /// <summary>
        /// Retorna todos os itens da área informada
        /// mbf_idPai => é o vinculo do item com a área
        /// </summary>
        /// <param name="mbf_idPai">Id do motivo</param>
        /// <returns>DataTable com os dados</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTodosItens_Por_MotivoBaixaFrequencia
        (
            int mbf_idPai
        )
        {
            ACA_MotivoBaixaFrequenciaDAO dao = new ACA_MotivoBaixaFrequenciaDAO();
            return dao.SelecionaTodosItens_Por_MotivoBaixaFrequencia(mbf_idPai);
        }

        /// <summary>
        /// Retorna um item especifico do motivo de infrequência informado. 
        /// </summary>
        /// <param name="mbf_id">Id do motivo baixa frequencia</param>
        /// <param name="mbf_idPai">Id do motivo - indicando o vinculo com a area(mbf_id) </param>
        /// <returns>DataTable com os dados</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTodosItens_Por_MotivoBaixaFrequencia
        (
            int mbf_id,
            int mbf_idPai
        )
        {
            ACA_MotivoBaixaFrequenciaDAO dao = new ACA_MotivoBaixaFrequenciaDAO();
            return dao.SelecionaItem_Por_MotivoBaixaFrequencia(mbf_id, mbf_idPai);
        }

        public static string RetornaChaveCache_MotivoBaixaFrequenciaAtivos()
        {
            return "Cache_MotivoBaixaFrequenciaAtivos";
        }

        /// <summary>
        /// Retorna uma lista de entidades com os itens dos motivo de infrequencia informado
        /// </summary>
        /// <param name="mbf_idPai">Id do motivo</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<ACA_MotivoBaixaFrequencia> Seleciona_Entidades_ItensMotivoInfrequencia
        (
            int mbf_idPai
        )
        {
            List<ACA_MotivoBaixaFrequencia> lista = new List<ACA_MotivoBaixaFrequencia>();

            ACA_MotivoBaixaFrequenciaDAO dao = new ACA_MotivoBaixaFrequenciaDAO();
            DataTable dt = dao.SelecionaTodosItens_Por_MotivoBaixaFrequencia(mbf_idPai);

            foreach (DataRow dr in dt.Rows)
            {
                ACA_MotivoBaixaFrequencia entity = new ACA_MotivoBaixaFrequencia();
                entity = dao.DataRowToEntity(dr, entity);

                lista.Add(entity);
            }
            return lista;
        }
        #endregion
    }
}