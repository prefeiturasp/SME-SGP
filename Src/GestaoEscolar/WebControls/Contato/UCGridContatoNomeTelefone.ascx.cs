using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using System.Data;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;

public partial class WebControls_Contato_UCGridContatoNomeTelefone : MotherUserControl
{
    #region CONSTANTES

    private const int IndiceColunaTipoResponsavel = 1;
    private const int IndiceColunaExcluir = 5;

    #endregion

    #region PROPRIEDADES

    private DataTable dtTiposResponsaveis;
    /// <summary>
    /// ViewState contendo os tipos de responsavel cadastrados
    /// </summary>
    private DataTable _VS_Tipo_Responsaveis
    {
        get
        {
            if (dtTiposResponsaveis == null)
            {
                dtTiposResponsaveis = ACA_TipoResponsavelAlunoBO.SelecionaTipoResponsavelAluno(false, false);                
            }

            return dtTiposResponsaveis;
        }
    }

    /// <summary>
    /// ViewState com datatable de contatos
    /// Retorno e atribui valores para o DataTable de contatos
    /// </summary>
    public DataTable _VS_contatos
    {
        get 
        {
            if (ViewState["_VS_contatos"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("alu_id");
                dt.Columns.Add("fmc_id");
                dt.Columns.Add("fmc_nome");
                dt.Columns.Add("fmc_telefone");
                dt.Columns.Add("tra_id");
                dt.Columns.Add("fmc_ordem");
                ViewState["_VS_contatos"] = dt;
            }
            return (DataTable)ViewState["_VS_contatos"];
        }
        set
        {
            ViewState["_VS_contatos"] = value;
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
                return Convert.ToInt32(ViewState["_VS_seq"]);
            return -1;
        }
        set
        {
            ViewState["_VS_seq"] = value;
        }
    }

    /// <summary>
    /// Armazena a ordem para inclusão no DataTable quando for do tipo int
    /// </summary>
    public int _VS_ordem
    {
        get
        {
            if (ViewState["_VS_ordem"] == null)
                ViewState["_VS_ordem"] = 1;
            return Convert.ToInt32(ViewState["_VS_ordem"]);
        }
        set
        {
            ViewState["_VS_ordem"] = value;
        }
    }

    /// <summary>
    /// Retorna e atribui valores para o grid.
    /// </summary>
    public GridView _grvContatoNomeTel
    {
        get
        {
            return _grvContatoNomeTelefone;
        }
        set
        {
            _grvContatoNomeTelefone = value;
        }
    }

    /// <summary>
    /// Retorna e atribui valores para o update panel.
    /// </summary>
    public UpdatePanel _updGridContatoNomeTel
    {
        get
        {
            return updGridContatoNomeTelefone;
        }
        set
        {
            updGridContatoNomeTelefone = value;
        }
    }

    /// <summary>
    /// Retorna e atribui valores para o label de mensagens de erro
    /// </summary>
    public Label _MensagemErro
    {
        get { return _lblMessage; }
        set { _lblMessage = value; }
    }

    /// <summary>
    /// Indica a visibilidade do usercontrol de tipo de responsaveis pelo aluno
    /// </summary>
    public bool VS_Mostra_Tipo_Responsaveis
    {
        get
        {
            if (ViewState["VS_Mostra_Tipo_Responsaveis"] != null)
                return Convert.ToBoolean(ViewState["VS_Mostra_Tipo_Responsaveis"]);
            return false;
        }
        set
        {
            ViewState["VS_Mostra_Tipo_Responsaveis"] = value;
        }
    }    
    
    #endregion

    #region METODOS

    /// <summary>
    /// Método para carregar dados no grid
    /// </summary>
    public void _CarregarContato()
    {
        _grvContatoNomeTel.DataSource = _VS_contatos;
        _grvContatoNomeTel.DataBind();

        if (_grvContatoNomeTel.Rows.Count > 0)
            _grvContatoNomeTel.Rows[_grvContatoNomeTel.Rows.Count - 1].FindControl("btnAdicionar").Visible = true;

        _updGridContatoNomeTel.Update();

        if (_VS_contatos.Rows.Count > 0)
            _VS_ordem = Convert.ToInt32(_VS_contatos.Rows[_VS_contatos.Rows.Count - 1]["fmc_ordem"]);  
    }

    /// <summary>
    /// Adiciona uma nova linha ao grid
    /// </summary>
    private void AdicionaLinhaGrid()
    {
        // adiciona nova linha do grid
        DataRow dr = _VS_contatos.NewRow();

        if (_VS_seq == -1)
            _VS_seq = 1;
        else
            _VS_seq = _VS_seq + 1;

        _VS_ordem = _VS_ordem + 1;

        dr["fmc_id"] = _VS_seq;
        dr["fmc_ordem"] = _VS_ordem;

        _VS_contatos.Rows.Add(dr);        

        // mostra nova linha
        _CarregarContato();        
    }

    /// <summary>
    /// Verifica se a linha cujo indíce é passado por parâmetro está preenchida
    /// </summary>
    /// <param name="index">indíce da linha a ser verificada</param>
    /// <returns></returns>
    private bool VerificaLinhaPreenchida(int index)
    {
        GridViewRow linha = _grvContatoNomeTel.Rows[index];

        if (!String.IsNullOrEmpty(((TextBox)linha.FindControl("txtContatoNome")).Text))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Salva o conteúdo do grid no ViewState
    /// </summary>
    public bool SalvaConteudoGrid(out string msgErro)
    {
        string mensagemErro = "";

        foreach (GridViewRow linha in _grvContatoNomeTel.Rows)
        {
            string id = _grvContatoNomeTel.DataKeys[linha.RowIndex].Value.ToString();
            int index = RetornaIndice(id);

            if (VerificaLinhaPreenchida(linha.RowIndex))
            {
                _VS_contatos.Rows[index]["fmc_nome"] = ((TextBox)linha.FindControl("txtContatoNome")).Text;
                _VS_contatos.Rows[index]["fmc_telefone"] = ((TextBox)linha.FindControl("txtContatoTelefone")).Text;
                if (VS_Mostra_Tipo_Responsaveis)
                {
                    WebControls_Combos_UCComboTipoResponsavelAluno UCComboTipoResponsavelAluno1 =
                        ((WebControls_Combos_UCComboTipoResponsavelAluno)linha.FindControl("UCComboTipoResponsavelAluno1"));

                    _VS_contatos.Rows[index]["tra_id"] = UCComboTipoResponsavelAluno1.Valor;
                }
            }
            else
            {
                if (index != -1)
                    _VS_contatos.Rows[index].Delete();
            }
        }

        msgErro = mensagemErro;
        return true;
    }

    /// <summary>
    /// Retorna o indice na tabela do viewstate pelo id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private int RetornaIndice(string id)
    {
        int indice = -1;

        var x = from DataRow dr in _VS_contatos.Rows
                where
                    (dr.RowState != DataRowState.Deleted) &&
                    (dr["fmc_id"].ToString().Equals(id, StringComparison.OrdinalIgnoreCase))

                select _VS_contatos.Rows.IndexOf(dr);

        if (x.Count() > 0)
        {
            indice = x.First();
        }

        return indice;
    }

    private void OrdenaTabela(int idDescer, int idSubir, int fmc_ordemSubir, int fmc_ordemDescer)
    {
        foreach (DataRow row in _VS_contatos.Rows)
        {
            if (Convert.ToInt32(row["fmc_id"]) == idDescer)
                row["fmc_ordem"] = fmc_ordemSubir;
            else if (Convert.ToInt32(row["fmc_id"]) == idSubir)
                row["fmc_ordem"] = fmc_ordemDescer;
        }

        string msgErro;
        if (SalvaConteudoGrid(out msgErro))
        {
            _lblMessage.Visible = false; 
            
            _VS_contatos = _VS_contatos.AsEnumerable().OrderBy(r => Convert.ToInt32(r["fmc_ordem"])).CopyToDataTable();

            _grvContatoNomeTel.DataSource = _VS_contatos;
            _grvContatoNomeTel.DataBind();

            if (_grvContatoNomeTel.Rows.Count > 0)
                _grvContatoNomeTel.Rows[_grvContatoNomeTel.Rows.Count - 1].FindControl("btnAdicionar").Visible = true;

            _updGridContatoNomeTel.Update();
        }
        else
        {
            _lblMessage.Visible = true;
            _lblMessage.Text = UtilBO.GetErroMessage(msgErro, UtilBO.TipoMensagem.Alerta);
        }
    }

    #endregion

    #region EVENTOS

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Esconde ou mostra a coluna de responsaveis pelo aluno de acordo com o VS
            _grvContatoNomeTel.Columns[IndiceColunaTipoResponsavel].Visible = VS_Mostra_Tipo_Responsaveis;
            _grvContatoNomeTel.Columns[IndiceColunaExcluir].Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;
        }

        // Verificar se o DataTable está vazio.
        var x = from DataRow dr in _VS_contatos.Rows
                where dr.RowState != DataRowState.Deleted
                select dr;

        if (x.Count() == 0)
        {            
            AdicionaLinhaGrid();
        }
    }

