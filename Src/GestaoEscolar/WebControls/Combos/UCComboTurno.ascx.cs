using System;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.Entities;

public partial class WebControls_Combos_UCComboTurno : MotherUserControl
{
    #region DELEGATES

    public delegate void SelectedIndexChanged();
    public event SelectedIndexChanged IndexChanged;

    #endregion

    #region Propriedades

    /// <summary>
    /// Retorna e seta o valor selecionado no combo
    /// </summary>
    public int Valor
    {
        get 
        { 
            return string.IsNullOrEmpty(ddlCombo.SelectedValue) ? -1 : Convert.ToInt32(ddlCombo.SelectedValue); 
        }
        set
        {
            ddlCombo.SelectedValue = value.ToString();
        }
    }

    /// <summary>
    /// Retorna o texto do campo selecionado
    /// </summary>
    public string Texto
    {
        get
        {
            return ddlCombo.SelectedItem.ToString();
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
            return ddlCombo.ClientID;
        }
    }

    /// <summary>
    /// Deixa o combo habilitado de acordo com o valor passado
    /// </summary>
    public bool PermiteEditar
    {
        set
        {
            ddlCombo.Enabled = value;
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
    }

    /// <summary>
    /// Mostra o tipo do turno no lugar da descrição
    /// </summary>
    public bool MostrarPorTipoTurno
    {
        get { return MostraPorTipoTurno; }
        set { MostraPorTipoTurno = value; }
    }

    private bool MostraPorTipoTurno;

    /// <summary>
    /// Cancela o consulta do ObjectDataSource ao carregar a página pela primeira vez.
    /// </summary>
    public bool CancelSelect
    {
        get { return CancelaSelect; }
        set { CancelaSelect = value; }
    }

    private bool CancelaSelect;

    #endregion

    #region Métodos

    /// <summary>
    /// Seta o foco no combo    
    /// </summary>
    public void SetarFoco()
    {
        ddlCombo.Focus();
    }

    /// <summary>
    /// Retorna uma flag informando se o valor existe no combo.
    /// </summary>
    /// <param name="valor">Valor a ser pesquisado</param>
    /// <returns>Flag informando se o valor existe no combo</returns>
    public bool ExisteValor(Int32 valor)
    {
        return (ddlCombo.Items.FindByValue(valor.ToString()) != null);
    }

    /// <summary>
    /// Adiciona um item vazio no combo, com valor "Selecione um item".
    /// </summary>
    public void AdicionaItemVazio()
    {
        CancelaSelect = true;

        ddlCombo.Items.Clear();
        ddlCombo.Items.Add(new ListItem("-- Selecione um turno --", "-1", true));
    }

    /// <summary>
    /// Carrega os tipos de turno associados aos turnos cadastrados no ent_id selecionado
    /// </summary>
    public void CarregarTurnoPorTipoTurno()
    {
        ddlCombo.Items.Clear();
        odsDados.SelectParameters.Clear();

        odsDados.DataObjectTypeName = "MSTech.GestaoEscolar.Entities.ACA_TipoTurno";
        odsDados.TypeName = "MSTech.GestaoEscolar.BLL.ACA_TipoTurnoBO";
        odsDados.SelectMethod = "SelecionaPorEntidadeTurno";

        odsDados.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());

        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um turno --", "-1", true));
        ddlCombo.DataBind();
    }

