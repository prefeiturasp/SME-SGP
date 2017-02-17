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
using MSTech.GestaoEscolar.CustomResourceProviders;
using MSTech.GestaoEscolar.BLL.Caching;

namespace MSTech.GestaoEscolar.BLL
{
    #region Excessões

    public class EditarAtividade_ValidationException : ValidationException
    {
        private string _mensagem;

        public EditarAtividade_ValidationException()
        {
        }

        public EditarAtividade_ValidationException(string Mensagem)
        {
            _mensagem = Mensagem;
        }

        public override string Message
        {
            get
            {
                if (string.IsNullOrEmpty(_mensagem))
                    _mensagem = base.Message;

                return _mensagem;
            }
        }
    }

    #endregion Excessões

    /// <summary>
    /// Situações da atividade da disciplina da turma
    /// </summary>
    public enum CLS_TurmaNotaSituacao : byte
    {
        AtividadePrevista = 1
        ,

        Excluido = 3
        ,

        AtividadeDada = 4
        ,

        AtividadeCancelada = 6
    }

    public class CLS_TurmaNotaBO : BusinessBase<CLS_TurmaNotaDAO, CLS_TurmaNota>
    {
        /// <summary>
        ///	Retorna as aulas da disciplina da turma e do período do calendário
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="tpc_id">ID do Período do calendário</param>
        /// <param name="tau_id">ID da aula da disciplina da turma</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaPorTurmaDisciplinaPeriodoCalendario
        (
            long tud_id
            , int tpc_id
            , int tau_id
            , Guid ent_id
            , TalkDBTransaction banco = null
        )
        {
            CLS_TurmaNotaDAO dao = new CLS_TurmaNotaDAO();
            if (banco != null)
                dao._Banco = banco;
            return dao.SelectBy_tud_id_tpc_id(tud_id, tpc_id, tau_id, ent_id);
        }

        /// <summary>
        ///	Retorna as aulas da disciplina da turma
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaPorTurmaDisciplina(long tud_id, int tpc_id = -1)
        {
            CLS_TurmaNotaDAO dao = new CLS_TurmaNotaDAO();
            return dao.SelectBy_tud_id(tud_id, tpc_id);
        }

        /// <summary>
        ///	Retorna as aulas da turma e do período do calendário
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="tpc_id">ID do Período do calendário</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaPorTurmaPeriodo
        (
            long tud_id
            , int tpc_id
            , Guid ent_id
        )
        {
            CLS_TurmaNotaDAO dao = new CLS_TurmaNotaDAO();
            return dao.SelectByTurmaPeriodo(tud_id, tpc_id, ent_id);
        }

        /// <summary>
        /// Verifica se já existe uma atividade da disciplina da turma cadastrada com o mesmo nome
        /// </summary>
        /// <param name="entity">Entidade CLS_TurmaNota</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaAtividadeExistente
        (
            CLS_TurmaNota entity
        )
        {
            CLS_TurmaNotaDAO dao = new CLS_TurmaNotaDAO();
            return dao.SelectBy_Nome(entity.tud_id, entity.tnt_id, entity.tnt_nome);
        }

        /// <summary>
        /// Utilizado para atualizar a data da atividade de acordo com a data da aula
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="tau_id">ID da aula da disciplina da turma</param>
        /// <param name="tau_data">Data da aula da disciplina da turma</param>
        /// <param name="banco">Conexão aberta com o banco de dados</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool AtualizaDataAtividade
        (
            long tud_id
            , int tau_id
            , DateTime tau_data
            , TalkDBTransaction banco
        )
        {
            CLS_TurmaNotaDAO dao = new CLS_TurmaNotaDAO { _Banco = banco };
            return dao.UpdateBy_Aula(tud_id, tau_id, tau_data);
        }

        /// <summary>
        /// Retorna as atividades (TurmaNota) para a TurmaDisciplina e período informados, junto com
        /// os alunos matriculados naquela disciplina e suas notas nas avaliações, quando tiver.
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="tpc_id">ID do tipo de período do calendário</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_TurmaDisciplina_Periodo
        (
            long tud_id
            , int tpc_id
        )
        {
            CLS_TurmaNotaDAO dao = new CLS_TurmaNotaDAO();
            return GetSelectBy_TurmaDisciplina_Periodo(tud_id, tpc_id, Guid.Empty, 0);
        }

