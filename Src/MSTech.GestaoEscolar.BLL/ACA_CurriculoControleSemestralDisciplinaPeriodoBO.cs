/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/
using MSTech.Business.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL;
using System.Collections.Generic;
using System.Data;
using System;
using System.Linq;

namespace MSTech.GestaoEscolar.BLL
{
	

	/// <summary>
	/// Description: ACA_CurriculoControleSemestralDisciplinaPeriodo Business Object. 
	/// </summary>
	public class ACA_CurriculoControleSemestralDisciplinaPeriodoBO : BusinessBase<ACA_CurriculoControleSemestralDisciplinaPeriodoDAO, ACA_CurriculoControleSemestralDisciplinaPeriodo>
    {
        #region Estruturas

        /// <summary>
        /// Estrutura da matriz curricular
        /// </summary>
        [Serializable]
        public class MatrizCurricular
        {
            public long tud_id { get; set; }

            public int tpc_id { get; set; }

            public bool csp_frequencia { get; set; }

            public bool csp_nota { get; set; }
        }

        #endregion

        #region Métodos de consulta
       
        /// <summary>
        /// Retorna a matriz curricular das disciplinas da turma
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="cur_efetivacaoSemestral">flag que indica se o curso permite efetivação semestral</param>        
        /// <returns></returns>
        public static List<MatrizCurricular> SelecionaMatrizCurricularTurma
        (
            long tur_id
            , out bool efetivacaoSemestral
        )
        {
            List<TUR_TurmaCurriculo> listaCurriculo = TUR_TurmaCurriculoBO.GetSelectBy_Turma(tur_id, GestaoEscolarUtilBO.MinutosCacheLongo);

            ACA_Curso entityCurso = new ACA_Curso { cur_id = listaCurriculo[0].cur_id};
            ACA_CursoBO.GetEntity(entityCurso);

            List<MatrizCurricular> lista = new List<MatrizCurricular>();

            if (entityCurso.cur_efetivacaoSemestral)
            {
                efetivacaoSemestral = true;

                ACA_CurriculoControleSemestralDisciplinaPeriodoDAO dao = new ACA_CurriculoControleSemestralDisciplinaPeriodoDAO();
                DataTable dt = dao.SelecionaMatrizCurricularTurma(tur_id);

                foreach (DataRow dr in dt.Rows)
                {
                    MatrizCurricular ent = new MatrizCurricular();

                    ent.tud_id = Convert.ToInt64(dr["tud_id"]);
                    ent.tpc_id = Convert.ToInt32(dr["tpc_id"]);
                    ent.csp_nota = Convert.ToBoolean(dr["csp_nota"]);
                    ent.csp_frequencia = Convert.ToBoolean(dr["csp_frequencia"]);

                    lista.Add(ent);
                }
            }
            else
            {
                efetivacaoSemestral = false;
            }

            return lista;
        }

        /// <summary>
        /// Retorna a matriz curricular das disciplinas da turma
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="cur_efetivacaoSemestral">flag que indica se o curso permite efetivação semestral</param>        
        /// <returns></returns>
        public static List<MatrizCurricular> SelecionaUltimoPeriodoNotaTurma
        (
            long tur_id
        )
        {
            ACA_CurriculoControleSemestralDisciplinaPeriodoDAO dao = new ACA_CurriculoControleSemestralDisciplinaPeriodoDAO();
            DataTable dt = dao.SelecionaUltimoPeriodoNotaTurma(tur_id);

            return dt.Rows.Count > 0 ?
                dt.Rows.Cast<DataRow>().AsParallel().Select(dr => new MatrizCurricular { tud_id = Convert.ToInt64(dr["tud_id"]), tpc_id = Convert.ToInt32(dr["tpc_id"]) }).ToList() :
                new List<MatrizCurricular>();
        }

        #endregion
     
	}
}