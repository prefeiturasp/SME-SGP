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
    using System.Linq;
    using System.Data;
    #region Estruturas

    [Serializable]
    public struct sAlunoDeficiencia
    {
        public Guid tde_id { get; set; }
        public string tde_nome { get; set; }
        public List<sAlunoDeficienciaDetalhe> lstDeficienciaDetalhe { get; set; }
    }

    [Serializable]
    public struct sAlunoDeficienciaDetalhe
    {
        public int dfd_id { get; set; }
        public string dfd_nome { get; set; }
        public bool possuiDeficienciaDetalhe { get; set; }
    }

    #endregion 

    /// <summary>
    /// Description: CLS_AlunoDeficienciaDetalhe Business Object. 
    /// </summary>
    public class CLS_AlunoDeficienciaDetalheBO : BusinessBase<CLS_AlunoDeficienciaDetalheDAO, CLS_AlunoDeficienciaDetalhe>
	{
        #region Métodos de consulta

        /// <summary>
        /// Seleciona os tipos deficiência e seus detalhes por aluno.
        /// </summary>
        /// <param name="alu_id"></param>
        /// <returns></returns>
        public static List<sAlunoDeficiencia> SelecionaPorAluno(long alu_id)
        {
            return (from DataRow dr in new CLS_AlunoDeficienciaDetalheDAO().SelecionaPorAluno(alu_id).Rows
                    group dr by new Guid(dr["tde_id"].ToString()) into grupo
                    select new sAlunoDeficiencia
                    {
                        tde_id = grupo.Key,
                        tde_nome = grupo.First()["tde_nome"].ToString(),
                        lstDeficienciaDetalhe = grupo.Select(p => (sAlunoDeficienciaDetalhe)GestaoEscolarUtilBO.DataRowToEntity(p, new sAlunoDeficienciaDetalhe())).ToList()
                    }).ToList();
        }

        #endregion Métodos de consulta
    }
}