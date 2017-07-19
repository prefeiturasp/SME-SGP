using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.BLL;

public partial class WebControls_Combos_UCComboCalendario : MotherUserControl
{
    #region Delegates

    public delegate void SelectedIndexChanged();

    public event SelectedIndexChanged IndexChanged;

    #endregion Delegates

    #region Proriedades

    private bool cancelSelect = false;

    /// <summary>
    /// Cancela o consulta do ObjectDataSource ao carregar a página pela primeira vez.
    /// </summary>
    public bool CancelSelect
    {
        get { return cancelSelect; }
        set { cancelSelect = value; }
    }

    /// <summary>
    /// Retorna e seta o valor selecionado no combo
    /// </summary>
    public int Valor
    {
        get
        {
            if (string.IsNullOrEmpty(ddlComboCalendario.SelectedValue))
                return -1;

            return Convert.ToInt32(ddlComboCalendario.SelectedValue);
        }
        set
        {
            ddlComboCalendario.SelectedValue = value.ToString();
        }
    }

    /// <summary>
    /// Retorna o texto selecionado no combo
    /// </summary>
    public string Texto
    {
        get
        {
            return ddlComboCalendario.SelectedItem.ToString();
        }
    }

    /// <summary>
    /// Coloca na primeira linha a mensagem de selecione um item.
    /// </summary>
    public bool MostrarMensagemSelecione
    {
        set
        {
            if (value)
                ddlComboCalendario.Items.Insert(0, new ListItem("-- Selecione um calendário escolar --", "-1", true));
        }
    }