        /// <summary>
        /// Retorna as atividades (TurmaNota) para a TurmaDisciplina e período informados, junto com
        /// os alunos matriculados naquela disciplina e suas notas nas avaliações, quando tiver.
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="tpc_id">ID do tipo de período do calendário</param>
        /// <param name="usu_id">ID do docente logado</param>
        /// <param name="tdt_posicao">posição do docente a ser filtrado</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_TurmaDisciplina_Periodo
        (
            long tud_id
            , int tpc_id
            , Guid usu_id
            , byte tdt_posicao
            , string tur_ids = null
        )
        {
            CLS_TurmaNotaDAO dao = new CLS_TurmaNotaDAO();

            DataTable dtTurmas = TUR_Turma.TipoTabela_Turma();

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
            return dao.SelectBy_TurmaDisciplina_Periodo_NotaAluno(tud_id, tpc_id, usu_id, tdt_posicao, dtTurmas);
        }

        /// <summary>
        /// Retorna as atividades (TurmaNota) para a TurmaDisciplina e período informados, junto com
        /// os alunos matriculados naquela disciplina e suas notas nas avaliações, quando tiver.
        /// Filtrando os alunos com ou sem deficiência, dependendo do docente.
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="tpc_id">ID do tipo de período do calendário</param>
        /// <param name="tdc_id">ID do tipo de docente</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_TurmaDisciplina_PeriodoFiltroDeficiencia
        (
            long tud_id
            , int tpc_id
            , EnumTipoDocente tipoDocente
        )
        {
            return GetSelectBy_TurmaDisciplina_PeriodoFiltroDeficiencia(tud_id, tpc_id, tipoDocente, Guid.Empty, 0);
        }

        /// <summary>
        /// Retorna as atividades (TurmaNota) para a TurmaDisciplina e período informados, junto com
        /// os alunos matriculados naquela disciplina e suas notas nas avaliações, quando tiver.
        /// Filtrando os alunos com ou sem deficiência, dependendo do docente.
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="tpc_id">ID do tipo de período do calendário</param>
        /// <param name="tdc_id">ID do tipo de docente</param>
        /// <param name="usu_id">ID do docente logado</param>
        /// <param name="tdt_posicao">Posição do docente</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_TurmaDisciplina_PeriodoFiltroDeficiencia
        (
            long tud_id
            , int tpc_id
            , EnumTipoDocente tipoDocente
            , Guid usu_id
            , byte tdt_posicao
        )
        {
            return new CLS_TurmaNotaDAO().SelectBy_TurmaDisciplina_Periodo_NotaAlunoFiltroDeficiencia(tud_id, tpc_id, (byte)tipoDocente, usu_id, tdt_posicao);
        }

        /// <summary>
        /// Retorna as atividades(disciplina e secretaria) para a TurmaDisciplina no período informado, junto com
        /// os alunos matriculados naquela disciplina e suas notas nas avaliações, quando tiver.
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="tpc_id">ID do tipo de período do calendário</param>
        /// <param name="usu_id">ID do docente logado</param>
        /// <param name="tdt_posicao">Indica a posição do docente a ser filtrado</param>
        /// <param name="tud_idRelacionada">Disciplina relacionada</param>
        /// <param name="usuario_superior">Indica se é um usuário superior</param>
        /// <param name="trazerAvaSecretaria">Indica se deve trazer avaliação da secretaria</param>
        /// <param name="ausenteTurmaNota">Indica se está ausente</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_TurmaDisciplina_PeriodoTodos
        (
            long tud_id
            , int tpc_id
            , Guid usu_id
            , byte tdt_posicao
            , long tud_idRelacionada
            , bool usuario_superior
            , bool trazerAvaSecretaria = true
            , bool ausenteTurmaNota = false
            , string tur_ids = null
        )
        {
            CLS_TurmaNotaDAO dao = new CLS_TurmaNotaDAO();

            DataTable dtTurmas = TUR_Turma.TipoTabela_Turma();

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

            return dao.SelectBy_TurmaDisciplina_Periodo_NotaAlunoTodos(tud_id, tpc_id, usu_id, tdt_posicao, tud_idRelacionada, usuario_superior, trazerAvaSecretaria, ausenteTurmaNota, dtTurmas);
        }

