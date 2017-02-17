using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GestaoEscolar.WebControls.EscolaOrigem;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;

namespace GestaoEscolar.WebControls.Movimentacao
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using MSTech.Validation.Exceptions;

    public partial class UCMovimentacao : MotherUserControl
    {
        #region Constantes

        private const int indiceColunaEscolaAnterior = 2;
        private const int indiceColunaEscolaAtual = 3;

        #endregion Constantes

        #region Propriedades

        /// <summary>
        /// Seta se é obrigatório o usuário informar a movimentação.
        /// </summary>
        public bool MovimentacaoObrigatoria
        {
            set
            {
                UCComboTipoMovimentacaoMatricula1.Obrigatorio = value;
            }
        }

        /// <summary>
        /// Retorna se o combo de tipo de movimentação tem valor selecionado, caso seja
        /// obrigatório.
        /// </summary>
        public bool IsValidComboTipoMovimentacao
        {
            get
            {
                return !Visible || UCComboTipoMovimentacaoMatricula1.IsValid;
            }
        }
        

        /// <summary>
        /// User control de busca e cadastro de escola de origem.
        /// </summary>
        public UCEscolaOrigem ucEscolaOrigem
        {
            private get;
            set;
        }

        /// <summary>
        /// User control de cadastro de Cidades (estará na página).
        /// </summary>
        public WebControls_Cidade_UCCadastroCidade UCCidades1
        {
            private get;
            set;
        }

        /// <summary>
        /// UpdatePanel dentro do cadastro de cidades (estará na página).
        /// </summary>
        public UpdatePanel UpnCadastroCidades
        {
            private get;
            set;
        }

        /// <summary>
        /// Guarda o ID do Curriculo do Aluno Anterior
        /// </summary>
        public int VS_alc_idAnterior
        {
            get { return Convert.ToInt32(ViewState["VS_alc_idAnterior"]); }
            set { ViewState["VS_alc_idAnterior"] = value; }
        }

        /// <summary>
        /// Guarda se o curso é exclusivo deficiente
        /// </summary>
        public bool VS_cur_exclusivoDefAnterior
        {
            get
            {
                if (ViewState["VS_cur_exclusivoDefAnterior"] == null)
                {
                    return false;
                }

                return Convert.ToBoolean(ViewState["VS_cur_exclusivoDefAnterior"]);
            }

            set
            {
                ViewState["VS_cur_exclusivoDefAnterior"] = value;
            }
        }

        /// <summary>
        /// Guarda o tipo de escola
        /// 1 - Movimentação - Origem
        /// 2 - Movimentação - Destino
        /// </summary>
        public byte VS_tipoEscola
        {
            get { return Convert.ToByte(ViewState["VS_tipoEscola"]); }
            set { ViewState["VS_tipoEscola"] = value; }
        }

        /// <summary>
        /// Retorna se algum tipo de movimentação foi selecionada
        /// </summary>
        public bool TemMovimentacao
        {
            get
            {
                return UCComboTipoMovimentacaoMatricula1.Valor[0] > 0;
            }
        }

        /// <summary>
        /// Indica se o aluno é excedente
        /// </summary>
        public bool VS_Excedente
        {
            get
            {
                if (ViewState["VS_Excedente"] == null)
                {
                    return false;
                }

                return Convert.ToBoolean(ViewState["VS_Excedente"]);
            }

            set
            {
                ViewState["VS_Excedente"] = value;
            }
        }

        /// <summary>
        /// Guarda o ID da escola do excedente
        /// </summary>
        public int VS_Esc_id_Excedente
        {
            get
            {
                if (ViewState["VS_Esc_id_Excedente"] == null)
                {
                    return -1;
                }

                return Convert.ToInt32(ViewState["VS_Esc_id_Excedente"]);
            }

            set
            {
                ViewState["VS_Esc_id_Excedente"] = value;
            }
        }

        /// <summary>
        /// Guarda o ID da unidade do excedente
        /// </summary>
        public int VS_Uni_id_Excedente
        {
            get
            {
                if (ViewState["VS_Uni_id_Excedente"] == null)
                {
                    return -1;
                }

                return Convert.ToInt32(ViewState["VS_Uni_id_Excedente"]);
            }

            set
            {
                ViewState["VS_Uni_id_Excedente"] = value;
            }
        }

        /// <summary>
        /// Guarda o ID do curso do excedente
        /// </summary>
        public int VS_Cur_id_Excedente
        {
            get
            {
                if (ViewState["VS_Cur_id_Excedente"] == null)
                {
                    return -1;
                }

                return Convert.ToInt32(ViewState["VS_Cur_id_Excedente"]);
            }

            set
            {
                ViewState["VS_Cur_id_Excedente"] = value;
            }
        }

        /// <summary>
        /// Guarda o ID do currículo do excedente
        /// </summary>
        public int VS_Crr_id_Excedente
        {
            get
            {
                if (ViewState["VS_Crr_id_Excedente"] == null)
                {
                    return -1;
                }

                return Convert.ToInt32(ViewState["VS_Crr_id_Excedente"]);
            }

            set
            {
                ViewState["VS_Crr_id_Excedente"] = value;
            }
        }

        /// <summary>
        /// Guarda o ID do período do excedente
        /// </summary>
        public int VS_Crp_id_Excedente
        {
            get
            {
                if (ViewState["VS_Crp_id_Excedente"] == null)
                {
                    return -1;
                }

                return Convert.ToInt32(ViewState["VS_Crp_id_Excedente"]);
            }

            set
            {
                ViewState["VS_Crp_id_Excedente"] = value;
            }
        }

        private int VS_Ttn_id_Excedente
        {
            get
            {
                if (ViewState["VS_Ttn_id_Excedente"] == null)
                {
                    return -1;
                }

                return Convert.ToInt32(ViewState["VS_Ttn_id_Excedente"]);
            }

            set
            {
                ViewState["VS_Ttn_id_Excedente"] = value;
            }
        }
        
        private bool dataVazia;

        /// <summary>
        /// Retorna o tmo_tipoMovimento do item selecionado dos tipos de movimentação.
        /// </summary>
        public MTR_TipoMovimentacaoTipoMovimento TipoMovimentoSelecionado
        {
            get { return (MTR_TipoMovimentacaoTipoMovimento)UCComboTipoMovimentacaoMatricula1.Valor[1]; }
        }

        /// <summary>
        /// Guarda o ID do aluno
        /// </summary>
        private long VS_alu_id
        {
            get { return Convert.ToInt64(ViewState["VS_alu_id"]); }
            set { ViewState["VS_alu_id"] = value; }
        }

        /// <summary>
        /// Guarda o ID da movimentação
        /// </summary>
        private int VS_mov_id
        {
            get { return Convert.ToInt32(ViewState["VS_mov_id"]); }
            set { ViewState["VS_mov_id"] = value; }
        }

        /// <summary>
        /// Guarda o ID dos dados adicionais da movimentação
        /// </summary>
        private int VS_mda_id
        {
            get { return Convert.ToInt32(ViewState["VS_mda_id"]); }
            set { ViewState["VS_mda_id"] = value; }
        }

        /// <summary>
        /// Guarda o ID do Matrícula do Aluno Anterior
        /// </summary>
        private int VS_mtu_idAnterior
        {
            get { return Convert.ToInt32(ViewState["VS_mtu_idAnterior"]); }
            set { ViewState["VS_mtu_idAnterior"] = value; }
        }

        /// <summary>
        /// Guarda o ID do Matrícula do Aluno Anterior
        /// </summary>
        private int VS_alc_idAtual
        {
            get { return Convert.ToInt32(ViewState["VS_alc_idAtual"]); }
            set { ViewState["VS_alc_idAtual"] = value; }
        }

        /// <summary>
        /// Guarda o ID do Matrícula do Aluno Anterior
        /// </summary>
        private int VS_ala_idAtual
        {
            get { return Convert.ToInt32(ViewState["VS_ala_idAtual"]); }
            set { ViewState["VS_ala_idAtual"] = value; }
        }

        /// <summary>
        /// Guarda o ID do Curriculo do Aluno Atual
        /// </summary>
        private int VS_mtu_idAtual
        {
            get { return Convert.ToInt32(ViewState["VS_mtu_idAtual"]); }
            set { ViewState["VS_mtu_idAtual"] = value; }
        }

        /// <summary>
        /// Propriedade que retorna se está configurado para filtrar por UA.
        /// </summary>
        private bool VS_FiltroEscola
        {
            get { return Convert.ToBoolean(ViewState["VS_FiltroEscola"]); }
            set { ViewState["VS_FiltroEscola"] = value; }
        }

        /// <summary>
        /// Guarda o ID do tipo de unidade administrativa superior para filtrar escolas
        /// </summary>
        private Guid VS_tua_id
        {
            get { return new Guid(ViewState["VS_tua_id"].ToString()); }
            set { ViewState["VS_tua_id"] = value; }
        }

        /// <summary>
        /// Guarda o ID da escola
        /// </summary>
        private int VS_esc_idAnterior
        {
            get { return Convert.ToInt32(ViewState["VS_esc_idAnterior"]); }
            set { ViewState["VS_esc_idAnterior"] = value; }
        }

        /// <summary>
        /// Guarda o ID da unidade da escola
        /// </summary>
        private int VS_uni_idAnterior
        {
            get { return Convert.ToInt32(ViewState["VS_uni_idAnterior"]); }
            set { ViewState["VS_uni_idAnterior"] = value; }
        }

        /// <summary>
        /// Guarda o ID do curso
        /// </summary>
        private int VS_cur_idAnterior
        {
            get { return Convert.ToInt32(ViewState["VS_cur_idAnterior"]); }
            set { ViewState["VS_cur_idAnterior"] = value; }
        }

        /// <summary>
        /// Guarda o ID do curriculo
        /// </summary>
        private int VS_crr_idAnterior
        {
            get { return Convert.ToInt32(ViewState["VS_crr_idAnterior"]); }
            set { ViewState["VS_crr_idAnterior"] = value; }
        }

        /// <summary>
        /// Guarda o ID do curriculo
        /// </summary>
        private int VS_crp_idAnterior
        {
            get { return Convert.ToInt32(ViewState["VS_crp_idAnterior"]); }
            set { ViewState["VS_crp_idAnterior"] = value; }
        }

        /// <summary>
        /// Guarda o ID do curriculo
        /// </summary>
        private long VS_tur_idAnterior
        {
            get { return Convert.ToInt64(ViewState["VS_tur_idAnterior"]); }
            set { ViewState["VS_tur_idAnterior"] = value; }
        }

        /// <summary>
        /// Guarda o número da avaliação
        /// </summary>
        private int VS_ala_idAnterior
        {
            get { return Convert.ToInt32(ViewState["VS_ala_idAnterior"]); }
            set { ViewState["VS_ala_idAnterior"] = value; }
        }

        /// <summary>
        /// Guarda o número da avaliação da turma
        /// </summary>
        private int VS_tca_idAnterior
        {
            get { return Convert.ToInt32(ViewState["VS_tca_idAnterior"]); }
            set { ViewState["VS_tca_idAnterior"] = value; }
        }

        /// <summary>
        /// Guarda o ID do cidade dos dados adicionais da movimentação
        /// </summary>
        private Guid VS_cid_idAnterior
        {
            get
            {
                if (ViewState["VS_cid_idAnterior"] != null)
                {
                    return new Guid(ViewState["VS_cid_idAnterior"].ToString());
                }

                return Guid.Empty;
            }

            set
            {
                ViewState["VS_cid_idAnterior"] = value;
            }
        }

        /// <summary>
        /// Guarda o ID da unidade federativa dos dados adicionais da movimentação
        /// </summary>
        private Guid VS_unf_idAnterior
        {
            get
            {
                if (ViewState["VS_unf_idAnterior"] != null)
                {
                    return new Guid(ViewState["VS_unf_idAnterior"].ToString());
                }

                return Guid.Empty;
            }

            set
            {
                ViewState["VS_unf_idAnterior"] = value;
            }
        }

        /// <summary>
        /// Data/Hora que o usuário acessou a tela
        /// </summary>
        private DateTime VS_dataAcesso
        {
            get { return Convert.ToDateTime(ViewState["VS_dataAcesso"]); }
            set { ViewState["VS_dataAcesso"] = value; }
        }

        /// <summary>
        /// Flag que indica se o user control está sendo utilizado para matricular alunos no período
        /// antes do fechamento de matrícula (durante a renovação).
        /// </summary>
        private bool VS_MovimentacaoAntesFechamentoMatricula
        {
            get
            {
                if (ViewState["VS_MovimentacaoAntesFechamentoMatricula"] == null)
                {
                    return false;
                }

                return Convert.ToBoolean(ViewState["VS_MovimentacaoAntesFechamentoMatricula"]);
            }
            set
            {
                ViewState["VS_MovimentacaoAntesFechamentoMatricula"] = value;
            }
        }

        /// <summary>
        /// Guarda a ordem do curriculo período
        /// </summary>
        private int VS_crp_ordem
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_crp_ordem"] ?? "-1");
            }

            set
            {
                ViewState["VS_crp_ordem"] = value;
            }
        }

        /// <summary>
        /// ViewState que armazena o calendário da última matrícula do aluno.
        /// </summary>
        private int VS_cal_anoAnterior
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_cal_anoAnterior"] ?? "0");
            }

            set
            {
                ViewState["VS_cal_anoAnterior"] = value;
            }
        }

        #endregion Propriedades

        #region Delegates

        /// <summary>
        /// Trata Alteração de index do Tipo movimentacao
        /// </summary>
        protected void UCComboTipoMovimentacaoMatricula1_IndexChanged()
        {
            MTR_TipoMovimentacaoTipoMovimento tipoMovimento;
            if (VS_MovimentacaoAntesFechamentoMatricula)
            {
                tipoMovimento = UCTipoMovimentacao_MatriculaAntesFechamento1.Tmo_tipoMovimento;
            }
            else
            {
                int tipo = UCComboTipoMovimentacaoMatricula1.Valor[1];
                if (tipo < 0)
                    tipo = 0;
                tipoMovimento = (MTR_TipoMovimentacaoTipoMovimento)tipo;
            }

            // Só limpa os valores dos combos se for uma inclusão de movimentação
            if (VS_mov_id <= 0)
            {
                ACA_AlunoCurriculo entityAlunoCurriculo;
                ACA_AlunoCurriculoBO.CarregaUltimoCurriculo(VS_alu_id, out entityAlunoCurriculo);
                txtMatriculaAnterior.Text = entityAlunoCurriculo.alc_matricula;
                txtMatriculaEstadualAnterior.Text = entityAlunoCurriculo.alc_matriculaEstadual;

                if (MTR_TipoMovimentacaoBO.VerificarMovimentacaoInclusao((int)tipoMovimento))
                {
                    LoadInicialEscolaDados(false);
                    txtMatriculaAtual.Text = entityAlunoCurriculo.alc_matricula;
                    txtMatriculaEstadualAtual.Text = entityAlunoCurriculo.alc_matriculaEstadual;
                }
                else if (MTR_TipoMovimentacaoBO.VerificarMovimentacaoRealocacao((int)tipoMovimento))
                {
                    switch (tipoMovimento)
                    {
                        case MTR_TipoMovimentacaoTipoMovimento.TransferenciaPropriaRede:
                        case MTR_TipoMovimentacaoTipoMovimento.Remanejamento:
                        case MTR_TipoMovimentacaoTipoMovimento.MudancaModalidaeEnsino:
                            LoadInicialEscolaDados(true);
                            break;
                        case MTR_TipoMovimentacaoTipoMovimento.RegularizacaoRecurso:
                        case MTR_TipoMovimentacaoTipoMovimento.Adequacao:
                        case MTR_TipoMovimentacaoTipoMovimento.Reclassificacao:
                        case MTR_TipoMovimentacaoTipoMovimento.MudancaTurma:
                        case MTR_TipoMovimentacaoTipoMovimento.MudancaBlocoPEJA:
                            LoadInicialEscolaDados(false);
                            break;
                    }
                }
                else if (MTR_TipoMovimentacaoBO.VerificarMovimentacaoExclusao((int)tipoMovimento))
                {
                    LoadInicialEscolaDados(true);
                }
                else if (MTR_TipoMovimentacaoBO.VerificarMovimentacaoRenovacao((int)tipoMovimento))
                {
                    LoadInicialEscolaDados(false);
                }

                if (tipoMovimento > 0)
                {
                    divDataMovimentacao.Visible = !VS_MovimentacaoAntesFechamentoMatricula;
                    if (!dataVazia)
                    {
                        string parPreencherData = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.PREENCHER_DATA_MOVIMENTACAO_AUTOMATICAMENTE, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                        txtDataMovimentacao.Text =
                            (string.IsNullOrEmpty(parPreencherData) || ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PREENCHER_DATA_MOVIMENTACAO_AUTOMATICAMENTE, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
                            ? DateTime.Now.ToString("dd/MM/yyyy")
                            : string.Empty;
                    }
                    else
                        txtDataMovimentacao.Text = string.Empty;
                }
                else
                {
                    divDataMovimentacao.Visible = false;
                    txtDataMovimentacao.Text = string.Empty;
                }
            }
            else
            {
                divDataMovimentacao.Visible = false;
                txtDataMovimentacao.Text = string.Empty;
            }

            EsconderCamposMovimentacao();

            if (VS_mov_id <= 0 && (tipoMovimento == MTR_TipoMovimentacaoTipoMovimento.TransferenciaDaRedeParticular ||
                tipoMovimento == MTR_TipoMovimentacaoTipoMovimento.TransferenciaDaRedePublicaOutrosEstadosFederal ||
                tipoMovimento == MTR_TipoMovimentacaoTipoMovimento.TransferenciaDaRedePublicaOutrosMunicipios))
            {
                divFrequencia.Visible = !VS_MovimentacaoAntesFechamentoMatricula;
            }

            string tre_id;

            switch (tipoMovimento)
            {
                case MTR_TipoMovimentacaoTipoMovimento.MatriculaInicial:
                case MTR_TipoMovimentacaoTipoMovimento.RenovacaoInicial:
                case MTR_TipoMovimentacaoTipoMovimento.RenovacaoManual:
                    VS_tipoEscola = Convert.ToByte(ACA_AlunoEscolaOrigemTipoCadastro.MovimentacaoOrigem);

                    cpvUnidadeAdministrativa.Visible = VS_FiltroEscola;
                    cpvUnidadeEscola.Visible = true;
                    divEscolaPropriaRede.Visible = true;

                    UCComboCursoCurriculo1.PermiteEditar = false;
                    UCComboCursoCurriculo1.ValidationGroup = "Aluno";
                    UCComboCursoCurriculo1.Obrigatorio = true;

                    UCComboCurriculoPeriodo1.PermiteEditar = false;
                    UCComboCurriculoPeriodo1.ValidationGroup = "Aluno";
                    UCComboCurriculoPeriodo1.Obrigatorio = true;
                    divCurso.Visible = true;
                    divPeriodo.Visible = true;

                    UCComboCurriculoPeriodoAvaliacao1.Obrigatorio = true;
                    UCComboCurriculoPeriodoAvaliacao1.ValidationGroup = "Aluno";

                    UCComboTurma1.PermiteEditar = false;
                    UCComboTurma1.ValidationGroup = "Aluno";
                    UCComboTurma1.Obrigatorio = !VS_MovimentacaoAntesFechamentoMatricula;
                    divTurma.Visible = !VS_MovimentacaoAntesFechamentoMatricula;

                    divMatriculaNovo.Visible = true;
                    divAvaliacao.Visible = tipoMovimento != MTR_TipoMovimentacaoTipoMovimento.RenovacaoManual;

                    rfvMatriculaAtual.Visible = txtMatriculaAtual.Visible && lblMatriculaAtual.Text.Contains("*");

                    break;
                case MTR_TipoMovimentacaoTipoMovimento.TransferenciaDaRedeParticular:
                    VS_tipoEscola = Convert.ToByte(ACA_AlunoEscolaOrigemTipoCadastro.MovimentacaoOrigem);

                    cpvUnidadeAdministrativa.Visible = VS_FiltroEscola;
                    cpvUnidadeEscola.Visible = true;
                    divEscolaPropriaRede.Visible = true;

                    UCComboCursoCurriculo1.PermiteEditar = false;
                    UCComboCursoCurriculo1.ValidationGroup = "Aluno";
                    UCComboCursoCurriculo1.Obrigatorio = true;

                    UCComboCurriculoPeriodo1.PermiteEditar = false;
                    UCComboCurriculoPeriodo1.ValidationGroup = "Aluno";
                    UCComboCurriculoPeriodo1.Obrigatorio = true;
                    divCurso.Visible = true;
                    divPeriodo.Visible = true;

                    UCComboCurriculoPeriodoAvaliacao1.Obrigatorio = true;
                    UCComboCurriculoPeriodoAvaliacao1.ValidationGroup = "Aluno";

                    UCComboTurma1.PermiteEditar = false;
                    UCComboTurma1.ValidationGroup = "Aluno";
                    UCComboTurma1.Obrigatorio = !VS_MovimentacaoAntesFechamentoMatricula;
                    divTurma.Visible = !VS_MovimentacaoAntesFechamentoMatricula;

                    divMatriculaNovo.Visible = true;

                    tre_id = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_REDE_ENSINO_PARTICULAR, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    divEscolaDestino.Visible = !string.IsNullOrEmpty(tre_id);

                    lblInfoEscolaDestino.Text = "Para selecionar a escola de origem, é necessário configurar o tipo de rede de ensino particular nas configurações do sistema.";
                    lblInfoEscolaDestino.Text = UtilBO.GetErroMessage(lblInfoEscolaDestino.Text, UtilBO.TipoMensagem.Informacao);
                    lblInfoEscolaDestino.Visible = string.IsNullOrEmpty(tre_id);

                    break;
                case MTR_TipoMovimentacaoTipoMovimento.TransferenciaDaRedePublicaOutrosMunicipios:
                    VS_tipoEscola = Convert.ToByte(ACA_AlunoEscolaOrigemTipoCadastro.MovimentacaoOrigem);

                    cpvUnidadeAdministrativa.Visible = VS_FiltroEscola;
                    cpvUnidadeEscola.Visible = true;
                    divEscolaPropriaRede.Visible = true;

                    UCComboCursoCurriculo1.PermiteEditar = false;
                    UCComboCursoCurriculo1.ValidationGroup = "Aluno";
                    UCComboCursoCurriculo1.Obrigatorio = true;

                    UCComboCurriculoPeriodo1.PermiteEditar = false;
                    UCComboCurriculoPeriodo1.ValidationGroup = "Aluno";
                    UCComboCurriculoPeriodo1.Obrigatorio = true;
                    divCurso.Visible = true;
                    divPeriodo.Visible = true;

                    UCComboCurriculoPeriodoAvaliacao1.Obrigatorio = true;
                    UCComboCurriculoPeriodoAvaliacao1.ValidationGroup = "Aluno";

                    UCComboTurma1.PermiteEditar = false;
                    UCComboTurma1.ValidationGroup = "Aluno";
                    UCComboTurma1.Obrigatorio = !VS_MovimentacaoAntesFechamentoMatricula;
                    divTurma.Visible = !VS_MovimentacaoAntesFechamentoMatricula;

                    divMatriculaNovo.Visible = true;
                    divMunicipio.Visible = true;
                    lblMunicipio.Text += " *";
                    rfvMunicipio.Visible = true;

                    tre_id = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_REDE_ENSINO_PUBLICA_OUTROS_MUNICIPIOS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    divEscolaDestino.Visible = !string.IsNullOrEmpty(tre_id);

                    lblInfoEscolaDestino.Text = "Para selecionar a escola de origem, é necessário configurar o tipo de rede de ensino de outros municípios nas configurações do sistema.";
                    lblInfoEscolaDestino.Text = UtilBO.GetErroMessage(lblInfoEscolaDestino.Text, UtilBO.TipoMensagem.Informacao);
                    lblInfoEscolaDestino.Visible = string.IsNullOrEmpty(tre_id);

                    break;
                case MTR_TipoMovimentacaoTipoMovimento.TransferenciaDaRedePublicaOutrosEstadosFederal:
                    VS_tipoEscola = Convert.ToByte(ACA_AlunoEscolaOrigemTipoCadastro.MovimentacaoOrigem);

                    cpvUnidadeAdministrativa.Visible = VS_FiltroEscola;
                    cpvUnidadeEscola.Visible = true;
                    divEscolaPropriaRede.Visible = true;

                    UCComboCursoCurriculo1.PermiteEditar = false;
                    UCComboCursoCurriculo1.ValidationGroup = "Aluno";
                    UCComboCursoCurriculo1.Obrigatorio = true;

                    UCComboCurriculoPeriodo1.PermiteEditar = false;
                    UCComboCurriculoPeriodo1.ValidationGroup = "Aluno";
                    UCComboCurriculoPeriodo1.Obrigatorio = true;
                    divCurso.Visible = true;
                    divPeriodo.Visible = true;

                    UCComboCurriculoPeriodoAvaliacao1.Obrigatorio = true;
                    UCComboCurriculoPeriodoAvaliacao1.ValidationGroup = "Aluno";

                    UCComboTurma1.PermiteEditar = false;
                    UCComboTurma1.ValidationGroup = "Aluno";
                    UCComboTurma1.Obrigatorio = !VS_MovimentacaoAntesFechamentoMatricula;
                    divTurma.Visible = !VS_MovimentacaoAntesFechamentoMatricula;

                    divMatriculaNovo.Visible = true;
                    divUnidadeFederativa.Visible = true;

                    tre_id = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_REDE_ENSINO_PUBLICA_OUTROS_ESTADOS_FEDERAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    divEscolaDestino.Visible = !string.IsNullOrEmpty(tre_id);

                    lblInfoEscolaDestino.Text = "Para selecionar a escola de origem, é necessário configurar o tipo de rede de ensino de outros estados/federal nas configurações do sistema.";
                    lblInfoEscolaDestino.Text = UtilBO.GetErroMessage(lblInfoEscolaDestino.Text, UtilBO.TipoMensagem.Informacao);
                    lblInfoEscolaDestino.Visible = string.IsNullOrEmpty(tre_id);

                    break;
                case MTR_TipoMovimentacaoTipoMovimento.Reconducao:
                    VS_tipoEscola = Convert.ToByte(ACA_AlunoEscolaOrigemTipoCadastro.MovimentacaoOrigem);

                    cpvUnidadeAdministrativa.Visible = VS_FiltroEscola;
                    cpvUnidadeEscola.Visible = true;
                    divEscolaPropriaRede.Visible = true;

                    UCComboCursoCurriculo1.PermiteEditar = false;
                    UCComboCursoCurriculo1.ValidationGroup = "Aluno";
                    UCComboCursoCurriculo1.Obrigatorio = true;

                    UCComboCurriculoPeriodo1.PermiteEditar = false;
                    UCComboCurriculoPeriodo1.ValidationGroup = "Aluno";
                    UCComboCurriculoPeriodo1.Obrigatorio = true;
                    divCurso.Visible = true;
                    divPeriodo.Visible = true;

                    UCComboCurriculoPeriodoAvaliacao1.Obrigatorio = true;
                    UCComboCurriculoPeriodoAvaliacao1.ValidationGroup = "Aluno";

                    UCComboTurma1.PermiteEditar = false;
                    UCComboTurma1.ValidationGroup = "Aluno";
                    UCComboTurma1.Obrigatorio = !VS_MovimentacaoAntesFechamentoMatricula;
                    divTurma.Visible = !VS_MovimentacaoAntesFechamentoMatricula;

                    divMatriculaNovo.Visible = true;

                    break;
                case MTR_TipoMovimentacaoTipoMovimento.TransferenciaPropriaRede:
                    cpvUnidadeAdministrativa.Visible = VS_FiltroEscola;
                    cpvUnidadeEscola.Visible = true;
                    divEscolaPropriaRede.Visible = true;

                    UCComboCursoCurriculo1.PermiteEditar = false;
                    UCComboCursoCurriculo1.ValidationGroup = "Aluno";
                    UCComboCursoCurriculo1.Obrigatorio = true;

                    UCComboCurriculoPeriodo1.PermiteEditar = false;
                    UCComboCurriculoPeriodo1.ValidationGroup = "Aluno";
                    UCComboCurriculoPeriodo1.Obrigatorio = true;

                    divCurso.Visible = true;
                    divPeriodo.Visible = true;

                    divRespLancNota.Visible = !MTR_MovimentacaoBO.VerificaEfetivacaoEscolaOrigem(VS_tur_idAnterior, DateTime.Now.Date, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                    pnlResponsavelTransferencia.Visible = true;

                    break;
                case MTR_TipoMovimentacaoTipoMovimento.MudancaModalidaeEnsino:
                    cpvUnidadeAdministrativa.Visible = VS_FiltroEscola;
                    cpvUnidadeEscola.Visible = true;
                    divEscolaPropriaRede.Visible = true;

                    UCComboCursoCurriculo1.PermiteEditar = false;
                    UCComboCursoCurriculo1.ValidationGroup = "Aluno";
                    UCComboCursoCurriculo1.Obrigatorio = true;

                    UCComboCurriculoPeriodo1.PermiteEditar = false;
                    UCComboCurriculoPeriodo1.ValidationGroup = "Aluno";
                    UCComboCurriculoPeriodo1.Obrigatorio = true;
                    divCurso.Visible = true;
                    divPeriodo.Visible = true;

                    break;
                case MTR_TipoMovimentacaoTipoMovimento.Remanejamento:
                    cpvUnidadeAdministrativa.Visible = VS_FiltroEscola;
                    cpvUnidadeEscola.Visible = true;
                    divEscolaPropriaRede.Visible = true;
                    divRespLancNota.Visible = !MTR_MovimentacaoBO.VerificaEfetivacaoEscolaOrigem(VS_tur_idAnterior, DateTime.Now.Date, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                    pnlResponsavelTransferencia.Visible = true;

                    UCComboCursoCurriculo1.PermiteEditar = false;
                    UCComboCursoCurriculo1.ValidationGroup = "Aluno";
                    UCComboCursoCurriculo1.Obrigatorio = true;

                    UCComboCurriculoPeriodo1.PermiteEditar = false;
                    UCComboCurriculoPeriodo1.ValidationGroup = "Aluno";
                    UCComboCurriculoPeriodo1.Obrigatorio = true;
                    divCurso.Visible = true;
                    divPeriodo.Visible = true;

                    break;
                case MTR_TipoMovimentacaoTipoMovimento.Adequacao:
                    UCComboCursoCurriculo1.PermiteEditar = true;
                    UCComboCursoCurriculo1.ValidationGroup = "Aluno";
                    UCComboCursoCurriculo1.CancelSelect = false;

                    // Se for uma nova movimentação carrega apenas os cursos ativos
                    // Se for uma alteração de movimentação, carrega todos os cursos
                    byte cur_situacao = Convert.ToByte(VS_mov_id > 0 ? 0 : 1);
                    // Alterado para carregar todos as etapas de ensino ativas do sistema
                    UCComboCursoCurriculo1.CarregarCursoCurriculoSituacao(cur_situacao);
                    UCComboCursoCurriculo1.Obrigatorio = true;

                    UCComboCurriculoPeriodo1.PermiteEditar = false;
                    UCComboCurriculoPeriodo1.ValidationGroup = "Aluno";
                    UCComboCurriculoPeriodo1.Obrigatorio = true;
                    divCurso.Visible = true;
                    divPeriodo.Visible = true;

                    UCComboCurriculoPeriodoAvaliacao1.Obrigatorio = true;
                    UCComboCurriculoPeriodoAvaliacao1.ValidationGroup = "Aluno";

                    UCComboTurma1.PermiteEditar = false;
                    UCComboTurma1.ValidationGroup = "Aluno";
                    UCComboTurma1.Obrigatorio = !VS_MovimentacaoAntesFechamentoMatricula;
                    divTurma.Visible = !VS_MovimentacaoAntesFechamentoMatricula;

                    break;
                case MTR_TipoMovimentacaoTipoMovimento.Reclassificacao:
                    UCComboCurriculoPeriodo1.PermiteEditar = true;
                    UCComboCurriculoPeriodo1.ValidationGroup = "Aluno";
                    UCComboCurriculoPeriodo1.Obrigatorio = true;
                    UCComboCurriculoPeriodo1.CancelSelect = false;
                    // Alterado para carregar todos os grupamentos oferecidos para o curso                    
                    UCComboCurriculoPeriodo1._Load(VS_cur_idAnterior, VS_crr_idAnterior);
                    divPeriodo.Visible = true;

                    UCComboCurriculoPeriodoAvaliacao1.Obrigatorio = true;
                    UCComboCurriculoPeriodoAvaliacao1.ValidationGroup = "Aluno";

                    UCComboTurma1.PermiteEditar = false;
                    UCComboTurma1.ValidationGroup = "Aluno";
                    UCComboTurma1.Obrigatorio = !VS_MovimentacaoAntesFechamentoMatricula;
                    divTurma.Visible = !VS_MovimentacaoAntesFechamentoMatricula;

                    break;
                case MTR_TipoMovimentacaoTipoMovimento.MudancaTurma:
                    UCComboTurma1.CancelSelect = false;
                    UCComboTurma1.CarregarTurmasCursosEquivalentes(VS_esc_idAnterior, VS_uni_idAnterior, VS_cur_idAnterior, VS_crr_idAnterior, VS_crp_idAnterior);
                    UCComboTurma1.PermiteEditar = true;
                    UCComboTurma1.ValidationGroup = "Aluno";
                    UCComboTurma1.Obrigatorio = !VS_MovimentacaoAntesFechamentoMatricula;
                    divTurma.Visible = !VS_MovimentacaoAntesFechamentoMatricula;

                    // Exclui a turma do aluno da lista caso seja movimentação de Mudança de Turma
                    TUR_Turma turma = new TUR_Turma { tur_id = VS_tur_idAnterior };
                    TUR_TurmaBO.GetEntity(turma);
                    ACA_Turno trn = new ACA_Turno { trn_id = turma.trn_id };
                    ACA_TurnoBO.GetEntity(trn);

                    string value = VS_tur_idAnterior + ";" + VS_crp_idAnterior + ";" + trn.ttn_id;
                    ListItem item = UCComboTurma1._Combo.Items.FindByValue(value);
                    if (item != null)
                    {
                        UCComboTurma1._Combo.Items.Remove(item);
                    }

                    break;
                case MTR_TipoMovimentacaoTipoMovimento.MudancaBlocoPEJA:
                    UCComboCurriculoPeriodo1.PermiteEditar = true;
                    UCComboCurriculoPeriodo1.ValidationGroup = "Aluno";
                    UCComboCurriculoPeriodo1.Obrigatorio = true;
                    UCComboCurriculoPeriodo1.CancelSelect = false;
                    UCComboCurriculoPeriodo1._LoadBy_cur_id_crr_id_esc_id_uni_id(VS_cur_idAnterior, VS_crr_idAnterior, VS_esc_idAnterior, VS_uni_idAnterior);
                    divPeriodo.Visible = true;

                    UCComboCurriculoPeriodoAvaliacao1.Obrigatorio = true;
                    UCComboCurriculoPeriodoAvaliacao1.ValidationGroup = "Aluno";

                    UCComboTurma1.PermiteEditar = false;
                    UCComboTurma1.ValidationGroup = "Aluno";
                    UCComboTurma1.Obrigatorio = !VS_MovimentacaoAntesFechamentoMatricula;
                    divTurma.Visible = !VS_MovimentacaoAntesFechamentoMatricula;

                    break;
                case MTR_TipoMovimentacaoTipoMovimento.TransferenciaParaRedeParticular:
                    tre_id = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_REDE_ENSINO_PARTICULAR, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    divEscolaDestino.Visible = !string.IsNullOrEmpty(tre_id);

                    lblInfoEscolaDestino.Text = "Para selecionar a escola de destino, é necessário configurar o tipo de rede de ensino particular nas configurações do sistema.";
                    lblInfoEscolaDestino.Text = UtilBO.GetErroMessage(lblInfoEscolaDestino.Text, UtilBO.TipoMensagem.Informacao);
                    lblInfoEscolaDestino.Visible = string.IsNullOrEmpty(tre_id);

                    break;
                case MTR_TipoMovimentacaoTipoMovimento.TransferenciaParaRedePublicaOutrosMunicipios:
                    divMunicipio.Visible = true;
                    lblMunicipio.Text += " *";
                    rfvMunicipio.Visible = true;

                    tre_id = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_REDE_ENSINO_PUBLICA_OUTROS_MUNICIPIOS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    divEscolaDestino.Visible = !string.IsNullOrEmpty(tre_id);

                    lblInfoEscolaDestino.Text = "Para selecionar a escola de destino, é necessário configurar o tipo de rede de ensino de outros municípios nas configurações do sistema.";
                    lblInfoEscolaDestino.Text = UtilBO.GetErroMessage(lblInfoEscolaDestino.Text, UtilBO.TipoMensagem.Informacao);
                    lblInfoEscolaDestino.Visible = string.IsNullOrEmpty(tre_id);

                    break;
                case MTR_TipoMovimentacaoTipoMovimento.TransferenciaParaRedePublicaOutrosEstadosFederal:
                    divUnidadeFederativa.Visible = true;
                    lblEstado.Text += " *";
                    cpvEstado.Visible = true;
                    divMunicipio.Visible = true;

                    tre_id = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_REDE_ENSINO_PUBLICA_OUTROS_ESTADOS_FEDERAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    divEscolaDestino.Visible = !string.IsNullOrEmpty(tre_id);

                    lblInfoEscolaDestino.Text = "Para selecionar a escola de destino, é necessário configurar o tipo de rede de ensino de outros estados/federal nas configurações do sistema.";
                    lblInfoEscolaDestino.Text = UtilBO.GetErroMessage(lblInfoEscolaDestino.Text, UtilBO.TipoMensagem.Informacao);
                    lblInfoEscolaDestino.Visible = string.IsNullOrEmpty(tre_id);

                    break;
                case MTR_TipoMovimentacaoTipoMovimento.Falecimento:
                    divObservacoes.Visible = true;
                    lblObservacao.Text = "Dados do atestado de óbito e a causa da morte *";
                    rfvObservacao.ErrorMessage = "Dados do atestado de óbito e a causa da morte é obrigatório.";
                    rfvObservacao.Visible = true;

                    break;
                case MTR_TipoMovimentacaoTipoMovimento.DesistenciaMatricula:
                    divObservacoes.Visible = true;
                    lblObservacao.Text = "Motivo de cancelamento *";
                    rfvObservacao.ErrorMessage = "Motivo de cancelamento é obrigatório.";
                    rfvObservacao.Visible = true;

                    tre_id = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_REDE_ENSINO_PUBLICA_OUTROS_ESTADOS_FEDERAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    divEscolaDestino.Visible = !string.IsNullOrEmpty(tre_id);

                    lblInfoEscolaDestino.Text = "Para selecionar a escola de destino, é necessário configurar o tipo de rede de ensino de outros estados/federal nas configurações do sistema.";
                    lblInfoEscolaDestino.Text = UtilBO.GetErroMessage(lblInfoEscolaDestino.Text, UtilBO.TipoMensagem.Informacao);
                    lblInfoEscolaDestino.Visible = string.IsNullOrEmpty(tre_id);

                    break;

            }

            // Se existir parâmetro vigente, com a configuração numeração automática.
            rfvMatriculaAtual.Visible = txtMatriculaAtual.Visible && lblMatriculaAtual.Text.Contains("*");

            UCBuscaEscolaOrigem1.Titulo = VS_tipoEscola == Convert.ToByte(ACA_AlunoEscolaOrigemTipoCadastro.MovimentacaoOrigem) ? "Escola de origem" : "Escola de destino";

            // Só limpa os valores dos combos se for uma inclusão de movimentação
            if (VS_mov_id <= 0)
            {
                if (!ddlUASuperior.Visible)
                {
                    if (ddlUnidadeEscola.Visible)
                    {
                        if (ddlUnidadeEscola.Items.Count == 2)
                        {
                            ddlUnidadeEscola.SelectedValue = ddlUnidadeEscola.Items[1].Value;
                            ddlUnidadeEscola_SelectedIndexChanged(null, null);
                            UCComboCursoCurriculo1.PermiteEditar = true;
                            UCComboCursoCurriculo1.Focus();
                        }
                    }
                }

                if (ddlUnidadeEscola.SelectedValue != "-1;-1")
                {
                    if (UCComboCursoCurriculo1.QuantidadeItensCombo == 2)
                    {
                        UCComboCursoCurriculo1.SelectedIndex = 1;
                        UCComboCursoCurriculo1_IndexChanged();
                    }
                }

                if ((UCComboCursoCurriculo1.Valor[0] != -1) && (UCComboCursoCurriculo1.Valor[1] != -1))
                {
                    if (UCComboCurriculoPeriodo1.QuantidadeItensCombo == 2)
                    {
                        UCComboCurriculoPeriodo1.SelectedIndex = 1;
                        UCComboCurriculoPeriodo1__OnSelectedIndexChange();
                    }
                }

                if (divPeriodoAvaliacao.Visible)
                {
                    if (UCComboCurriculoPeriodo1.Valor[0] != -1 && UCComboCurriculoPeriodo1.Valor[1] != -1 && UCComboCurriculoPeriodo1.Valor[2] != -1)
                    {
                        if (UCComboCurriculoPeriodoAvaliacao1.QuantidadeItensCombo == 2)
                        {
                            UCComboCurriculoPeriodoAvaliacao1.SelectedIndex = 1;
                            UCComboCurriculoPeriodoAvaliacao1_IndexChanged();
                        }
                    }

                    if (UCComboCurriculoPeriodoAvaliacao1.Valor != -1)
                    {
                        if (UCComboTurma1.QuantidadeItemsCombo == 2)
                        {
                            UCComboTurma1.SelectedIndex = 1;
                        }
                    }
                }
                else
                {
                    if (UCComboCurriculoPeriodo1.Valor[0] != -1 && UCComboCurriculoPeriodo1.Valor[1] != -1 && UCComboCurriculoPeriodo1.Valor[2] != -1)
                    {
                        if (UCComboTurma1.QuantidadeItemsCombo == 2)
                        {
                            UCComboTurma1.SelectedIndex = 1;
                        }
                    }
                }
            }

            if (VS_Excedente)
            {
                if (VS_FiltroEscola)
                {
                    ESC_Escola escola = new ESC_Escola { esc_id = VS_Esc_id_Excedente };
                    ESC_EscolaBO.GetEntity(escola);

                    ddlUASuperior.SelectedValue = escola.uad_idSuperior.ToString();
                    ddlUASuperior.Enabled = false;
                    ddlUASuperior.Visible = false;
                    lblUASuperior.Visible = false;
                    ddlUASuperior_SelectedIndexChanged(null, null);
                }

                ddlUnidadeEscola.SelectedValue = VS_Esc_id_Excedente + ";" + VS_Uni_id_Excedente;
                ddlUnidadeEscola.Enabled = false;
                ddlUnidadeEscola.Visible = false;
                lblUnidadeEscola.Visible = false;
                ddlUnidadeEscola_SelectedIndexChanged(null, null);

                UCComboCursoCurriculo1.Valor = new[] { VS_Cur_id_Excedente, VS_Crr_id_Excedente };
                UCComboCursoCurriculo1.PermiteEditar = false;
                UCComboCursoCurriculo1.ExibeCombo = false;
                UCComboCursoCurriculo1_IndexChanged();

                UCComboCurriculoPeriodo1.Valor = new[] { VS_Cur_id_Excedente, VS_Crr_id_Excedente, VS_Crp_id_Excedente };
                UCComboCurriculoPeriodo1.PermiteEditar = false;
                UCComboCurriculoPeriodo1.ExibeCombo = false;
                UCComboCurriculoPeriodo1__OnSelectedIndexChange();

                UCComboTurma1.PermiteEditar = true;
            }
        }

        private void UCComboCursoCurriculo1_IndexChanged()
        {
            try
            {
                UCComboCurriculoPeriodo1._Combo.SelectedValue = "-1;-1;-1";

                divPeriodoAvaliacao.Visible = false;
                UCComboCurriculoPeriodoAvaliacao1.Valor = -1;
                UCComboCurriculoPeriodoAvaliacao1.PermiteEditar = false;
                rdbAvaliadoPeriodoCorrente.SelectedValue = "Sim";
                rdbAvaliadoPeriodoCorrente.Enabled = false;

                UCComboTurma1._Combo.SelectedValue = "-1;-1;-1";
                UCComboTurma1._Combo.Enabled = false;

                int esc_id2;
                int uni_id2;

                if (divEscolaPropriaRede.Visible)
                {
                    esc_id2 = Convert.ToInt32(ddlUnidadeEscola.SelectedValue.Split(';')[0]);
                    uni_id2 = Convert.ToInt32(ddlUnidadeEscola.SelectedValue.Split(';')[1]);
                }
                else
                {
                    esc_id2 = VS_esc_idAnterior;
                    uni_id2 = VS_uni_idAnterior;
                }

                UCComboCurriculoPeriodo1._Combo.Items.Clear();
                UCComboCurriculoPeriodo1._MostrarMessageSelecione = true;
                UCComboCurriculoPeriodo1.CancelSelect = false;

                if (TipoMovimentoSelecionado == MTR_TipoMovimentacaoTipoMovimento.TransferenciaPropriaRede)
                {
                    // Carrega cursos equivalentes somente.
                    UCComboCurriculoPeriodo1.Carregar_Por_CursoEscola_PeriodoEquivalente(
                            UCComboCursoCurriculo1.Valor[0],
                            UCComboCursoCurriculo1.Valor[1],
                            Convert.ToInt32(ddlUnidadeEscola.SelectedValue.Split(';')[0]),
                            Convert.ToInt32(ddlUnidadeEscola.SelectedValue.Split(';')[1]),
                            VS_cur_idAnterior,
                            VS_crr_idAnterior,
                            VS_crp_idAnterior);

                    UCComboCurriculoPeriodo1.SelecionaPrimeiroItem();
                }
                // Alterado para carregar todos os grupamentos de ensino do curso na adequação de matrícula
                else if (TipoMovimentoSelecionado == MTR_TipoMovimentacaoTipoMovimento.Adequacao)
                {
                    UCComboCurriculoPeriodo1._Load(UCComboCursoCurriculo1.Valor[0], UCComboCursoCurriculo1.Valor[1]);
                }
                else if (TipoMovimentoSelecionado == MTR_TipoMovimentacaoTipoMovimento.RenovacaoManual)
                {
                    int crp_ordem = UCComboCursoCurriculo1.Valor[0] != VS_cur_idAnterior ||
                        UCComboCursoCurriculo1.Valor[1] != VS_crr_idAnterior ? 0 : VS_crp_ordem;

                    UCComboCurriculoPeriodo1.CarregarPorEscolaCursoPeriodoOrdem
                        (
                            Convert.ToInt32(ddlUnidadeEscola.SelectedValue.Split(';')[0]),
                            Convert.ToInt32(ddlUnidadeEscola.SelectedValue.Split(';')[1]),
                            UCComboCursoCurriculo1.Valor[0],
                            UCComboCursoCurriculo1.Valor[1],
                            crp_ordem
                        );

                    UCComboCurriculoPeriodo1.SelecionaPrimeiroItem();
                }
                else
                {
                    // Se for uma nova movimentação carrega apenas os periodos relacionados à escola
                    // Se for uma alteração de movimentação, carrega todos os periodos do curso
                    if (VS_mov_id > 0)
                    {
                        UCComboCurriculoPeriodo1._Load(UCComboCursoCurriculo1.Valor[0], UCComboCursoCurriculo1.Valor[1]);
                    }
                    else
                    {
                        UCComboCurriculoPeriodo1._LoadBy_cur_id_crr_id_esc_id_uni_id(UCComboCursoCurriculo1.Valor[0], UCComboCursoCurriculo1.Valor[1], esc_id2, uni_id2);
                    }
                }

                UCComboCurriculoPeriodo1.PermiteEditar = UCComboCursoCurriculo1.Valor[0] > 0;

                // Exclui o curriculo periodo do aluno da lista caso seja movimentação de Remanejamento
                if (UCComboCurriculoPeriodo1._Combo.Enabled && TipoMovimentoSelecionado == MTR_TipoMovimentacaoTipoMovimento.Remanejamento)
                {
                    string value = VS_cur_idAnterior + ";" + VS_crr_idAnterior + ";" + VS_crp_idAnterior;
                    ListItem item = UCComboCurriculoPeriodo1._Combo.Items.FindByValue(value);
                    if (item != null)
                    {
                        UCComboCurriculoPeriodo1._Combo.Items.Remove(item);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        private void UCComboCurriculoPeriodo1__OnSelectedIndexChange()
        {
            try
            {
                UCComboCurriculoPeriodoAvaliacao1.Valor = -1;
                UCComboCurriculoPeriodoAvaliacao1.PermiteEditar = false;
                rdbAvaliadoPeriodoCorrente.SelectedValue = "Sim";
                rdbAvaliadoPeriodoCorrente.Enabled = false;

                int esc_id2;
                int uni_id2;
                int cur_id2;
                int crr_id2;

                if (divEscolaPropriaRede.Visible)
                {
                    esc_id2 = Convert.ToInt32(ddlUnidadeEscola.SelectedValue.Split(';')[0]);
                    uni_id2 = Convert.ToInt32(ddlUnidadeEscola.SelectedValue.Split(';')[1]);
                }
                else
                {
                    esc_id2 = VS_esc_idAnterior;
                    uni_id2 = VS_uni_idAnterior;
                }

                if (divCurso.Visible)
                {
                    cur_id2 = UCComboCursoCurriculo1.Valor[0];
                    crr_id2 = UCComboCursoCurriculo1.Valor[1];
                }
                else
                {
                    cur_id2 = VS_cur_idAnterior;
                    crr_id2 = VS_crr_idAnterior;
                }

                int crp_id2 = divPeriodo.Visible ? UCComboCurriculoPeriodo1.Valor[2] : VS_crp_idAnterior;

                // Indica se será exibido o combo de ua/escolas na movimentação de adequação/reclassificação
                bool exibeEscolaAdequacaoReclassificacao = false;

                // Exibe o combo de turma de acordo com o tipo de movimentação
                switch (TipoMovimentoSelecionado)
                {
                    case MTR_TipoMovimentacaoTipoMovimento.MatriculaInicial:
                    case MTR_TipoMovimentacaoTipoMovimento.TransferenciaDaRedeParticular:
                    case MTR_TipoMovimentacaoTipoMovimento.TransferenciaDaRedePublicaOutrosMunicipios:
                    case MTR_TipoMovimentacaoTipoMovimento.TransferenciaDaRedePublicaOutrosEstadosFederal:
                    case MTR_TipoMovimentacaoTipoMovimento.Reconducao:
                    case MTR_TipoMovimentacaoTipoMovimento.MudancaTurma:
                    case MTR_TipoMovimentacaoTipoMovimento.MudancaBlocoPEJA:
                    case MTR_TipoMovimentacaoTipoMovimento.RenovacaoInicial:
                        divTurma.Visible = !VS_MovimentacaoAntesFechamentoMatricula;
                        break;
                    // Alterado para exibir o combo de turma apenas se a escola oferecer o período do curso selecionado
                    case MTR_TipoMovimentacaoTipoMovimento.Adequacao:
                    case MTR_TipoMovimentacaoTipoMovimento.Reclassificacao:
                        // Verifica se a escola oferece o curso/período selecionado ou equivalente                        
                        List<ACA_CurriculoEscolaPeriodo> listaCurriculos = ACA_CursoBO.SelecionaCursosRelacionados_Por_EscolaCursoPeriodo(cur_id2, crr_id2, crp_id2, esc_id2, uni_id2);

                        // Se a escola oferecer o curso/período selecionado ou equivalente, o combo de turma é exibido.
                        if (listaCurriculos.Count() > 0)
                        {
                            divTurma.Visible = !VS_MovimentacaoAntesFechamentoMatricula;
                            divEscolaAdequacaoReclassificao.Visible = false;
                            divRespLancNota.Visible = false;
                        }
                        // Se a escola não oferecer o período do curso selecionado, o combo de ua/escolas é exibido.
                        else
                        {
                            divTurma.Visible = false;

                            LoadInicialEscolaDadosAdequacaoReclassificacao(true);

                            cpvUnidadeAdministrativaAdequacaoReclassificao.Visible = VS_FiltroEscola;
                            cpvUnidadeEscolaAdequacaoReclassificao.Visible = true;
                            divEscolaAdequacaoReclassificao.Visible = true;

                            divRespLancNota.Visible = !MTR_MovimentacaoBO.VerificaEfetivacaoEscolaOrigem(VS_tur_idAnterior, DateTime.Now.Date, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                            exibeEscolaAdequacaoReclassificacao = true;
                        }

                        break;
                }

                UCComboTurma1._Combo.SelectedValue = "-1;-1;-1";
                UCComboTurma1._Combo.Enabled = false;

                if (cur_id2 > 0)
                {
                    ACA_CurriculoPeriodo crp = new ACA_CurriculoPeriodo { cur_id = cur_id2, crr_id = crr_id2, crp_id = crp_id2 };

                    // Variavel para verificar se tem o curriculo periodo está marcado como TurmaPorAvaliação
                    bool TurmaPorAvaliacao = ACA_CurriculoPeriodoBO.VerificaTurmaPorAvaliacao(ref crp);
                    bool Peja = ACA_CurriculoPeriodoBO.VerificaSomenteCurriculoPeriodoEJA(ref crp);

                    // Verifica se o curso tem regime de matrícula seriado por avaliações (Independente do campo turma por avaliação estar marcado ou não)
                    if (Peja)
                    {
                        // Recupera os dados do curriculo do curso
                        ACA_Curriculo crr = new ACA_Curriculo { cur_id = cur_id2, crr_id = crr_id2 };
                        ACA_CurriculoBO.GetEntity(crr);

                        // Se for alteração de modalidade de ensino para outra escola, não exibe o combo de avaliação
                        if (TipoMovimentoSelecionado == MTR_TipoMovimentacaoTipoMovimento.MudancaModalidaeEnsino)
                        {
                            if (ddlUnidadeEscola.SelectedValue.Split(';')[0] == VS_esc_idAnterior.ToString()
                                && ddlUnidadeEscola.SelectedValue.Split(';')[1] == VS_uni_idAnterior.ToString())
                            {
                                divPeriodoAvaliacao.Visible = true;
                            }
                            else
                            {
                                divPeriodoAvaliacao.Visible = false;
                            }
                        }
                        // Se for adequação ou reclassificação e mudar a escola, não exibe o combo de avaliação
                        else if (TipoMovimentoSelecionado == MTR_TipoMovimentacaoTipoMovimento.Reclassificacao || TipoMovimentoSelecionado == MTR_TipoMovimentacaoTipoMovimento.Adequacao)
                        {
                            divPeriodoAvaliacao.Visible = !exibeEscolaAdequacaoReclassificacao;
                        }
                        else
                        {
                            divPeriodoAvaliacao.Visible = true;
                        }

                        // Se for turma por avaliação, carrega as avaliações do período
                        UCComboCurriculoPeriodoAvaliacao1.Texto = crp.crp_nomeAvaliacao + " *";

                        // Na movimentação de reclassificação ou adequação
                        if (TipoMovimentoSelecionado == MTR_TipoMovimentacaoTipoMovimento.Reclassificacao
                            || TipoMovimentoSelecionado == MTR_TipoMovimentacaoTipoMovimento.Adequacao)
                        {
                            // Se não mudar a ua/escola do aluno
                            if (!exibeEscolaAdequacaoReclassificacao)
                            {
                                // Verifica se foi selecionado o mesmo curso e grupamento de ensino
                                // Nesse caso não, será exibido o combo de turma, pois será apenas uma troca de avaliação
                                if (UCComboCurriculoPeriodo1.Valor[0] == VS_cur_idAnterior
                                    && UCComboCurriculoPeriodo1.Valor[1] == VS_crr_idAnterior
                                    && UCComboCurriculoPeriodo1.Valor[2] == VS_crp_idAnterior)
                                {
                                    UCComboCurriculoPeriodoAvaliacao1.CarregarAvaliacaoPorTurma(VS_tur_idAnterior);
                                    divTurma.Visible = false;
                                }
                                else
                                {
                                    UCComboCurriculoPeriodoAvaliacao1.CarregarAvaliacaoPorCurriculoPeriodo(crr.crr_qtdeAvaliacaoProgressao, crp.crp_nomeAvaliacao);
                                    divTurma.Visible = true;
                                }
                            }
                        }
                        else
                        {
                            UCComboCurriculoPeriodoAvaliacao1.CarregarAvaliacaoPorCurriculoPeriodo(crr.crr_qtdeAvaliacaoProgressao, crp.crp_nomeAvaliacao);
                        }

                        // Se o campo Turma por avaliação não estiver marcado, o combo deve ser preenchido com o primeiro valor e escondido
                        if (!TurmaPorAvaliacao)
                        {
                            UCComboCurriculoPeriodoAvaliacao1.SelecionaPrimeiroItem();
                            UCComboCurriculoPeriodoAvaliacao1.Visible = false;
                        }
                        else
                        {
                            UCComboCurriculoPeriodoAvaliacao1.Visible = true;
                        }

                        if (TipoMovimentoSelecionado == MTR_TipoMovimentacaoTipoMovimento.Adequacao
                            || TipoMovimentoSelecionado == MTR_TipoMovimentacaoTipoMovimento.Reclassificacao)
                        {
                            UCComboTurma1.CarregarTurmasCursosEquivalentes(esc_id2, uni_id2, cur_id2, crr_id2, crp_id2);
                            UCComboTurma1._Combo.Enabled = UCComboCurriculoPeriodo1._Combo.SelectedValue != "-1;-1;-1";
                        }

                        UCComboCurriculoPeriodoAvaliacao1.PermiteEditar = UCComboCurriculoPeriodo1._Combo.SelectedValue != "-1;-1;-1";
                        rdbAvaliadoPeriodoCorrente.Enabled = UCComboCurriculoPeriodo1._Combo.SelectedValue != "-1;-1;-1";
                    }
                    else
                    {
                        divPeriodoAvaliacao.Visible = false;

                        UCComboTurma1.CancelSelect = false;

                        if (TipoMovimentoSelecionado == MTR_TipoMovimentacaoTipoMovimento.Adequacao
                            || TipoMovimentoSelecionado == MTR_TipoMovimentacaoTipoMovimento.Reclassificacao
                            || TipoMovimentoSelecionado == MTR_TipoMovimentacaoTipoMovimento.MudancaTurma)
                        {
                            UCComboTurma1.CarregarTurmasCursosEquivalentes(esc_id2, uni_id2, cur_id2, crr_id2, crp_id2);
                        }
                        else if (TipoMovimentoSelecionado == MTR_TipoMovimentacaoTipoMovimento.RenovacaoManual)
                        {
                            UCComboTurma1.CarregarPorEscolaCursoPeriodoCalendarioMinimo(esc_id2, uni_id2, cur_id2, crr_id2, crp_id2, VS_cal_anoAnterior);
                        }
                        else
                        {
                            UCComboTurma1.CarregarPorEscolaCurriculoPeriodoMomentoAno(esc_id2, uni_id2, cur_id2, crr_id2, crp_id2, VS_Ttn_id_Excedente);
                        }

                        UCComboTurma1._Combo.Enabled = UCComboCurriculoPeriodo1._Combo.SelectedValue != "-1;-1;-1";
                    }
                }

                // Exclui a turma do aluno da lista caso seja movimentação de Adequacao, Reclassificacao, Mudança do
                // Bloco PEJA ou Mudança de Modalidade de Ensino
                if (UCComboTurma1._Combo.Enabled &&
                   (TipoMovimentoSelecionado == MTR_TipoMovimentacaoTipoMovimento.Adequacao ||
                    TipoMovimentoSelecionado == MTR_TipoMovimentacaoTipoMovimento.Reclassificacao ||
                    TipoMovimentoSelecionado == MTR_TipoMovimentacaoTipoMovimento.MudancaBlocoPEJA ||
                    TipoMovimentoSelecionado == MTR_TipoMovimentacaoTipoMovimento.MudancaModalidaeEnsino))
                {
                    TUR_Turma tur = new TUR_Turma { tur_id = VS_tur_idAnterior };
                    TUR_TurmaBO.GetEntity(tur);
                    ACA_Turno trn = new ACA_Turno { trn_id = tur.trn_id };
                    ACA_TurnoBO.GetEntity(trn);

                    string value = VS_tur_idAnterior + ";" + VS_crp_idAnterior + ";" + trn.ttn_id;
                    ListItem item = UCComboTurma1._Combo.Items.FindByValue(value);
                    if (item != null)
                    {
                        UCComboTurma1._Combo.Items.Remove(item);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        private void UCComboCurriculoPeriodoAvaliacao1_IndexChanged()
        {
            try
            {
                UCComboTurma1._Combo.SelectedValue = "-1;-1;-1";

                int esc_id2;
                int uni_id2;
                int cur_id2;
                int crr_id2;

                if (divEscolaPropriaRede.Visible)
                {
                    esc_id2 = Convert.ToInt32(ddlUnidadeEscola.SelectedValue.Split(';')[0]);
                    uni_id2 = Convert.ToInt32(ddlUnidadeEscola.SelectedValue.Split(';')[1]);
                }
                else
                {
                    esc_id2 = VS_esc_idAnterior;
                    uni_id2 = VS_uni_idAnterior;
                }

                if (divCurso.Visible)
                {
                    cur_id2 = UCComboCursoCurriculo1.Valor[0];
                    crr_id2 = UCComboCursoCurriculo1.Valor[1];
                }
                else
                {
                    cur_id2 = VS_cur_idAnterior;
                    crr_id2 = VS_crr_idAnterior;
                }

                int crp_id2 = divPeriodo.Visible ? UCComboCurriculoPeriodo1.Valor[2] : VS_crp_idAnterior;
                int tca_numeroAvalicao = UCComboCurriculoPeriodoAvaliacao1.Valor;

                UCComboTurma1.CancelSelect = false;

                if (TipoMovimentoSelecionado == MTR_TipoMovimentacaoTipoMovimento.Reclassificacao
                     || TipoMovimentoSelecionado == MTR_TipoMovimentacaoTipoMovimento.Adequacao)
                {
                    UCComboTurma1.CarregarTurmasCursosEquivalentesAvaliacao(esc_id2, uni_id2, cur_id2, crr_id2, crp_id2, tca_numeroAvalicao);
                }
                else
                {
                    UCComboTurma1.CarregarPorEscolaCurriculoPeriodoMomentoAnoAvaliacao(esc_id2, uni_id2, cur_id2, crr_id2, crp_id2, tca_numeroAvalicao);
                }

                UCComboTurma1._Combo.Enabled = UCComboCurriculoPeriodoAvaliacao1.Valor != -1;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        private void UCBuscaEscolaOrigem1_NovaEscola()
        {
            int tre_id = 0;
            if (UCComboTipoMovimentacaoMatricula1.Valor[1] == Convert.ToInt32(MTR_TipoMovimentacaoTipoMovimento.TransferenciaParaRedeParticular) ||
                UCComboTipoMovimentacaoMatricula1.Valor[1] == Convert.ToInt32(MTR_TipoMovimentacaoTipoMovimento.TransferenciaDaRedeParticular))
            {
                tre_id = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_REDE_ENSINO_PARTICULAR, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }
            else if (UCComboTipoMovimentacaoMatricula1.Valor[1] == Convert.ToInt32(MTR_TipoMovimentacaoTipoMovimento.TransferenciaParaRedePublicaOutrosMunicipios) ||
                UCComboTipoMovimentacaoMatricula1.Valor[1] == Convert.ToInt32(MTR_TipoMovimentacaoTipoMovimento.TransferenciaDaRedePublicaOutrosMunicipios))
            {
                tre_id = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_REDE_ENSINO_PUBLICA_OUTROS_MUNICIPIOS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }
            else if (UCComboTipoMovimentacaoMatricula1.Valor[1] == Convert.ToInt32(MTR_TipoMovimentacaoTipoMovimento.TransferenciaParaRedePublicaOutrosEstadosFederal) ||
                UCComboTipoMovimentacaoMatricula1.Valor[1] == Convert.ToInt32(MTR_TipoMovimentacaoTipoMovimento.TransferenciaDaRedePublicaOutrosEstadosFederal))
            {
                tre_id = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_REDE_ENSINO_PUBLICA_OUTROS_ESTADOS_FEDERAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }

            if (ucEscolaOrigem != null)
            {
                ucEscolaOrigem.LimparBuscaEscolaOrigem();
                ucEscolaOrigem.VS_tre_id = tre_id;
                ucEscolaOrigem.VS_tipoEscola = VS_tipoEscola;
                ucEscolaOrigem.SetarTipoEscola();
            }
        }

        private void UCBuscaEscolaOrigem1_LimparEscola()
        {
            if (ucEscolaOrigem != null)
            {
                ucEscolaOrigem.VS_eco_id = -1;
                ucEscolaOrigem.LimparCadastroEscolaOrigem();
            }
        }

        private void UCEscolaOrigem1_Selecionar(string eco_nome)
        {
            UCBuscaEscolaOrigem1.Texto = eco_nome;
            UCBuscaEscolaOrigem1.ExibirBotaoLimpar = true;
        }

        private void UCComboMotivoTransferencia1_IndexChanged()
        {
            divOutroMotivoTransferencia.Visible = UCComboMotivoTransferencia1.Valor == 0;
            tbOutroMotivoTransferencia.Text = string.Empty;
        }

        #endregion Delegates

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(Page);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsUCMovimentacao.js"));
            }

            try
            {
                if (!IsPostBack)
                {
                    grvHistoricoMovimentacao.PageSize = ApplicationWEB._Paginacao;

                    lblAvaliadoPeriodoCorrente.Text = String.Format(
                        "O aluno será avaliado no {0} corrente?",
                        GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id));

                    if (!string.IsNullOrEmpty(ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MATRICULA_ESTADUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id)))
                    {
                        if (pnlDadosAtuais.GroupingText != "Última matrícula do aluno")
                        {
                            divMatriculaEstadualAnterior.Visible = true;
                            lblMatriculaEstadualAnterior.Text = GestaoEscolarUtilBO.nomePadraoMatriculaEstadual(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
                        }

                        lblMatriculaAnterior.Text = GetGlobalResourceObject("Mensagens","MSG_NUMEROMATRICULA").ToString();
                        rfvMatriculaAnterior.Visible = false;


                        divMatriculaEstadualAtual.Visible = true;

                        lblMatriculaEstadualAtual.Text = GestaoEscolarUtilBO.nomePadraoMatriculaEstadual(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + " *";
                        rfvMatriculaEstadualAtual.ErrorMessage = GestaoEscolarUtilBO.nomePadraoMatriculaEstadual(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + " é obrigatório.";
                        lblMatriculaEstadualAnterior.Text = GestaoEscolarUtilBO.nomePadraoMatriculaEstadual(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + " *";
                        rfvMatriculaEstadualAnterior.ErrorMessage = GestaoEscolarUtilBO.nomePadraoMatriculaEstadual(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + " é obrigatório.";

                        if (!string.IsNullOrEmpty(ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.FORMATO_MATRICULA_ESTADUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id)))
                        {
                            rexMatriculaEstadualAtual.ErrorMessage = GestaoEscolarUtilBO.nomePadraoMatriculaEstadual(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + " está fora do formato padrão.";
                            rexMatriculaEstadualAtual.ValidationExpression = @"" + ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.FORMATO_MATRICULA_ESTADUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                            rexMatriculaEstadualAnterior.ErrorMessage = GestaoEscolarUtilBO.nomePadraoMatriculaEstadual(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + " está fora do formato padrão.";
                            rexMatriculaEstadualAnterior.ValidationExpression = @"" + ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.FORMATO_MATRICULA_ESTADUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                        }
                        else
                            rexMatriculaEstadualAnterior.Enabled = rexMatriculaEstadualAtual.Enabled = false;

                        lblMatriculaAtual.Text = GetGlobalResourceObject("Mensagens","MSG_NUMEROMATRICULA").ToString();
                        rfvMatriculaAtual.Visible = false;
                    }
                    else
                    {
                        lblMatriculaAnterior.Text = GetGlobalResourceObject("Mensagens","MSG_NUMEROMATRICULA") + " *";
                        rfvMatriculaAnterior.Visible = txtMatriculaAnterior.Visible;

                        lblMatriculaAtual.Text = GetGlobalResourceObject("Mensagens","MSG_NUMEROMATRICULA") + " *";
                        rfvMatriculaAtual.Visible = txtMatriculaAtual.Visible;

                        divMatriculaEstadualAnterior.Visible = false;
                        divMatriculaEstadualAtual.Visible = false;
                    }

                    UCComboMotivoTransferencia1.CarregarMotivosTransferencia();
                    UCComboMotivoTransferencia1.MostrarMessageOutros = true;

                    grvHistoricoMovimentacao.Columns[indiceColunaEscolaAnterior].HeaderText = "Escola / " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + " / " + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + " anterior";
                    grvHistoricoMovimentacao.Columns[indiceColunaEscolaAtual].HeaderText = "Escola / " + GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + " / " + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + " atual";
                }

                UCBuscaEscolaOrigem1.Titulo = VS_tipoEscola == Convert.ToByte(ACA_AlunoEscolaOrigemTipoCadastro.MovimentacaoOrigem) ? "Escola de origem" : "Escola de destino";
                UCComboTipoMovimentacaoMatricula1.IndexChanged += UCComboTipoMovimentacaoMatricula1_IndexChanged;
                UCBuscaEscolaOrigem1.NovaEscola += UCBuscaEscolaOrigem1_NovaEscola;
                UCBuscaEscolaOrigem1.LimparEscola += UCBuscaEscolaOrigem1_LimparEscola;

                if (ucEscolaOrigem != null)
                {
                    ucEscolaOrigem.Selecionar += UCEscolaOrigem1_Selecionar;
                }

                UCComboCursoCurriculo1.IndexChanged += UCComboCursoCurriculo1_IndexChanged;
                UCComboCurriculoPeriodo1._OnSelectedIndexChange += UCComboCurriculoPeriodo1__OnSelectedIndexChange;
                UCComboCurriculoPeriodoAvaliacao1.IndexChanged += UCComboCurriculoPeriodoAvaliacao1_IndexChanged;
                UCComboMotivoTransferencia1.IndexChanged += UCComboMotivoTransferencia1_IndexChanged;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnCadastrarMunicipio_Click(object sender, ImageClickEventArgs e)
        {
            AbrirJanelaCadastroMunicipio();
        }

        protected void grvHistoricoMovimentacao_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros1.Total = MTR_MovimentacaoBO.GetTotalRecords();

            // Seta propriedades necessárias para ordenação nas colunas.
            ConfiguraColunasOrdenacao(grvHistoricoMovimentacao);
        }

        protected void ValidarDataMovimentacao_ServerValidate(object source, ServerValidateEventArgs args)
        {
            bool flag = true;

            DateTime dataMovimentacao;
            DateTime.TryParse(args.Value, out dataMovimentacao);

            if ((dataMovimentacao != new DateTime()) && (dataMovimentacao > DateTime.Now))
            {
                flag = false;
            }

            args.IsValid = flag;
        }

        #region Escolas  para: Transferência na Própria Rede, Mudança de Modalidade de Ensino, Remanejamento

        protected void ddlUASuperior_SelectedIndexChanged(object sender, EventArgs e)
        {
            UCComboCursoCurriculo1.Valor = new[] { -1, -1 };
            UCComboCursoCurriculo1.PermiteEditar = false;

            UCComboCurriculoPeriodo1._Combo.SelectedValue = "-1;-1;-1";
            UCComboCurriculoPeriodo1._Combo.Enabled = false;

            divPeriodoAvaliacao.Visible = false;
            UCComboCurriculoPeriodoAvaliacao1.Valor = -1;
            UCComboCurriculoPeriodoAvaliacao1.PermiteEditar = false;
            rdbAvaliadoPeriodoCorrente.Enabled = false;

            UCComboTurma1._Combo.SelectedValue = "-1;-1;-1";
            UCComboTurma1._Combo.Enabled = false;

            try
            {
                if (VS_FiltroEscola)
                {
                    LoadEscolaDados(new Guid(ddlUASuperior.SelectedValue), false);
                }

                ddlUnidadeEscola.Enabled = ddlUASuperior.SelectedValue != Guid.Empty.ToString();

                // Exclui a escola do aluno da lista caso seja movimentação de Transferencia na Propria Rede
                if (ddlUnidadeEscola.Enabled &&
                    UCComboTipoMovimentacaoMatricula1.Valor[1] ==
                    Convert.ToInt32(MTR_TipoMovimentacaoTipoMovimento.TransferenciaPropriaRede))
                {
                    string value = VS_esc_idAnterior.ToString() + ';' + VS_uni_idAnterior;
                    ListItem item = ddlUnidadeEscola.Items.FindByValue(value);
                    if (item != null)
                    {
                        ddlUnidadeEscola.Items.Remove(item);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void ddlUnidadeEscola_SelectedIndexChanged(object sender, EventArgs e)
        {
            UnidadeEscola_IndexChanged();
        }

        #endregion

        #region Escolas  para: Adequação de matrícula, Reclassificação

        protected void ddlUASuperiorAdequacaoReclassificao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (VS_FiltroEscola)
                {
                    LoadEscolaDadosAdequacaoReclassificacao(new Guid(ddlUASuperiorAdequacaoReclassificao.SelectedValue), false);
                }

                ddlUnidadeEscolaAdequacaoReclassificao.Enabled = ddlUASuperiorAdequacaoReclassificao.SelectedValue != Guid.Empty.ToString();

                string value = VS_esc_idAnterior.ToString() + ';' + VS_uni_idAnterior;
                ListItem item = ddlUnidadeEscolaAdequacaoReclassificao.Items.FindByValue(value);
                if (item != null)
                {
                    ddlUnidadeEscolaAdequacaoReclassificao.Items.Remove(item);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion

        #endregion Eventos
        
        #region Métodos

        /// <summary>
        /// Carrega a matrícula anterior do aluno
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="alu_situacao"></param>
        public void CarregarMatriculaAtual(long alu_id, byte alu_situacao)
        {
            // Atualiza o id do aluno
            VS_alu_id = alu_id;
            VS_mov_id = -1;
            VS_dataAcesso = DateTime.Now;

            // Volta a tela ao seu estado inicial, apenas com o combo
            // de tipo de movimentação aparecendo.
            UCComboTipoMovimentacaoMatricula1.Valor = new[] { -1, -1 };
            EsconderCamposMovimentacao();

            bool novo_aluno = VS_alu_id > 0 ? false : true;

            bool existeSolicitacaoTransferencia = false;

            DataTable dtUltimaMatricula = ACA_AlunoCurriculoBO.SelecionaDadosUltimaMatricula(VS_alu_id);

            pnlDadosAtuais.Visible = false;

            if (dtUltimaMatricula.Rows.Count > 0)
            {
                pnlDadosAtuais.Visible = true;
                lblDadosAluno.Text = string.Empty;

                ESC_Escola escola = new ESC_Escola { esc_id = Convert.ToInt32(dtUltimaMatricula.Rows[0]["esc_id"].ToString()) };
                ESC_EscolaBO.GetEntity(escola);

                if (ACA_ParametroAcademicoBO.VerificaFiltroUniAdmSuperiorPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id))
                {
                    SYS_UnidadeAdministrativa ua = new SYS_UnidadeAdministrativa { uad_id = escola.uad_id, ent_id = escola.ent_id };
                    SYS_UnidadeAdministrativaBO.GetEntity(ua);

                    Guid uad_idSuperior = escola.uad_idSuperiorGestao.Equals(Guid.Empty) ? ua.uad_idSuperior : escola.uad_idSuperiorGestao;

                    SYS_UnidadeAdministrativa uaSuperior = new SYS_UnidadeAdministrativa { uad_id = uad_idSuperior, ent_id = ua.ent_id };
                    SYS_UnidadeAdministrativaBO.GetEntity(uaSuperior);

                    SYS_TipoUnidadeAdministrativa tua = new SYS_TipoUnidadeAdministrativa { tua_id = uaSuperior.tua_id };
                    SYS_TipoUnidadeAdministrativaBO.GetEntity(tua);

                    if (!string.IsNullOrEmpty(uaSuperior.uad_nome))
                    {
                        lblDadosAluno.Text = tua.tua_nome + ": <b>" + uaSuperior.uad_nome + "</b><br />";
                    }
                }

                bool paramExibirCodigoEscola = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                if (paramExibirCodigoEscola)
                    lblDadosAluno.Text += "Escola: <b> " + dtUltimaMatricula.Rows[0]["esc_codigo"] + " - " + dtUltimaMatricula.Rows[0]["esc_nome"] + "</b><br />";
                else
                    lblDadosAluno.Text += "Escola: <b> " + dtUltimaMatricula.Rows[0]["esc_nome"] + "</b><br />";

                lblDadosAluno.Text += GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + ": <b>" + dtUltimaMatricula.Rows[0]["cur_nome"] + "</b><br />";
                lblDadosAluno.Text += GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id) + ": <b>" + dtUltimaMatricula.Rows[0]["crp_descricao"] + "</b><br />";

                if (!string.IsNullOrEmpty(dtUltimaMatricula.Rows[0]["tur_codigo"].ToString()))
                    lblDadosAluno.Text += "Turma: <b>" + dtUltimaMatricula.Rows[0]["tur_codigo"] + "</b><br />";

                if (!string.IsNullOrEmpty(dtUltimaMatricula.Rows[0]["crp_nomeAvaliacao"].ToString()) && !string.IsNullOrEmpty(dtUltimaMatricula.Rows[0]["tca_numeroAvaliacao"].ToString()))
                    lblDadosAluno.Text += dtUltimaMatricula.Rows[0]["crp_nomeAvaliacao"] + ": <b>" + dtUltimaMatricula.Rows[0]["tca_numeroAvaliacao"] + "</b><br />";

                // Recupera a situação do último curriculo do aluno
                byte alc_situacao = string.IsNullOrEmpty(dtUltimaMatricula.Rows[0]["alc_situacao"].ToString())
                                        ? Convert.ToByte(0)
                                        : Convert.ToByte(dtUltimaMatricula.Rows[0]["alc_situacao"].ToString());

                // Verifica a situação do aluno e configura as opções de movimentação
                if (alc_situacao == (byte)ACA_AlunoCurriculoSituacao.Ativo)
                {
                    VS_alc_idAnterior = Convert.ToInt32(string.IsNullOrEmpty(dtUltimaMatricula.Rows[0]["alc_id"].ToString()) ? "-1" : dtUltimaMatricula.Rows[0]["alc_id"].ToString());
                    VS_mtu_idAnterior = Convert.ToInt32(string.IsNullOrEmpty(dtUltimaMatricula.Rows[0]["mtu_id"].ToString()) ? "-1" : dtUltimaMatricula.Rows[0]["mtu_id"].ToString());

                    VS_esc_idAnterior = Convert.ToInt32(dtUltimaMatricula.Rows[0]["esc_id"].ToString());
                    VS_uni_idAnterior = Convert.ToInt32(dtUltimaMatricula.Rows[0]["uni_id"].ToString());
                    VS_cur_idAnterior = Convert.ToInt32(dtUltimaMatricula.Rows[0]["cur_id"].ToString());
                    VS_crr_idAnterior = Convert.ToInt32(dtUltimaMatricula.Rows[0]["crr_id"].ToString());
                    VS_crp_idAnterior = Convert.ToInt32(dtUltimaMatricula.Rows[0]["crp_id"].ToString());
                    VS_tur_idAnterior = Convert.ToInt64(String.IsNullOrEmpty(dtUltimaMatricula.Rows[0]["tur_id"].ToString()) ? "-1" : dtUltimaMatricula.Rows[0]["tur_id"].ToString());
                    VS_crp_ordem = Convert.ToInt32(dtUltimaMatricula.Rows[0]["crp_ordem"] != DBNull.Value ? dtUltimaMatricula.Rows[0]["crp_ordem"] : "0");
                    VS_cur_exclusivoDefAnterior = Convert.ToBoolean(dtUltimaMatricula.Rows[0]["cur_exclusivoDeficiente"]);

                    VS_cal_anoAnterior = Convert.ToInt32(dtUltimaMatricula.Rows[0]["cal_ano"].ToString());

                    // Variáveis do PEJA
                    VS_ala_idAnterior = Convert.ToInt32(string.IsNullOrEmpty(dtUltimaMatricula.Rows[0]["ala_id"].ToString()) ? "-1" : dtUltimaMatricula.Rows[0]["ala_id"].ToString());
                    VS_tca_idAnterior = Convert.ToInt32(string.IsNullOrEmpty(dtUltimaMatricula.Rows[0]["tca_id"].ToString()) ? "-1" : dtUltimaMatricula.Rows[0]["tca_id"].ToString());

                    txtMatriculaAnterior.Text = dtUltimaMatricula.Rows[0]["alc_matricula"].ToString();

                    txtMatriculaEstadualAnterior.Text = dtUltimaMatricula.Rows[0]["alc_matriculaEstadual"].ToString();

                    // Carrega o numero do IdCenso que fica no campo alc_codigoInep
                    txtIDCenso.Text = dtUltimaMatricula.Rows[0]["alc_codigoInep"].ToString();

                    pnlDadosAtuais.Visible = true;
                    pnlMovimentacao.Visible = true;

                    // Verifica se existe solicitação de transferência pendente
                    existeSolicitacaoTransferencia = VerificarSolicitacaoTransferenciaPendente();

                    UCComboTipoMovimentacaoMatricula1.CarregarTipoMovimentacaoPorCategoria(false, true, true, false, true, novo_aluno, alu_id, alu_situacao);
                    LoadEscolaDados(Guid.Empty, true);

                    pnlMovimentacao.GroupingText = "Dados da movimentação do aluno";

                    PesquisarMovimentacoes();
                }
                else
                {
                    // Mostra a matrícula
                    lblDadosAluno.Text += GestaoEscolarUtilBO.nomePadraoMatriculaEstadual(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    if (string.IsNullOrEmpty(ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MATRICULA_ESTADUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id)))
                    {
                        lblDadosAluno.Text += ": <b>" + dtUltimaMatricula.Rows[0]["alc_matricula"] + "</b><br />";
                    }
                    else
                    {
                        lblDadosAluno.Text += ": <b>" + dtUltimaMatricula.Rows[0]["alc_matriculaEstadual"] + "</b><br />";
                    }

                    pnlDadosAtuais.GroupingText = "Última matrícula do aluno";

                    divTextboxes.Visible = false;
                    divMatriculaEstadualAnterior.Visible = false;

                    pnlMovimentacao.GroupingText = "Dados da matrícula do aluno";

                    pnlMovimentacao.Visible = true;
                    UCComboTipoMovimentacaoMatricula1.Visible = true;
                    UCComboTipoMovimentacaoMatricula1.CarregarTipoMovimentacaoPorCategoria(true, false, false, false, false, novo_aluno, alu_id, alu_situacao);

                    if (VS_alu_id > 0)
                    {
                        PesquisarMovimentacoes();
                    }
                }
            }
            else // Matrícula Inicial
            {
                pnlMovimentacao.GroupingText = "Dados da matrícula do aluno";
                pnlMovimentacao.Visible = true;
                UCComboTipoMovimentacaoMatricula1.Visible = true;
                UCComboTipoMovimentacaoMatricula1.CarregarTipoMovimentacaoPorCategoria(true, false, false, false, false, novo_aluno, alu_id, alu_situacao);
            }

            rfvMatriculaAtual.Visible = txtMatriculaAtual.Visible && lblMatriculaAtual.Text.Contains("*");

            // Verifica se existe movimentação retroativa pendente
            if (!existeSolicitacaoTransferencia)
            {
                VerificarMovimentacaoRetroativa();
            }

            divDataMovimentacao.Visible = false;
            txtDataMovimentacao.Text = string.Empty;
        }

        /// <summary>
        /// Esconde todos os campos adicionais da movimentação
        /// </summary>
        private void EsconderCamposMovimentacao()
        {
            VS_tipoEscola = Convert.ToByte(ACA_AlunoEscolaOrigemTipoCadastro.MovimentacaoDestino);

            divEscolaPropriaRede.Visible = false;
            cpvUnidadeAdministrativa.Visible = false;
            cpvUnidadeEscola.Visible = false;

            divCurso.Visible = false;
            divPeriodo.Visible = false;
            divPeriodoAvaliacao.Visible = false;
            divTurma.Visible = false;
            divMatriculaNovo.Visible = false;
            divFrequencia.Visible = false;

            divAvaliacao.Visible = false;
            txtAvaliacao.Text = string.Empty;

            divRespLancNota.Visible = false;
            pnlResponsavelTransferencia.Visible = false;

            int escolaPadraoResponsavel = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.ESCOLA_RESPONSAVEL_EFETIVACAO_TRANSFERENCIA_ENTRE_ESCOLAS, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            if (escolaPadraoResponsavel > 0)
                rblLancamentoNotas.SelectedValue = escolaPadraoResponsavel.ToString();

            divUnidadeFederativa.Visible = false;
            lblEstado.Text = "Estado";
            ddlEstado.SelectedValue = Guid.Empty.ToString();
            cpvEstado.Visible = false;

            divMunicipio.Visible = false;
            lblMunicipio.Text = "Município";
            txtMunicipio.Text = string.Empty;
            txtCid_idMunicipio.Value = string.Empty;
            rfvMunicipio.Visible = false;

            divEscolaDestino.Visible = false;
            UCBuscaEscolaOrigem1.Texto = string.Empty;
            UCBuscaEscolaOrigem1.ExibirBotaoLimpar = false;

            if (ucEscolaOrigem != null)
            {
                ucEscolaOrigem.VS_eco_id = -1;
            }

            lblInfoEscolaDestino.Visible = false;

            divObservacoes.Visible = false;
            txtObservacao.Text = string.Empty;
            rfvObservacao.Visible = false;

            divEscolaAdequacaoReclassificao.Visible = false;

            // Só limpa os valores dos combos se for uma inclusão de movimentação
            if (VS_mov_id <= 0)
            {
                if (VS_FiltroEscola)
                {
                    ddlUASuperior.SelectedValue = Guid.Empty.ToString();
                }

                ddlUnidadeEscola.SelectedValue = "-1;-1";
                UCComboCursoCurriculo1.Valor = new[] { -1, -1 };
                UCComboCurriculoPeriodo1.Valor = new[] { -1, -1, -1 };
                UCComboCurriculoPeriodoAvaliacao1.Valor = -1;
                rdbAvaliadoPeriodoCorrente.SelectedValue = "Sim";

                UCComboTurma1.Valor = new long[] { -1, -1, -1 };
            }

            string existeRA = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MATRICULA_ESTADUAL, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            if (!string.IsNullOrEmpty(existeRA))
            {
                bool exibeNumeroMatricula = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBE_NUMERO_MATRICULA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                txtMatriculaAnterior.Visible = exibeNumeroMatricula;
                lblMatriculaAnterior.Visible = exibeNumeroMatricula;
                txtMatriculaAtual.Visible = exibeNumeroMatricula;
                lblMatriculaAtual.Visible = exibeNumeroMatricula;
            }
            
        }

        /// <summary>
        /// Verifica se existe alguma solicitação de transferência pendente para o aluno
        /// </summary>
        private bool VerificarSolicitacaoTransferenciaPendente()
        {
            lblInformacao.Visible = false;
            UCComboTipoMovimentacaoMatricula1.Visible = !VS_MovimentacaoAntesFechamentoMatricula;

            return false;
        }

        /// <summary>
        /// Verifica se existe alguma movimentação retroativa não aprovada para o aluno
        /// </summary>
        private void VerificarMovimentacaoRetroativa()
        {
            
                lblInformacao.Visible = false;
                UCComboTipoMovimentacaoMatricula1.Visible = !VS_MovimentacaoAntesFechamentoMatricula;
        }

        /// <summary>
        /// Abrir dialog para cadastro imediato de cidade no banco de dados
        /// </summary>
        private void AbrirJanelaCadastroMunicipio()
        {
            if (UCCidades1 != null)
            {
                if (!UCCidades1.Visible)
                {
                    UCCidades1.CarregarCombos();
                }

                UCCidades1.Visible = true;
                UCCidades1.SetaFoco();

                UpnCadastroCidades.Update();

                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "CadastroMunicipio", "$('#" + UCCidades1.ContainerName + "').dialog('open');", true);
            }
        }

        /// <summary>
        /// Pesquisa o histórico de movimentações do aluno
        /// </summary>
        private void PesquisarMovimentacoes()
        {
            odsHistoricoMovimentacao.SelectParameters.Clear();
            odsHistoricoMovimentacao.SelectParameters.Add("alu_id", VS_alu_id.ToString());
            odsHistoricoMovimentacao.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());

            //grvHistoricoMovimentacao.DataBind();
            pnlHistoricoMovimentacoes.Visible = true;
        }

        #region Escolas  para: Transferência na Própria Rede, Mudança de Modalidade de Ensino, Remanejamento

        /// <summary>
        /// Verifica os parâmetros acadêmicos cadastrados, mostrando e carregando os combos
        /// conforme a configuração. Não mostra o combo de escola, só o de UA.
        /// Se parâmetro FILTRAR_ESCOLA_UA_SUPERIOR = "Sim", mostra combo de Unidade Administrativa,
        /// e carrega no combo pelo tipo de UA que estiver setada no parâmetro
        /// TIPO_UNIDADE_ADMINISTRATIVA_FILTRO_ESCOLA.
        /// Desconsiderando as permissões do usuário
        /// </summary>
        /// <param name="todaEscolas">True para carregar todas as escolas/False para carregar as escolas de permissão do usuário</param>
        private void LoadInicialEscolaDados(bool todaEscolas)
        {
            string textoUASuperior = string.Empty;

            // Carrega todas as escolas do sistema
            if (todaEscolas)
            {
                // Carrega as escolas
                if (!ACA_ParametroAcademicoBO.VerificaFiltroUniAdmSuperiorPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id))
                {
                    lblUASuperior.Visible = false;
                    ddlUASuperior.Visible = false;

                    CarregaEscolas();

                    VS_FiltroEscola = false;
                }
                else
                {
                    // Carrega as escolas por unidade administrativa superior
                    VS_tua_id = ACA_ParametroAcademicoBO.VerificaFiltroEscolaPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    SYS_TipoUnidadeAdministrativa TipoUnidadeAdm = new SYS_TipoUnidadeAdministrativa { tua_id = VS_tua_id };
                    SYS_TipoUnidadeAdministrativaBO.GetEntity(TipoUnidadeAdm);

                    lblUASuperior.Text = string.IsNullOrEmpty(TipoUnidadeAdm.tua_nome) ? "Unidade administrativa superior" : TipoUnidadeAdm.tua_nome;
                    lblUASuperior.Text += " *";

                    cpvUnidadeAdministrativa.ErrorMessage = lblUASuperior.Text.Replace("*", string.Empty) + " é obrigatório.";

                    if (lblUASuperior.Text.EndsWith(ApplicationWEB.TextoAsteriscoObrigatorio))
                    {
                        textoUASuperior = lblUASuperior.Text.Replace(ApplicationWEB.TextoAsteriscoObrigatorio, string.Empty);
                    }
                    else if (lblUASuperior.Text.EndsWith("*"))
                    {
                        textoUASuperior = lblUASuperior.Text.Replace("*", string.Empty);
                    }

                    DataTable dt = SYS_UnidadeAdministrativaBO.GetSelectBy_Pesquisa_PermissaoTotal(VS_tua_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                    ddlUASuperior.Items.Clear();
                    ddlUASuperior.Items.Insert(0, new ListItem("-- Selecione um(a) " + textoUASuperior + " --", Guid.Empty.ToString(), true));
                    ddlUASuperior.DataSource = dt;
                    ddlUASuperior.DataBind();

                    ddlUnidadeEscola.DataTextField = "esc_uni_nome";
                    ddlUnidadeEscola.Items.Clear();
                    ddlUnidadeEscola.Items.Insert(0, new ListItem("-- Selecione uma escola --", "-1;-1", true));
                    ddlUnidadeEscola.Enabled = false;

                    lblUASuperior.Visible = true;
                    ddlUASuperior.Visible = true;

                    VS_FiltroEscola = true;
                }
            }
            else
            {
                // Carrega as escola que o usuário tem permissão
                if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa ||
                    !ACA_ParametroAcademicoBO.VerificaFiltroUniAdmSuperiorPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id))
                {
                    lblUASuperior.Visible = false;
                    ddlUASuperior.Visible = false;

                    ddlUnidadeEscola.Enabled = true;
                    ddlUnidadeEscola.DataTextField = "uni_escolaNome";
                    ddlUnidadeEscola.Items.Clear();
                    ddlUnidadeEscola.Items.Insert(0, new ListItem("-- Selecione uma escola --", "-1;-1", true));
                    ddlUnidadeEscola.DataSource = ESC_UnidadeEscolaBO.SelecionaEscolasControladas(__SessionWEB.__UsuarioWEB.Usuario.ent_id, 
                                                                                                  __SessionWEB.__UsuarioWEB.Grupo.gru_id, 
                                                                                                  __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                                                                                                  true, GestaoEscolarUtilBO.MinutosCacheLongo);
                    ddlUnidadeEscola.DataBind();

                    VS_FiltroEscola = false;
                }
                else
                {
                    // Carrega as escolas por unidade administrativa superior
                    VS_tua_id = ACA_ParametroAcademicoBO.VerificaFiltroEscolaPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    SYS_TipoUnidadeAdministrativa TipoUnidadeAdm = new SYS_TipoUnidadeAdministrativa { tua_id = VS_tua_id };
                    SYS_TipoUnidadeAdministrativaBO.GetEntity(TipoUnidadeAdm);

                    lblUASuperior.Text = string.IsNullOrEmpty(TipoUnidadeAdm.tua_nome) ? "Unidade administrativa superior" : TipoUnidadeAdm.tua_nome;
                    lblUASuperior.Text += " *";

                    cpvUnidadeAdministrativa.ErrorMessage = lblUASuperior.Text.Replace("*", string.Empty) + " é obrigatório.";

                    if (lblUASuperior.Text.EndsWith(ApplicationWEB.TextoAsteriscoObrigatorio))
                    {
                        textoUASuperior = lblUASuperior.Text.Replace(ApplicationWEB.TextoAsteriscoObrigatorio, string.Empty);
                    }
                    else if (lblUASuperior.Text.EndsWith("*"))
                    {
                        textoUASuperior = lblUASuperior.Text.Replace("*", string.Empty);
                    }

                    ddlUASuperior.Items.Clear();
                    ddlUASuperior.Items.Insert(0, new ListItem("-- Selecione um(a) " + textoUASuperior + " --", Guid.Empty.ToString(), true));
                    ddlUASuperior.DataSource = ESC_UnidadeEscolaBO.GetSelectBy_Pesquisa_PermissaoUsuario_Cache(VS_tua_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, __SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Usuario.usu_id, 0, Guid.Empty, GestaoEscolarUtilBO.MinutosCacheLongo);
                    ddlUASuperior.DataBind();
                    ddlUASuperior.Enabled = true;

                    ddlUnidadeEscola.DataTextField = "esc_uni_nome";
                    ddlUnidadeEscola.Items.Clear();
                    ddlUnidadeEscola.Items.Insert(0, new ListItem("-- Selecione uma escola --", "-1;-1", true));
                    ddlUnidadeEscola.Enabled = false;

                    lblUASuperior.Visible = true;
                    ddlUASuperior.Visible = true;

                    VS_FiltroEscola = true;
                }
            }
        }

        /// <summary>
        /// Atualiza os combos de unidade administrativa superior e de unidade da escola da seguinte forma:
        /// 1) Se o usuário for da visão administrador sempre carrega todas as UA's superior e todas as escolas
        /// 2) Se o usuário for da visão UA ou Gestão e selecionar um aluno matriculado em alguma escola que ele tem permissão,
        ///    carrega todas as UA's superior e todas as escolas
        /// 3) Se o usuário for da visão UA ou Gestão e selecionar um aluno NÃO matriculado em alguma escola que ele tem permissão,
        ///    carrega as UA's superior e escolas que ele tem permissão
        /// </summary>
        private void LoadEscolaDados(Guid uad_idSuperior, bool metodoExecutadoPrimeiraVez)
        {
            string textoUASuperior = string.Empty;

            if (VS_FiltroEscola)
            {
                DataTable dtUA = SYS_UnidadeAdministrativaBO.GetSelectBy_Pesquisa_PermissaoTotal(VS_tua_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                if (lblUASuperior.Text.EndsWith(ApplicationWEB.TextoAsteriscoObrigatorio))
                {
                    textoUASuperior = lblUASuperior.Text.Replace(ApplicationWEB.TextoAsteriscoObrigatorio, string.Empty);
                }
                else if (lblUASuperior.Text.EndsWith("*"))
                {
                    textoUASuperior = lblUASuperior.Text.Replace("*", string.Empty);
                }

                ddlUASuperior.Items.Clear();
                ddlUASuperior.Items.Insert(0, new ListItem("-- Selecione um(a) " + textoUASuperior + " --", Guid.Empty.ToString(), true));
                ddlUASuperior.DataSource = dtUA;
                ddlUASuperior.DataBind();

                // Só seta o valor da unidade administrativa superior quando não é a primeira
                // vez que o método é executado
                if (!metodoExecutadoPrimeiraVez)
                {
                    ddlUASuperior.SelectedValue = uad_idSuperior.ToString();
                }

                ddlUnidadeEscola.Items.Clear();
                ddlUnidadeEscola.Items.Insert(0, new ListItem("-- Selecione uma escola --", "-1;-1", true));
                ddlUnidadeEscola.Enabled = false;
                ddlUnidadeEscola.DataSource = ESC_UnidadeEscolaBO.SelecionaEscolasControladasPorUASuperiorPermissaoTotal(uad_idSuperior, __SessionWEB.__UsuarioWEB.Usuario.ent_id, true, true, 0, GestaoEscolarUtilBO.MinutosCacheLongo);
                ddlUnidadeEscola.DataBind();
            }
            else
            {
                ddlUnidadeEscola.Items.Clear();
                ddlUnidadeEscola.Items.Insert(0, new ListItem("-- Selecione uma escola --", "-1;-1", true));
                ddlUnidadeEscola.Enabled = true;
                ddlUnidadeEscola.DataSource = ESC_UnidadeEscolaBO.GetSelectPermissaoTotal_Cache(__SessionWEB.__UsuarioWEB.Usuario.ent_id, true, true, GestaoEscolarUtilBO.MinutosCacheLongo); ;
                ddlUnidadeEscola.DataBind();
            }
        }

        #endregion

        #region Escolas  para: Adequação, Reclassificação

        /// <summary>
        /// Verifica os parâmetros acadêmicos cadastrados, mostrando e carregando os combos
        /// conforme a configuração. Não mostra o combo de escola, só o de UA.
        /// Se parâmetro FILTRAR_ESCOLA_UA_SUPERIOR = "Sim", mostra combo de Unidade Administrativa,
        /// e carrega no combo pelo tipo de UA que estiver setada no parâmetro
        /// TIPO_UNIDADE_ADMINISTRATIVA_FILTRO_ESCOLA.
        /// Desconsiderando as permissões do usuário
        /// </summary>
        /// <param name="todaEscolas">True para carregar todas as escolas/False para carregar as escolas de permissão do usuário</param>
        private void LoadInicialEscolaDadosAdequacaoReclassificacao(bool todaEscolas)
        {
            string textoUASuperior = string.Empty;

            int cur_id2;
            int crr_id2;

            if (divCurso.Visible)
            {
                cur_id2 = UCComboCursoCurriculo1.Valor[0];
                crr_id2 = UCComboCursoCurriculo1.Valor[1];
            }
            else
            {
                cur_id2 = VS_cur_idAnterior;
                crr_id2 = VS_crr_idAnterior;
            }

            int crp_id2 = divPeriodo.Visible ? UCComboCurriculoPeriodo1.Valor[2] : VS_crp_idAnterior;

            // Carrega todas as escolas do sistema
            if (todaEscolas)
            {
                // Carrega as escolas
                if (!ACA_ParametroAcademicoBO.VerificaFiltroUniAdmSuperiorPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id))
                {
                    lblUASuperiorAdequacaoReclassificao.Visible = false;
                    ddlUASuperiorAdequacaoReclassificao.Visible = false;

                    DataTable dt = ESC_UnidadeEscolaBO.SelecionaEscolasControladasPorCursoPeriodo_PermissaoTotal(cur_id2, crr_id2, crp_id2, __SessionWEB.__UsuarioWEB.Usuario.ent_id, true, true);

                    ddlUnidadeEscolaAdequacaoReclassificao.DataTextField = "uni_escolaNome";
                    ddlUnidadeEscolaAdequacaoReclassificao.Items.Clear();
                    ddlUnidadeEscolaAdequacaoReclassificao.Items.Insert(0, new ListItem("-- Selecione uma escola --", "-1;-1", true));
                    ddlUnidadeEscolaAdequacaoReclassificao.DataSource = dt;
                    ddlUnidadeEscolaAdequacaoReclassificao.DataBind();

                    VS_FiltroEscola = false;
                }
                else
                {
                    // Carrega as escolas por unidade administrativa superior
                    VS_tua_id = ACA_ParametroAcademicoBO.VerificaFiltroEscolaPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    SYS_TipoUnidadeAdministrativa TipoUnidadeAdm = new SYS_TipoUnidadeAdministrativa { tua_id = VS_tua_id };
                    SYS_TipoUnidadeAdministrativaBO.GetEntity(TipoUnidadeAdm);

                    lblUASuperiorAdequacaoReclassificao.Text = string.IsNullOrEmpty(TipoUnidadeAdm.tua_nome) ? "Unidade administrativa superior" : TipoUnidadeAdm.tua_nome;
                    lblUASuperiorAdequacaoReclassificao.Text += " *";

                    cpvUnidadeAdministrativaAdequacaoReclassificao.ErrorMessage = lblUASuperiorAdequacaoReclassificao.Text.Replace("*", string.Empty) + " é obrigatório.";

                    if (lblUASuperiorAdequacaoReclassificao.Text.EndsWith(ApplicationWEB.TextoAsteriscoObrigatorio))
                    {
                        textoUASuperior = lblUASuperiorAdequacaoReclassificao.Text.Replace(ApplicationWEB.TextoAsteriscoObrigatorio, string.Empty);
                    }
                    else if (lblUASuperiorAdequacaoReclassificao.Text.EndsWith("*"))
                    {
                        textoUASuperior = lblUASuperiorAdequacaoReclassificao.Text.Replace("*", string.Empty);
                    }

                    DataTable dt = SYS_UnidadeAdministrativaBO.GetSelectBy_Pesquisa_PermissaoTotal(VS_tua_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                    ddlUASuperiorAdequacaoReclassificao.Items.Clear();
                    ddlUASuperiorAdequacaoReclassificao.Items.Insert(0, new ListItem("-- Selecione um(a) " + textoUASuperior + " --", Guid.Empty.ToString(), true));
                    ddlUASuperiorAdequacaoReclassificao.DataSource = dt;
                    ddlUASuperiorAdequacaoReclassificao.DataBind();

                    ddlUnidadeEscolaAdequacaoReclassificao.DataTextField = "esc_uni_nome";
                    ddlUnidadeEscolaAdequacaoReclassificao.Items.Clear();
                    ddlUnidadeEscolaAdequacaoReclassificao.Items.Insert(0, new ListItem("-- Selecione uma escola --", "-1;-1", true));
                    ddlUnidadeEscolaAdequacaoReclassificao.Enabled = false;

                    lblUASuperiorAdequacaoReclassificao.Visible = true;
                    ddlUASuperiorAdequacaoReclassificao.Visible = true;

                    VS_FiltroEscola = true;
                }
            }
            else
            {
                // Carrega as escola que o usuário tem permissão
                if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa ||
                    !ACA_ParametroAcademicoBO.VerificaFiltroUniAdmSuperiorPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id))
                {
                    lblUASuperiorAdequacaoReclassificao.Visible = false;
                    ddlUASuperiorAdequacaoReclassificao.Visible = false;

                    ddlUnidadeEscolaAdequacaoReclassificao.Enabled = true;
                    ddlUnidadeEscolaAdequacaoReclassificao.DataTextField = "uni_escolaNome";
                    ddlUnidadeEscolaAdequacaoReclassificao.Items.Clear();
                    ddlUnidadeEscolaAdequacaoReclassificao.Items.Insert(0, new ListItem("-- Selecione uma escola --", "-1;-1", true));
                    ddlUnidadeEscolaAdequacaoReclassificao.DataSource = ESC_UnidadeEscolaBO.SelecionaEscolasControladas(__SessionWEB.__UsuarioWEB.Usuario.ent_id, 
                                                                        __SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Usuario.usu_id,
                                                                        true, GestaoEscolarUtilBO.MinutosCacheLongo);
                    ddlUnidadeEscolaAdequacaoReclassificao.DataBind();

                    VS_FiltroEscola = false;
                }
                else
                {
                    // Carrega as escolas por unidade administrativa superior
                    VS_tua_id = ACA_ParametroAcademicoBO.VerificaFiltroEscolaPorEntidade(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    SYS_TipoUnidadeAdministrativa TipoUnidadeAdm = new SYS_TipoUnidadeAdministrativa { tua_id = VS_tua_id };
                    SYS_TipoUnidadeAdministrativaBO.GetEntity(TipoUnidadeAdm);

                    lblUASuperiorAdequacaoReclassificao.Text = string.IsNullOrEmpty(TipoUnidadeAdm.tua_nome) ? "Unidade administrativa superior" : TipoUnidadeAdm.tua_nome;
                    lblUASuperiorAdequacaoReclassificao.Text += " *";

                    cpvUnidadeAdministrativa.ErrorMessage = lblUASuperiorAdequacaoReclassificao.Text.Replace("*", string.Empty) + " é obrigatório.";

                    if (lblUASuperiorAdequacaoReclassificao.Text.EndsWith(ApplicationWEB.TextoAsteriscoObrigatorio))
                    {
                        textoUASuperior = lblUASuperiorAdequacaoReclassificao.Text.Replace(ApplicationWEB.TextoAsteriscoObrigatorio, string.Empty);
                    }
                    else if (lblUASuperiorAdequacaoReclassificao.Text.EndsWith("*"))
                    {
                        textoUASuperior = lblUASuperiorAdequacaoReclassificao.Text.Replace("*", string.Empty);
                    }

                    ddlUASuperiorAdequacaoReclassificao.Items.Clear();
                    ddlUASuperiorAdequacaoReclassificao.Items.Insert(0, new ListItem("-- Selecione um(a) " + textoUASuperior + " --", Guid.Empty.ToString(), true));
                    ddlUASuperiorAdequacaoReclassificao.DataSource = ESC_UnidadeEscolaBO.GetSelectBy_Pesquisa_PermissaoUsuario_Cache(VS_tua_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, __SessionWEB.__UsuarioWEB.Grupo.gru_id, __SessionWEB.__UsuarioWEB.Usuario.usu_id, 0, Guid.Empty, GestaoEscolarUtilBO.MinutosCacheLongo);
                    ddlUASuperiorAdequacaoReclassificao.DataBind();
                    ddlUASuperiorAdequacaoReclassificao.Enabled = true;

                    ddlUnidadeEscolaAdequacaoReclassificao.DataTextField = "esc_uni_nome";
                    ddlUnidadeEscolaAdequacaoReclassificao.Items.Clear();
                    ddlUnidadeEscolaAdequacaoReclassificao.Items.Insert(0, new ListItem("-- Selecione uma escola --", "-1;-1", true));
                    ddlUnidadeEscolaAdequacaoReclassificao.Enabled = false;

                    lblUASuperiorAdequacaoReclassificao.Visible = true;
                    ddlUASuperiorAdequacaoReclassificao.Visible = true;

                    VS_FiltroEscola = true;
                }
            }
        }

        /// <summary>
        /// Atualiza os combos de unidade administrativa superior e de unidade da escola da seguinte forma:
        /// 1) Se o usuário for da visão administrador sempre carrega todas as UA's superior e todas as escolas
        /// 2) Se o usuário for da visão UA ou Gestão e selecionar um aluno matriculado em alguma escola que ele tem permissão,
        ///    carrega todas as UA's superior e todas as escolas
        /// 3) Se o usuário for da visão UA ou Gestão e selecionar um aluno NÃO matriculado em alguma escola que ele tem permissão,
        ///    carrega as UA's superior e escolas que ele tem permissão
        /// </summary>
        private void LoadEscolaDadosAdequacaoReclassificacao(Guid uad_idSuperior, bool metodoExecutadoPrimeiraVez)
        {
            string textoUASuperior = string.Empty;

            int cur_id2;
            int crr_id2;

            if (divCurso.Visible)
            {
                cur_id2 = UCComboCursoCurriculo1.Valor[0];
                crr_id2 = UCComboCursoCurriculo1.Valor[1];
            }
            else
            {
                cur_id2 = VS_cur_idAnterior;
                crr_id2 = VS_crr_idAnterior;
            }

            int crp_id2 = divPeriodo.Visible ? UCComboCurriculoPeriodo1.Valor[2] : VS_crp_idAnterior;

            if (VS_FiltroEscola)
            {
                DataTable dtUA = SYS_UnidadeAdministrativaBO.GetSelectBy_Pesquisa_PermissaoTotal(VS_tua_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

                if (lblUASuperiorAdequacaoReclassificao.Text.EndsWith(ApplicationWEB.TextoAsteriscoObrigatorio))
                {
                    textoUASuperior = lblUASuperiorAdequacaoReclassificao.Text.Replace(ApplicationWEB.TextoAsteriscoObrigatorio, string.Empty);
                }
                else if (lblUASuperiorAdequacaoReclassificao.Text.EndsWith("*"))
                {
                    textoUASuperior = lblUASuperiorAdequacaoReclassificao.Text.Replace("*", string.Empty);
                }

                ddlUASuperiorAdequacaoReclassificao.Items.Clear();
                ddlUASuperiorAdequacaoReclassificao.Items.Insert(0, new ListItem("-- Selecione um(a) " + textoUASuperior + " --", Guid.Empty.ToString(), true));
                ddlUASuperiorAdequacaoReclassificao.DataSource = dtUA;
                ddlUASuperiorAdequacaoReclassificao.DataBind();

                // Só seta o valor da unidade administrativa superior quando não é a primeira
                // vez que o método é executado
                if (!metodoExecutadoPrimeiraVez)
                {
                    ddlUASuperiorAdequacaoReclassificao.SelectedValue = uad_idSuperior.ToString();
                }

                DataTable dt = ESC_UnidadeEscolaBO.SelecionaEscolasControladasPorUASuperiorCursoPeriodo_PermissaoTotal(uad_idSuperior, cur_id2, crr_id2, crp_id2, __SessionWEB.__UsuarioWEB.Usuario.ent_id, true, true);

                ddlUnidadeEscolaAdequacaoReclassificao.Items.Clear();
                ddlUnidadeEscolaAdequacaoReclassificao.Items.Insert(0, new ListItem("-- Selecione uma escola --", "-1;-1", true));
                ddlUnidadeEscolaAdequacaoReclassificao.Enabled = false;
                ddlUnidadeEscolaAdequacaoReclassificao.DataSource = dt;
                ddlUnidadeEscolaAdequacaoReclassificao.DataBind();
            }
            else
            {
                DataTable dt = ESC_UnidadeEscolaBO.SelecionaEscolasControladasPorCursoPeriodo_PermissaoTotal(cur_id2, crr_id2, crp_id2, __SessionWEB.__UsuarioWEB.Usuario.ent_id, true, true);

                ddlUnidadeEscolaAdequacaoReclassificao.Items.Clear();
                ddlUnidadeEscolaAdequacaoReclassificao.Items.Insert(0, new ListItem("-- Selecione uma escola --", "-1;-1", true));
                ddlUnidadeEscolaAdequacaoReclassificao.Enabled = true;
                ddlUnidadeEscolaAdequacaoReclassificao.DataSource = dt;
                ddlUnidadeEscolaAdequacaoReclassificao.DataBind();
            }
        }

        #endregion

        /// <summary>
        /// Carregas the escolas.
        /// </summary>
        /// <datetime>07/10/2013-11:04</datetime>
        private void CarregaEscolas()
        {
            ddlUnidadeEscola.DataTextField = "uni_escolaNome";
            ddlUnidadeEscola.Items.Clear();
            ddlUnidadeEscola.Items.Insert(0, new ListItem("-- Selecione uma escola --", "-1;-1", true));
            ddlUnidadeEscola.DataSource = ESC_UnidadeEscolaBO.GetSelectPermissaoTotal_Cache(__SessionWEB.__UsuarioWEB.Usuario.ent_id, false, true, GestaoEscolarUtilBO.MinutosCacheLongo);
            ddlUnidadeEscola.DataBind();
        }

        /// <summary>
        /// Unidades the escola_ index changed.
        /// </summary>
        /// <datetime>07/10/2013-11:06</datetime>
        private void UnidadeEscola_IndexChanged()
        {
            try
            {
                UCComboCursoCurriculo1.Valor = new[] { -1, -1 };
                UCComboCursoCurriculo1.PermiteEditar = ddlUnidadeEscola.SelectedValue != "-1;-1";

                UCComboCurriculoPeriodo1.Valor = new[] { -1, -1, -1 };
                UCComboCurriculoPeriodo1.PermiteEditar = false;

                divPeriodoAvaliacao.Visible = false;
                UCComboCurriculoPeriodoAvaliacao1.Valor = -1;
                UCComboCurriculoPeriodoAvaliacao1.PermiteEditar = false;
                rdbAvaliadoPeriodoCorrente.Enabled = false;

                UCComboTurma1._Combo.SelectedValue = "-1;-1;-1";
                UCComboTurma1._Combo.Enabled = false;

                UCComboCursoCurriculo1.CancelSelect = false;

                if ((TipoMovimentoSelecionado == MTR_TipoMovimentacaoTipoMovimento.TransferenciaPropriaRede) ||
                    (TipoMovimentoSelecionado == MTR_TipoMovimentacaoTipoMovimento.Remanejamento))
                {
                    UCComboCursoCurriculo1.CarregaCursos_RelacionadosVigentes_Por_Escola(
                        VS_cur_idAnterior,
                        VS_crr_idAnterior,
                        Convert.ToInt32(ddlUnidadeEscola.SelectedValue.Split(';')[0]),
                        Convert.ToInt32(ddlUnidadeEscola.SelectedValue.Split(';')[1]));

                    UCComboCursoCurriculo1.PermiteEditar = true;

                    UCComboCursoCurriculo1.SelecionaPrimeiroItem();
                }
                else
                {
                    // Se for uma nova movimentação carrega apenas os cursos ativos
                    // Se for uma alteração de movimentação, carrega todos os cursos
                    byte cur_situacao = Convert.ToByte(VS_mov_id > 0 ? 0 : 1);
                    UCComboCursoCurriculo1.CarregarCursoCurriculoPorEscola(Convert.ToInt32(ddlUnidadeEscola.SelectedValue.Split(';')[0]), Convert.ToInt32(ddlUnidadeEscola.SelectedValue.Split(';')[1]), cur_situacao);
                }

                // Se for mudança de modalidade de ensino
                if (TipoMovimentoSelecionado == MTR_TipoMovimentacaoTipoMovimento.MudancaModalidaeEnsino)
                {
                    if (ddlUnidadeEscola.SelectedValue.Split(';')[0] == VS_esc_idAnterior.ToString()
                        && ddlUnidadeEscola.SelectedValue.Split(';')[1] == VS_uni_idAnterior.ToString())
                    {
                        UCComboCurriculoPeriodoAvaliacao1.Obrigatorio = true;
                        UCComboCurriculoPeriodoAvaliacao1.ValidationGroup = "Aluno";

                        UCComboTurma1.PermiteEditar = false;
                        UCComboTurma1.ValidationGroup = "Aluno";
                        UCComboTurma1.Obrigatorio = true;
                        divTurma.Visible = true;

                        divRespLancNota.Visible = false;
                        pnlResponsavelTransferencia.Visible = false;
                    }
                    else
                    {
                        divTurma.Visible = false;
                        divPeriodoAvaliacao.Visible = false;
                        divRespLancNota.Visible = !MTR_MovimentacaoBO.VerificaEfetivacaoEscolaOrigem(VS_tur_idAnterior, DateTime.Now.Date, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                        pnlResponsavelTransferencia.Visible = true;
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
        /// Seta a propriedade Enabled passada para todos os WebControl do ControlCollection
        /// passado.
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="enabled"></param>
        private void HabilitaControles(ControlCollection controls, bool enabled)
        {
            foreach (Control c in controls)
            {
                if (c.Controls.Count > 0)
                    HabilitaControles(c.Controls, enabled);

                WebControl wb = c as WebControl;

                if (wb != null)
                    wb.Enabled = enabled;
            }
        }

        #endregion Métodos

    }
}