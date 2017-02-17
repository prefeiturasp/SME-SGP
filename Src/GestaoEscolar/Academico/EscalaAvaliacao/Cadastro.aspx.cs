using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.BLL;
using System.Data;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL;
using System.Web;
using System.Collections.Generic;
using System.Linq;

public partial class Academico_EscalaAvaliacao_Cadastro : MotherPageLogado
{
    #region [Variaveis]
    string msgparecer = string.Empty;
    #endregion

    #region Propriedades

    private int _VS_esa_id
    {
        get
        {
            if (ViewState["_VS_esa_id"] != null)
            {
                return Convert.ToInt32(ViewState["_VS_esa_id"]);
            }
            return -1;
        }
        set
        {
            ViewState["_VS_esa_id"] = value;
        }
    }

    #endregion

    #region Métodos

    private void _LoadFromEntity(int esa_id)
    {
        try
        {
            ACA_EscalaAvaliacao _escalaAvaliacao = new ACA_EscalaAvaliacao { esa_id = esa_id };
            ACA_EscalaAvaliacaoBO.GetEntity(_escalaAvaliacao);

            if (_escalaAvaliacao.ent_id != __SessionWEB.__UsuarioWEB.Usuario.ent_id)
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage("A escala de avaliação não pertence à entidade na qual você está logado.", UtilBO.TipoMensagem.Alerta);
                Response.Redirect("~/Academico/EscalaAvaliacao/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

            _VS_esa_id = _escalaAvaliacao.esa_id;
            _ddlTipo.SelectedIndex = _escalaAvaliacao.esa_tipo;
            _ddlTipo.Enabled = false;
            _txtNome.Text = _escalaAvaliacao.esa_nome;
            _ckbBloqueado.Checked = _escalaAvaliacao.esa_situacao.Equals(2);

            ESC_Escola escola = new ESC_Escola { esc_id = _escalaAvaliacao.esc_id };
            ESC_EscolaBO.GetEntity(escola);
            MSTech.CoreSSO.Entities.SYS_UnidadeAdministrativa unidAdm = new MSTech.CoreSSO.Entities.SYS_UnidadeAdministrativa { ent_id = escola.ent_id, uad_id = escola.uad_id };
            SYS_UnidadeAdministrativaBO.GetEntity(unidAdm);

            _fieldParecer.Visible = _escalaAvaliacao.esa_tipo == 2;

            if (_ddlTipo.SelectedIndex.Equals(2))
            {
                DataTable dtParecer = ACA_EscalaAvaliacaoParecerBO.Seleciona_esa_id(esa_id, false, 1, 1);
                if (dtParecer.Rows.Count > 0)
                {
                    DataView dv = dtParecer.DefaultView;
                    dv.Sort = "eap_ordem";
                }

                _dgvParecer.DataSource = dtParecer;
                _dgvParecer.DataBind();
            }
            else if (_ddlTipo.SelectedIndex.Equals(1))
            {
                ACA_EscalaAvaliacaoNumerica _escalaNumerica = new ACA_EscalaAvaliacaoNumerica { esa_id = esa_id };
                ACA_EscalaAvaliacaoNumericaBO.GetEntity(_escalaNumerica);

                _txtMaiorValor.Text = _escalaNumerica.ean_maiorValor.ToString().TrimEnd('0');
                _txtMaiorValor.Text = _txtMaiorValor.Text.TrimEnd(',');


                _txtMenorValor.Text = _escalaNumerica.ean_menorValor.ToString().TrimEnd('0');
                _txtMenorValor.Text = _txtMenorValor.Text.TrimEnd(',');


                _txtVariacao.Text = _escalaNumerica.ean_variacao.ToString().TrimEnd('0');
                _txtVariacao.Text = _txtVariacao.Text.TrimEnd(',');


                _AlteraTelaEscalaAvaliacao = false;
            }
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar a escala de avaliação.", UtilBO.TipoMensagem.Erro);
        }
    }

    private bool _AlteraTelaEscalaAvaliacao
    {
        set
        {
            _lblMenorValor.Visible = !value;
            _lblMaiorValor.Visible = !value;
            _lblVariacao.Visible = !value;

            _rfvMenorValor.Visible = !value;
            _rfvMaiorValor.Visible = !value;
            _rfvVariacao.Visible = !value;

            _txtMenorValor.Visible = !value;
            _txtMaiorValor.Visible = !value;
            _txtVariacao.Visible = !value;

            _fieldParecer.Visible = value;
        }
    }

    /// <summary>
    /// Lê dados da grid ordena por "eap_ordem" e constroi a grid novamente.
    /// </summary>
    private void AtualizaGrid(List<ACA_EscalaAvaliacaoParecer> list)
    {
        var x = from ACA_EscalaAvaliacaoParecer ent in list                
                orderby ent.eap_ordem
                select ent;
                
       
        _dgvParecer.DataSource = x;
        _dgvParecer.DataBind();
    }

    /// <summary>
    /// Lê todos os dados existentes na Grid _dgvParecer e atribui para um List.
    /// </summary>
    /// <returns>List list</returns>
    private List<ACA_EscalaAvaliacaoParecer> readGrid() 
    { 
        List<ACA_EscalaAvaliacaoParecer> list = new List<ACA_EscalaAvaliacaoParecer>();

        try
        {
            string  msgvalor = string.Empty
                    ,msgdescricao = string.Empty;

            foreach(GridViewRow row in _dgvParecer.Rows)
            {
                ACA_EscalaAvaliacaoParecer campos = new ACA_EscalaAvaliacaoParecer();
                
                string eap_equivalenteInicio = ((TextBox)row.FindControl("eap_equivalenteInicio")).Text
                        , eap_equivalenteFim = ((TextBox)row.FindControl("eap_equivalenteFim")).Text;
                
                
                campos.esa_id = _VS_esa_id;
                campos.eap_id = Convert.ToInt32(((Label)row.FindControl("lblEap_id")).Text);
                campos.eap_valor = ((TextBox)row.FindControl("txtValor")).Text;
                campos.eap_descricao = ((TextBox)row.FindControl("txtDescricao")).Text;
                campos.eap_abreviatura = ((TextBox)row.FindControl("txtAbreviatura")).Text;
                campos.eap_equivalenteInicio = String.IsNullOrEmpty(eap_equivalenteInicio) ? 0 : Convert.ToDecimal(eap_equivalenteInicio);
                campos.eap_equivalenteFim = String.IsNullOrEmpty(eap_equivalenteFim) ? 0 : Convert.ToDecimal(eap_equivalenteFim);
                campos.eap_ordem = row.RowIndex + 1;
                campos.eap_situacao = 1;

                list.Add(campos);
                
                if (_txtNome.Text.Length <= 0 && _ddlTipo.Text.Equals("2"))
                {
                    if (campos.eap_valor.Length <= 0)
                        msgvalor = "</br>Valor é obrigatório.</br>";                        
                    if(campos.eap_descricao.Length <= 0)
                        msgdescricao = "Descrição é obrigatório.</br>";
                }
            }
            msgparecer = msgvalor + msgdescricao;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar ler os dados na tabela.", UtilBO.TipoMensagem.Erro);
        }

        return list;
    }

    #endregion

    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblMsgParecer.Text = UtilBO.GetErroMessage(lblMsgParecer.Text, UtilBO.TipoMensagem.Informacao);

            try
            {
                if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                {
                    _LoadFromEntity(PreviousPage.EditItem);
                    Page.Form.DefaultFocus = _txtNome.ClientID;
                }
                else
                {
                    _ckbBloqueado.Visible = false;

                    _dgvParecer.DataSource = new DataTable();
                    _dgvParecer.DataBind();

                    Page.Form.DefaultFocus = _ddlTipo.ClientID;
                }

                if (_dgvParecer.Rows.Count > 0)
                {
                    ((ImageButton)_dgvParecer.Rows[0].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                    ((ImageButton)_dgvParecer.Rows[_dgvParecer.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
                }


                HabilitaControles(_uppCadastro.Controls, false);
                HabilitaControles(_fieldParecer.Controls, false);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
            
        }

        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
            sm.Scripts.Add(new ScriptReference("~/Includes/JS-ModuloAcademico.js"));

        }
    }

    protected void _dgvParecer_RowDataBound(object sender, GridViewRowEventArgs e)
    {  
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton btnDelete = (ImageButton)e.Row.FindControl("_btnExcluir");
            if (btnDelete != null)
            {
                btnDelete.CommandArgument = e.Row.RowIndex.ToString();
            }

            ImageButton _btnSubir = (ImageButton)e.Row.FindControl("_btnSubir");
            if (_btnSubir != null)
            {
                _btnSubir.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "cima.png";
                _btnSubir.CommandArgument = e.Row.RowIndex.ToString();                               
                _btnSubir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                
                    if (e.Row.RowIndex == 0)
                        _btnSubir.Visible = false;
                    else
                        _btnSubir.Visible = true;
            }

            ImageButton _btnDescer = (ImageButton)e.Row.FindControl("_btnDescer");
            if (_btnDescer != null)
            {                
                _btnDescer.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "baixo.png";
                _btnDescer.CommandArgument = e.Row.RowIndex.ToString();
                _btnDescer.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
               
                    if (e.Row.RowIndex == 0)
                        _btnDescer.Visible = false;                    
                    else
                        if (e.Row.RowIndex > 0)
                        {
                            _dgvParecer.Rows[_dgvParecer.Rows.Count - 1].FindControl("_btnDescer").Visible = true;
                            _btnDescer.Visible = false;                            
                        }
            }
        }        
            if (_dgvParecer.Rows.Count == 1)
            {
                    _dgvParecer.Rows[_dgvParecer.Rows.Count - 1].FindControl("_btnExcluir").Visible = false;
                    _dgvParecer.Rows[_dgvParecer.Rows.Count - 1].FindControl("ibtnAdd").Visible = true;
            }
            else
                if (_dgvParecer.Rows.Count > 1)
                {
                        _dgvParecer.Rows[_dgvParecer.Rows.Count - 1].FindControl("_btnExcluir").Visible = true;
                        _dgvParecer.Rows[_dgvParecer.Rows.Count - 1].FindControl("ibtnAdd").Visible = true;

                        _dgvParecer.Rows[_dgvParecer.Rows.Count - 2].FindControl("_btnExcluir").Visible = true;
                        _dgvParecer.Rows[_dgvParecer.Rows.Count - 2].FindControl("ibtnAdd").Visible = false;
                }
    }

    protected void _dgvParecer_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Excluir")
        {
            try
            {
                List<ACA_EscalaAvaliacaoParecer> listParecer = readGrid();
                
                listParecer.RemoveAt(Convert.ToInt32(e.CommandArgument));

                // Atualiza Grid.
                AtualizaGrid(listParecer);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir.", UtilBO.TipoMensagem.Erro);
            }
        }
        else if (e.CommandName == "Subir")
        {
            try
            {
                int indice = Convert.ToInt32(e.CommandArgument);

                List<ACA_EscalaAvaliacaoParecer> list = readGrid();

                ACA_EscalaAvaliacaoParecer Subir = list[indice];
                ACA_EscalaAvaliacaoParecer Descer = list[indice - 1];

                int eap_ordemSubir = Subir.eap_ordem;

                Subir.eap_ordem = Descer.eap_ordem;
                Descer.eap_ordem = eap_ordemSubir;

                // Atualiza Grid.
                AtualizaGrid(list);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar mudar ordem para cima.", UtilBO.TipoMensagem.Erro);
            }
        }
        else if (e.CommandName == "Descer")
        {
            try
            {
                int indice = Convert.ToInt32(e.CommandArgument);

                List<ACA_EscalaAvaliacaoParecer> list = readGrid();

                ACA_EscalaAvaliacaoParecer Descer = list[indice];
                ACA_EscalaAvaliacaoParecer Subir = list[indice + 1];

                int eap_ordemSubir = Subir.eap_ordem;

                Subir.eap_ordem = Descer.eap_ordem;
                Descer.eap_ordem = eap_ordemSubir;

                // Atualiza Grid.
                AtualizaGrid(list);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar mudar ordem para baixo.", UtilBO.TipoMensagem.Erro);
            }            
        }
        else if (e.CommandName == "Adicionar") 
        {
            AdicionaItemGrid();
        }
    }