        /// <summary>
        /// Inclui ou altera a atividade da disciplina da turma
        /// </summary>
        /// <param name="entity">Entidade CLS_TurmaNota</param>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool Save
        (
            CLS_TurmaNota entity,
            TUR_Turma entTurma,
            DateTime cal_dataInicio,
            DateTime cal_dataFim,
            DateTime cap_dataInicio,
            DateTime cap_dataFim,
            Guid ent_id,
            bool fav_permiteRecuperacaoForaPeriodo
        )
        {
            return Save(
                    entity,
                    entTurma,
                    cal_dataInicio,
                    cal_dataFim,
                    cap_dataInicio,
                    cap_dataFim,
                    ent_id,
                    new List<CLS_TurmaNotaOrientacaoCurricular>(),
                    fav_permiteRecuperacaoForaPeriodo
                );
        }

        /// <summary>
        /// Inclui ou altera a atividade da disciplina da turma
        /// </summary>
        /// <param name="entity">Entidade CLS_TurmaNota</param>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool Save
        (
            CLS_TurmaNota entity,
            TUR_Turma entTurma,
            DateTime cal_dataInicio,
            DateTime cal_dataFim,
            DateTime cap_dataInicio,
            DateTime cap_dataFim,
            Guid ent_id,
            List<CLS_TurmaNotaOrientacaoCurricular> lstHabilidades,
            bool fav_permiteRecuperacaoForaPeriodo,
            CLS_TurmaAula entityTurmaAula = null,
            CLS_TurmaNotaRegencia entityTurmaNotaRegencia = null,
            bool validarDataAula = true,
            Guid usu_id = new Guid(),
            byte origemLogNota = 0,
            byte tipoLogNota = 0
        )
        {

            TalkDBTransaction banco = new CLS_TurmaNotaDAO()._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);

