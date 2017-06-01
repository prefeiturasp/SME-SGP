/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.ComponentModel;
    using Validation.Exceptions;
    using Data.Common;
    using System;
    using System.Data;
    using System.Collections.Generic;
    using System.Linq;

    #region Estruturas

    /// <summary>
    /// Estrutura para controle dos capítulos do currículo.
    /// </summary>
    [Serializable]
    public struct sCurriculoCapitulo
    {
        public int crc_id { get; set; }
        public int tds_id { get; set; }
        public string crc_titulo { get; set; }
        public string crc_descricao { get; set; }
        public int crc_ordem { get; set; }
    }

    #endregion

    /// <summary>
    /// Description: ACA_CurriculoCapitulo Business Object. 
    /// </summary>
    public class ACA_CurriculoCapituloBO : BusinessBase<ACA_CurriculoCapituloDAO, ACA_CurriculoCapitulo>
	{
        #region Consulta

        /// <summary>
        /// Retorna os capítulos do currículo cadastrados de acordo com o tipo de nível de ensino e disciplina.
        /// </summary>
        /// <param name="tne_id">Id do tipo de nível de ensino</param>
        /// <param name="tds_id">Id do tipo de disciplina</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sCurriculoCapitulo> SelecionaPorNivelEnsinoDisciplina(int tne_id, int tds_id)
        {
            DataTable dt = new ACA_CurriculoCapituloDAO().SelecionaPorNivelEnsinoDisciplina(tne_id, tds_id);
            List<sCurriculoCapitulo> retorno = (from DataRow dr in dt.Rows
                            select new sCurriculoCapitulo
                            {
                                crc_id = Convert.ToInt32(dr["crc_id"])
                                ,
                                tds_id = Convert.ToInt32(dr["tds_id"])
                                ,
                                crc_titulo = dr["crc_titulo"].ToString()
                                ,
                                crc_descricao = dr["crc_descricao"].ToString()
                                ,
                                crc_ordem = Convert.ToInt32(dr["crc_ordem"])
                            }).ToList();
            return retorno;
        }

        #endregion Consulta

        #region Salvar

        /// <summary>
        /// Altera a ordem dos capítulos do currículo.
        /// </summary>
        /// <param name="entitySubir">Entidade do tipo ACA_CurriculoCapitulo</param>
        /// <param name="entityDescer">Entidade do tipo ACA_CurriculoCapitulo</param>        
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool SaveOrdem
        (
            ACA_CurriculoCapitulo entityDescer
            , ACA_CurriculoCapitulo entitySubir
        )
        {
            ACA_CurriculoCapituloDAO dao = new ACA_CurriculoCapituloDAO();
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