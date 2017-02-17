using System;
using System.Linq;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using System.Data;
using System.Collections.Generic;

public partial class WebControls_Combos_UCComboPeriodoCalendario : MotherUserControl
{
    #region Delegates

    public delegate void SelectedIndexChanged();
    public event SelectedIndexChanged IndexChanged;

    #endregion

    #region Propriedades

    /// <summary>
    /// Retorna e seta o valor selecionado no combo
    /// Valor[0] = tpc_id
    /// Valor[1] = cap_id
    /// </summary>
    public int[] Valor
    {
        get
        {
            if (string.IsNullOrEmpty(ddlComboPeriodoCalendario.SelectedValue))
            {
                return new[] { -1, -1 };
            }

            int[] ids =
                {
                    Convert.ToInt32(ddlComboPeriodoCalendario.SelectedValue.Split(';')[0])
                    ,
                    Convert.ToInt32(ddlComboPeriodoCalendario.SelectedValue.Split(';')[1])
                };

            return ids;
        }
        set
        {
            string valor = value[0] + ";" + value[1];
            if (ddlComboPeriodoCalendario.Items.FindByValue(valor) != null)
                ddlComboPeriodoCalendario.SelectedValue = valor;
        }
    }

    /// <summary>
    /// Retorna e seta o valor do Tpc_ID selecionado no combo
    /// </summary>
    public int Tpc_ID
    {
        get
        {
            return Convert.ToInt32(ddlComboPeriodoCalendario.SelectedValue.Split(';')[0]);
        }
        set
        {
            var x = from ListItem list in ddlComboPeriodoCalendario.Items
                    where (Convert.ToInt32(list.Value.Split(';')[0]) == value)
                    select Convert.ToInt32(list.Value.Split(';')[1]);

            if (x.Count() > 0 && ddlComboPeriodoCalendario.Items.FindByValue(x.ToList()[0].ToString()) != new ListItem())
                Valor = new[] { value, x.ToList()[0] };
        }
    }

    /// <summary>
    /// Retorna e seta o valor do Cap_ID selecionado no combo
    /// </summary>
    public int Cap_ID
    {
        get
        {
            string[] s = ddlComboPeriodoCalendario.SelectedValue.Split(';');
            if (s.Length == 2)
                return Convert.ToInt32(ddlComboPeriodoCalendario.SelectedValue.Split(';')[1]);

            return -1;
        }
        set
        {
            var x = from ListItem list in ddlComboPeriodoCalendario.Items
                    where (Convert.ToInt32(list.Value.Split(';')[1]) == value)
                    select Convert.ToInt32(list.Value.Split(';')[0]);

            if (x.Count() > 0 && ddlComboPeriodoCalendario.Items.FindByValue(x.ToList()[0].ToString()) != new ListItem())
                Valor = new[] { x.ToList()[0], value };
        }
    }

    /// <summary>
    /// Verifica se o combo possui somente um item e seleciona o primeiro.
    /// </summary>
    public bool SelecionaPrimeiroItem()
    {
        if (ExisteAlgumItem())
        {
            // Seleciona o primeiro item.
            ddlComboPeriodoCalendario.SelectedValue = ddlComboPeriodoCalendario.Items[1].Value;

            if (IndexChanged != null)
                IndexChanged();

            return true;
        }

        return false;
    }

    /// <summary>
    /// Retorna o texto selecionado no combo
    /// </summary>
    public string Texto
    {
        get
        {
            return ddlComboPeriodoCalendario.SelectedItem.ToString();
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
            return ddlComboPeriodoCalendario.ClientID;
        }
    }

    /// <summary>
    /// Seta o SkinID do combo.
    /// </summary>
    public string SkinID_Combo
    {
        set
        {
            cpvCombo.SkinID = value;
        }
    }

