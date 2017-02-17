using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebControls_PessoaDocumento_UCGridPessoaDocumento : MotherUserControl
{
    #region DELEGATES

    public delegate void TextChanged();
    public event TextChanged _TextChanged;

    #endregion

    #region PROPRIEDADES

    /// <summary>
    /// Indica se o documento é referente a um aluno.
    /// </summary>
    private bool isAluno;

    /// <summary>
    /// Indica se o documento é referente a um aluno.
    /// </summary>
    public bool _isAluno
    {
        get
        {
            return isAluno;
        }
        set
        {
            isAluno = value;
        }
    }

    /// <summary>
    /// Retorna e atribui valores para o update panel.
    /// </summary>
    public UpdatePanel _updGridDocumentos
    {
        get
        {
            return updGridDocumentos;
        }
        set
        {
            updGridDocumentos = value;
        }
    }

    /// <summary>
    /// Armazena o id da pessoa.
    /// </summary>
    public Guid _VS_pes_id
    {
        get
        {
            if (ViewState["_VS_pes_id"] != null)
                return new Guid(ViewState["_VS_pes_id"].ToString());
            return Guid.Empty;
        }
        set
        {
            ViewState["_VS_pes_id"] = value;
        }
    }

    /// <summary>
    /// Armazena os tipos de documentos.
    /// </summary>
    public DataTable _VS_tipos_documentos
    {
        get
        {
            if (ViewState["_VS_tipos_documentos"] == null)
            {
                DataTable dtDocumento = new DataTable();

                dtDocumento.Columns.Add("tdo_id");
                dtDocumento.Columns.Add("tdo_nome");
                dtDocumento.Columns.Add("tdo_sigla");
                dtDocumento.Columns.Add("tdo_validacao");
                dtDocumento.Columns.Add("tdo_situacao");
                dtDocumento.Columns.Add("tdo_integridade");
                dtDocumento.Columns.Add("tdo_classificacao");
                dtDocumento.Columns.Add("tdo_atributos");
            }
            return (DataTable)ViewState["_VS_tipos_documentos"];
        }
        set
        {
            ViewState["_VS_tipos_documentos"] = value;
        }
    }

    public string _VS_tipos_documentos_atributos_default
    {
        get
        {
            return (string)ViewState["_VS_tipos_documentos_atributos_default"];
        }
        set
        {
            ViewState["_VS_tipos_documentos_atributos_default"] = value;
        }
    }

    /// <summary>
    /// Retorna e atribui valores para o label de mensagens de erro.
    /// </summary>
    public Label _MensagemErro
    {
        get { return _lblMessage; }
        set { _lblMessage = value; }
    }

    /// <summary>
    /// ViewState com datatable de documentos
    /// Retorno e atribui valores para o DataTable de documentos
    /// </summary>
    public DataTable _VS_documentos
    {
        get
        {
            if (ViewState["_VS_documentos"] == null)
            {
                DataTable dtDocumento = new DataTable();

                dtDocumento.Columns.Add("tdo_id");
                dtDocumento.Columns.Add("unf_idEmissao");
                dtDocumento.Columns.Add("unf_idAntigo");
                dtDocumento.Columns.Add("tdo_nome");
                dtDocumento.Columns.Add("numero");
                dtDocumento.Columns.Add("dataemissao");
                dtDocumento.Columns.Add("orgaoemissao");
                dtDocumento.Columns.Add("unf_nome");
                dtDocumento.Columns.Add("pai_idOrigem");
                dtDocumento.Columns.Add("pai_idAntigo");
                dtDocumento.Columns.Add("pai_nome");
                dtDocumento.Columns.Add("info");
                dtDocumento.Columns.Add("categoria");
                dtDocumento.Columns.Add("classificacao");
                dtDocumento.Columns.Add("csm");
                dtDocumento.Columns.Add("dataEntrada");
                dtDocumento.Columns.Add("dataValidade");
                dtDocumento.Columns.Add("serie");
                dtDocumento.Columns.Add("tipoGuarda");
                dtDocumento.Columns.Add("via");
                dtDocumento.Columns.Add("secao");
                dtDocumento.Columns.Add("zona");
                dtDocumento.Columns.Add("regiaoMilitar");
                dtDocumento.Columns.Add("numeroRA");
                dtDocumento.Columns.Add("dataexpedicao");
                dtDocumento.Columns.Add("tdo_atributos");
                dtDocumento.Columns.Add("banco", typeof(Boolean));
                dtDocumento.Columns.Add("excluido", typeof(Boolean));

                ViewState["_VS_documentos"] = dtDocumento;
            }
            return (DataTable)ViewState["_VS_documentos"];
        }
        set
        {
            ViewState["_VS_documentos"] = value;
        }
    }

    /// <summary>
    /// Armazena o cpf da pessoa
    /// </summary>
    public string VS_CPF
    {
        get
        {
            if (ViewState["VS_CPF"] != null)
                return ViewState["VS_CPF"].ToString();
            return string.Empty;
        }
        set
        {
            ViewState["VS_CPF"] = value;
        }
    }

    #endregion

    #region METODOS

    /// <summary>
    /// Retorna os dados dos documentos a serem salvos e seta as rowstates.
    /// </summary>
    /// <returns> Data table com os dados dos documentos a serem salvos.</returns>
    public DataTable RetornaDocumentoSave()
    {
        DataTable dtDocumento = RetornaDocumento();

        for (int index = 0; index < dtDocumento.Rows.Count; index++)
        {
            if (Convert.ToBoolean(dtDocumento.Rows[index]["excluido"]))
            {
                // Se não for do banco e estiver excluído, remove o registro do dtDocumento.
                if (!(Convert.ToBoolean(dtDocumento.Rows[index]["banco"])))
                {
                    dtDocumento.Rows.Remove(dtDocumento.Rows[index]);
                    // Diminui o índice pois foi removida uma linha.
                    index--;
                }
                else
                {
                    // Força o rowstate a ficar como deleted.
                    dtDocumento.Rows[index].AcceptChanges();
                    dtDocumento.Rows[index].Delete();
                }
            }
            else if (Convert.ToBoolean(dtDocumento.Rows[index]["banco"]))
            {
                // Força o rowstate a ficar como modified.
                dtDocumento.Rows[index].AcceptChanges();
                dtDocumento.Rows[index].SetModified();
            }
        }

        return dtDocumento;
    }

    /// <summary>
    /// Cria o dtDocumento com suas colunas.
    /// </summary>
    /// <returns>DataTable documento configurado.</returns>
    private DataTable CriaDataTableDocumento()
    {
        DataTable dtDocumento = new DataTable();

        dtDocumento.Columns.Add("tdo_id");
        dtDocumento.Columns.Add("unf_idEmissao");
        dtDocumento.Columns.Add("unf_idAntigo");
        dtDocumento.Columns.Add("tdo_nome");
        dtDocumento.Columns.Add("numero");
        dtDocumento.Columns.Add("dataemissao");
        dtDocumento.Columns.Add("orgaoemissao");
        dtDocumento.Columns.Add("unf_nome");
        dtDocumento.Columns.Add("pai_idOrigem");
        dtDocumento.Columns.Add("pai_idAntigo");
        dtDocumento.Columns.Add("pai_nome");
        dtDocumento.Columns.Add("info");
        dtDocumento.Columns.Add("categoria");
        dtDocumento.Columns.Add("classificacao");
        dtDocumento.Columns.Add("csm");
        dtDocumento.Columns.Add("dataEntrada");
        dtDocumento.Columns.Add("dataValidade");
        dtDocumento.Columns.Add("serie");
        dtDocumento.Columns.Add("tipoGuarda");
        dtDocumento.Columns.Add("via");
        dtDocumento.Columns.Add("secao");
        dtDocumento.Columns.Add("zona");
        dtDocumento.Columns.Add("regiaoMilitar");
        dtDocumento.Columns.Add("numeroRA");
        dtDocumento.Columns.Add("dataexpedicao");
        dtDocumento.Columns.Add("tdo_atributos");
        dtDocumento.Columns.Add("banco", typeof(Boolean));
        dtDocumento.Columns.Add("excluido", typeof(Boolean));

        return dtDocumento;
    }

    /// <summary>
    /// Método para validar os campos a serem salvos.
    /// </summary>    
    /// <param name="row">Linha a ser validada.</param>
    /// <param name="msgErro">Mensagens de erros.</param>
    /// <returns></returns>
    private bool ValidarLinhaGrid(RepeaterItem row, out string msgErro)
    {
        try
        {
            string msg = "";
            Guid tdo_id = new Guid(((DropDownList)row.FindControl("ddlTipoDoc")).SelectedValue);

            //Valida o tipo de documento.
            if (tdo_id != Guid.Empty)
            {
                SYS_TipoDocumentacao tdo = new SYS_TipoDocumentacao { tdo_id = tdo_id };

                SYS_TipoDocumentacaoBO.GetEntity(tdo);

                if (((TextBox)row.FindControl("tbNumDoc")).Visible == true
                    && string.IsNullOrEmpty(((TextBox)row.FindControl("tbNumDoc")).Text))
                {
                    //Valida o número do documento.
                    msg += "Número é obrigatório.</br>";
                }
                else
                {
                    if (tdo.tdo_validacao == 1)
                    {
                        if (!UtilBO._ValidaCPF(((TextBox)row.FindControl("tbNumDoc")).Text))
                            msg += "Número inválido para CPF.</br>";
                    }

                    if (tdo.tdo_validacao == 2)
                    {
                        Regex regex = new Regex("^[0-9]+$");
                        if (!regex.IsMatch(((TextBox)row.FindControl("tbNumDoc")).Text))
                            msg += "Número inválido para este CPF.</br>";
                    }

                    string docPadraoCPF = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_CPF);
                    if (((DropDownList)row.FindControl("ddlTipoDoc")).SelectedValue == docPadraoCPF)
                    {
                        if (PES_PessoaDocumentoBO.VerificaDocumentoExistente(((TextBox)row.FindControl("tbNumDoc")).Text, _VS_pes_id))
                            msg += "Já existe uma pessoa cadastrada com esse documento. (" + ((DropDownList)row.FindControl("ddlTipoDoc")).SelectedItem + ")</br>";
                    }
                }
            }
            else
                msg += "Tipo de documento é obrigatório.</br>";

            // Valida a data de emissão.
            if (!string.IsNullOrEmpty(((TextBox)row.FindControl("tbDtEmissao")).Text.Trim()))
            {
                DateTime dt;
                if (!DateTime.TryParse(((TextBox)row.FindControl("tbDtEmissao")).Text, out dt))
                    msg += "Data de emissão não está no formato dd/mm/aaaa ou é inexistente.</br>";
            }

            // Valida se já existe o documentos do mesmo tipo cadastrados.
            var x = from RepeaterItem gvr in rptDocumento.Items
                    where
                        (((DropDownList)gvr.FindControl("ddlTipoDoc")).SelectedValue.Equals(((DropDownList)row.FindControl("ddlTipoDoc")).SelectedValue, StringComparison.OrdinalIgnoreCase))
                        && !string.IsNullOrEmpty((((TextBox)gvr.FindControl("tbNumDoc")).Text))

                    select gvr;

            if (x.Count() > 1)
                msg += "Existem documentos cadastrados com mesmo tipo.";

            if (string.IsNullOrEmpty(msg))
            {
                _lblMessage.Visible = false;
                msgErro = "";
                return true;
            }
            else
            {
                _lblMessage.Visible = true;
                msgErro = msg;
                return false;
            }

        }
        catch (Exception ex)
        {
            msgErro = "Não foi possível verificar o(s) documento(s).";
            ApplicationWEB._GravaErro(ex);
            return false;
        }
    }

    /// <summary>
    /// Verifica se a linha cujo indice passado por parâmetro tem algum campo preenchido.
    /// </summary>
    /// <param name="index"> Indice da linha a ser verificada.</param>
    /// <returns> True = Tem algum campo preenchido / False = Não tem campo preenchido.</returns>
    private bool VerificaLinhaPreenchida(int index)
    {
        RepeaterItem linha = rptDocumento.Items[index];
        if (!string.IsNullOrEmpty(((TextBox)linha.FindControl("tbNumDoc")).Text)
            || !string.IsNullOrEmpty(((TextBox)(linha.FindControl("tbDtEmissao"))).Text)
            || !string.IsNullOrEmpty(((TextBox)(linha.FindControl("tbOrgEmissor"))).Text)
            || !string.IsNullOrEmpty(((DropDownList)linha.FindControl("ddlUF")).SelectedValue)
            || !string.IsNullOrEmpty(((TextBox)(linha.FindControl("tbInfCompl"))).Text)
            || !string.IsNullOrEmpty(((TextBox)(linha.FindControl("tbCategoria"))).Text)
            || !string.IsNullOrEmpty(((TextBox)(linha.FindControl("tbClassificacao"))).Text)
            || !string.IsNullOrEmpty(((TextBox)(linha.FindControl("tbCsm"))).Text)
            || !string.IsNullOrEmpty(((TextBox)(linha.FindControl("tbDataEntrada"))).Text)
            || !string.IsNullOrEmpty(((TextBox)(linha.FindControl("tbDataValidade"))).Text)
            || !string.IsNullOrEmpty(((DropDownList)linha.FindControl("ddlPais")).SelectedValue)
            || !string.IsNullOrEmpty(((TextBox)(linha.FindControl("tbSerie"))).Text)
            || !string.IsNullOrEmpty(((TextBox)(linha.FindControl("tbTipoGuarda"))).Text)
            || !string.IsNullOrEmpty(((TextBox)(linha.FindControl("tbVia"))).Text)
            || !string.IsNullOrEmpty(((TextBox)(linha.FindControl("tbSecao"))).Text)
            || !string.IsNullOrEmpty(((TextBox)(linha.FindControl("tbZona"))).Text)
            || !string.IsNullOrEmpty(((TextBox)(linha.FindControl("tbRegiaoMilitar"))).Text)
            || !string.IsNullOrEmpty(((TextBox)(linha.FindControl("tbNumeroRA"))).Text)
            || !string.IsNullOrEmpty(((TextBox)(linha.FindControl("tbDataExpedicao"))).Text)
            )
            return true;
        else
            return false;
    }

    /// <summary>
    /// Valida os dados do grid.
    /// </summary>
    public bool ValidaConteudoGrid(out string msgErro)
    {
        string mensagemErro = "";

        foreach (RepeaterItem linha in rptDocumento.Items)
        {
            //Caso algum campo esteja preenchido realiza a validação.
            if (VerificaLinhaPreenchida(linha.ItemIndex))
            {
                if (!ValidarLinhaGrid(linha, out mensagemErro))
                {
                    msgErro = mensagemErro;
                    return false;
                }
            }
        }

        msgErro = mensagemErro;
        return true;
    }

    /// <summary>
    /// Método para carregar os documentos da pessoa no grid.
    /// </summary>
    /// <param name="pes_id"> Id da pessoa.</param>
    public void _CarregarDocumento(Guid pes_id)
    {
        // Carrega dados dos documentos da pessoa.
        var pessoaDocumentos = PES_PessoaDocumentoBO.GetSelect(pes_id, false, 1, 1);

        if (pessoaDocumentos.Rows.Count > 0)
        {
            // Adiciona colunas banco e excluído.
            pessoaDocumentos.Columns.Add("banco", typeof(Boolean));
            pessoaDocumentos.Columns.Add("excluido", typeof(Boolean));

            // Atribui a cada registro que ele veio do banco.
            foreach (DataRow dr in pessoaDocumentos.Rows)
            {
                dr["banco"] = true;
                dr["excluido"] = false;
            }

            _VS_documentos = pessoaDocumentos;

            rptDocumento.DataSource = _VS_documentos;
            rptDocumento.DataBind();
        }
        else
        {
            AdicionarLinhaRepeater();
        }

        _VS_pes_id = pes_id;

        updGridDocumentos.Update();
    }

    /// <summary>
    /// Adiciona uma nova linha no repeater
    /// </summary>
    public void AdicionarLinhaRepeater()
    {
        var dt = RetornaDocumento();
        DataRow dr = dt.NewRow();

        dr["banco"] = false;
        dr["excluido"] = false;
        dt.Rows.Add(dr);

        // Carrega a nova linha no grid.
        rptDocumento.DataSource = dt;
        rptDocumento.DataBind();

        //atualiza grid
        updGridDocumentos.Update();
    }

    /// <summary>
    /// Verifica o tipo de documento selecionado
    /// e mostra somente os campos correspondentes a esse tipo de documento.
    /// </summary>
    /// <param name="e">Item do repeater</param>
    public void MostraCamposCorretosDeCadaDocumento(RepeaterItemEventArgs e)
    {
        var tiposDocumentos = _VS_tipos_documentos;

        //CASO A VIEWSTATE DE TIPOS DE DOCUMENTOS NÃO EXISTA, DÁ GET NO BANCO E SETA PARA A MESMA
        if (tiposDocumentos == null)
        {
            tiposDocumentos = SYS_TipoDocumentacaoBO.GetSelect(Guid.Empty, "", "", 0, false, 1, 1);

            _VS_tipos_documentos = tiposDocumentos;
        }

        //CAMPOS(DIVS) QUE DEVERÃO SER MOSTRADAS OU OCULTADAS DEPENDENDO DO TIPO DE DOCUMENTO
        var campos = new string[] { "Categoria", "Classificacao",  "Csm",  "DtEmissao", "DataEntrada"
                                        , "DataExpedicao" ,  "DataValidade" , "UF" ,"InfCompl" , "NumDoc" ,"OrgEmissor", "Pais"
                                        , "NumeroRA", "RegiaoMilitar" , "Secao",  "Serie", "TipoGuarda", "Via"
                                        , "Zona"
                                    };

        //PEGA OS ATRIBUTOS DE CADA DOCUMENTO
        var atributos = (from DataRow dr in tiposDocumentos.Rows
                         where (Guid)dr["tdo_id"] == new Guid(((DropDownList)e.Item.FindControl("ddlTipoDoc")).SelectedValue)
                         select (dr["tdo_atributos"] != DBNull.Value ? (string)dr["tdo_atributos"] : null)).FirstOrDefault();

        //CASO O CAMPO ATRIBUTOS ESTEJA NULO, DA GET NO BANCO NOS ATRIBUTOS DEFAULT
        if (String.IsNullOrEmpty(atributos))
        {
            atributos = _VS_tipos_documentos_atributos_default;

            if (String.IsNullOrEmpty(atributos))
            {
                try
                {
                    var tipoDocumentacaoAtributos = SYS_TipoDocumentacaoAtributoBO.SelecionarAtributos();

                    var auxAtributo = (from DataRow dr in tipoDocumentacaoAtributos.Rows
                                       select (dr["tda_default"] != DBNull.Value ? (bool)dr["tda_default"] : false));

                    auxAtributo.ToList().ForEach(x => atributos += Convert.ToByte(x));
                }
                catch (Exception ex)
                {
                    atributos = "0001000111100000000";
                }

                _VS_tipos_documentos_atributos_default = atributos;
            }
        }

        if (atributos != null)
        {
            //ESCONDE OS CAMPOS QUE NÃO PERTENCEM AO TIPO DE DOCUMENTO ESCOLHIDO
            for (int i = 0; i < atributos.Length; i++)
            {
                if (atributos[i] == '0')
                {
                    (e.Item.FindControl(campos[i])).Visible = false;
                }
            }
        }
    }

    /// <summary>
    /// Retorna todos os dados dos documentos que estão no repeater.
    /// </summary>
    /// <returns> Data table com dados dos documentos.</returns>
    private DataTable RetornaDocumento()
    {
        DataTable dtDocumento = CriaDataTableDocumento();

        //Preenche o dtDocumento com os dados do grid.
        foreach (RepeaterItem linha in rptDocumento.Items)
        {
            DataRow rowDoc = dtDocumento.NewRow();

            string tdo_id = ((Label)linha.FindControl("lbltdo_id")).Text;
            string unf_idEmissao = ((Label)linha.FindControl("lblUnf_idEmissao")).Text;
            string pai_idOrigem = ((Label)linha.FindControl("lblPai_idOrigem")).Text;

            if (string.IsNullOrEmpty(tdo_id))
                tdo_id = Guid.Empty.ToString();
            if (string.IsNullOrEmpty(unf_idEmissao))
                unf_idEmissao = Guid.Empty.ToString();
            if (string.IsNullOrEmpty(pai_idOrigem))
                pai_idOrigem = Guid.Empty.ToString();

            rowDoc["tdo_id"] = new Guid(((DropDownList)(linha.FindControl("ddlTipoDoc"))).SelectedValue != Guid.Empty.ToString() ?
                    ((DropDownList)(linha.FindControl("ddlTipoDoc"))).SelectedValue : tdo_id);
            rowDoc["unf_idEmissao"] = !string.IsNullOrEmpty(((DropDownList)linha.FindControl("ddlUF")).SelectedValue) ?
                    ((DropDownList)linha.FindControl("ddlUF")).SelectedValue : unf_idEmissao;
            rowDoc["unf_idAntigo"] = ((Label)linha.FindControl("lblUnf_idAntigo")).Text;
            rowDoc["tdo_nome"] = ((DropDownList)linha.FindControl("ddlTipoDoc")).SelectedItem.Text;
            rowDoc["numero"] = ((TextBox)(linha.FindControl("tbNumDoc"))).Text;
            rowDoc["dataemissao"] = ((TextBox)(linha.FindControl("tbDtEmissao"))).Text;
            rowDoc["orgaoemissao"] = ((TextBox)(linha.FindControl("tbOrgEmissor"))).Text;
            rowDoc["unf_nome"] = !string.IsNullOrEmpty(((DropDownList)linha.FindControl("ddlUF")).SelectedValue) ?
                    ((DropDownList)linha.FindControl("ddlUF")).SelectedItem.Text : string.Empty;
            rowDoc["info"] = ((TextBox)(linha.FindControl("tbInfCompl"))).Text;
            rowDoc["categoria"] = ((TextBox)(linha.FindControl("tbCategoria"))).Text;
            rowDoc["classificacao"] = ((TextBox)(linha.FindControl("tbClassificacao"))).Text;
            rowDoc["csm"] = ((TextBox)(linha.FindControl("tbCSM"))).Text;
            rowDoc["dataEntrada"] = ((TextBox)(linha.FindControl("tbDataEntrada"))).Text;
            rowDoc["dataValidade"] = ((TextBox)(linha.FindControl("tbDataValidade"))).Text;
            rowDoc["pai_idOrigem"] = !string.IsNullOrEmpty(((DropDownList)linha.FindControl("ddlPais")).SelectedValue) ?
                    ((DropDownList)linha.FindControl("ddlPais")).SelectedValue : pai_idOrigem;
            rowDoc["pai_idAntigo"] = ((Label)linha.FindControl("lblPai_IdAntigo")).Text;
            rowDoc["serie"] = ((TextBox)(linha.FindControl("tbSerie"))).Text;
            rowDoc["tipoGuarda"] = ((TextBox)(linha.FindControl("tbTipoGuarda"))).Text;
            rowDoc["via"] = ((TextBox)(linha.FindControl("tbVia"))).Text;
            rowDoc["secao"] = ((TextBox)(linha.FindControl("tbSecao"))).Text;
            rowDoc["zona"] = ((TextBox)(linha.FindControl("tbZona"))).Text;
            rowDoc["regiaoMilitar"] = ((TextBox)(linha.FindControl("tbRegiaoMilitar"))).Text;
            rowDoc["numeroRA"] = ((TextBox)(linha.FindControl("tbNumeroRA"))).Text;
            rowDoc["dataexpedicao"] = ((TextBox)(linha.FindControl("tbDataexpedicao"))).Text;
            rowDoc["banco"] = ((Label)linha.FindControl("lblBanco")).Text;
            rowDoc["excluido"] = (String.IsNullOrEmpty(((TextBox)linha.FindControl("tbNumDoc")).Text.Trim())
                    && String.IsNullOrEmpty(((TextBox)linha.FindControl("tbDtEmissao")).Text.Trim())
                    && String.IsNullOrEmpty(((TextBox)linha.FindControl("tbOrgEmissor")).Text.Trim())
                    && String.IsNullOrEmpty(((TextBox)linha.FindControl("tbDtEmissao")).Text.Trim())
                    && String.IsNullOrEmpty(((TextBox)linha.FindControl("tbOrgEmissor")).Text.Trim())
                    && String.IsNullOrEmpty(((TextBox)linha.FindControl("tbCategoria")).Text.Trim())
                    && String.IsNullOrEmpty(((TextBox)linha.FindControl("tbInfCompl")).Text.Trim())
                    && String.IsNullOrEmpty(((TextBox)linha.FindControl("tbClassificacao")).Text.Trim())
                    && String.IsNullOrEmpty(((TextBox)linha.FindControl("tbCSM")).Text.Trim())
                    && String.IsNullOrEmpty(((TextBox)linha.FindControl("tbDataEntrada")).Text.Trim())
                    && String.IsNullOrEmpty(((TextBox)linha.FindControl("tbSerie")).Text.Trim())
                    && String.IsNullOrEmpty(((TextBox)linha.FindControl("tbTipoGuarda")).Text.Trim())
                    && String.IsNullOrEmpty(((TextBox)linha.FindControl("tbVia")).Text.Trim())
                    && String.IsNullOrEmpty(((TextBox)linha.FindControl("tbSecao")).Text.Trim())
                    && String.IsNullOrEmpty(((TextBox)linha.FindControl("tbZona")).Text.Trim())
                    && String.IsNullOrEmpty(((TextBox)linha.FindControl("tbRegiaoMilitar")).Text.Trim())
                    && String.IsNullOrEmpty(((TextBox)linha.FindControl("tbNumeroRA")).Text.Trim())
                    && String.IsNullOrEmpty(((TextBox)linha.FindControl("tbDataexpedicao")).Text.Trim())
                    && string.IsNullOrEmpty(((DropDownList)linha.FindControl("ddlPais")).SelectedValue)
                    && string.IsNullOrEmpty(((DropDownList)linha.FindControl("ddlUF")).SelectedValue)
                    );

            dtDocumento.Rows.Add(rowDoc);
        }

        return dtDocumento;
    }

    ///// <summary>
    ///// Atualiza o conteúdo do grid sem validação.
    ///// </summary>
    private void AtualizaGridNovo()
    {
        DataTable dtAux = RetornaDocumento();

        if (dtAux.Rows.Count > 0)
        {
            foreach (RepeaterItem linha in rptDocumento.Items)
            {
                int index = linha.ItemIndex;

                dtAux.Rows[index]["tdo_id"] = new Guid(((DropDownList)(linha.FindControl("ddlTipoDoc"))).SelectedValue != Guid.Empty.ToString() ?
                    ((DropDownList)(linha.FindControl("ddlTipoDoc"))).SelectedValue : Guid.Empty.ToString());
                dtAux.Rows[index]["tdo_nome"] = ((DropDownList)linha.FindControl("ddlTipoDoc")).SelectedItem.Text;
                dtAux.Rows[index]["numero"] = ((TextBox)(linha.FindControl("tbNumDoc"))).Text;
                dtAux.Rows[index]["dataemissao"] = ((TextBox)(linha.FindControl("tbDtEmissao"))).Text;
                dtAux.Rows[index]["orgaoemissao"] = ((TextBox)(linha.FindControl("tbOrgEmissor"))).Text;
                dtAux.Rows[index]["unf_idEmissao"] = !string.IsNullOrEmpty(((DropDownList)linha.FindControl("ddlUF")).SelectedValue) ?
                    ((DropDownList)linha.FindControl("ddlUF")).SelectedValue : Convert.ToString(Guid.Empty);
                dtAux.Rows[index]["unf_nome"] = !string.IsNullOrEmpty(((DropDownList)linha.FindControl("ddlUF")).SelectedValue) ?
                    ((DropDownList)linha.FindControl("ddlUF")).SelectedItem.Text : string.Empty;
                dtAux.Rows[index]["info"] = ((TextBox)(linha.FindControl("tbInfCompl"))).Text;
                dtAux.Rows[index]["categoria"] = ((TextBox)(linha.FindControl("tbCategoria"))).Text;
                dtAux.Rows[index]["classificacao"] = ((TextBox)(linha.FindControl("tbClassificacao"))).Text;
                dtAux.Rows[index]["csm"] = ((TextBox)(linha.FindControl("tbCSM"))).Text;
                dtAux.Rows[index]["dataEntrada"] = ((TextBox)(linha.FindControl("tbDataEntrada"))).Text;
                dtAux.Rows[index]["dataValidade"] = ((TextBox)(linha.FindControl("tbDataValidade"))).Text;
                dtAux.Rows[index]["pai_idOrigem"] = !string.IsNullOrEmpty(((DropDownList)linha.FindControl("ddlPais")).SelectedValue) ?
                        ((DropDownList)linha.FindControl("ddlPais")).SelectedValue : Convert.ToString(Guid.Empty);
                dtAux.Rows[index]["serie"] = ((TextBox)(linha.FindControl("tbSerie"))).Text;
                dtAux.Rows[index]["tipoGuarda"] = ((TextBox)(linha.FindControl("tbTipoGuarda"))).Text;
                dtAux.Rows[index]["via"] = ((TextBox)(linha.FindControl("tbVia"))).Text;
                dtAux.Rows[index]["secao"] = ((TextBox)(linha.FindControl("tbSecao"))).Text;
                dtAux.Rows[index]["zona"] = ((TextBox)(linha.FindControl("tbZona"))).Text;
                dtAux.Rows[index]["regiaoMilitar"] = ((TextBox)(linha.FindControl("tbRegiaoMilitar"))).Text;
                dtAux.Rows[index]["numeroRA"] = ((TextBox)(linha.FindControl("tbNumeroRA"))).Text;
                dtAux.Rows[index]["dataexpedicao"] = ((TextBox)(linha.FindControl("tbDataexpedicao"))).Text;
                //dtAux.Rows[index]["tdo_atributos"] = ((Label)linha.FindControl("lblAtributos")).Text;
                dtAux.Rows[index]["banco"] = ((Label)linha.FindControl("lblBanco")).Text;
                dtAux.Rows[index]["excluido"] = (String.IsNullOrEmpty(((TextBox)linha.FindControl("tbNumDoc")).Text.Trim())
                        && String.IsNullOrEmpty(((TextBox)linha.FindControl("tbDtEmissao")).Text.Trim())
                        && String.IsNullOrEmpty(((TextBox)linha.FindControl("tbOrgEmissor")).Text.Trim())
                        && String.IsNullOrEmpty(((TextBox)linha.FindControl("tbInfCompl")).Text.Trim()));
            }
        }

        rptDocumento.DataSource = dtAux;
        rptDocumento.DataBind();
    }

    #endregion

    #region EVENTOS

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

    protected void ddlTipoDoc_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        RepeaterItem linha = (RepeaterItem)ddl.NamingContainer;

        ((Label)linha.FindControl("lbltdo_id")).Text = Guid.Empty.ToString();

        // Seta o valor do label tdo_id.
        string tdo_id = ((Label)linha.FindControl("lbltdo_id")).Text;
        ((Label)linha.FindControl("lbltdo_id")).Text = ddl.SelectedValue != Guid.Empty.ToString() ? ddl.SelectedValue : tdo_id;

        // Retorna as linhas já existentes no grid.
        DataTable dtDocumento = new DataTable();
        dtDocumento = RetornaDocumento();

        // Carrega a nova linha no grid.
        rptDocumento.DataSource = dtDocumento;
        rptDocumento.DataBind();

        Guid tdo_idCPF = new Guid(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_CPF));
        string CPF = string.Empty;

        Guid tdo_idSelecionado = string.IsNullOrEmpty(ddl.SelectedValue) ? Guid.Empty : new Guid(ddl.SelectedValue);

        if (tdo_idSelecionado != Guid.Empty)
        {
            if (tdo_idCPF == tdo_idSelecionado)
            {
                TextBox txtCPF = ((TextBox)linha.FindControl("tbNumDoc"));

                VS_CPF = txtCPF.Text;
                _TextChanged();
            }
            else
            {
                if (_VS_documentos.Rows.Count > 0)
                {
                    DataTable tblCPF = _VS_documentos.AsEnumerable()
                                            .Where(row => row.Field<Guid>("tdo_id") == tdo_idCPF)
                                            .CopyToDataTable();

                    VS_CPF = tblCPF.Rows[0]["numero"].ToString();
                    _TextChanged();
                }
            }
        }
    }

    protected void grvDocumento_DataBound(object sender, EventArgs e)
    {
        // Mostra botão adicionar.
        if (rptDocumento.Items.Count > 0)
            rptDocumento.Items[rptDocumento.Items.Count - 1].FindControl("btnAdicionar").Visible = true;
    }

    protected void btnLimparDocumento_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btnLimparDocumento = (ImageButton)sender;
        RepeaterItem linha = ((RepeaterItem)btnLimparDocumento.NamingContainer);

        try
        {
            // Se não for um registro do banco, limpa o drop tipo de documento.
            if (!Convert.ToBoolean(((Label)linha.FindControl("lblBanco")).Text))
                ((DropDownList)linha.FindControl("ddlTipoDoc")).SelectedIndex = -1;

            ((TextBox)linha.FindControl("tbNumDoc")).Text = "";
            ((TextBox)linha.FindControl("tbDtEmissao")).Text = "";
            ((TextBox)linha.FindControl("tbOrgEmissor")).Text = "";
            ((DropDownList)linha.FindControl("ddlUF")).SelectedIndex = -1;
            ((TextBox)linha.FindControl("tbInfCompl")).Text = "";
            ((TextBox)linha.FindControl("tbCategoria")).Text = "";
            ((TextBox)linha.FindControl("tbClassificacao")).Text = "";
            ((TextBox)linha.FindControl("tbCsm")).Text = "";
            ((TextBox)linha.FindControl("tbDataEntrada")).Text = "";
            ((TextBox)linha.FindControl("tbDataValidade")).Text = "";
            ((DropDownList)linha.FindControl("ddlPais")).SelectedIndex = -1;
            ((TextBox)linha.FindControl("tbSerie")).Text = "";
            ((TextBox)linha.FindControl("tbTipoGuarda")).Text = "";
            ((TextBox)linha.FindControl("tbVia")).Text = "";
            ((TextBox)linha.FindControl("tbSecao")).Text = "";
            ((TextBox)linha.FindControl("tbZona")).Text = "";
            ((TextBox)linha.FindControl("tbRegiaoMilitar")).Text = "";
            ((TextBox)linha.FindControl("tbNumeroRA")).Text = "";
            ((TextBox)linha.FindControl("tbDataExpedicao")).Text = "";

            ((DropDownList)linha.FindControl("ddlTipoDoc")).Focus();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar limpar os campos do documento.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void rptDocumento_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if ((e.Item.ItemType == ListItemType.AlternatingItem) ||
                (e.Item.ItemType == ListItemType.Item))
        {
            Label lblTdoId = (Label)e.Item.FindControl("lbltdo_id");
            Label lblUnf_idEmissao = (Label)e.Item.FindControl("lblUnf_idEmissao");
            Label lblPai_idOrigem = (Label)e.Item.FindControl("lblPai_idOrigem");
            Label lblBanco = (Label)e.Item.FindControl("lblBanco");

            if (lblTdoId != null && !string.IsNullOrEmpty(lblTdoId.Text))
                ((DropDownList)e.Item.FindControl("ddlTipoDoc")).SelectedValue = lblTdoId.Text;

            MostraCamposCorretosDeCadaDocumento(e);

            DropDownList ddlPais = (DropDownList)e.Item.FindControl("ddlPais");
            if (lblPai_idOrigem != null && !string.IsNullOrEmpty(lblPai_idOrigem.Text) && ddlPais != null)
            {
                ddlPais.SelectedValue = lblPai_idOrigem.Text;
                if (((HtmlGenericControl)e.Item.FindControl("Pais")).Visible)
                    ddlPais_SelectedIndexChanged(ddlPais, new EventArgs());
            }

            DropDownList ddlUF = (DropDownList)e.Item.FindControl("ddlUF");
            if ((!(lblPai_idOrigem != null && !string.IsNullOrEmpty(lblPai_idOrigem.Text)) ||
                 !((HtmlGenericControl)e.Item.FindControl("Pais")).Visible) && ddlUF != null)
            {
                ddlUF.Enabled = true;
                ddlUF.Items.Clear();
                ddlUF.DataSource = END_UnidadeFederativaBO.GetSelect(Guid.Empty, new Guid(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.PAIS_PADRAO_BRASIL)), "", "", 0, false, 0, 0);
                ddlUF.DataBind();
                ddlUF.Items.Insert(0, new ListItem("-- Selecione uma UF --", "", true));
            }
            if (lblUnf_idEmissao != null && !string.IsNullOrEmpty(lblUnf_idEmissao.Text) && ddlUF != null)
            {
                ddlUF.SelectedValue = lblUnf_idEmissao.Text;
            }

            // Se o registro for do banco, deixa enable false o drop de tipo de documento.
            if (lblBanco != null && Convert.ToBoolean(lblBanco.Text))
                ((DropDownList)e.Item.FindControl("ddlTipoDoc")).Enabled = false;
        }

    }

    protected void btAdicionar_Click(object sender, EventArgs e)
    {
        AdicionarLinhaRepeater();
    }

    protected void ddlPais_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        RepeaterItem linha = (RepeaterItem)ddl.NamingContainer;

        DropDownList ddlUF = (DropDownList)linha.FindControl("ddlUF");
        if (ddlUF != null)
        {
            ddlUF.Enabled = false;
            if (!ddl.SelectedValue.Equals(""))
            {
                ddlUF.Enabled = true;
                ddlUF.Items.Clear();
                ddlUF.DataSource = END_UnidadeFederativaBO.GetSelect(Guid.Empty, new Guid(ddl.SelectedValue), "", "", 0, false, 1, 1);
                ddlUF.DataBind();
                ddlUF.Items.Insert(0, new ListItem("-- Selecione uma UF --", "", true));
            }
        }
    }    

    protected void tbNumDoc_TextChanged(object sender, EventArgs e)
    {
        TextBox txtCPF = (TextBox)sender;
        RepeaterItem linha = (RepeaterItem)txtCPF.NamingContainer;
        DropDownList ddlTipoDoc = ((DropDownList)linha.FindControl("ddlTipoDoc"));

        Guid tdo_idCPF = new Guid(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TIPO_DOCUMENTACAO_CPF));
        string CPF = string.Empty;

        Guid tdo_idSelecionado = string.IsNullOrEmpty(ddlTipoDoc.SelectedValue) ? Guid.Empty : new Guid(ddlTipoDoc.SelectedValue);
        _lblMessage.Text = "";

        if (tdo_idSelecionado != Guid.Empty)
        {
            if (tdo_idCPF == tdo_idSelecionado)
            {
                VS_CPF = txtCPF.Text;
                _TextChanged();
                if (!UtilBO._ValidaCPF(txtCPF.Text))
                    _lblMessage.Text = UtilBO.GetErroMessage("CPF inválido.", UtilBO.TipoMensagem.Alerta);
            }
            else
            {
                if (_VS_documentos.Rows.Count > 0)
                {
                    DataTable tblCPF = _VS_documentos.AsEnumerable()
                                            .Where(row => row.Field<Guid>("tdo_id") == tdo_idCPF)
                                            .CopyToDataTable();

                    VS_CPF = tblCPF.Rows[0]["numero"].ToString();
                    _TextChanged();
                }
            }
        }
    }

    #endregion
}