using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace GestaoEscolar.WebControls.LancamentoFrequencia
{
    public partial class UCLancamentoFrequencia : MotherUserControl
    {
        #region Structs
        /// <summary>
        /// Classe utilizada para dar DataBind no repeater de aulas.
        /// </summary>
        protected class AulasAlunos
        {
            public int tau_id { get; set; }

            public DateTime tau_data { get; set; }

            public bool tau_efetivado { get; set; }

            public byte tdt_posicao { get; set; }

            public int tau_numeroAulas { get; set; }

            public int taa_frequencia { get; set; }

            public string taa_frequenciaBitMap { get; set; }

            public string falta_justificada { get; set; }

            public bool AlunoDispensado { get; set; }

            public bool permissaoAlteracao { get; set; } //

            public int mtd_situacao { get; set; }

            public bool AlunoComCompensacao { get; set; }

            public Guid usu_id { get; set; }

            public bool tau_reposicao { get; set; }

            public bool falta_abonada { get; set; }
        }
        #endregion Structs

        #region Propriedades
        public bool VisivelAlunoDispensado
        {
            set
            {
                trExibirAlunoDispensadoListaoFreq.Visible = value;
            }
        }

        public string ClientIdComboOrdenacao
        {
            get
            {
                return _UCComboOrdenacao1.ComboClientID;
            }
        }

        public string ClientIdHdnOrdenacao
        {
            get
            {
                return hdnOrdenacaoFrequencia.ClientID;
            }
        }

        public string ValorHdnOrdenacao
        {
            set
            {
                hdnOrdenacaoFrequencia.Value = value;
            }
        }

        public string TextoMsgParecer
        {
            get
            {
                return lblMsgParecer.Text;
            }
            set
            {
                lblMsgParecer.Text = value;
            }
        }

        private CFG_PermissaoModuloOperacao PermissaoModuloLancamentoFrequencia
        {
            get
            {
                if (permissaoModuloLancamentoFrequencia == null)
                {
                    permissaoModuloLancamentoFrequencia = new CFG_PermissaoModuloOperacao()
                    {
                        gru_id = __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                        sis_id = ApplicationWEB.SistemaID,
                        mod_id = __SessionWEB.__UsuarioWEB.GrupoPermissao.mod_id,
                        pmo_operacao = Convert.ToInt32(CFG_PermissaoModuloOperacaoBO.Operacao.DiarioClasseLancamentoFrequencia)
                    };
                    CFG_PermissaoModuloOperacaoBO.GetEntity(permissaoModuloLancamentoFrequencia);
                }
                return permissaoModuloLancamentoFrequencia;
            }
        }

        private CFG_PermissaoModuloOperacao PermissaoModuloLancamentoFrequenciaInfantil
        {
            get
            {
                if (permissaoModuloLancamentoFrequenciaInfantil == null)
                {
                    if (tne_id == ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_NIVEL_ENSINO_EDUCACAO_INFANTIL, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                    {
                        permissaoModuloLancamentoFrequenciaInfantil = new CFG_PermissaoModuloOperacao()
                        {
                            gru_id = __SessionWEB.__UsuarioWEB.Grupo.gru_id,
                            sis_id = ApplicationWEB.SistemaID,
                            mod_id = __SessionWEB.__UsuarioWEB.GrupoPermissao.mod_id,
                            pmo_operacao = Convert.ToInt32(CFG_PermissaoModuloOperacaoBO.Operacao.DiarioClasseLancamentoFrequenciaInfantil)
                        };
                        CFG_PermissaoModuloOperacaoBO.GetEntity(permissaoModuloLancamentoFrequenciaInfantil);
                    }
                    else
                    {
                        permissaoModuloLancamentoFrequenciaInfantil = new CFG_PermissaoModuloOperacao();
                    }
                }
                return permissaoModuloLancamentoFrequenciaInfantil;
            }
        }

        /// <summary>
        /// Tabela carregada com as aulas e alunos para a disciplina e período.
        /// </summary>
        private List<sTurmaAulaAluno> VS_Aulas_Alunos;

        /// <summary>
        /// Usada no dataBind do repeater de aulas, para saber a data de matrícula do aluno.
        /// </summary>
        private DateTime mtd_dataMatriculaAluno;

        /// <summary>
        /// Usada no dataBind do repeater de aulas, para saber a data de saída do aluno.
        /// Se for nulo, guarda DateTime.MaxValue.
        /// </summary>
        private DateTime mtd_dataSaidaAluno;

        private byte tudTipo;
        private bool permiteVisualizarCompensacao;
        private List<sPermissaoDocente> ltPermissaoFrequencia;
        private bool permiteLancarFrequencia;
        private bool permiteEdicao;
        private bool usuarioPermissao;
        private int situacaoTurmaDisciplina;
        private byte posicaoDocente;
        private bool periodoEfetivado;
        private bool periodoAberto;
        private byte crpControleTempo;
        private bool possuiRegencia;
        private byte tipoApuracaoFrequencia;
        private CFG_PermissaoModuloOperacao permissaoModuloLancamentoFrequencia;
        private CFG_PermissaoModuloOperacao permissaoModuloLancamentoFrequenciaInfantil;
        private int tne_id;

        private List<Struct_PreenchimentoAluno> lstAlunosRelatorioRP = new List<Struct_PreenchimentoAluno>();

        #endregion Propriedades

        #region Delegates

        public delegate void commandRecarregar(bool atualizaData, bool proximo, bool anterior, bool inalterado);

        public event commandRecarregar Recarregar;

        public delegate void commandCarregarAusencias(long alu_id, int mtu_id, int mtd_id);

        public event commandCarregarAusencias CarregarAusencias;

        public delegate void commandAbrirRelatorioRP(long alu_id, string tds_idRP);

        public event commandAbrirRelatorioRP AbrirRelatorioRP;

        public delegate void commandAbrirRelatorioAEE(long alu_id);

        public event commandAbrirRelatorioAEE AbrirRelatorioAEE;

        #endregion Delegates

        #region Métodos
        /// <summary>
        /// Carrega as cores da legenda.
        /// </summary>
        public void CarregarLegenda()
        {
            HtmlTableCell cell = tbLegendaFrequencia.Rows[0].Cells[0];
            if (cell != null)
                cell.BgColor = ApplicationWEB.AlunoDispensado;
            cell = tbLegendaFrequencia.Rows[1].Cells[0];
            if (cell != null)
                cell.BgColor = ApplicationWEB.AlunoInativo;
        }

        /// <summary>
        /// Carrega dados de lançamento de frequência na tela.
        /// Só carrega caso a disciplina não seja do tipo
        /// complementação da regência.
        /// </summary>
        public void Carregar(bool proximo
                            , bool anterior
                            , bool inalterado
                            , ControleTurmas entitiesControleTurma            
                            , int tpcId
                            , DateTime capDataInicio
                            , DateTime capDataFim
                            , byte tdtPosicao
                            , EnumTipoDocente tipoDocente
                            , long tudIdRelacionada
                            , bool permiteVisualizarCompensacao
                            , List<sPermissaoDocente> ltPermissaoFrequencia
                            , bool permiteLancarFrequencia
                            , out int countAulas
                            , int situacaoTurmaDisciplina
                            , ref bool permiteEdicao
                            , bool usuarioPermissao
                            , bool periodoEfetivado
                            , bool periodoAberto
                            , ref bool esconderSalvar
                            , ref int paginaFreq
                            , int tne_id
                            , string tur_ids = null)
        {
            countAulas = 0;
            long tudId = entitiesControleTurma.turmaDisciplina.tud_id;
            long turId = entitiesControleTurma.turma.tur_id;
            int qtdAulasSemana = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.QUANTIDADE_AULAS_LISTAO_FREQUENCIA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            DateTime dtInicio = new DateTime();
            DateTime dtFim = new DateTime();

            if (proximo)
                paginaFreq++;
            else if (anterior && paginaFreq > 1)
                paginaFreq--;
            else if (!inalterado)
                paginaFreq = 1;

            // Carregar tabela com aulas e frequências das aulas para os alunos.
            VS_Aulas_Alunos =
                CLS_TurmaAulaAlunoBO.GetSelectBy_TurmaDisciplina(tudId, tpcId,
                (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0 ? __SessionWEB.__UsuarioWEB.Usuario.usu_id : Guid.Empty),
                capDataInicio, capDataFim, tdtPosicao, __SessionWEB.__UsuarioWEB.Docente.doc_id == 0, tudIdRelacionada, tur_ids);

            int qtdAlunos = VS_Aulas_Alunos.GroupBy(p => new { p.alu_id, p.mtu_id }).Count();

            int skip = qtdAulasSemana * (paginaFreq - 1) * qtdAlunos;

            while (proximo && VS_Aulas_Alunos.Count < skip)
            {
                paginaFreq--;
                skip = (qtdAulasSemana * (paginaFreq - 1)) * qtdAlunos;

                if (paginaFreq == 1)
                    break;
            }

            //Quando carrega pela primeira vez e o bimestre é ativo então abre a página que possui a data atual
            if (!proximo && !anterior && !inalterado && paginaFreq == 1 && 
                capDataInicio <= DateTime.Today && capDataFim >= DateTime.Today)
            {
                while (VS_Aulas_Alunos.Skip(skip).Take(qtdAulasSemana * qtdAlunos).ToList().LastOrDefault().tau_data < DateTime.Today &&
                       VS_Aulas_Alunos.Skip(skip).Count() > (qtdAulasSemana * qtdAlunos))
                {
                    paginaFreq++;
                    skip = (qtdAulasSemana * (paginaFreq - 1)) * qtdAlunos;
                }
            }

            lkbProximo.Visible = VS_Aulas_Alunos.Skip(skip).Count() > (qtdAulasSemana * qtdAlunos);
            lkbAnterior.Visible = skip > 0;

            VS_Aulas_Alunos = VS_Aulas_Alunos.Skip(skip).Take(qtdAulasSemana * qtdAlunos).ToList();

            dtInicio = VS_Aulas_Alunos.Count > 0 ? VS_Aulas_Alunos.FirstOrDefault().tau_data : capDataInicio.Date;
            dtFim = VS_Aulas_Alunos.Count > 0 ? VS_Aulas_Alunos.LastOrDefault().tau_data : capDataFim.Date;
            lblInicio.Text = dtInicio == new DateTime() ? "" : dtInicio.ToShortDateString();
            lblFim.Text = dtFim == new DateTime() ? "" : dtFim.ToShortDateString();

            // Carregar repeater de alunos.
            rptAlunosFrequencia.DataSource = MTR_MatriculaTurmaDisciplinaBO.SelecionaAlunosAtivosCOCPorTurmaDisciplina(tudId,
            tpcId, tipoDocente, false, capDataInicio, capDataFim, ApplicationWEB.AppMinutosCacheMedio, tur_ids)
            .Where(p => ((p.mtd_dataSaida > dtInicio) || (p.mtd_dataSaida == null)) && (p.mtd_dataMatricula <= dtFim));

            if (entitiesControleTurma.turma.tur_tipo == (byte)TUR_TurmaTipo.Normal)
            {
                lstAlunosRelatorioRP = CLS_RelatorioPreenchimentoAlunoTurmaDisciplinaBO.SelecionaAlunoPreenchimentoPorPeriodoDisciplina(tpcId, turId, tudId, ApplicationWEB.AppMinutosCacheMedio);
            }

            this.tudTipo = entitiesControleTurma.turmaDisciplina.tud_tipo;
            this.permiteVisualizarCompensacao = permiteVisualizarCompensacao;
            this.ltPermissaoFrequencia = ltPermissaoFrequencia;
            this.permiteLancarFrequencia = permiteLancarFrequencia;
            this.permiteEdicao = false;
            this.situacaoTurmaDisciplina = situacaoTurmaDisciplina;
            this.posicaoDocente = tdtPosicao;
            this.usuarioPermissao = usuarioPermissao;
            this.periodoEfetivado = periodoEfetivado;
            this.periodoAberto = periodoAberto;
            ACA_CurriculoPeriodo entityCrp = ACA_CurriculoPeriodoBO.SelecionaPorTurmaTipoNormal(turId, ApplicationWEB.AppMinutosCacheLongo);
            this.crpControleTempo = entityCrp.crp_controleTempo;
            this.possuiRegencia = TUR_TurmaBO.VerificaPossuiDisciplinaPorTipo(turId, TurmaDisciplinaTipo.Regencia, ApplicationWEB.AppMinutosCacheLongo);
            this.tipoApuracaoFrequencia = entitiesControleTurma.formatoAvaliacao.fav_tipoApuracaoFrequencia;
            this.tne_id = tne_id;
            rptAlunosFrequencia.DataBind();
            // Limpa o hiddenfield do listão de frequência pra zerar a ordenação.
            hdnOrdenacaoFrequencia.Value = "";

            //Fazendo as validações após carregar os dados.
            if (rptAlunosFrequencia.Items.Count == 0)
            {
                EscondeGridAlunosFrequencia("Não foram encontrados alunos na turma selecionada.");
                esconderSalvar = true;
            }
            else
            {
                MostraPeriodo(true);
                pnlLancamentoFrequencias.Visible = true;

                RepeaterItem header = (RepeaterItem)rptAlunosFrequencia.Controls[0];
                Repeater rptAulas = (Repeater)header.FindControl("rptAulas");

                lblMsgParecer.Visible = rptAulas.Items.Count > 0;

                _lblMsgRepeater.Visible = rptAulas.Items.Count == 0;

                if (rptAulas.Items.Count == 0)
                {
                    _lblMsgRepeater.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "ControleTurma.Listao.MensagemSemAulas").ToString(),
                                                                 UtilBO.TipoMensagem.Alerta);
                    esconderSalvar = true;
                }

                countAulas = rptAulas.Items.Count;
                rptAlunosFrequencia.Visible = true;
            }

            if (this.permiteEdicao && !periodoEfetivado)
                permiteEdicao = true;
        }

        /// <summary>
        /// Esconde o grid de alunos, e mostra a mensagem do parâmetro no lugar dele.
        /// </summary>
        /// <param name="msg">Mensagem a ser mostrada ao usuário</param>
        public void EscondeGridAlunosFrequencia(string msg)
        {
            bool mostra = !string.IsNullOrEmpty(msg);

            pnlLancamentoFrequencias.Visible = mostra;
            _UCComboOrdenacao1.Visible = false;
            rptAlunosFrequencia.Visible = false;
            lblMsgParecer.Visible = false;
            _lblMsgRepeater.Visible = mostra;
            _lblMsgRepeater.Text = UtilBO.GetErroMessage(msg, UtilBO.TipoMensagem.Alerta);

            MostraPeriodo(mostra);
        }

        /// <summary>
        /// Mostra/esconde os controles de mudança de semana.
        /// </summary>
        /// <param name="mostra">Indica se vai exibir os controles</param>
        private void MostraPeriodo(bool mostra)
        {
            lblInicio.Visible = mostra;
            lblFim.Visible = mostra;
            td_lkbAnterior.Visible = mostra;
            td_lkbProximo.Visible = mostra;
        }

        /// <summary>
        /// Valida se as frequencias lançadas estão no intervalo correto (Percorre o repeater)
        /// </summary>
        /// <returns>True caso as frequencias estejam corretas</returns>
        private bool ValidaFrequencias()
        {
            for (int i = 0; i < rptAlunosFrequencia.Items.Count; i++)
            {
                Repeater rptAulas = (Repeater)rptAlunosFrequencia.FindControl("rptAulas");

                if (rptAulas != null)
                {
                    for (int j = 0; j < rptAulas.Items.Count; j++)
                    {
                        RangeValidator rvFreq = (RangeValidator)rptAulas.Items[j].FindControl("rvFreq");
                        if (rvFreq != null)
                        {
                            rvFreq.Validate();
                            if (!rvFreq.IsValid)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Salva no banco as frequências.
        /// </summary>
        public bool Salvar(out string msg
                            , bool periodoEfetivado
                            , bool periodoAberto
                            , ref List<int> lstTauSalvas
                            , ControleTurmas entitiesControleTurma
                            , DateTime dataUltimaAlteracao
                            , ref bool recarregarDataAula
                            , int tpc_id
                            , byte posicaoDocente
                            , bool permiteLancarFrequencia
                            , int situacaoTurmaDisciplina)
        {
            msg = "";
            this.periodoEfetivado = periodoEfetivado;
            if (ValidaFrequencias())
            {
                if (!periodoAberto)
                    throw new ValidationException(
                        String.Format(GetGlobalResourceObject("WebControls", "UCLancamentoFrequencia.FrequenciaDisponivelApenasConsulta").ToString(),
                            GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id)));

                List<CLS_TurmaAulaAluno> listTurmaAulaAluno = new List<CLS_TurmaAulaAluno>();
                List<CLS_TurmaAula> listTurmaAula = new List<CLS_TurmaAula>();

                RepeaterItem header = (RepeaterItem)rptAlunosFrequencia.Controls[0];
                Repeater rptAulasEfetivado = (Repeater)header.FindControl("rptAulasEfetivado");

                lstTauSalvas = new List<int>();

                List<CLS_TurmaAula> listaTurmaAulaBD = new List<CLS_TurmaAula>();
                if (rptAulasEfetivado.Items.Count > 0)
                {
                    List<string> lstTauIdsSalvar = (from RepeaterItem item in rptAulasEfetivado.Items
                                                    select ((Label)item.FindControl("lbltau_id")).Text).ToList();
                    listaTurmaAulaBD = CLS_TurmaAulaBO.SelecionarListaAulasPorIds(entitiesControleTurma.turmaDisciplina.tud_id, string.Join(",", lstTauIdsSalvar));
                }

                // Adiciona itens na lista de TurmaAula - só pra alterar o tau_efetivado.
                foreach (RepeaterItem itemAtividade in rptAulasEfetivado.Items)
                {
                    CheckBox chkEfetivado = (CheckBox)itemAtividade.FindControl("chkEfetivado");
                    int tau_id = Convert.ToInt32(((Label)itemAtividade.FindControl("lbltau_id")).Text);
                    DateTime tau_data = Convert.ToDateTime(((Label)itemAtividade.FindControl("lbltnt_data")).Text);

                    Guid usu_id_criou_aula = Guid.Empty;
                    Label lblUsuId = (Label)itemAtividade.FindControl("lblUsuId");
                    if (lblUsuId != null && !string.IsNullOrEmpty(lblUsuId.Text))
                    {
                        usu_id_criou_aula = new Guid(lblUsuId.Text);
                    }

                    bool permissaoAlteracao = permiteLancarFrequencia && Convert.ToBoolean(((HiddenField)itemAtividade.FindControl("hdnPermissaoAlteracao")).Value);
                    if (permissaoAlteracao && __SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                    {
                        permissaoAlteracao = (situacaoTurmaDisciplina == 1 || (situacaoTurmaDisciplina != 1 && __SessionWEB.__UsuarioWEB.Usuario.usu_id == usu_id_criou_aula));
                    }

                    permissaoAlteracao &= !periodoEfetivado;

                    if (permissaoAlteracao)
                    {
                        if ((entitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Encerrada
                                || entitiesControleTurma.turma.tur_situacao == (byte)TUR_TurmaSituacao.Extinta) &&
                            entitiesControleTurma.turma.tur_dataEncerramento != new DateTime() &&
                            tau_data > entitiesControleTurma.turma.tur_dataEncerramento)
                        {
                            throw new ValidationException("Existem aulas com data maior que a data de encerramento da turma.");
                        }

                        CLS_TurmaAula ent = listaTurmaAulaBD.FirstOrDefault(p => p.tud_id == entitiesControleTurma.turmaDisciplina.tud_id && p.tau_id == tau_id);
                        if (!ent.IsNew && ent.tau_dataAlteracao > dataUltimaAlteracao)
                        {
                            recarregarDataAula = false;
                            throw new ValidationException(GetGlobalResourceObject("Academico", "ControleTurma.Listao.Validacao_Data_TurmaFrequencia").ToString());
                        }

                        lstTauSalvas.Add(tau_id);

                        ent.tau_efetivado = chkEfetivado.Checked;
                        ent.tau_statusFrequencia = (byte)(chkEfetivado.Checked ? CLS_TurmaAulaStatusFrequencia.Efetivada
                                                                               : CLS_TurmaAulaStatusFrequencia.Preenchida);
                        ent.usu_idDocenteAlteracao = __SessionWEB.__UsuarioWEB.Usuario.usu_id;
                        ent.tpc_id = tpc_id;


                        listTurmaAula.Add(ent);
                    }
                }

                foreach (RepeaterItem itemAluno in rptAlunosFrequencia.Items)
                {
                    rptAulasEfetivado = (Repeater)itemAluno.FindControl("rptAulas");
                    Int64 alu_id = Convert.ToInt64(((Label)itemAluno.FindControl("lblalu_id")).Text);
                    Int32 mtu_id = Convert.ToInt32(((Label)itemAluno.FindControl("lblmtu_id")).Text);
                    Int32 mtd_id = Convert.ToInt32(((Label)itemAluno.FindControl("lblmtd_id")).Text);

                    // Adiciona itens na lista de TurmaNota - só pra alterar o tnt_efetivado.
                    foreach (RepeaterItem itemAtividadeAluno in rptAulasEfetivado.Items)
                    {
                        Guid usu_id_criou_aula = Guid.Empty;
                        Label lblUsuId = (Label)itemAtividadeAluno.FindControl("lblUsuId");
                        if (lblUsuId != null && !string.IsNullOrEmpty(lblUsuId.Text))
                        {
                            usu_id_criou_aula = new Guid(lblUsuId.Text);
                        }

                        bool permiteAlteracao; 
                        Boolean.TryParse(((HiddenField)itemAtividadeAluno.FindControl("hdnPermissaoAlteracao")).Value, out permiteAlteracao);
                        bool permissaoAlteracao = permiteLancarFrequencia && permiteAlteracao;

                        if (permissaoAlteracao && __SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                        {
                            permissaoAlteracao = (situacaoTurmaDisciplina == 1 || (situacaoTurmaDisciplina != 1 && __SessionWEB.__UsuarioWEB.Usuario.usu_id == usu_id_criou_aula));
                        }

                        permissaoAlteracao &= !periodoEfetivado;

                        if (permissaoAlteracao)
                        {
                            int tau_id = Convert.ToInt32(((Label)itemAtividadeAluno.FindControl("lbltau_id")).Text);
                            CheckBoxList cblFrequencia = (CheckBoxList)itemAtividadeAluno.FindControl("cblFrequencia");
                            int frequencia = 0;
                            string bitmap = "";
                            for (int i = 0; i < cblFrequencia.Items.Count; i++)
                            {
                                frequencia += cblFrequencia.Items[i].Selected ? 1 : 0;
                                bitmap += cblFrequencia.Items[i].Selected ? "1" : "0";
                            }

                            CLS_TurmaAulaAluno ent = new CLS_TurmaAulaAluno
                            {
                                tud_id = entitiesControleTurma.turmaDisciplina.tud_id
                                    ,
                                tau_id = tau_id
                                    ,
                                alu_id = alu_id
                                    ,
                                mtu_id = mtu_id
                                    ,
                                mtd_id = mtd_id
                                    ,
                                taa_frequencia = frequencia
                                    ,
                                taa_situacao = 1
                                        ,
                                taa_frequenciaBitMap = bitmap
                            };

                            listTurmaAulaAluno.Add(ent);
                        }
                    }
                }

                CLS_TurmaAulaAlunoBO.Save(listTurmaAulaAluno, listTurmaAula, entitiesControleTurma.turma.tur_id, 
                                          entitiesControleTurma.turmaDisciplina.tud_id, posicaoDocente, 
                                          entitiesControleTurma.turma, entitiesControleTurma.formatoAvaliacao, 
                                          entitiesControleTurma.curriculoPeriodo, __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                                          (byte)LOG_TurmaAula_Alteracao_Origem.WebListao, (byte)LOG_TurmaAula_Alteracao_Tipo.AlteracaoFreq, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                try
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, string.Concat(GetGlobalResourceObject("WebControls", "UCLancamentoFrequencia.Frequencia").ToString(),
                                                                            "cal_id: ", entitiesControleTurma.turma.cal_id, " | tpc_id: ", tpc_id,
                                                                            " | tur_id: ", entitiesControleTurma.turma.tur_id, "; tud_id: ", entitiesControleTurma.turmaDisciplina.tud_id));
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                }

                if (this.Recarregar != null)
                    this.Recarregar(false, false, false, true);

                msg = UtilBO.GetErroMessage(GetGlobalResourceObject("WebControls", "UCLancamentoFrequencia.FrequanciaSalva").ToString(), UtilBO.TipoMensagem.Sucesso);
            }
            return true;
        }
        #endregion Métodos

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        
        protected void lkbAnterior_Click(object sender, EventArgs e)
        {
            if (this.Recarregar != null)
                this.Recarregar(true, false, true, false);
        }

        protected void lkbProximo_Click(object sender, EventArgs e)
        {
            if (this.Recarregar != null)
                this.Recarregar(true, true, false, false);
        }
        
        protected void rptAlunosFrequencia_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
                (e.Item.ItemType == ListItemType.AlternatingItem) ||
                (e.Item.ItemType == ListItemType.Header))
            {
                Repeater rptAulas = (Repeater)e.Item.FindControl("rptAulas");
                Repeater rptAulasEfetivado = (Repeater)e.Item.FindControl("rptAulasEfetivado"); 
                //DataTable dtAulas;
                List<AulasAlunos> dados;

                bool AlunoDispensado = Convert.ToBoolean((DataBinder.Eval(e.Item.DataItem, "AlunoDispensado") ?? false));

                if (e.Item.ItemType == ListItemType.Header)
                {
                    HtmlControl thCompensacao = (HtmlControl)e.Item.FindControl("thCompensacao");
                    thCompensacao.Visible = permiteVisualizarCompensacao;

                    // Carrega o cabeçalho com os nomes das Aulas.
                    dados = (from sTurmaAulaAluno dr in VS_Aulas_Alunos
                             group dr by dr.tau_id
                                 into g
                                 where ltPermissaoFrequencia.Any(p => p.tdt_posicaoPermissao == g.First().tdt_posicao && p.pdc_permissaoConsulta)
                                 select new AulasAlunos
                                 {
                                     tau_id = g.Key
                                     ,
                                     tau_data = g.First().tau_data
                                     ,
                                     tau_efetivado = g.First().tau_efetivado
                                     ,
                                     AlunoDispensado = AlunoDispensado
                                     ,
                                     tdt_posicao = g.First().tdt_posicao
                                     ,
                                     permissaoAlteracao = g.First().permissaoAlteracao && !periodoEfetivado
                                     ,
                                     usu_id = g.First().usu_id
                                     ,
                                     tau_reposicao = g.First().tau_reposicao
                                 }).ToList();
                }
                else
                {
                    long Alu_id = Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "alu_id"));
                    int Mtu_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "mtu_id"));

                    Int32 mtd_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "mtd_id"));
                    HtmlControl tdCompensacao = (HtmlControl)e.Item.FindControl("tdCompensacao");

                    HtmlControl tdNomeAluno = (HtmlControl)e.Item.FindControl("tdNomeAluno");
                    HtmlControl tdNumeroChamada = (HtmlControl)e.Item.FindControl("tdNumeroChamada");

                    int situacao = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "mtd_situacao"));
                    if (situacao == Convert.ToInt32(MTR_MatriculaTurmaDisciplinaSituacao.Inativo))
                    {
                        tdNumeroChamada.Style["background-color"] = tdNomeAluno.Style["background-color"] = tdCompensacao.Style["background-color"] = ApplicationWEB.AlunoInativo;
                    }

                    tdCompensacao.Visible = permiteVisualizarCompensacao;

                    dados = (from sTurmaAulaAluno dr in VS_Aulas_Alunos
                             where
                                 dr.alu_id == Alu_id
                                 && dr.mtu_id == Mtu_id
                                 && dr.mtd_id == mtd_id
                                 && ltPermissaoFrequencia.Any(p => p.tdt_posicaoPermissao == dr.tdt_posicao && p.pdc_permissaoConsulta)
                             select new AulasAlunos
                             {
                                 tau_id = dr.tau_id
                                 ,
                                 tau_data = dr.tau_data
                                 ,
                                 taa_frequencia = dr.taa_frequencia
                                 ,
                                 taa_frequenciaBitMap = dr.taa_frequenciaBitMap
                                 ,
                                 tau_efetivado = dr.tau_efetivado
                                 ,
                                 tau_numeroAulas = dr.tau_numeroAulas
                                 ,
                                 tdt_posicao = dr.tdt_posicao
                                 ,
                                 falta_justificada = dr.falta_justificada
                                 ,
                                 AlunoDispensado = AlunoDispensado
                                 ,
                                 permissaoAlteracao = dr.permissaoAlteracao && !periodoEfetivado
                                 ,
                                 mtd_situacao = situacao
                                 ,
                                 AlunoComCompensacao = dr.AlunoComCompensacao
                                 ,
                                 usu_id = dr.usu_id
                                 ,
                                 tau_reposicao = dr.tau_reposicao
                                 ,
                                 falta_abonada = dr.falta_abonada
                             }).ToList();

                    // Seta as datas de matrícula e saída para serem usadas no databind de Aulas.
                    mtd_dataMatriculaAluno = Convert.ToDateTime(DataBinder.Eval(e.Item.DataItem, "mtd_dataMatricula"));
                    mtd_dataSaidaAluno = DataBinder.Eval(e.Item.DataItem, "mtd_dataSaida") != null ? Convert.ToDateTime(DataBinder.Eval(e.Item.DataItem, "mtd_dataSaida")) : DateTime.MaxValue;

                    Image imgDetalharCompensacaoSituacao = (Image)e.Item.FindControl("imgDetalharCompensacaoSituacao");
                    if (imgDetalharCompensacaoSituacao != null)
                        imgDetalharCompensacaoSituacao.Visible = dados.Count > 0
                            ? dados.FirstOrDefault().AlunoComCompensacao
                            : false;

                    LinkButton btnRelatorioAEE = (LinkButton)e.Item.FindControl("btnRelatorioAEE");
                    if (btnRelatorioAEE != null)
                    {
                        btnRelatorioAEE.Visible = Convert.ToByte(DataBinder.Eval(e.Item.DataItem, "alu_situacaoID")) == (byte)ACA_AlunoSituacao.Ativo
                                                    && Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "PossuiDeficiencia"));
                        btnRelatorioAEE.CommandArgument = Alu_id.ToString();
                    }

                    // Mostra o ícone para as anotações de recuperação paralela (RP):
                    // - para todos os alunos, quando a turma for de recuperação paralela,
                    // - ou apenas para alunos com anotações de RP, quando for a turma regular relacionada com a recuperação paralela.
                    if (tudTipo == (byte)TurmaDisciplinaTipo.DisciplinaEletivaAluno
                        || lstAlunosRelatorioRP.Any(p => p.alu_id == Alu_id))
                    {
                        LinkButton btnRelatorioRP = (LinkButton)e.Item.FindControl("btnRelatorioRP");
                        if (btnRelatorioRP != null)
                        {
                            btnRelatorioRP.Visible = true;
                            btnRelatorioRP.CommandArgument = string.Format("{0};-1", Alu_id.ToString());
                        }
                    }
                }

                if (rptAulas != null)
                {
                    rptAulas.DataSource = dados;
                    rptAulas.DataBind();
                }
                if (rptAulasEfetivado != null)
                {
                    rptAulasEfetivado.DataSource = dados;
                    rptAulasEfetivado.DataBind();
                }
            }
        }

        protected void rptAlunosFrequencia_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DetalharCompensacao")
            {
                long alu_id;
                int mtu_id, mtd_id;
                alu_id = mtu_id = mtd_id = 0;

                string[] commandArgs = e.CommandArgument.ToString().Split(';');
                if (commandArgs.Length == 3)
                {
                    alu_id = Convert.ToInt64(commandArgs[0]);
                    mtu_id = Convert.ToInt32(commandArgs[1]);
                    mtd_id = Convert.ToInt32(commandArgs[2]);
                }
                if (alu_id > 0 && mtu_id > 0 && mtd_id > 0)
                {
                    if (CarregarAusencias != null)
                        CarregarAusencias(alu_id, mtu_id, mtd_id);
                }
            }
            else if (e.CommandName == "RelatorioRP")
            {
                try
                {
                    if (AbrirRelatorioRP != null)
                    {
                        string[] args = e.CommandArgument.ToString().Split(';');
                        AbrirRelatorioRP(Convert.ToInt64(args[0]), args[1]);
                    }
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    //lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar abrir as anotações da recuperação paralela para o aluno.", UtilBO.TipoMensagem.Erro);
                }
            }
            else if (e.CommandName == "RelatorioAEE")
            {
                try
                {
                    AbrirRelatorioAEE(Convert.ToInt64(e.CommandArgument.ToString()));
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    //lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar abrir os relatórios do AEE para o aluno.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void rptAulasHeader_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
               (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                // Se for cabeçalho, setar valor do checkbox.
                CheckBox chkEfetivado = (CheckBox)e.Item.FindControl("chkEfetivado");

                Guid usu_id_criou_ativ = Guid.Empty;
                Label lblUsuId = (Label)e.Item.FindControl("lblUsuId");
                if (lblUsuId != null && !string.IsNullOrEmpty(lblUsuId.Text))
                {
                    usu_id_criou_ativ = new Guid(lblUsuId.Text);
                }
                Label lblAulaReposicao = (Label)e.Item.FindControl("lblAulaReposicao");
                bool tau_reposicao = false;
                
                //Verifica se o lançamento de nota foi efetivado.
                bool tau_efetivado = false;
                bool permissaoAlteracao = permiteLancarFrequencia && Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "permissaoAlteracao")) > 0;
                bool permissaoModuloAlteracao = false;
                bool permissaoModuloAlteracaoInfantil = PermissaoModuloLancamentoFrequenciaInfantil.IsNew || PermissaoModuloLancamentoFrequenciaInfantil.pmo_permissaoEdicao;

                if (!usuarioPermissao)
                {
                    permissaoModuloAlteracao = !PermissaoModuloLancamentoFrequencia.IsNew && PermissaoModuloLancamentoFrequencia.pmo_permissaoEdicao;
                }

                if (permissaoAlteracao && permissaoModuloAlteracaoInfantil && __SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                {
                    permissaoAlteracao = (situacaoTurmaDisciplina == 1 || (situacaoTurmaDisciplina != 1 && __SessionWEB.__UsuarioWEB.Usuario.usu_id == usu_id_criou_ativ));
                }
                if (permissaoAlteracao && permissaoModuloAlteracaoInfantil)
                {
                    permiteEdicao = true;
                }

                permiteEdicao &= !periodoEfetivado;

                if (!String.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "tau_efetivado").ToString()))
                {
                    tau_efetivado = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "tau_efetivado"));
                }

                if (chkEfetivado != null)
                {
                    chkEfetivado.Checked = tau_efetivado;

                    if (posicaoDocente > 0)
                    {
                        Int16 tdt_posicao = Convert.ToInt16(DataBinder.Eval(e.Item.DataItem, "tdt_posicao"));
                        bool permiteEditar = (__SessionWEB.__UsuarioWEB.Docente.doc_id == 0 || (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0 && ltPermissaoFrequencia.Any(p => p.tdt_posicaoPermissao == tdt_posicao & p.pdc_permissaoEdicao)));
                        chkEfetivado.Enabled &= permiteEditar;
                    }

                    chkEfetivado.Enabled &= ((usuarioPermissao && permissaoModuloAlteracaoInfantil) || permissaoModuloAlteracao) && periodoAberto && permissaoAlteracao && !periodoEfetivado;
                }

                if (!String.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "tau_reposicao").ToString()))
                {
                    tau_reposicao = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "tau_reposicao"));
                }
                if (lblAulaReposicao != null)
                    lblAulaReposicao.Text = (tau_reposicao) ? "Aula de reposição<br/>" : string.Empty;
            }
        }

        protected void rptAulas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
                (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                CheckBoxList cblFrequencia = (CheckBoxList)e.Item.FindControl("cblFrequencia");
                int frequencia = Convert.ToInt32(String.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "taa_frequencia").ToString()) ? "0" : DataBinder.Eval(e.Item.DataItem, "taa_frequencia"));
                char[] taa_frequenciaBitMap = (String.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "taa_frequenciaBitMap").ToString()) ? "" : DataBinder.Eval(e.Item.DataItem, "taa_frequenciaBitMap")).ToString().ToCharArray();
                string sNumeroAulas = DataBinder.Eval(e.Item.DataItem, "tau_numeroAulas").ToString();
                int numeroAulas = string.IsNullOrEmpty(sNumeroAulas) ? 0 : Convert.ToInt32(sNumeroAulas);

                Guid usu_id_criou_ativ = Guid.Empty;
                Label lblUsuId = (Label)e.Item.FindControl("lblUsuId");
                if (lblUsuId != null && !string.IsNullOrEmpty(lblUsuId.Text))
                {
                    usu_id_criou_ativ = new Guid(lblUsuId.Text);
                }

                bool permissaoAlteracao = permiteLancarFrequencia && Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "permissaoAlteracao")) > 0;
                bool permissaoModuloAlteracao = false;
                bool permissaoModuloAlteracaoInfantil = PermissaoModuloLancamentoFrequenciaInfantil.IsNew || PermissaoModuloLancamentoFrequenciaInfantil.pmo_permissaoEdicao;

                if (!usuarioPermissao)
                {
                    permissaoModuloAlteracao = !PermissaoModuloLancamentoFrequencia.IsNew && PermissaoModuloLancamentoFrequencia.pmo_permissaoEdicao;
                }

                if (permissaoAlteracao && permissaoModuloAlteracaoInfantil && __SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                {
                    permissaoAlteracao = (situacaoTurmaDisciplina == 1 || (situacaoTurmaDisciplina != 1 && __SessionWEB.__UsuarioWEB.Usuario.usu_id == usu_id_criou_ativ));
                }
                if (permissaoAlteracao && permissaoModuloAlteracaoInfantil)
                {
                    permiteEdicao = true;
                }

                permiteEdicao &= !periodoEfetivado;

                // Verificar se a data da aula está dentro do período da matrícula do aluno (data de matrícula
                //  e data de saída).
                string data = DataBinder.Eval(e.Item.DataItem, "tau_data").ToString();
                if (!String.IsNullOrEmpty(data))
                {
                    DateTime tau_data = Convert.ToDateTime(data);

                    cblFrequencia.Visible = ((tau_data.Date >= mtd_dataMatriculaAluno.Date) && (tau_data.Date < mtd_dataSaidaAluno.Date));
                }

                if (cblFrequencia != null)
                {
                    cblFrequencia.TabIndex = Convert.ToInt16(e.Item.ItemIndex + 1);

                    if (tipoApuracaoFrequencia == (byte)ACA_FormatoAvaliacaoTipoApuracaoFrequencia.Dia && crpControleTempo == (byte)ACA_CurriculoPeriodoControleTempo.Horas)
                    {
                        if (possuiRegencia && tudTipo != (byte)TurmaDisciplinaTipo.Regencia)
                        {
                            for (int i = 0; i < numeroAulas; i++)
                            {
                                ListItem li = new ListItem();
                                if (taa_frequenciaBitMap.Length > i && taa_frequenciaBitMap[i].Equals('1'))
                                    li.Selected = true;
                                li.Enabled = ((usuarioPermissao && permissaoModuloAlteracaoInfantil) || permissaoModuloAlteracao);
                                cblFrequencia.Items.Add(li);
                            }
                        }
                        else
                        {
                            if (numeroAulas > 0)
                            {
                                ListItem li = new ListItem();
                                if (frequencia > 0)
                                    li.Selected = true;
                                li.Enabled = ((usuarioPermissao && permissaoModuloAlteracaoInfantil) || permissaoModuloAlteracao);
                                cblFrequencia.Items.Add(li);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < numeroAulas; i++)
                        {
                            ListItem li = new ListItem();
                            if (taa_frequenciaBitMap.Length > i && taa_frequenciaBitMap[i].Equals('1'))
                                li.Selected = true;
                            li.Enabled = ((usuarioPermissao && permissaoModuloAlteracaoInfantil) || permissaoModuloAlteracao);
                            cblFrequencia.Items.Add(li);
                        }
                    }

                    cblFrequencia.Enabled &= periodoAberto && permissaoAlteracao && permissaoModuloAlteracaoInfantil && !periodoEfetivado;

                    if (Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "falta_abonada")))
                    {
                        cblFrequencia.CssClass += " esconderCheckBox";
                    }
                }

                bool AlunoDispensado = Convert.ToBoolean((DataBinder.Eval(e.Item.DataItem, "AlunoDispensado") ?? false));

                if (AlunoDispensado)
                {
                    // Pinta célula que possui falta justificada.
                    HtmlGenericControl divAulasAluno = (HtmlGenericControl)e.Item.FindControl("divAulasAluno");

                    // Pinta célula que possui aluno dispensado.
                    if (divAulasAluno != null)
                    {
                        divAulasAluno.Style["background-color"] = ApplicationWEB.AlunoDispensado;
                    }
                }

                int situacao = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "mtd_situacao"));
                if (situacao == Convert.ToInt32(MTR_MatriculaTurmaDisciplinaSituacao.Inativo))
                {
                    HtmlControl tdAulas = (HtmlControl)e.Item.FindControl("tdAulas");
                    tdAulas.Style["background-color"] = ApplicationWEB.AlunoInativo;
                }
            }
        }
        #endregion Eventos
    }
}