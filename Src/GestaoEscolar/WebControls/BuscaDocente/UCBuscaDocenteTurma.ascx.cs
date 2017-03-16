using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.WebControls.BuscaDocente
{
    public partial class UCBuscaDocenteTurma : MotherUserControl
    {
        #region Propriedades

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
        /// Propridade usada para permitir gerar relatório sem selecionar a escola
        /// </summary>
        public bool _VS_PermiteSemEscola
        {
            get
            {
                if (ViewState["_VS_PermiteSemEscola"] != null)
                    return Convert.ToBoolean(ViewState["_VS_PermiteSemEscola"]);
                return false;
            }
            set
            {
                ViewState["_VS_PermiteSemEscola"] = value;
            }
        }

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
        /// Propriedade na qual cria variavel em ViewState armazenando valor do id da escola
        /// </summary>
        public int _VS_esc_id
        {
            get
            {
                if (ViewState["_VS_esc_id"] != null)
                    return Convert.ToInt32(ViewState["_VS_esc_id"]);
                return -1;
            }
            set
            {
                ViewState["_VS_esc_id"] = value;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor do id da unidade
        /// </summary>
        public int _VS_uni_id
        {
            get
            {
                if (ViewState["_VS_uni_id"] != null)
                    return Convert.ToInt32(ViewState["_VS_uni_id"]);
                return -1;
            }
            set
            {
                ViewState["_VS_uni_id"] = value;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor de doc_id
        /// </summary>
        public long _VS_doc_id
        {
            get
            {
                if (ViewState["_VS_doc_id"] != null)
                    return Convert.ToInt64(ViewState["_VS_doc_id"]);
                return -1;
            }
            set
            {
                ViewState["_VS_doc_id"] = value;
            }
        }

        /// <summary>
        /// Propriedade na qual cria variavel em ViewState armazenando valor do id da unidade
        /// </summary>
        public int Tci_id
        {
            get
            {
                return UCComboTipoCiclo.Valor;
            }
            set
            {
                UCComboTipoCiclo.Valor = value;
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
        /// Propriedade que retorna o combo de escolas
        /// </summary>
        public Combos.UCComboUAEscola ComboEscola
        {
            get
            {
                return UCComboUAEscola;
            }
        }

        /// <summary>
        /// Propriedade que retorna o combo de cursos
        /// </summary>
        public Combos.Novos.UCCCursoCurriculo ComboCursoCurriculo
        {
            get
            {
                return UCCCursoCurriculo;
            }
        }

        /// <summary>
        /// Propriedade que retorna o combo de séries
        /// </summary>
        public WebControls_Combos_UCComboCurriculoPeriodo ComboCurriculoPeriodo
        {
            get
            {
                return UCComboCurriculoPeriodo;
            }
        }

        /// <summary>
        /// Propriedade que retorna o combo de calendários
        /// </summary>
        public Combos.Novos.UCCCalendario ComboCalendario
        {
            get
            {
                return UCCCalendario;
            }
        }

        /// <summary>
        /// Propriedade que retorna o combo de turmas
        /// </summary>
        public Combos.Novos.UCCTurma ComboTurma
        {
            get
            {
                return UCComboTurma;
            }
        }

        public bool mostraDivComboCursoTurma
        {
            set
            {
                divComboCursoTurma.Visible = value;
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

        #endregion Propriedades

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            // Seta o metodo criado no delegate ao evento do componente
            UCComboUAEscola.IndexChangedUnidadeEscola += UCComboUAEscola_IndexChangedUnidadeEscola;
            UCComboUAEscola.IndexChangedUA += UCComboUAEscola_IndexChangedUA;
            UCCCursoCurriculo.IndexChanged += UCCCursoCurriculo_IndexChanged;
            UCCCalendario.IndexChanged += UCCCalendario_IndexChanged;
            UCComboTipoCiclo.IndexChanged += UCComboTipoCiclo_IndexChanged;
            UCComboCurriculoPeriodo._OnSelectedIndexChange += UCComboCurriculoPeriodo__OnSelectedIndexChange;
            UCComboTurma.IndexChanged += UCComboTurma_IndexChanged;
        }

        /// <summary>
        /// Inicializa os combos na tela quando a visão do usuário é docente.
        /// </summary>
        public bool CarregaTelaInicialVisaoDocente()
        {
            if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual)
            {
                // Busca o doc_id do usuário logado.
                if (__SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
                {
                    //Esconde os campos não visíveis para docentes
                    UCComboUAEscola.Visible = false;
                    UCCCursoCurriculo.Visible = false;
                    UCComboCurriculoPeriodo.Visible = false;

                    //Seta o docente
                    _VS_doc_id = __SessionWEB.__UsuarioWEB.Docente.doc_id;

                    //Inicializa os campos de busca para visão individual (docentes)
                    //Carrega os campos
                    if (_VS_MostarComboEscola)
                    {
                        UCComboUAEscola.Visible = true;
                        UCComboUAEscola.InicializarVisaoIndividual
                            (_VS_doc_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                                (byte)(BuscaEscolasPorVinculoColaboradorDocente ? 1 : 3));

                        UCComboTurma.Obrigatorio = true;

                        if (UCComboUAEscola.QuantidadeItemsComboEscolas == 2)
                        {
                            _VS_esc_id = UCComboUAEscola.Esc_ID;
                            _VS_uni_id = UCComboUAEscola.Uni_ID;
                        }

                        UCComboUAEscola_IndexChangedUA();
                    }
                    else
                    {
                        if (UCCCalendario.Valor > 0)
                        {
                            UCComboTurma.CarregarPorDocente(_VS_doc_id, 0, _VS_AnosAnteriores ? UCCCalendario.Valor : 0, false, _VS_CarregarApenasTurmasNormais, _VS_MostraTurmasEletivas);
                            UCComboTurma.PermiteEditar = true;
                            UCComboTurma.Obrigatorio = true;
                        }
                    }

                    return true;
                }
                else
                {
                    divPesquisa.Visible = false;
                    lblMessage.Text = UtilBO.GetErroMessage("Essa tela é exclusiva para docentes.", UtilBO.TipoMensagem.Alerta);
                }
            }

            return false;
        }

        protected void ddlTurma_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IndexChanged_Turma != null)
                IndexChanged_Turma();
        }

        #endregion Eventos

        #region Delegates

        /// <summary>
        /// Verifica alteracao do index do combo UA Escola e trata o combo Escola e Curso curriculo.
        /// </summary>
        protected void UCComboUAEscola_IndexChangedUA()
        {
            UCComboUAEscola.MostraApenasAtivas = true;

            //UCComboUAEscola.SelectedValueEscolas = new[] { _VS_esc_id, _VS_uni_id };
            //UCComboUAEscola.SelectedValueEscolas = new[] { -1, -1 };

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

                if (UCComboUAEscola.Esc_ID > 0 && UCComboUAEscola.Uni_ID > 0)
                {
                    if (!_VS_AnosAnteriores)
                    {
                        UCCCalendario.CarregarCalendarioAnualAnoAtualEscola(UCComboUAEscola.Esc_ID);
                    }
                    else
                    {
                        UCCCalendario.CarregarPorEscola(UCComboUAEscola.Esc_ID);
                    }
                    UCCCalendario.SetarFoco();
                    UCCCalendario.PermiteEditar = true;
                }
                else if (_VS_PermiteSemEscola && _VS_alteraCalendario && UCComboUAEscola.Uad_ID != Guid.Empty)
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
                    if (__SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.Individual)
                    {
                        if (IndexChanged_Turma != null)
                            IndexChanged_Turma();
                    }

                    UCCCursoCurriculo.CarregarPorEscolaCalendarioSituacaoCursoNivelEnsino(UCComboUAEscola.Esc_ID, UCComboUAEscola.Uni_ID, UCCCalendario.Valor, 0, VS_FiltroTipoNivelEnsino);

                    UCCCursoCurriculo.SetarFoco();
                    UCCCursoCurriculo.PermiteEditar = true;
                }

                if (IndexChanged_Calendario != null)
                    IndexChanged_Calendario();
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

                        UCComboCurriculoPeriodo__OnSelectedIndexChange();
                    }
                }
                else
                {
                    UCComboCurriculoPeriodo__OnSelectedIndexChange();
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

                UCComboCurriculoPeriodo__OnSelectedIndexChange();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Verifica alteracao do index do combo curriculoperiodo e trata o combo turma
        /// </summary>
        public void UCComboCurriculoPeriodo__OnSelectedIndexChange()
        {
            try
            {
                if (UCComboTurma.Visible)
                {
                    UCComboTurma.Valor = new long[] { -1, -1, -1 };
                    //Condição usada na tela de Documentos do docente
                    if (_VS_MostarComboEscola)
                    {
                        //Carrega as turmas
                        if (UCComboUAEscola.Esc_ID > 0 && UCCCalendario.Valor > 0)
                            UCComboTurma.CarregarPorDocente(_VS_doc_id, UCComboUAEscola.Esc_ID, UCCCalendario.Valor, false, _VS_CarregarApenasTurmasNormais, _VS_MostraTurmasEletivas);

                        UCComboTurma.PermiteEditar = UCComboUAEscola.Esc_ID > 0 && UCCCalendario.Valor > 0;
                    }
                    else
                    {
                        UCComboTurma.PermiteEditar = _VS_doc_id > 0 && UCCCalendario.Valor > 0;

                        if (UCComboCurriculoPeriodo.Valor[0] > 0 && UCComboCurriculoPeriodo.Valor[1] > 0 && UCComboCurriculoPeriodo.Valor[2] > 0 &&
                            UCComboUAEscola.Esc_ID > 0 && UCComboUAEscola.Uni_ID > 0 && UCCCalendario.Valor > 0)
                        {
                            if (_VS_CarregarApenasTurmasNormais)
                            {
                                UCComboTurma.CarregarPorEscolaCurriculoCalendario_TurmasNormais(UCComboUAEscola.Esc_ID,
                                                                                        UCComboUAEscola.Uni_ID,
                                                                                        UCCCursoCurriculo.Valor[0],
                                                                                        UCCCursoCurriculo.Valor[1],
                                                                                        UCComboCurriculoPeriodo.Valor[2],
                                                                                        UCCCalendario.Valor, _VS_MostraTurmasEletivas);
                            }
                            else
                            {
                                UCComboTurma.CarregaPorEscolaCurriculoPeriodoCalendario(UCComboUAEscola.Esc_ID,
                                                                                        UCComboUAEscola.Uni_ID,
                                                                                        UCCCursoCurriculo.Valor[0],
                                                                                        UCCCursoCurriculo.Valor[1],
                                                                                        UCComboCurriculoPeriodo.Valor[2],
                                                                                        UCCCalendario.Valor, 0, _VS_MostraTurmasEletivas);
                            }
                            UCComboTurma.SetarFoco();
                            UCComboTurma.PermiteEditar = true;
                        }
                    }
                }
                if (IndexChanged_CurriculoPeriodo != null)
                    IndexChanged_CurriculoPeriodo();
                if (UCComboTurma.Visible)
                    UCComboTurma_IndexChanged();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Delegate do combo de turma.
        /// </summary>
        private void UCComboTurma_IndexChanged()
        {
            if (IndexChanged_Turma != null)
                IndexChanged_Turma();
        }

        public delegate void SelectedIndexChangedTurma();
        public event SelectedIndexChangedTurma IndexChanged_Turma;
        public delegate void SelectedIndexChangedCalendario();
        public event SelectedIndexChangedCalendario IndexChanged_Calendario;
        public delegate void SelectedIndexChangedCurriculoPeriodo();
        public event SelectedIndexChangedCurriculoPeriodo IndexChanged_CurriculoPeriodo;

        #endregion Delegates

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

            // Carrega combos quando a visão é docente, e retorna a flag indicando se é docente o usuário logado.
            if (!CarregaTelaInicialVisaoDocente())
            {
                UCComboUAEscola_IndexChangedUA();
            }

        }

        #endregion Métodos

    }
}