            try
            {

            bool bRetorno = false;

            // Caso tenha selecionado uma aula para vincular à atividade.
            if (validarDataAula && entity.tnt_data == new DateTime())
            {
                CLS_TurmaAula entityAula = new CLS_TurmaAula
                {
                    tud_id = entity.tud_id,
                    tau_id = entity.tau_id
                };
                    CLS_TurmaAulaBO.GetEntity(entityAula, banco);

                // Verifica se o usuário logado tem uma posição compatível à da aula.
                if ((entity.tdt_posicao > 0) && (entity.tdt_posicao != entityAula.tdt_posicao))
                {
                    throw new ValidationException("Você não tem permissão para vincular a nova atividade à esta data da aula.");
                }

                // Caso o usuário logado tenha selecionado uma aula para vincular e
                // não seja um docente, copia a posição da aula.
                entity.tdt_posicao = entityAula.tdt_posicao;
                entity.usu_id = entityAula.usu_id;
            }

            // Caso o usuário logado não seja um docente, grava como posição 1.
            if (entity.tdt_posicao < 1)
            {
                entity.tdt_posicao = 1;
            }

            if (entity.Validate())
            {
                // Verifica se a atividade foi alterada/excluída por outra pessoa enquanto o usuário tentava alterar a mesma.
                CLS_TurmaNota entityAtividadeAuxiliar = new CLS_TurmaNota
                {
                    tud_id = entity.tud_id,
                    tnt_id = entity.tnt_id
                };
                    GetEntity(entityAtividadeAuxiliar, banco);

                if (!entityAtividadeAuxiliar.IsNew)
                {
                    entity.tdt_posicao = entityAtividadeAuxiliar.tdt_posicao;
                    entity.usu_id = entityAtividadeAuxiliar.usu_id;
                }

                if (entityAtividadeAuxiliar.tnt_dataAlteracao != entity.tnt_dataAlteracao)
                {
                    throw new EditarAtividade_ValidationException("Esta atividade já foi alterada recentemente.");
                }

                string nomeAtividade = GestaoEscolarUtilBO.nomePadraoAtividadeAvaliativa(ent_id);

                if (entity.tav_id <= 0 && string.IsNullOrEmpty(entity.tnt_nome) && ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.NOME_TODAS_ATIVIDADES_AVALIATIVAS, ent_id) == "False")
                {
                    throw new ArgumentException(string.Format("Outro tipo de {0} é obrigatório.", nomeAtividade.ToLower()));
                }

                CLS_TipoAtividadeAvaliativa entityAtividade = new CLS_TipoAtividadeAvaliativa();
                if (entity.tav_id <= 0)
                {
                    if (entity.tnt_nome.Length > 100)
                    {
                        throw new ArgumentException(nomeAtividade + " pode conter até 100 caracteres.");
                    }
                }
                else
                {
                    entityAtividade.tav_id = entity.tav_id;
                        CLS_TipoAtividadeAvaliativaBO.GetEntity(entityAtividade, banco);
                }

                if (entity.tnt_situacao != Convert.ToByte(CLS_TurmaNotaSituacao.AtividadeCancelada) || (entityAtividadeAuxiliar.tnt_situacao == entity.tnt_situacao))
                {
                    if ((entityAtividade.tav_situacao == Convert.ToByte(CLS_TipoAtividadeAvaliativaSituacao.Inativo)) && (entityAtividade.tav_id != entityAtividadeAuxiliar.tav_id))
                    {
                        throw new ArgumentException(string.Format("Tipo de {0} está inativo.", nomeAtividade.ToLower()));
                    }
                }

                CLS_TurmaNota avaRecuperacao = CLS_TurmaNotaBO.GetSelectRelacionadaFilho(entity.tud_id, entity.tnt_id, banco);
                if (entity.tnt_data != new DateTime())
                {
                    if (entTurma.tur_id <= 0)
                    {
                        entTurma = new TUR_Turma { tur_id = entity.tur_id };
                            TUR_TurmaBO.GetEntity(entTurma, banco);
                    }

                    // Compara as datas das avaliacoes relacionadas
                    bool relacionadaPai = false;
                    CLS_TurmaNota avaRelacionada = CLS_TurmaNotaBO.GetSelectRelacionadaFilho(entity.tud_id, entity.tnt_id, banco);
                    if (avaRelacionada.tnt_id > 0)
                    {
                        // se a data da recuperacao filho eh menor que a data da avaliacao pai...
                        if (avaRecuperacao.tnt_data != new DateTime() && avaRecuperacao.tnt_data < entity.tnt_data)
                        {
                            throw new ArgumentException(CustomResource.GetGlobalResourceObject("Academico", "LancamentoAvaliacao.UCCadastroAvaliacao.lblMessageAtividade.ValidacaoDataFilho").ToString());
                        }
                    }
                    else
                    {
                        avaRelacionada = CLS_TurmaNotaBO.GetSelectRelacionadaPai(entity.tud_id, entity.tnt_id);
                        relacionadaPai = avaRelacionada.tnt_id > 0;
                        avaRelacionada = CLS_TurmaNotaBO.GetSelectRelacionadaPai(entity.tud_id, entity.tnt_id, banco);
                        // se a data da avaliacao pai eh maior que a data da recuperacao filho...
                        if (avaRelacionada.tnt_id > 0 && avaRelacionada.tnt_data != new DateTime() && avaRelacionada.tnt_data > entity.tnt_data)
                        {
                            throw new ArgumentException(CustomResource.GetGlobalResourceObject("Academico", "LancamentoAvaliacao.UCCadastroAvaliacao.lblMessageAtividade.ValidacaoDataPai").ToString());
                        }
                    }

                    if (!(fav_permiteRecuperacaoForaPeriodo && relacionadaPai) &&
                       (entity.tnt_data > cal_dataFim || entity.tnt_data < cal_dataInicio))
                    {
                        throw new ArgumentException("A data da atividade deve estar dentro do período do calendário escolar (" + cal_dataInicio.ToString("dd/MM/yyyy") + " - " + cal_dataFim.ToString("dd/MM/yyyy") + ").");
                    }

                    if (!(fav_permiteRecuperacaoForaPeriodo && relacionadaPai) &&
                        (entity.tnt_data > cap_dataFim || entity.tnt_data < cap_dataInicio))
                    {
                        throw new ArgumentException("A data da atividade deve estar dentro do período do calendário (" + cap_dataInicio.ToString("dd/MM/yyyy") + " - " + cap_dataFim.ToString("dd/MM/yyyy") + ").");
                    }
                }

                // Valida se existe aluno com habilidade nao selecionada na avaliacao
                if (entity.tnt_id > 0 && !CLS_TurmaNotaAlunoOrientacaoCurricularBO.ValidarHabilidadesAvaliacao(lstHabilidades, banco))
                {
                    throw new ArgumentException(CustomResource.GetGlobalResourceObject("Academico", "LancamentoAvaliacao.UCCadastroAvaliacao.lblMessageAtividade.ValidaHabilidadesAluno").ToString());
                }

                // Valida se existe aluno na avaliacao de recuperacao, com habilidade 
                // que nao esta mais selecionada na avaliacao normal
                if (avaRecuperacao.tnt_id > 0 && !CLS_TurmaNotaAlunoOrientacaoCurricularBO.ValidarHabilidadesAvaliacao(lstHabilidades, banco, avaRecuperacao.tnt_id))
                {
                    throw new ArgumentException(CustomResource.GetGlobalResourceObject("Academico", "LancamentoAvaliacao.UCCadastroAvaliacao.lblMessageAtividade.ValidaHabilidadesAlunoRecuperacao").ToString());
                }

                CLS_ConfiguracaoAtividadeQualificador configQualificador = new CLS_ConfiguracaoAtividadeQualificador();
                    CLS_TurmaNotaDAO dao = new CLS_TurmaNotaDAO { _Banco = banco};
                if (entity.IsNew && entity.tav_id > 0 && entityAtividade.qat_id > 0)
                {
                    CLS_ConfiguracaoAtividadeQualificadorDAO configQualificadorDao = new CLS_ConfiguracaoAtividadeQualificadorDAO();
                        configQualificadorDao._Banco = banco;
                    configQualificador = configQualificadorDao.GetSelectByTudQualificador(entity.tud_id, entityAtividade.qat_id);

                    // Valida a quantidade configurada para o qualificador
                    if (configQualificador.caa_id > 0 && configQualificador.caq_quantidade >= 0)
                    {
                        if (dao.ValidaQuantidadeMaxima(entity.tud_id, entityAtividade.qat_id, entity.tpc_id, configQualificador.caq_quantidade))
                        {
                            throw new ArgumentException(CustomResource.GetGlobalResourceObject("Academico", "LancamentoAvaliacao.UCCadastroAvaliacao.lblMessageAtividade.ValidacaoQuantidade").ToString());
                        }
                    }
                }

                bRetorno = dao.Salvar(entity);

                if (entity.IsNew 
                    && configQualificador.caa_id > 0 
                    && configQualificador.caq_possuiRecuperacao)
                {
                        int tavIdRelacionado = CLS_TipoAtividadeAvaliativaBO.SelecionaTipoAtividadeAvaliativaRelacionado(configQualificador.caa_id, configQualificador.qat_id, banco);
                    if (tavIdRelacionado > 0)
                    {
                        CLS_TurmaNota entityRelacionada = new CLS_TurmaNota();
                        entityRelacionada.tud_id = entity.tud_id;
                        entityRelacionada.IsNew = true;
                        entityRelacionada.tpc_id = entity.tpc_id;
                        entityRelacionada.tnt_situacao = 1;
                        entityRelacionada.tav_id = tavIdRelacionado;
                        entityRelacionada.tdt_posicao = entity.tdt_posicao;
                        entityRelacionada.tnt_exclusiva = false;
                        entityRelacionada.usu_id = entity.usu_id;
                        entityRelacionada.usu_idDocenteAlteracao = entity.usu_idDocenteAlteracao;
                        dao.Salvar(entityRelacionada);

                        CLS_TurmaNotaRelacionada entityRelacionamento = new CLS_TurmaNotaRelacionada();
                        entityRelacionamento.IsNew = true;
                        entityRelacionamento.tud_id = entity.tud_id;
                        entityRelacionamento.tnt_id = entity.tnt_id;
                        entityRelacionamento.tud_idRelacionada = entityRelacionada.tud_id;
                        entityRelacionamento.tnt_idRelacionada = entityRelacionada.tnt_id;
                        CLS_TurmaNotaRelacionadaBO.Save(entityRelacionamento, banco);
                        
                        avaRecuperacao = entityRelacionada;
                    }
                }

                #region Salva as Orientacoes curriculares ligadas a avaliacao

                if (bRetorno && lstHabilidades.Any())
                {
                    lstHabilidades.ForEach(x => x.tnt_id = entity.tnt_id);
                    bRetorno = CLS_TurmaNotaOrientacaoCurricularBO.SalvarEmLote(lstHabilidades, banco);

                    // Copia as habilidades na avaliacao de recuperacao
                    if (avaRecuperacao.tnt_id > 0)
                    {
                        // Salva as Orientacoes curriculares ligadas a avaliacao - específico para recuperação.
                        lstHabilidades.ForEach(x => x.tnt_id = avaRecuperacao.tnt_id);
                        CLS_TurmaNotaOrientacaoCurricularBO.SalvarEmLote(lstHabilidades, banco);
                    }
                }

                #endregion Salva as Orientacoes curriculares ligadas a avaliacao

                #region Salva a turma Aula

                if (entityTurmaAula != null)
                        bRetorno = CLS_TurmaAulaBO.Save(entityTurmaAula, banco);

                #endregion Salva a turma Aula

                // Salva o vinculo com a aula, caso seja regência
                if (entityTurmaNotaRegencia != null)
                {
                    entityTurmaNotaRegencia.tnt_id = entity.tnt_id;
                        bRetorno = CLS_TurmaNotaRegenciaBO.Save(entityTurmaNotaRegencia, banco);
                }

                if (origemLogNota > 0 && tipoLogNota > 0)
                {
                    DateTime dataLogNota = DateTime.Now;
                    LOG_TurmaNota_Alteracao entLogNota = new LOG_TurmaNota_Alteracao
                    {
                        tud_id = entity.tud_id,
                        tnt_id = entity.tnt_id,
                        usu_id = usu_id,
                        ltn_origem = origemLogNota,
                        ltn_tipo = tipoLogNota,
                        ltn_data = dataLogNota
                    };

                        LOG_TurmaNota_AlteracaoBO.Save(entLogNota, banco);
                }

                return bRetorno;
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));

            }
            catch (Exception err)
            {
                banco.Close(err);
                throw;
            }
            finally
            {
                banco.Close();
            }

        }

        /// <summary>
        /// Método save sobrescrito para validar o usuário e a posição do docente.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="banco"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Save
        (
            CLS_TurmaNota entity,
            TalkDBTransaction banco
        )
        {
            if (entity.Validate())
            {
                CLS_TurmaNota entityAtividadeAuxiliar = new CLS_TurmaNota
                {
                    tud_id = entity.tud_id,
                    tnt_id = entity.tnt_id
                };
                GetEntity(entityAtividadeAuxiliar, banco);

                if (!entityAtividadeAuxiliar.IsNew)
                {
                    entity.tdt_posicao = entityAtividadeAuxiliar.tdt_posicao;
                    entity.usu_id = entityAtividadeAuxiliar.usu_id;
                    //entity.usu_idDocenteAlteracao = entityAtividadeAuxiliar.usu_idDocenteAlteracao;
                }

                return new CLS_TurmaNotaDAO { _Banco = banco }.Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }

        /// <summary>
        /// Deleta logicamente a atividade
        /// </summary>
        /// <param name="entity">Entidade CLS_TurmaNota</param>
        /// <returns>True = deletado/alterado | False = não deletado/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Delete
        (
            CLS_TurmaNota entity,
            Guid usu_id = new Guid(),
            byte origemLogNota = 0,
            byte tipoLogNota = 0
        )
        {
            CLS_TurmaNotaDAO dao = new CLS_TurmaNotaDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                //Verifica se for a última atividade e algum aluno da turma possuir nota/parecer final então não permite excluir.
                if (!SelecionaPorTurmaDisciplinaPeriodoCalendario(entity.tud_id, entity.tpc_id, 0, Guid.Empty, dao._Banco).AsEnumerable()
                    .Any(p => Convert.ToInt32(p["tnt_id"]) != entity.tnt_id) &&
                    CLS_AlunoAvaliacaoTurmaDisciplinaMediaBO.BuscaNotasFinaisTud(entity.tud_id, entity.tpc_id, dao._Banco).Any())
                    throw new ValidationException(CustomResource.GetGlobalResourceObject("BLL", "CLS_TurmaNota.ValidacaoExclusaoUltimaAvaliacao").ToString());

                //Deleta logicamente a atividade
                dao.Delete(entity);

                if (origemLogNota > 0 && tipoLogNota > 0)
                {
                    DateTime dataLogNota = DateTime.Now;
                    LOG_TurmaNota_Alteracao entLogNota = new LOG_TurmaNota_Alteracao
                    {
                        tud_id = entity.tud_id,
                        tnt_id = entity.tnt_id,
                        usu_id = usu_id,
                        ltn_origem = origemLogNota,
                        ltn_tipo = tipoLogNota,
                        ltn_data = dataLogNota
                    };

                    LOG_TurmaNota_AlteracaoBO.Save(entLogNota, dao._Banco);
                }

                return true;
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
        /// Busca as atividades de uma aula atravéz da TurmaDisciplina e Data
        /// </summary>
        /// <param name="tud_id">ID da TurmaDisciplina</param>
        /// <param name="tnt_data">Data da aula</param>
        /// <returns></returns>
        public static DataTable BuscaAtividadesDaAula
        (
            long tud_id
            , int tau_id
        )
        {
            CLS_TurmaNotaDAO dao = new CLS_TurmaNotaDAO();
            return dao.BuscaAtividadesDaAula(tud_id, tau_id);
        }

        /// <summary>
        /// Busca as atividades de uma aula atravéz da TurmaDisciplina e Data
        /// </summary>
        /// <param name="tud_id">ID da TurmaDisciplina</param>
        /// <param name="tnt_data">Data da aula</param>
        /// <returns>Lista de atividades</returns>
        public static List<CLS_TurmaNota> BuscaListaAtividadesDaAula
        (
            long tud_id
            , int tau_id
        )
        {
            CLS_TurmaNotaDAO dao = new CLS_TurmaNotaDAO();
            return dao.BuscaAtividadesDaAula(tud_id, tau_id).Rows.Cast<DataRow>().Select(p => dao.DataRowToEntity(p, new CLS_TurmaNota())).ToList();
        }

        /// <summary>
        /// Busca as atividades pelo dia da aula quando esta for por regencia
        /// </summary>
        /// <param name="tnt_data">Dia da aula</param>
        /// <param name="tud_idRegencia">ID da disciplina da regência</param>
        /// <returns>DataTable</returns>
        public static DataTable BuscaAtividadesDoDia(DateTime tnt_data, long tud_idRegencia)
        {
            byte tud_tipoComponente = (byte)TurmaDisciplinaTipo.ComponenteRegencia;
            return new CLS_TurmaNotaDAO().BuscaAtividadesDoDia(tnt_data, tud_idRegencia, tud_tipoComponente);
        }

        /// <summary>
        /// Busca as atividades pelo dia da aula quando esta for por regencia
        /// </summary>
        /// <param name="tnt_data">Dia da aula</param>
        /// <param name="tud_idRegencia">ID da disciplina da regência</param>
        /// <returns>Lista de atividades</returns>
        public static List<CLS_TurmaNota> BuscaListaAtividadesDoDia(DateTime tnt_data, long tud_idRegencia)
        {
            CLS_TurmaNotaDAO dao = new CLS_TurmaNotaDAO();
            byte tud_tipoComponente = (byte)TurmaDisciplinaTipo.ComponenteRegencia;
            return dao.BuscaAtividadesDoDia(tnt_data, tud_idRegencia, tud_tipoComponente).Rows.Cast<DataRow>().Select(p => dao.DataRowToEntity(p, new CLS_TurmaNota())).ToList();
        }

        /// <summary>
        /// Salva uma lista de atividades, transformando-as em datatable.
        /// </summary>
        /// <param name="ltTurmaNota"></param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static bool SalvarAtividades(List<CLS_TurmaNota> ltTurmaNota, TalkDBTransaction banco = null)
        {
            DataTable dtTurmaNota = CLS_TurmaNota.TipoTabela_TurmaNota();

            ltTurmaNota.ForEach(p =>
                                    {
                                        if (p.Validate())
                                            dtTurmaNota.Rows.Add(TurmaNotaToDataRow(p, dtTurmaNota.NewRow()));
                                        else
                                            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(p));
                                    });

            return banco == null ?
                new CLS_TurmaNotaDAO().SalvarAtividades(dtTurmaNota) :
                new CLS_TurmaNotaDAO { _Banco = banco }.SalvarAtividades(dtTurmaNota);
        }

        /// <summary>
        /// Calcula as notas automaticas dos alunos.
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina</param>     
        /// <param name="tpc_id">ID do tipo periodo calendario</param> 
        /// <param name="esa_idDocente">ID da escala de avaliacao</param>
        /// <param name="aluno">Alunos para o calculo das notas</param> 
        /// <returns>Lista de avaliacoes automaticas</returns>      
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable CalculaNotaAlunos
        (
            long tud_id
            , int tpc_id
            , int esa_idDocente
            , DataTable alunos
        )
        {
            return new CLS_TurmaNotaDAO().CalculaNotaAlunos(tud_id, tpc_id, esa_idDocente, alunos);
        }

        /// <summary>
        /// Seleciona uma avaliacao unica para o tipo de atividade, disciplina e periodo.
        /// </summary>
        /// <param name="tudId">Id da turma disciplina</param>
        /// <param name="tpcId">Id do periodo do calendario</param>
        /// <param name="tavId">Id do tipo de atividade</param>
        /// <returns></returns>
        public static CLS_TurmaNota GetSelectByTipoAtividade(long tudId, int tpcId, int tavId)
        {
            return new CLS_TurmaNotaDAO().GetSelectByTipoAtividade(tudId, tpcId, tavId);
        }

        /// <summary>
        /// Seleciona a avaliacao filho relacionada.
        /// </summary>
        /// <param name="tudId"></param>
        /// <param name="tntId"></param>
        /// <returns></returns>
        public static CLS_TurmaNota GetSelectRelacionadaFilho(long tudId, int tntId, TalkDBTransaction banco = null)
        {
            return banco == null ?
                        new CLS_TurmaNotaDAO().GetSelectRelacionadaFilho(tudId, tntId) :
                        new CLS_TurmaNotaDAO { _Banco = banco }.GetSelectRelacionadaFilho(tudId, tntId);
        }

        /// <summary>
        /// Seleciona a avaliacao pai relacionada.
        /// </summary>
        /// <param name="tudId"></param>
        /// <param name="tntId"></param>
        /// <returns></returns>
        public static CLS_TurmaNota GetSelectRelacionadaPai(long tudId, int tntId, TalkDBTransaction banco = null)
        {
            return banco == null ?
                        new CLS_TurmaNotaDAO().GetSelectRelacionadaPai(tudId, tntId) :
                        new CLS_TurmaNotaDAO { _Banco = banco }.GetSelectRelacionadaPai(tudId, tntId);
        }

        /// <summary>
        /// Retorna um datarow com os dados de uma atividade.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static DataRow TurmaNotaToDataRow(CLS_TurmaNota entity, DataRow dr, DateTime tnt_dataAlteracao = new DateTime())
        {
            if (entity.idAula > 0)
                dr["idAula"] = entity.idAula;
            else
                dr["idAula"] = DBNull.Value;

            dr["tud_id"] = entity.tud_id;
            dr["tnt_id"] = entity.tnt_id;

            if (entity.tpc_id > 0)
                dr["tpc_id"] = entity.tpc_id;
            else
                dr["tpc_id"] = DBNull.Value;

            if (entity.tau_id > 0)
                dr["tau_id"] = entity.tau_id;
            else
                dr["tau_id"] = DBNull.Value;

            if (!string.IsNullOrEmpty(entity.tnt_nome))
                dr["tnt_nome"] = entity.tnt_nome;
            else
                dr["tnt_nome"] = DBNull.Value;

            if (entity.tnt_data != new DateTime())
                dr["tnt_data"] = entity.tnt_data;
            else
                dr["tnt_data"] = DBNull.Value;

            if (!string.IsNullOrEmpty(entity.tnt_descricao))
                dr["tnt_descricao"] = entity.tnt_descricao;
            else
                dr["tnt_descricao"] = DBNull.Value;

            dr["tnt_situacao"] = entity.tnt_situacao;
            dr["tnt_efetivado"] = entity.tnt_efetivado;

            if (entity.tav_id > 0)
                dr["tav_id"] = entity.tav_id;
            else
                dr["tav_id"] = DBNull.Value;

            dr["tdt_posicao"] = entity.tdt_posicao;
            dr["tnt_exclusiva"] = entity.tnt_exclusiva;

            if (entity.usu_id != Guid.Empty)
                dr["usu_id"] = entity.usu_id;
            else
                dr["usu_id"] = DBNull.Value;

            if (entity.pro_id != Guid.Empty)
                dr["pro_id"] = entity.pro_id;
            else
                dr["pro_id"] = DBNull.Value;

            if (entity.tnt_chaveDiario > 0)
                dr["tnt_chaveDiario"] = entity.tnt_chaveDiario;
            else
                dr["tnt_chaveDiario"] = DBNull.Value;

            if (tnt_dataAlteracao != new DateTime())
                dr["tnt_dataAlteracao"] = tnt_dataAlteracao;
            else
                dr["tnt_dataAlteracao"] = DBNull.Value;

            if (entity.usu_idDocenteAlteracao != Guid.Empty)
                dr["usu_idDocenteAlteracao"] = entity.usu_idDocenteAlteracao;
            else
                dr["usu_idDocenteAlteracao"] = DBNull.Value;

            return dr;
        }

        /// <summary>
        /// Seleciona a lista com as atividades de acordo com os tnt_ids informados.
        /// </summary>
        /// <param name="tud_id">ID da disciplina da atividade.</param>
        /// <param name="tnt_ids">IDs das atividades.</param>
        /// <returns>Lista com as atividades.</returns>
        public static List<CLS_TurmaNota> SelecionarListaAtividadesPorIds(long tud_id, string tnt_ids)
        {
            return new CLS_TurmaNotaDAO().SelecionarListaAtividadesPorIds(tud_id, tnt_ids);
        }

    }
}