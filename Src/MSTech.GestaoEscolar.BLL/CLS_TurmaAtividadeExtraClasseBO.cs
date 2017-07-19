/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.Data;
    using System.Linq;
    using Validation.Exceptions;
    using System;
    using System.Collections.Generic;
    using Data.Common;

    /// <summary>
    /// Description: CLS_TurmaAtividadeExtraClasse Business Object. 
    /// </summary>
    public class CLS_TurmaAtividadeExtraClasseBO : BusinessBase<CLS_TurmaAtividadeExtraClasseDAO, CLS_TurmaAtividadeExtraClasse>
	{
        #region Métodos de verificação

        /// <summary>
        /// Verifica se já existe a atividade por disciplina, nome, tipo e bimestre.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool VerificaExistePorDisciplinaNomeTipoBimestre(CLS_TurmaAtividadeExtraClasse entity)
        {
            return new CLS_TurmaAtividadeExtraClasseDAO().VerificaExistePorDisciplinaNomeTipoBimestre(entity.tud_id, entity.tae_id, entity.tae_nome, entity.tav_id, entity.tpc_id);
        }

        /// <summary>
        /// Verifica se a carga horaria da atividade extraclasse vai estourar o permitido pelo curso no ano letivo
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="tae_id"></param>
        /// <param name="tae_cargaHoraria"></param>
        /// <param name="cur_cargaHorariaExtraClasse"></param>
        /// <param name="cargaAtividadeExtraTotal"></param>
        /// <returns></returns>
        public static bool VerificaCargaHorariaCursoCalendario(CLS_TurmaAtividadeExtraClasse entity, int cal_id, out decimal che_cargaHoraria, out decimal cargaAtividadeExtraTotal)
        {
            return new CLS_TurmaAtividadeExtraClasseDAO().VerificaCargaHorariaCursoCalendario(entity.tud_id, cal_id, entity.tpc_id, entity.tae_id, entity.tae_cargaHoraria, out che_cargaHoraria, out cargaAtividadeExtraTotal);
        }

        #endregion Métodos de verificação

        #region Métodos de consulta

        /// <summary>
        /// Retorna as todas as Atividades extraclasse(por disciplina) com as notas do aluno
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="tpc_id"></param>
        /// <param name="dtTurmas"></param>
        /// <returns></returns>
        public static DataTable SelecionaPorPeriodoDisciplina_Alunos(long tud_id, int tpc_id, bool usuario_superior, byte tdt_posicao, string tur_ids = null)
        {
            using (DataTable dtTurmas = TUR_Turma.TipoTabela_Turma())
            {
                if (!string.IsNullOrEmpty(tur_ids))
                {
                    tur_ids.Split(';').ToList().ForEach
                        (
                            p =>
                            {
                                DataRow dr = dtTurmas.NewRow();
                                dr["tur_id"] = p.ToString();
                                dtTurmas.Rows.Add(dr);
                            }
                        );
                }

                return new CLS_TurmaAtividadeExtraClasseDAO().SelecionaPorPeriodoDisciplina_Alunos(tud_id, tpc_id, usuario_superior, tdt_posicao, dtTurmas);
            }
        }

        /// <summary>
        /// Retorna as disciplinas relacionadas pela atividade extraclasse.
        /// </summary>
        /// <param name="taer_id">ID do relacionamento entre atividades extraclasse.</param>
        /// <returns></returns>
        public static List<TUR_TurmaDisciplina> SelecionaDisciplinaAtividadeExtraclasseRelacionada(Guid taer_id)
        {
            DataTable dt = new CLS_TurmaAtividadeExtraClasseDAO().SelecionaDisciplinaAtividadeExtraclasseRelacionada(taer_id);
            List<TUR_TurmaDisciplina> dados = new List<TUR_TurmaDisciplina>();
            foreach (DataRow dr in dt.Rows)
            {
                TUR_TurmaDisciplina ent = new TUR_TurmaDisciplina();
                dados.Add(new TUR_TurmaDisciplinaDAO().DataRowToEntity(dr, ent));
            }
            return dados;
        }

        /// <summary>
        /// Retorna atividades extraclasse relacionadas.
        /// </summary>
        /// <param name="taer_id">ID do relacionamento entre atividades extraclasse.</param>
        /// <returns></returns>
        public static List<CLS_TurmaAtividadeExtraClasse> SelecionaAtividadeExtraclasseRelacionada(Guid taer_id)
        {
            CLS_TurmaAtividadeExtraClasseDAO dao = new CLS_TurmaAtividadeExtraClasseDAO();
            DataTable dt = dao.SelecionaAtividadeExtraclasseRelacionada(taer_id);
            List<CLS_TurmaAtividadeExtraClasse> dados = new List<CLS_TurmaAtividadeExtraClasse>();
            foreach (DataRow dr in dt.Rows)
            {
                CLS_TurmaAtividadeExtraClasse ent = new CLS_TurmaAtividadeExtraClasse();
                dados.Add(dao.DataRowToEntity(dr, ent));
            }
            return dados;
        }

        #endregion Métodos de consulta

        #region Métodos de alteração/inclusão

        /// <summary>
        /// Realiza as validações necessárias e salva a atividade extraclasse.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool Salvar(CLS_TurmaAtividadeExtraClasse entity, int cal_id, bool fechamentoAutomatico, Guid ent_id, List<long> lstDisciplinas, Guid usu_id, long tur_id)
        {
            TalkDBTransaction banco = new CLS_TurmaAtividadeExtraClasseDAO()._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);
            try
            {
                List<CLS_TurmaAtividadeExtraClasse> lstEntities = new List<CLS_TurmaAtividadeExtraClasse>();

                bool isNew = entity.IsNew;
                if (lstDisciplinas.Count > 0)
                {
                    if (isNew)
                    {
                        CLS_TurmaAtividadeExtraClasseRelacionada relacionada = new CLS_TurmaAtividadeExtraClasseRelacionada();
                        relacionada.IsNew = true;
                        relacionada.usu_id = usu_id;
                        if (CLS_TurmaAtividadeExtraClasseRelacionadaBO.Save(relacionada, banco))
                        {
                            entity.taer_id = relacionada.taer_id;
                            lstDisciplinas.ForEach(p =>
                            {
                                if (p != entity.tud_id)
                                {
                                    lstEntities.Add(new CLS_TurmaAtividadeExtraClasse { tud_id = p, tae_id = -1, taer_id = entity.taer_id, IsNew = true });
                                }
                            });
                        }
                        else
                        {
                            throw new Exception("Erro ao tentar salvar componente curricular relacionado.");
                        }
                    }
                    else
                    {
                        List<CLS_TurmaAtividadeExtraClasse> lstRelacionadas = SelecionaAtividadeExtraclasseRelacionada(entity.taer_id);
                        if (lstRelacionadas.Count > 0)
                        {
                            lstEntities.AddRange(lstRelacionadas.FindAll(p => p.tud_id != entity.tud_id));
                        }
                    }

                    lstEntities.ForEach(p =>
                    {
                        p.tpc_id = entity.tpc_id;
                        p.tav_id = entity.tav_id;
                        p.tae_nome = entity.tae_nome;
                        p.tae_descricao = entity.tae_descricao;
                        p.tae_cargaHoraria = entity.tae_cargaHoraria;
                        p.tdt_posicao = entity.tdt_posicao;
                        p.IsNew = p.tae_id <= 0;
                    });
                }

                lstEntities.Add(entity);
                List<TUR_TurmaDisciplina> turmaDisciplina = TUR_TurmaDisciplinaBO.GetSelectBy_Turma(tur_id, null, GestaoEscolarUtilBO.MinutosCacheLongo);

                CLS_TurmaAtividadeExtraClasseDAO dao = new CLS_TurmaAtividadeExtraClasseDAO();
                dao._Banco = banco;
                foreach (CLS_TurmaAtividadeExtraClasse atividade in lstEntities)
                {
                    if (atividade.Validate())
                    {
                        if (VerificaExistePorDisciplinaNomeTipoBimestre(atividade))
                        {
                            throw new ValidationException(string.Format("Já existe uma atividade extraclasse com o mesmo nome e tipo para o componente curricular '{0}' no bimestre.", TUR_TurmaDisciplinaBO.GetEntity(new TUR_TurmaDisciplina { tud_id = atividade.tud_id }).tud_nome));
                        }

                        decimal dis_cargaHorariaExtraClasse = 0, cargaAtividadeExtraTotal = 0;

                        if (VerificaCargaHorariaCursoCalendario(atividade, cal_id, out dis_cargaHorariaExtraClasse, out cargaAtividadeExtraTotal))
                        {
                            throw new ValidationException(string.Format("A soma de carga horária de atividades extraclasse ({0}) está acima da máxima permitida pelo componente curricular '{1}' ({2}).", cargaAtividadeExtraTotal, TUR_TurmaDisciplinaBO.GetEntity(new TUR_TurmaDisciplina { tud_id = atividade.tud_id }).tud_nome, dis_cargaHorariaExtraClasse));
                        }

                        if (dao.Salvar(atividade))
                        {
                            // Caso o fechamento seja automático, grava na fila de processamento.
                            if (!isNew && fechamentoAutomatico 
                                && turmaDisciplina.Find(t => t.tud_id == atividade.tud_id).tud_tipo != (byte)TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia 
                                && atividade.tpc_id != ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_PERIODO_CALENDARIO_RECESSO, ent_id))
                            {
                                CLS_AlunoFechamentoPendenciaBO.SalvarFilaFrequencia(atividade.tud_id, atividade.tpc_id, banco);
                            }
                        }
                    }
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

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }

        #endregion Métodos de alteração/inclusão

        #region Métodos de exclusão

        /// <summary>
        /// Exclui a atividade extraclasse
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool Deletar(CLS_TurmaAtividadeExtraClasse entity)
        {
            TalkDBTransaction banco = new CLS_TurmaAtividadeExtraClasseDAO()._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);
            try
            {
                CLS_TurmaAtividadeExtraClasseDAO dao = new CLS_TurmaAtividadeExtraClasseDAO();
                dao._Banco = banco;
                if (entity.taer_id != Guid.Empty)
                {
                    List<CLS_TurmaAtividadeExtraClasse> lstRelacionadas = SelecionaAtividadeExtraclasseRelacionada(entity.taer_id);
                    lstRelacionadas.ForEach(p => dao.Deletar(p.tud_id, p.tae_id));
                }
                return dao.Deletar(entity.tud_id, entity.tae_id);
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

        #endregion
    }
}