    protected void btnAdicionar_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow linha = (GridViewRow)((ImageButton)sender).NamingContainer;

        if (VerificaLinhaPreenchida(linha.RowIndex))
        {
            string msgErro;
            if (SalvaConteudoGrid(out msgErro))
            {
                _lblMessage.Visible = false;
                AdicionaLinhaGrid();
            }
            else
            {
                _lblMessage.Visible = true;
                _lblMessage.Text = UtilBO.GetErroMessage(msgErro, UtilBO.TipoMensagem.Alerta);
            }
        }
    }

    protected void btnDescer_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow linha = (GridViewRow)((ImageButton)sender).NamingContainer;
        if (VerificaLinhaPreenchida(linha.RowIndex) &&
            _grvContatoNomeTel.Rows.Count > linha.RowIndex + 1)
        {
            int idDescer = Convert.ToInt32(_grvContatoNomeTel.DataKeys[linha.RowIndex].Value.ToString());
            int idSubir = Convert.ToInt32(_grvContatoNomeTel.DataKeys[linha.RowIndex + 1].Value.ToString());

            int fmc_ordemDescer = Convert.ToInt32(_VS_contatos.AsEnumerable().Where(r => Convert.ToInt32(r["fmc_id"]) == idDescer).FirstOrDefault()["fmc_ordem"]);
            int fmc_ordemSubir = Convert.ToInt32(_VS_contatos.AsEnumerable().Where(r => Convert.ToInt32(r["fmc_id"]) == idSubir).FirstOrDefault()["fmc_ordem"]);

            OrdenaTabela(idDescer, idSubir, fmc_ordemSubir, fmc_ordemDescer);
        }
    }

    protected void btnSubir_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow linha = (GridViewRow)((ImageButton)sender).NamingContainer;
        if (VerificaLinhaPreenchida(linha.RowIndex) &&
            linha.RowIndex > 0)
        {
            int idSubir = Convert.ToInt32(_grvContatoNomeTel.DataKeys[linha.RowIndex].Value.ToString());
            int idDescer = Convert.ToInt32(_grvContatoNomeTel.DataKeys[linha.RowIndex - 1].Value.ToString());

            int fmc_ordemDescer = Convert.ToInt32(_VS_contatos.AsEnumerable().Where(r => Convert.ToInt32(r["fmc_id"]) == idDescer).FirstOrDefault()["fmc_ordem"]);
            int fmc_ordemSubir = Convert.ToInt32(_VS_contatos.AsEnumerable().Where(r => Convert.ToInt32(r["fmc_id"]) == idSubir).FirstOrDefault()["fmc_ordem"]);

            OrdenaTabela(idDescer, idSubir, fmc_ordemSubir, fmc_ordemDescer);
        }
    }

    protected void _grvContatoNomeTelefone_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton btnExcluir = (ImageButton)e.Row.FindControl("btnExcluir");
            if (btnExcluir != null)
            {
                btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
                btnExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;
            }
            ImageButton btnSubir = (ImageButton)e.Row.FindControl("btnSubir");
            if (btnSubir != null)
            {
                btnSubir.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "cima.png";
                btnSubir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar &&
                                   e.Row.RowIndex > 0;
            }

            ImageButton btnDescer = (ImageButton)e.Row.FindControl("btnDescer");
            if (btnDescer != null)
            {
                btnDescer.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "baixo.png";
                btnDescer.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar &&
                                    e.Row.RowIndex < _VS_contatos.AsEnumerable().Select(r => r.RowState != DataRowState.Deleted).Count() - 1;
            }

            //Se o usercontrol de tipo de responsaveis estiver visivel, o grid de contatos é percorrido e o combo de tipo
            //de responsaveis é preenchido e tem o valor setado para a linha selecionada.
            if (VS_Mostra_Tipo_Responsaveis)
            {
                string id = _grvContatoNomeTel.DataKeys[e.Row.RowIndex].Value.ToString();
                int index = RetornaIndice(id);

                WebControls_Combos_UCComboTipoResponsavelAluno UCComboTipoResponsavelAluno1 =
                    ((WebControls_Combos_UCComboTipoResponsavelAluno)e.Row.FindControl("UCComboTipoResponsavelAluno1"));

                UCComboTipoResponsavelAluno1.Combo.DataSource = _VS_Tipo_Responsaveis;
                UCComboTipoResponsavelAluno1.DataBind();

                int tra_id = -1;

                if (!string.IsNullOrEmpty(_VS_contatos.Rows[index]["tra_id"].ToString()))
                    tra_id = Convert.ToInt32(_VS_contatos.Rows[index]["tra_id"].ToString());

                UCComboTipoResponsavelAluno1.Valor = tra_id;
            }
        }
    }

    protected void _grvContatoNomeTelefone_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Excluir"))
        {
            int indexComando = Convert.ToInt32(e.CommandArgument);
            string id = _grvContatoNomeTel.DataKeys[indexComando].Value.ToString();
            int indexTabela = RetornaIndice(id);

            if (indexTabela >= 0 && indexTabela < _VS_contatos.Rows.Count)
            {
                _VS_contatos.Rows[indexTabela].Delete();

                if (_VS_contatos.Rows.Count == 0)
                    AdicionaLinhaGrid();
                
                _grvContatoNomeTel.DataSource = _VS_contatos;
                _grvContatoNomeTel.DataBind();

                if (_grvContatoNomeTel.Rows.Count > 0)
                    _grvContatoNomeTel.Rows[_grvContatoNomeTel.Rows.Count - 1].FindControl("btnAdicionar").Visible = true;

                _updGridContatoNomeTel.Update();
            }
        }
    }

    #endregion
}
