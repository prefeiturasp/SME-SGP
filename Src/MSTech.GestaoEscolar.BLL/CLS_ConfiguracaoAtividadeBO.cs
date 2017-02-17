/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

using MSTech.Data.Common;
    using MSTech.Validation.Exceptions;
    using System.ComponentModel;

	/// <summary>
	/// Description: CLS_ConfiguracaoAtividade Business Object. 
	/// </summary>
	public class CLS_ConfiguracaoAtividadeBO : BusinessBase<CLS_ConfiguracaoAtividadeDAO, CLS_ConfiguracaoAtividade>
	{
        #region Enumerador

        /// <summary>
        /// Enumerador das situações.
        /// </summary>
        public enum eSituacao 
        {
            Ativo = 1
            ,
            Excluido = 3
        }

        #endregion

        [Serializable]
        public struct ConfiguracaoAtividade
        {
            public int crp_id { get; set; }
            public int dis_id { get; set; }
            public string dis_nome { get; set; }
            public int caa_id { get; set; }
            public int ativDiversQtd { get; set; }
            public bool ativDiversRecuperacao { get; set; }
            public int instruAvalQtd { get; set; }
            public bool instruAvalRecuperacao { get; set; }
            public bool caa_possuiAtividadeAutomatica { get; set; }
        }

        public struct CurriculoPeriodoConfiguracaoAtividade
        {
            public int crp_id { get; set; }
            public string crp_descricao { get; set; }
        }
        
        /// <summary>
        /// Seleciona os dados pelo ano, curso, currículo e período
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="crp_id">ID do período</param>
        /// <param name="anoLetivo">Ano letivo da configuração</param>
        /// <returns></returns>
        public static List<ConfiguracaoAtividade> GetSelectBy_CurriculoPeriodo(
              int cur_id
            , int crr_id
            , int crp_id
            , int anoLetivo
            )
        {
            CLS_ConfiguracaoAtividadeDAO dao = new CLS_ConfiguracaoAtividadeDAO();
            DataTable dt = dao.GetSelectBy_CurriculoPeriodo(cur_id, crr_id, crp_id, anoLetivo);

            List<ConfiguracaoAtividade> listConfgAtiv = new List<ConfiguracaoAtividade>();
            ConfiguracaoAtividade configuracaoAtividade;

            foreach (DataRow row in dt.Rows)
            {
                configuracaoAtividade = new ConfiguracaoAtividade
                {
                    crp_id = Convert.ToInt32(row["crp_id"]),
                    dis_id = Convert.ToInt32(row["dis_id"]),
                    dis_nome = row["dis_nome"].ToString(),
                    caa_id = Convert.ToInt32(row["caa_id"]),
                    ativDiversQtd = Convert.ToInt32(row["ativDiversQtd"]),
                    ativDiversRecuperacao = Convert.ToBoolean(row["ativDiversRecuperacao"]),
                    instruAvalQtd = Convert.ToInt32(row["instruAvalQtd"]),
                    instruAvalRecuperacao = Convert.ToBoolean(row["instruAvalRecuperacao"]),
                    caa_possuiAtividadeAutomatica = Convert.ToBoolean(row["caa_possuiAtividadeAutomatica"])
                };

                listConfgAtiv.Add(configuracaoAtividade);                
            }

            return listConfgAtiv;
        }

        /// <summary>
        /// Seleciona os dados pelo ano, curso, currículo, período e escola
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="crp_id">ID do período</param>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade da escola</param>
        /// <param name="anoLetivo">Ano letivo da configuração</param>
        /// <returns></returns>
        public static List<ConfiguracaoAtividade> GetSelectBy_CurriculoPeriodoEscola(
              int cur_id
            , int crr_id
            , int crp_id
            , int esc_id
            , int uni_id
            , int anoLetivo
            )
        {
            CLS_ConfiguracaoAtividadeDAO dao = new CLS_ConfiguracaoAtividadeDAO();
            DataTable dt = dao.GetSelectBy_CurriculoPeriodoEscola(cur_id, crr_id, crp_id, esc_id, uni_id, anoLetivo);

            List<ConfiguracaoAtividade> listConfgAtiv = new List<ConfiguracaoAtividade>();
            ConfiguracaoAtividade configuracaoAtividade;

            foreach (DataRow row in dt.Rows)
            {
                configuracaoAtividade = new ConfiguracaoAtividade
                {
                    crp_id = Convert.ToInt32(row["crp_id"]),
                    dis_id = Convert.ToInt32(row["dis_id"]),
                    dis_nome = row["dis_nome"].ToString(),
                    caa_id = Convert.ToInt32(row["caa_id"]),
                    ativDiversQtd = Convert.ToInt32(row["ativDiversQtd"]),
                    ativDiversRecuperacao = Convert.ToBoolean(row["ativDiversRecuperacao"]),
                    instruAvalQtd = Convert.ToInt32(row["instruAvalQtd"]),
                    instruAvalRecuperacao = Convert.ToBoolean(row["instruAvalRecuperacao"]),
                    caa_possuiAtividadeAutomatica = Convert.ToBoolean(row["caa_possuiAtividadeAutomatica"])
                };

                listConfgAtiv.Add(configuracaoAtividade);
            }

            return listConfgAtiv;
        }

        /// <summary>
        /// Atualiza a situação de configuração de atividade por ano, escola, curso e período.
        /// </summary>
        /// <param name="caa_anoLetivo">Ano letivo</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade de escola</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="crp_id">ID do período</param>
        /// <param name="caa_situacao">Situação</param>
        /// <returns></returns>
        public static bool UpdateSituacaoPorEscolaCursoPeriodoAno
        (
             int caa_anoLetivo
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , eSituacao caa_situacao
        )
        {
            return new CLS_ConfiguracaoAtividadeDAO().UpdateSituacaoPorEscolaCursoPeriodoAno
                                                        (
                                                            caa_anoLetivo,
                                                            esc_id,
                                                            uni_id,
                                                            cur_id,
                                                            crr_id,
                                                            crp_id,
                                                            (byte)caa_situacao
                                                        );
        }
        
    }
}