using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System.Text;

namespace GestaoEscolar.Classe.PlanejamentoDiario
{
    public partial class Cadastro : MotherPageLogado
    {
        #region CONSTANTES

        private const int indiceColunaCalendario = 1;
        private const int indiceColunaCurso = 2;
        private const int indiceColunaSab = 10;

        #endregion

        #region PROPRIEDADES

        private string periodosEfetivados;

        /// <summary>
        /// Retorna e seta o id do docente.
        /// </summary>
        private long VsIdDoc
        {
            get
            {
                if (ViewState["VsIdDoc"] != null)
                {
                    return Convert.ToInt64(ViewState["VsIdDoc"]);
                }
                return -1;
            }
            set
            {
                ViewState["VsIdDoc"] = value;
            }
        }

        /// <summary>
        /// Página de retorno.
        /// </summary>
        private string VS_URL_Retorno
        {
            get
            {
                return (ViewState["VS_URL_Retorno"] ?? "~/Classe/PlanejamentoDiario/Cadastro.aspx").ToString();
            }

            set
            {
                ViewState["VS_URL_Retorno"] = value;
            }
        }

        /// <summary>
        /// Informa se o período já foi fechado (evento de fechamento já acabou) e não há nenhum evento de fechamento por vir.
        /// Se o período ainda estiver ativo então não verifica o evento de fechamento
        /// </summary>
        /// <param name="tpc_id">ID do período do calendário</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="cap_dataFim">Data fim do período</param>
        /// <returns></returns>
        private bool VS_PeriodoEfetivado(int tpc_id, int cal_id, long tur_id, DateTime cap_dataFim)
        {
            bool efetivado = false;

            //Se o bimestre está ativo ou nem começou então não bloqueia pelo evento de fechamento
            if (DateTime.Today <= cap_dataFim)
                return false;

            List<ACA_Evento> lstEventos = ACA_EventoBO.GetEntity_Efetivacao_List(cal_id, tur_id, __SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo, false, __SessionWEB.__UsuarioWEB.Docente.doc_id);

            //Só permite editar o bimestre se tiver evento ativo
            efetivado = !lstEventos.Exists(p => p.tpc_id == tpc_id && p.tev_id == ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id) &&
                                                DateTime.Today >= p.evt_dataInicio && DateTime.Today <= p.evt_dataFim);

            return efetivado;
        }

        #endregion PROPRIEDADES

        #region EVENTOS
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Guid ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;
                long doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;
                int visao = __SessionWEB.__UsuarioWEB.Grupo.vis_id;

                lblAviso.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Classe", "PlanejamentoDiario.Cadastro.lblAviso.Text").ToString(), UtilBO.TipoMensagem.Informacao);

                VsIdDoc = doc_id;

                ScriptManager sm = ScriptManager.GetCurrent(this);
                if (sm != null)
                {
                    RegistrarParametrosMensagemSair(true, (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.ExitPageConfirm));
                    sm.Scripts.Add(new ScriptReference("~/Includes/jsSetExitPageConfirmer.js"));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                    sm.Scripts.Add(new ScriptReference("~/Includes/jsPlanejamentoDiario.js"));
                }

