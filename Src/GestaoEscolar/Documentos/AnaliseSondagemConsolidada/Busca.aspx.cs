using System;
using System.Web;
using System.Web.UI;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Security.Cryptography;
using ReportNameGestaoAcademicaDocumentosDocente = MSTech.GestaoEscolar.BLL.ReportNameGestaoAcademicaDocumentosDocente;
using CFG_RelatorioBO = MSTech.GestaoEscolar.BLL.CFG_RelatorioBO;
using System.Collections.Generic;
using System.Linq;
using MSTech.Validation.Exceptions;

namespace GestaoEscolar.Documentos.AnaliseSondagemConsolidada
{
    public partial class Busca : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando se altera o calendário quando altera os combos de UA
        /// </summary>
        public bool _VS_alteraCalendario
        {
            get
            {
                if (ViewState["_VS_alteraCalendario"] != null)
                    return Convert.ToBoolean(ViewState["_VS_alteraCalendario"]);
                return true;
            }
            set
            {
                ViewState["_VS_alteraCalendario"] = value;
            }
        }

        /// <summary>
        /// Propridade usada para mostrar/esconder o combo de escola do UserControl UCComboUAEscola
        /// </summary>
        public bool _VS_AnosAnteriores
        {
            get
            {
                if (ViewState["_VS_AnosAnteriores"] != null)
                    return Convert.ToBoolean(ViewState["_VS_AnosAnteriores"]);
                return false;
            }
            set
            {
                ViewState["_VS_AnosAnteriores"] = value;
            }
        }
        
        /// <summary>
        /// Array de níveis de ensino que serão carregados no combo de curso.
        /// </summary>
        public int[] VS_FiltroTipoNivelEnsino
        {
            get
            {
                if (ViewState["VS_FiltroTipoNivelEnsino"] == null)
                {
                    ViewState["VS_FiltroTipoNivelEnsino"] = ACA_TipoNivelEnsinoBO.SelecionaTipoNivelEnsino().Select().Select(p => Convert.ToInt32(p["tne_id"])).ToArray();
                }

                return (int[])(ViewState["VS_FiltroTipoNivelEnsino"]);
            }

            set
            {
                ViewState["VS_FiltroTipoNivelEnsino"] = value;
            }
        }

        /// <summary>
        /// Propridade usada para mostrar/esconder o combo de escola do UserControl UCComboUAEscola
        /// </summary>
        public bool _VS_MostarComboEscola
        {
            get
            {
                if (ViewState["_VS_mostrarComboEscola"] != null)
                    return Convert.ToBoolean(ViewState["_VS_mostrarComboEscola"]);
                return true;
            }
            set
            {
                ViewState["_VS_mostrarComboEscola"] = value;
            }
        }

        /// <summary>
        /// Propridade usada para mostrar/esconder o combo de tipo de ciclo.
        /// </summary>
        public bool _VS_MostarComboTipoCiclo
        {
            get
            {
                if (ViewState["_VS_MostarComboTipoCiclo"] != null)
                    return Convert.ToBoolean(ViewState["_VS_MostarComboTipoCiclo"]);
                return false;
            }
            set
            {
                ViewState["_VS_MostarComboTipoCiclo"] = value;
            }
        }
        
        /// <summary>
        /// Propriedade que define se apenas as turmas normais devem ser carregadas no combo de turmas
        /// </summary>
        public bool _VS_CarregarApenasTurmasNormais
        {
            get
            {
                if (ViewState["_VS_CarregarApenasTurmasNormais"] != null)
                    return Convert.ToBoolean(ViewState["_VS_CarregarApenasTurmasNormais"]);
                return false;
            }
            set
            {
                ViewState["_VS_CarregarApenasTurmasNormais"] = value;
            }
        }

        /// <summary>
        /// Propridade usada para mostrar/esconder as turmas eletivas do combo de turma
        /// </summary>
        public bool _VS_MostraTurmasEletivas
        {
            get
            {
                if (ViewState["_VS_MostraTurmasEletivas"] != null)
                    return Convert.ToBoolean(ViewState["_VS_MostraTurmasEletivas"]);
                return false;
            }
            set
            {
                ViewState["_VS_MostraTurmasEletivas"] = value;
            }
        }

        private bool buscaEscolasPorVinculoColaboradorDocente = false;
        /// <summary>
        /// Indica se irá buscar as escolas pelo vínculo do docente no cargo (ColaboradorCargo).
        /// Caso passe false, irá buscar as escolas onde o docente possua pelo menos uma turma
        /// com atribuição ativa (independente da posição).
        /// </summary>
        public bool BuscaEscolasPorVinculoColaboradorDocente
        {
            get
            {
                return buscaEscolasPorVinculoColaboradorDocente;
            }
            set
            {
                buscaEscolasPorVinculoColaboradorDocente = value;
            }
        }
        
