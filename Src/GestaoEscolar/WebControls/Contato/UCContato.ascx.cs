using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using System.Linq;

public partial class WebControls_Contato_UCContato : MotherUserControl
{
    #region Propriedades
    
    /// <summary>
    /// Indica se é contato com variavel Guid ou com variavel Int
    /// </summary>
    public bool _VS_GuidSeq
    {
        get
        {
            if (ViewState["_VS_GuidSeq"] != null)
            {
                return Convert.ToBoolean(ViewState["_VS_GuidSeq"]);
            }

            return true;
        }

        set
        {
            ViewState["_VS_GuidSeq"] = value;
        }
    }

    /// <summary>
    /// Armazena o sequencial para inclusão no DataTable quando for do tipo int
    /// </summary>
    public int _VS_seq
    {
        get
        {
            if (ViewState["_VS_seq"] != null)
            {
                return Convert.ToInt32(ViewState["_VS_seq"]);
            }

            return -1;
        }

        set
        {
            ViewState["_VS_seq"] = value;
        }
    }

    /// <summary>
    /// Propriedade que devolve o DataTable com os dados de contato.
    /// </summary>
    public DataTable _carregaDataTableComContatos
    {
        get { return CarregarDataTableComContatos(false); }
    }

    /// <summary>
    /// Propriedade que controla a visualização da mensagem de erro.
    /// </summary>
    public bool _MensagemErro_Mostrar
    {
        get { return lblMensagemErroContato.Visible; }
        set { lblMensagemErroContato.Visible = value; }
    }

    public string _VS_aba
    {
        get
        {
            if (ViewState["_VS_aba"] != null)
            {
                return ViewState["_VS_aba"].ToString();
            }

            return null;
        }

        set
        {
            ViewState["_VS_aba"] = value;
        }
    }

    public string _VS_ValidationGroup
    {
        get
        {
            if (ViewState["_VS_ValidationGroup"] != null)
            {
                return ViewState["_VS_ValidationGroup"].ToString();
            }

            return string.Empty;
        }

        set
        {
            ViewState["_VS_ValidationGroup"] = value;
        }
    }

    /// <summary>
    /// Propriedade que guarda o tipo de contato, usada para controlar a visualização do botão adicionar.
    /// </summary>
    private string tmc_id
    {
        get;
        set;
    }

    #endregion

    #region Métodos

    /// <summary>
    /// Método que prepara os dados para a gravação no banco.
    /// </summary>
    /// <param name="msgErro">Mensagm de erro.</param>
    /// <returns>Verdadeiro se está para ser gravado.</returns>
    public bool SalvaConteudo(out string msgErro)
    {
        string erro = string.Empty;
        bool sucesso = true;

        DataTable dt = CarregarDataTableComContatos(false);

        foreach (DataRow dr in dt.Rows)
        {
            string mensagemErro;

            if (!ValidarLinha(dr, out mensagemErro))
            {
                erro += mensagemErro;
                sucesso = false;
            }
        }

        msgErro = erro;
        return sucesso;
    }

