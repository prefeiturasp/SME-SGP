using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

public partial class WebControls_Combos_UCComboDocente : MotherUserControl
{
    #region Delegates

    public delegate void onSelecionaDocente();

    public event onSelecionaDocente _SelecionaDocente;

    #endregion Delegates

    #region Propriedades

    /// <summary>
    /// Retorna e seta o valor selecionado no combo, quando o combo foi carregado com o doc_id mais a chave do cargo dele.
    /// Valor[0] = doc_id
    /// Valor[1] = col_id
    /// Valor[2] = crg_id
    /// Valor[3] = coc_id
    /// </summary>
    public long[] Valor 
    {
        get
        {
            long doc_id, col_id, crg_id, coc_id;

            doc_id = col_id = coc_id = crg_id = -1;

            string[] s = _ddlDocente.SelectedValue.Split(';');

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
        set
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

            if (_ddlDocente.Items.FindByValue(s) != null)
                _ddlDocente.SelectedValue = s;
        }
    }

    /// <summary>
    /// ID do docente selecionado no combo. Utilizar somente quando o combo foi carregado só com o doc_id como chave.
    /// </summary>
    public long Doc_id
    {
        get
        {
            return Valor[0];
        }
        set
        {
            if (_ddlDocente.Items.FindByValue(value.ToString()) != null)
                Valor = new long[] { value };
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
                AdicionaAsteriscoObrigatorio(_lblDocente);
            }
            else
            {
                RemoveAsteriscoObrigatorio(_lblDocente);
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
    /// Deixa o combo habilitado de acordo com o valor passado
    /// </summary>
    public bool PermiteEditar
    {
        set
        {
            _ddlDocente.Enabled = value;
        }
        get
        {
            return _ddlDocente.Enabled;
        }
    }

    /// <summary>
    /// Texto do título ao combo.
    /// </summary>
    public string Texto
    {
        set
        {
            _lblDocente.Text = value;
            cpvCombo.ErrorMessage = value + " é obrigatório.";
        }
    }

    public DropDownList _Combo
    {
        get
        {
            return _ddlDocente;
        }
        set
        {
            _ddlDocente = value;
        }
    }

    /// <summary>
    /// Nome do docente selecionado no combo.
    /// </summary>
    public string TextoSelecionado
    {
        get
        {
            return _ddlDocente.SelectedItem.Text;
        }
    }

    /// <summary>
    /// Exibir informação de vínculo extra.
    /// </summary>
    public bool VS_vinculoExtra
    {
        get
        {
            if (ViewState["VS_vinculoExtra"] != null)
            {
                return Convert.ToBoolean(ViewState["VS_vinculoExtra"]);
            }

            return false;
        }

        set
        {
            ViewState["VS_vinculoExtra"] = value;
        }
    }

    #region Remover - Não utilizar

    /// <summary>
    /// Variavel (flag) na qual cancela o metodo de select do ObjectDataSource
    /// do DropDownList, deixando o combo vazio.
    /// </summary>
    public bool _CancelaSelect { set; get; }

    /// <summary>
    /// Adiciona e remove a mensagem "Selecione um Docente" do dropdownlist.
    /// Por padrão é false e a mensagem "Selecione um Docente" não é exibida.
    /// </summary>
    public bool _MostrarMessageSelecione
    {
        set
        {
            if (value)
                _ddlDocente.Items.Insert(0, new ListItem("-- Selecione um docente --", "-1", true));
            _ddlDocente.AppendDataBoundItems = value;
        }
    }

    /// <summary>
    /// Propriedade onde armazena em ViewState codigo tdt_id da TurmaDocente. (TUR_TurmaDocente)
    /// Usada no cadastro de Turma.
    /// </summary>
    public int _TurmaDocente_tdt_id
    {
        get
        {
            if (ViewState["_TurmaDocente_tdt_id"] != null)
            {
                return Convert.ToInt32(ViewState["_TurmaDocente_tdt_id"]);
            }
            return -1;
        }
        set
        {
            ViewState["_TurmaDocente_tdt_id"] = value;
        }
    }

    /// <summary>
    /// Propriedade onde armazena em ViewState codigo doc_id da TurmaDocente. (TUR_TurmaDocente)
    /// Usada no cadastro de Turma.
    /// </summary>
    public int _TurmaDocente_doc_id_carregado
    {
        get
        {
            if (ViewState["_TurmaDocente_doc_id_carregado"] != null)
            {
                return Convert.ToInt32(ViewState["_TurmaDocente_doc_id_carregado"]);
            }
            return -1;
        }
        set
        {
            ViewState["_TurmaDocente_doc_id_carregado"] = value;
        }
    }

    /// <summary>
    /// Seta a propriedade Visible do label do combo.m
    /// </summary>
    public bool VisibleLabel
    {
        set
        {
            _lblDocente.Visible = value;
        }
    }

    /// <summary>
    /// Seta a propriedade do tamanho do combo.
    /// </summary>
    public string TamanhoCombo
    {
        set
        {
            _ddlDocente.SkinID = value;
        }
    }

    #endregion Remover - Não utilizar

    #endregion Propriedades

    #region Métodos

    /// <summary>
    /// Método que carrega os docentes
    /// </summary>
    /// <param name="esc_id">Id da escola</param>
    public void CarregarDocentePorTurma(int esc_id, long tur_id)
    {
        _ddlDocente.Items.Clear();
        _MostrarMessageSelecione = true;
        _CancelaSelect = false;

        _ddlDocente.DataTextField = "doc_nome";

        DataTable dt = ACA_DocenteBO.SelecionaPorTurma(__SessionWEB.__UsuarioWEB.Usuario.ent_id, esc_id, tur_id);

        _ddlDocente.DataSource = dt;
        _ddlDocente.DataBind();
    }

    /// <summary>
    /// Mostra os docentes não excluídos logicamente no dropdownlist, podendo
    /// fazer a busca considerando ou desconsiderando a escola unidade e os bloqueados.
    /// </summary>
    public void _Load_By_esc_uni_id(string esc_uni_id, byte doc_situacao)
    {
        try
        {
            int esc_id = 0;
            int uni_id = 0;

            if (!string.IsNullOrEmpty(esc_uni_id))
            {
                esc_id = Convert.ToInt32(esc_uni_id.Split(';')[0]);
                uni_id = Convert.ToInt32(esc_uni_id.Split(';')[1]);
            }
            _ddlDocente.Items.Clear();

            _ddlDocente.DataTextField = "doc_nome";

            DataTable dt = ACA_DocenteBO.SelecionaPorEscola(esc_id, uni_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            _MostrarMessageSelecione = true;
            _ddlDocente.DataSource = dt;
            _ddlDocente.DataBind();
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Traz os docentes que lecionam na escola informda (pela UA do cargo).
    /// </summary>
    /// <param name="mostraCargo">booleano que determina se o cargo deve ser retornado junto ao nome do docente</param>
    /// <param name="esc_id">ID da escola</param>
    /// <returns></returns>
    public void _LoadBy_EscolaCargo(bool mostraCargo, int esc_id)
    {
        if (mostraCargo)
            CarregarDocenteExibindoCargo(esc_id);
        else
            CarregarDocente(esc_id);
    }

    /// <summary>
    /// Traz os docentes que lecionam na escola informda (pela UA do cargo), Seleciona um docente só se passar o doc_id
    /// </summary>
    /// <param name="esc_id">ID da escola</param>
    /// <param name="doc_id">ID do docente</param>
    /// <returns></returns>
    public void _LoadBy_Docente_EscolaCargo(int esc_id, long doc_id)
    {
        CarregarDocenteExibindoCargo(esc_id, doc_id);
    }

    /// <summary>
    /// Traz os docentes que lecionam na turma informda (pela turmadocente)
    /// </summary>
    /// <param name="esc_id">ID da escola</param>
    /// <param name="tur_id">ID da turma</param>
    /// <returns></returns>
    public void _LoadBy_Turma_EscolaCargo(int esc_id, long tur_id)
    {
        CarregarDocenteExibindoCargo_Turma(esc_id, tur_id);
    }

    /// <summary>
    /// Método que carrega os docentes e seus respectivos cargos
    /// </summary>
    /// <param name="esc_id">Id da escola</param>
    private void CarregarDocenteExibindoCargo(int esc_id)
    {
        _ddlDocente.Items.Clear();
        _CancelaSelect = false;

        _ddlDocente.Items.Insert(0, new ListItem("-- Selecione um docente --", "-1;-1;-1;-1", true));
        _ddlDocente.AppendDataBoundItems = true;

        _ddlDocente.DataTextField = "doc_NomeCargo";
        _ddlDocente.DataValueField = "doc_col_crg_coc_id";
        cpvCombo.ValueToCompare = "-1;-1;-1;-1";

        DataTable dt = ACA_DocenteBO.GetSelectBy_EscolaCargo_ExibindoCargo(esc_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

        _ddlDocente.DataSource = dt;
        _ddlDocente.DataBind();
    }

    /// <summary>
    /// Método que carrega os docentes e seus respectivos cargos, Seleciona um docente só se passar o doc_id
    /// </summary>
    /// <param name="esc_id">Id da escola</param>
    /// <param name="doc_id">ID do docente</param>
    private void CarregarDocenteExibindoCargo(int esc_id, long doc_id)
    {
        _ddlDocente.Items.Clear();
        _CancelaSelect = false;

        _ddlDocente.Items.Insert(0, new ListItem("-- Selecione um docente --", "-1;-1;-1;-1", true));
        _ddlDocente.AppendDataBoundItems = true;

        _ddlDocente.DataTextField = "doc_NomeCargo";
        _ddlDocente.DataValueField = "doc_col_crg_coc_id";
        cpvCombo.ValueToCompare = "-1;-1;-1;-1";

        DataTable dt = ACA_DocenteBO.GetSelectBy_Docente_EscolaCargo_ExibindoCargo(esc_id, doc_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
        _ddlDocente.DataSource = dt;
        _ddlDocente.DataBind();
    }

    /// <summary>
    /// Método que carrega os docentes e seus respectivos cargos da turma
    /// </summary>
    /// <param name="esc_id">Id da escola</param>
    /// <param name="tur_id">ID da turma</param>
    private void CarregarDocenteExibindoCargo_Turma(int esc_id, long tur_id)
    {
        _ddlDocente.Items.Clear();
        _CancelaSelect = false;

        _ddlDocente.Items.Insert(0, new ListItem("-- Selecione um docente --", "-1;-1;-1;-1", true));
        _ddlDocente.AppendDataBoundItems = true;

        _ddlDocente.DataTextField = "doc_NomeCargo";
        _ddlDocente.DataValueField = "doc_col_crg_coc_id";
        cpvCombo.ValueToCompare = "-1;-1;-1;-1";

        DataTable dt = ACA_DocenteBO.GetSelectBy_Turma_EscolaCargo_ExibindoCargo(esc_id, tur_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

        _ddlDocente.DataSource = dt;
        _ddlDocente.DataBind();
    }

    /// <summary>
    /// Método que carrega os docentes
    /// </summary>
    /// <param name="esc_id">Id da escola</param>
    private void CarregarDocente(int esc_id)
    {
        _ddlDocente.Items.Clear();
        _MostrarMessageSelecione = true;
        _CancelaSelect = false;

        _ddlDocente.DataTextField = "doc_nome";

        DataTable dt = ACA_DocenteBO.GetSelectBy_EscolaCargo(esc_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);

        _ddlDocente.DataSource = dt;
        _ddlDocente.DataBind();
    }

    /// <summary>
    /// Carrega o combo com os professores que podem lecionar a matéria (tds_id) ou que
    /// sejam especialistas, caso no crg_especialista seja passado true.
    /// </summary>
    public void _LoadBy_Especialidade
        (Int32 esc_id, Int32 uni_id, bool crg_especialista, Int32 tds_id, bool mensagemSelecione, ref DataTable dtDocentesEscola)
    {
        _ddlDocente.Items.Clear();

        if (mensagemSelecione)
        {
            _ddlDocente.Items.Insert(0, new ListItem("-- Selecione um docente --", "-1;-1;-1;-1", true));
            _ddlDocente.AppendDataBoundItems = true;
            cpvCombo.ValueToCompare = "-1;-1;-1;-1";
        }

        _ddlDocente.DataTextField = "doc_NomeCargo";
        _ddlDocente.DataValueField = "doc_col_crg_coc_id";

        if (dtDocentesEscola == null)
        {
            dtDocentesEscola = ACA_DocenteBO.GetSelectBy_Especialidade_Escola(esc_id, uni_id, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
        }

        // Filtra a tabela por cargo especialista e tds_id.
        var lista =
            (from DataRow dr in dtDocentesEscola.Rows
             where
                 (crg_especialista && Convert.ToBoolean(dr["crg_especialista"]))
                 ||
                 (tds_id == Convert.ToInt32(dr["tds_id"]))
             select
                 new
                     {
                         doc_NomeCargo = dr["doc_NomeCargo"].ToString() + ((Convert.ToBoolean(dr["coc_vinculoExtra"]) && VS_vinculoExtra) ? " - Vínculo extra" : string.Empty)
                         ,
                         doc_col_crg_coc_id = dr["doc_col_crg_coc_id"]
                         ,
                     }
            ).Distinct();

        _ddlDocente.DataSource = lista;
        _ddlDocente.DataBind();

        _CancelaSelect = true;
    }

    /// <summary>
    /// Carrega o combo com os professores que possuem a função informado para a escola
    /// </summary>
    /// <param name="esc_id">ID  da escola</param>
    /// <param name="uni_id"> ID da unidade</param>
    /// <param name="fun_id">ID da função</param>
    /// <param name="doc_id">ID do docente especifico(pode ser ignorado o filtro)</param>
    public void LoadBy_Funcao(Int32 esc_id, Int32 uni_id, Int32 fun_id, Int64 doc_id = 0)
    {
        _ddlDocente.Items.Clear();
        _ddlDocente.DataSource = ACA_DocenteBO.SelecionaDocentesPorFuncaoEscola(esc_id, uni_id, fun_id, doc_id);
        _MostrarMessageSelecione = true;
        _ddlDocente.DataBind();
        _CancelaSelect = true;
    }

    #endregion Métodos

    #region Eventos

    protected void Page_PreRender(object sender, EventArgs e)
    {
        _ddlDocente.AutoPostBack = (_SelecionaDocente != null);
    }

    protected void _ddlDocente_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_SelecionaDocente != null)
        {
            _SelecionaDocente();
        }
    }

    #endregion Eventos
}