    /// <summary>
    /// Deixa o combo habilitado de acordo com o valor passado
    /// </summary>
    public bool PermiteEditar
    {
        set
        {
            ddlComboPeriodoCalendario.Enabled = value;
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
            return lblTitulo.Text;
        }
    }

    /// <summary>
    /// Retorna instância do combo.
    /// </summary>
    public DropDownList _Combo
    {
        get
        {
            return ddlComboPeriodoCalendario;
        }
    }

    /// <summary>
    /// Guarda o título do combo.
    /// </summary>
    public string VS_TituloPadrao
    {
        get
        {
            if (ViewState["VS_TituloPadrao"] != null)
            {
                return ViewState["VS_TituloPadrao"].ToString();
            }

            return "um " + GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
        }
        set
        {
            ViewState["VS_TituloPadrao"] = value;
        }
    }

    /// <summary>
    /// Mostra primeira linha do combo com mensagem de selecione.
    /// </summary>
    public bool _MostrarMessageSelecione
    {
        get
        {
            if (ViewState["_MostrarMessageSelecione"] != null)
                return Convert.ToBoolean(ViewState["_MostrarMessageSelecione"]);
            return true;
        }
        set
        {
            if ((value) && (ddlComboPeriodoCalendario.Items.FindByValue("-1;-1") == null))
                ddlComboPeriodoCalendario.Items.Insert(0, new ListItem("-- Selecione " + VS_TituloPadrao + " --", "-1;-1", true));

            ViewState["_MostrarMessageSelecione"] = value;
            ddlComboPeriodoCalendario.AppendDataBoundItems = value;
        }
    }

    /// <summary>
    /// Propriedade que diz se será carregado automaticamente o período atual ou não.
    /// </summary>
    public bool SelecionaPeriodoAtualAoCarregar
    {
        get
        {
            if (ViewState["SelecionaPeriodoAtualAoCarregar"] != null)
                return Convert.ToBoolean(ViewState["SelecionaPeriodoAtualAoCarregar"]);
            return false;
        }
        set
        {
            ViewState["SelecionaPeriodoAtualAoCarregar"] = value;
        }
    }

    #endregion

    #region Métodos

    /// <summary>
    /// Seta o foco no combo    
    /// </summary>
    public void SetarFoco()
    {
        ddlComboPeriodoCalendario.Focus();
    }

    /// <summary>
    /// Verifica se existe pelo menos um item no dropdownlist.
    /// </summary>    
    public bool ExisteAlgumItem()
    {
        var x = from ListItem item in ddlComboPeriodoCalendario.Items
                where item.Value != "-1;-1"
                select item;

        return (x.Count() > 0);
    }

    /// <summary>
    /// Limpa todos os itens carregados no combo
    /// </summary>
    public void LimpaItens()
    {
        ddlComboPeriodoCalendario.Items.Clear();
    }

    /// <summary>
    /// Carrega os períodos do calendário de acordo com o calendário, e quando for
    /// disciplina eletiva ou eletiva do aluno, somente os períodos que a disciplina oferece.
    /// </summary>
    /// <param name="cal_id">ID do calendário</param>
    /// <param name="tud_id">ID da disciplina</param>
    /// <param name="tur_id">ID da turma - obrigatório</param>
    public void CarregarTodosPor_EventoEfetivacao(int cal_id, long tud_id, long tur_id, long doc_id = -1)
    {
        ddlComboPeriodoCalendario.Items.Clear();

        ddlComboPeriodoCalendario.DataSource = ACA_TipoPeriodoCalendarioBO.SelecionaTodosPor_EventoEfetivacao(cal_id, tud_id, tur_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, null, doc_id);
        _MostrarMessageSelecione = _MostrarMessageSelecione;
        ddlComboPeriodoCalendario.DataBind();
    }

