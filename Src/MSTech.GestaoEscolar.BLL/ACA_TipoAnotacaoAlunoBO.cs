/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/


using MSTech.Business.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using MSTech.Validation.Exceptions;
using System;
using System.Web;

namespace MSTech.GestaoEscolar.BLL
{
	/// <summary>
	/// Description: ACA_TipoAnotacaoAluno Business Object. 
	/// </summary>
	public class ACA_TipoAnotacaoAlunoBO : BusinessBase<ACA_TipoAnotacaoAlunoDAO, ACA_TipoAnotacaoAluno>
	{
		
        /// <summary>
        /// Retorna os tipos de tipos de anotacões do aluno
        /// </summary>
        /// <returns>Lista com os tipos</returns>
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public static DataTable SelecionarTipoAnotacaoAluno_ent_id(Guid ent_id)
        {
            ACA_TipoAnotacaoAlunoDAO dao = new ACA_TipoAnotacaoAlunoDAO();
            return dao.SelecionarTipoAnotacaoAluno_ent_id(ent_id);
        }

        /// <summary>
        /// Retorna os tipos de tipos de anotacões do aluno
        /// </summary>
        /// <returns>Lista com os tipos</returns>
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public static DataTable SelecionarTipoAnotacaoAluno_ent_id_tia_nome(Guid ent_id, string tia_nome)
        {
            ACA_TipoAnotacaoAlunoDAO dao = new ACA_TipoAnotacaoAlunoDAO();
            return dao.SelecionarTipoAnotacaoAluno_ent_id_tia_nome(ent_id, tia_nome);
        }

        /// <summary>
        /// Seleciona os dados do tipo de anotação do aluno de acordo com o id e a entidade da mesma.
        /// </summary>
        /// <param name="entity">Entidade ACA_TipoAnotacaoAluno</param>     
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetEntity_tia_id_ent_id(ACA_TipoAnotacaoAluno entity)
        {
            ACA_TipoAnotacaoAlunoDAO dao = new ACA_TipoAnotacaoAlunoDAO();
            return dao.GetEntity_tia_id_ent_id(entity.tia_id, entity.ent_id);
        }
        
        /// <summary>
        /// Remove do cache a entidade.
        /// </summary>
        /// <param name="entity"></param>
        private static void LimpaCache(ACA_TipoAnotacaoAluno entity)
        {
            if (HttpContext.Current != null)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                HttpContext.Current.Cache.Remove(RetornaChaveCache_GetEntity(entity));
            }
        }

        /// <summary>
        /// Retorna a chave para guardar em cache o GetEntity da classe.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static string RetornaChaveCache_GetEntity(ACA_TipoAnotacaoAluno entity)
        {
            return string.Format("ACA_TipoAnotacaoAluno_GetEntity_{0}", entity.tia_id);
        }
	}
}