using System;
using System.Linq;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.CoreSSO.BLL;
using System.Data;
using System.Web;
using MSTech.Validation.Exceptions;
using System.Web.UI.WebControls;
using System.Collections.Generic;

public partial class Configuracao_TipoDisciplina_Cadastro : MotherPageLogado
{
    #region Propriedades

    /// <summary>
    /// Propriedade que verifica o carregamento dos tipos de deficiências
    /// </summary>
    private bool _VS_CarregouTiposDeficiencias
    {
        get
        {
            if (ViewState["_VS_CarregouTiposDeficiencias"] != null)
            {
                return Convert.ToBoolean(ViewState["_VS_CarregouTiposDeficiencias"]);
            }

            return false;
        }

        set
        {
            ViewState["_VS_CarregouTiposDeficiencias"] = value;
        }
    }

    /// <summary>
    /// Propriedade em ViewState que armazena valor de tds_id
    /// no caso de atualização ou inclusão de um registro.
    /// </summary>
    private int _VS_tds_id
    {
        get
        {
            if (ViewState["_VS_tds_id"] != null)
                return Convert.ToInt32(ViewState["_VS_tds_id"]);
            return -1;
        }
        set
        {
            ViewState["_VS_tds_id"] = value;
        }
    }

    /// <summary>
    /// Propriedade em ViewState que armazena valor de tds_ordem
    /// </summary>
    private int _VS_tds_ordem
    {
        get
        {
            if (ViewState["_VS_tds_ordem"] != null)
            {
                return Convert.ToInt32(ViewState["_VS_tds_ordem"]);
            }
            return -1;
        }
        set
        {
            ViewState["_VS_tds_ordem"] = value;
        }
    }

