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

namespace GestaoEscolar.Documentos.AlunosPendenciaEfetivacao
{
    public partial class Busca : MotherPageLogado
    {
        #region Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ScriptManager sm = ScriptManager.GetCurrent(this);
                if (sm != null)
                {
                    sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                }

                if (!IsPostBack)
                {
                    InicializarTela();
                    CarregarBusca();
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
                pnlBusca.GroupingText = Modulo.mod_nome;
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa os componentes da tela
        /// </summary>
        private void InicializarTela()
        {
            //Se for usuário administrador ou gestor permite gerar o relatório sem filtrar por escola
            if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao ||
                __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Gestao)
            {
                UCBuscaDocenteTurma._VS_PermiteSemEscola = true;
                UCBuscaDocenteTurma.ComboEscola.ObrigatorioEscola = false;
            }

            UCBuscaDocenteTurma._VS_AnosAnteriores = true;
            UCBuscaDocenteTurma._VS_MostarComboEscola = false;
            UCBuscaDocenteTurma.ComboTurma.Obrigatorio = UCBuscaDocenteTurma.ComboCursoCurriculo.Obrigatorio =
                UCBuscaDocenteTurma.ComboCurriculoPeriodo.Obrigatorio = false;

            UCBuscaDocenteTurma.InicializaCamposBusca();

            UCBuscaDocenteTurma.ComboEscola.FocusUA();
        }

