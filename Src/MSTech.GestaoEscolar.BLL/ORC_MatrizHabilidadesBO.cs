/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/
using MSTech.Business.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL;
using MSTech.Data.Common;
using System.Data;
using MSTech.Validation.Exceptions;
using MSTech.GestaoEscolar.BLL;
using System;
using MSTech.GestaoEscolar.Entities;
using System.ComponentModel;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace MSTech.GestaoEscolar.BLL
{
    /// <summary>
    /// Description: ORC_MatrizHabilidades Business Object. 
    /// </summary>
    public class ORC_MatrizHabilidadesBO : BusinessBase<ORC_MatrizHabilidadesDAO, ORC_MatrizHabilidades>
    {
        /// <summary>
        /// Salva a matriz de habilidade
        /// </summary>
        /// <param name="entMatriz">entidade ORC_MatrizHabilidade</param>
        /// <returns></returns>
        public static bool Salvar(ORC_MatrizHabilidades entMatriz)
        {
            TalkDBTransaction banco = new ORC_MatrizHabilidadesDAO()._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);

            try
            {                
                return Save(entMatriz, banco);                                                
            }
            catch (Exception ex)
            {
                banco.Close(ex);
                throw;
            }
            finally
            {
                banco.Close();
            }
        }


        /// <summary>
        /// Busca as matrizes de habilidade para a tela de busca.
        /// </summary>
        /// <param name="mat_nome">Nome da matriz</param>
        /// <param name="ent_id">ID da entidade</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable BuscaMatrizHabilidades
        (
            string mat_nome
            , Guid ent_id
        )
        {
            ORC_MatrizHabilidadesDAO dao = new ORC_MatrizHabilidadesDAO();
            return dao.BuscaMatrizHabilidades(mat_nome, ent_id, out totalRecords);
        }

        /// <summary>
        /// Busca as matrizes de habilidade para a combo do UC de matriz de habilidades.
        /// </summary>        
        /// <param name="ent_id">ID da entidade</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelectComboMatrizHabilidades(Guid ent_id)
        {
            ORC_MatrizHabilidadesDAO dao = new ORC_MatrizHabilidadesDAO();
            return dao.SelectComboMatrizHabilidades(ent_id);
        }

        /// <summary>
        /// Seleciona as matrizes por curso/período/componente/calendário
		/// e por padrão
        /// </summary>        
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelectMatrizHabilidades_ByCursoPeriodoDisciplinaPadrao(int cur_id, int crr_id, int crp_id, int cal_id, int tds_id, bool mat_padrao, Guid ent_id)
        {
            ORC_MatrizHabilidadesDAO dao = new ORC_MatrizHabilidadesDAO();
            return dao.SelectMatrizHabilidades_ByCursoPeriodoDisciplinaPadrao(cur_id, crr_id, crp_id, cal_id, tds_id, mat_padrao, ent_id);
        }

        /// <summary>
        /// Verifica se existe uma matriz cadastrada com esse nome
        /// </summary>
        /// <param name="entity">Entidade ORC_MatrizHabilidades</param> 
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaExistente
        (
            ORC_MatrizHabilidades entity
        )
        {
            ORC_MatrizHabilidadesDAO dao = new ORC_MatrizHabilidadesDAO();
            return dao.SelectBy_Nome(entity);
        }
    }
}