    /// <summary>
    /// Carrega os períodos do calendário de acordo com o calendário, e quando for
    /// disciplina eletiva ou eletiva do aluno, somente os períodos que a disciplina oferece.
    /// Traz períodos que estejam vigentes (período atual), ou se houver um evento de efetivação
    /// vigente ligado ao tpc_id.
    /// </summary>
    /// <param name="cal_id">ID do calendário</param>
    /// <param name="tud_id">ID da disciplina</param>
    /// <param name="tur_id">ID da turma - obrigatório</param>
    public void CarregarPor_PeriodoVigente_EventoEfetivacaoVigente
    (
        int cal_id
        , long tud_id
        , long tur_id
    )
    {
        ddlComboPeriodoCalendario.Items.Clear();

        ddlComboPeriodoCalendario.DataSource = ACA_TipoPeriodoCalendarioBO.SelecionaPor_PeriodoVigente_EventoEfetivacaoVigente(cal_id, tud_id, tur_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id, ApplicationWEB.AppMinutosCacheLongo, __SessionWEB.__UsuarioWEB.Docente.doc_id);
        _MostrarMessageSelecione = _MostrarMessageSelecione;
        ddlComboPeriodoCalendario.DataBind();
    }

    /// <summary>
    /// Carrega os períodos do calendário de acordo com o calendário, e quando for
    /// disciplina eletiva ou eletiva do aluno, somente os períodos que a disciplina oferece.
    /// Não adiciona o item "-- Selecione --".
    /// </summary>
    /// <param name="cal_id">ID do calendário</param>
    /// <param name="tud_id">ID da disciplina</param>
    public void CarregarPorPeriodoVigente_AteAtual
    (
        int cal_id
        , long tud_id
    )
    {
        DataTable dt = ACA_TipoPeriodoCalendarioBO.CarregarPeriodosAteDataAtual(cal_id, tud_id);
        ddlComboPeriodoCalendario.Items.Clear();

        ddlComboPeriodoCalendario.DataSource = dt;
        _MostrarMessageSelecione = _MostrarMessageSelecione;
        ddlComboPeriodoCalendario.DataBind();

        if (tud_id > 0)
        {
            // Se passou a disciplina, verificar se o período atual está na lista de períodos da
            // disciplina.
            var x = from DataRow dr in dt.Rows
                    where Convert.ToBoolean(dr["PeriodoAtual"])
                    select dr[ddlComboPeriodoCalendario.DataValueField].ToString();

            if (x.Count() > 0)
            {
                ddlComboPeriodoCalendario.SelectedValue = x.First();
            }
        }
    }

    /// <summary>
    /// Carrega os períodos do calendário de acordo com o calendário, e quando for
    /// disciplina eletiva ou eletiva do aluno, somente os períodos que a disciplina oferece.
    /// Não adiciona o item "-- Selecione --".
    /// </summary>
    /// <param name="cal_id">ID do calendário</param>
    /// <param name="tur_id">ID da turma</param>
    public void CarregarPorPeriodoVigente_AteAtualPorTurma
    (
        int cal_id
        , long tur_id
    )
    {
        DataTable dt = ACA_TipoPeriodoCalendarioBO.CarregarPeriodosAteDataAtualPorTurma(cal_id, tur_id);
        ddlComboPeriodoCalendario.Items.Clear();

        ddlComboPeriodoCalendario.DataSource = dt;
        _MostrarMessageSelecione = _MostrarMessageSelecione;
        ddlComboPeriodoCalendario.DataBind();
    }

    /// <summary>
    /// Mostra os dados não excluídos logicamente no dropdownlist avaliações do Formato de avaliação
    /// </summary>
    /// <param name="Tur_id">ID da Turma</param>
    public void CarregarTipoPeriodoCalendario_FAV
    (
        long Tur_id
    )
    {
        ddlComboPeriodoCalendario.DataTextField = "cap_descricao";

        ddlComboPeriodoCalendario.Items.Clear();
        ddlComboPeriodoCalendario.DataSource = ACA_TipoPeriodoCalendarioBO.SelecionaTipoPeriodoCalendario_Fav_Tur(Tur_id);
        _MostrarMessageSelecione = _MostrarMessageSelecione;
        ddlComboPeriodoCalendario.DataBind();
    }