        /// <summary>
        /// O método gera o relatório de alunos abaixo da frequência
        /// </summary>
        private void GerarRelatorio()
        {
            string report, parametros;

            report = ((int)ReportNameGestaoAcademicaDocumentosDocente.DocDctAlunosPendenciaEfetivacao).ToString();
            parametros = "uad_idSuperiorGestao=" + UCBuscaDocenteTurma.ComboEscola.Uad_ID +
                         "&esc_id=" + UCBuscaDocenteTurma.ComboEscola.Esc_ID +
                         "&uni_id=" + UCBuscaDocenteTurma.ComboEscola.Uni_ID +
                         "&cal_id=" + UCBuscaDocenteTurma.ComboCalendario.Valor +
						 "&cal_ano=" + UCBuscaDocenteTurma.ComboCalendario.Cal_ano.ToString() +
                         "&cur_id=" + UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[0] +
                         "&crr_id=" + UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[1] +
                         "&crp_id=" + UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[2] +
                         "&tur_id=" + UCBuscaDocenteTurma.ComboTurma.Valor[0] +
                         "&ent_id=" + __SessionWEB.__UsuarioWEB.Usuario.ent_id +
                         "&doc_id=" + __SessionWEB.__UsuarioWEB.Docente.doc_id +
                         "&tud_ids=" + "" +
                         "&tev_EfetivacaoNotas=" + ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                         "&tev_EfetivacaoFinal=" + ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_FINAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                         "&nomeParecerConclusivo=" + GetGlobalResourceObject("Mensagens", "MSG_RESULTADOEFETIVACAO") +
                         "&nomePeriodoCalendario=" + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                         "&nomeComponenteCurricular=" + GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") +
                         "&mostraCodigoEscola=" + ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, __SessionWEB.__UsuarioWEB.Usuario.ent_id) +
                         "&nomeMunicipio=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Municipio") +
                         "&nomeSecretaria=" + GetGlobalResourceObject("Reporting", "Reporting.DocDctSubCabecalhoRetrato.Secretaria") +
                         "&logo=" + String.Concat(MSTech.GestaoEscolar.BLL.CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo).srr_pastaRelatorios.ToString()
                                 , ApplicationWEB.LogoRelatorioSSRS);

            CFG_RelatorioBO.CallReport("Relatorios", report, parametros, HttpContext.Current);
        }

        /// <summary>
        /// Método para salvar os filtros última busca realizada
        /// </summary>
        protected void SalvaBusca()
        {
            Dictionary<string, string> filtros = new Dictionary<string, string>();
            filtros.Add("uad_idSuperior", UCBuscaDocenteTurma.ComboEscola.Uad_ID.ToString());
            filtros.Add("esc_id", UCBuscaDocenteTurma.ComboEscola.Esc_ID.ToString());
            filtros.Add("uni_id", UCBuscaDocenteTurma.ComboEscola.Uni_ID.ToString());
            filtros.Add("cur_id", UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[0].ToString());
            filtros.Add("crr_id", UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[1].ToString());
            filtros.Add("crp_id", UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor[2].ToString());
            filtros.Add("cal_id", UCBuscaDocenteTurma.ComboCalendario.Valor.ToString());
            filtros.Add("tur_id", UCBuscaDocenteTurma.ComboTurma.Valor[0].ToString());
            filtros.Add("crp_idTurma", UCBuscaDocenteTurma.ComboTurma.Valor[1].ToString());
            filtros.Add("ttn_id", UCBuscaDocenteTurma.ComboTurma.Valor[2].ToString());


            __SessionWEB.BuscaRealizada = new BuscaGestao { PaginaBusca = PaginaGestao.AlunosPendenciaEfetivacao, Filtros = filtros };

        }

        /// <summary>
        /// Método carrega os filtros última busca realizada
        /// </summary>
        protected void CarregarBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.AlunosPendenciaEfetivacao)
            {
                // Recuperar busca realizada e pesquisar automaticamente
                string valor, valor2, valor3;

                long doc_id = -1;
                UCBuscaDocenteTurma.ComboEscola.Inicializar();

                if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual)
                {
                    doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;
                    UCBuscaDocenteTurma.ComboEscola.InicializarVisaoIndividual(doc_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                    string esc_id;
                    string uni_id;

                    if ((__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out esc_id)) &&
                        (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out uni_id)))
                    {
                        UCBuscaDocenteTurma.ComboEscola.SelectedValueEscolas = new[] { Convert.ToInt32(esc_id), Convert.ToInt32(uni_id) };
                        UCBuscaDocenteTurma.UCComboUAEscola_IndexChangedUnidadeEscola();
                    }
                }
                else
                {
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("uad_idSuperior", out valor);
                    if (!string.IsNullOrEmpty(valor))
                    {
                        UCBuscaDocenteTurma.ComboEscola.Uad_ID = new Guid(valor);
                        UCBuscaDocenteTurma.ComboEscola.CarregaEscolaPorUASuperiorSelecionada();

                        if (UCBuscaDocenteTurma.ComboEscola.Uad_ID != Guid.Empty)
                        {
                            UCBuscaDocenteTurma.ComboEscola.FocoEscolas = true;
                            UCBuscaDocenteTurma.ComboEscola.PermiteAlterarCombos = true;
                        }
                        string esc_id;
                        string uni_id;

                        if ((__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out esc_id)) &&
                            (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out uni_id)))
                        {
                            UCBuscaDocenteTurma.ComboEscola.SelectedValueEscolas = new[] { Convert.ToInt32(esc_id), Convert.ToInt32(uni_id) };
                            UCBuscaDocenteTurma.UCComboUAEscola_IndexChangedUnidadeEscola();
                        }
                    }
                }

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cal_id", out valor);
                UCBuscaDocenteTurma.ComboCalendario.Valor = Convert.ToInt32(valor);
                UCBuscaDocenteTurma.UCCCalendario_IndexChanged();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cur_id", out valor2);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crr_id", out valor);
                UCBuscaDocenteTurma.ComboCursoCurriculo.Valor = new[] { Convert.ToInt32(valor2), Convert.ToInt32(valor) };
                UCBuscaDocenteTurma.UCCCursoCurriculo_IndexChanged();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crp_id", out valor);
                if (doc_id <= 0)
                    UCBuscaDocenteTurma.ComboCurriculoPeriodo.Valor = new[] { UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[0], UCBuscaDocenteTurma.ComboCursoCurriculo.Valor[1], Convert.ToInt32(valor) };
                else
                    UCBuscaDocenteTurma._VS_doc_id = doc_id;
                UCBuscaDocenteTurma.UCComboCurriculoPeriodo__OnSelectedIndexChange();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tur_id", out valor);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("ttn_id", out valor2);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crp_idTurma", out valor3);
                UCBuscaDocenteTurma.ComboTurma.Valor = new[] { Convert.ToInt64(valor), Convert.ToInt64(valor3), Convert.ToInt64(valor2) };


                updFiltros.Update();
            }
        }

        #endregion Métodos

        #region Eventos

        protected void btnGerar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SalvaBusca();
                GerarRelatorio();
            }
        }

        #endregion Eventos

    }
}