    /// <summary>
    /// ViewState com datatable de tipos de deficiencias
    /// Retorno e atribui valores para o DataTable de tipos de deficiencias
    /// </summary>
    public DataTable _VS_TipoDeficiencias
    {
        get
        {
            if (ViewState["_VS_TipoDeficiencias"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("tde_id");
                dt.Columns.Add("tde_nome");
                dt.Columns.Add("tde_situacao");
                dt.Columns.Add("tde_dataCriacao");
                dt.Columns.Add("tde_dataAlteracao");
                dt.Columns.Add("tde_integridade");

                ViewState["_VS_TipoDeficiencias"] = dt;
            }

            return (DataTable)ViewState["_VS_TipoDeficiencias"];
        }

        set
        {
            ViewState["_VS_TipoDeficiencias"] = value;
        }
    }

    #endregion

    #region Métodos

    /// <summary>
    /// Método para carregar um registro de tipo de disciplina, a fim de atualizar suas informações.
    /// Recebe dados referente ao tipo de disciplina para realizar a busca.
    /// </summary>
    /// <param name="tds_id">ID do tipo de disciplina</param>
    private void _Carregar(int tds_id)
    {
        try
        {
            ACA_TipoDisciplina _TipoDisciplina = new ACA_TipoDisciplina { tds_id = tds_id };
            ACA_TipoDisciplinaBO.GetEntity(_TipoDisciplina);
            _VS_tds_id = _TipoDisciplina.tds_id;
            _VS_tds_ordem = _TipoDisciplina.tds_ordem;
            _txtTipoDisciplina.Text = _TipoDisciplina.tds_nome;
            _ddlBase.SelectedValue = _TipoDisciplina.tds_base.ToString();
            UCComboTipoNivelEnsino1.Valor = _TipoDisciplina.tne_id;
            UCComboTipoNivelEnsino1.PermiteEditar = false;
            txtNomeDisciplinaEspecial.Text = _TipoDisciplina.tds_nomeDisciplinaEspecial;
            UCComboAreaConhecimento1.Valor = _TipoDisciplina.aco_id;
            txtQtdeDisciplinaRelacionada.Text = _TipoDisciplina.tds_qtdeDisciplinaRelacionada.ToString();

            if (!String.IsNullOrEmpty(txtNomeDisciplinaEspecial.Text.Trim()))
            {
                ckbAlunoEspecial.Checked = true;
            }

            if (_TipoDisciplina.tds_tipo == (byte)ACA_TipoDisciplinaBO.TipoDisciplina.RecuperacaoParalela)
            {
                DataTable dtDisciplinas = ACA_TipoDisciplinaBO.SelecionaTipoDisciplinaRelacionadaPorTipo(_TipoDisciplina.tds_id, ((byte)ACA_TipoDisciplinaBO.TipoDisciplina.Disciplina).ToString());
                rptRelacionadas.DataSource = dtDisciplinas;
                rptRelacionadas.DataBind();
                fdsRelacionadas.Visible = true;
                btnSalvar.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o tipo de " + GetGlobalResourceObject("Mensagens","MSG_DISCIPLINA") + ".", UtilBO.TipoMensagem.Erro);
        }
    }
    
    /// <summary>
    /// Método para habilitar/desabilitar a Div de disciplina especial
    /// </summary>
    private void habilitaDivDisciplinaEspecial()
    {
        divDisciplinaEspecial.Visible = false;
        if (ckbAlunoEspecial.Checked)
        {
            divDisciplinaEspecial.Visible = true;
        }

        if (_VS_CarregouTiposDeficiencias)
        {  // procedimento que impede a execução desnecessária do trecho de código abaixo e evita a perda de marcações que ainda 
            // não estão gravadas ao desmarcar o ckbAlunoEspecial.  
            return;
        }
        _VS_CarregouTiposDeficiencias = true;

        // Procedimentos para carregar e adicionar as deficiencias no checkboxlist
        CarregaDeficiencias();
        AdicionaDeficiencias();

        inicializaChkDeficiencias();
 
        if (ckbAlunoEspecial.Checked)
        {
            // realizo esse procedimento quando estou carregando os dados já cadastrados
            List<DataRow> ltDeficienciasSelecionadas =
                          (from DataRow dr in ACA_TipoDisciplinaDeficienciaBO.SelectBy_ACA_TipoDisciplina_ACA_TipoDisciplinaDeficiencia(_VS_tds_id).Rows
                           where Convert.ToInt32(dr["tds_id"]) == _VS_tds_id
                           select dr).ToList();

            foreach (ListItem item in ltDeficienciasSelecionadas.SelectMany(row => chkDeficiencias.Items.Cast<ListItem>().Where(i => i.Value == row["tde_id"].ToString())))
            { // procedimento compara o que esta cadastrado em TipoDisciplinaDeficiencia com o que foi adicionado no checkboxlist e marcar  
                item.Selected = true;
            }
        }
    }

    /// <summary>
    /// Método utilizado para trazer todo/iniciliza com desmarcado
    /// </summary>
    private void inicializaChkDeficiencias()
    {
        foreach (ListItem item in chkDeficiencias.Items)
        {
            item.Selected = false;
        }
    }

    /// <summary>
    /// Carregas as deficiências cadastradas no Core
    /// </summary>
    private void CarregaDeficiencias()
    {   // busca do core deficiencias cadastradas
        DataTable dtTipoDeficiencias = PES_TipoDeficienciaBO.GetSelect(Guid.Empty, string.Empty, 1, false, 1, 1);
        if (dtTipoDeficiencias.Rows.Count == 0)
        {
            dtTipoDeficiencias = null;
        }
        _VS_TipoDeficiencias = dtTipoDeficiencias;

        pnlDeficiencias.Visible = true;
    }

    /// <summary>
    /// Adiciona as deficiencias carregadas ao checkboxlist
    /// </summary>
    private void AdicionaDeficiencias()
    { // adiciono no checkboxlist
        try
        {
            chkDeficiencias.Items.Clear();

            for (int i = 0; i < _VS_TipoDeficiencias.Rows.Count; i++)
            {
                if (_VS_TipoDeficiencias.Rows[i].RowState != DataRowState.Deleted)
                {
                    ListItem lt = new ListItem
                    {
                        Value = _VS_TipoDeficiencias.Rows[i]["tde_id"].ToString(),
                        Text = _VS_TipoDeficiencias.Rows[i]["tde_nome"].ToString(),
                        Selected = false
                    };
                    chkDeficiencias.Items.Add(lt);
                    pnlDeficiencias.Visible = true;
                }
            }
        }
        catch
        {
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao adicionar os tipo de deficiencias.", UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion

    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                _VS_CarregouTiposDeficiencias = false;

                UCComboAreaConhecimento1.CarregarAreaConhecimento();

                if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                    _Carregar(PreviousPage.EditItem);

                UCComboTipoNivelEnsino1.Obrigatorio = true;
                UCComboTipoNivelEnsino1.CarregarTipoNivelEnsino();

                Page.Form.DefaultFocus = _txtTipoDisciplina.ClientID;

                _rfvTipoDisciplina.ErrorMessage = "Tipo de " + GetGlobalResourceObject("Mensagens","MSG_DISCIPLINA") + " é obrigatório.";

                habilitaDivDisciplinaEspecial();

                pnlDeficiencias.GroupingText = GestaoEscolarUtilBO.nomePadraoTipoDeficiencia(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }
    }

    protected void _btnCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Configuracao/TipoDisciplina/Busca.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }
    
    protected void ckbAlunoEspecial_CheckedChanged(object sender, EventArgs e)
    {
        habilitaDivDisciplinaEspecial();
    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        List<ACA_TipoDisciplinaRelacionada> lstRelacionadas = new List<ACA_TipoDisciplinaRelacionada>();
        if (rptRelacionadas.Visible)
        {
            try
            {                
                foreach (RepeaterItem dis in rptRelacionadas.Items)
                {
                    HiddenField hdnId = (HiddenField)dis.FindControl("hdnId");
                    CheckBox ckbRelacionada = (CheckBox)dis.FindControl("ckbRelacionada");
                    if (ckbRelacionada.Checked)
                    {
                        lstRelacionadas.Add(new ACA_TipoDisciplinaRelacionada { tds_id = _VS_tds_id, tds_idRelacionada = Convert.ToInt32(hdnId.Value) });
                        ACA_TipoDisciplinaRelacionada disRelacionada = new ACA_TipoDisciplinaRelacionada();
                    }
                }

                if (lstRelacionadas.Count() > 0)
                {
                    ACA_TipoDisciplinaRelacionadaBO.Save(lstRelacionadas);

                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Tipo de componente curricular salvo com sucesso.", UtilBO.TipoMensagem.Sucesso);

                    Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Configuracao/TipoDisciplina/Busca.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar tipo de componente curricular.", UtilBO.TipoMensagem.Erro);
            }
        }
        
        if (lstRelacionadas.Count() == 0)
        {
            _lblMessage.Text = UtilBO.GetErroMessage("Tipo de componente curricular relacionado é obrigatório.", UtilBO.TipoMensagem.Alerta);
        }
    }

    #endregion
}
