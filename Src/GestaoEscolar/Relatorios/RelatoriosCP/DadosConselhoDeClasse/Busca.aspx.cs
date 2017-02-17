using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Security.Cryptography;
using MSTech.Validation.Exceptions;

namespace GestaoEscolar.Relatorios.RelatoriosCP.DadosConselhoDeClasse
{
    public partial class Busca : MotherPageLogado
    {
        #region Page Life Cycle
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.Json));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmBtn));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsBuscaDocumentosAluno.js"));
            }

            UCComboCursoCurriculo1.IndexChanged += UCComboCursoCurriculo1_IndexChanged;
            UCComboCurriculoPeriodo1.IndexChanged += UCComboCurriculoPeriodo1__OnSelectedIndexChange;
            UCComboCalendario1.IndexChanged += UCComboCalendario1_IndexChanged;

            //_btnGerarRelatorio.Attributes.Add("onclick", String.Format("javascript: return ExtensaoDosFiltros('{0}');", __SessionWEB.__UsuarioWEB.Usuario.ent_id));
            //btnGerarRelatorioCima.Attributes.Add("onclick", String.Format("javascript: return ExtensaoDosFiltros('{0}');", __SessionWEB.__UsuarioWEB.Usuario.ent_id));

            if (!IsPostBack)
            {
                try
                {
                    string message = __SessionWEB.PostMessages;
                    if (!String.IsNullOrEmpty(message))
                    {
                        _lblMessage.Text = message;
                        __SessionWEB.PostMessages = String.Empty;
                    }

                    if (!__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                    {
                        uppPesquisa.Visible = false;
                        _lblMessage.Text = UtilBO.GetErroMessage("Você não possui permissão para acessar a página solicitada.", UtilBO.TipoMensagem.Alerta);
                    }
                    else
                    { 
                        // Inserção do título do relatório
                        lgdTitulo.InnerText = Modulo.mod_nome;

                        HabilitarFiltrosPadrao(false);

                        HabilitarFiltrosPadrao(true);

                        Inicializar();

                        // Carrega o nome referente ao parametro de matricula estadual.
                        string nomeMatriculaEstadual = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MATRICULA_ESTADUAL
                            , __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                        bool mostraMatriculaEstadual = !string.IsNullOrEmpty(nomeMatriculaEstadual);

                        UCCamposBuscaAluno1.MostrarMatriculaEstadual = mostraMatriculaEstadual;
                        UCCamposBuscaAluno1.TituloMatriculaEstadual = nomeMatriculaEstadual;
                    }

                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                }
            }
        }
        #endregion

        #region Eventos
        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                GerarRelatorio();
            }
        }

        /// <summary>
        /// O método gera o relatório de alunos abaixo da frequência
        /// </summary>
        private void GerarRelatorio()
        {
            string report, parametros = string.Empty;

            report = ((int)MSTech.GestaoEscolar.BLL.ReportNameGestaoAcademica.AlunoConselhoDeClasse).ToString();

            parametros = "uad_idSuperior=" + (UCComboUAEscola.Uad_ID == Guid.Empty ? string.Empty : UCComboUAEscola.Uad_ID.ToString()) +
                         "&esc_id=" + UCComboUAEscola.Esc_ID +
                         "&uni_id=" + UCComboUAEscola.Uni_ID +
                         "&cur_id=" + UCComboCursoCurriculo1.Valor[0] +
                         "&crr_id=" + UCComboCursoCurriculo1.Valor[1] +
                         "&crp_id=" + UCComboCurriculoPeriodo1.Valor[2] +
                         "&cal_id=" + UCComboCalendario1.Valor +
                         "&tur_id=" + UCComboTurma1.Valor[0] +
                         "&tipobusca=" + UCCamposBuscaAluno1.TipoBuscaNomeAluno +
                         "&pes_nome=" + (chkBuscaAvancada.Checked ? UCCamposBuscaAluno1.NomeAluno : string.Empty) +
                         "&alc_matricula=" + (chkBuscaAvancada.Checked ? UCCamposBuscaAluno1.MatriculaAluno : string.Empty) +
                         "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                         "&adm=" + (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao) +
                         "&usu_id=" + __SessionWEB.__UsuarioWEB.Usuario.usu_id +
                         "&gru_id=" + __SessionWEB.__UsuarioWEB.Grupo.gru_id +
                         "&emitirDocAnoAnt=false" +
                         "&trazMatriculaIgual=false" +
                         "&MostraCodigoEscola=" + ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                         "&nomeMunicipio=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoPaisagem.Municipio") +
                         "&nomeSecretaria=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoPaisagem.Secretaria") +
                         "&logo=" + String.Concat(MSTech.GestaoEscolar.BLL.CFG_ServidorRelatorioBO.CarregarCredencialServidorPorRelatorio(__SessionWEB.__UsuarioWEB.Usuario.ent_id, Convert.ToInt32(report)).srr_pastaRelatorios.ToString()
                                                , ApplicationWEB.LogoRelatorioSSRS) +
                         "&lblCodigoEOL=" + GetGlobalResourceObject("Reporting", "Reporting.AlunoConselhoDeClasse.lblCodigoEOL.label") +
                         "&cal_ano=" + UCComboCalendario1.Cal_ano.ToString() +
                         "&documentoOficial=true";

            SymmetricAlgorithm sa = new SymmetricAlgorithm(SymmetricAlgorithm.Tipo.TripleDES);
            MSTech.GestaoEscolar.BLL.CFG_RelatorioBO.SendParametersToReport(report, parametros);
            Response.Redirect(String.Format("~/Relatorios/Relatorio.aspx?dummy='{0}'", HttpUtility.UrlEncode(sa.Encrypt(report))), false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void chkBuscaAvancada_CheckedChanged(object sender, EventArgs e)
        {
            divBuscaAvancada.Visible = chkBuscaAvancada.Checked;
            UCComboCalendario1.Obrigatorio = UCComboTurma1.Obrigatorio = !chkBuscaAvancada.Checked;
            uppPesquisa.Update();
        }

        #endregion

        #region Delegates

        protected void UCComboUAEscola_IndexChangedUA()
        {
            if (UCComboUAEscola.Uad_ID == Guid.Empty)
                UCComboUAEscola.SelectedValueEscolas = new[] { -1, -1 };

            UCComboUAEscola_IndexChangedUnidadeEscola();
        }

        protected void UCComboUAEscola_IndexChangedUnidadeEscola()
        {
            try
            {
                UCComboCursoCurriculo1.Valor = new[] { -1, -1 };
                UCComboCursoCurriculo1.PermiteEditar = false;

                if (UCComboUAEscola.Esc_ID > 0 && UCComboUAEscola.Uni_ID > 0)
                {
                    UCComboCursoCurriculo1.CarregarPorEscola(UCComboUAEscola.Esc_ID,
                                                                            UCComboUAEscola.Uni_ID);
                    UCComboCursoCurriculo1.SetarFoco();
                    UCComboCursoCurriculo1.PermiteEditar = true;
                    
                }
                UCComboCursoCurriculo1_IndexChanged();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        private void UCComboCalendario1_IndexChanged()
        {
            try
            {
                UCCPeriodoCalendario.CarregarPorCalendario(-1);
                if (UCComboCalendario1.Valor > 0)
                {
                    UCCPeriodoCalendario.CarregarPorCalendario(UCComboCalendario1.Valor);
                    UCCPeriodoCalendario.PermiteEditar = true;
                    UCCPeriodoCalendario.Focus();
                }
                else
                {
                    UCCPeriodoCalendario.PermiteEditar = false;
                }

                // carrega combo turmas
                UCComboTurma1.Valor = new long[] { -1, -1, -1 };
                UCComboTurma1.PermiteEditar = false;

                if (UCComboCalendario1.Valor > 0)
                {
                    UCComboTurma1.CarregarPorEscolaCurriculoCalendario_TurmasNormais(UCComboUAEscola.Esc_ID, UCComboUAEscola.Uni_ID, UCComboCurriculoPeriodo1.Valor[0], UCComboCurriculoPeriodo1.Valor[1], UCComboCurriculoPeriodo1.Valor[2], UCComboCalendario1.Valor);
                    UCComboTurma1.PermiteEditar = true;
                    UCComboTurma1.Focus();
                }

            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a(s) turma(s) do período.", UtilBO.TipoMensagem.Erro);
            }
        }

        private void UCComboCurriculoPeriodo1__OnSelectedIndexChange()
        {
            try
            {
                 UCComboCalendario1.Valor = -1;

                 UCComboCalendario1.PermiteEditar = false;

                 if (UCComboCurriculoPeriodo1.Valor[0] > 0)
                 {
                    UCComboCalendario1.CarregarPorCurso(UCComboCursoCurriculo1.Valor[0]);
                    UCComboCalendario1.PermiteEditar = true;
                    UCComboCalendario1.SetarFoco();
                 }

                 UCComboCalendario1_IndexChanged();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o(s) calendários(s).", UtilBO.TipoMensagem.Erro);
            }
        }

        private void UCComboCursoCurriculo1_IndexChanged()
        {
            try
            {
                UCComboCurriculoPeriodo1.Valor = new[] { -1, -1, -1 };
                UCComboCurriculoPeriodo1.PermiteEditar = false;

                if (UCComboCursoCurriculo1.Valor[0] > 0)
                {
                   UCComboCurriculoPeriodo1.CarregarPorCursoCurriculo(UCComboCursoCurriculo1.Valor[0], UCComboCursoCurriculo1.Valor[1]);
                   UCComboCurriculoPeriodo1.PermiteEditar = true;
                   UCComboCurriculoPeriodo1.Focus();
                }

                UCComboCurriculoPeriodo1__OnSelectedIndexChange();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Delegates

        #region Métodos

        #region Filtros

        /// <summary>
        ///
        /// </summary>
        /// <param name="habilita"></param>
        protected void HabilitarFiltrosPadrao(bool habilita)
        {
            UCComboUAEscola.Visible =
            UCComboCursoCurriculo1.Visible =
            UCComboCurriculoPeriodo1.Visible =
            UCComboCalendario1.Visible =
            UCComboTurma1.Visible =
            UCCamposBuscaAluno1.Visible =
            btnGerarRelatorio.Visible = habilita;

            HabilitarValidacao(false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="habilita"></param>
        protected void HabilitarValidacao(bool habilita)
        {
            UCComboUAEscola.ObrigatorioUA =
            UCComboUAEscola.ObrigatorioEscola =
            UCComboCursoCurriculo1.Obrigatorio =
            UCComboCurriculoPeriodo1.Obrigatorio =
            UCComboCalendario1.Obrigatorio =
            UCComboTurma1.Obrigatorio = habilita;
        }

        protected void Inicializar()
        {
            UCCamposObrigatorios.Visible = UCComboUAEscola.ObrigatorioEscola = UCComboUAEscola.ObrigatorioUA =
                        UCComboCursoCurriculo1.Obrigatorio = UCComboCurriculoPeriodo1.Obrigatorio =
                            UCComboCalendario1.Obrigatorio = UCCPeriodoCalendario.Obrigatorio = 
                            UCComboTurma1.Obrigatorio = true;
            UCCPeriodoCalendario.SelecionaPeriodoAtualAoCarregar = true;

            UCComboUAEscola.Inicializar();

            if (UCComboUAEscola.VisibleUA)
                UCComboUAEscola_IndexChangedUA();

            UCCamposBuscaAluno1.VisibleDataNascimento = UCCamposBuscaAluno1.VisibleMatriculaEstadual = UCCamposBuscaAluno1.VisibleNomeMae = false;
        }

        #endregion Filtros
        
        #endregion Métodos
    }
}