using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System.Web;
using System.Threading;

namespace GestaoEscolar.Academico.RecursosHumanos.AtribuicaoDocentes
{
    public partial class Busca : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// retorna os dados do docente selecionado (tanto no acesso individual quanto normal)
        /// </summary>
        private long[] ComboDocenteValor
        {
            get
            {
                if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual)
                    return UCComboDocente2.Valor;
                else
                {
                    long doc_id, col_id, crg_id, coc_id;
                    doc_id = col_id = coc_id = crg_id = -1;

                    string[] s = hdnDocente.Value.Split(';');
                    if (s.Length > 0)
                        doc_id = Convert.ToInt64(s[0] != "" ? s[0] : "-1");

                    if (s.Length > 1)
                        col_id = Convert.ToInt64(s[1] != "" ? s[1] : "-1");

                    if (s.Length > 2)
                        crg_id = Convert.ToInt64(s[2] != "" ? s[2] : "-1");

                    if (s.Length > 3)
                        coc_id = Convert.ToInt64(s[3] != "" ? s[3] : "-1");

                    return new long[] { doc_id, col_id, crg_id, coc_id };
                }
            }
            set
            {
                if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual)
                    UCComboDocente2.Valor = value;
                else
                {
                    string s = "";
                    if (value.Length > 0)
                        s = value[0].ToString();

                    if (value.Length > 1)
                        s += ";" + value[1];

                    if (value.Length > 2)
                        s += ";" + value[2];

                    if (value.Length > 3)
                        s += ";" + value[3];

                    if (string.IsNullOrEmpty(s))
                        s = "-1";

                    hdnDocente.Value = s;
                }
            }
        }

        /// <summary>
        /// retorna o esc_id da escola selecionada (tanto no acesso individual quanto normal)
        /// </summary>
        private int esc_id
        {
            get
            {
                //if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual)
                //    return Convert.ToInt32(ddlEscola.SelectedValue.Split(';')[0]);
                //else
                return UCComboUAEscola1.Esc_ID;
            }
        }

        /// <summary>
        /// retorna o uni_id da escola selecionada (tanto no acesso individual quanto normal)
        /// </summary>
        private int uni_id
        {
            get
            {
                //if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual)
                //    return Convert.ToInt32(ddlEscola.SelectedValue.Split(';')[1]);
                //else
                return UCComboUAEscola1.Uni_ID;
            }
        }

        /// <summary>
        /// retorna o uad_id da escola selecionada (tanto no acesso individual quanto normal)
        /// </summary>
        private Guid uad_id
        {
            get
            {
                //if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual)
                //    return new Guid(ddlEscola.SelectedValue.Split(';')[2]);
                //else
                return UCComboUAEscola1.Uad_ID;
            }
        }

        /// <summary>
        /// Retorna se o usuario logado tem permissao para visualizar os botoes de salvar
        /// </summary>
        private bool usuarioPermissao
        {
            get
            {
                return __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar || __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
            }
        }

        /// <summary>
        /// Guarda o sortExpression da coluna ordenada
        /// </summary>
        private string VS_Ordenacao
        {
            get
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.AtribuicaoDocentes)
                {
                    Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;
                    string valor;
                    if (filtros.TryGetValue("VS_Ordenacao", out valor))
                    {
                        return valor;
                    }
                }

                return "";
            }
        }

        /// <summary>
        /// Guarda o sortExpression da coluna ordenada
        /// </summary>
        private SortDirection VS_SortDirection
        {
            get
            {
                if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.AtribuicaoDocentes)
                {
                    Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;
                    string valor;
                    if (filtros.TryGetValue("VS_SortDirection", out valor))
                    {
                        return (SortDirection)Enum.Parse(typeof(SortDirection), valor);
                    }
                }

                return SortDirection.Ascending;
            }
        }

        /// <summary>
        /// ViewState que armazena o ID da turma disciplina compartilhada,
        /// cujo docente selecionado é o titular.
        /// </summary>
        private Int64 VS_TudId_DocenciaCompartilhada
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_TudId_DocenciaCompartilhada"] ?? "-1");
            }

            set
            {
                ViewState["VS_TudId_DocenciaCompartilhada"] = value;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de cal_ano
        /// </summary>
        protected int VS_cal_ano
        {
            get
            {
                if (ViewState["VS_cal_ano"] == null)
                {
                    ViewState["VS_cal_ano"] = ACA_CalendarioAnualBO.SelecionaPorTurma(UCCTurma1.Valor[0]).cal_ano;
                }
                return Convert.ToInt32(ViewState["VS_cal_ano"]);
            }
        }

        /// <summary>
        /// Retorna se a nova regra de docencia compartilhada ja esta valendo,
        /// turma de calendário anterior à 2015 não deve aplicar essa nova regra.
        /// </summary>
        private bool aplicarNovaRegraDocenciaCompartilhada
        {
            get
            {
                return VS_cal_ano >= 2015;
            }
        }

        /// <summary>
        /// Retorna lista com o tipos de docente por posição.
        /// </summary>
        private List<ACA_TipoDocente> TipoDocente
        {
            get
            {
                return ACA_TipoDocenteBO.SelecionaAtivos();
            }
        }

        /// <summary>
        /// Retorna campo esc_terceirizada da escola selecionada no combo
        /// </summary>
        private bool esc_terceirizada
        {
            get
            {
                if (esc_id > 0)
                {
                    return ESC_EscolaBO.GetEntity(new ESC_Escola
                    {
                        esc_id = esc_id,
                        uad_idSuperior = uad_id,
                        ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id
                    }).esc_terceirizada;
                }
                else
                {
                    return false;
                }
            }
        }

        #endregion Propriedades

        #region Constantes

        public const string validationGroup = "vgConsultaTurmas";
        private const int indexColumnTitular = 3;
        private const int indexColumnSegundoTitular = 4;
        private const int indexColumnProjeto = 5;
        private const int indexColumnCompartilhada = 6;
        private const int indexColumnSubstituto = 7;
        private const int indexColumnVigenciaInicio = 8;
        private const int indexColumnVigenciaFim = 9;

        #endregion Constantes

        #region Delegates

        public void UCComboUAEscola1_IndexChanged()
        {
            UCCCalendario1.Valor = -1;
            UCCCalendario1.PermiteEditar = false;
            UCCCursoCurriculo1.Valor = new[] { -1, -1 };
            UCCCursoCurriculo1.PermiteEditar = false;
            UCCCurriculoPeriodo1.Valor = new[] { -1, -1, -1 };
            UCCCurriculoPeriodo1.PermiteEditar = false;
            UCCTurma1.Valor = new long[] { -1, -1, -1 };
            UCCTurma1.PermiteEditar = false;

            if (__SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.Individual)
            {
                ComboDocenteValor = new long[] { -1, -1, -1, -1 };
                txtNomeDocente.Text = string.Empty;
                btnBuscaDocente.Enabled = false;
            }
            else
            {
                UCComboDocente2._Combo.SelectedIndex = 0;
                UCComboDocente2.PermiteEditar = false;

                UCCCalendario1.Visible = false;
            }

            fdsResultado.Visible = false;
            lblInfoDocenciaCompartilhada.Visible = false;
            _lblMsgRegencia.Visible = false;

            if (UCComboUAEscola1.Esc_ID != -1)
            {
                if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual)
                {
                    UCComboDocente2._LoadBy_Docente_EscolaCargo(UCComboUAEscola1.Esc_ID, __SessionWEB.__UsuarioWEB.Docente.doc_id);

                    /**** Seleciona a primeira posição do docente, pois ele tem 2 atribuições na escola e aparece 2 vzs no combo.****/
                    if (UCComboDocente2._Combo.Items.Count > 2)
                    {
                        UCComboDocente2.PermiteEditar = true;
                        UCComboDocente2._Combo.SelectedIndex = UCComboDocente2._Combo.Items.Count > 2 ? 1 : 0;
                    }
                    else
                    {
                        UCComboDocente2.PermiteEditar = false;
                        UCComboDocente2._Combo.SelectedIndex = UCComboDocente2._Combo.Items.Count > 1 ? 1 : 0;
                    }
                }
                else
                {
                    btnBuscaDocente.Enabled = true;
                    UCBuscaDocenteEscola1.IdEscola = UCComboUAEscola1.Esc_ID;
                }

                UCCCalendario1.CarregarPorEscola(UCComboUAEscola1.Esc_ID);
                UCCCalendario1.PermiteEditar = true;
                if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual && __SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                {
                    UCCCalendario1.Visible = (UCCCalendario1.QuantidadeItensCombo > 1 && !UCCCalendario1.MostrarMensagemSelecione) || (UCCCalendario1.QuantidadeItensCombo > 2 && UCCCalendario1.MostrarMensagemSelecione);
                }

                if (UCCCalendario1.Valor != -1)
                    UCCCalendario1_IndexChanged();
            }
        }

        public void UCCCalendario1_IndexChanged()
        {
            UCCCursoCurriculo1.Valor = new[] { -1, -1 };
            UCCCursoCurriculo1.PermiteEditar = false;
            UCCCurriculoPeriodo1.Valor = new[] { -1, -1, -1 };
            UCCCurriculoPeriodo1.PermiteEditar = false;
            UCCTurma1.Valor = new long[] { -1, -1, -1 };
            UCCTurma1.PermiteEditar = false;
            fdsResultado.Visible = false;
            lblInfoDocenciaCompartilhada.Visible = false;
            _lblMsgRegencia.Visible = false;

            if (UCCCalendario1.Valor != -1)
            {
                UCCCursoCurriculo1.CarregarPorEscolaCalendarioSituacaoCurso(esc_id, uni_id, UCCCalendario1.Valor, (byte)(UCCCalendario1.Cal_ano < DateTime.Now.Year ? 0 : 1));
                UCCCursoCurriculo1.PermiteEditar = true;

                if (UCCCursoCurriculo1.Valor[0] != -1)
                    UCCCursoCurriculo1_IndexChanged();
            }
        }

        public void UCCCursoCurriculo1_IndexChanged()
        {
            UCCCurriculoPeriodo1.Valor = new[] { -1, -1, -1 };
            UCCCurriculoPeriodo1.PermiteEditar = false;
            UCCTurma1.Valor = new long[] { -1, -1, -1 };
            UCCTurma1.PermiteEditar = false;
            fdsResultado.Visible = false;
            lblInfoDocenciaCompartilhada.Visible = false;
            _lblMsgRegencia.Visible = false;

            if (UCCCursoCurriculo1.Valor[0] != -1 || __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual)
            {
                if (__SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.Individual)
                {
                    UCCCurriculoPeriodo1.CarregaPorCursoCurriculoEscola(__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual ? 0 : UCCCursoCurriculo1.Valor[0],
                        UCCCursoCurriculo1.Valor[1], esc_id, uni_id);
                }

                UCCCurriculoPeriodo1.PermiteEditar = true;

                if (UCCCurriculoPeriodo1.Valor[0] != -1 || __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual)
                    UCCCurriculoPeriodo1_IndexChanged();
            }
        }

        public void UCCCurriculoPeriodo1_IndexChanged()
        {
            UCCTurma1.Valor = new long[] { -1, -1, -1 };
            UCCTurma1.PermiteEditar = false;
            fdsResultado.Visible = false;
            lblInfoDocenciaCompartilhada.Visible = false;
            _lblMsgRegencia.Visible = false;

            if ((UCCCurriculoPeriodo1.Valor[0]) != -1 || __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual)
            {
                if (__SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.Individual)
                {
                    UCCTurma1.CarregarPorEscolaCalendarioEPeriodo(esc_id, uni_id, UCCCalendario1.Valor, UCCCursoCurriculo1.Valor[0], UCCCursoCurriculo1.Valor[1], UCCCurriculoPeriodo1.Valor[2], (byte)TUR_TurmaSituacao.Ativo);
                }
                else
                {
                    UCCTurma1.CarregarPorEscolaCalendarioEPeriodo(esc_id, uni_id, UCCCalendario1.Valor, UCCCursoCurriculo1.Valor[0], UCCCursoCurriculo1.Valor[1], 0, (byte)TUR_TurmaSituacao.Ativo);
                }

                UCCTurma1.PermiteEditar = true;

                if (UCCTurma1.Valor[0] != -1)
                    UCCTurma1_IndexChanged();
            }
        }

        public void UCCTurma1_IndexChanged()
        {
            ViewState["VS_cal_ano"] = null;
            fdsResultado.Visible = false;
            lblInfoDocenciaCompartilhada.Visible = false;
            _lblMsgRegencia.Visible = false;

            if (UCCTurma1.Valor[0] != -1)
            {
                if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual)
                {
                    fdsResultado.Visible = ComboDocenteValor[0] > 0;
                    if (ComboDocenteValor[0] > 0)
                        CarregaDadosTurma(UCCTurma1.Valor[0], ComboDocenteValor[0]);
                }
                else
                {
                    UCComboDocente1_IndexChanged();
                }
            }
        }

        public void UCComboDocente1_IndexChanged()
        {
            fdsResultado.Visible = false;
            lblInfoDocenciaCompartilhada.Visible = false;
            _lblMsgRegencia.Visible = false;

            if (ComboDocenteValor[0] != -1)
            {
                fdsResultado.Visible = UCCTurma1.Valor[0] > 0;
                if (UCCTurma1.Valor[0] > 0)
                    CarregaDadosTurma(UCCTurma1.Valor[0], ComboDocenteValor[0]);
            }
        }

        private void UCConfirmacaoOperacao_ConfimaOperacao()
        {
            Salvar();
        }

        #endregion Delegates

        #region Métodos

        /// <summary>
        /// Seta os delegates
        /// </summary>
        public void SetaDelegates()
        {
            UCComboUAEscola1.IndexChangedUA += UCComboUAEscola1_IndexChanged;
            UCComboUAEscola1.IndexChangedUnidadeEscola += UCComboUAEscola1_IndexChanged;
            UCCCalendario1.IndexChanged += UCCCalendario1_IndexChanged;
            UCCCursoCurriculo1.IndexChanged += UCCCursoCurriculo1_IndexChanged;
            UCCCurriculoPeriodo1.IndexChanged += UCCCurriculoPeriodo1_IndexChanged;
            UCCTurma1.IndexChanged += UCCTurma1_IndexChanged;
            UCComboDocente2._SelecionaDocente += UCComboDocente1_IndexChanged;
            UCBuscaDocenteEscola1.ReturnValues += UCBuscaDocenteEscola1_ReturnValues;
            UCConfirmacaoOperacao.ConfimaOperacao += UCConfirmacaoOperacao_ConfimaOperacao;
        }

        /// <summary>
        /// Carrega todos os dados da turma utilizados na tela
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        public void CarregaDadosTurma(long tur_id, long doc_id)
        {
            string _doc_id = "0";

            if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual && __SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                _doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id.ToString();

            Dictionary<string, string> filtros = new Dictionary<string, string>();
            _dgvTurma.PageIndex = 0;

            _odsTurma.SelectParameters.Clear();
            _odsTurma.SelectParameters.Add("tur_id", tur_id.ToString());
            _odsTurma.SelectParameters.Add("doc_id", _doc_id);

            _dgvTurma.Sort(VS_Ordenacao, VS_SortDirection);

            #region Salvar busca realizada com os parâmetros do ODS.

            int coc_id = Convert.ToInt32(ComboDocenteValor[3]);
            coc_id = coc_id.Equals(-1) ? RHU_ColaboradorCargoBO.SelectColaboradorCargoID(ComboDocenteValor[1], Convert.ToInt32(ComboDocenteValor[2]),
                __SessionWEB.__UsuarioWEB.Usuario.ent_id, UCComboUAEscola1.Esc_ID) : coc_id;

            //Carrega os filtros necessários para recarregar a tela novamente
            filtros.Add("uad_idSuperior", uad_id.ToString());
            filtros.Add("esc_id", esc_id.ToString());
            filtros.Add("uni_id", uni_id.ToString());
            filtros.Add("cal_id", UCCCalendario1.Valor.ToString());
            filtros.Add("cur_id", UCCCursoCurriculo1.Valor[0].ToString());
            filtros.Add("crr_id", UCCCursoCurriculo1.Valor[1].ToString());
            filtros.Add("crp_id", UCCCurriculoPeriodo1.Valor[2].ToString());
            filtros.Add("tur_id", tur_id.ToString());
            filtros.Add("ttn_id", UCCTurma1.Valor[2].ToString());
            filtros.Add("doc_id", ComboDocenteValor[0].ToString());
            filtros.Add("col_id", ComboDocenteValor[1].ToString());
            filtros.Add("crg_id", ComboDocenteValor[2].ToString());
            filtros.Add("coc_id", coc_id.ToString());
            filtros.Add("pes_nome", txtNomeDocente.Text);

            __SessionWEB.BuscaRealizada = new BuscaGestao
            {
                PaginaBusca = PaginaGestao.AtribuicaoDocentes
                ,
                Filtros = filtros
            };

            #endregion

            #region Verifica Colunas de Projetos e Docência Compartilhada
            if (aplicarNovaRegraDocenciaCompartilhada)
            {
                VS_TudId_DocenciaCompartilhada = TUR_TurmaDocenteBO.SelectTitularDisciplinaDocenciaCompartilhada(tur_id, ComboDocenteValor[0]);
                if (VS_TudId_DocenciaCompartilhada > 0)
                {
                    TUR_TurmaDisciplina turmaDisciplinaCompartilhada = TUR_TurmaDisciplinaBO.GetEntity(new TUR_TurmaDisciplina { tud_id = VS_TudId_DocenciaCompartilhada });
                    _dgvTurma.Columns[indexColumnProjeto].Visible = false;
                    _dgvTurma.Columns[indexColumnCompartilhada].Visible = true;
                    _dgvTurma.Columns[indexColumnCompartilhada].HeaderText = turmaDisciplinaCompartilhada.tud_nome;
                    lblInfoDocenciaCompartilhada.Visible = __SessionWEB.__UsuarioWEB.Docente.doc_id > 0;
                }
                else
                {
                    _dgvTurma.Columns[indexColumnProjeto].Visible = _dgvTurma.Columns[indexColumnCompartilhada].Visible = false;
                }
            }
            else
            {
                List<TUR_TurmaDocenteProjeto> ltTurmaDocenteProjeto = TUR_TurmaDocenteProjetoBO.SelectBy_TurmaDocenteColaboradorCargo(
                    tur_id,
                    ComboDocenteValor[0],
                    ComboDocenteValor[1],
                    Convert.ToInt32(ComboDocenteValor[2]),
                    coc_id
                );

                _dgvTurma.Columns[indexColumnProjeto].Visible = ltTurmaDocenteProjeto.Any(p => p.tdc_id == (byte)EnumTipoDocente.Projeto);
                _dgvTurma.Columns[indexColumnCompartilhada].Visible = ltTurmaDocenteProjeto.Any(p => p.tdc_id == (byte)EnumTipoDocente.Compartilhado);
            }

            _dgvTurma.Columns[indexColumnVigenciaInicio].Visible = _dgvTurma.Columns[indexColumnVigenciaFim].Visible = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_VIGENCIA_ATRIBUICAO_DOCENTE, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            _odsTurma.SelectParameters.Add("tud_id_Compartilhada", VS_TudId_DocenciaCompartilhada.ToString());

            #endregion

            if ((__SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.Individual) && esc_terceirizada)

            {
                _dgvTurma.Columns[indexColumnTitular].Visible = true;
            }
            else
            {
                _dgvTurma.Columns[indexColumnTitular].Visible = false;
            }

            // atualiza o grid
            _dgvTurma.DataBind();
        }

        /// <summary>
        /// Salva a frequência mensal
        /// </summary>
        public void Salvar()
        {
            try
            {
                long tud_id = 0;
                string msgLog = "";
                int posicaoRegencia = 0;
                int posicaoRegencia2 = 0;
                int posicao = 0;
                int posicao2 = 0;
                List<int> lstPosicoesAnteriores = new List<int>();
                bool docProjetoAntes = false;
                bool docProjetoDepois = false;
                bool docCompartilhadaAntes = false;
                bool docCompartilhadaDepois = false;
                List<int> lstPosicaoSalvar = new List<int>();
                List<TUR_TurmaDocente> lstTdtSalvar = new List<TUR_TurmaDocente>();
                List<TUR_TurmaDisciplinaRelacionada> lstTdrSalvar = new List<TUR_TurmaDisciplinaRelacionada>();

                bool chkTitularRegenciaChecked = false;
                bool chkSegundoTitularRegenciaChecked = false;
                bool chkProjetoRegenciaChecked = false;
                bool chkCompartilhadaRegenciaChecked = false;
                bool chkSubstitutoRegenciaChecked = false;

                int posicaoDocenteTitular = TipoDocente.Find(f => f.tdc_id == (byte)EnumTipoDocente.Titular).tdc_posicao;
                int posicaoDocenteSegundoTitular = TipoDocente.Find(f => f.tdc_id == (byte)EnumTipoDocente.SegundoTitular).tdc_posicao;
                int posicaoDocenteProjeto = TipoDocente.Find(f => f.tdc_id == (byte)EnumTipoDocente.Projeto).tdc_posicao;
                int posicaoDocenteCompartilhado = TipoDocente.Find(f => f.tdc_id == (byte)EnumTipoDocente.Compartilhado).tdc_posicao;
                int posicaoDocenteSubstituto = TipoDocente.Find(f => f.tdc_id == (byte)EnumTipoDocente.Substituto).tdc_posicao;

                List<ACA_TipoDocente> lstTipoDocente = ACA_TipoDocenteBO.SelecionaAtivos(ApplicationWEB.AppMinutosCacheLongo);
                bool possuiTerritorio = (from GridViewRow row in _dgvTurma.Rows
                                         let tud_tipo = Convert.ToInt32(_dgvTurma.DataKeys[row.RowIndex].Values["tud_tipo"])
                                         where tud_tipo == (int)TurmaDisciplinaTipo.Experiencia
                                         select tud_tipo).Any();

                List<TUR_TurmaDisciplinaTerritorio> lstTurmaDisciplinaTerritorio = new List<TUR_TurmaDisciplinaTerritorio>();

                int maxQtdSubstituto = 1;
                if (lstTipoDocente.Any(p => p.tdc_posicao == posicaoDocenteSubstituto))
                {
                    maxQtdSubstituto = lstTipoDocente.Find(p => p.tdc_posicao == posicaoDocenteSubstituto).tdc_quantidade;
                }

                bool exibirVigenciaAtribuicaoDocente = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_VIGENCIA_ATRIBUICAO_DOCENTE, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                bool mudouVigenciaSubstituto = false;
                DateTime dataAtual = DateTime.Now;
                string nomeDisciplinaRegencia = string.Empty;
                string inicioSubstitutoRegencia = string.Empty;
                string fimSubstitutoRegencia = string.Empty;
                int qtdDocenciaCompartilhada = 0;
                int maxQtdDocenciaCompartilhada = 0;
                if (aplicarNovaRegraDocenciaCompartilhada && VS_TudId_DocenciaCompartilhada > 0)
                {
                    ACA_TipoDisciplina tipoDisciplina = ACA_TipoDisciplinaBO.SelecionaTipoDisciplinaPorTudId(VS_TudId_DocenciaCompartilhada);
                    maxQtdDocenciaCompartilhada = tipoDisciplina.tds_qtdeDisciplinaRelacionada;
                }

                Dictionary<long, int> posicaoExperiencia;
                Dictionary<long, int> posicaoExperiencia2;
                Dictionary<long, bool> chkTitularExperienciaChecked;
                Dictionary<long, bool> chkSegundoTitularExperienciaChecked;
                Dictionary<long, bool> chkSubstitutoExperienciaChecked;
                Dictionary<long, bool> chkProjetoExperienciaChecked;
                Dictionary<long, string> nomeDisciplinaExperiencia;
                Dictionary<long, string> inicioSubstitutoExperiencia;
                Dictionary<long, string> fimSubstitutoExperiencia;

                if (possuiTerritorio)
                {

                    lstTurmaDisciplinaTerritorio = TUR_TurmaDisciplinaTerritorioBO.SelecionaPorTurma(UCCTurma1.Valor[0], ApplicationWEB.AppMinutosCacheLongo)
                        .Where(p => p.tte_vigenciaInicio <= DateTime.Now.Date &&
                            (p.tte_vigenciaFim == new DateTime() || p.tte_vigenciaFim >= DateTime.Now.Date)).ToList();

                    var tudExperiencia = from GridViewRow row in _dgvTurma.Rows
                                         where Convert.ToInt32(_dgvTurma.DataKeys[row.RowIndex].Values["tud_tipo"]) == (int)TurmaDisciplinaTipo.Experiencia
                                         select new
                                         {
                                             tud_id = Convert.ToInt64(_dgvTurma.DataKeys[row.RowIndex].Values["tud_id"])
                                         };

                    var experiencias = from exp in tudExperiencia
                                       group exp by exp.tud_id into gTte
                                       select new
                                       {
                                           tud_id = gTte.Key
                                           ,
                                           inteiro = 0
                                           ,
                                           booleano = false
                                           ,
                                           texto = string.Empty
                                       };

                    posicaoExperiencia = experiencias.ToDictionary(p => p.tud_id, p => p.inteiro);
                    posicaoExperiencia2 = experiencias.ToDictionary(p => p.tud_id, p => p.inteiro);
                    chkTitularExperienciaChecked = experiencias.ToDictionary(p => p.tud_id, p => p.booleano);
                    chkSegundoTitularExperienciaChecked = experiencias.ToDictionary(p => p.tud_id, p => p.booleano);
                    chkSubstitutoExperienciaChecked = experiencias.ToDictionary(p => p.tud_id, p => p.booleano);
                    chkProjetoExperienciaChecked = experiencias.ToDictionary(p => p.tud_id, p => p.booleano);

                    nomeDisciplinaExperiencia = experiencias.ToDictionary(p => p.tud_id, p => p.texto);
                    inicioSubstitutoExperiencia = experiencias.ToDictionary(p => p.tud_id, p => p.texto);
                    fimSubstitutoExperiencia = experiencias.ToDictionary(p => p.tud_id, p => p.texto);
                }
                else
                {
                    posicaoExperiencia = new Dictionary<long, int>();
                    posicaoExperiencia2 = new Dictionary<long, int>();
                    chkTitularExperienciaChecked = new Dictionary<long, bool>();
                    chkSegundoTitularExperienciaChecked = new Dictionary<long, bool>();
                    chkSubstitutoExperienciaChecked = new Dictionary<long, bool>();
                    chkProjetoExperienciaChecked = new Dictionary<long, bool>();
                    nomeDisciplinaExperiencia = new Dictionary<long, string>();
                    inicioSubstitutoExperiencia = new Dictionary<long, string>();
                    fimSubstitutoExperiencia = new Dictionary<long, string>();

                }

                //Grava qual a posição da disciplina de regência para usar essa posição nas disciplinas de componente regência
                foreach (GridViewRow row in _dgvTurma.Rows)
                {
                    CheckBox _chkCompartilhada = (CheckBox)row.FindControl("_chkCompartilhada");
                    int tud_tipo = Convert.ToInt32(_dgvTurma.DataKeys[row.RowIndex].Values["tud_tipo"]);
                    if (tud_tipo == (int)TurmaDisciplinaTipo.Regencia)
                    {
                        CheckBox chkTitular = (CheckBox)row.FindControl("chkTitular");
                        CheckBox chkSegundoTitular = (CheckBox)row.FindControl("chkSegundoTitular");
                        CheckBox _chkProjeto = (CheckBox)row.FindControl("_chkProjeto");
                        CheckBox _chkSubstituto = (CheckBox)row.FindControl("_chkSubstituto");
                        TextBox txtVigenciaInicio = (TextBox)row.FindControl("txtVigenciaInicio");
                        TextBox txtVigenciaFim = (TextBox)row.FindControl("txtVigenciaFim");

                        nomeDisciplinaRegencia = _dgvTurma.DataKeys[row.RowIndex].Values["tud_nome"].ToString();

                        chkTitularRegenciaChecked = chkTitular.Checked;
                        chkSegundoTitularRegenciaChecked = chkSegundoTitular.Checked;
                        chkProjetoRegenciaChecked = _chkProjeto.Checked;
                        chkCompartilhadaRegenciaChecked = _chkCompartilhada.Checked;
                        chkSubstitutoRegenciaChecked = _chkSubstituto.Checked;
                        inicioSubstitutoRegencia = txtVigenciaInicio.Text;
                        fimSubstitutoRegencia = txtVigenciaFim.Text;

                        if (aplicarNovaRegraDocenciaCompartilhada)
                        {
                            posicaoRegencia = chkTitular.Checked ? posicaoDocenteTitular :
                                    chkSegundoTitular.Checked ? posicaoDocenteSegundoTitular :
                                    _chkSubstituto.Checked ? posicaoDocenteSubstituto : 0;
                            posicaoRegencia2 = 0;
                        }
                        else
                        {
                            posicaoRegencia = _chkProjeto.Checked ? posicaoDocenteProjeto :
                                              _chkCompartilhada.Checked ? posicaoDocenteCompartilhado :
                                              _chkSubstituto.Checked ? posicaoDocenteSubstituto : 0;
                            posicaoRegencia2 = _chkSubstituto.Checked ? posicaoDocenteSubstituto : 0;

                            if (posicaoRegencia == posicaoRegencia2)
                                posicaoRegencia2 = 0;
                        }
                    }

                    if (tud_tipo == (int)TurmaDisciplinaTipo.Experiencia)
                    {
                        CheckBox chkTitular = (CheckBox)row.FindControl("chkTitular");
                        CheckBox chkSegundoTitular = (CheckBox)row.FindControl("chkSegundoTitular");
                        CheckBox _chkProjeto = (CheckBox)row.FindControl("_chkProjeto");
                        CheckBox _chkSubstituto = (CheckBox)row.FindControl("_chkSubstituto");
                        TextBox txtVigenciaInicio = (TextBox)row.FindControl("txtVigenciaInicio");
                        TextBox txtVigenciaFim = (TextBox)row.FindControl("txtVigenciaFim");

                        long tud_idExperiencia = Convert.ToInt64(_dgvTurma.DataKeys[row.RowIndex].Values["tud_id"]);

                        nomeDisciplinaExperiencia[tud_idExperiencia] = _dgvTurma.DataKeys[row.RowIndex].Values["tud_nome"].ToString();

                        chkSubstitutoExperienciaChecked[tud_idExperiencia] = _chkSubstituto.Checked;
                        chkProjetoExperienciaChecked[tud_idExperiencia] = _chkProjeto.Checked;

                        inicioSubstitutoExperiencia[tud_idExperiencia] = txtVigenciaInicio.Text;
                        fimSubstitutoExperiencia[tud_idExperiencia] = txtVigenciaFim.Text;

                        posicaoExperiencia[tud_idExperiencia] = chkTitular.Checked ? posicaoDocenteTitular :
                                                                chkSegundoTitular.Checked ? posicaoDocenteSegundoTitular :
                                                                _chkProjeto.Checked ? posicaoDocenteProjeto :
                                                                _chkSubstituto.Checked ? posicaoDocenteSubstituto : 0;

                        posicaoExperiencia2[tud_idExperiencia] = _chkSubstituto.Checked ? posicaoDocenteSubstituto : 0;

                        if (posicaoExperiencia[tud_idExperiencia] == posicaoExperiencia2[tud_idExperiencia])
                            posicaoExperiencia2[tud_idExperiencia] = 0;
                    }

                    if (aplicarNovaRegraDocenciaCompartilhada
                        && _dgvTurma.Columns[indexColumnCompartilhada].Visible
                        && _chkCompartilhada.Checked)
                    {
                        qtdDocenciaCompartilhada++;
                    }
                }

                if (qtdDocenciaCompartilhada > 0
                    && maxQtdDocenciaCompartilhada > 0
                    && qtdDocenciaCompartilhada > maxQtdDocenciaCompartilhada)
                {
                    throw new ValidationException(string.Format(GetGlobalResourceObject("Academico", "RecursosHumanos.AtribuicaoDocentes.Busca.ValidacaoQuantidadeDocenciaCompartilhada").ToString(), _dgvTurma.Columns[indexColumnCompartilhada].HeaderText, maxQtdDocenciaCompartilhada));
                }

                //Armazena o coc_id
                int coc_id = (int)ComboDocenteValor[3];
                coc_id = coc_id.Equals(-1) ? RHU_ColaboradorCargoBO.SelectColaboradorCargoID(ComboDocenteValor[1], Convert.ToInt32(ComboDocenteValor[2]),
                    __SessionWEB.__UsuarioWEB.Usuario.ent_id, UCComboUAEscola1.Esc_ID) : coc_id;

                DateTime dataDocenciaCompartilhada = new DateTime(dataAtual.Year, dataAtual.Month, dataAtual.Day);
                int qtdSubstituto;
                foreach (GridViewRow row in _dgvTurma.Rows)
                {
                    qtdSubstituto = 0;

                    CheckBox chkTitular = (CheckBox)row.FindControl("chkTitular");
                    CheckBox chkSegundoTitular = (CheckBox)row.FindControl("chkSegundoTitular");
                    CheckBox _chkProjeto = (CheckBox)row.FindControl("_chkProjeto");
                    CheckBox _chkCompartilhada = (CheckBox)row.FindControl("_chkCompartilhada");
                    CheckBox _chkSubstituto = (CheckBox)row.FindControl("_chkSubstituto");
                    int tud_tipo = Convert.ToInt32(_dgvTurma.DataKeys[row.RowIndex].Values["tud_tipo"]);
                    TextBox txtVigenciaInicio = (TextBox)row.FindControl("txtVigenciaInicio");
                    TextBox txtVigenciaFim = (TextBox)row.FindControl("txtVigenciaFim");

                    if (aplicarNovaRegraDocenciaCompartilhada)
                    {
                        posicao = chkTitular.Checked ? posicaoDocenteTitular :
                                    chkSegundoTitular.Checked ? posicaoDocenteSegundoTitular :
                                    _chkSubstituto.Checked ? posicaoDocenteSubstituto : 0;
                        posicao2 = 0;
                    }
                    else
                    {
                        posicao = _chkProjeto.Checked ? posicaoDocenteProjeto :
                                    _chkCompartilhada.Checked ? posicaoDocenteCompartilhado :
                                    _chkSubstituto.Checked ? posicaoDocenteSubstituto : 0;
                        posicao2 = _chkSubstituto.Checked ? posicaoDocenteSubstituto : 0;

                        if (posicao == posicao2)
                            posicao2 = 0;
                    }

                    //Se for uma disciplina de componente regência então pega a mesma posição da disciplina de regência
                    if (tud_tipo == (int)TurmaDisciplinaTipo.ComponenteRegencia)
                    {
                        posicao = posicaoRegencia;
                        posicao2 = posicaoRegencia2;

                        chkTitular.Checked = chkProjetoRegenciaChecked;
                        chkSegundoTitular.Checked = chkProjetoRegenciaChecked;

                        _chkProjeto.Checked = chkProjetoRegenciaChecked;
                        _chkCompartilhada.Checked = chkCompartilhadaRegenciaChecked;
                        _chkSubstituto.Checked = chkSubstitutoRegenciaChecked;

                        txtVigenciaInicio.Text = inicioSubstitutoRegencia;
                        txtVigenciaFim.Text = fimSubstitutoRegencia;
                    }

                    if (tud_tipo == (int)TurmaDisciplinaTipo.TerritorioSaber)
                    {
                        long tud_idTerritorio = Convert.ToInt64(_dgvTurma.DataKeys[row.RowIndex].Values["tud_id"]);
                        long tud_idExperiencia = lstTurmaDisciplinaTerritorio.Where(p => p.tud_idTerritorio == tud_idTerritorio).First().tud_idExperiencia;

                        posicao = posicaoExperiencia[tud_idExperiencia];
                        posicao2 = posicaoExperiencia2[tud_idExperiencia];

                        chkTitular.Checked = chkTitularExperienciaChecked[tud_idExperiencia];
                        chkSegundoTitular.Checked = chkSegundoTitularExperienciaChecked[tud_idExperiencia];

                        _chkSubstituto.Checked = chkSubstitutoExperienciaChecked[tud_idExperiencia];
                        _chkProjeto.Checked = chkProjetoExperienciaChecked[tud_idExperiencia];

                        txtVigenciaInicio.Text = inicioSubstitutoExperiencia[tud_idExperiencia];
                        txtVigenciaFim.Text = fimSubstitutoExperiencia[tud_idExperiencia];
                    }

                    // Verifico se a vigencia de inicio para o substituto foi preenchida
                    if (exibirVigenciaAtribuicaoDocente && _chkSubstituto.Checked)
                    {
                        if (string.IsNullOrEmpty(txtVigenciaInicio.Text))
                        {
                            throw new ValidationException(GetGlobalResourceObject("Academico", "RecursosHumanos.AtribuicaoDocentes.Busca.ValidacaoVigenciaInicio").ToString());
                        }
                        if (!string.IsNullOrEmpty(txtVigenciaFim.Text) && Convert.ToDateTime(txtVigenciaInicio.Text) > Convert.ToDateTime(txtVigenciaFim.Text))
                        {
                            throw new ValidationException(GetGlobalResourceObject("Academico", "RecursosHumanos.AtribuicaoDocentes.Busca.ValidacaoVigenciaFim").ToString());
                        }
                    }

                    bool mudouPosicao = true, mudouPosicao2 = true, mudouPosicao3 = true;
                    if (aplicarNovaRegraDocenciaCompartilhada)
                    {
                        // Considera que nao mudou a posicao de projeto/compartilhado se ja estava checado e continuou assim
                        // ou se nao estava checado e continuou assim
                        if (VS_TudId_DocenciaCompartilhada <= 0
                            || (
                                    (_chkCompartilhada.Checked && _dgvTurma.DataKeys[row.RowIndex].Values["tdr_id"].ToString() != "-1")
                                    || (!_chkCompartilhada.Checked && _dgvTurma.DataKeys[row.RowIndex].Values["tdr_id"].ToString() == "-1")
                                )
                            )
                        {
                            mudouPosicao3 = false;
                        }

                        //Controla a obrigatoriedade de ter pelo menos uma disciplina atribuida como compartilhada 
                        //se já tinha alguma atribuida como compartilhada antes
                        if (_dgvTurma.DataKeys[row.RowIndex].Values["tdr_id"].ToString() != "-1" &&
                            _chkCompartilhada.Enabled && _dgvTurma.Columns[indexColumnCompartilhada].Visible)
                            docCompartilhadaAntes = true;
                    }
                    else
                    {
                        mudouPosicao3 = false;
                    }

                    if (!string.IsNullOrEmpty(_dgvTurma.DataKeys[row.RowIndex].Values["tdt_id"].ToString()))
                    {
                        for (int i = 0; i < _dgvTurma.DataKeys[row.RowIndex].Values["tdt_id"].ToString().Split(',').Length; i++)
                        {
                            int posicaoSalva = Convert.ToInt32(_dgvTurma.DataKeys[row.RowIndex].Values["tdt_posicao"].ToString().Split(',')[i]);
                            if (posicao == posicaoSalva)
                                mudouPosicao = false;

                            if (posicao2 == posicaoSalva)
                                mudouPosicao2 = false;

                            if (!aplicarNovaRegraDocenciaCompartilhada)
                            {
                                //Controla a obrigatoriedade de ter pelo menos uma disciplina atribuida como projeto 
                                //se já tinha alguma atribuida como projeto antes
                                if (Convert.ToInt32(_dgvTurma.DataKeys[row.RowIndex].Values["tdt_posicao"].ToString().Split(',')[i]) == posicaoDocenteProjeto &&
                                    _chkProjeto.Enabled && _dgvTurma.Columns[indexColumnProjeto].Visible)
                                    docProjetoAntes = true;

                                //Controla a obrigatoriedade de ter pelo menos uma disciplina atribuida como compartilhada 
                                //se já tinha alguma atribuida como compartilhada antes
                                if (Convert.ToInt32(_dgvTurma.DataKeys[row.RowIndex].Values["tdt_posicao"].ToString().Split(',')[i]) == posicaoDocenteCompartilhado &&
                                    _chkCompartilhada.Enabled && _dgvTurma.Columns[indexColumnCompartilhada].Visible)
                                    docCompartilhadaAntes = true;
                            }
                        }
                    }

                    if (aplicarNovaRegraDocenciaCompartilhada)
                    {
                        if (_dgvTurma.Columns[indexColumnCompartilhada].Visible && _chkCompartilhada.Checked)
                            docCompartilhadaDepois = true;
                    }
                    else
                    {
                        if (posicao == posicaoDocenteProjeto)
                            docProjetoDepois = true;
                        if (posicao == posicaoDocenteCompartilhado)
                            docCompartilhadaDepois = true;
                    }

                    //Se não mudou nenhuma posição então não salva
                    if (!mudouPosicao && !mudouPosicao2 && !mudouPosicao3)
                        continue;

                    //Verifica se é a mesma disciplina que está testando
                    if (tud_id != Convert.ToInt32(_dgvTurma.DataKeys[row.RowIndex].Values["tud_id"]))
                    {
                        tud_id = Convert.ToInt32(_dgvTurma.DataKeys[row.RowIndex].Values["tud_id"]);
                        lstPosicoesAnteriores = new List<int>();
                        //Carrega as posições da disciplina
                        foreach (var item in TUR_TurmaDocenteBO.SelecionaDocentesPosicaoPorDisciplina(Convert.ToInt32(_dgvTurma.DataKeys[row.RowIndex].Values["tud_id"])))
                            lstPosicoesAnteriores.Add(item.Key);
                    }

                    //Verifica se houve mudança de docencia compartilhada
                    if (aplicarNovaRegraDocenciaCompartilhada)
                    {
                        bool alterado = true;
                        if (_dgvTurma.DataKeys[row.RowIndex].Values["tdr_id"].ToString() != "-1")
                        {
                            // Existia o relacionamento com a disciplina compartilhada e continua a existir
                            if (_chkCompartilhada.Checked)
                                alterado = false;
                        }
                        else
                        {
                            // Não existia o relacionamento com a disciplina compartilhada e continua não existindo
                            if (!_chkCompartilhada.Checked)
                                alterado = false;
                        }

                        // Remove o relacionamento com a disciplina compartilhada
                        if (alterado
                            && VS_TudId_DocenciaCompartilhada > 0
                            && _dgvTurma.DataKeys[row.RowIndex].Values["tdr_id"].ToString() != "-1"
                            && !_chkCompartilhada.Checked)
                        {
                            TUR_TurmaDisciplinaRelacionada tdrAnterior = new TUR_TurmaDisciplinaRelacionada
                            {
                                tdr_id = Convert.ToInt64(_dgvTurma.DataKeys[row.RowIndex].Values["tdr_id"]),
                                tud_id = VS_TudId_DocenciaCompartilhada,
                                tud_idRelacionada = Convert.ToInt64(_dgvTurma.DataKeys[row.RowIndex].Values["tud_id"])
                            };
                            TUR_TurmaDisciplinaRelacionadaBO.GetEntity(tdrAnterior);

                            if (tdrAnterior.tdr_id > 0 && tdrAnterior.IsNew)
                                throw new ValidationException("Erro ao carregar disciplina relacionada.");

                            if (!tdrAnterior.IsNew)
                            {
                                tdrAnterior.tdr_dataAlteracao = dataAtual;
                                tdrAnterior.tdr_vigenciaFim = dataDocenciaCompartilhada;
                                tdrAnterior.tdr_situacao = (byte)TUR_TurmaDisciplinaRelacionadaSituacao.Inativo;

                                lstTdrSalvar.Add(tdrAnterior);

                                msgLog += " Removido: tud_id: " + tdrAnterior.tud_id.ToString() + " tud_idRelacionada: " + tdrAnterior.tud_idRelacionada.ToString() + ".";
                            }
                        }
                    }

                    mudouVigenciaSubstituto = false;
                    if (!string.IsNullOrEmpty(_dgvTurma.DataKeys[row.RowIndex].Values["tdt_id"].ToString()))
                    {
                        for (int i = 0; i < _dgvTurma.DataKeys[row.RowIndex].Values["tdt_id"].ToString().Split(',').Length; i++)
                        {
                            TUR_TurmaDocente tdtAnterior = new TUR_TurmaDocente
                            {
                                tud_id = Convert.ToInt64(_dgvTurma.DataKeys[row.RowIndex].Values["tud_id"]),
                                tdt_id = Convert.ToInt32(_dgvTurma.DataKeys[row.RowIndex].Values["tdt_id"].ToString().Split(',')[i])
                            };
                            TUR_TurmaDocenteBO.GetEntity(tdtAnterior);

                            if (tdtAnterior.tdt_id > 0 && tdtAnterior.IsNew)
                                throw new ValidationException("Erro ao carregar atribuição do docente.");

                            //Conta a quantidade de docentes na posicao de substituto
                            if (tdtAnterior.tdt_posicao == posicaoDocenteSubstituto)
                                qtdSubstituto++;

                            //Verifica se houve mudança de posição ou se houve mudança de docente
                            if ((tdtAnterior.tdt_posicao == (byte)posicao || tdtAnterior.tdt_posicao == (byte)posicao2)//|| tdtAnterior.tdt_posicao == 1) 
                                && ComboDocenteValor[0] == tdtAnterior.doc_id)
                            {
                                if (exibirVigenciaAtribuicaoDocente &&
                                        (tdtAnterior.tdt_posicao == posicaoDocenteSubstituto ||
                                        tdtAnterior.tdt_posicao == posicaoDocenteTitular ||
                                        tdtAnterior.tdt_posicao == posicaoDocenteSegundoTitular
                                    ))
                                {
                                    // O docente ja estava atribuido como substituto
                                    mudouVigenciaSubstituto = true;

                                    // Verifica se a vigencia foi alterada
                                    if (tdtAnterior.tdt_vigenciaInicio != Convert.ToDateTime(txtVigenciaInicio.Text)
                                        || ((string.IsNullOrEmpty(txtVigenciaFim.Text) && tdtAnterior.tdt_vigenciaFim != new DateTime())
                                            || (!string.IsNullOrEmpty(txtVigenciaFim.Text) && tdtAnterior.tdt_vigenciaFim != Convert.ToDateTime(txtVigenciaFim.Text))
                                        ))
                                    {
                                        tdtAnterior.tdt_dataAlteracao = dataAtual;
                                        tdtAnterior.tdt_vigenciaInicio = Convert.ToDateTime(txtVigenciaInicio.Text);
                                        tdtAnterior.tdt_vigenciaFim = !string.IsNullOrEmpty(txtVigenciaFim.Text) ? Convert.ToDateTime(txtVigenciaFim.Text) : new DateTime();
                                        tdtAnterior.tdt_situacao = (byte)TUR_TurmaDocenteSituacao.Ativo;
                                        lstTdtSalvar.Add(tdtAnterior);

                                        msgLog += " Alterado: tud_id: " + tdtAnterior.tud_id.ToString() + " doc_id: " + tdtAnterior.doc_id.ToString() + " tdt_posicao: " + tdtAnterior.tdt_posicao.ToString() + ".";
                                    }
                                }
                                continue;
                            }

                            if (!mudouPosicao
                                && ComboDocenteValor[0] != tdtAnterior.doc_id
                                && (tdtAnterior.tdt_posicao == (byte)posicao || tdtAnterior.tdt_posicao == (byte)posicao2))
                            {
                                mudouPosicao = true;
                            }

                            if ((tdtAnterior.tdt_posicao == posicaoDocenteTitular && !chkTitular.Enabled)
                                || (tdtAnterior.tdt_posicao == posicaoDocenteSegundoTitular && !chkSegundoTitular.Enabled)
                                || (tdtAnterior.tdt_posicao == posicaoDocenteProjeto && !_chkProjeto.Enabled)
                                || (tdtAnterior.tdt_posicao == posicaoDocenteSubstituto && !_chkSubstituto.Enabled)
                                || (tdtAnterior.tdt_posicao == posicaoDocenteCompartilhado && !_chkCompartilhada.Enabled))
                                continue;

                            // So desativo automaticamente o outro substituto, se for permitido apenas um docente para essa posicao,
                            // ou desativo um substituto atual se o check foi desmarcado.
                            bool desativarAnterior = (tdtAnterior.tdt_posicao != posicaoDocenteSubstituto || maxQtdSubstituto == 1)
                                                     || (posicao == 0 && posicao2 == 0
                                                        && (exibirVigenciaAtribuicaoDocente || maxQtdSubstituto != 1)
                                                        && ComboDocenteValor[0] == tdtAnterior.doc_id
                                                        && tdtAnterior.tdt_posicao == posicaoDocenteSubstituto
                                                        && !_chkSubstituto.Checked);

                            //Atualiza lista de posições
                            if (desativarAnterior)
                            {
                                lstPosicoesAnteriores.RemoveAll(p => p == tdtAnterior.tdt_posicao);
                            }

                            //Remove o registro com a posição antiga
                            if (!tdtAnterior.IsNew && desativarAnterior)
                            {
                                long doc_idAtual = ComboDocenteValor[0];
                                bool anteriorTitular = tdtAnterior.tdt_posicao == (byte)posicaoDocenteTitular;
                                bool anteriorSegundoTitular = tdtAnterior.tdt_posicao == (byte)posicaoDocenteSegundoTitular;
                                bool anteriorProjeto = aplicarNovaRegraDocenciaCompartilhada ? false : tdtAnterior.tdt_posicao == (byte)posicaoDocenteProjeto;
                                bool anteriorSubstituto = tdtAnterior.tdt_posicao == (byte)posicaoDocenteSubstituto;
                                bool anteriorCompartilhado = aplicarNovaRegraDocenciaCompartilhada ? false : tdtAnterior.tdt_posicao == (byte)posicaoDocenteCompartilhado;

                                if (((tdtAnterior.doc_id != doc_idAtual && ((anteriorProjeto && _chkProjeto.Checked) || (anteriorSubstituto && _chkSubstituto.Checked) || (anteriorCompartilhado && _chkCompartilhada.Checked) || (anteriorTitular && chkTitular.Checked) || (anteriorSegundoTitular && chkSegundoTitular.Checked)))
                                    || (tdtAnterior.doc_id == doc_idAtual && ((anteriorProjeto && !_chkProjeto.Checked) || (anteriorSubstituto && !_chkSubstituto.Checked) || (anteriorCompartilhado && !_chkCompartilhada.Checked) || (anteriorTitular && !chkTitular.Checked) || (anteriorSegundoTitular && !chkSegundoTitular.Checked))))
                                    && (tdtAnterior.tdt_situacao != (byte)TUR_TurmaDocenteSituacao.Inativo || (exibirVigenciaAtribuicaoDocente && tdtAnterior.tdt_vigenciaInicio > dataAtual)))
                                {
                                    tdtAnterior.tdt_dataAlteracao = dataAtual;
                                    if (tdtAnterior.tdt_vigenciaInicio > dataAtual)
                                    {
                                        tdtAnterior.tdt_vigenciaInicio = dataAtual;
                                    }
                                    tdtAnterior.tdt_vigenciaFim = dataAtual;
                                    tdtAnterior.tdt_situacao = (byte)TUR_TurmaDocenteSituacao.Inativo;

                                    lstTdtSalvar.Add(tdtAnterior);

                                    msgLog += " Removido: tud_id: " + tdtAnterior.tud_id.ToString() + " doc_id: " + tdtAnterior.doc_id.ToString() + " tdt_posicao: " + tdtAnterior.tdt_posicao.ToString() + ".";
                                }
                            }
                        }
                    }

                    // Valida a quantidade de docente substituto
                    if (mudouPosicao2
                        && (posicao == posicaoDocenteSubstituto || posicao2 == posicaoDocenteSubstituto)
                        && _chkSubstituto.Checked
                        && maxQtdSubstituto > 1
                        && !mudouVigenciaSubstituto
                        && (qtdSubstituto + 1) > maxQtdSubstituto)
                    {
                        string nomeDisciplina = _dgvTurma.DataKeys[row.RowIndex].Values["tud_nome"].ToString();

                        if (tud_tipo == (int)TurmaDisciplinaTipo.ComponenteRegencia)
                        {
                            nomeDisciplina = nomeDisciplinaRegencia;
                        }
                        else if (tud_tipo == (int)TurmaDisciplinaTipo.TerritorioSaber)
                        {
                            nomeDisciplina = nomeDisciplinaExperiencia[Convert.ToInt64(_dgvTurma.DataKeys[row.RowIndex].Values["tud_id"])];
                        }
                        throw new ValidationException(String.Format(GetGlobalResourceObject("Academico", "RecursosHumanos.AtribuicaoDocentes.Busca.ValidacaoQuantidadeSubstituto").ToString()
                                                                    , nomeDisciplina));
                    }

                    //Não deixa inserir 2 docentes na mesma posição na mesma disciplina
                    if (mudouPosicao && posicao != 0 && lstPosicoesAnteriores.Contains((byte)posicao)
                        && (posicao != posicaoDocenteSubstituto || maxQtdSubstituto == 1))
                        throw new ValidationException("Posição já ocupada na turma.");
                    //Não deixa inserir 2 docentes na mesma posição na mesma disciplina
                    if (mudouPosicao2 && posicao2 != 0 && lstPosicoesAnteriores.Contains((byte)posicao2)
                        && (posicao2 != posicaoDocenteSubstituto || maxQtdSubstituto == 1))
                        throw new ValidationException("Posição já ocupada na turma.");

                    //Se não foi marcado como "Sem atribuição" então grava um novo registro no turmadocente para o docente
                    if (mudouPosicao && posicao != 0 && !mudouVigenciaSubstituto && (!aplicarNovaRegraDocenciaCompartilhada || posicao == posicaoDocenteSubstituto || posicao == posicaoDocenteTitular || posicao == posicaoDocenteSegundoTitular))
                    {
                        DateTime tdt_vigenciaInicio, tdt_vigenciaFim;
                        RetornaVigencias(lstTurmaDisciplinaTerritorio, Convert.ToInt64(_dgvTurma.DataKeys[row.RowIndex].Values["tud_id"]), tud_tipo, posicao, posicaoDocenteSubstituto, exibirVigenciaAtribuicaoDocente, dataAtual, coc_id, txtVigenciaInicio, txtVigenciaFim, out tdt_vigenciaInicio, out tdt_vigenciaFim);

                        //Se mudou o tipo de atribuição do docente então cria um novo registro com a nova posição
                        TUR_TurmaDocente tdtNovo = new TUR_TurmaDocente
                        {
                            tud_id = Convert.ToInt64(_dgvTurma.DataKeys[row.RowIndex].Values["tud_id"]),
                            tdt_tipo = 1,//Regular:
                            tdt_vigenciaInicio = tdt_vigenciaInicio,
                            tdt_vigenciaFim = tdt_vigenciaFim,
                            col_id = ComboDocenteValor[1],
                            crg_id = (int)ComboDocenteValor[2],
                            coc_id = coc_id,
                            doc_id = ComboDocenteValor[0],
                            tdt_posicao = (byte)posicao,
                            tdt_situacao = (byte)TUR_TurmaDocenteSituacao.Ativo,
                            tdt_dataAlteracao = dataAtual,
                            tdt_dataCriacao = dataAtual
                        };

                        lstTdtSalvar.Add(tdtNovo);
                        msgLog += " Adicionado: tud_id: " + _dgvTurma.DataKeys[row.RowIndex].Values["tud_id"].ToString() +
                                  " doc_id: " + tdtNovo.doc_id.ToString() +
                                  " tdt_posicao: " + tdtNovo.tdt_posicao.ToString() + ". ";
                    }
                    //Se foi marcado mais uma posição então grava um novo registro no turmadocente para o docente
                    if (mudouPosicao2 && posicao2 != 0 && !mudouVigenciaSubstituto && (!aplicarNovaRegraDocenciaCompartilhada || posicao2 == posicaoDocenteSubstituto || posicao2 == posicaoDocenteTitular || posicao2 == posicaoDocenteSegundoTitular))
                    {
                        DateTime tdt_vigenciaInicio, tdt_vigenciaFim;
                        RetornaVigencias(lstTurmaDisciplinaTerritorio, Convert.ToInt64(_dgvTurma.DataKeys[row.RowIndex].Values["tud_id"]), tud_tipo, posicao, posicaoDocenteSubstituto, exibirVigenciaAtribuicaoDocente, dataAtual, coc_id, txtVigenciaInicio, txtVigenciaFim, out tdt_vigenciaInicio, out tdt_vigenciaFim);

                        //Se mudou o tipo de atribuição do docente então cria um novo registro com a nova posição
                        TUR_TurmaDocente tdtNovo = new TUR_TurmaDocente
                        {
                            tud_id = Convert.ToInt64(_dgvTurma.DataKeys[row.RowIndex].Values["tud_id"]),
                            tdt_tipo = 1,//Regular:
                            tdt_vigenciaInicio = tdt_vigenciaInicio,
                            tdt_vigenciaFim = tdt_vigenciaFim,
                            col_id = ComboDocenteValor[1],
                            crg_id = (int)ComboDocenteValor[2],
                            coc_id = coc_id,
                            doc_id = ComboDocenteValor[0],
                            tdt_posicao = (byte)posicao2,
                            tdt_situacao = (byte)TUR_TurmaDocenteSituacao.Ativo,
                            tdt_dataAlteracao = dataAtual,
                            tdt_dataCriacao = dataAtual
                        };

                        lstTdtSalvar.Add(tdtNovo);
                        msgLog += " Adicionado: tud_id: " + _dgvTurma.DataKeys[row.RowIndex].Values["tud_id"].ToString() +
                                  " doc_id: " + tdtNovo.doc_id.ToString() +
                                  " tdt_posicao: " + tdtNovo.tdt_posicao.ToString() + ". ";
                    }
                    // Cria o relacionamento com a disciplina compartilhada
                    if (aplicarNovaRegraDocenciaCompartilhada && mudouPosicao3 && _chkCompartilhada.Checked)
                    {
                        //Se mudou a atribuição do docente então cria um novo registro
                        TUR_TurmaDisciplinaRelacionada tdrNovo = new TUR_TurmaDisciplinaRelacionada
                        {
                            tdr_id = -1,
                            tud_id = VS_TudId_DocenciaCompartilhada,
                            tud_idRelacionada = Convert.ToInt64(_dgvTurma.DataKeys[row.RowIndex].Values["tud_id"]),
                            tdr_vigenciaInicio = dataDocenciaCompartilhada,
                            tdr_vigenciaFim = new DateTime(),
                            tdr_situacao = (byte)TUR_TurmaDocenteSituacao.Ativo,
                            tdr_dataAlteracao = dataAtual,
                            tdr_dataCriacao = dataAtual,
                            IsNew = true
                        };

                        lstTdrSalvar.Add(tdrNovo);
                        msgLog += " Adicionado: tud_id: " + tdrNovo.tud_id.ToString() +
                                  " tud_idRelacionada: " + tdrNovo.tud_idRelacionada.ToString() + ". ";
                    }
                }

                if (aplicarNovaRegraDocenciaCompartilhada)
                {
                    if (docCompartilhadaAntes && !docCompartilhadaDepois)
                        throw new ValidationException(String.Format(GetGlobalResourceObject("Academico", "RecursosHumanos.AtribuicaoDocentes.Busca.ValidacaoDocenciaCompartilhada2").ToString(), _dgvTurma.Columns[indexColumnCompartilhada].HeaderText));
                }
                else
                {
                    if (docProjetoAntes && !docProjetoDepois)
                        throw new ValidationException("Deve ser selecionada pelo menos uma atribuição como projeto.");

                    if (docCompartilhadaAntes && !docCompartilhadaDepois)
                        throw new ValidationException("Deve ser selecionada pelo menos uma atribuição como docência compartilhada.");
                }

                if (lstTdtSalvar.Count > 0 || lstTdrSalvar.Count > 0)
                {
                    //Salva alterações
                    if (lstTdtSalvar.Count > 0 || lstTdrSalvar.Count > 0)
                    {
                        if (lstTdtSalvar.Count > 0)
                        {
                            TUR_TurmaDocenteBO.SalvarTurmaDocente(UCCTurma1.Valor[0], lstTdtSalvar, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                        }
                        if (lstTdrSalvar.Count > 0)
                        {
                            TUR_TurmaDisciplinaRelacionadaBO.SalvarTurmaDisciplinaRelacionada(lstTdrSalvar, __SessionWEB.__UsuarioWEB.Usuario.ent_id, UCCTurma1.Valor[0].ToString(), ComboDocenteValor[0].ToString());
                        }
                    }

                    if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual)
                        UCCTurma1_IndexChanged();
                    else
                        UCComboDocente1_IndexChanged();

                    _lblMessage.Text = UtilBO.GetErroMessage("Atribuições alteradas com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, msgLog);
                }
                else
                    _lblMessage.Text = UtilBO.GetErroMessage("Nenhuma alteração a realizar.", UtilBO.TipoMensagem.Alerta);
            }
            catch (ThreadAbortException) { }
            catch (ValidationException e)
            {
                _lblMessage.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception e)
            {
                ApplicationWEB._GravaErro(e);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar atribuições do docente.", UtilBO.TipoMensagem.Erro);
            }
            ScriptManager.RegisterStartupScript(Page, GetType(), "ValidacaoSalvarAtribuicao", "$(document).ready(scrollToTop);", true);
        }

        /// <summary>
        /// Retorna as vigências de início e fim da atribuição de acordo com as regras.
        /// </summary>
        /// <param name="posicao"></param>
        /// <param name="posicaoDocenteSubstituto"></param>
        /// <param name="exibirVigenciaAtribuicaoDocente"></param>
        /// <param name="dataAtual"></param>
        /// <param name="coc_id"></param>
        /// <param name="txtVigenciaInicio"></param>
        /// <param name="txtVigenciaFim"></param>
        /// <param name="tdt_vigenciaInicio"></param>
        /// <param name="tdt_vigenciaFim"></param>
        private void RetornaVigencias(List<TUR_TurmaDisciplinaTerritorio> lstTerritorio, long tud_id, int tud_tipo, int posicao, int posicaoDocenteSubstituto, bool exibirVigenciaAtribuicaoDocente, DateTime dataAtual, int coc_id, TextBox txtVigenciaInicio, TextBox txtVigenciaFim, out DateTime tdt_vigenciaInicio, out DateTime tdt_vigenciaFim)
        {
            tdt_vigenciaInicio = exibirVigenciaAtribuicaoDocente
                                                                && posicao == posicaoDocenteSubstituto ? Convert.ToDateTime(txtVigenciaInicio.Text) : dataAtual;
            tdt_vigenciaFim = exibirVigenciaAtribuicaoDocente
                                    && posicao == posicaoDocenteSubstituto
                                    && !string.IsNullOrEmpty(txtVigenciaFim.Text) ? Convert.ToDateTime(txtVigenciaFim.Text) : new DateTime();

            if (tud_tipo == (int)TurmaDisciplinaTipo.Experiencia ||
                tud_tipo == (int)TurmaDisciplinaTipo.TerritorioSaber)
            {
                tdt_vigenciaFim = lstTerritorio.FirstOrDefault(p => tud_id == (tud_tipo == (int)TurmaDisciplinaTipo.Experiencia ? p.tud_idExperiencia : p.tud_idTerritorio)).tte_vigenciaFim;
            }

            RHU_Cargo entCargo = RHU_CargoBO.GetEntity
                (new RHU_Cargo
                {
                    crg_id = (int)ComboDocenteValor[2]
                });

            if (entCargo.crg_tipo == (byte)eTipoCargo.AtribuicaoEsporadica)
            {
                // Se for uma atribuição esporádica, precisa trazer a vigência de início e fim do cargo do docente.
                RHU_ColaboradorCargo entColaboradorCargo =
                    RHU_ColaboradorCargoBO.GetEntity(new RHU_ColaboradorCargo
                    {
                        col_id = ComboDocenteValor[1],
                        crg_id = (int)ComboDocenteValor[2],
                        coc_id = coc_id
                    });

                tdt_vigenciaInicio = entColaboradorCargo.coc_vigenciaInicio;
                tdt_vigenciaFim = entColaboradorCargo.coc_vigenciaFim;
            }
        }

        /// <summary>
        /// Verifica se tem busca salva na sessão e realiza automaticamente, caso positivo.
        /// </summary>
        private void VerificaBusca()
        {
            if (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.AtribuicaoDocentes)
            {
                // Recuperar busca realizada e pesquisar automaticamente
                string valor, valor2, valor3, valor4, valor5;
                if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual)
                {
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("doc_id", out valor);
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("col_id", out valor2);
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crg_id", out valor3);
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("coc_id", out valor4);
                    ComboDocenteValor = new long[] { Convert.ToInt64(valor), Convert.ToInt64(valor2),
                                                     Convert.ToInt64(valor3), Convert.ToInt64(valor4) };
                }
                else
                {
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("uad_idSuperior", out valor);
                    if (!string.IsNullOrEmpty(valor))
                    {
                        UCComboUAEscola1.Uad_ID = new Guid(valor);
                        UCComboUAEscola1.CarregaEscolaPorUASuperiorSelecionada();

                        if (UCComboUAEscola1.Uad_ID != Guid.Empty)
                        {
                            UCComboUAEscola1.FocoEscolas = true;
                            UCComboUAEscola1.PermiteAlterarCombos = true;
                        }
                    }
                }

                string esc_id;
                string uni_id;

                if ((__SessionWEB.BuscaRealizada.Filtros.TryGetValue("esc_id", out esc_id)) &&
                    (__SessionWEB.BuscaRealizada.Filtros.TryGetValue("uni_id", out uni_id)))
                {
                    UCComboUAEscola1.SelectedValueEscolas = new[] { Convert.ToInt32(esc_id), Convert.ToInt32(uni_id) };
                }

                if (UCComboUAEscola1.Esc_ID != -1)
                    UCComboUAEscola1_IndexChanged();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cal_id", out valor);
                UCCCalendario1.Valor = Convert.ToInt32(valor);
                UCCCalendario1_IndexChanged();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("cur_id", out valor2);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crr_id", out valor);
                UCCCursoCurriculo1.Valor = new[] { Convert.ToInt32(valor2), Convert.ToInt32(valor) };
                UCCCursoCurriculo1_IndexChanged();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crp_id", out valor);
                UCCCurriculoPeriodo1.Valor = new[] { UCCCursoCurriculo1.Valor[0], UCCCursoCurriculo1.Valor[1], Convert.ToInt32(valor) };
                UCCCurriculoPeriodo1_IndexChanged();

                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("tur_id", out valor);
                __SessionWEB.BuscaRealizada.Filtros.TryGetValue("ttn_id", out valor2);
                UCCTurma1.Valor = new[] { Convert.ToInt64(valor), -1, Convert.ToInt64(valor2) };
                UCCTurma1_IndexChanged();

                if (__SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.Individual)
                {
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("doc_id", out valor);
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("col_id", out valor2);
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("crg_id", out valor3);
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("coc_id", out valor4);
                    __SessionWEB.BuscaRealizada.Filtros.TryGetValue("pes_nome", out valor5);
                    ComboDocenteValor = new long[] { Convert.ToInt64(valor), Convert.ToInt64(valor2),
                                                     Convert.ToInt64(valor3), Convert.ToInt64(valor4) };
                    txtNomeDocente.Text = valor5;
                    UCComboDocente1_IndexChanged();
                }
            }
            else
            {
                fdsResultado.Visible = false;
                lblInfoDocenciaCompartilhada.Visible = false;
                _lblMsgRegencia.Visible = false;

                if (UCComboUAEscola1.Esc_ID != -1)
                    UCComboUAEscola1_IndexChanged();
            }
        }

        #endregion Métodos

        #region Eventos

        #region Page Life Cycle

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.ExitPageConfirm));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsSetExitPageConfirmer.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsAtribuicaoDocente.js"));

                if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.BOTAO_SALVAR_PERMANECE_TELA, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                {
                    if (!Convert.ToString(btnCancelar.CssClass).Contains("btnMensagemUnload"))
                    {
                        btnCancelar.CssClass += " btnMensagemUnload";
                    }
                }
            }

            if (!IsPostBack)
            {
                lblInfoDocenciaCompartilhada.Visible = false;
                if (!aplicarNovaRegraDocenciaCompartilhada)
                {
                    _dgvTurma.Columns[indexColumnCompartilhada].HeaderText = ACA_TipoDocenteBO.GetEntity(new ACA_TipoDocente { tdc_id = (byte)EnumTipoDocente.Compartilhado }).tdc_nome;
                    _dgvTurma.Columns[indexColumnProjeto].HeaderText = ACA_TipoDocenteBO.GetEntity(new ACA_TipoDocente { tdc_id = (byte)EnumTipoDocente.Projeto }).tdc_nome;
                }
                if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual)
                {
                    if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                    {
                        UCComboUAEscola1.InicializarVisaoIndividual(__SessionWEB.__UsuarioWEB.Docente.doc_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, 2);

                        //divComboDoceteIndividual.Visible = UCComboDocente2.PermiteEditar = divComboDocete.Visible = false;
                        UCComboDocente2.PermiteEditar = divComboDocete.Visible = false;
                        UCCCalendario1.Visible = UCCCurriculoPeriodo1.Visible = false;
                        UCComboDocente2._MostrarMessageSelecione = UCComboDocente2.Obrigatorio = true;
                    }
                    else
                    {
                        _lblMessage.Text = UtilBO.GetErroMessage("Este usuário não tem permissão de acesso a esta página.", UtilBO.TipoMensagem.Alerta);
                        fdsResultado.Visible = false;
                        fdsPesquisa.Visible = false;
                        return;
                    }
                }
                else
                {
                    UCComboUAEscola1.Inicializar();
                    divComboDocete.Visible = true;
                    btnBuscaDocente.Enabled = divComboDoceteIndividual.Visible = false;
                }

                // Verifico se foi um redirecionamento do Minhas turmas para atribuicao de docencia compartilhada
                if (Session["DadosPaginaRetorno"] != null && __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual)
                {
                    // Carrego os filtros da atribuicao de acordo com a turma selecionada no Minhas turmas
                    Dictionary<string, string> listaDados = (Dictionary<string, string>)Session["DadosPaginaRetorno"];
                    Session.Remove("DadosPaginaRetorno");

                    long tur_id = Convert.ToInt64(listaDados["Edit_tur_id"]);
                    TUR_Turma turma = TUR_TurmaBO.GetEntity(new TUR_Turma { tur_id = tur_id });

                    UCComboUAEscola1.SelectedValueEscolas = new[] { Convert.ToInt32(listaDados["Edit_esc_id"]), turma.uni_id };
                    if (UCComboUAEscola1.Esc_ID != -1)
                        UCComboUAEscola1_IndexChanged();

                    UCCCalendario1.Valor = Convert.ToInt32(listaDados["Edit_cal_id"]);
                    UCCCalendario1_IndexChanged();

                    List<TUR_TurmaCurriculo> Curriculos = TUR_TurmaCurriculoBO.GetSelectBy_Turma(tur_id, ApplicationWEB.AppMinutosCacheLongo);
                    if (Curriculos.Count > 0)
                    {
                        UCCCursoCurriculo1.Valor = new[] { Curriculos.First().cur_id, Curriculos.First().crr_id };
                        UCCCursoCurriculo1_IndexChanged();

                        UCCCurriculoPeriodo1.Valor = new[] { UCCCursoCurriculo1.Valor[0], UCCCursoCurriculo1.Valor[1], Curriculos.First().crp_id };
                        UCCCurriculoPeriodo1_IndexChanged();
                    }

                    ACA_Turno turno = ACA_TurnoBO.GetEntity(new ACA_Turno { trn_id = turma.trn_id });
                    UCCTurma1.Valor = new[] { tur_id, -1, turno.ttn_id };
                    UCCTurma1_IndexChanged();

                    Session.Remove("VS_DadosTurmas");
                }
                else
                {
                    VerificaBusca();
                }

                if (!usuarioPermissao)
                    btnSalvar.Visible = false;

                _lblLegenda.Text = GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA_PLURAL") + " da turma";
                _dgvTurma.Columns[0].HeaderText = GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA").ToString();
                lblInfoDocenciaCompartilhada.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "RecursosHumanos.AtribuicaoDocentes.Busca.lblInfoDocenciaCompartilhada.Text").ToString(), UtilBO.TipoMensagem.Informacao);

                UCConfirmacaoOperacao.Mensagem = (string)GetGlobalResourceObject("Academico", "RecursosHumanos.AtribuicaoDocentes.Busca.divConfirmacao.MensagemResponsavel.text");
                UCConfirmacaoOperacao.ObservacaoVisivel = false;
                UCConfirmacaoOperacao.ObservacaoObrigatorio = false;
            }

            RegistrarParametrosMensagemSair(btnSalvar.Visible, (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0));

            SetaDelegates();
        }

        #endregion Page Life Cycle

        protected void _dgvTurma_DataBound(object sender, EventArgs e)
        {
            // Seta propriedades necessárias para ordenação nas colunas.
            #region Ordenação

            ConfiguraColunasOrdenacao(_dgvTurma);

            if ((!string.IsNullOrEmpty(_dgvTurma.SortExpression)) &&
                (__SessionWEB.BuscaRealizada.PaginaBusca == PaginaGestao.AtribuicaoDocentes))
            {
                Dictionary<string, string> filtros = __SessionWEB.BuscaRealizada.Filtros;

                if (filtros.ContainsKey("VS_Ordenacao"))
                {
                    filtros["VS_Ordenacao"] = _dgvTurma.SortExpression;
                }
                else
                {
                    filtros.Add("VS_Ordenacao", _dgvTurma.SortExpression);
                }

                if (filtros.ContainsKey("VS_SortDirection"))
                {
                    filtros["VS_SortDirection"] = _dgvTurma.SortDirection.ToString();
                }
                else
                {
                    filtros.Add("VS_SortDirection", _dgvTurma.SortDirection.ToString());
                }

                __SessionWEB.BuscaRealizada = new BuscaGestao
                {
                    PaginaBusca = PaginaGestao.AtribuicaoDocentes
                    ,
                    Filtros = filtros
                };
            }

            #endregion

            string msgInformacao = string.Empty;
            if (_dgvTurma.Rows.Cast<GridViewRow>().Any(p => Convert.ToInt32(_dgvTurma.DataKeys[p.RowIndex].Values["tud_tipo"]) == (int)TurmaDisciplinaTipo.Regencia))
            {
                msgInformacao = "Configuração selecionada para regência será válida para todos os seus componentes.";
            }

            if (_dgvTurma.Rows.Cast<GridViewRow>().Any(p => Convert.ToInt32(_dgvTurma.DataKeys[p.RowIndex].Values["tud_tipo"]) == (int)TurmaDisciplinaTipo.Experiencia))
            {
                msgInformacao = (string.IsNullOrEmpty(msgInformacao) ? msgInformacao : msgInformacao + "<br />") + "Configuração selecionada para experiência será válida para todos os seus territórios.";
            }

            if (!string.IsNullOrEmpty(msgInformacao))
            {
                _lblMsgRegencia.Text = UtilBO.GetErroMessage(msgInformacao, UtilBO.TipoMensagem.Informacao);
                _lblMsgRegencia.Visible = true;
            }

            if ((__SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.Individual) &&
                (_dgvTurma.Rows.Cast<GridViewRow>().Any(p => Convert.ToBoolean(_dgvTurma.DataKeys[p.RowIndex].Values["duplaRegencia"]))))
            {
                _dgvTurma.Columns[indexColumnSegundoTitular].Visible = true;
            }
            else
            {
                _dgvTurma.Columns[indexColumnSegundoTitular].Visible = false;
            }
        }

        protected void _dgvTurma_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkTitular = (CheckBox)e.Row.FindControl("chkTitular");
                CheckBox chkSegundoTitular = (CheckBox)e.Row.FindControl("chkSegundoTitular");
                CheckBox _chkProjeto = (CheckBox)e.Row.FindControl("_chkProjeto");
                CheckBox _chkCompartilhada = (CheckBox)e.Row.FindControl("_chkCompartilhada");
                CheckBox _chkSubstituto = (CheckBox)e.Row.FindControl("_chkSubstituto");
                Label lblDocentes = (Label)e.Row.FindControl("lblDocentes");
                TextBox txtVigenciaInicio = (TextBox)e.Row.FindControl("txtVigenciaInicio");
                TextBox txtVigenciaFim = (TextBox)e.Row.FindControl("txtVigenciaFim");

                lblDocentes.Text = DataBinder.Eval(e.Row.DataItem, "docentes").ToString();
                string docenciaCompartilhada = DataBinder.Eval(e.Row.DataItem, "docenciaCompartilhada").ToString();
                if (!String.IsNullOrEmpty(docenciaCompartilhada))
                {
                    lblDocentes.Text += String.IsNullOrEmpty(lblDocentes.Text) ? docenciaCompartilhada : "<br />" + docenciaCompartilhada;
                }

                if (chkSegundoTitular != null)
                {
                    chkSegundoTitular.Visible = Convert.ToBoolean(_dgvTurma.DataKeys[e.Row.RowIndex].Values["duplaRegencia"]);
                }

                _chkProjeto.Checked = _chkCompartilhada.Checked = _chkSubstituto.Checked = false;
                _chkProjeto.Enabled = _chkSubstituto.Enabled = _chkCompartilhada.Enabled = usuarioPermissao;

                //Se for disciplina do tipo Componente Regencia então não mostra a opção
                if (Convert.ToInt32(_dgvTurma.DataKeys[e.Row.RowIndex].Values["tud_tipo"]) == (int)TurmaDisciplinaTipo.ComponenteRegencia ||
                    Convert.ToInt32(_dgvTurma.DataKeys[e.Row.RowIndex].Values["tud_tipo"]) == (int)TurmaDisciplinaTipo.TerritorioSaber)
                {
                    _chkProjeto.Visible = _chkProjeto.Checked =
                    _chkCompartilhada.Visible = _chkCompartilhada.Checked =
                    _chkSubstituto.Visible = _chkSubstituto.Checked = e.Row.Visible = false;
                }
                else
                {
                    if (aplicarNovaRegraDocenciaCompartilhada)
                    {
                        // Checa compartilhado se é uma turma disciplina relacionada com a turma disciplina compartilhada
                        _chkCompartilhada.Checked = _dgvTurma.Columns[indexColumnCompartilhada].Visible && _chkCompartilhada.Visible && _dgvTurma.DataKeys[e.Row.RowIndex].Values["tdr_id"].ToString() != "-1";
                        if (_chkCompartilhada.Checked)
                        {
                            lblInfoDocenciaCompartilhada.Visible = false;
                        }
                    }

                    //Verifica se o docente está atribuido
                    if (!string.IsNullOrEmpty(_dgvTurma.DataKeys[e.Row.RowIndex].Values["tdt_id"].ToString()))
                        for (int i = 0; i < _dgvTurma.DataKeys[e.Row.RowIndex].Values["tdt_id"].ToString().Split(',').Length; i++)
                        {
                            TUR_TurmaDocente tdtAnterior = new TUR_TurmaDocente
                            {
                                tud_id = Convert.ToInt32(_dgvTurma.DataKeys[e.Row.RowIndex].Values["tud_id"]),
                                tdt_id = Convert.ToInt32(_dgvTurma.DataKeys[e.Row.RowIndex].Values["tdt_id"].ToString().Split(',')[i])
                            };
                            TUR_TurmaDocenteBO.GetEntity(tdtAnterior);

                            if (tdtAnterior.tdt_id > 0 && tdtAnterior.IsNew)
                                throw new ValidationException("Erro ao carregar atribuição do docente.");

                            if (tdtAnterior.doc_id == ComboDocenteValor[0])
                            {
                                if (aplicarNovaRegraDocenciaCompartilhada)
                                {

                                    if (Convert.ToInt32(_dgvTurma.DataKeys[e.Row.RowIndex].Values["tdt_posicao"].ToString().Split(',')[i]) == TipoDocente.Find(f => f.tdc_id == (byte)EnumTipoDocente.Titular).tdc_posicao)
                                    {
                                        chkTitular.Checked = true;
                                        txtVigenciaInicio.Text = tdtAnterior.tdt_vigenciaInicio == new DateTime() ? string.Empty : tdtAnterior.tdt_vigenciaInicio.ToShortDateString();
                                        txtVigenciaFim.Text = tdtAnterior.tdt_vigenciaFim == new DateTime() ? string.Empty : tdtAnterior.tdt_vigenciaFim.ToShortDateString();
                                    }

                                    else if (Convert.ToInt32(_dgvTurma.DataKeys[e.Row.RowIndex].Values["tdt_posicao"].ToString().Split(',')[i]) == TipoDocente.Find(f => f.tdc_id == (byte)EnumTipoDocente.SegundoTitular).tdc_posicao)
                                    {
                                        chkSegundoTitular.Checked = true;
                                        txtVigenciaInicio.Text = tdtAnterior.tdt_vigenciaInicio == new DateTime() ? string.Empty : tdtAnterior.tdt_vigenciaInicio.ToShortDateString();
                                        txtVigenciaFim.Text = tdtAnterior.tdt_vigenciaFim == new DateTime() ? string.Empty : tdtAnterior.tdt_vigenciaFim.ToShortDateString();
                                    }

                                    else if (Convert.ToInt32(_dgvTurma.DataKeys[e.Row.RowIndex].Values["tdt_posicao"].ToString().Split(',')[i]) == TipoDocente.Find(f => f.tdc_id == (byte)EnumTipoDocente.Substituto).tdc_posicao)
                                    {
                                        _chkSubstituto.Checked = true;
                                        txtVigenciaInicio.Text = tdtAnterior.tdt_vigenciaInicio == new DateTime() ? string.Empty : tdtAnterior.tdt_vigenciaInicio.ToShortDateString();
                                        txtVigenciaFim.Text = tdtAnterior.tdt_vigenciaFim == new DateTime() ? string.Empty : tdtAnterior.tdt_vigenciaFim.ToShortDateString();
                                    }
                                    else if (Convert.ToInt32(_dgvTurma.DataKeys[e.Row.RowIndex].Values["tdt_posicao"].ToString().Split(',')[i]) == 1)
                                        _chkProjeto.Enabled = _chkProjeto.Checked =
                                        _chkCompartilhada.Enabled = _chkCompartilhada.Checked =
                                        _chkSubstituto.Enabled = _chkSubstituto.Checked = false;
                                }
                                else
                                {
                                    if (Convert.ToInt32(_dgvTurma.DataKeys[e.Row.RowIndex].Values["tdt_posicao"].ToString().Split(',')[i]) == TipoDocente.Find(f => f.tdc_id == (byte)EnumTipoDocente.Projeto).tdc_posicao)
                                        _chkProjeto.Visible = _chkProjeto.Checked = _dgvTurma.Columns[indexColumnProjeto].Visible = true;
                                    else if (Convert.ToInt32(_dgvTurma.DataKeys[e.Row.RowIndex].Values["tdt_posicao"].ToString().Split(',')[i]) == TipoDocente.Find(f => f.tdc_id == (byte)EnumTipoDocente.Substituto).tdc_posicao)
                                    {
                                        _chkSubstituto.Checked = true;
                                        txtVigenciaInicio.Text = tdtAnterior.tdt_vigenciaInicio == new DateTime() ? string.Empty : tdtAnterior.tdt_vigenciaInicio.ToShortDateString();
                                        txtVigenciaFim.Text = tdtAnterior.tdt_vigenciaFim == new DateTime() ? string.Empty : tdtAnterior.tdt_vigenciaFim.ToShortDateString();
                                    }
                                    else if (Convert.ToInt32(_dgvTurma.DataKeys[e.Row.RowIndex].Values["tdt_posicao"].ToString().Split(',')[i]) == TipoDocente.Find(f => f.tdc_id == (byte)EnumTipoDocente.Compartilhado).tdc_posicao)
                                        _chkCompartilhada.Visible = _chkCompartilhada.Checked = _dgvTurma.Columns[indexColumnCompartilhada].Visible = true;
                                    else if (Convert.ToInt32(_dgvTurma.DataKeys[e.Row.RowIndex].Values["tdt_posicao"].ToString().Split(',')[i]) == 1)
                                        _chkProjeto.Enabled = _chkProjeto.Checked =
                                        _chkCompartilhada.Enabled = _chkCompartilhada.Checked =
                                        _chkSubstituto.Enabled = _chkSubstituto.Checked = false;
                                }
                            }
                            //Comentando essa parte que desabilita o chkbox permite-se gravar outro docente na mesma posição (substituindo o docente que estiver nessa posição)
                            //else
                            //{
                            //    if (Convert.ToInt32(_dgvTurma.DataKeys[e.Row.RowIndex].Values["tdt_posicao"].ToString().Split(',')[i]) == TipoDocente.Find(f => f.tdc_id == (byte)EnumTipoDocente.Projeto).tdc_posicao)
                            //        _chkProjeto.Enabled = false;
                            //    //else if (Convert.ToInt32(_dgvTurma.DataKeys[e.Row.RowIndex].Values["tdt_posicao"].ToString().Split(',')[i]) == TipoDocente.Find(f => f.tdc_id == (byte)EnumTipoDocente.Substituto).tdc_posicao)
                            //    //_chkSubstituto.Enabled = false;
                            //    else if (Convert.ToInt32(_dgvTurma.DataKeys[e.Row.RowIndex].Values["tdt_posicao"].ToString().Split(',')[i]) == TipoDocente.Find(f => f.tdc_id == (byte)EnumTipoDocente.Compartilhado).tdc_posicao)
                            //        _chkCompartilhada.Enabled = false;
                            //}
                        }
                }

                //Carrega a quantidade de aulas da disciplina
                Label _lbltud_cargaHorariaSemanal = (Label)e.Row.FindControl("_lbltud_cargaHorariaSemanal");

                //Se for conceito global não mostra a quantidade
                if (Convert.ToInt32(_dgvTurma.DataKeys[e.Row.RowIndex].Values["tud_tipo"]) == (int)TurmaDisciplinaTipo.DisciplinaPrincipal)
                    _lbltud_cargaHorariaSemanal.Text = "-";
                else
                    _lbltud_cargaHorariaSemanal.Text = _dgvTurma.DataKeys[e.Row.RowIndex].Values["tud_cargaHorariaSemanal"].ToString();
            }
        }

        protected void _chkProjeto_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox _chkProjeto = (CheckBox)sender;
            if (_chkProjeto.Checked)
            {
                CheckBox _chkCompartilhada = (CheckBox)_chkProjeto.NamingContainer.FindControl("_chkCompartilhada");
                //CheckBox _chkSubstituto = (CheckBox)_chkProjeto.NamingContainer.FindControl("_chkSubstituto");
                _chkCompartilhada.Checked = false;//_chkSubstituto.Checked = 
            }
        }

        protected void _chkCompartilhada_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox _chkCompartilhada = (CheckBox)sender;
            if (_chkCompartilhada.Checked)
            {
                CheckBox _chkProjeto = (CheckBox)_chkCompartilhada.NamingContainer.FindControl("_chkProjeto");
                //CheckBox _chkSubstituto = (CheckBox)_chkCompartilhada.NamingContainer.FindControl("_chkSubstituto");
                _chkProjeto.Checked = false;//_chkSubstituto.Checked = 
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            UCCTurma1.SelectedIndex = 0;
            fdsResultado.Visible = false;
            lblInfoDocenciaCompartilhada.Visible = false;
            _lblMsgRegencia.Visible = false;
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            //Exibi a msg de responsabilidade das informações
            if (esc_terceirizada && (__SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.Individual))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "ExibirConfirmacaoPadrão", "$(document).ready(function(){ $('#divConfirmacao').dialog('open'); });", true);
            }
            else
            {
                Salvar();
            }
        }

        protected void btnBuscaDocente_Click(object sender, EventArgs e)
        {
            try
            {
                UCBuscaDocenteEscola1.Limpar();
                UCBuscaDocenteEscola1.VisibleDisciplina = true;

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "AbrirBuscaDocente", "$('#divBuscaDocente').dialog('option', 'title', '"
                                                    + GetGlobalResourceObject("Academico", "RecursosHumanos.AtribuicaoDocentes.Busca.divBuscaDocente.title").ToString()
                                                    + "');  $('#divBuscaDocente').dialog('open');", true);
            }
            catch (ArgumentException ex)
            {
                _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os docentes.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void UCBuscaDocenteEscola1_ReturnValues(IDictionary<string, object> parameters)
        {
            try
            {
                txtNomeDocente.Text = parameters["pes_nome"].ToString();
                hdnDocente.Value = String.Format("{0};{1};{2};{3};", parameters["doc_id"], parameters["col_id"], parameters["crg_id"], parameters["coc_id"]);
                UCComboDocente1_IndexChanged();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados do docente.", UtilBO.TipoMensagem.Erro);
            }
            finally
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "FecharBuscaDocente", "$('#divBuscaDocente').dialog('close');", true);
            }
        }

        #endregion Eventos

    }
}