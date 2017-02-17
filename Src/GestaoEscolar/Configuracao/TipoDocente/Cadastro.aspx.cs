using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

public partial class Configuracao_TipoDocente_Cadastro : MotherPageLogado
{
    
    #region Eventos Page Life Cycle

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            CarregarComboEnum<EnumTipoDocente>(ddlTipoDocente);
            ddlTipoDocente.DataBind();  

            if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                Carregar(PreviousPage.EditItem);
            else
            {
                btnSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
            }

            Page.Form.DefaultFocus = ddlTipoDocente.ClientID;
            Page.Form.DefaultButton = btnSalvar.UniqueID;
            
        }
    }

    #endregion

    #region PROPRIEDADES

    /// <summary>
    /// Propriedade em ViewState que armazena valor de tdc_id
    /// no caso de atualização de um registro ja existente.
    /// </summary>
    private byte _VS_tdc_id
    {
        get
        {
            if (ViewState["_VS_tdc_id"] != null)
                return Convert.ToByte(ViewState["_VS_tdc_id"]);
            return 0;
        }
        set
        {
            ViewState["_VS_tdc_id"] = value;
        }
    }

    #endregion PROPRIEDADES

    #region METODOS

    /// <summary>
    /// Insere e altera um Tipo de Docente.
    /// </summary>
    private void Salvar()
    {
        try
        {
            Byte posicao;

            ACA_TipoDocente _TipoDocente = new ACA_TipoDocente
            {
                tdc_id = _VS_tdc_id > 0 ? _VS_tdc_id : Convert.ToByte(ddlTipoDocente.SelectedItem.Value)
                ,
                tdc_descricao = txtDescricao.Text
                ,
                tdc_nome = txtNome.Text
                ,
                tdc_posicao = Byte.TryParse(txtPosicao.Text, out posicao) ? posicao : posicao  // uso a mesma variavel "posicao", pq se a conversão falhar sera retornado 0
                ,
                tdc_corDestaque = txtCorDestaque.Text
                ,
                IsNew = _VS_tdc_id > 0 ? false : true
            };
                    
            // verifica se existe duplicidade no campo Posicao.
            if (ACA_TipoDocenteBO.VerificaDuplicidadePorPosicao(_TipoDocente))
            {
                  // se existe duplicidade no campo "Posição", não deixo gravar
                 throw new ValidationException("O valor do campo Posição já consta cadastrado para outro tipo de docente.");
            }

            if (ACA_TipoDocenteBO.Save(_TipoDocente))
            {
                if (_VS_tdc_id <= 0)
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "tdc_id: " + _TipoDocente.tdc_id);
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Tipo de docente incluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
                else
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tdc_id: " + _TipoDocente.tdc_id);
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Tipo de docente alterado com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }

                Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Configuracao/TipoDocente/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else
            {
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o tipo de docente.", UtilBO.TipoMensagem.Erro);
            }
        }
        catch (MSTech.Validation.Exceptions.ValidationException e)
        {
            lblMessage.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (DuplicateNameException e)
        {
            lblMessage.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (ArgumentException e)
        {
            lblMessage.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (Exception e)
        {
            ApplicationWEB._GravaErro(e);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o tipo de docente.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Carrega os dados do Tipo de docente nos controles caso seja alteração.
    /// </summary>
    /// <param name="tdc_id"></param>
    private void Carregar(byte tdc_id)
    {
        try
        {
            ACA_TipoDocente _TipoDocente = new ACA_TipoDocente { tdc_id = tdc_id };
            ACA_TipoDocenteBO.GetEntity(_TipoDocente);
            _VS_tdc_id = _TipoDocente.tdc_id;

            txtDescricao.Text = _TipoDocente.tdc_descricao;

            txtNome.Text = _TipoDocente.tdc_nome;

            txtPosicao.Text = _TipoDocente.tdc_posicao.ToString();

            txtCorDestaque.Text = _TipoDocente.tdc_corDestaque;

            ddlTipoDocente.Enabled = false;  // qdo alteração deixa o combobox desabilitado
            ddlTipoDocente.SelectedValue = _VS_tdc_id.ToString();

        }
        catch (Exception e)
        {
            ApplicationWEB._GravaErro(e);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o tipo de docente.", UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion METODOS

    #region EVENTOS

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Configuracao/TipoDocente/Busca.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        Salvar();
    }

    protected void CarregarComboEnum<T>(DropDownList cbo)
    {
        Type objType = typeof(T);
        FieldInfo[] propriedades = objType.GetFields();
        foreach (FieldInfo objField in propriedades)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])objField.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
                cbo.Items.Add(new ListItem(attributes[0].Description, objField.GetRawConstantValue().ToString()));
        }
    }
    
    #endregion EVENTOS

}