    /// <summary>
    /// Mostra os dados não excluídos logicamente no dropdownlist    
    /// </summary>
    public void CarregarTurno()
    {
        ddlCombo.Items.Clear();
        odsDados.SelectParameters.Clear();

        odsDados.DataObjectTypeName = "MSTech.GestaoEscolar.Entities.ACA_Turno";
        odsDados.TypeName = "MSTech.GestaoEscolar.BLL.ACA_TurnoBO";
        odsDados.SelectMethod = "SelecionaTurno";

        odsDados.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());

        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um turno --", "-1", true));
        ddlCombo.DataBind();
    }

    /// <summary>
    /// Mostra os turnos ativos 
    /// </summary>
    /// <param name="trn_id">Id do turno</param>
    public void CarregarTurnoPorTurnoAtivo(int trn_id)
    {
        ddlCombo.Items.Clear();
        odsDados.SelectParameters.Clear();

        odsDados.DataObjectTypeName = "MSTech.GestaoEscolar.Entities.ACA_Turno";
        odsDados.TypeName = "MSTech.GestaoEscolar.BLL.ACA_TurnoBO";
        odsDados.SelectMethod = "SelecionaTurnoPorTurnoAtivo";

        odsDados.SelectParameters.Add("trn_id", trn_id.ToString());
        odsDados.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsDados.SelectParameters.Add("mostrarportipoturno", MostraPorTipoTurno.ToString());

        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um turno --", "-1", true));
        ddlCombo.DataBind();

        if (ddlCombo.Items.Count == 1)
            ddlCombo.SelectedIndex = 1;
    }

    /// <summary>
    /// Mostra os turnos ativos 
    /// E que possuem horários cadastrados para o turno
    /// </summary>
    /// <param name="trn_id">Id do turno</param>
    public void SelecionaTurnoPorTurnoAtivoHorarioCadastrado(int trn_id)
    {
        ddlCombo.Items.Clear();
        odsDados.SelectParameters.Clear();

        odsDados.DataObjectTypeName = "MSTech.GestaoEscolar.Entities.ACA_Turno";
        odsDados.TypeName = "MSTech.GestaoEscolar.BLL.ACA_TurnoBO";
        odsDados.SelectMethod = "SelecionaTurnoPorTurnoAtivoHorarioCadastrado";

        odsDados.SelectParameters.Add("trn_id", trn_id.ToString());
        odsDados.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsDados.SelectParameters.Add("mostrarportipoturno", MostraPorTipoTurno.ToString());

        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um turno --", "-1", true));
        ddlCombo.DataBind();

        if (ddlCombo.Items.Count == 1)
            ddlCombo.SelectedIndex = 1;
    }

    /// <summary>
    /// Mostra os dados não excluídos logicamente no dropdownlist
    /// de acordo com o controle de tempo do período do curso
    /// que estejam ativos
    /// </summary>        
    /// <param name="trn_id">Id do turno</param>
    /// <param name="crp_controleTempo">Tipo de controle de tempo do período do curso</param>
    /// <param name="crp_qtdeDiasSemana">Quantidade de dias da semana que tem aula</param>
    /// <param name="crp_qtdeTempoSemana">Quantidade de tempos de aula por semana</param>        
    /// <param name="crp_qtdeHorasDia">Quantidade de horas por dia</param>        
    /// <param name="crp_qtdeMinutosDia">Quantidade de minutos por dia</param>             
    public void CarregarTurnoPorTurnoPeriodoControleTempoAtivo
    (
        int trn_id
        , byte crp_controleTempo
        , byte crp_qtdeDiasSemana
        , byte crp_qtdeTempoSemana
        , byte crp_qtdeHorasDia
        , byte crp_qtdeMinutosDia
    )
    {
        CancelaSelect = false;
        ddlCombo.Items.Clear();
        odsDados.SelectParameters.Clear();

        odsDados.DataObjectTypeName = "MSTech.GestaoEscolar.Entities.ACA_Turno";
        odsDados.TypeName = "MSTech.GestaoEscolar.BLL.ACA_TurnoBO";
        odsDados.SelectMethod = "SelecionaTurnoPorTurnoPeriodoControleTempoAtivo";

        odsDados.SelectParameters.Add("trn_id", trn_id.ToString());
        odsDados.SelectParameters.Add("crp_controleTempo", crp_controleTempo.ToString());
        odsDados.SelectParameters.Add("crp_qtdeDiasSemana", crp_qtdeDiasSemana.ToString());
        odsDados.SelectParameters.Add("crp_qtdeTempoSemana", crp_qtdeTempoSemana.ToString());
        odsDados.SelectParameters.Add("crp_qtdeHorasDia", crp_qtdeHorasDia.ToString());
        odsDados.SelectParameters.Add("crp_qtdeMinutosDia", crp_qtdeMinutosDia.ToString());
        odsDados.SelectParameters.Add("ent_id", __SessionWEB.__UsuarioWEB.Usuario.ent_id.ToString());
        odsDados.SelectParameters.Add("mostrarportipoturno", MostraPorTipoTurno.ToString());

        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um turno --", "-1", true));
        ddlCombo.DataBind();
    }

    public void CarregarTurnoPorParametroPeriodo(MTR_ParametroFormacaoTurma entity)
    {
        CancelaSelect = false;
        ddlCombo.Items.Clear();
        odsDados.SelectParameters.Clear();

        odsDados.DataObjectTypeName = "MSTech.GestaoEscolar.Entities.MTR_ParametroFormacaoTurmaTurno";
        odsDados.TypeName = "MSTech.GestaoEscolar.BLL.MTR_ParametroFormacaoTurmaTurnoBO";
        odsDados.SelectMethod = "SelecionaPorProcessoParametroTable";

        odsDados.SelectParameters.Add("pfi_id", entity.pfi_id.ToString());
        odsDados.SelectParameters.Add("pft_id", entity.pft_id.ToString());
        odsDados.SelectParameters.Add("mostrarportipoturno", MostraPorTipoTurno.ToString());

        ddlCombo.Items.Insert(0, new ListItem("-- Selecione um turno --", "-1", true));
        ddlCombo.DataBind();

        if (ddlCombo.Items.Count == 2)
            ddlCombo.SelectedIndex = 1;

    }

    #endregion

    #region Eventos

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
        e.Cancel = CancelaSelect;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        ddlCombo.AutoPostBack = (IndexChanged != null);
    }

    protected void ddlCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (IndexChanged != null)
            IndexChanged();
    }

    #endregion
}
