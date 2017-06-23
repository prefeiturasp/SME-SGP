using System;
using System.Data;
using System.Web;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using System.Web.UI.WebControls;
using System.Collections.Generic;

public partial class Configuracao_TipoAtividadeAvaliativa_Cadastro : MotherPageLogado
{

    #region Eventos Page Life Cycle

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CarregarQualificadores();

            if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                Carregar(PreviousPage.EditItem);
            else
                btnSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;

            Page.Form.DefaultFocus = txtTipoAtividadeAvaliativa.ClientID;
            Page.Form.DefaultButton = btnSalvar.UniqueID;
        }
    }

    #endregion

    #region PROPRIEDADES

    /// <summary>
    /// Propriedade em ViewState que armazena valor de tav_id
    /// no caso de atualização de um registro ja existente.
    /// </summary>
    private int _VS_tav_id
    {
        get
        {
            if (ViewState["_VS_tav_id"] != null)
                return Convert.ToInt32(ViewState["_VS_tav_id"]);
            return -1;
        }
        set
        {
            ViewState["_VS_tav_id"] = value;
        }
    }

    #endregion

    #region METODOS

    /// <summary>
    /// Insere ou altera um tipo de atividade avaliativa
    /// </summary>
    private void Salvar()
    {
        try
        {

            CLS_TipoAtividadeAvaliativa entity = new CLS_TipoAtividadeAvaliativa();
            entity.tav_id = _VS_tav_id;
            entity = CLS_TipoAtividadeAvaliativaBO.GetEntity(entity);
            entity.tav_nome = txtTipoAtividadeAvaliativa.Text;
            entity.qat_id = Int32.Parse(ddlQualificador.SelectedValue);

            if (_VS_tav_id > 0)
            {
                entity.IsNew = false;
                entity.tav_dataAlteracao = DateTime.Now;
            }
            else
            {
                entity.IsNew = true;
                entity.tav_situacao = 1;
                entity.tav_dataAlteracao = DateTime.Now;
                entity.tav_dataCriacao = DateTime.Now;
            }        

            if (CLS_TipoAtividadeAvaliativaBO.Save(entity))
            {
                if (_VS_tav_id <= 0)
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "tav_id: " + entity.tav_id);
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Tipo de atividade avaliativa incluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }
                else
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tav_id: " + entity.tav_id);
                    __SessionWEB.PostMessages = UtilBO.GetErroMessage("Tipo de atividade avaliativa alterado com sucesso.", UtilBO.TipoMensagem.Sucesso);
                }

                Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Configuracao/TipoAtividadeAvaliativa/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else
            {
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o tipo de atividade avaliativa.", UtilBO.TipoMensagem.Erro);
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
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o tipo de atividade avaliativa.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Carrega os dados do tipo de atividade avaliativa nos controles caso seja alteração.
    /// </summary>
    /// <param name="tav_id"></param>
    private void Carregar(int tav_id)
    {
        try
        {
            CLS_TipoAtividadeAvaliativa entity = new CLS_TipoAtividadeAvaliativa { tav_id = tav_id };
            entity = CLS_TipoAtividadeAvaliativaBO.CarregaDados(entity.tav_id);

            _VS_tav_id = entity.tav_id;
            txtTipoAtividadeAvaliativa.Text = entity.tav_nome;
            ddlQualificador.SelectedValue = entity.qat_id.ToString();
        }
        catch (Exception e)
        {
            ApplicationWEB._GravaErro(e);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o tipo de atividade avaliativa.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Carrega o dropDownList com os Qualificadores
    /// </summary>
    protected void CarregarQualificadores()
    {
        ddlQualificador.Items.Clear();

        ddlQualificador.DataTextField = "qat_nome";
        ddlQualificador.DataValueField = "qat_id";

        ddlQualificador.Items.Add(new ListItem(GetGlobalResourceObject("Configuracao", "TipoAtividadeAvaliativa.Cadastro.ddlQualificador.MensagemSelecione").ToString(), "-1"));

        IList<CLS_QualificadorAtividade> lista = CLS_QualificadorAtividadeBO.GetSelect();

        foreach (CLS_QualificadorAtividade lt in lista)
        {
            ddlQualificador.Items.Add(new ListItem(lt.qat_nome, lt.qat_id.ToString()));
        }
    }

    #endregion

    #region EVENTOS

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect("Busca.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        Salvar();
    }
    
    #endregion
}