    /// <summary>
    /// retorna o ano selecionado no combo
    /// </summary>
    public int _cal_ano
    {
        get
        {
            int ano = 0;
            if (ddlComboCalendario.SelectedIndex > 0)
            {
                Int32.TryParse(Texto.Substring(0, 4), out ano);
                return ano;
            }
            return ano;
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
                AdicionaAsteriscoObrigatorio(lblTitulo);
            }
            else
            {
                RemoveAsteriscoObrigatorio(lblTitulo);
            }

            cpvCombo.Visible = value;
        }
    }

    /// <summary>
    /// Seta o validationGroup do combo.
    /// </summary>
    public string ValidationGroup
    {
        set
        {
            cpvCombo.ValidationGroup = value;
        }
    }

    /// <summary>
    /// ClientID do combo
    /// </summary>
    public string Combo_ClientID
    {
        get
        {
            return ddlComboCalendario.ClientID;
        }
    }

    /// <summary>
    /// Deixa o combo habilitado de acordo com o valor passado
    /// </summary>
    public bool PermiteEditar
    {
        get
        {
            return ddlComboCalendario.Enabled;
        }
        set
        {
            ddlComboCalendario.Enabled = value;
        }
    }

    /// <summary>
    /// Seta um titulo diferente do padrão para o combo
    /// </summary>
    public string Titulo
    {
        set
        {
            lblTitulo.Text = value;
            cpvCombo.ErrorMessage = value.Replace('*', ' ') + " é obrigatório.";
        }
        get
        {
            return lblTitulo.Text.Replace(ApplicationWEB.TextoAsteriscoObrigatorio, string.Empty);
        }
    }

    /// <summary>
    /// Propriedade que verifica quantos items existem no combo
    /// </summary>
    public int QuantidadeItensCombo
    {
        get
        {
            return ddlComboCalendario.Items.Count;
        }
    }

    /// <summary>
    /// Propriedade que seta o SelectedIndex do Combo.
    /// </summary>
    public int SelectedIndex
    {
        set
        {
            ddlComboCalendario.SelectedValue = ddlComboCalendario.Items[value].Value;
        }
        get
        {
            return ddlComboCalendario.SelectedIndex;
        }
    }

    /// <summary>
    /// Propriedade para controlar o viewstate dos controles:
    /// odsDados
    /// </summary>
    public bool GuardaViewState
    {
        get
        {
            return odsDados.EnableViewState;
        }
        set
        {
            odsDados.EnableViewState = value;
        }
    }

    #endregion Proriedades

    #region Métodos

    /// <summary>
    /// Seta o foco no combo
    /// </summary>
    public void SetarFoco()
    {
        ddlComboCalendario.Focus();
    }

    /// <summary>
    /// Seleciona uma opção com o ano corrente no combo
    /// </summary>
    private void SelecionaOpcaoAnoCorrente()
    {
        var x = from ListItem item in ddlComboCalendario.Items
                where
                    item.Value != "-1" &&
                    Convert.ToInt32(item.Text.Substring(0, 4)) == DateTime.Now.Year
                select item.Value;

        if (x.Count() == 1)
        {
            ddlComboCalendario.SelectedValue = x.First();

            if (IndexChanged != null)
                IndexChanged();
        }
        else if (ddlComboCalendario.Items.Count == 2)
        {
            ddlComboCalendario.SelectedValue = ddlComboCalendario.Items[1].Value;

            if (IndexChanged != null)
                IndexChanged();
        }
    }
    
    /// <summary>
    /// Retorna todos os calendários não excluídos logicamente que possuem vinculo com curso filtrando pelo id da escola
    /// </summary>
    /// <param name="esc_id">id da escola</param>
    public void CarregarCalendarioAnualRelCurso_EscId(int esc_id)
    {
        ddlComboCalendario.DataSourceID = odsDados.ID;
        ddlComboCalendario.DataSource = null;

        ddlComboCalendario.Items.Clear();
        odsDados.SelectParameters.Clear();

        //Se for visão de docente carrega apenas os calendários que o docente tem acesso
        if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual && __SessionWEB.__UsuarioWEB.Docente != null && __SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
        {
            odsDados.SelectMethod = "SelecionarCalendarioAnualRelCurso_EscId";
            odsDados.SelectParameters.Add("doc_id", __SessionWEB.__UsuarioWEB.Docente.doc_id.ToString());
        }
        //Se for visão de unidade escolar carrega apenas os calendários ligados aos cursos da escola
        else if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa)
        {
            odsDados.SelectMethod = "SelecionarCalendarioAnualRelCurso_EscId";
            odsDados.SelectParameters.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
            odsDados.SelectParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
        }
        //Carrega todos os calendários
        else
            odsDados.SelectMethod = "SelecionarCalendarioAnualRelCurso_EscId";
        odsDados.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsDados.SelectParameters.Add("esc_id", esc_id.ToString());
        odsDados.SelectParameters.Add("appMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());
        ddlComboCalendario.Items.Insert(0, new ListItem("-- Selecione um calendário escolar --", "-1", true));
        ddlComboCalendario.DataBind();
        SelecionaOpcaoAnoCorrente();
    }
    
    /// <summary>
    /// Mostra os dados não excluídos logicamente no dropdownlist
    /// </summary>
    public void CarregarCalendarioAnual()
    {
        ddlComboCalendario.DataSourceID = odsDados.ID;
        ddlComboCalendario.DataSource = null;

        ddlComboCalendario.Items.Clear();
        odsDados.SelectParameters.Clear();

        //Se for visão de docente carrega apenas os calendários que o docente tem acesso
        if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual && __SessionWEB.__UsuarioWEB.Docente != null && __SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
        {
            odsDados.SelectMethod = "SelecionaCalendarioAnual";
            odsDados.SelectParameters.Add("doc_id", __SessionWEB.__UsuarioWEB.Docente.doc_id.ToString());
        }
        //Se for visão de unidade escolar carrega apenas os calendários ligados aos cursos da escola
        else if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa)
        {
            odsDados.SelectMethod = "SelecionaCalendarioAnual";
            odsDados.SelectParameters.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
            odsDados.SelectParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
        }
        //Carrega todos os calendários
        else
            odsDados.SelectMethod = "SelecionaCalendarioAnual";
        odsDados.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsDados.SelectParameters.Add("appMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());

        ddlComboCalendario.Items.Insert(0, new ListItem("-- Selecione um calendário escolar --", "-1", true));
        ddlComboCalendario.DataBind();
        SelecionaOpcaoAnoCorrente();
    }

    /// <summary>
    /// Mostra os dados não excluídos logicamente no dropdownlist por curso
    /// </summary>
    public void CarregarCalendarioAnualPorCurso(int cur_id)
    {
        ddlComboCalendario.DataSourceID = odsDados.ID;
        ddlComboCalendario.DataSource = null;

        ddlComboCalendario.Items.Clear();
        odsDados.SelectParameters.Clear();

        //Se for visão de docente carrega apenas os calendários que o docente tem acesso
        if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual && __SessionWEB.__UsuarioWEB.Docente != null && __SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
        {
            odsDados.SelectMethod = "SelecionaCalendarioAnualPorCurso";
            odsDados.SelectParameters.Add("doc_id", __SessionWEB.__UsuarioWEB.Docente.doc_id.ToString());
        }
        //Se for visão de unidade escolar carrega apenas os calendários ligados aos cursos da escola
        else if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa)
        {
            odsDados.SelectMethod = "SelecionaCalendarioAnualPorCurso";
            odsDados.SelectParameters.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
            odsDados.SelectParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
        }
        //Carrega todos os calendários
        else
            odsDados.SelectMethod = "SelecionaCalendarioAnualPorCurso";
        odsDados.SelectParameters.Add("cur_id", cur_id.ToString());
        odsDados.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsDados.SelectParameters.Add("appMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());

        ddlComboCalendario.Items.Insert(0, new ListItem("-- Selecione um calendário escolar --", "-1", true));
        ddlComboCalendario.DataBind();

        SelecionaOpcaoAnoCorrente();

        if (IndexChanged != null)
            IndexChanged();
    }

    /// <summary>
    /// Mostra os dados não excluídos logicamente no dropdownlist por curso e ano de início do processo.
    /// </summary>
    public void CarregarCalendarioAnualPorCursoAnoInicio(int cur_id, int pfi_id)
    {
        ddlComboCalendario.DataSourceID = odsDados.ID;
        ddlComboCalendario.DataSource = null;

        ddlComboCalendario.Items.Clear();
        odsDados.SelectParameters.Clear();

        //Se for visão de docente carrega apenas os calendários que o docente tem acesso
        if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual && __SessionWEB.__UsuarioWEB.Docente != null && __SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
        {
            odsDados.SelectMethod = "SelecionaCalendarioAnualPorCursoAnoInicio";
            odsDados.SelectParameters.Add("doc_id", __SessionWEB.__UsuarioWEB.Docente.doc_id.ToString());
        }
        //Se for visão de unidade escolar carrega apenas os calendários ligados aos cursos da escola
        else if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa)
        {
            odsDados.SelectMethod = "SelecionaCalendarioAnualPorCursoAnoInicio";
            odsDados.SelectParameters.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
            odsDados.SelectParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
        }
        //Carrega todos os calendários
        else
            odsDados.SelectMethod = "SelecionaCalendarioAnualPorCursoAnoInicio";
        odsDados.SelectParameters.Add("cur_id", cur_id.ToString());
        odsDados.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsDados.SelectParameters.Add("pfi_id", pfi_id.ToString());
        odsDados.SelectParameters.Add("appMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());

        ddlComboCalendario.Items.Insert(0, new ListItem("-- Selecione um calendário escolar --", "-1", true));
        ddlComboCalendario.DataBind();
        SelecionaOpcaoAnoCorrente();
    }

    /// <summary>
    /// Mostra os dados de cursos que possuem disciplina eletiva
    /// </summary>
    public void CarregarPorCursoComDisciplinaEletiva(int cur_id, int esc_id, int uni_id)
    {
        ddlComboCalendario.DataSourceID = odsDados.ID;
        ddlComboCalendario.DataSource = null;

        ddlComboCalendario.Items.Clear();
        odsDados.SelectParameters.Clear();

        //Se for visão de docente carrega apenas os calendários que o docente tem acesso
        if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual && __SessionWEB.__UsuarioWEB.Docente != null && __SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
        {
            odsDados.SelectMethod = "SelecionaPorCursoComDisciplinaEletiva";
            odsDados.SelectParameters.Add("doc_id", __SessionWEB.__UsuarioWEB.Docente.doc_id.ToString());
        }
        //Se for visão de unidade escolar carrega apenas os calendários ligados aos cursos da escola
        else if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa)
        {
            odsDados.SelectMethod = "SelecionaPorCursoComDisciplinaEletiva";
            odsDados.SelectParameters.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
            odsDados.SelectParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
        }
        //Carrega todos os calendários
        else
            odsDados.SelectMethod = "SelecionaPorCursoComDisciplinaEletiva";
        odsDados.SelectParameters.Add("cur_id", cur_id.ToString());
        odsDados.SelectParameters.Add("esc_id", esc_id.ToString());
        odsDados.SelectParameters.Add("uni_id", uni_id.ToString());
        odsDados.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsDados.SelectParameters.Add("appMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());

        ddlComboCalendario.Items.Insert(0, new ListItem("-- Selecione um calendário escolar --", "-1", true));
        ddlComboCalendario.DataBind();
        SelecionaOpcaoAnoCorrente();
    }

    /// <summary>
    /// Mostra os dados não excluídos logicamente no dropdownlist, por curso e quantidade de períodos do calendário
    /// </summary>
    public void CarregarCalendarioAnualPorCursoQtdePeriodos(int cur_id, int qtdePeriodos)
    {
        ddlComboCalendario.DataSourceID = odsDados.ID;
        ddlComboCalendario.DataSource = null;

        ddlComboCalendario.Items.Clear();
        odsDados.SelectParameters.Clear();

        //Se for visão de docente carrega apenas os calendários que o docente tem acesso
        if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Individual && __SessionWEB.__UsuarioWEB.Docente != null && __SessionWEB.__UsuarioWEB.Docente.doc_id > 0)
        {
            odsDados.SelectMethod = "SelecionaCalendarioAnualPorCursoQtdePeriodos";
            odsDados.SelectParameters.Add("doc_id", __SessionWEB.__UsuarioWEB.Docente.doc_id.ToString());
        }
        //Se for visão de unidade escolar carrega apenas os calendários ligados aos cursos da escola
        else if (__SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.UnidadeAdministrativa)
        {
            odsDados.SelectMethod = "SelecionaCalendarioAnualPorCursoQtdePeriodos";
            odsDados.SelectParameters.Add("gru_id", __SessionWEB.__UsuarioWEB.Grupo.gru_id.ToString());
            odsDados.SelectParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
        }
        //Carrega todos os calendários
        else
            odsDados.SelectMethod = "SelecionaCalendarioAnualPorCursoQtdePeriodos";
        odsDados.SelectParameters.Add("cur_id", cur_id.ToString());
        odsDados.SelectParameters.Add("qtdePeriodos", qtdePeriodos.ToString());
        odsDados.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsDados.SelectParameters.Add("appMinutosCacheLongo", ApplicationWEB.AppMinutosCacheLongo.ToString());

        ddlComboCalendario.Items.Insert(0, new ListItem("-- Selecione um calendário escolar --", "-1", true));
        ddlComboCalendario.DataBind();
        SelecionaOpcaoAnoCorrente();
    }
    
    #endregion Métodos

    #region Eventos

    protected void Page_PreRender(object sender, EventArgs e)
    {
        ddlComboCalendario.AutoPostBack = IndexChanged != null;
    }

    protected void ddlCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (IndexChanged != null)
            IndexChanged();
    }

    protected void odsDados_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (e.Exception != null)
        {
            // Grava o erro e mostra pro usuário.
            ApplicationWEB._GravaErro(e.Exception.InnerException);

            e.ExceptionHandled = true;

            lblMessage.Text = "Erro ao tentar carregar " + lblTitulo.Text.Replace('*', ' ').ToLower() + ".";
            lblMessage.Visible = true;
        }
    }

    protected void odsDados_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.Cancel = cancelSelect;
        if (e.Cancel)
        {
            ddlComboCalendario.Items.Clear();
            ddlComboCalendario.Items.Insert(0, new ListItem("-- Selecione um calendário escolar --", "-1"));
        }
    }

    #endregion Eventos
}