    /// <summary>
    /// Pega os itens do grid e adiciona uma linha vazia.
    /// </summary>
    private void AdicionaItemGrid()
    {
        List<ACA_EscalaAvaliacaoParecer> list = readGrid();//alterado

        list.Add(new ACA_EscalaAvaliacaoParecer { eap_ordem = list.Count + 2, eap_situacao = 1 });

        AtualizaGrid(list);
    }

    protected void _ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_ddlTipo.SelectedValue.Equals("1"))
        {
            _AlteraTelaEscalaAvaliacao = false;
        }
        else if (_ddlTipo.SelectedValue.Equals("2"))
        {
            _AlteraTelaEscalaAvaliacao = true;
            if (_dgvParecer.Rows.Count == 0)
            // Adiciona item vazio.
            AdicionaItemGrid();
        }
        else
        {
            _lblMenorValor.Visible = false;
            _lblMaiorValor.Visible = false;
            _lblVariacao.Visible = false;

            _rfvMenorValor.Visible = false;
            _rfvMaiorValor.Visible = false;
            _rfvVariacao.Visible = false;

            _txtMenorValor.Visible = false;
            _txtMaiorValor.Visible = false;
            _txtVariacao.Visible = false;

            _fieldParecer.Visible = false;
        }
        if (_ddlTipo.SelectedValue.Equals("-1"))
        {
            _ddlTipo.Focus();
        }
        else
        {
            _txtNome.Focus();
        }
    }
    
    protected void _btnCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/EscalaAvaliacao/Busca.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    #endregion
}