    /// <summary>
    /// Carrega os dados de contato da escola do banco de dados para a tela.
    /// </summary>
    /// <param name="esc_id">Id da escola.</param>
    /// <param name="uni_id">Id da unidade.</param>
    public void CarregarContatosDeEscolaDoBanco(int esc_id, int uni_id)
    {
        try
        {
            DataTable dt = ESC_UnidadeEscolaContatoBO.SelecionaContatosDaEscola(esc_id, uni_id);
            rptTipoContato.DataSource = dt;
            rptTipoContato.DataBind();

            ConfigurarValidacao();
        }
        catch (Exception ex)
        {
            lblMensagemErroContato.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Alerta);
            ApplicationWEB._GravaErro(ex);
        }
    }

    /// <summary>
    /// Carrega os dados de contato do banco de dados para a tela.
    /// </summary>
    /// <param name="pes_id">Id da pessoa.</param>
    public void CarregarContatosDoBanco(Guid pes_id)
    {
        try
        {
            DataTable dt = SYS_TipoMeioContatoBO.SelecionaContatosDaPessoa(pes_id);
            rptTipoContato.DataSource = dt;
            rptTipoContato.DataBind();

            ConfigurarValidacao();
        }
        catch (Exception ex)
        {
            lblMensagemErroContato.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Alerta);
            ApplicationWEB._GravaErro(ex);
        }
    }

    /// <summary>
    /// Carrega os dados de contato da pessoa do banco para a tela, com exceção do informado.
    /// </summary>
    /// <param name="pes_id">Id da pessoa.</param>
    /// <param name="tmcExceto">Tipo meio contato.</param>
    public void CarregarContatosDoBancoExceto(Guid pes_id, Guid tmcExceto)
    {
        try
        {
            DataTable dt = SYS_TipoMeioContatoBO.SelecionaContatosDaPessoa(pes_id);

            var x = dt.AsEnumerable().Where(r => r.Field<Guid>("tmc_id") != tmcExceto);

            rptTipoContato.DataSource = x.CopyToDataTable();
            rptTipoContato.DataBind();

            ConfigurarValidacao();
        }
        catch (Exception ex)
        {
            lblMensagemErroContato.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Alerta);
            ApplicationWEB._GravaErro(ex);
        }
    }

    /// <summary>
    /// Carrega um DataTable com os dados de contato na tela.
    /// </summary>
    /// <param name="carregarTodos">Se verdadeiro carrega mesmo os contatos em branco.</param>
    /// <returns>DataTable com os dados de contato.</returns>
    private DataTable CarregarDataTableComContatos(bool carregarTodos)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("tmc_id");
        dt.Columns.Add("tmc_validacao");
        dt.Columns.Add("tmc_nome");
        dt.Columns.Add("contato");
        dt.Columns.Add("id");
        dt.Columns.Add("banco");

        foreach (RepeaterItem ri in rptTipoContato.Items)
        {
            DataRow dr = dt.NewRow();
            dr["tmc_id"] = ((HiddenField)ri.FindControl("lbl_tmc_id")).Value;
            dr["tmc_validacao"] = ((HiddenField)ri.FindControl("lbl_tmc_validacao")).Value;
            dr["tmc_nome"] = ((Label)ri.FindControl("lbl_tmc_nome")).Text;
            dr["contato"] = ((TextBox)ri.FindControl("txt_psc_contato")).Text;

            if (String.IsNullOrEmpty(((HiddenField)ri.FindControl("lbl_psc_id")).Value))
            {
                if (_VS_GuidSeq)
                {
                    dr["id"] = Guid.NewGuid();
                }
                else
                {
                    if (_VS_seq == -1)
                    {
                        _VS_seq = 1;
                    }
                    else
                    {
                        _VS_seq = _VS_seq + 1;
                    }

                    dr["id"] = _VS_seq;
                }
            }
            else
            {
                dr["id"] = ((HiddenField)ri.FindControl("lbl_psc_id")).Value;
            }

            dr["banco"] = ((HiddenField)ri.FindControl("lblBanco")).Value;
            dt.Rows.Add(dr);

            if (!carregarTodos && (((HiddenField)ri.FindControl("lblBanco")).Value == "true"))
            {
                dr.AcceptChanges();
                dr.SetModified();
            }

            if (!carregarTodos && String.IsNullOrEmpty(dr["contato"].ToString()))
            {
                dr.Delete();
            }
        }

        return dt;
    }

    /// <summary>
    /// Configura a validação dos campos.
    /// </summary>
    private void ConfigurarValidacao()
    {
        foreach (RepeaterItem ri in rptTipoContato.Items)
        {
            byte tmc_validacao;

            if (Byte.TryParse(((HiddenField)ri.FindControl("lbl_tmc_validacao")).Value, out tmc_validacao))
            {
                if (tmc_validacao == (byte) SYS_TipoMeioContatoValidacao.Email)
                {
                    Regex regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.None);
                    ((RegularExpressionValidator) ri.FindControl("revGenerico")).ValidationExpression = regex.ToString();
                    ((RegularExpressionValidator) ri.FindControl("revGenerico")).ErrorMessage =
                        ((Label) ri.FindControl("lbl_tmc_nome")).Text + " está fora do padrão ( seuEmail@seuProvedor ).";
                    ri.FindControl("revGenerico").Visible = true;
                }
                else if (tmc_validacao == (byte) SYS_TipoMeioContatoValidacao.Telefone)
                {
                    Regex regex = new Regex(@"^(\(\d{2}\))?[\s]?\d{3,5}-?\d{4}$", RegexOptions.None);
                    ((RegularExpressionValidator) ri.FindControl("revGenerico")).ValidationExpression = regex.ToString();
                    ((RegularExpressionValidator) ri.FindControl("revGenerico")).ErrorMessage =
                        ((Label) ri.FindControl("lbl_tmc_nome")).Text +
                        " está fora do padrão ( (XX) XXX-XXXX ou (XX) XXXX-XXXX ou (XX) XXXXX-XXXX ou (XX) XXXXXXX ou (XX) XXXXXXXX ou (XX) XXXXXXXXX ou XXXX-XXXX ou XXXXX-XXXX ou XXXXXXXX ou XXXXXXXXX).";
                    ri.FindControl("revGenerico").Visible = true;
                }
                else if (tmc_validacao == (byte) SYS_TipoMeioContatoValidacao.WebSite)
                {
                    Regex regex = new Regex(@"^(http(s)?://)?([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$", RegexOptions.None);
                    ((RegularExpressionValidator) ri.FindControl("revGenerico")).ValidationExpression = regex.ToString();
                    ((RegularExpressionValidator) ri.FindControl("revGenerico")).ErrorMessage =
                        ((Label) ri.FindControl("lbl_tmc_nome")).Text +
                        " está fora do padrão ( http(s)://seuSite.dominio ou http(s)://www.seuSite.dominio ).";
                    ri.FindControl("revGenerico").Visible = true;
                }
            }
        }
    }

    /// <summary>
    /// Adiciona uma nova linha na posição e com o tipo contato.
    /// </summary>
    /// <param name="index">Posição a ser adicionada.</param>
    private void AdicionarLinha(int index)
    {
        DataTable dt = CarregarDataTableComContatos(true);
        DataRow dr = dt.NewRow();

        RepeaterItem ri = rptTipoContato.Items[index];

        dr["tmc_id"] = ((HiddenField)ri.FindControl("lbl_tmc_id")).Value;
        dr["tmc_validacao"] = ((HiddenField)ri.FindControl("lbl_tmc_validacao")).Value;
        dr["tmc_nome"] = ((Label)ri.FindControl("lbl_tmc_nome")).Text;
        dr["contato"] = string.Empty;
        dr["id"] = string.Empty;
        dr["banco"] = false;

        dt.Rows.InsertAt(dr, index + 1);

        rptTipoContato.DataSource = dt;
        rptTipoContato.DataBind();
    }

    /// <summary>
    /// Limpa o campo texto de contato da linha.
    /// </summary>
    /// <param name="index">Linha a ser limpa.</param>
    private void LimparLinha(int index)
    {
        RepeaterItem ri = rptTipoContato.Items[index];
        ((TextBox)ri.FindControl("txt_psc_contato")).Text = string.Empty;
    }

    /// <summary>
    /// Método para validar os campos a serem inseridos.
    /// </summary>
    /// <param name="dr">Linha a ser validada.</param>
    /// <param name="mensagemErro">Mensagem de erro.</param>
    /// <returns>Retorna verdadeiro se foi validado corrretamente.</returns>
    private bool ValidarLinha(DataRow dr, out string mensagemErro)
    {
        try
        {
            string erro = string.Empty;
            bool retorno = true;

            if (dr.RowState != DataRowState.Deleted)
            {
                byte tmc_validacao;

                if (Byte.TryParse(dr["tmc_validacao"].ToString(), out tmc_validacao))
                {
                    if (tmc_validacao == (byte) SYS_TipoMeioContatoValidacao.Email)
                    {
                        Regex regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.None);
                        if (!regex.IsMatch(dr["contato"].ToString()))
                        {
                            erro = dr["tmc_nome"] + " está fora do padrão ( seuEmail@seuProvedor ).";
                            retorno = false;
                        }
                    }
                    else if (tmc_validacao == (byte) SYS_TipoMeioContatoValidacao.Telefone)
                    {
                        Regex regex = new Regex(@"^(\(\d{2}\))?[\s]?\d{3,5}-?\d{4}$", RegexOptions.None);
                        if (!regex.IsMatch(dr["contato"].ToString()))
                        {
                            erro = dr["tmc_nome"] +
                                   " está fora do padrão ( (XX) XXX-XXXX ou (XX) XXXX-XXXX ou (XX) XXXXX-XXXX ou (XX) XXXXXXX ou (XX) XXXXXXXX ou (XX) XXXXXXXXX ou XXXX-XXXX ou XXXXX-XXXX ou XXXXXXXX ou XXXXXXXXX ).";
                            retorno = false;
                        }
                    }
                    else if (tmc_validacao == (byte) SYS_TipoMeioContatoValidacao.WebSite)
                    {
                        Regex regex = new Regex(@"^(http(s)?://)?([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$", RegexOptions.None);
                        if (!regex.IsMatch(dr["contato"].ToString()))
                        {
                            erro = dr["tmc_nome"] +
                                   " está fora do padrão ( http(s)://seuSite.dominio ou http(s)://www.seuSite.dominio ).";
                            retorno = false;
                        }
                    }
                }
            }

            mensagemErro = erro + "<br />";
            lblMensagemErroContato.Visible = !retorno;
            return retorno;
        }
        catch (Exception ex)
        {
            mensagemErro = "Não foi possível verificar o(s) contato(s).";
            ApplicationWEB._GravaErro(ex);
            return false;
        }
    }

    public void BloquearEdicao()
    {
        foreach (RepeaterItem ri in rptTipoContato.Items)
        {
            TextBox txt_psc_contato = ((TextBox)ri.FindControl("txt_psc_contato"));
            if (txt_psc_contato != null)
            {
                txt_psc_contato.Enabled = false;
            }

            ImageButton btnLimpar = ((ImageButton)ri.FindControl("btnLimpar"));
            if (btnLimpar != null)
            {
                btnLimpar.Visible = false;
            }

            ImageButton btnAdicionar = ((ImageButton)ri.FindControl("btnAdicionar"));
            if (btnAdicionar != null)
            {
                btnAdicionar.Visible = false;
            }
        }
    }

    #endregion

    #region Eventos

    protected void rptTipoContato_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer && e.Item.ItemType != ListItemType.Pager)
        {
            // Controla a visualização do botão adicionar, não aparece se houver outros contatos do mesmo tipo.
            if (DataBinder.Eval(e.Item.DataItem, "tmc_id").ToString() == tmc_id)
            {
                rptTipoContato.Items[e.Item.ItemIndex - 1].FindControl("btnAdicionar").Visible = false;
            }

            tmc_id = DataBinder.Eval(e.Item.DataItem, "tmc_id").ToString();

            ImageButton btnLimpar = (ImageButton)e.Item.FindControl("btnLimpar");
            if (btnLimpar != null)
            {
                btnLimpar.CommandArgument = e.Item.ItemIndex.ToString();
            }

            ImageButton btnAdicionar = (ImageButton)e.Item.FindControl("btnAdicionar");
            if (btnAdicionar != null)
            {
                btnAdicionar.CommandArgument = e.Item.ItemIndex.ToString();
            }
        }
    }

    protected void rptTipoContato_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Limpar")
        {
            int index = int.Parse(e.CommandArgument.ToString());
            LimparLinha(index);
        }

        if (e.CommandName == "Adicionar")
        {
            int index = int.Parse(e.CommandArgument.ToString());
            AdicionarLinha(index);
        }
    }

    #endregion
}