        #endregion Propriedades

        #region Delegates

        /// <summary>
        /// Verifica alteracao do index do combo UA Escola e trata o combo Escola e Curso curriculo.
        /// </summary>
        protected void UCComboUAEscola_IndexChangedUA()
        {
            UCComboUAEscola.MostraApenasAtivas = true;

            UCComboUAEscola.EnableEscolas = (UCComboUAEscola.Uad_ID != Guid.Empty || !UCComboUAEscola.DdlUA.Visible);
            if (UCComboUAEscola.DdlUA.Visible)
                UCComboUAEscola.SelectedIndexEscolas = 0;

            UCComboUAEscola_IndexChangedUnidadeEscola();
        }

        /// <summary>
        /// Verifica alteracao do index do combo Escola e trata o combo calendario
        /// </summary>
        public void UCComboUAEscola_IndexChangedUnidadeEscola()
        {
            try
            {
                UCCCalendario.Visible = false;
                if (_VS_alteraCalendario)
                {
                    UCCCalendario.Valor = -1;
                    UCCCalendario.PermiteEditar = false;
                }
                else
                {
                    if (!_VS_AnosAnteriores)
                    {
                        UCCCalendario.CarregarCalendarioAnualAnoAtual();
                    }
                    else
                    {
                        UCCCalendario.CarregarCalendarioAnual();
                    }
                }

                if (_VS_alteraCalendario && UCComboUAEscola.Uad_ID != Guid.Empty)
                {
                    UCCCalendario.CarregarCalendarioAnualAnoAtual();
                    UCCCalendario.PermiteEditar = true;
                }

                if (UCCCalendario.QuantidadeItensCombo > 2 && UCCCalendario.PermiteEditar)
                    UCCCalendario.Visible = true;

                UCCCalendario_IndexChanged();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Verifica alteracao do index do combo calendario e trata o combo cursocurriculo
        /// </summary>
        public void UCCCalendario_IndexChanged()
        {
            try
            {
                UCCCursoCurriculo.Valor = new[] { -1, -1 };
                UCCCursoCurriculo.PermiteEditar = false;

                if (UCCCalendario.Valor > 0)
                {
                    UCCCursoCurriculo.CarregarPorEscolaCalendarioSituacaoCursoNivelEnsino(UCComboUAEscola.Esc_ID, UCComboUAEscola.Uni_ID, UCCCalendario.Valor, 0, VS_FiltroTipoNivelEnsino);

                    UCCCursoCurriculo.SetarFoco();
                    UCCCursoCurriculo.PermiteEditar = true;
                }

                UCCCursoCurriculo_IndexChanged();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Verifica alteracao do index do combo cursocurriculo e trata o combo curriculoperiodo
        /// </summary>
        public void UCCCursoCurriculo_IndexChanged()
        {
            try
            {
                UCComboCurriculoPeriodo.Valor = new[] { -1, -1, -1 };
                UCComboCurriculoPeriodo.PermiteEditar = false;
                UCComboTipoCiclo.Visible = false;
                UCComboTipoCiclo.Valor = -1;
                UCComboTipoCiclo.Enabled = false;

                bool carregarCurriculoPeriodo = false;
                if (UCCCursoCurriculo.Valor[0] > 0 && UCCCursoCurriculo.Valor[1] > 0)
                {
                    if (_VS_MostarComboTipoCiclo)
                    {
                        // Carrega o ciclo.
                        UCComboTipoCiclo.CarregarCicloPorCursoCurriculo(UCCCursoCurriculo.Valor[0], UCCCursoCurriculo.Valor[1]);
                        if (UCComboTipoCiclo.ddlCombo.Items.Count > 0)
                        {
                            UCComboTipoCiclo.Visible = true;
                            UCComboTipoCiclo.Enabled = true;
                            UCComboTipoCiclo.ddlCombo.Focus();

                            UCComboTipoCiclo_IndexChanged();
                        }
                        else
                        {
                            carregarCurriculoPeriodo = true;
                        }
                    }
                    else
                    {
                        carregarCurriculoPeriodo = true;
                    }

                    if (carregarCurriculoPeriodo)
                    {
                        UCComboCurriculoPeriodo._Load(UCCCursoCurriculo.Valor[0], UCCCursoCurriculo.Valor[1]);

                        UCComboCurriculoPeriodo.FocaCombo();
                        UCComboCurriculoPeriodo.PermiteEditar = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Evento change do combo de tipo de ciclo
        /// </summary>
        public void UCComboTipoCiclo_IndexChanged()
        {
            try
            {
                if (UCComboTipoCiclo.Valor > 0)
                {
                    UCComboCurriculoPeriodo.CarregarPorCursoCurriculoTipoCiclo(UCCCursoCurriculo.Valor[0], UCCCursoCurriculo.Valor[1], UCComboTipoCiclo.Valor);
                    UCComboCurriculoPeriodo.PermiteEditar = true;
                    UCComboCurriculoPeriodo.FocaCombo();
                }
                else
                {
                    UCComboCurriculoPeriodo.Valor = new[] { -1, -1, -1 };
                    UCComboCurriculoPeriodo.PermiteEditar = false;
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Delegates

        #region Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                UCComboUAEscola.IndexChangedUnidadeEscola += UCComboUAEscola_IndexChangedUnidadeEscola;
                UCComboUAEscola.IndexChangedUA += UCComboUAEscola_IndexChangedUA;
                UCCCursoCurriculo.IndexChanged += UCCCursoCurriculo_IndexChanged;
                UCCCalendario.IndexChanged += UCCCalendario_IndexChanged;
                UCComboTipoCiclo.IndexChanged += UCComboTipoCiclo_IndexChanged;

                ScriptManager sm = ScriptManager.GetCurrent(this);
                if (sm != null)
                {
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                }

                if (!IsPostBack)
                {
                    InicializarTela();
                    VerificaBusca();
                }
            }
            catch (Exception error)
            {
                ApplicationWEB._GravaErro(error);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
            finally
            {
                string message = __SessionWEB.PostMessages;
                if (!String.IsNullOrEmpty(message))
                    lblMessage.Text = message;
                // Inserção do título do relatório
                relTitulo.InnerText = Modulo.mod_nome;
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Carrega os campos de busca com a configuração padrão para uma nova busca
        /// </summary>
        public void InicializaCamposBusca()
        {
            //Carrega os campos
            if (!_VS_MostarComboEscola)
            {
                UCComboUAEscola.FiltroEscolasControladas = true;
                UCComboUAEscola.Inicializar();
            }

            UCComboUAEscola.ExibeComboEscola = false;
            UCComboUAEscola_IndexChangedUA();
        }
        
        /// <summary>
        /// Inicializa os componentes da tela
        /// </summary>
        private void InicializarTela()
        {
            _VS_AnosAnteriores = true;
            _VS_MostarComboEscola = false;
            UCCCursoCurriculo.Obrigatorio = UCComboCurriculoPeriodo.Obrigatorio = false;

            UCComboSondagem.Obrigatorio = true;
            UCComboSondagem.Carregar(true);

            InicializaCamposBusca();
        }

        /// <summary>
        /// O método gera o relatório de Análise de Sondagem abaixo da frequência
        /// </summary>
        private void GerarRelatorio()
        {
            try
            {
                string report, parametros;

                DateTime dataInicio = new DateTime();
                DateTime dataFim = new DateTime();

                if (string.IsNullOrEmpty(txtDataInicio.Text) || !DateTime.TryParse(txtDataInicio.Text, out dataInicio))
                    throw new ValidationException(GetGlobalResourceObject("Documentos", "AnaliseSondagem.Busca.DataInicioInvalida").ToString());

                if (dataInicio > DateTime.Today)
                    throw new ValidationException(GetGlobalResourceObject("Documentos", "AnaliseSondagem.Busca.DataInicioMaiorHoje").ToString());

                if (string.IsNullOrEmpty(txtDataFim.Text) || !DateTime.TryParse(txtDataFim.Text, out dataFim))
                    throw new ValidationException(GetGlobalResourceObject("Documentos", "AnaliseSondagem.Busca.DataFimInvalida").ToString());

                if (dataFim > DateTime.Today)
                    throw new ValidationException(GetGlobalResourceObject("Documentos", "AnaliseSondagem.Busca.DataFimMaiorHoje").ToString());

                if (dataInicio > dataFim)
                    throw new ValidationException(GetGlobalResourceObject("Documentos", "AnaliseSondagem.Busca.DataFimMenorInicio").ToString());

                SalvaBusca();

                report = ((int)MSTech.GestaoEscolar.BLL.ReportNameGestaoAcademica.AnaliseSondagemConsolidada).ToString();
                parametros = "uad_idSuperiorGestao=" + UCComboUAEscola.Uad_ID +
                             "&cal_id=" + UCCCalendario.Valor +
                             "&cal_ano=" + UCCCalendario.Cal_ano.ToString() +
                             "&cur_id=" + UCCCursoCurriculo.Valor[0] +
                             "&crr_id=" + UCCCursoCurriculo.Valor[1] +
                             "&crp_id=" + UCComboCurriculoPeriodo.Valor[2] +
                             "&snd_id=" + UCComboSondagem.Valor +
                             "&dataInicio=" + txtDataInicio.Text +
                             "&dataFim=" + txtDataFim.Text +
                             "&suprimirPercentual=" + chkSuprimirPercentual.Checked +
                             "&dre=" + UCComboUAEscola.TextoComboUA +
                             "&escola=" + (UCComboUAEscola.Esc_ID > 0 ? UCComboUAEscola.TextoComboEscola : "") +
                             "&logo=" + String.Concat(MSTech.GestaoEscolar.BLL.CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo).srr_pastaRelatorios.ToString()
                                                   , ApplicationWEB.LogoRelatorioSSRS) +
                             "&nomeMunicipio=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Municipio") +
                             "&nomeSecretaria=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Secretaria");

                CFG_RelatorioBO.CallReport("Relatorios", report, parametros, HttpContext.Current);
            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar gerar o relatório.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Método para salvar os filtros última busca realizada
        /// </summary>
        protected void SalvaBusca()
        {
            Dictionary<string, string> filtros = new Dictionary<string, string>();
            filtros.Add("uad_idSuperior", UCComboUAEscola.Uad_ID.ToString());
            filtros.Add("cur_id", UCCCursoCurriculo.Valor[0].ToString());
            filtros.Add("crr_id", UCCCursoCurriculo.Valor[1].ToString());
            filtros.Add("crp_id", UCComboCurriculoPeriodo.Valor[2].ToString());
            filtros.Add("cal_id", UCCCalendario.Valor.ToString());
            filtros.Add("snd_id", UCComboSondagem.Valor.ToString());
            filtros.Add("snd_dataIncio", Convert.ToDateTime(txtDataInicio.Text).ToString());
            filtros.Add("snd_dataFim", Convert.ToDateTime(txtDataFim.Text).ToString());
            filtros.Add("suprimirPercentual", chkSuprimirPercentual.Checked.ToString());

            __SessionWEB.BuscaRealizada = new BuscaGestao { PaginaBusca = PaginaGestao.RelatorioAnaliseSondagemConsolidada, Filtros = filtros };

        }

        /// <summary>
        /// Método carrega os filtros última busca realizada
        /// </summary>
        protected void VerificaBusca()
        {
            try
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.RelatorioAnaliseSondagemConsolidada)
                {
                    // Recuperar busca realizada e pesquisar automaticamente
                    string valor, valor2, valor3;
                    
                    UCComboUAEscola.Inicializar();
                    
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("uad_idSuperior", out valor);
                    if (!string.IsNullOrEmpty(valor))
                    {
                        UCComboUAEscola.Uad_ID = new Guid(valor);
                        UCComboUAEscola.CarregaEscolaPorUASuperiorSelecionada();

                        if (UCComboUAEscola.Uad_ID != Guid.Empty)
                        {
                            UCComboUAEscola.FocoEscolas = true;
                            UCComboUAEscola.PermiteAlterarCombos = true;
                        }
                    }

                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cal_id", out valor);
                    UCCCalendario.Valor = Convert.ToInt32(valor);
                    UCCCalendario_IndexChanged();

                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cur_id", out valor2);
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crr_id", out valor);
                    UCCCursoCurriculo.Valor = new[] { Convert.ToInt32(valor2), Convert.ToInt32(valor) };
                    UCCCursoCurriculo_IndexChanged();

                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crp_id", out valor);
                    UCComboCurriculoPeriodo.Valor = new[] { UCCCursoCurriculo.Valor[0], UCCCursoCurriculo.Valor[1], Convert.ToInt32(valor) };
                    
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("snd_id", out valor);
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("snd_dataIncio", out valor2);
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("snd_dataFim", out valor3);
                    UCComboSondagem.Valor = Convert.ToInt32(valor);
                    txtDataInicio.Text = valor2;
                    txtDataFim.Text = valor3;

                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("suprimirPercentual", out valor);
                    chkSuprimirPercentual.Checked = Convert.ToBoolean(valor);

                    updFiltros.Update();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Métodos

        #region Eventos

        protected void btnGerar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                GerarRelatorio();
            }
        }

        protected void btnLimparPesquisa_Click(object sender, EventArgs e)
        {
            // Limpa busca da sessão.
            __SessionWEB.BuscaRealizada = new BuscaGestao();
            Response.Redirect("Busca.aspx", false);
        }

        #endregion Eventos
    }
}