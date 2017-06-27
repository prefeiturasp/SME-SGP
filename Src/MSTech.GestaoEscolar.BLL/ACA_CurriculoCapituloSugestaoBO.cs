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
    using Data.Common;
    using System.Data;
    using System.Collections.Generic;
    using System.Linq;

    #region Estruturas

    /// <summary>
    /// Estrutura para controle das sugestões nos capítulos do currículo.
    /// </summary>
    [Serializable]
    public struct sCurriculoSugestaoCapitulo
    {
        public int crc_id { get; set; }
        public int crs_id { get; set; }
        public string crs_sugestao { get; set; }
        public byte crs_tipo { get; set; }
        public string pes_nome { get; set; }
        public DateTime data { get; set; }
    }

    #endregion

    /// <summary>
    /// Description: ACA_CurriculoCapituloSugestao Business Object. 
    /// </summary>
    public class ACA_CurriculoCapituloSugestaoBO : BusinessBase<ACA_CurriculoCapituloSugestaoDAO, ACA_CurriculoCapituloSugestao>
	{
        #region Consulta

        /// <summary>
        /// Retorna as sugestões cadastradas de acordo com o tipo de nível de ensino, disciplina e usuário.
        /// </summary>
        /// <param name="tne_id">Id do tipo de nível de ensino</param>
        /// <param name="tme_id">Id do tipo de modalidade de ensino</param>
        /// <param name="tds_id">Id do tipo de disciplina</param>
        /// <param name="usu_id">Id do usuário</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sCurriculoSugestaoCapitulo> SelecionaPorNivelEnsinoDisciplinaUsuario(int tne_id, int tme_id, int tds_id, Guid usu_id)
        {
            DataTable dt = new ACA_CurriculoCapituloSugestaoDAO().SelecionaPorNivelEnsinoDisciplinaUsuario(tne_id, tme_id, tds_id, usu_id);
            List<sCurriculoSugestaoCapitulo> retorno = (from DataRow dr in dt.Rows
                                                select new sCurriculoSugestaoCapitulo
                                                {
                                                    crc_id = Convert.ToInt32(dr["crc_id"])
                                                    ,
                                                    crs_id = Convert.ToInt32(dr["crs_id"])
                                                    ,
                                                    crs_sugestao = dr["crs_sugestao"].ToString()
                                                    ,
                                                    crs_tipo = Convert.ToByte(dr["crs_tipo"])
                                                    ,
                                                    pes_nome = Convert.ToString(dr["pes_nome"])
                                                    ,
                                                    data = Convert.ToDateTime(dr["data"])
                                                }).ToList();
            return retorno;
        }

        #endregion Consulta

        #region Salvar

        /// <summary>
        /// Salva a sugestão para o capítulo do currículo.
        /// </summary>
        /// <param name="crc_id">ID do capítulo do currículo</param>
        /// <param name="entity">Entidade do tipo ACA_CurriculoSugestao</param>        
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool Save
        (
            int crc_id
            , ACA_CurriculoSugestao entity
        )
        {
            ACA_CurriculoCapituloSugestaoDAO dao = new ACA_CurriculoCapituloSugestaoDAO();
            TalkDBTransaction banco = dao._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                bool novo = entity.IsNew;
                ACA_CurriculoSugestaoBO.Save(entity, banco);
                if (novo)
                {
                    Save(new ACA_CurriculoCapituloSugestao { crc_id = crc_id, crs_id = entity.crs_id }, banco);
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