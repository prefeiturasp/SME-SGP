using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.CoreSSO.BLL;

namespace GestaoEscolar.WebControls.NavegacaoLancamentoClasse
{
    public partial class UCNavegacaoLancamentoClasse : MotherUserControl
    {
        #region Propriedades

        public WebControls_Combos_UCComboPeriodoCalendario uccPeriodoCalendario;

        /// <summary>
        /// Seta a visibilidade da div com todos os botões.
        /// </summary>
        public bool VisibleBotoes
        {
            set
            {
                divBotoesNavegacao.Visible = value;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de fav_id
        /// </summary>
        public int _VS_fav_id
        {
            get
            {
                if (ViewState["_VS_fav_id"] != null)
                    return Convert.ToInt32(ViewState["_VS_fav_id"]);
                return -1;
            }
            set
            {
                ViewState["_VS_fav_id"] = value;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de tur_id
        /// </summary>
        public long _VS_tur_id
        {
            get
            {
                if (ViewState["_VS_tur_id"] != null)
                    return Convert.ToInt64(ViewState["_VS_tur_id"]);
                return -1;
            }
            set
            {
                ViewState["_VS_tur_id"] = value;
            }
        }

        /// <summary>
        /// ID da avaliação selecionada.
        /// </summary>
        public int _VS_ava_id
        {
            get
            {
                if (ViewState["_VS_ava_id"] != null)
                    return Convert.ToInt32(ViewState["_VS_ava_id"]);
                return -1;
            }
            set
            {
                ViewState["_VS_ava_id"] = value;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de tud_id
        /// </summary>
        public long _VS_tud_id
        {
            get
            {
                if (ViewState["_VS_tud_id"] != null)
                    return Convert.ToInt32(ViewState["_VS_tud_id"]);
                return -1;
            }
            set
            {
                ViewState["_VS_tud_id"] = value;
            }
        }

        #region Efetivação

        /// <summary>
        /// Retorna ou seta a visibilidade do botão de efetivação.
        /// </summary>
        public bool VisibleEfetivacao
        {
            get
            {
                return btnEfetivacao.Visible;
            }
            set
            {
                btnEfetivacao.Visible = value;
            }
        }

        #endregion

        #region Lançamento de Notas

        /// <summary>
        /// Retorna ou seta a visbilidade do botão de lançamento de notas.
        /// </summary>
        public bool VisibleLancamentoNotas
        {
            get
            {
                return btnLancamentoAvaliacao.Visible;
            }
            set
            {
                btnLancamentoAvaliacao.Visible = value;
            }
        }

        #endregion

        #region Lançamento de Frequência

        ///// <summary>
        ///// Retorna ou seta a visibilidade do botão de lançamento de frequência.
        ///// </summary>
        //public bool VisibleLancamentoFrequencia
        //{
        //    get
        //    {
        //        return btnLancamentoFrequencia.Visible;
        //    }
        //    set
        //    {
        //        btnLancamentoFrequencia.Visible = value;
        //    }
        //}

        #endregion

        #region Lançamento de Frequência Mensal

        /// <summary>
        /// Retorna ou seta a visibilidade do botão de lançamento de frequência mensal.
        /// </summary>
        public bool VisibleLancamentoFrequenciaMensal
        {
            get
            {
                return btnLancamentoFrequenciaMensal.Visible;
            }
            set
            {
                btnLancamentoFrequenciaMensal.Visible = value;
            }
        }

        #endregion

        #endregion

        #region Eventos Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(Page);
            if (sm != null)
            {
                if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.BOTAO_SALVAR_PERMANECE_TELA
                    , __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                {
                    if (!Convert.ToString(btnEfetivacao.CssClass).Contains("btnMensagemUnload"))
                    {
                        btnEfetivacao.CssClass += " btnMensagemUnload";
                    }

                    if (!Convert.ToString(btnLancamentoAvaliacao.CssClass).Contains("btnMensagemUnload"))
                    {
                        btnLancamentoAvaliacao.CssClass += " btnMensagemUnload";
                    }

                    if (!Convert.ToString(btnLancamentoFrequencia.CssClass).Contains("btnMensagemUnload"))
                    {
                        btnLancamentoFrequencia.CssClass += " btnMensagemUnload";
                    }

                    if (!Convert.ToString(btnLancamentoFrequenciaMensal.CssClass).Contains("btnMensagemUnload"))
                    {
                        btnLancamentoFrequenciaMensal.CssClass += " btnMensagemUnload";
                    }
                }
            }

            if (!IsPostBack)
            {
                btnEfetivacao.Text = GestaoEscolarUtilBO.BuscaNomeModulo("~/Classe/Efetivacao/Cadastro.aspx", "Efetivação de notas", __SessionWEB.__UsuarioWEB.Grupo.gru_id);
                btnLancamentoAvaliacao.Text = GestaoEscolarUtilBO.BuscaNomeModulo("~/Classe/LancamentoAvaliacoes/Cadastro.aspx", "Lançamento de notas", __SessionWEB.__UsuarioWEB.Grupo.gru_id);
                btnLancamentoFrequencia.Text = GestaoEscolarUtilBO.BuscaNomeModulo("~/Classe/LancamentoFrequencia/Cadastro.aspx", "Lançamento de frequências", __SessionWEB.__UsuarioWEB.Grupo.gru_id);
                btnLancamentoFrequenciaMensal.Text = GestaoEscolarUtilBO.BuscaNomeModulo("~/Classe/LancamentoFrequencia/LancamentoMensal.aspx", "Lançamento de frequência mensal", __SessionWEB.__UsuarioWEB.Grupo.gru_id);
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Guarda o período selecionado na tela atual para a próxima tela que está sendo navegado.
        /// </summary>
        private void SetaPeriodoSelecionado()
        {
            if (uccPeriodoCalendario != null)
            {
                Session["valorPeriodoSelecionado"] = uccPeriodoCalendario.Valor;
            }
        }

        /// <summary>
        /// Recupera da sessão o valor selecionado no combo de período antes de mudar de tela, para
        /// manter a seleção de período.
        /// </summary>
        /// <returns></returns>
        public int[] RetornaPeriodoSelecionado
        {
            get
            {
                try
                {
                    if (Session["valorPeriodoSelecionado"] != null && Session["valorPeriodoSelecionado"] is int[])
                    {
                        return (int[])Session["valorPeriodoSelecionado"];
                    }

                    return null;
                }
                finally
                {
                    Session["valorPeriodoSelecionado"] = null;
                }
            }
        }

        /// <summary>
        /// Inicializa o user control, setando a visibilidade os botões
        /// conforme a página inicial.
        /// </summary>
        /// <param name="TelaInicial">Tela inicial a ser carregada.</param>
        /// <param name="fav">Formato de avaliação</param>
        /// <param name="tipo">Tipo de turma</param>
        public void Inicializar
        (
            PaginaGestao TelaInicial
            , ACA_FormatoAvaliacao fav
            , TUR_TurmaTipo tipo
            , TUR_TurmaDisciplina entTurmaDisciplina
        )
        {
            lblInfoLancamentoFrequenciaMensal.Text = UtilBO.GetErroMessage("Para lançar a frequência mensal dessa turma, acessar a frequência mensal das turmas regulares dos alunos.", UtilBO.TipoMensagem.Informacao);
            divInfoLancamentoFrequenciaMensal.Visible = (tipo == TUR_TurmaTipo.EletivaAluno);

            btnEfetivacao.Visible = (TelaInicial != PaginaGestao.EfetivacaoNotas);
            btnLancamentoFrequencia.Visible = 
                (TelaInicial != PaginaGestao.Lancamento_Frequencia) 
                && (fav.fav_tipoLancamentoFrequencia != 
                    Convert.ToByte(ACA_FormatoAvaliacaoTipoLancamentoFrequencia.Mensal))
                // Se a disciplina for do tipo Complementação de regência, não exibe o botão frequência.
                && (entTurmaDisciplina == null 
                    || entTurmaDisciplina.tud_tipo != (byte)TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia);
            btnLancamentoAvaliacao.Visible = (TelaInicial != PaginaGestao.Lancamento_Avaliacoes);
            btnLancamentoFrequenciaMensal.Visible = (TelaInicial != PaginaGestao.Lancamento_FrequenciaMensal) &&
                                                    (tipo != TUR_TurmaTipo.EletivaAluno) &&
                                                     ((fav.fav_tipoLancamentoFrequencia == Convert.ToByte(ACA_FormatoAvaliacaoTipoLancamentoFrequencia.Mensal) ||
                                                     fav.fav_tipoLancamentoFrequencia == Convert.ToByte(ACA_FormatoAvaliacaoTipoLancamentoFrequencia.AulasPlanejadasMensal)) &&
                                                     __SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.Individual);
        }

        /// <summary>
        /// Redireciona a página para o caminho especificado.
        /// </summary>
        /// <param name="Caminho">Caminho de redirecionamento.</param>
        public void RedirecionaPagina(string Caminho)
        {
            Response.Redirect(Caminho, false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// Verifica se o botão de frequência será exibido ou não
        /// Se a turma existir disciplina principal, só exibe o botão de frequência para essa disciplina
        /// </summary>
        /// <param name="tud_id">Id da disciplina da turma</param>
        /// <param name="TelaInicial">Tela inicial a ser carregada.</param>
        /// <param name="entityTurma">The entity turma.</param>
        /// <param name="entityFormato">The entity formato.</param>
        public void ExibirFrequencia(long tud_id, PaginaGestao TelaInicial, TUR_Turma entityTurma, ACA_FormatoAvaliacao entityFormato)
        {
            bool BtnVisible = (TelaInicial != PaginaGestao.Lancamento_Frequencia) && (entityFormato.fav_tipoLancamentoFrequencia != Convert.ToByte(ACA_FormatoAvaliacaoTipoLancamentoFrequencia.Mensal));

            if (tud_id > 0)
            {
                List<TUR_TurmaDisciplina> lista = TUR_TurmaDisciplinaBO.GetSelectBy_Turma(entityTurma.tur_id, null, GestaoEscolarUtilBO.MinutosCacheLongo);
                
                // Se a turma possui disciplina principal e tem lançamento em conjunto.
                if (lista.Exists(p => p.tud_tipo == (byte)TurmaDisciplinaTipo.DisciplinaPrincipal) &&
                    (!(entityFormato.fav_planejamentoAulasNotasConjunto && entityTurma.tur_docenteEspecialista)))
                {
                    // Esconde o botão de lançamento de frequência caso a disciplina não seja a principal.
                    btnLancamentoFrequencia.Visible =
                        (lista.Find(p => p.tud_tipo == (byte)TurmaDisciplinaTipo.DisciplinaPrincipal).tud_id == tud_id) &&
                        BtnVisible;
                }
                else if (lista.Exists
                    (p => 
                        p.tud_id == tud_id 
                        && p.tud_tipo == (byte)TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia
                    ))
                {
                    // Se for a disciplina do tipo Complementação de regência, não exibe o botão de frequência.
                    btnLancamentoFrequencia.Visible = false;
                }
                else
                {
                    btnLancamentoFrequencia.Visible = BtnVisible;
                }
            }
            else
            {
                btnLancamentoFrequencia.Visible = BtnVisible;
            }
        }

        #endregion

        #region Eventos

        protected void btnEfetivacao_Click(object sender, EventArgs e)
        {
            try
            {
                SetaPeriodoSelecionado();
                Session["tur_idEfetivacao"] = _VS_tur_id;
                Session["tud_idEfetivacao"] = _VS_tud_id > 0 ? _VS_tud_id : -1;
                Session["fav_idEfetivacao"] = _VS_fav_id;
                Session["ava_idEfetivacao"] = -1;
                Session["URL_Retorno_Efetivacao"] = Convert.ToByte(URL_Retorno_Efetivacao.EfetivacaoBusca);
                RedirecionaPagina("~/Classe/Efetivacao/Cadastro.aspx");
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);

                __SessionWEB.PostMessages = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                Response.Redirect("~/Classe/Efetivacao/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        protected void btnLancamentoAvaliacao_Click(object sender, EventArgs e)
        {
            try
            {
                SetaPeriodoSelecionado();
                Session["tur_idAvaliacao"] = _VS_tur_id.ToString();
                Session["fav_idAvaliacao"] = _VS_fav_id.ToString();
                Session["tud_idEfetivacao"] = _VS_tud_id;

                RedirecionaPagina("~/Classe/LancamentoAvaliacoes/Cadastro.aspx");
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);

                __SessionWEB.PostMessages = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                Response.Redirect("~/Classe/LancamentoAvaliacoes/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        protected void btnLancamentoFrequencia_Click(object sender, EventArgs e)
        {
            try
            {
                SetaPeriodoSelecionado();
                Session["tud_idEfetivacao"] = _VS_tud_id;
                ACA_FormatoAvaliacao fav = new ACA_FormatoAvaliacao
                {
                    fav_id = _VS_fav_id
                };

                ACA_FormatoAvaliacaoBO.GetEntity(fav);

                if (fav.fav_tipoLancamentoFrequencia == Convert.ToByte(ACA_FormatoAvaliacaoTipoLancamentoFrequencia.AulasPlanejadas) ||
                    fav.fav_tipoLancamentoFrequencia == Convert.ToByte(ACA_FormatoAvaliacaoTipoLancamentoFrequencia.AulasPlanejadasMensal) ||
                    fav.fav_tipoLancamentoFrequencia == Convert.ToByte(ACA_FormatoAvaliacaoTipoLancamentoFrequencia.AulasDadas))
                {
                    Session["tur_idFrequencia"] = _VS_tur_id.ToString();
                    RedirecionaPagina("~/Classe/LancamentoFrequencia/Cadastro.aspx");
                }
                else if (fav.fav_tipoLancamentoFrequencia == Convert.ToByte(ACA_FormatoAvaliacaoTipoLancamentoFrequencia.Periodo))
                {
                    Session["tur_idFrequencia"] = _VS_tur_id.ToString();
                    RedirecionaPagina("~/Classe/LancamentoFrequencia/Periodo.aspx");
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);

                __SessionWEB.PostMessages = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                Response.Redirect("~/Classe/LancamentoFrequencia/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        protected void btnLancamentoFrequenciaMensal_Click(object sender, EventArgs e)
        {
            try
            {
                SetaPeriodoSelecionado();
                ACA_FormatoAvaliacao fav = new ACA_FormatoAvaliacao
                {
                    fav_id = _VS_fav_id
                };

                ACA_FormatoAvaliacaoBO.GetEntity(fav);

                if (fav.fav_tipoLancamentoFrequencia == Convert.ToByte(ACA_FormatoAvaliacaoTipoLancamentoFrequencia.Mensal) ||
                    fav.fav_tipoLancamentoFrequencia == Convert.ToByte(ACA_FormatoAvaliacaoTipoLancamentoFrequencia.AulasPlanejadasMensal))
                {
                    Session["tur_idFrequencia"] = _VS_tur_id.ToString();
                    RedirecionaPagina("~/Classe/LancamentoFrequencia/LancamentoMensal.aspx");
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);

                __SessionWEB.PostMessages = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                Response.Redirect("~/Classe/LancamentoFrequencia/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        #endregion
    }
}