    /// <summary>
    /// Mostra os dados não excluídos logicamente no dropdownlist por calendário    
    /// </summary>
    /// <param name="cal_id">ID do calendário</param>
    public void CarregarTipoPeriodoCalendarioPorCalendario
    (
        int cal_id
    )
    {
        ddlComboPeriodoCalendario.Items.Clear();
        ddlComboPeriodoCalendario.DataTextField = "cap_descricao";

        List<sTipoPeriodoCalendario> dt = ACA_TipoPeriodoCalendarioBO.SelecionaTipoPeriodoCalendarioPorCalendario(cal_id, ApplicationWEB.AppMinutosCacheLongo);
        ddlComboPeriodoCalendario.DataSource = dt;
        _MostrarMessageSelecione = _MostrarMessageSelecione;
        ddlComboPeriodoCalendario.DataBind();

        if (SelecionaPeriodoAtualAoCarregar)
        {
            var x = from dr in dt
                    where dr.PeriodoAtual.Equals(1)
                    select dr.tpc_cap_id.ToString();

            if (x.Count() > 0)
            {
                ddlComboPeriodoCalendario.SelectedValue = x.First();
            }
        }
    }

    /// <summary>
    /// Mostra os dados não excluídos logicamente no dropdownlist por calendário    
    /// </summary>
    /// <param name="cal_id">ID do calendário</param>
    public void CarregarTipoPeriodoCalendarioPorCalendario_Cap_id
    (
        int cal_id
    )
    {
        ddlComboPeriodoCalendario.Items.Clear();
        ddlComboPeriodoCalendario.DataTextField = "cap_descricao";

        ddlComboPeriodoCalendario.DataSource = ACA_TipoPeriodoCalendarioBO.SelecionaTipoPeriodoCalendarioPorCalendario(cal_id);
        _MostrarMessageSelecione = _MostrarMessageSelecione;
        ddlComboPeriodoCalendario.DataBind();
    }

    /// <summary>
    /// Mostra os dados não excluídos logicamente no dropdownlist por calendário    
    /// </summary>
    /// <param name="cal_id">ID do calendário</param>
    public void CarregarTipoPeriodoCalendarioPorCalendario_Cap_id_Listao
    (
        int cal_id
    )
    {
        ddlComboPeriodoCalendario.Items.Clear();
        ddlComboPeriodoCalendario.DataTextField = "cap_descricao";

        ddlComboPeriodoCalendario.DataSource = ACA_TipoPeriodoCalendarioBO.SelecionaTipoPeriodoCalendarioPorCalendario(cal_id);
        _MostrarMessageSelecione = _MostrarMessageSelecione;
        ddlComboPeriodoCalendario.DataBind();

        // se tem 5 periodos, nao mostrar o ultimo
        if ((_MostrarMessageSelecione && ddlComboPeriodoCalendario.Items.Count == 6) || (!_MostrarMessageSelecione && ddlComboPeriodoCalendario.Items.Count == 5))
        {
            ddlComboPeriodoCalendario.Items.RemoveAt(ddlComboPeriodoCalendario.Items.Count - 1);
        }
    }

    #endregion

    #region Eventos

    #region Page Life Cycle

    protected void Page_Init(object sender, EventArgs e)
    {
        lblTitulo.Text = GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
        //Altera a mensagem de validação para o nome padrão de periodo calendario
        cpvCombo.ErrorMessage = lblTitulo.Text + " é obrigatório.";
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        ddlComboPeriodoCalendario.AutoPostBack = IndexChanged != null;
    }

    #endregion

    protected void ddlCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (IndexChanged != null)
            IndexChanged();
    }

    #endregion
}