                if (!IsPostBack)
                {
                    if (visao == SysVisaoID.Individual && doc_id > 0)
                    {
                        if (Session["URL_Retorno"] != null)
                        {
                            VS_URL_Retorno = Session["URL_Retorno"].ToString();
                            Session.Remove("URL_Retorno");
                        }

                        CarregaPeriodo();
                    }
                    else
                    {
                        __SessionWEB.PostMessages = UtilBO.GetErroMessage("Você não possui permissão para acessar a página solicitada.", UtilBO.TipoMensagem.Alerta);
                        Response.Redirect("~/Index.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }

                UCCCalendario1.IndexChanged += UCCCalendario1_IndexChanged;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar planejamento diário.", UtilBO.TipoMensagem.Erro);
            }

        }

        protected void btnGerar_Click(object sender, EventArgs e)
        {
            Salvar();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            RedirecionarPagina(VS_URL_Retorno);
        }

        protected void UCCCalendario1_IndexChanged()
        {
            chkIntervalo.Checked = divPeriodo.Visible = divPlanejamentoDiario.Visible = btnGerar.Visible = divIntervalo.Visible = false;

            if (UCCCalendario1.Valor > 0 || UCCCalendario1.QuantidadeItensCombo == 1)
            {
                List<sTipoPeriodoCalendario> lstPeriodo = UCCCalendario1.QuantidadeItensCombo == 1 ? 
                                                          ACA_TipoPeriodoCalendarioBO.SelecionaTipoPeriodoCalendario(ApplicationWEB.AppMinutosCacheLongo)
                                                          : ACA_TipoPeriodoCalendarioBO.SelecionaTipoPeriodoCalendarioPorCalendario(UCCCalendario1.Valor, ApplicationWEB.AppMinutosCacheLongo);
                var x = from dr in lstPeriodo
                        where dr.tpc_situacao.ToString().Equals("Não", StringComparison.OrdinalIgnoreCase)
                        select dr;

                rbtPeriodo.DataSource = x;
                rbtPeriodo.DataBind();

                divPeriodo.Visible = true;

                if (!x.Any())
                {
                    divPeriodo.Visible = false;
                    lblMessage.Text = UtilBO.GetErroMessage("Não foram encontrados períodos para o calendário selecionado.", UtilBO.TipoMensagem.Alerta);
                }
                else
                {
                    rbtPeriodo.SelectedIndex = 0;
                    rbtPeriodo_SelectedIndexChanged(rbtPeriodo, new EventArgs());
                }
            }
        }

        protected void rbtPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            divPlanejamentoDiario.Visible = true;
            CarregaQuantidadeAula();
        }

        protected void chkIntervalo_CheckedChanged(object sender, EventArgs e)
        {
            divIntervalo.Visible = chkIntervalo.Checked;

            txtDataInicio.Text = txtDataFim.Text = "";
        }

        protected void ValidarDatasPeriodoLetivo_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime dtIni = Convert.ToDateTime(txtDataInicio.Text);
            DateTime dtFim = Convert.ToDateTime(txtDataFim.Text);

