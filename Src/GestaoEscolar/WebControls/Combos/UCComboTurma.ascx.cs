using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class WebControls_Combos_UCComboTurma : MotherUserControl
{
    #region Delegates

    public delegate void onSelecionaTurma();

    public event onSelecionaTurma _SelecionaTurma;

    #endregion Delegates

    #region Propriedades

    /// <summary>
    /// Propriedade que seta o Width do combo.   
    /// </summary>
    public int WidthCombo
    {
        set
        {
            _ddlTurma.Width = value;
        }
    }

    /// <summary>
    /// Indica se é para mostrar os dados adicionais da turma quando selecionada no combo.
    /// </summary>
    public bool MostraDadosAdicionais
    {
        get
        {
            if (ViewState["MostraDadosAdicionais"] == null)
                return false;

            return (bool)ViewState["MostraDadosAdicionais"];
        }
        set
        {
            ViewState["MostraDadosAdicionais"] = value;
        }
    }

    /// <summary>
    /// Indica se é para mostrar os dados adicionais da turma dentro do combo quando selecionada no combo.
    /// </summary>
    public bool MostraDadosAdicionaisInternos
    {
        get
        {
            if (ViewState["MostraDadosAdicionaisInternos"] == null)
                return false;

            return (bool)ViewState["MostraDadosAdicionaisInternos"];
        }
        set
        {
            ViewState["MostraDadosAdicionaisInternos"] = value;
        }
    }

    private bool cancelSelect;

    /// <summary>
    /// Cancela o consulta do ObjectDataSource ao carregar a página pela primeira vez.
    /// </summary>
    public bool CancelSelect
    {
        get { return cancelSelect; }
        set { cancelSelect = value; }
    }

    /// <summary>
    /// Retorna e seta o valor selecionado no combo.
    /// valor[0] = tur_id
    /// valor[1] = crp_id
    /// valor[2] = ttn_id
    /// </summary>
    public Int64[] Valor
    {
        get
        {
            string[] s = _ddlTurma.SelectedValue.Split(';');

            if (s.Length == 3)
                return new[] { Convert.ToInt64(s[0]), Convert.ToInt64(s[1]), Convert.ToInt64(s[2]) };

            return new Int64[] { -1, -1, -1 };
        }
        set
        {
            string s;
            if (value.Length == 3)
                s = value[0] + ";" + value[1] + ";" + value[2];
            else
                s = "-1;-1;-1";

            _ddlTurma.SelectedValue = s;

            if ((_ddlTurma.SelectedIndex > 0) && (MostraDadosAdicionais))
            {
                SetaDadosAdicionais();
            }
        }
    }

    public DropDownList _Combo
    {
        get
        {
            return _ddlTurma;
        }
        set
        {
            _ddlTurma = value;
        }
    }

    /// <summary>
    /// Atribui valores para o label
    /// </summary>
    public Label _Label
    {
        get
        {
            return lblTurmas;
        }
        set
        {
            lblTurmas = value;
        }
    }

    public bool _MostrarMessageSelecione
    {
        set
        {
            if (value && (_ddlTurma.Items.FindByValue("-1;-1;-1") == null))
                _ddlTurma.Items.Insert(0, new ListItem("-- Selecione uma turma --", "-1;-1;-1", true));
            _ddlTurma.AppendDataBoundItems = value;
        }
    }

    public CompareValidator _Validator
    {
        get
        {
            return _cpvCombo;
        }
        set
        {
            _cpvCombo = value;
        }
    }

    /// <summary>
    /// Deixa o combo habilitado de acordo com o valor passado
    /// </summary>
    public bool PermiteEditar
    {
        set
        {
            _ddlTurma.Enabled = value;
        }
        get
        {
            return _ddlTurma.Enabled;
        }
    }

    /// <summary>
    /// Propriedade que seta a label e a validação do combo
    /// </summary>
    public bool Obrigatorio
    {
        set
        {
            if (value)
            {
                AdicionaAsteriscoObrigatorio(lblTurmas);
            }
            else
            {
                RemoveAsteriscoObrigatorio(lblTurmas);
            }

            _cpvCombo.Visible = value;
        }
    }

    /// <summary>
    /// Propriedade que seta o SelectedIndex do Combo.       
    /// </summary>
    public int SelectedIndex
    {
        set
        {
            _ddlTurma.SelectedValue = _ddlTurma.Items[value].Value; 
        }
        get
        {
            return _ddlTurma.SelectedIndex;
        }
    }

    /// <summary>
    /// Propriedade que verifica quantos items existem no combo
    /// </summary>
    public int QuantidadeItemsCombo
    {
        get
        {
            return _ddlTurma.Items.Count;
        }
    }

    /// <summary>
    /// Seta o validationGroup do combo.
    /// </summary>
    public string ValidationGroup
    {
        set
        {
            _cpvCombo.ValidationGroup = value;
        }
    }

    /// <summary>
    /// Propriedade para controlar o viewstate dos controles:
    /// odsTurma
    /// </summary>
    public bool GuardaViewState
    {
        get
        {
            return odsTurma.EnableViewState;
        }
        set
        {
            odsTurma.EnableViewState = value;
        }
    }

    #endregion Propriedades

    #region Métodos

    /// <summary>
    /// Seta nos labels de dados adicionais os valores referentes a turma.
    /// </summary>
    private void SetaDadosAdicionais()
    {
        try
        {
            divDadosTurma.Visible = MostraDadosAdicionais;

            if (divDadosTurma.Visible)
            {
                int qtVagas, qtMatriculados;
                TUR_TurmaBO.RetornaVagasMatriculadosPor_Turma(Valor[0], out qtVagas, out qtMatriculados);

                lblCapacidade.Text = "<b>Capacidade: " + qtVagas + "</b><br />";
                lblMatriculados.Text = "<b>Matriculados: " + qtMatriculados + "</b>";
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblCapacidade.Text = "";
            lblMatriculados.Text = "";
        }
    }

    /// <summary>
    /// Coloca dados de capacidade e matriculados em cada item do combo.
    /// </summary>
    private void SetaDadosAdicionaisInternos()
    {
        try
        {
            if (MostraDadosAdicionaisInternos && _ddlTurma.Items.Count > 0)
            {
                foreach (ListItem item in _ddlTurma.Items)
                {
                    string[] idTurma = item.Value.Split(';');
                    if (Convert.ToInt64(idTurma[0]) > -1)
                    {
                        int qtVagas, qtMatriculados;
                        TUR_TurmaBO.RetornaVagasMatriculadosPor_Turma(Convert.ToInt64(idTurma[0]), out qtVagas, out qtMatriculados);

                        item.Text += " - Capacidade: " + qtVagas;
                        item.Text += " - Matriculados: " + qtMatriculados;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblCapacidade.Text = "";
            lblMatriculados.Text = "";
        }
    }

    /// <summary>
    /// Seta o foco no combo    
    /// </summary>
    public void SetarFoco()
    {
        _ddlTurma.Focus();
    }

    /// <summary>
    /// Verifica se o combo possui somente um item e seleciona o primeiro.
    /// </summary>
    public bool SelecionaPrimeiroItem()
    {
        if ((QuantidadeItemsCombo == 2) &&
            (Valor[0] == -1))
        {
            // Seleciona o primeiro item.
            _ddlTurma.SelectedValue = _ddlTurma.Items[1].Value;

            _ddlTurma_SelectedIndexChanged(this, null);

            return true;
        }

        return false;
    }

    /// <summary>
    /// Usado para carregar combo com turmas de uma escola
    /// e com todos CalendarioAno de um ano
    /// </summary>
    /// <param name="esc_id">id da escola</param>
    /// <param name="uni_id">unidade adm da escola</param>
    /// <param name="cal_ano">ano dos calendarios da escola</param>
    public void _LoadBy_EscolaAno(int esc_id, int uni_id, int cal_ano)
    {
        // Ok
        odsTurma.SelectParameters.Clear();
        odsTurma.SelectParameters.Add("esc_id", esc_id.ToString());
        odsTurma.SelectParameters.Add("uni_id", uni_id.ToString());
        odsTurma.SelectParameters.Add("cal_ano", DbType.Int32, cal_ano.ToString());

        odsTurma.SelectMethod = "GetSelectBy_Escola_Ano";

        _ddlTurma.Items.Clear();
        _ddlTurma.Items.Insert(0, new ListItem("-- Selecione uma turma --", "-1;-1;-1"));
        _ddlTurma.DataBind();
        _ddlTurma.SelectedValue = "-1;-1;-1";
    }

    public void _LoadBy_Escola_CurriculoPeriodo(int esc_id, int uni_id, int cur_id, int crr_id, int crp_id, int cal_id)
    {
        // Ok
        odsTurma.SelectParameters.Clear();
        odsTurma.SelectParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
        odsTurma.SelectParameters.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
        odsTurma.SelectParameters.Add("adm", (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao).ToString());
        odsTurma.SelectParameters.Add("esc_id", esc_id.ToString());
        odsTurma.SelectParameters.Add("uni_id", uni_id.ToString());
        odsTurma.SelectParameters.Add("cal_id", cal_id.ToString());
        odsTurma.SelectParameters.Add("cur_id", cur_id.ToString());
        odsTurma.SelectParameters.Add("crr_id", crr_id.ToString());
        odsTurma.SelectParameters.Add("crp_id", crp_id.ToString());
        odsTurma.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsTurma.SelectParameters.Add("tur_tipo", "0");
        odsTurma.SelectParameters.Add("tur_situacao", "0");
        odsTurma.SelectParameters.Add("appMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());

        odsTurma.SelectMethod = "GetSelectBy_Escola_Periodo_Situacao";

        _ddlTurma.DataBind();
    }

    public void _LoadBy_Escola_CurriculoPeriodo_Situacao(int esc_id, int uni_id, int cur_id, int crr_id, int crp_id, byte tur_situacao)
    {
        // Ok
        odsTurma.SelectParameters.Clear();
        odsTurma.SelectParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
        odsTurma.SelectParameters.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
        odsTurma.SelectParameters.Add("adm", (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao).ToString());
        odsTurma.SelectParameters.Add("esc_id", esc_id.ToString());
        odsTurma.SelectParameters.Add("uni_id", uni_id.ToString());
        odsTurma.SelectParameters.Add("cal_id", "-1");
        odsTurma.SelectParameters.Add("cur_id", cur_id.ToString());
        odsTurma.SelectParameters.Add("crr_id", crr_id.ToString());
        odsTurma.SelectParameters.Add("crp_id", crp_id.ToString());
        odsTurma.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsTurma.SelectParameters.Add("tur_tipo", "0");
        odsTurma.SelectParameters.Add("tur_situacao", tur_situacao.ToString());
        odsTurma.SelectParameters.Add("appMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());

        odsTurma.SelectMethod = "GetSelectBy_Escola_Periodo_Situacao";

        _ddlTurma.DataBind();
    }

    public void CarregarPorEscolaCurriculoPeriodoMomentoAno(int esc_id, int uni_id, int cur_id, int crr_id, int crp_id, int ttn_id = -1)
    {
        // Ok
        _ddlTurma.Items.Clear();

        odsTurma.SelectParameters.Clear();
        odsTurma.SelectParameters.Add("esc_id", esc_id.ToString());
        odsTurma.SelectParameters.Add("uni_id", uni_id.ToString());
        odsTurma.SelectParameters.Add("cal_id", "-1");
        odsTurma.SelectParameters.Add("cur_id", cur_id.ToString());
        odsTurma.SelectParameters.Add("crr_id", crr_id.ToString());
        odsTurma.SelectParameters.Add("crp_id", crp_id.ToString());
        odsTurma.SelectParameters.Add("ttn_id", ttn_id.ToString());

        odsTurma.SelectMethod = "GetSelectBy_Escola_Periodo_MomentoAno";

        _ddlTurma.Items.Insert(0, new ListItem("-- Selecione uma turma --", "-1;-1;-1"));
        _ddlTurma.AppendDataBoundItems = true;
        _ddlTurma.DataBind();         
    }

    public void CarregarTurmasCursosEquivalentes(int esc_id, int uni_id, int cur_id, int crr_id, int crp_id)
    {
        // Ok
        _ddlTurma.Items.Clear();

        odsTurma.SelectParameters.Clear();
        odsTurma.SelectParameters.Add("esc_id", esc_id.ToString());
        odsTurma.SelectParameters.Add("uni_id", uni_id.ToString());
        odsTurma.SelectParameters.Add("cur_id", cur_id.ToString());
        odsTurma.SelectParameters.Add("crr_id", crr_id.ToString());
        odsTurma.SelectParameters.Add("crp_id", crp_id.ToString());
        odsTurma.SelectParameters.Add("cal_id", "-1");

        odsTurma.SelectMethod = "SelecionaTurmasCursoEquivalentes";

        _ddlTurma.Items.Insert(0, new ListItem("-- Selecione uma turma --", "-1;-1;-1"));
        _ddlTurma.AppendDataBoundItems = true;
        _ddlTurma.DataBind();
    }

    public void CarregarPorEscolaCurriculoPeriodoMomentoAnoAcertoSituacao(int esc_id, int uni_id, int cur_id, int crr_id, int crp_id)
    {
        _ddlTurma.Items.Clear();

        odsTurma.SelectParameters.Clear();
        odsTurma.SelectParameters.Add("esc_id", esc_id.ToString());
        odsTurma.SelectParameters.Add("uni_id", uni_id.ToString());
        odsTurma.SelectParameters.Add("cal_id", "-1");
        odsTurma.SelectParameters.Add("cur_id", cur_id.ToString());
        odsTurma.SelectParameters.Add("crr_id", crr_id.ToString());
        odsTurma.SelectParameters.Add("crp_id", crp_id.ToString());

        odsTurma.SelectMethod = "SelecionaPorEscolaPeriodoMomentoAnoAcertoSituacao";

        _ddlTurma.Items.Insert(0, new ListItem("-- Selecione uma turma --", "-1;-1;-1"));
        _ddlTurma.AppendDataBoundItems = true;
        _ddlTurma.DataBind();
    }

    public void CarregarPorEscolaCurriculoPeriodoMomentoAnoAvaliacao(int esc_id, int uni_id, int cur_id, int crr_id, int crp_id, int tca_numeroAvaliacao)
    {
        // Ok
        _ddlTurma.Items.Clear();

        odsTurma.SelectParameters.Clear();
        odsTurma.SelectParameters.Add("esc_id", esc_id.ToString());
        odsTurma.SelectParameters.Add("uni_id", uni_id.ToString());
        odsTurma.SelectParameters.Add("cal_id", "-1");
        odsTurma.SelectParameters.Add("cur_id", cur_id.ToString());
        odsTurma.SelectParameters.Add("crr_id", crr_id.ToString());
        odsTurma.SelectParameters.Add("crp_id", crp_id.ToString());
        odsTurma.SelectParameters.Add("tca_numeroAvaliacao", tca_numeroAvaliacao.ToString());

        odsTurma.SelectMethod = "GetSelectBy_Escola_Periodo_MomentoAno_Avaliacao";

        _ddlTurma.Items.Insert(0, new ListItem("-- Selecione uma turma --", "-1;-1;-1"));
        _ddlTurma.AppendDataBoundItems = true;
        _ddlTurma.DataBind();
    }

    public void CarregarTurmasCursosEquivalentesAvaliacao(int esc_id, int uni_id, int cur_id, int crr_id, int crp_id, int tca_numeroAvaliacao)
    {
        // Ok
        _ddlTurma.Items.Clear();

        odsTurma.SelectParameters.Clear();
        odsTurma.SelectParameters.Add("esc_id", esc_id.ToString());
        odsTurma.SelectParameters.Add("uni_id", uni_id.ToString());
        odsTurma.SelectParameters.Add("cur_id", cur_id.ToString());
        odsTurma.SelectParameters.Add("crr_id", crr_id.ToString());
        odsTurma.SelectParameters.Add("crp_id", crp_id.ToString());
        odsTurma.SelectParameters.Add("tca_numeroAvaliacao", tca_numeroAvaliacao.ToString());

        odsTurma.SelectMethod = "SelecionaTurmasCursosEquivalentesAvaliacao";

        _ddlTurma.Items.Insert(0, new ListItem("-- Selecione uma turma --", "-1;-1;-1"));
        _ddlTurma.AppendDataBoundItems = true;
        _ddlTurma.DataBind();
    }

    public void CarregarPorEscolaCurriculoPeriodoMomentoAnoAvaliacaoAcertoSituacao(int esc_id, int uni_id, int cur_id, int crr_id, int crp_id, int tca_numeroAvaliacao)
    {
        _ddlTurma.Items.Clear();

        odsTurma.SelectParameters.Clear();
        odsTurma.SelectParameters.Add("esc_id", esc_id.ToString());
        odsTurma.SelectParameters.Add("uni_id", uni_id.ToString());
        odsTurma.SelectParameters.Add("cal_id", "-1");
        odsTurma.SelectParameters.Add("cur_id", cur_id.ToString());
        odsTurma.SelectParameters.Add("crr_id", crr_id.ToString());
        odsTurma.SelectParameters.Add("crp_id", crp_id.ToString());
        odsTurma.SelectParameters.Add("tca_numeroAvaliacao", tca_numeroAvaliacao.ToString());

        odsTurma.SelectMethod = "SelecionaPorEscolaPeriodoMomentoAnoAvaliacaoAcertoSituacao";

        _ddlTurma.Items.Insert(0, new ListItem("-- Selecione uma turma --", "-1;-1;-1"));
        _ddlTurma.AppendDataBoundItems = true;
        _ddlTurma.DataBind();
    }

    /// <summary>
    /// Carrega o dropdownlist com os dados da turma filtrado por unidade escolar, curriculo do curso e calendário escolar.
    /// </summary>
    /// <param name="esc_id">id da escolar</param>
    /// <param name="uni_id">id da unidade escolar</param>
    /// <param name="cur_id">id do curso</param>
    /// <param name="crr_id">id do curriculo do curso</param>
    /// <param name="crp_id">id do período do curso</param>
    /// <param name="cal_id">id do calendário escolar</param>
    public void CarregarPorEscolaCurriculoCalendario(int esc_id, int uni_id, int cur_id, int crr_id, int crp_id, int cal_id)
    {
        // Ok
        _ddlTurma.Items.Clear();

        odsTurma.SelectParameters.Clear();
        odsTurma.SelectParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
        odsTurma.SelectParameters.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
        odsTurma.SelectParameters.Add("adm", (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao).ToString());
        odsTurma.SelectParameters.Add("esc_id", esc_id.ToString());
        odsTurma.SelectParameters.Add("uni_id", uni_id.ToString());
        odsTurma.SelectParameters.Add("cal_id", cal_id.ToString());
        odsTurma.SelectParameters.Add("cur_id", cur_id.ToString());
        odsTurma.SelectParameters.Add("crr_id", crr_id.ToString());
        odsTurma.SelectParameters.Add("crp_id", crp_id.ToString());
        odsTurma.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsTurma.SelectParameters.Add("tur_tipo", "0");
        odsTurma.SelectParameters.Add("tur_situacao", "0");
        odsTurma.SelectParameters.Add("appMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());

        odsTurma.SelectMethod = "GetSelectBy_Escola_Periodo_Situacao";

        _ddlTurma.Items.Insert(0, new ListItem("-- Selecione uma turma --", "-1;-1;-1"));
        _ddlTurma.DataBind();
        _ddlTurma.SelectedValue = "-1;-1;-1";
    }

    /// <summary>
    /// Carrega o dropdownlist com os dados da turma filtrado por unidade escolar, curriculo e período do curso.
    /// Tendo a situação das turmas = 1 "Ativo"
    /// </summary>
    /// <param name="esc_id">id da escolar</param>
    /// <param name="uni_id">id da unidade escolar</param>
    /// <param name="cur_id">id do curso</param>
    /// <param name="crr_id">id do curriculo do curso</param>
    /// <param name="crp_id">id do período do curso</param>
    public void CarregarPorEscolaCurriculoPeriodo(int esc_id, int uni_id, int cur_id, int crr_id, int crp_id)
    {
        // Ok
        _ddlTurma.Items.Clear();

        odsTurma.SelectParameters.Clear();
        odsTurma.SelectParameters.Add("tur_id", "-1");
        odsTurma.SelectParameters.Add("esc_id", esc_id.ToString());
        odsTurma.SelectParameters.Add("uni_id", uni_id.ToString());
        odsTurma.SelectParameters.Add("cal_id", "-1");
        odsTurma.SelectParameters.Add("cur_id", cur_id.ToString());
        odsTurma.SelectParameters.Add("crr_id", crr_id.ToString());
        odsTurma.SelectParameters.Add("crp_id", crp_id.ToString());
        //Situação das turma = 1 "Ativo"
        odsTurma.SelectParameters.Add("tur_situacao", "1");

        odsTurma.SelectMethod = "RetornaTurmas";

        _ddlTurma.Items.Insert(0, new ListItem("-- Selecione uma turma --", "-1;-1;-1"));
        _ddlTurma.AppendDataBoundItems = true;
        _ddlTurma.DataBind();
        _ddlTurma.SelectedValue = "-1;-1;-1";
    }

    /// <summary>
    /// Carrega o dropdownlist com os dados da turma filtrado por unidade escolar, curriculo, período do curso
    /// e situação da turma.
    /// </summary>
    /// <param name="esc_id">Id da escolar</param>
    /// <param name="uni_id">Id da unidade escolar</param>
    /// <param name="cur_id">Id do curso</param>
    /// <param name="crr_id">Id do curriculo do curso</param>
    /// <param name="crp_id">Id do período do curso</param>
    /// <param name="cal_ano"></param>
    /// <param name="tur_situacao">Situação da turma</param>
    public void CarregarPorEscolaCurriculoCalendario_Situacao(int esc_id, int uni_id, int cur_id, int crr_id, int crp_id, int cal_ano, TUR_TurmaSituacao tur_situacao)
    {
        // Ok
        CancelSelect = false;
        _ddlTurma.Items.Clear();

        odsTurma.SelectParameters.Clear();
        odsTurma.SelectParameters.Add("tur_id", "-1");
        odsTurma.SelectParameters.Add("esc_id", esc_id.ToString());
        odsTurma.SelectParameters.Add("uni_id", uni_id.ToString());
        odsTurma.SelectParameters.Add("cal_id", "-1");
        odsTurma.SelectParameters.Add("cur_id", cur_id.ToString());
        odsTurma.SelectParameters.Add("crr_id", crr_id.ToString());
        odsTurma.SelectParameters.Add("crp_id", crp_id.ToString());
        odsTurma.SelectParameters.Add("cal_ano", cal_ano.ToString());
        odsTurma.SelectParameters.Add("tur_situacao", ((byte)tur_situacao).ToString());

        odsTurma.SelectMethod = "RetornaTurmasCalendario";

        _ddlTurma.Items.Insert(0, new ListItem("-- Selecione uma turma --", "-1;-1;-1"));
        _ddlTurma.AppendDataBoundItems = true;
        _ddlTurma.DataBind();
        _ddlTurma.SelectedValue = "-1;-1;-1";
    }

    /// <summary>
    /// Busca turmas da escola, ano, curso e período informados.
    /// Considera os cursos equivalentes.
    /// Traz somente turmas do tipo 1-Normal.
    /// Somente turmas ativas.
    /// </summary>
    /// <param name="esc_id">Id da escolar</param>
    /// <param name="uni_id">Id da unidade escolar</param>
    /// <param name="cur_id">Id do curso</param>
    /// <param name="crr_id">Id do curriculo do curso</param>
    /// <param name="crp_id">Id do período do curso</param>
    /// <param name="cal_ano"></param>
    public void CarregarPorEscola_Calendario_CursoPeriodo_Equivalentes
        (int esc_id, int uni_id, int cur_id, int crr_id, int crp_id, int cal_ano)
    {
        // Ok
        CancelSelect = false;
        _ddlTurma.Items.Clear();

        odsTurma.SelectParameters.Clear();
        odsTurma.SelectParameters.Add("esc_id", esc_id.ToString());
        odsTurma.SelectParameters.Add("uni_id", uni_id.ToString());
        odsTurma.SelectParameters.Add("cur_id", cur_id.ToString());
        odsTurma.SelectParameters.Add("crr_id", crr_id.ToString());
        odsTurma.SelectParameters.Add("crp_id", crp_id.ToString());
        odsTurma.SelectParameters.Add("cal_ano", cal_ano.ToString());

        odsTurma.SelectMethod = "SelecionaPor_Escola_Calendario_CursoPeriodo_Equivalentes";

        _ddlTurma.Items.Insert(0, new ListItem("-- Selecione uma turma --", "-1;-1;-1"));
        _ddlTurma.AppendDataBoundItems = true;
        _ddlTurma.DataBind();
        _ddlTurma.SelectedValue = "-1;-1;-1";
    }

    /// <summary>
    /// Carrega as turmas eletivas do aluno
    /// </summary>
    /// <param name="esc_id">ID da escola</param>
    /// <param name="uni_id">ID da unidade da escola</param>
    /// <param name="cur_id">ID do curso</param>
    /// <param name="crr_id">ID do currículo do curso</param>
    /// <param name="dis_id">ID da disciplina eletiva do aluno</param>
    /// <param name="cal_id">ID do calendário escolar</param>
    public void CarregarTurmasEletivasAlunosAtivas
    (
        int esc_id
        , int uni_id
        , int cur_id
        , int crr_id
        , int dis_id
        , int cal_id
    )
    {
        odsTurma.SelectMethod = "SelecionaTurmasEletivasAluno";
        odsTurma.SelectParameters.Clear();        

        odsTurma.SelectParameters.Add("esc_id", esc_id.ToString());
        odsTurma.SelectParameters.Add("uni_id", uni_id.ToString());        
        odsTurma.SelectParameters.Add("cur_id", cur_id.ToString());
        odsTurma.SelectParameters.Add("crr_id", crr_id.ToString());
        odsTurma.SelectParameters.Add("dis_id", dis_id.ToString());
        odsTurma.SelectParameters.Add("cal_id", cal_id.ToString());
        odsTurma.SelectParameters.Add("adm", (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao).ToString());
        
        _ddlTurma.DataBind();

        SetaDadosAdicionaisInternos();
    }

    /// <summary>
    /// Retorna as turmas da escola, curso, período do curso e calendário. 
    /// Traz somente turmas do tipo 1-Normal, e com fav_tipoLancamentoFrequencia = 3 ou 4.
    /// </summary>
    /// <param name="esc_id">ID da escola.</param>
    /// <param name="uni_id">ID da unidade.</param>
    /// <param name="cal_id">ID do calendário.</param>
    /// <param name="cur_id">ID do curso.</param>
    /// <param name="crr_id">ID do currículo.</param>
    /// <param name="crp_id">ID do currículo período.</param>
    public void CarregarPorEscolaPeriodoCalendarioComFrequenciaMensal
    (
        int esc_id
        , int uni_id
        , int cal_id
        , int cur_id
        , int crr_id
        , int crp_id
    )
    {
        odsTurma.SelectMethod = "SelecionaPorEscolaPeriodoCalendarioComFrequenciaMensal";
        odsTurma.SelectParameters.Clear();

        odsTurma.SelectParameters.Add("esc_id", esc_id.ToString());
        odsTurma.SelectParameters.Add("uni_id", uni_id.ToString());
        odsTurma.SelectParameters.Add("cal_id", cal_id.ToString());
        odsTurma.SelectParameters.Add("cur_id", cur_id.ToString());
        odsTurma.SelectParameters.Add("crr_id", crr_id.ToString());
        odsTurma.SelectParameters.Add("crp_id", crp_id.ToString());

        _ddlTurma.DataBind();
    }

    /// <summary>
    /// Seleciona turmas por escola, curso, período e calendário mínimo.
    /// </summary>
    /// <param name="esc_id">ID da escola.</param>
    /// <param name="uni_id">ID da unidade de escola.</param>
    /// <param name="cur_id">ID do curso.</param>
    /// <param name="crr_id">ID do currículo do curso.</param>
    /// <param name="crp_id">ID do período do currículo.</param>
    /// <param name="cal_ano">Ano limite mínimo.</param>
    public void CarregarPorEscolaCursoPeriodoCalendarioMinimo
    (
        int esc_id,
        int uni_id,
        int cur_id,
        int crr_id,
        int crp_id,
        int cal_ano
    )
    {
        _ddlTurma.Items.Clear();
        _ddlTurma.DataSourceID = "";

        _ddlTurma.DataSource = TUR_TurmaBO.SelecionaPorEscolaCursoPeriodoCalendarioMinimo(esc_id, uni_id, cur_id, crr_id, crp_id, cal_ano, ApplicationWEB.AppMinutosCacheLongo);

        _ddlTurma.Items.Insert(0, new ListItem("-- Selecione uma turma --", "-1;-1;-1"));
        _ddlTurma.AppendDataBoundItems = true;
        _ddlTurma.DataBind();
        _ddlTurma.SelectedValue = "-1;-1;-1";
    }

    #endregion Métodos

    #region Eventos

    protected void Page_PreRender(object sender, EventArgs e)
    {
        // Se for pra mostrar dados adicionais dá postback também.
        _ddlTurma.AutoPostBack = (_SelecionaTurma != null);

        if (_ddlTurma.SelectedValue == "-1;-1;-1")
        {
            lblCapacidade.Text = "";
            lblMatriculados.Text = "";
        }

        if (!_ddlTurma.AutoPostBack && MostraDadosAdicionais)
        {
            // Seta o evento de change por javascript para mostrar a capacidade e matriculados da turma.
            string script =
                @"$('#" + _ddlTurma.ClientID + @"').bind('change', function () {
                var valor = $(this).find('option:selected').attr('value');
                if ($('#" + hdnQuantidadesDoCombo.ClientID + @"').val() != '') {
                    var ar = $('#" + hdnQuantidadesDoCombo.ClientID + @"').val().split('|');
                    $('#" + lblCapacidade.ClientID + @"').html('');
                    $('#" + lblMatriculados.ClientID + @"').html('');
                    for (var i = 0; i < ar.length; i++) {
                        var id = ar[i].split(',')[0];
                        if (id != undefined && id == valor) {
                            var Capacidade = ar[i].split(',')[1];
                            if (Capacidade != undefined && Capacidade != null) {
                                $('#" +
                                    lblCapacidade.ClientID +
                                    @"').html('<b>Capacidade: ' + Capacidade + '</b><br />');
                            }
                            else {
                                $('#" +
                                    lblCapacidade.ClientID +
                                    @"').html('');
                            }
                            var qtmatriculasativas = ar[i].split(',')[2];
                            if (qtmatriculasativas != undefined && qtmatriculasativas != null) {
                                $('#" +
                                    lblMatriculados.ClientID +
                                    @"').html('<b>Matriculados: ' + qtmatriculasativas + '</b>');
                            }
                            else {
                                $('#" +
                                    lblMatriculados.ClientID + @"').html('');
                            }
                        }
                    }
                }
            });";

            if (Valor[0] > 0)
            {
                // Se já tem algo selecionado no combo, dispara o evento para mostrar o valor.
                script += "$('#" + _ddlTurma.ClientID + "').trigger('change');";
            }

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "selecionaTurma", script, true);
        }
    }

    protected void _ddlTurma_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_SelecionaTurma != null)
        {
            _SelecionaTurma();
            SetaDadosAdicionais();
        }
    }

    protected void odsTurma_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.Cancel = cancelSelect;
        if (e.Cancel)
        {
            if ((_ddlTurma.Items.FindByValue("-1;-1;-1") == null))
            _ddlTurma.Items.Insert(0, new ListItem("-- Selecione uma turma --", "-1;-1;-1"));
        }
        else
        {
            e.InputParameters["ent_id"] = __SessionWEB.__UsuarioWEB.Usuario.ent_id;
            e.InputParameters["gru_id"] = __SessionWEB.__UsuarioWEB.Grupo.gru_id;
            e.InputParameters["usu_id"] = __SessionWEB.__UsuarioWEB.Usuario.usu_id;
        }
    }

    protected void odsTurma_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (e.Exception != null)
        {
            // Grava o erro e mostra pro usuário.
            ApplicationWEB._GravaErro(e.Exception.InnerException);

            //lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);

            e.ExceptionHandled = true;

            lblMessage.Text = "Erro ao tentar carregar " + _Label.Text.Replace('*', ' ').ToLower() + ".";
            lblMessage.Visible = true;
        }
    }

    protected void _ddlTurma_DataBound(object sender, EventArgs e)
    {
        if (MostraDadosAdicionais && _SelecionaTurma == null)
        {
            divDadosTurma.Visible = true;

            try
            {
                string tur_ids = string.Join(",",
                    // Busca os tur_id dos itens do combo (value dos itens).
                                             (from ListItem item in _ddlTurma.Items
                                              where Convert.ToInt64(item.Value.Split(';')[0]) > 0
                                              select item.Value.Split(';')[0]
                                             ).ToArray());

                // Buscar os dados adicionais pras turmas e jogar no combo, caso ele não tenha
                // evento de postback setado.
                DataTable dt = TUR_TurmaBO.RetornaVagasMatriculadosPor_Turma(tur_ids);
                List<string> ids = new List<string>();
                foreach (ListItem item in _ddlTurma.Items)
                {
                    string tur_id = item.Value.Split(';')[0];

                    var x = (from DataRow dr in dt.Rows
                             where dr["tur_id"].ToString() == tur_id
                             select new
                             {
                                 Capacidade = dr["Capacidade"].ToString()
                                 ,
                                 QtMatriculasAtivas = dr["QtMatriculasAtivas"].ToString()
                             }
                            );

                    if (x.Count() > 0)
                    {
                        ids.Add(item.Value + ","
                                + x.First().Capacidade + ","
                                + x.First().QtMatriculasAtivas);
                    }
                }

                hdnQuantidadesDoCombo.Value = string.Join("|", ids.ToArray());
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblCapacidade.Text = "";
                lblMatriculados.Text = "";
            }
        }
    }

    #endregion Eventos
}
