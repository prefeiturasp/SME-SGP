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
    using System;
    using System.Collections.Generic;
    using MSTech.Validation.Exceptions;
    using System.Linq;

    /// <summary>
	/// Description: TUR_TurmaDisciplinaMultisseriada Business Object. 
	/// </summary>
	public class TUR_TurmaDisciplinaMultisseriadaBO : BusinessBase<TUR_TurmaDisciplinaMultisseriadaDAO, TUR_TurmaDisciplinaMultisseriada>
	{

        /// <summary>
        /// Retorna as turmas do aluno de acordo com os filtros informados e com a permissão do usuário
        /// </summary>
        /// <param name="usu_id">ID do usuário - obrigatório</param>
        /// <param name="gru_id">ID do grupo</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="doc_id"></param>
        /// <param name="ttn_id"></param>
        /// <param name="tur_codigo">Código da turma</param>
        /// <param name="ent_id">ID da entidade - obrigatório</param>
        /// <param name="uad_idSuperior">ID da unidade superior a escola</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crp_id">ID do período do currículo</param>
        /// <param name="crr_id">ID do curriculo do curso</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_Pesquisa_TurmasDisciplinasMultisseriada
        (
            Guid usu_id
            , Guid gru_id
            , int esc_id
            , int uni_id
            , int doc_id
            , int ttn_id
            , string tur_codigo
            , Guid ent_id
            , Guid uad_idSuperior
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
        )
        {
            bool MostraCodigoEscola = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);

            totalRecords = 0;
            TUR_TurmaDisciplinaMultisseriadaDAO dao = new TUR_TurmaDisciplinaMultisseriadaDAO();
            return dao.GetSelectBy_Pesquisa_TurmasDisciplinasMultisseriada
            (
                usu_id
                , gru_id
                , esc_id
                , uni_id
                , doc_id
                , ttn_id
                , tur_codigo
                , ent_id
                , uad_idSuperior
                , MostraCodigoEscola
                , cal_id
                , cur_id
                , crr_id
                , crp_id
                , out totalRecords
            );
        }

        public static bool SaveMultisseriadaDocente
        (
            List<TUR_TurmaDisciplinaMultisseriada> list
            , List<TUR_TurmaDisciplinaMultisseriada> listBanco
        )
        {
            TUR_TurmaDisciplinaMultisseriadaDAO dao = new TUR_TurmaDisciplinaMultisseriadaDAO();

            try
            {
                // Valida se o usuário incluiu pelo menos um aluno na turma multisseriada do docente
                if (list.Count <= 0 && listBanco.Count <= 0)
                    throw new ValidationException("Selecione pelo menos um aluno para efetuar a matrícula em turma multisseriada do docente.");

                // Agrupa a lista de matriculas por turma e ordena por nome do aluno
                var x = from TUR_TurmaDisciplinaMultisseriada tdm in list
                        orderby tdm.pes_nome
                        group tdm by tdm.tur_codigo into g
                        select new { g.Key, dados = g };

                // Inclui ou altera a matricula na turma multisseriada do docente
                // sempre atualizando o número de chamada
                foreach (var turma in x)
                {
                    foreach (var entity in turma.dados)
                    {
                        TUR_TurmaDisciplinaMultisseriada entTdm = new TUR_TurmaDisciplinaMultisseriada
                                                                       {
                                                                           tud_idDocente = entity.tud_idDocente,
                                                                           alu_id = entity.alu_id,
                                                                           mtu_id = entity.mtu_id,
                                                                           mtd_id = entity.mtd_id
                                                                       };
                        TUR_TurmaDisciplinaMultisseriadaBO.GetEntity(entTdm);
            
                        if(entTdm.IsNew)
                        {
                            Save(entity);
                        }
                    }
                }

                // Exclui as matrículas inexistentes na lista do usuário                               
                foreach (TUR_TurmaDisciplinaMultisseriada mtd in listBanco)
                {
                    if (!list.Exists(p => p.alu_id == mtd.alu_id && p.mtu_id == mtd.mtu_id && p.mtd_id == mtd.mtd_id))
                        Delete(mtd, dao._Banco);
                }

                return true;
            }
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}