            args.IsValid = (dtIni <= dtFim);
        }

        protected void gdvAulas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Dictionary<CLS_TurmaAulaGeradaDiaSemana, string> diasSemana = CLS_TurmaAulaGeradaBO.RetornaDiasSemanaGerarAulas();

                if (VS_PeriodoEfetivado(Convert.ToInt32(rbtPeriodo.SelectedValue), UCCCalendario1.Valor,
                                        Convert.ToInt64(gdvAulas.DataKeys[e.Row.RowIndex]["tur_id"]),
                                        Convert.ToDateTime(gdvAulas.DataKeys[e.Row.RowIndex]["cap_dataFim"])))
                {

                    periodosEfetivados += "<br/>" + gdvAulas.DataKeys[e.Row.RowIndex]["cap_descricao"].ToString() + ", " +
                                                    gdvAulas.DataKeys[e.Row.RowIndex]["escola"].ToString() + ", " +
                                                    gdvAulas.DataKeys[e.Row.RowIndex]["curso"].ToString() + ", " +
                                                    gdvAulas.DataKeys[e.Row.RowIndex]["turno"].ToString() + ", " +
                                                    gdvAulas.DataKeys[e.Row.RowIndex]["TurmaDisciplina"].ToString();

                    foreach (var dia in diasSemana)
                    {
                        TextBox txt = e.Row.FindControl(string.Format("txtAulas{0}", dia.Value)) as TextBox;
                        if (txt != null)
                        {
                            //txt.Text = "";
                            txt.Enabled = false;
                        }
                    }
                }
                else
                {
                    var tud_tipo = Convert.ToByte(gdvAulas.DataKeys[e.Row.RowIndex]["tud_tipo"]);
                    var ttn_tipo = Convert.ToByte(gdvAulas.DataKeys[e.Row.RowIndex]["ttn_tipo"]);

                    foreach (var dia in diasSemana)
                    {
                        TextBox txt = e.Row.FindControl(string.Format("txtAulas{0}", dia.Value)) as TextBox;
                        if (txt != null && tud_tipo == (byte)TurmaDisciplinaTipo.DisciplinaPrincipal && ttn_tipo == (byte)ACA_TipoTurnoBO.TipoTurno.Integral)
                        {
                            //txt.Text = "";
                            txt.Enabled = false;
                        }
                    }

                    btnGerar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar ||
                                       __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
                }
            }
        }

        #endregion EVENTOS

        #region MÉTODOS

        /// <summary>
        /// Salva as aulas geradas.
        /// </summary>
        private void Salvar()
        {
            try
            {
                if (Page.IsValid)
                {
                    DateTime? dataInicio = null;
                    DateTime? dataFim = null;

                    #region Se informado um intervalo de datas, valida

                    if (chkIntervalo.Checked)
                    {
                        DateTime temp;
                        if (!DateTime.TryParse(txtDataInicio.Text, out temp))
                            throw new ValidationException("Data de início está inválida, deve estar no formato DD/MM/AAAA.");
                        dataInicio = temp;

                        if (!DateTime.TryParse(txtDataFim.Text, out temp))
                            throw new ValidationException("Data de fim está inválida, deve estar no formato DD/MM/AAAA.");
                        dataFim = temp;

                        if (dataFim < dataInicio)
                            throw new ValidationException("Data de início não pode ser maior que a data de fim.");
                    }

                    #endregion

                    int tpc_id = Convert.ToInt32(rbtPeriodo.SelectedValue);

                    var aulasSalvar = new List<CLS_TurmaAulaGerada>();
                    var dicTurmasDisciplinas = new Dictionary<long, string>();

                    #region Valida a qtde de horas semanais e preenche aulasSalvar e dicTurmasDisciplinas

                    Dictionary<CLS_TurmaAulaGeradaDiaSemana, string> diasSemana = CLS_TurmaAulaGeradaBO.RetornaDiasSemanaGerarAulas();

                    string str_tud_ids = string.Join(";", (from GridViewRow row in gdvAulas.Rows
                                                           select gdvAulas.DataKeys[row.RowIndex]["tud_id"].ToString()).Distinct().ToArray());
                    // Recupera as CLS_TurmaAulaGerada que já existem no banco para o docente e período informado
                    var tagsBanco = CLS_TurmaAulaGeradaBO.SelectBy_DisciplinaDocente(str_tud_ids, VsIdDoc, tpc_id);

                    long tud_idAnterior = 0;
                    int cargaHorariaSemanal = 0;
                    int cargaHorariaDia = 0;
                    var nomeTurmaDisciplina = string.Empty;

                    foreach (GridViewRow row in gdvAulas.Rows)
                    {
                        var tud_id = Convert.ToInt64(gdvAulas.DataKeys[row.RowIndex]["tud_id"]);
                        var tud_cargaHorariaSemanal = Convert.ToInt32(gdvAulas.DataKeys[row.RowIndex]["tud_cargaHorariaSemanal"]);
                        var tud_tipo = Convert.ToByte(gdvAulas.DataKeys[row.RowIndex]["tud_tipo"]);
                        var tud_idRelacionada = String.IsNullOrEmpty(gdvAulas.DataKeys[row.RowIndex]["tud_idRelacionada"].ToString()) ? -1 : Convert.ToInt64(gdvAulas.DataKeys[row.RowIndex]["tud_idRelacionada"]);
                        var tdt_posicao = Convert.ToByte(gdvAulas.DataKeys[row.RowIndex]["tdt_posicao"].ToString());
                        var fav_fechamentoAutomatico = Convert.ToBoolean(gdvAulas.DataKeys[row.RowIndex]["fav_fechamentoAutomatico"]);
                        var fav_tipoApuracaoFrequencia = Convert.ToByte(gdvAulas.DataKeys[row.RowIndex]["fav_tipoApuracaoFrequencia"]);
                        var ttn_tipo = Convert.ToByte(gdvAulas.DataKeys[row.RowIndex]["ttn_tipo"]);

                        //Se a turma estiver em um período efetivado não adiciona para salvar
                        if (VS_PeriodoEfetivado(tpc_id, Convert.ToInt32(gdvAulas.DataKeys[row.RowIndex]["cal_id"]),
                                                Convert.ToInt64(gdvAulas.DataKeys[row.RowIndex]["tur_id"]),
                                                Convert.ToDateTime(gdvAulas.DataKeys[row.RowIndex]["cap_dataFim"])))
                            continue;

                        // Mantem a contagem de aulas a gerar, caso seja o mesmo tud_id (docencia compartilhada com mais de uma disciplina),
                        // pois o somatorio deve considerar todas as disciplinas relacionadas.
                        if (tud_id != tud_idAnterior)
                        {
                            cargaHorariaSemanal = 0;
                            tud_idAnterior = tud_id;

                            nomeTurmaDisciplina = gdvAulas.DataKeys[row.RowIndex]["TurmaDisciplina"].ToString();

                            if (tud_tipo == (byte)TurmaDisciplinaTipo.DocenciaCompartilhada)
                                nomeTurmaDisciplina = nomeTurmaDisciplina.Substring(0, nomeTurmaDisciplina.LastIndexOf('/') - 1);

                            if (!dicTurmasDisciplinas.ContainsKey(tud_id))
                                dicTurmasDisciplinas.Add(tud_id, nomeTurmaDisciplina);
                        }                        

                        foreach (var dia in diasSemana)
                        {
                            cargaHorariaDia = 0;

                            TextBox txt = row.FindControl(string.Format("txtAulas{0}", dia.Value)) as TextBox;
                            if (!string.IsNullOrEmpty(txt.Text) 
                                && (!int.TryParse(txt.Text, out cargaHorariaDia) || cargaHorariaDia <= 0))
                            {
                                // se informou um valor não-numérico ou negativo
                                throw new ValidationException(GetGlobalResourceObject("Classe", "PlanejamentoDiario.Cadastro.lblAviso.Text").ToString());
                            }

                            cargaHorariaSemanal += cargaHorariaDia;

                            // se estourou o limite de horas da semana, dá erro
                            if (cargaHorariaSemanal > tud_cargaHorariaSemanal)
                            {
                                throw new ValidationException(string.Format(GetGlobalResourceObject("Classe", "PlanejamentoDiario.Cadastro.MensagemCargaHorariaTurma").ToString(), nomeTurmaDisciplina));
                            }

                            var tag = tagsBanco.FirstOrDefault(i => i.tud_id == tud_id
                                                                 && i.tag_diaSemana == (byte)dia.Key
                                                                 && i.tdt_posicao == tdt_posicao
                                                                 && i.tpc_id == tpc_id
                                                                 && (tud_idRelacionada <= 0 || i.tud_idRelacionada == tud_idRelacionada));

                            if (tag == null)
                            {
                                tag = new CLS_TurmaAulaGerada()
                                                {
                                                    tag_id = -1
                                                };
                            }

                            tag.tud_id = tud_id;
                            tag.tag_diaSemana = (byte)dia.Key;
                            tag.tag_numeroAulas = cargaHorariaDia;
                            tag.tdt_posicao = tdt_posicao;
                            tag.tag_situacao = 1;
                            tag.tpc_id = tpc_id;
                            tag.tud_idRelacionada = tud_idRelacionada;
                            tag.uni_id = Convert.ToInt32(gdvAulas.DataKeys[row.RowIndex]["uni_id"]);
                            tag.esc_id = Convert.ToInt32(gdvAulas.DataKeys[row.RowIndex]["esc_id"]);
                            tag.cal_id = Convert.ToInt32(gdvAulas.DataKeys[row.RowIndex]["cal_id"]);
                            tag.tur_id = Convert.ToInt64(gdvAulas.DataKeys[row.RowIndex]["tur_id"]);
                            tag.tud_tipo = tud_tipo;
                            tag.tud_cargaHorariaSemanal = tud_cargaHorariaSemanal;
                            tag.fav_fechamentoAutomatico = fav_fechamentoAutomatico;
                            tag.fav_tipoApuracaoFrequencia = fav_tipoApuracaoFrequencia;
                            tag.ttn_tipo = ttn_tipo;
                            aulasSalvar.Add(tag);
                        }
                    }

                    #endregion

                    bool gerouTodasAulas;
                    Dictionary<long, string> ultrapassouCargaHorariaSemanal;
                    Dictionary<long, string> semVigencia;
                    Dictionary<long, string> semAulasPrevistas;
                    Dictionary<long, Exception> outrosErros;

                    if (CLS_TurmaAulaGeradaBO.GerarAulasPlanejamentoDiario(aulasSalvar, tpc_id, VsIdDoc, dataInicio, dataFim, 
                                                                           __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                                                                           __SessionWEB.__UsuarioWEB.Usuario.ent_id, 
                                                                           dicTurmasDisciplinas,
                                                                           out gerouTodasAulas,
                                                                           out ultrapassouCargaHorariaSemanal,
                                                                           out semVigencia,
                                                                           out semAulasPrevistas,
                                                                           out outrosErros,
                                                                           (byte)LOG_TurmaAula_Alteracao_Origem.WebAgenda))
                    {
                        ApplicationWEB._GravaLogSistema(tagsBanco.Count > 0 ? LOG_SistemaTipo.Update : LOG_SistemaTipo.Insert
                                , "Agenda gerada | " + "doc_id: " + VsIdDoc + " | tpc_id: " + tpc_id + " | tud_id: " + str_tud_ids);

                        StringBuilder alerta = new StringBuilder();

                        if (!gerouTodasAulas)
                            alerta.Append(GetGlobalResourceObject("Classe", "PlanejamentoDiario.Cadastro.MensagemAulaFuturaComConteudo"));

                        if (ultrapassouCargaHorariaSemanal.Any())
                        {
                            alerta.Append(alerta.Length == 0 ? string.Empty : "<br />")
                                  .Append(GetGlobalResourceObject("Classe", "PlanejamentoDiario.Cadastro.MensagemCargaHorariaUltrapassada"));

                            ultrapassouCargaHorariaSemanal.Values.ToList().ForEach(msg => alerta.Append("<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- ")
                                                                                                .Append(msg));
                        }

                        if (semVigencia.Any())
                        {
                            alerta.Append(alerta.Length == 0 ? string.Empty : "<br />")
                                  .Append(GetGlobalResourceObject("Classe", "PlanejamentoDiario.Cadastro.MensagemSemVigencia"));

                            semVigencia.Values.ToList().ForEach(msg => alerta.Append("<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- ")
                                                                                                .Append(msg));
                        }

                        if (outrosErros.Any())
                        {
                            alerta.Append(alerta.Length == 0 ? string.Empty : "<br /><br />")
                                  .Append("Outros alertas:");

                            foreach (var erro in outrosErros)
                            {
                                if (erro.Value is ValidationException)
                                {
                                    alerta.AppendFormat("<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- {0}", erro.Value.Message);
                                }
                                else
                                {
                                    ApplicationWEB._GravaErro(erro.Value);

                                    alerta.Append("<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- ")
                                          .AppendFormat("Falha inesperada ao gravar {0}", dicTurmasDisciplinas[erro.Key]);
                                }
                            }
                        }

                        lblAlerta.Text = UtilBO.GetErroMessage(alerta.ToString(), UtilBO.TipoMensagem.Alerta);
                        lblAlerta.Visible = (alerta.Length > 0);

                        if (alerta.Length == 0)
                        {
                            var msg = tagsBanco.Count > 0 ? GetGlobalResourceObject("Classe", "PlanejamentoDiario.Cadastro.AgendaAlteradaComSucesso").ToString()
                                                          : GetGlobalResourceObject("Classe", "PlanejamentoDiario.Cadastro.AgendaGeradaComSucesso").ToString();

                            lblMessage.Text = UtilBO.GetErroMessage(msg, UtilBO.TipoMensagem.Sucesso);
                        }
                    }
                }
            }
            catch (ArgumentException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (DuplicateNameException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar gerar agenda.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Carrega as quantidades de aulas.
        /// </summary>
        public void CarregaQuantidadeAula()
        {
            try
            {
                btnGerar.Visible = false;

                if (UCCCalendario1.Valor > 0 && !rbtPeriodo.SelectedValue.Equals(""))
                {
                    int tpc_id = Convert.ToInt32(rbtPeriodo.SelectedValue);
                    DataTable dtAula = CLS_TurmaAulaGeradaBO.GerarAula(VsIdDoc, UCCCalendario1.Valor, tpc_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                    gdvAulas.Columns[indiceColunaCurso].HeaderText = GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    gdvAulas.Columns[indiceColunaSab].Visible = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_SABADO_GERAR_AULAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                    periodosEfetivados = "";
                    gdvAulas.DataSource = dtAula;
                    gdvAulas.DataBind();

                    if (!string.IsNullOrEmpty(periodosEfetivados))
                    {
                        lblPeriodoEfetivado.Visible = true;
                        lblPeriodoEfetivado.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Classe", "PlanejamentoDiario.MensagemEfetivado").ToString(),
                                                                         UtilBO.TipoMensagem.Informacao);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao carregar gerar aulas.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summar>
        /// Carrega na tela períodos de geração.
        /// </summary>
        public void CarregaPeriodo()
        {
            try
            {
                UCCCalendario1.Obrigatorio = true;
                UCCCalendario1.SelecionarAnoCorrente = true;
                UCCCalendario1.CarregarPorDocente(VsIdDoc);

                if (UCCCalendario1.Valor > 0 && UCCCalendario1.QuantidadeItensCombo <= 2)
                    gdvAulas.Columns[indiceColunaCalendario].Visible = !(divCalendario.Visible = false);

                UCCCalendario1_IndexChanged();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao carregar os períodos de geração.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion MÉTODOS
    }
}
