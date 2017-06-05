/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.Data;
    using Data.Common;
    using System.Linq;

    #region Estruturas

    /// <summary>
    /// Estrutura para controle das sugestões nos objetivos do currículo.
    /// </summary>
    [Serializable]
    public struct sCurriculoSugestaoObjetivo
    {
        public int cro_id { get; set; }
        public int crs_id { get; set; }
        public string crs_sugestao { get; set; }
        public byte crs_tipo { get; set; }
    }

    #endregion

    /// <summary>
    /// Description: ACA_CurriculoObjetivoSugestao Business Object. 
    /// </summary>
    public class ACA_CurriculoObjetivoSugestaoBO : BusinessBase<ACA_CurriculoObjetivoSugestaoDAO, ACA_CurriculoObjetivoSugestao>
	{
        #region Consulta

        /// <summary>
        /// Retorna as sugestões cadastradas de acordo com o tipo de nível de ensino, disciplina e usuário.
        /// </summary>
        /// <param name="tne_id">Id do tipo de nível de ensino</param>
        /// <param name="tme_id">Id do tipo de modalidade de ensino</param>
        /// <param name="tds_id">Id do tipo de disciplina</param>
        /// <param name="tcp_id">Id do tipo de currículo período</param>
        /// <param name="usu_id">Id do usuário</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sCurriculoSugestaoObjetivo> SelecionaPorNivelEnsinoDisciplinaPeriodoUsuario(int tne_id, int tme_id, int tds_id, int tcp_id, Guid usu_id)
        {
            DataTable dt = new ACA_CurriculoObjetivoSugestaoDAO().SelecionaPorNivelEnsinoDisciplinaPeriodoUsuario(tne_id, tme_id, tds_id, tcp_id, usu_id);
            List<sCurriculoSugestaoObjetivo> retorno = (from DataRow dr in dt.Rows
                                                        select new sCurriculoSugestaoObjetivo
                                                        {
                                                            cro_id = Convert.ToInt32(dr["cro_id"])
                                                            ,
                                                            crs_id = Convert.ToInt32(dr["crs_id"])
                                                            ,
                                                            crs_sugestao = dr["crs_sugestao"].ToString()
                                                            ,
                                                            crs_tipo = Convert.ToByte(dr["crs_tipo"])
                                                        }).ToList();
            return retorno;
        }

        #endregion Consulta			

        #region Salvar

        /// <summary>
        /// Salva a sugestão para o objetivo do currículo.
        /// </summary>
        /// <param name="cro_id">ID do objetivo do currículo</param>
        /// <param name="entity">Entidade do tipo ACA_CurriculoSugestao</param>        
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool Save
        (
            int cro_id
            , ACA_CurriculoSugestao entity
        )
        {
            ACA_CurriculoObjetivoSugestaoDAO dao = new ACA_CurriculoObjetivoSugestaoDAO();
            TalkDBTransaction banco = dao._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                bool novo = entity.IsNew;
                ACA_CurriculoSugestaoBO.Save(entity, banco);
                if (novo)
                {
                    Save(new ACA_CurriculoObjetivoSugestao { cro_id = cro_id, crs_id = entity.crs_id }, banco);
                }
                return true;
            }
            catch (Exception ex)
            {
                banco.Close(ex);
                throw;
            }
            finally
            {
                if (banco.ConnectionIsOpen)
                    banco.Close();
            }
        }

        #endregion Salvar
    }
}