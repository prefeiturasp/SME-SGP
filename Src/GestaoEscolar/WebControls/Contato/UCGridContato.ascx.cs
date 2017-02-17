using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using System.Data;
using System.Linq;
using MSTech.CoreSSO.Entities;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using System.Text.RegularExpressions;
using MSTech.Validation.Exceptions;

public partial class WebControls_Contato_UCGridContato : MotherUserControl
{
    #region Eventos Page Life Cycle

    protected void Page_Init(object sender, EventArgs e)
    {
        if (ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CONTATO_OBRIGATORIO_MATRICULA_ALUNO, __SessionWEB.__UsuarioWEB.Usuario.ent_id))
        {
            grvContato.Columns[0].HeaderText = "Tipo de contato " + ApplicationWEB.TextoAsteriscoObrigatorio;
            grvContato.Columns[1].HeaderText = "Contato " + ApplicationWEB.TextoAsteriscoObrigatorio;
        }
        else
        {
            grvContato.Columns[0].HeaderText = "Tipo de contato";
            grvContato.Columns[1].HeaderText = "Contato";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(Page);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
        }
    }

    #endregion

    #region Métodos

    /// <summary>
    /// Retorna os dados dos contatos a serem salvos e seta as rowstates.
    /// </summary>
    /// <returns> Data table com os dados dos contatos a serem salvos.</returns>
    public DataTable RetornaContatoSave()
    {
        DataTable dtContato = CriaDataTableContato();

        //Preenche o dtDocumento com os dados do grid.
        foreach (GridViewRow linha in grvContato.Rows)
        {
            if (!string.IsNullOrEmpty(((TextBox)linha.FindControl("tbContato")).Text))
            {
                string erro;
                if (ValidarLinhaGrid(linha, out erro))
                {
                    DataRow rowDoc = dtContato.NewRow();

                    rowDoc["id"] = grvContato.DataKeys[linha.RowIndex].Value.ToString();
                    rowDoc["tmc_id"] = ((DropDownList)linha.FindControl("ddlTipoMeioContato")).SelectedValue;
                    rowDoc["tmc_nome"] = ((DropDownList)linha.FindControl("ddlTipoMeioContato")).SelectedItem;
                    rowDoc["contato"] = ((TextBox)linha.FindControl("tbContato")).Text;

                    dtContato.Rows.Add(rowDoc);
                }
                else
                {
                    throw new ValidationException(erro);
                }
            }
        }
        return dtContato;
    }

    /// <summary>
    /// Retorna todos os dados dos contatos que estão no grid.
    /// </summary>
    /// <returns> Data table com dados dos contatos.</returns>
    private DataTable RetornaContato()
    {
        DataTable dtContato = CriaDataTableContato();


        //Preenche o dtDocumento com os dados do grid.
        foreach (GridViewRow linha in grvContato.Rows)
        {

            DataRow rowDoc = dtContato.NewRow();




            rowDoc["id"] = grvContato.DataKeys[linha.RowIndex].Value.ToString();
            rowDoc["tmc_id"] = ((DropDownList)linha.FindControl("ddlTipoMeioContato")).SelectedValue;
            rowDoc["tmc_nome"] = ((DropDownList)linha.FindControl("ddlTipoMeioContato")).SelectedItem;
            rowDoc["contato"] = ((TextBox)linha.FindControl("tbContato")).Text;



            dtContato.Rows.Add(rowDoc);

        }


        return dtContato;
    }

    /// <summary>
    /// Cria o dtDocumento com suas colunas.
    /// </summary>
    /// <returns>DataTable documento configurado.</returns>
    private DataTable CriaDataTableContato()
    {
        DataTable dtContato = new DataTable();

        dtContato.Columns.Add("id");
        dtContato.Columns.Add("tmc_id");
        dtContato.Columns.Add("tmc_nome");
        dtContato.Columns.Add("contato");


        return dtContato;
    }



    /// <summary>
    /// Método para validar os campos a serem inseridos
    /// </summary>
    /// <param name="row">Linha a ser validada</param>
    /// <param name="msgErro"></param>
    /// <returns></returns>
    private bool ValidarLinhaGrid(GridViewRow row, out string erro)
    {
        try
        {
            DropDownList ddlTipoMeioContato = (DropDownList)row.FindControl("ddlTipoMeioContato");
            TextBox contato = (TextBox)row.FindControl("tbContato");
            erro = string.Empty;
            bool retorno = true;

            if (!String.IsNullOrEmpty(contato.Text.Trim()) && ddlTipoMeioContato.SelectedValue.Equals("-1"))
            {
                erro = "O tipo de contato é obrigatório.";
                retorno = false;
            }
            else
            {
                Regex regex;
                Guid tmc_id = new Guid(ddlTipoMeioContato.SelectedValue);

                SYS_TipoMeioContato tmc = new SYS_TipoMeioContato { tmc_id = tmc_id };
                SYS_TipoMeioContatoBO.GetEntity(tmc);

                if (tmc.tmc_validacao == (byte)SYS_TipoMeioContatoValidacao.Email)
                {
                    regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.None);
                    if (!regex.IsMatch(contato.Text))
                    {
                        erro = ddlTipoMeioContato.SelectedItem.Text + " está fora do padrão ( seuEmail@seuProvedor )";
                        retorno = false;
                    }
                }
                else
                {
                    if (tmc.tmc_validacao == (byte)SYS_TipoMeioContatoValidacao.Telefone)
                    {
                        regex = new Regex(@"^(\(\d{2}\))?[\s]?\d{3,5}-?\d{4}$", RegexOptions.None);
                        if (!regex.IsMatch(contato.Text))
                        {
                            erro = ddlTipoMeioContato.SelectedItem.Text + " está fora do padrão ( (XX) XXX-XXXX ou (XX) XXXX-XXXX ou (XX) XXXXX-XXXX ou (XX) XXXXXXX ou (XX) XXXXXXXX ou (XX) XXXXXXXXX ou XXXX-XXXX ou XXXXX-XXXX ou XXXXXXXX ou XXXXXXXXX)";
                            retorno = false;
                        }
                    }
                    else
                    {
                        if (tmc.tmc_validacao == (byte)SYS_TipoMeioContatoValidacao.WebSite)
                        {
                            regex = new Regex(@"^(http(s)?://)?([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$", RegexOptions.None);
                            if (!regex.IsMatch(contato.Text))
                            {
                                erro = ddlTipoMeioContato.SelectedItem.Text + " está fora do padrão (http(s)://seuSite.dominio ou http(s)://www.seuSite.dominio)";
                                retorno = false;
                            }
                        }
                    }
                }
            }
            _lblMessage.Visible = !retorno;
            return retorno;
        }
        catch (Exception ex)
        {
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Alerta);
            ApplicationWEB._GravaErro(ex);
            erro = string.Empty;

            return false;
        }
    }
    /// <summary>
    /// Adiciona uma nova linha ao grid.
    /// </summary>
    private void AdicionaLinhaGrid()
    {
        // Retorna as linhas já existentes no grid.
        DataTable dtContato = RetornaContato();


        // Adiciona uma nova linha.
        DataRow dr = dtContato.NewRow();
        dr["id"] = Guid.Empty;
        dtContato.Rows.Add(dr);

        // Carrega a nova linha no grid.
        grvContato.DataSource = dtContato;
        grvContato.DataBind();

    }

    /// <summary>
    /// Método para carregar os contatos
    /// </summary>
    /// <param name="pes_id"> Id da pessoa.</param>
    public void IniciaContato(Guid pes_id)
    {
        try
        {
            DataTable dt = CriaDataTableContato();
            if (pes_id != Guid.Empty)
                dt = SYS_TipoMeioContatoBO.SelecionaContatosDaPessoa(pes_id);

            grvContato.DataSource = dt;
            grvContato.DataBind();
            AdicionaLinhaGrid();
        }
        catch (Exception ex)
        {
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Alerta);
            ApplicationWEB._GravaErro(ex);
        }
    }

    /// <summary>
    /// Método para carregar os contatos de uma entidade
    /// </summary>
    /// <param name="ent_id"> Id da entidade.</param>
    public void IniciaContatoEntidade(Guid ent_id)
    {
        try
        {
            DataTable dt = CriaDataTableContato();
            if (ent_id != Guid.Empty)
                dt = SYS_EntidadeContatoBO.GetSelect(ent_id,false,1,0);

            grvContato.DataSource = dt;
            grvContato.DataBind();
            AdicionaLinhaGrid();
        }
        catch (Exception ex)
        {
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Alerta);
            ApplicationWEB._GravaErro(ex);
        }
    }

    #endregion

    #region Eventos

    protected void btnAdicionar_Click(object sender, ImageClickEventArgs e)
    {
        //AtualizaGrid();
        AdicionaLinhaGrid();
    }

    protected void grvContato_DataBound(object sender, EventArgs e)
    {
        // Mostra botão adicionar.
        if (grvContato.Rows.Count > 0)
            grvContato.Rows[grvContato.Rows.Count - 1].FindControl("btnAdicionar").Visible = true;
    }

    protected void grvContato_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ((DropDownList)e.Row.FindControl("ddlTipoMeioContato")).SelectedValue = DataBinder.Eval(e.Row.DataItem, "tmc_id").ToString();

        }
    }

    #endregion
}
