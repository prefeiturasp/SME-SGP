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
    using System.Linq;
    using Data.Common;
    using Validation.Exceptions;
    #region Estruturas

    /// <summary>
    /// Estrutura para controle dos objetivos do currículo.
    /// </summary>
    [Serializable]
    public struct sCurriculoObjetivo
    {
        public int cro_id { get; set; }
        public string cro_descricao { get; set; }
        public int cro_ordem { get; set; }
        public byte cro_tipo { get; set; }
        public int cro_idPai { get; set; }
    }

    /// <summary>
    /// Tipos de objetivo
    /// </summary>
    public enum ACA_CurriculoObjetivoTipo : byte
    {
        Eixo = 1
        ,

        Topico = 2
        ,

        ObjetivoAprendizagem = 3
    }

    #endregion

    /// <summary>
    /// Description: ACA_CurriculoObjetivo Business Object. 
    /// </summary>
    public class ACA_CurriculoObjetivoBO : BusinessBase<ACA_CurriculoObjetivoDAO, ACA_CurriculoObjetivo>
	{
        #region Consulta

        /// <summary>
        /// Retorna os objetivos do currículo cadastrados de acordo com o tipo de nível de ensino e disciplina.
        /// </summary>
        /// <param name="tne_id">Id do tipo de nível de ensino</param>
        /// <param name="tds_id">Id do tipo de disciplina</param>
        /// <param name="tcp_id">Id do tipo de currículo período</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sCurriculoObjetivo> SelecionaPorNivelEnsinoDisciplinaPeriodo(int tne_id, int tme_id, int tds_id, int tcp_id)
        {
            DataTable dt = new ACA_CurriculoObjetivoDAO().SelecionaPorNivelEnsinoDisciplinaPeriodo(tne_id, tme_id, tds_id, tcp_id);
            List<sCurriculoObjetivo> retorno = (from DataRow dr in dt.Rows
                                                select new sCurriculoObjetivo
                                                {
                                                    cro_id = Convert.ToInt32(dr["cro_id"])
                                                    ,
                                                    cro_descricao = dr["cro_descricao"].ToString()
                                                    ,
                                                    cro_ordem = Convert.ToInt32(dr["cro_ordem"])
                                                    ,
                                                    cro_tipo = Convert.ToByte(dr["cro_tipo"])
                                                    ,
                                                    cro_idPai = Convert.ToInt32(dr["cro_idPai"])
                                                }).ToList();
            return retorno;
        }

        #endregion Consulta		

        #region Salvar

        /// <summary>
        /// Altera a ordem dos objetivos do currículo.
        /// </summary>
        /// <param name="entitySubir">Entidade do tipo ACA_CurriculoObjetivo</param>
        /// <param name="entityDescer">Entidade do tipo ACA_CurriculoObjetivo</param>        
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool SaveOrdem
        (
            ACA_CurriculoObjetivo entityDescer
            , ACA_CurriculoObjetivo entitySubir
        )
        {
            ACA_CurriculoObjetivoDAO dao = new ACA_CurriculoObjetivoDAO();
            TalkDBTransaction banco = dao._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                if (entityDescer.Validate())
                    Save(entityDescer, banco);
                else
                    throw new ValidationException(entityDescer.PropertiesErrorList[0].Message);

                if (entitySubir.Validate())
                    Save(entitySubir, banco);
                else
                    throw new ValidationException(entitySubir.PropertiesErrorList[0].Message);

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