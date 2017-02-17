/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;
    using MSTech.Business.Common;
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.Validation.Exceptions;
	
	/// <summary>
	/// Description: CLS_TurmaAulaRegencia Business Object. 
	/// </summary>
	public class CLS_TurmaAulaRegenciaBO : BusinessBase<CLS_TurmaAulaRegenciaDAO, CLS_TurmaAulaRegencia>
	{
        /// <summary>
        /// Salva anotações e recursos para os alunos.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="listTurmaAulaRecursoRegencia"></param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <param name="turmaIntegral">Indica se a turma é integral.</param>
        /// <param name="fechamentoAutomatico">Indica se o fechamento é automático.</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool SalvarAulaAnotacoesRecursos
        (
            CLS_TurmaAulaRegencia entity
            , List<CLS_TurmaAulaRecursoRegencia> listTurmaAulaRecursoRegencia
            , Guid ent_id
            , bool turmaIntegral
            , bool fechamentoAutomatico
            , List<VigenciaCriacaoAulas> vigenciasCriacaoAulas
            , CLS_TurmaAula entityTurmaAula = null
            , List<CLS_TurmaAulaOrientacaoCurricular> listOriCurTurAula = null
            , Guid usu_id = new Guid()
            , byte origemLogAula = 0
            , byte tipoLogAula = 0
        )
        {
            CLS_TurmaAulaRecursoDAO dao = new CLS_TurmaAulaRecursoDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                if (entityTurmaAula != null && !CLS_TurmaAulaBO.Save(entityTurmaAula, dao._Banco))
                    return false;

                if (listOriCurTurAula != null)
                    CLS_TurmaAulaOrientacaoCurricularBO.Salvar(listOriCurTurAula, dao._Banco);

                return SalvarAulaAnotacoesRecursos(entity, listTurmaAulaRecursoRegencia, dao._Banco, ent_id, turmaIntegral, fechamentoAutomatico, vigenciasCriacaoAulas, false, null, usu_id, origemLogAula, tipoLogAula);
            }
            catch (Exception err)
            {
                dao._Banco.Close(err);
                throw;
            }
            finally
            {
                dao._Banco.Close();
            }
        }

        /// <summary>
        /// Salva anotações e recursos para os alunos.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="listTurmaAulaRecursoRegencia"></param>
        /// <param name="banco"></param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <param name="turmaIntegral">Indica se a turma é integral.</param>
        /// <param name="fechamentoAutomatico">Indica se o fechamento é automático.</param>
        /// <param name="salvarFaltasAlunos"></param>
        /// <returns></returns>
        private static bool SalvarAulaAnotacoesRecursos
        (
            CLS_TurmaAulaRegencia entity
            , List<CLS_TurmaAulaRecursoRegencia> listTurmaAulaRecursoRegencia
            , TalkDBTransaction banco
            , Guid ent_id
            , bool turmaIntegral
            , bool fechamentoAutomatico
            , List<VigenciaCriacaoAulas> vigenciasCriacaoAulas
            , bool salvarFaltasAlunos = false
            , List<CLS_TurmaAulaOrientacaoCurricular> listOriCurTurAula = null
            , Guid usu_id = new Guid()
            , byte origemLogAula = 0
            , byte tipoLogAula = 0
        )
        {
            string mensagemInfo;

            if (entity.tuf_data == new DateTime())
                throw new ValidationException("Data da aula é obrigatório.");

            // Chama método padrão para salvar a aula
            if (entity.Validate())
                Save(entity, banco);
            else
                throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));

            if (origemLogAula > 0)
            {
                DateTime dataLogAula = DateTime.Now;
                LOG_TurmaAula_Alteracao entLogAula = new LOG_TurmaAula_Alteracao
                {
                    tud_id = entity.tud_id,
                    tau_id = entity.tau_id,
                    usu_id = usu_id,
                    lta_origem = origemLogAula,
                    lta_tipo = tipoLogAula,
                    lta_data = dataLogAula
                };

                LOG_TurmaAula_AlteracaoBO.Save(entLogAula, banco);
            }

            // regravo a CLS_TurmaAula apenas para atualizar o campo usu_idDocenteAlteracao, quando preenchido
            if (entity.usu_idDocenteAlteracao != null)
            {
                CLS_TurmaAula entityAula = new CLS_TurmaAula
                {
                    tud_id = entity.tud_id
                  ,
                    tau_id = entity.tau_id
                };
                CLS_TurmaAulaBO.GetEntity(entityAula);

                if (!entityAula.IsNew)
                {
                    entityAula.usu_idDocenteAlteracao = entity.usu_idDocenteAlteracao;
                    CLS_TurmaAulaBO.Save(entityAula, banco, out mensagemInfo, ent_id, turmaIntegral, fechamentoAutomatico, vigenciasCriacaoAulas);
                }
            }

            //Carrega Recursos gravados no banco
            List<CLS_TurmaAulaRecursoRegencia> listaBanco = CLS_TurmaAulaRecursoRegenciaBO.GetSelectBy_Turma_Aula_DisciplinaComponente(entity.tud_id
                                                                                                                                       , entity.tau_id
                                                                                                                                       , entity.tud_idFilho);
            //busca registros que devem ser excluidos
            IEnumerable<Int32> dadosTela =
            (from CLS_TurmaAulaRecursoRegencia dr in listTurmaAulaRecursoRegencia.AsEnumerable()
             orderby dr.rsa_id descending
             select dr.rsa_id).AsEnumerable();

            IEnumerable<Int32> dadosExcluir =
                (from CLS_TurmaAulaRecursoRegencia t in listaBanco.AsEnumerable()
                 orderby t.rsa_id descending
                 select t.rsa_id).Except(dadosTela);

            IList<Int32> dadosDif = dadosExcluir.ToList();
            //deleta registros que foram desmarcados
            for (int i = 0; i < dadosDif.Count; i++)
            {
                CLS_TurmaAulaRecursoRegenciaBO.Delete_Byrsa_id(entity.tud_id, entity.tau_id, dadosDif[i]);
            }

            //busca registro que devem ser alterados
            IEnumerable<Int32> dadosBanco =
                (from CLS_TurmaAulaRecursoRegencia t in listaBanco.AsEnumerable()
                 orderby t.rsa_id descending
                 select t.rsa_id).AsEnumerable();

            IEnumerable<Int32> dadosAlterar =
                (from CLS_TurmaAulaRecursoRegencia t in listTurmaAulaRecursoRegencia.AsEnumerable()
                 orderby t.rsa_id descending
                 select t.rsa_id).Intersect(dadosBanco);

            IList<Int32> dadosAlte = dadosAlterar.ToList();
            CLS_TurmaAulaRecursoRegencia entityAltera;
            for (int i = 0; i < dadosAlte.Count; i++)
            {
                entityAltera = listTurmaAulaRecursoRegencia.Find(p => p.rsa_id == dadosAlte[i]);
                entityAltera.trr_dataAlteracao = DateTime.Now;
                CLS_TurmaAulaRecursoRegenciaBO.Update_Byrsa_id(entityAltera);
                listTurmaAulaRecursoRegencia.Remove(entityAltera);
            }

            // Salva as recursos utilizados na aula
            foreach (CLS_TurmaAulaRecursoRegencia aux in listTurmaAulaRecursoRegencia)
            {
                aux.tau_id = entity.tau_id;
                if (aux.Validate())
                    CLS_TurmaAulaRecursoRegenciaBO.Salvar(aux, banco);
                else
                    throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(aux));
                }

            return true;
        }

        /// <summary>
        /// Retorna um datarow com dados do plano de aula de um componente da regência.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static DataRow TurmaAulaRegenciaToDataRow(CLS_TurmaAulaRegencia entity, DataRow dr, DateTime tuf_dataAlteracao = new DateTime())
        {
            if (entity.idAula > 0)
                dr["idAula"] = entity.idAula;
            else
                dr["idAula"] = DBNull.Value;

            dr["tud_id"] = entity.tud_id;
            dr["tau_id"] = entity.tau_id;
            dr["tud_idFilho"] = entity.tud_idFilho;

            if (entity.tuf_data != new DateTime())
                dr["tuf_data"] = entity.tuf_data;
            else
                dr["tuf_data"] = DBNull.Value;

            if (entity.tuf_numeroAulas > 0)
                dr["tuf_numeroAulas"] = entity.tuf_numeroAulas;
            else
                dr["tuf_numeroAulas"] = DBNull.Value;

            if (!string.IsNullOrEmpty(entity.tuf_planoAula))
                dr["tuf_planoAula"] = entity.tuf_planoAula;
            else
                dr["tuf_planoAula"] = DBNull.Value;

            if (!string.IsNullOrEmpty(entity.tuf_diarioClasse))
                dr["tuf_diarioClasse"] = entity.tuf_diarioClasse;
            else
                dr["tuf_diarioClasse"] = DBNull.Value;

            dr["tuf_situacao"] = entity.tuf_situacao;

            if (!string.IsNullOrEmpty(entity.tuf_conteudo))
                dr["tuf_conteudo"] = entity.tuf_conteudo;
            else
                dr["tuf_conteudo"] = DBNull.Value;

            if (!string.IsNullOrEmpty(entity.tuf_atividadeCasa))
                dr["tuf_atividadeCasa"] = entity.tuf_atividadeCasa;
            else
                dr["tuf_atividadeCasa"] = DBNull.Value;

            if (entity.pro_id != Guid.Empty)
                dr["pro_id"] = entity.pro_id;
            else
                dr["pro_id"] = DBNull.Value;

            if (!string.IsNullOrEmpty(entity.tuf_sintese))
                dr["tuf_sintese"] = entity.tuf_sintese;
            else
                dr["tuf_sintese"] = DBNull.Value;

            if (tuf_dataAlteracao != new DateTime())
                dr["tuf_dataAlteracao"] = tuf_dataAlteracao;
            else
                dr["tuf_dataAlteracao"] = DBNull.Value;

            dr["tuf_checadoAtividadeCasa"] = entity.tuf_checadoAtividadeCasa;

            return dr;
        }

        /// <summary>
        /// Seleciona as turmas aula de regencia por disciplina e turma aula
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="tau_id"></param>
        /// <returns></returns>
        public static List<CLS_TurmaAulaRegencia> SelecionaPorDisciplinaTurmaAula(long tud_id, int tau_id)
        {
            CLS_TurmaAulaRegenciaDAO dao = new CLS_TurmaAulaRegenciaDAO();
            return dao.SelecionaPorDisciplinaTurmaAula(tud_id, tau_id).Rows.Cast<DataRow>().Select(p => dao.DataRowToEntity(p, new CLS_TurmaAulaRegencia())).ToList();
        }
    }
}