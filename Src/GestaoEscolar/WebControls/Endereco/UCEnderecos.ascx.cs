using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

public partial class WebControls_Endereco_UCEnderecos : MotherUserControl
{
    #region Exceptions

    // QoS 9308 - Exception criada para controlar a excessão gerada nas páginas que possuem abas e cadastro de endereços, podendo assim setar a aba correta se a validação do endereço não for satisfeita
    [Serializable]
    public class EnderecoException : ValidationException
    {
        public EnderecoException(string msg)
            : base(msg)
        {
        }
    }

    #endregion Exceptions

    #region Propriedades

    /// <summary>
    /// Guarda a opção de mostrar a localização geográfica.
    /// </summary>
    public bool VS_LocalizacaoGeografica
    {
        get
        {
            if (ViewState["VS_LocalizacaoGeografica"] == null)
            {
                return false;
            }

            return (bool)ViewState["VS_LocalizacaoGeografica"];
        }

        set
        {
            ViewState["VS_LocalizacaoGeografica"] = value;
        }
    }

    /// <summary>
    /// Retorna os endereços cadastrados.
    /// </summary>
    public DataTable _VS_enderecos
    {
        get
        {
            return RetornaTabelaCadastro();
        }
    }

    /// <summary>
    /// Propriedade em ViewState para setar o ValidationGroup dos validators de cada item
    /// do repeater.
    /// </summary>
    protected string _ValidationGroup
    {
        get
        {
            if (ViewState["_ValidationGroup"] == null)
            {
                return "Endereco";
            }

            return ViewState["_ValidationGroup"].ToString();
        }

        set
        {
            ViewState["_ValidationGroup"] = value;
        }
    }

    /// <summary>
    /// Propriedade em ViewState para setar a visibilidade do ValidationGroup do repeater.
    /// </summary>
    protected bool _VisibleValSummary
    {
        get
        {
            if (ViewState["_VisibleValSummary"] == null)
            {
                return true;
            }

            return Convert.ToBoolean(ViewState["_VisibleValSummary"]);
        }

        set
        {
            ViewState["_VisibleValSummary"] = value;
        }
    }

    /// <summary>
    /// Propriedade guardada em ViewState que indica se é cadastro de um único endereço,
    /// ou de vários.
    /// </summary>
    private bool _VS_CadastroUnico
    {
        get
        {
            if (ViewState["_VS_CadastroUnico"] == null)
            {
                return false;
            }

            return (bool)ViewState["_VS_CadastroUnico"];
        }

        set
        {
            ViewState["_VS_CadastroUnico"] = value;
        }
    }

    /// <summary>
    /// Propriedade guardada em ViewState que indica se é cadastro de unidade administrativa,
    /// ou de vários.
    /// </summary>
    private bool _VS_CadastroUA
    {
        get
        {
            if (ViewState["_VS_CadastroUA"] == null)
            {
                return false;
            }

            return (bool)ViewState["_VS_CadastroUA"];
        }

        set
        {
            ViewState["_VS_CadastroUA"] = value;
        }
    }

    /// <summary>
    /// Propriedade guardada em ViewState que indica se mostra o check de adaptado,
    /// ou de vários.
    /// </summary>
    private bool _VS_Adaptado
    {
        get
        {
            if (ViewState["_VS_Adaptado"] == null)
            {
                return false;
            }

            return (bool)ViewState["_VS_Adaptado"];
        }

        set
        {
            ViewState["_VS_Adaptado"] = value;
        }
    }

    /// <summary>
    /// Propriedade que indica se os campos do endereço são obrigatórios (se não informado,
    /// o padrão é true).
    /// </summary>
    protected bool _VS_Obrigatorio
    {
        get
        {
            if (ViewState["_VS_Obrigatorio"] == null)
            {
                return true;
            }

            return (bool)ViewState["_VS_Obrigatorio"];
        }

        set
        {
            ViewState["_VS_Obrigatorio"] = value;
        }
    }

    /// <summary>
    /// Propriedade que indica se a mensagem de UCCombosObrigatorios deve ser mostrada
    /// </summary>
    public bool VS_MostraMensagem
    {
        get
        {
            if (ViewState["VS_MostraMensagem"] == null)
            {
                return true;
            }

            return (bool)ViewState["VS_MostraMensagem"];
        }
        set
        {
            ViewState["VS_MostraMensagem"] = value;            
        }
    }

    /// <summary>
    /// Propriedade que indica a definição de um endereço principal (valor default é false).
    /// </summary>
    protected bool _VS_EnderecoPrincipal
    {
        get
        {
            if (ViewState["_VS_EnderecoPrincipal"] == null)
            {
                return false;
            }

            return (bool)ViewState["_VS_EnderecoPrincipal"];
        }

        set
        {
            ViewState["_VS_EnderecoPrincipal"] = value;
        }
    }

    /// <summary>
    /// Retorna o ClientID do txtLogradouro (se não for cadastro único, retorna da última
    /// linha do repeater).
    /// </summary>
    public string ClientIDTxtLogradouro
    {
        get
        {
            if (rptEndereco.Items.Count > 0)
            {
                TextBox txtLogradouro = (TextBox)rptEndereco.Items[rptEndereco.Items.Count - 1].FindControl("txtLogradouro");
                return txtLogradouro.ClientID;
            }

            return string.Empty;
        }
    }

    /// <summary>
    /// Retorna o ClientID do txtCEP (se não for cadastro único, retorna da última
    /// linha do repeater).
    /// </summary>
    public string ClientIDTxtCEP
    {
        get
        {
            if (rptEndereco.Items.Count > 0)
            {
                TextBox txt = (TextBox)rptEndereco.Items[rptEndereco.Items.Count - 1].FindControl("txtCEP");

                return txt.ClientID;
            }

            return string.Empty;
        }
    }

    /// <summary>
    /// Seta se o número e o complemento ficarão visíveis.
    /// </summary>
    public bool VisibleNumero
    {
        set
        {
            foreach (RepeaterItem item in rptEndereco.Items)
            {
                item.FindControl("trNumeroCompl").Visible = value;
            }
        }
    }

    /// <summary>
    /// Retorna o id da cidade
    /// </summary>
    public List<Guid> _Cid_ids
    {
        get
        {
            List<Guid> cid_ids = new List<Guid>();
            Guid id;

            foreach (DataRow dr in _VS_enderecos.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    id = string.IsNullOrEmpty(dr["cid_id"].ToString()) ? Guid.Empty : new Guid(dr["cid_id"].ToString());
                }
                else
                {
                    id = Guid.Empty;
                }

                cid_ids.Add(id);
            }

            return cid_ids;
        }
    }

    /// <summary>
    /// Retorna os cep's cadastrados
    /// </summary>
    public List<String> _Ceps
    {
        get
        {
            List<String> ceps = new List<String>();
            String cep;

            foreach (DataRow dr in RetornaEnderecos().Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    cep = string.IsNullOrEmpty(dr["end_cep"].ToString()) ? string.Empty : dr["end_cep"].ToString();
                    if (cep.Length == 8)
                    {
                        ceps.Add(cep);
                    }
                }
            }

            return ceps;
        }
    }

    ///// <summary>
    ///// Seta se o número e o complemento ficarão visíveis.
    ///// </summary>
    //public bool Enabletxt_Cid_id
    //{
    //    set
    //    {
    //        foreach (RepeaterItem item in rptEndereco.Items)
    //        {
    //            HtmlInputHidden txtCid_id = ((HtmlInputHidden)item.FindControl("txtCid_id"));
    //            txtCid_id.en
    //        }
    //    }
    //}

    public bool VS_MostraDistrito
    {
        get
        {
            if (ViewState["VS_MostraDistrito"] != null)
                return Convert.ToBoolean(ViewState["VS_MostraDistrito"]);
            return true;
        }
        set
        {
            ViewState["VS_MostraDistrito"] = value;
        }
    }

    private int indice;

    #endregion Propriedades

    #region Métodos

    /// <summary>
    /// Cria um endereço vazio e seta propriedades necessárias, chamar no Page_Load.
    /// </summary>
    public void Inicializar(bool obrigatorio, bool cadastroUnico, string validationGroup, bool visibleVSEndereco = true, bool enderecoPrincipal = false)
    {
        _ValidationGroup = validationGroup;
        _VS_CadastroUnico = cadastroUnico;
        _VS_Obrigatorio = obrigatorio;
        _VisibleValSummary = visibleVSEndereco;
        _VS_EnderecoPrincipal = enderecoPrincipal;

        NovoEndereco();

        SetaBotoes();
    }

    /// <summary>
    /// Cria um endereço vazio e seta propriedades necessárias, chamar no Page_Load.
    /// </summary>
    public void Inicializar(bool obrigatorio, bool cadastroUnico, string validationGroup, bool incluiNovo, bool visibleVSEndereco = true, bool enderecoPrincipal = false)
    {
        _ValidationGroup = validationGroup;
        _VS_CadastroUnico = cadastroUnico;
        _VS_Obrigatorio = obrigatorio;
        _VisibleValSummary = visibleVSEndereco;
        _VS_EnderecoPrincipal = enderecoPrincipal;

        if (incluiNovo)
        {
            NovoEndereco();
        }

        SetaBotoes();
    }

    /// <summary>
    /// Cria um endereço vazio e seta propriedades necessárias, chamar no Page_Load.
    /// </summary>
    public void Inicializar(bool obrigatorio, bool cadastroUnico, string validationGroup, bool incluiNovo, bool enderecoPrincipal, bool mostraAdaptado = false, bool cadastroUA = false, bool visibleVSEndereco = true)
    {
        _ValidationGroup = validationGroup;
        _VS_CadastroUnico = cadastroUnico;
        _VS_CadastroUA = cadastroUA;
        _VS_Obrigatorio = obrigatorio;
        _VS_EnderecoPrincipal = enderecoPrincipal;
        _VS_Adaptado = mostraAdaptado;
        _VisibleValSummary = visibleVSEndereco;

        if (_VS_CadastroUA && !SYS_ParametroBO.ParametroValorBooleano(SYS_ParametroBO.eChave.PERMITIR_MULTIPLOS_ENDERECOS_UA))
        {
            _VS_EnderecoPrincipal = false;
            _VS_CadastroUnico = true;
            CriaDataTable(true);
        }

        if (incluiNovo)
        {
            NovoEndereco();
        }

        SetaBotoes();
    }

    /// <summary>
    /// Atualiza os campos que estão habilitados ou desabilitados no repeater.
    /// </summary>
    public void AtualizaEnderecos()
    {
        foreach (RepeaterItem item in rptEndereco.Items)
        {
            string end_id = ((HtmlInputHidden)item.FindControl("txtEnd_id")).Value;

            if ((!String.IsNullOrEmpty(end_id)) &&
                (new Guid(end_id) != Guid.Empty))
            {
                // Deixa os campos como ReadyOnly.
                HabilitaCamposEndereco(item, false);
                item.FindControl("btnLimparEndereco").Visible = true;
            }
        }
    }

    /// <summary>
    /// Carrega um endereço com os dados passados por parâmetro - Utilizar quando for
    /// cadastro único.
    /// </summary>
    /// <param name="entEndereco"></param>
    /// <param name="numero"></param>
    /// <param name="complemento"></param>
    public void CarregarEndereco(END_Endereco entEndereco, string numero, string complemento, decimal latitude = 0, decimal longitude = 0)
    {
        try
        {
            DataTable dt = CriaDataTable(true);

            dt.Rows[0]["end_id"] = entEndereco.end_id;

            // Se for endereço novo - não tem END_ID - setar novo ID.
            if (entEndereco.end_id == Guid.Empty)
            {
                dt.Rows[0]["end_id"] = Guid.NewGuid();
            }

            dt.Rows[0]["end_cep"] = entEndereco.end_cep;
            dt.Rows[0]["end_logradouro"] = entEndereco.end_logradouro;
            dt.Rows[0]["end_distrito"] = entEndereco.end_distrito;
            dt.Rows[0]["end_zona"] = entEndereco.end_zona;
            dt.Rows[0]["end_bairro"] = entEndereco.end_bairro;
            dt.Rows[0]["cid_id"] = entEndereco.cid_id;
            dt.Rows[0]["latitude"] = latitude;
            dt.Rows[0]["longitude"] = longitude;

            // Carregar cidade.
            END_Cidade cid = new END_Cidade
            {
                cid_id = entEndereco.cid_id
            };
            END_CidadeBO.GetEntity(cid);

            dt.Rows[0]["cid_nome"] = cid.cid_nome;
            dt.Rows[0]["numero"] = numero;
            dt.Rows[0]["complemento"] = complemento;

            CarregarEnderecos(dt);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os endereços.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Carrega um endereço com os dados passados por parâmetro - Utilizar quando for
    /// cadastro único.
    /// </summary>
    /// <param name="end_id">Id do endereço</param>
    /// <param name="numero">Número do endereço</param>
    /// <param name="complemento">Complemento do endereço</param>
    /// <param name="latitude">Latitude do endereço</param>
    /// <param name="longitude">Longitude do endereço</param>
    public void CarregarEndereco(Guid end_id, string numero, string complemento, decimal latitude, decimal longitude)
    {
        try
        {
            if (VS_LocalizacaoGeografica)
            {
                if (rptEndereco.Items.Count > 0)
                {
                    RepeaterItem item = rptEndereco.Items[0];
                    TextBox txtLatitude = (TextBox)item.FindControl("txtLatitude");
                    TextBox txtLongitude = (TextBox)item.FindControl("txtLongitude");
                    if (txtLatitude != null)
                        txtLatitude.Text = longitude.Equals(0) ? string.Empty : latitude.ToString("R").Replace(",", ".");
                    if (txtLongitude != null)
                        txtLongitude.Text = longitude.Equals(0) ? string.Empty : longitude.ToString("R").Replace(",", ".");
                }
            }

            END_Endereco ent = new END_Endereco { end_id = end_id };
            END_EnderecoBO.GetEntity(ent);

            CarregarEndereco(ent, numero, complemento, latitude, longitude);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os endereços.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Carrega os dados com o DataTable passado, e guarda os ids em ViewState.
    /// </summary>
    /// <param name="dt"></param>
    public void CarregarEnderecosBanco(DataTable dt)
    {
        try
        {
            indice = 0;
            dt.Columns.Add("banco", typeof(Boolean));
            dt.Columns.Add("excluido", typeof(Boolean));

            if (!dt.Columns.Contains("latitude"))
            {
                if (dt.Columns.Contains("pse_latitude"))
                    dt.Columns["pse_latitude"].ColumnName = "latitude";
                else
                    dt.Columns.Add("latitude");
            }
            if (!dt.Columns.Contains("longitude"))
            {
                if (dt.Columns.Contains("pse_longitude"))
                    dt.Columns["pse_longitude"].ColumnName = "longitude";
                else
                    dt.Columns.Add("longitude");
            }
            if (!dt.Columns.Contains("principal"))
            {
                if (dt.Columns.Contains("pse_enderecoPrincipal"))
                    dt.Columns["pse_enderecoPrincipal"].ColumnName = "principal";
                else
                    dt.Columns.Add("principal");
            }
            if (!dt.Columns.Contains("prd_id"))
                dt.Columns.Add("prd_id");
            if (!dt.Columns.Contains("ped_id"))
                dt.Columns.Add("ped_id");
            if (!dt.Columns.Contains("uep_id"))
                dt.Columns.Add("uep_id");
            if (!dt.Columns.Contains("prd_adaptado_especial"))
                dt.Columns.Add("prd_adaptado_especial");

            foreach (DataRow dr in dt.Rows)
            {
                dr["banco"] = true;
                dr["excluido"] = false;
            }

            if (_VS_CadastroUA && _VS_CadastroUnico && dt.Rows.Count == 0)
                dt = CriaDataTable(true);

            rptEndereco.DataSource = dt;
            rptEndereco.DataBind();

            SetaBotoes();
        }
        catch
        {
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os endereços.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Carrega os dados com o DataTable passado, guarda os ids em ViewState e verifica endereço principal.
    /// </summary>
    /// <param name="dt">Datatabel com dados</param>
    /// <param name="enderecoPrincipal">Id identificador do endereço principal</param>
    public void CarregarEnderecosComPrincipalBanco(DataTable dt, Guid enderecoPrincipal)
    {
        try
        {
            indice = 0;
            dt.Columns.Add("banco", typeof(Boolean));
            dt.Columns.Add("excluido", typeof(Boolean));
            dt.Columns.Add("principal", typeof(Boolean));

            foreach (DataRow dr in dt.Rows)
            {
                dr["banco"] = true;
                dr["excluido"] = false;

                if(new Guid(dr["end_id"].ToString()).Equals(enderecoPrincipal))
                    dr["principal"] = true;
                else
                    dr["principal"] = false;

                if (!dt.Columns.Contains("prd_adaptado_especial"))
                    dt.Columns.Add("prd_adaptado_especial");

                if (string.IsNullOrEmpty(dr["prd_adaptado_especial"].ToString()))
                    dr["prd_adaptado_especial"] = false;
            }

            CarregarEnderecos(dt);
        }
        catch
        {
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os endereços.", UtilBO.TipoMensagem.Erro);
        }
    }


    /// <summary>
    /// Carrega os dados com o DataTable passado.
    /// </summary>
    /// <param name="dt"></param>
    private void CarregarEnderecos(DataTable dt)
    {
        try
        {
            indice = 0;

            if (!dt.Columns.Contains("latitude"))
            {
                if (dt.Columns.Contains("pse_latitude"))
                    dt.Columns["pse_latitude"].ColumnName = "latitude";
                else
                    dt.Columns.Add("latitude");
            }
            if (!dt.Columns.Contains("longitude"))
            {
                if (dt.Columns.Contains("pse_longitude"))
                    dt.Columns["pse_longitude"].ColumnName = "longitude";
                else
                    dt.Columns.Add("longitude");
            }
            if (!dt.Columns.Contains("principal"))
            {
                if (dt.Columns.Contains("pse_enderecoPrincipal"))
                    dt.Columns["pse_enderecoPrincipal"].ColumnName = "principal";
                else
                    dt.Columns.Add("principal");
            }
            if (!dt.Columns.Contains("prd_id"))
                dt.Columns.Add("prd_id");
            if (!dt.Columns.Contains("ped_id"))
                dt.Columns.Add("ped_id");
            if (!dt.Columns.Contains("uep_id"))
                dt.Columns.Add("uep_id");
            if (!dt.Columns.Contains("prd_adaptado_especial"))
                dt.Columns.Add("prd_adaptado_especial");

            if (_VS_CadastroUA && _VS_CadastroUnico && dt.Rows.Count == 0)
                dt = CriaDataTable(true);

            rptEndereco.DataSource = dt;
            rptEndereco.DataBind();


            SetaBotoes();
        }
        catch
        {
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os endereços.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Carrega a entidade do endereço cadastrado quando for cadastro único, seta o número
    /// e o complemento.
    /// Retorna true:
    /// - Se o endereço não é obrigatório e está completo
    ///     (todos os campos obrigatórios estão preenchidos).
    /// - Se o endereço não é obrigatório e não foi preenchido nenhum campo.
    /// Retorna false:
    /// - Se o endereço é obrigatório e não foi preenchido todos os campos.
    /// - Se o endereço não é obrigatório e tem somente alguns campos preenchidos
    ///     (começou tem que terminar).
    /// </summary>
    /// <param name="ent"></param>
    /// <param name="numero"></param>
    /// <param name="complemento"></param>
    /// <param name="latitude"></param>
    /// <param name="longitude"></param>
    /// <param name="msgErro"></param>
    /// <returns></returns>
    public bool RetornaEnderecoCadastrado(out END_Endereco ent, out string numero, out string complemento, out string msgErro, out decimal latitude, out decimal longitude)
    {
        ent = new END_Endereco();
        msgErro = string.Empty;
        numero = string.Empty;
        complemento = string.Empty;
        latitude = longitude = 0;

        if (rptEndereco.Items.Count > 0)
        {
            RepeaterItem item = rptEndereco.Items[0];
            latitude = ((HtmlGenericControl)item.FindControl("divLatLongitude")).Visible ? Convert.ToDecimal(((TextBox)item.FindControl("txtLatitude")).Text.Replace(".", ",")) : 0;
            longitude = ((HtmlGenericControl)item.FindControl("divLatLongitude")).Visible ? Convert.ToDecimal(((TextBox)item.FindControl("txtLongitude")).Text.Replace(".", ",")) : 0;
        }

        DataTable dt = RetornaEnderecos();

        if (dt.Rows.Count > 0)
        {
            // Carregar dados do endereço.
            DataRow dr = dt.Rows[0];

            ent = RetornaEntidade(dr);

            numero = dr["numero"].ToString();
            complemento = dr["complemento"].ToString();
        }

        bool ret;

        // Verificar se endereço está válido.
        if (_VS_Obrigatorio)
        {
            ret = ent.Validate() && (ent.cid_id != Guid.Empty);

            if (!ret)
            {
                msgErro = UtilBO.ErrosValidacao(ent);
            }

            if (ent.cid_id == Guid.Empty)
            {
                ret = false;
                if (String.IsNullOrEmpty(ent.cid_nome))
                {
                    msgErro += "Cidade é obrigatório.<br/>";
                }
                else
                {
                    msgErro += "Cidade não encontrada.<br/>";
                }
            }

            if (String.IsNullOrEmpty(numero))
            {
                ret = false;
                msgErro += "Número do endereço é obrigatório.<br/>";
            }
        }
        else
        {
            if (!String.IsNullOrEmpty(ent.end_cep) ||
                !String.IsNullOrEmpty(ent.end_logradouro) ||
                !String.IsNullOrEmpty(numero) ||
                !String.IsNullOrEmpty(ent.end_distrito) ||
                (ent.end_zona > 0) ||
                !String.IsNullOrEmpty(ent.end_bairro) ||
                (ent.cid_id != Guid.Empty))
            {
                // Se preencheu pelo menos 1 campo, tem que preencher todos.
                ret = ent.Validate() && (ent.cid_id != Guid.Empty);

                if (!ret)
                {
                    msgErro += UtilBO.ErrosValidacao(ent);
                }

                if (ent.cid_id == Guid.Empty)
                {
                    ret = false;
                    msgErro += "Cidade é obrigatório.<br/>";
                }

                if (String.IsNullOrEmpty(numero))
                {
                    ret = false;
                    msgErro += "Número do endereço é obrigatório.<br/>";
                }
            }
            else
            {
                ret = true;
            }
        }

        return ret;
    }

    /// <summary>
    /// Carrega uma entidade a patir dos dados do DataRow passado por parâmetro
    /// </summary>
    /// <param name="dr">DataRow com dados a serem carregados na entidade</param>
    /// <returns>Entidade carregada</returns>
    private END_Endereco RetornaEntidade(DataRow dr)
    {
        END_Endereco ent = new END_Endereco();

        if (dr.RowState != DataRowState.Deleted)
        {
            string end_id = dr["end_id"].ToString();

            // Preenche o ID do endereço.
            Guid _end_id;
            if (String.IsNullOrEmpty(end_id) || end_id.Equals(Guid.Empty.ToString()))
            {
                GestaoEscolarUtilBO.GuidTryParse(dr["id"].ToString(), out _end_id);
                ent.end_id = _end_id;
                ent.IsNew = true;
            }
            else
            {
                GestaoEscolarUtilBO.GuidTryParse(dr["end_id"].ToString(), out _end_id);
                ent.end_id = _end_id;
                ent.IsNew = false;

                END_EnderecoBO.GetEntity(ent);
            }

            ent.end_cep = dr["end_cep"].ToString();
            ent.end_logradouro = dr["end_logradouro"].ToString();
            ent.end_distrito = dr["end_distrito"].ToString();
            ent.end_zona = Convert.ToByte(dr["end_zona"]);
            ent.end_bairro = dr["end_bairro"].ToString();
            ent.cid_id = String.IsNullOrEmpty(dr["cid_id"].ToString()) ? Guid.Empty : new Guid(dr["cid_id"].ToString());
            ent.cid_nome = dr["cid_nome"].ToString();
        }

        return ent;
    }

    /// <summary>
    /// Retorna um dataTable com os RowStates corretos, de acordo com o que foi alterado
    /// na tela.
    /// </summary>
    /// <returns></returns>
    private DataTable RetornaTabelaCadastro()
    {
        string msgErro = string.Empty;
        bool ret = true;
        DataTable dt = RetornaEnderecos();

        foreach (DataRow dr in dt.Rows)
        {
            // Se o endereço for vazio, seta Guid.Empty.
            if (String.IsNullOrEmpty(dr["end_id"].ToString()))
            {
                dr["end_id"] = Guid.Empty.ToString();
            }

            if (Convert.ToBoolean(dr["excluido"]))
            {
                // Forçar o RowState para ficar como Deleted.
                dr.AcceptChanges();
                if (!Convert.ToBoolean(dr["banco"]))
                    dr.Delete();
            }
            else if (Convert.ToBoolean(dr["banco"]))
            {
                // Forçar o RowState para ficar como Modified.
                dr.AcceptChanges();
                dr["end_id"] = dr["end_id"];
            }

            END_Endereco ent = RetornaEntidade(dr);
            if (!ent.IsNew)
            {
                dr["end_id"] = ent.end_id.ToString();
            }

            if (_VS_Obrigatorio && (dr.RowState != DataRowState.Deleted))
            {
                ret = ent.Validate() && (ent.cid_id != Guid.Empty);

                if (!ret)
                {
                    msgErro = UtilBO.ErrosValidacao(ent);
                }

                if (ent.cid_id == Guid.Empty)
                {
                    if (String.IsNullOrEmpty(ent.cid_nome))
                    {
                        msgErro += "Cidade é obrigatório.<br/>";
                    }
                    else
                    {
                        msgErro += "Cidade não encontrada.<br/>";
                    }
                }

                if (String.IsNullOrEmpty(dr["numero"].ToString()))
                {
                    ret = false;
                    msgErro += "Número do endereço é obrigatório.<br/>";
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(ent.end_cep) ||
                    !String.IsNullOrEmpty(ent.end_logradouro) ||
                    (dr.RowState != DataRowState.Deleted) &&
                    !String.IsNullOrEmpty(dr["numero"].ToString()) ||
                    !String.IsNullOrEmpty(ent.end_distrito) ||
                    !String.IsNullOrEmpty(ent.end_bairro) ||
                    (ent.end_zona > 0) ||
                    (ent.cid_id != Guid.Empty))
                {
                    // Se preencheu pelo menos 1 campo, tem que preencher todos.
                    ret = ent.Validate() && (ent.cid_id != Guid.Empty);

                    if (!ret)
                    {
                        msgErro += UtilBO.ErrosValidacao(ent);
                    }

                    if (ent.cid_id == Guid.Empty)
                    {
                        ret = false;
                        msgErro += "Cidade é obrigatório.<br/>";
                    }

                    if (String.IsNullOrEmpty(dr["numero"].ToString()))
                    {
                        ret = false;
                        msgErro += "Número do endereço é obrigatório.<br/>";
                    }
                }
            }
            if (dr.RowState != DataRowState.Deleted && (
               !string.IsNullOrEmpty(dr["latitude"].ToString()) ||
               !string.IsNullOrEmpty(dr["longitude"].ToString())))
            {
                decimal latitude = string.IsNullOrEmpty(dr["latitude"].ToString()) ? 0 : decimal.Parse(dr["latitude"].ToString().Replace(".", ","));
                decimal longitude = string.IsNullOrEmpty(dr["longitude"].ToString()) ? 0 : decimal.Parse(dr["longitude"].ToString().Replace(".", ","));

                if (latitude <= -100 || latitude >= 100)
                {
                    ret = false;
                    msgErro += "Latitude inválida, deve estar entre -99,999999 e 99,999999.<br/>";
                }

                if (longitude <= -100 || longitude >= 100)
                {
                    ret = false;
                    msgErro += "Longitude inválida, deve estar entre -99,999999 e 99,999999.<br/>";
                }
            }
        }

        if (!ret)
        {
            throw new EnderecoException(msgErro);
        }

        return dt;
    }

    /// <summary>
    /// Retorna um datatable contendo os dados que estão no repeater.
    /// </summary>
    /// <returns></returns>
    private DataTable RetornaEnderecos()
    {
        DataTable dt = CriaDataTable(false);
        bool enderecoPrincipal = false;
        foreach (RepeaterItem item in rptEndereco.Items)
        {
            string end_id = ((HtmlInputHidden)item.FindControl("txtEnd_id")).Value;

            DataRow dr = dt.NewRow();

            dr["end_id"] = end_id;

            dr["id"] = ((Label)item.FindControl("lblID")).Text;
            dr["endRel_id"] = ((Label)item.FindControl("lblID")).Text;

            dr["end_cep"] = ((TextBox)item.FindControl("txtCEP")).Text;
            dr["end_logradouro"] = ((TextBox)item.FindControl("txtLogradouro")).Text;
            dr["end_distrito"] = ((TextBox)item.FindControl("txtDistrito")).Text;

            WebControls_Combos_UCComboZona UCComboZona1 = (WebControls_Combos_UCComboZona)item.FindControl("UCComboZona1");

            dr["end_zona"] = UCComboZona1._Combo.SelectedValue == "-1" ? 0 : Convert.ToInt32(UCComboZona1._Combo.SelectedValue);

            dr["end_bairro"] = ((TextBox)item.FindControl("txtBairro")).Text;
            dr["cid_id"] = ((HtmlInputHidden)item.FindControl("txtCid_id")).Value;
            dr["cid_nome"] = ((TextBox)item.FindControl("txtCidade")).Text;

            if (string.IsNullOrEmpty(dr["cid_id"].ToString()) && !string.IsNullOrEmpty(dr["cid_nome"].ToString()))
            {
                string nomeCidade = ((TextBox)item.FindControl("txtCidade")).Text;
                string nomeEstado = ((HtmlInputHidden)item.FindControl("txtUF")).Value;
                DataTable dtCidade = END_CidadeBO.GetSelect(Guid.Empty, Guid.Empty, Guid.Empty, nomeCidade, string.Empty, nomeEstado, string.Empty, 0, false, 0, 1);

                if (dtCidade.Rows.Count > 0)
                {
                    dr["cid_id"] = dtCidade.Rows[0]["cid_id"];
                    dr["cid_nome"] = dtCidade.Rows[0]["cid_nome"];
                }
            }

            dr["numero"] = ((TextBox)item.FindControl("txtNumero")).Text;
            dr["complemento"] = ((TextBox)item.FindControl("txtComplemento")).Text;

            dr["prd_id"] = ((Label)item.FindControl("lblPrdID")).Text;
            dr["ped_id"] = ((Label)item.FindControl("lblPedID")).Text;
            dr["uep_id"] = ((Label)item.FindControl("lblUepID")).Text;

            dr["prd_adaptado_especial"] = _chkAdaptado.Checked;

            dr["latitude"] = ((HtmlGenericControl)item.FindControl("divLatLongitude")).Visible ? ((TextBox)item.FindControl("txtLatitude")).Text : "";
            dr["longitude"] = ((HtmlGenericControl)item.FindControl("divLatLongitude")).Visible ? ((TextBox)item.FindControl("txtLongitude")).Text : "";

            bool banco;
            bool convert = Boolean.TryParse(((Label)item.FindControl("lblBanco")).Text, out banco);

            dr["banco"] = convert && banco;
            dr["excluido"] = String.IsNullOrEmpty(((TextBox)item.FindControl("txtCEP")).Text.Trim())
                    && String.IsNullOrEmpty(((TextBox)item.FindControl("txtLogradouro")).Text.Trim())
                    && String.IsNullOrEmpty(((TextBox)item.FindControl("txtBairro")).Text.Trim())
                    && String.IsNullOrEmpty(((TextBox)item.FindControl("txtCidade")).Text.Trim())
                    && String.IsNullOrEmpty(((TextBox)item.FindControl("txtNumero")).Text.Trim())
                    || !item.Visible;

            if (_VS_EnderecoPrincipal)
            {
                CheckBox chkPrincipal = (CheckBox)item.FindControl("chkPrincipal");
                if (chkPrincipal != null)
                    enderecoPrincipal = chkPrincipal.Checked;
                dr["principal"] = enderecoPrincipal;
            }
            else if (dt.Columns.Contains("principal"))
            {
                dr["principal"] = enderecoPrincipal;
            }

            //Se o endereço em questão não está no banco e não foi preenchido o CEP então não precisa adicionar na lista para salvar.
            if (!banco && string.IsNullOrEmpty(((TextBox)item.FindControl("txtCEP")).Text))
                continue;

            dt.Rows.Add(dr);
        }
        
        return dt;
    }

    /// <summary>
    /// Cria a tabela de endereços com os campos necessários.
    /// </summary>
    /// <param name="adicionaLinhaVazia"></param>
    /// <returns></returns>
    private DataTable CriaDataTable(bool adicionaLinhaVazia)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("cpr_id");
        dt.Columns.Add("id");
        dt.Columns.Add("endRel_id");
        dt.Columns.Add("end_id");
        dt.Columns.Add("end_cep");
        dt.Columns.Add("end_logradouro");
        dt.Columns.Add("end_distrito");
        dt.Columns.Add("end_zona");
        dt.Columns.Add("end_bairro");
        dt.Columns.Add("cid_id");
        dt.Columns.Add("cid_nome");
        dt.Columns.Add("numero");
        dt.Columns.Add("complemento");
        dt.Columns.Add("prd_id");
        dt.Columns.Add("ped_id");
        dt.Columns.Add("uep_id");
        dt.Columns.Add("latitude");
        dt.Columns.Add("longitude");
        dt.Columns.Add("prd_adaptado_especial");
        dt.Columns.Add("banco", typeof(Boolean));
        dt.Columns.Add("excluido", typeof(Boolean));
        dt.Columns.Add("principal", typeof(Boolean));

        if (adicionaLinhaVazia)
        {
            dt = AdicionaLinha(dt);
        }

        return dt;
    }

    /// <summary>
    /// Cria uma nova linha na tabela.
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    private DataTable AdicionaLinha(DataTable dt)
    {
        DataRow dr = dt.NewRow();
        dr["banco"] = false;
        dr["excluido"] = false;
        dr["id"] = Guid.NewGuid();
        dr["prd_adaptado_especial"] = false;

        dt.Rows.Add(dr);

        return dt;
    }

    /// <summary>
    /// Adiciona novo endereço no repeater
    /// </summary>
    private void NovoEndereco()
    {
        // Atualizar o DataTable do ViewState com os dados que estão no Repeater.
        DataTable dt = RetornaEnderecos();

        dt = AdicionaLinha(dt);

        CarregarEnderecos(dt);
    }

    /// <summary>
    /// Preeche os dados do item do repeater com os dados da linha.
    /// </summary>
    /// <param name="dr"></param>
    /// <param name="item"></param>
    /// <param name="preencherNumero"></param>
    private void PreencherDados(DataRow dr, RepeaterItem item, bool preencherNumero)
    {
        ((HtmlInputHidden)item.FindControl("txtEnd_id")).Value = dr["end_id"].ToString();

        ((TextBox)item.FindControl("txtCEP")).Text = dr["end_cep"].ToString();
        ((TextBox)item.FindControl("txtLogradouro")).Text = dr["end_logradouro"].ToString();
        ((TextBox)item.FindControl("txtDistrito")).Text = dr["end_distrito"].ToString();

        WebControls_Combos_UCComboZona UCComboZona1 = (WebControls_Combos_UCComboZona)item.FindControl("UCComboZona1");

        if (!String.IsNullOrEmpty(dr["end_zona"].ToString()) && (Convert.ToByte(dr["end_zona"].ToString()) > 0))
        {
            UCComboZona1._Combo.SelectedValue = dr["end_zona"].ToString();
        }

        ((TextBox)item.FindControl("txtBairro")).Text = dr["end_bairro"].ToString();
        ((HtmlInputHidden)item.FindControl("txtCid_id")).Value = dr["cid_id"].ToString();
        ((TextBox)item.FindControl("txtCidade")).Text = dr["cid_nome"].ToString();

        if (preencherNumero)
        {
            ((TextBox)item.FindControl("txtNumero")).Text = dr["numero"].ToString();
            ((TextBox)item.FindControl("txtComplemento")).Text = dr["complemento"].ToString();
        }
    }

    /// <summary>
    /// Limpa dados da linha informada.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="limpaCEP"></param>
    private void LimpaDadosLinha(RepeaterItem item, bool limpaCEP)
    {
        if (limpaCEP)
        {
            ((TextBox)item.FindControl("txtCEP")).Text = string.Empty;
        }

        ((WebControls_Combos_UCComboZona)item.FindControl("UCComboZona1"))._Combo.SelectedValue = "-1";
        ((TextBox)item.FindControl("txtLogradouro")).Text = string.Empty;
        ((TextBox)item.FindControl("txtDistrito")).Text = string.Empty;
        ((TextBox)item.FindControl("txtBairro")).Text = string.Empty;
        ((HtmlInputHidden)item.FindControl("txtCid_id")).Value = string.Empty;
        ((TextBox)item.FindControl("txtCidade")).Text = string.Empty;
        ((TextBox)item.FindControl("txtComplemento")).Text = string.Empty;
        ((TextBox)item.FindControl("txtNumero")).Text = string.Empty;

        TextBox txtLatitude = (TextBox)item.FindControl("txtLatitude");
        TextBox txtLongitude = (TextBox)item.FindControl("txtLongitude");
        if (txtLatitude != null)
            txtLatitude.Text = string.Empty;
        if (txtLongitude != null)
            txtLongitude.Text = string.Empty;

        Button btnLocalizarPorEndereco = (Button)item.FindControl("btnLocalizarPorEndereco");
        if (btnLocalizarPorEndereco != null)
        {
            btnLocalizarPorEndereco.Visible = false;
        }
        
        HtmlGenericControl divLatLongitude = (HtmlGenericControl)item.FindControl("divLatLongitude");
        if (divLatLongitude != null)
        {
            divLatLongitude.Visible = false;
        }

        Button btnLocalizarPorCoordenadas = (Button)item.FindControl("btnLocalizarPorCoordenadas");
        if (btnLocalizarPorCoordenadas != null)
        {
            btnLocalizarPorCoordenadas.Visible = false;
        }
    }

    /// <summary>
    /// Habilita/Desabilita os campos comuns do endereço (cidade, distrito,
    /// bairro, zona).
    /// </summary>
    /// <param name="item"></param>
    /// <param name="habilitado"></param>
    private void HabilitaCamposEndereco(RepeaterItem item, bool habilitado)
    {
        if (habilitado)
        {
            ((TextBox)item.FindControl("txtDistrito")).Attributes.Remove("readOnly1");
            ((TextBox)item.FindControl("txtBairro")).Attributes.Remove("readOnly1");
            ((TextBox)item.FindControl("txtCidade")).Attributes.Remove("readOnly1");
            ((TextBox)item.FindControl("txtDistrito")).AutoCompleteType = AutoCompleteType.None;
        }
        else
        {
            ((TextBox)item.FindControl("txtDistrito")).Attributes.Add("readOnly1", "readOnly");
            ((TextBox)item.FindControl("txtBairro")).Attributes.Add("readOnly1", "readOnly");
            ((TextBox)item.FindControl("txtCidade")).Attributes.Add("readOnly1", "readOnly");
            ((TextBox)item.FindControl("txtDistrito")).AutoCompleteType = AutoCompleteType.Disabled;
        }

        ((WebControls_Combos_UCComboZona)item.FindControl("UCComboZona1"))._Combo.Enabled = habilitado;
    }

    /// <summary>
    /// Coloca visible=true no botão Novo da última linha do repeater.
    /// </summary>
    private void SetaBotoes()
    {
        divAdaptado.Visible = _VS_Adaptado;

        // Só mostra botão de novo e de excluir se não for cadastro único.
        if (!_VS_CadastroUnico)
        {
            btnNovo.Visible = true;
        }
    }

    /// <summary>
    /// Exclui o endereço do índice informado da tabela.
    /// </summary>
    /// <param name="i"></param>
    private void ExcluirEndereco(int i)
    {
        DataTable dt = RetornaEnderecos();

        if (dt.Rows.Count > i)
        {
            if (Convert.ToBoolean(dt.Rows[i]["banco"]))
            {
                // Exclui logicamente a linha.
                dt.Rows[i]["excluido"] = true;
            }
            else
            {
                dt.Rows[i].Delete();
            }
        }

        CarregarEnderecos(dt);
    }

    /// <summary>
    /// Verifica se existe algum item visível no repeater
    /// </summary>
    /// <returns>true caso exista pelo menos um item visível
    /// ou false caso não exista nenhum item visível</returns>
    private bool VerificaItemVisivel()
    {
        foreach (RepeaterItem ri in rptEndereco.Items)
        {
            if (ri.Visible)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Desabilita todos os campos do cadastro de endereço
    /// </summary>
    public void DesabilitarCamposEnderecos()
    {
        foreach (RepeaterItem item in rptEndereco.Items)
        {
            ((ImageButton)item.FindControl("btnLimparEndereco")).Visible = false;
            ((TextBox)item.FindControl("txtCEP")).Enabled = false;
            ((TextBox)item.FindControl("txtLogradouro")).Enabled = false;
            ((TextBox)item.FindControl("txtNumero")).Enabled = false;
            ((TextBox)item.FindControl("txtNumero")).Enabled = false;
            ((TextBox)item.FindControl("txtComplemento")).Enabled = false;
            ((TextBox)item.FindControl("txtBairro")).Enabled = false;
            ((TextBox)item.FindControl("txtCidade")).Enabled = false;
            ((TextBox)item.FindControl("txtDistrito")).Enabled = false;
            ((WebControls_Combos_UCComboZona)item.FindControl("UCComboZona1"))._Combo.Enabled = false;
            TextBox txtLatitude = (TextBox)item.FindControl("txtLatitude");
            TextBox txtLongitude = (TextBox)item.FindControl("txtLongitude");
            if (txtLatitude != null)
                txtLatitude.Enabled = false;
            if (txtLongitude != null)
                txtLongitude.Enabled = false;
            Button btnLocalizarPorCoordenadas = (Button)item.FindControl("btnLocalizarPorCoordenadas");
            if (btnLocalizarPorCoordenadas != null)
            {
                btnLocalizarPorCoordenadas.Visible = false;
            }
            Button btnLocalizarPorEndereco = (Button)item.FindControl("btnLocalizarPorEndereco");
            if (btnLocalizarPorEndereco != null)
            {
                btnLocalizarPorEndereco.Visible = false;
            }
            ((Button)item.FindControl("btnExcluir")).Visible = false;
        }
        _chkAdaptado.Enabled = false;
        btnNovo.Visible = false;
    }

    /// <summary>
    ///  Verifica se o combo de endereço principal esta checado e se não estiver marca o primeiro como principal
    /// </summary>
    public void VerificaEnderecoPrincipal()
    {
        bool temEndPrincipal = false;
        CheckBox chkPrincipal;
        foreach (RepeaterItem item in rptEndereco.Items)
        {
            chkPrincipal = (CheckBox)item.FindControl("chkPrincipal");
            if (chkPrincipal.Checked)
                temEndPrincipal = true;
        }
        if (!temEndPrincipal && rptEndereco.Items.Count > 0)
        {
            chkPrincipal = (CheckBox)rptEndereco.Items[0].FindControl("chkPrincipal");
            chkPrincipal.Checked = true;
        }
    }

    private void eventosBotoes()
    {
        foreach (RepeaterItem item in rptEndereco.Items)
        {
            TextBox txtCEP = (TextBox)item.FindControl("txtCEP");
            TextBox txtLogradouro = (TextBox)item.FindControl("txtLogradouro");
            TextBox txtNumero = (TextBox)item.FindControl("txtNumero");
            TextBox txtBairro = (TextBox)item.FindControl("txtBairro");
            TextBox txtCidade = (TextBox)item.FindControl("txtCidade");

            HtmlGenericControl divLatLongitude = (HtmlGenericControl)item.FindControl("divLatLongitude");
            if (divLatLongitude != null)
            {
                divLatLongitude.Visible = VS_LocalizacaoGeografica && !string.IsNullOrEmpty(txtCEP.Text);

                TextBox txtLatitude = (TextBox)item.FindControl("txtLatitude");
                TextBox txtLongitude = (TextBox)item.FindControl("txtLongitude");

                Button btnLocalizarPorCoordenadas = (Button)item.FindControl("btnLocalizarPorCoordenadas");
                if (btnLocalizarPorCoordenadas != null)
                {
                    btnLocalizarPorCoordenadas.Visible = txtLatitude.Enabled && txtLongitude.Enabled && VS_LocalizacaoGeografica && !string.IsNullOrEmpty(txtCEP.Text);
                }
                Button btnLocalizarPorEndereco = (Button)item.FindControl("btnLocalizarPorEndereco");
                if (btnLocalizarPorEndereco != null)
                {
                    btnLocalizarPorEndereco.Visible = txtLatitude.Enabled && txtLongitude.Enabled && VS_LocalizacaoGeografica && !string.IsNullOrEmpty(txtCEP.Text);
                }

                if (btnLocalizarPorCoordenadas != null)
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "Script" + btnLocalizarPorCoordenadas.ClientID,
                                                        "$(\"input[id$='" + btnLocalizarPorCoordenadas.ClientID + "']\").click(function (e) {" +
                                                            "e.preventDefault();" +
                                                            "GerarMapaPorCoordenadas('#" + txtLatitude.ClientID + "', '#" + txtLongitude.ClientID + "', '#" +
                                                                                     map_canvas.ClientID + "');" +
                                                        "});", true);

                if (btnLocalizarPorEndereco != null)
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "Script" + btnLocalizarPorEndereco.ClientID,
                                                        "$(\"input[id$='" + btnLocalizarPorEndereco.ClientID + "']\").click(function (e) {" +
                                                            "e.preventDefault();" +
                                                            "GerarMapaPorEndereco('#" + txtLatitude.ClientID + "', '#" + txtLongitude.ClientID + "', '#" +
                                                                                  txtCEP.ClientID + "', '#" + txtLogradouro.ClientID + "', '#" +
                                                                                  txtNumero.ClientID + "', '#" + txtCidade.ClientID + "', '#" +
                                                                                  map_canvas.ClientID + "');" +
                                                        "});", true);
            }
        }
    }

    #endregion Métodos

    #region Eventos page life cycle

    protected void Page_Load(object sender, EventArgs e)
    {
        eventosBotoes();

        ScriptManager sm = ScriptManager.GetCurrent(Page);

        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsUCCadastroEndereco.js"));
            if (VS_LocalizacaoGeografica)
                sm.Scripts.Add(new ScriptReference("~/Includes/jsEnderecoMaps.js"));
        }

        if (VS_LocalizacaoGeografica)
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "Script" + map_canvas.ClientID, "Inicializar(" + map_canvas.ClientID + ");", true);
    }

    #endregion Eventos page life cycle

    #region Eventos

    protected void ValidarCep_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = Page.IsValid;
    }

    protected void txtCEP_TextChanged(object sender, EventArgs e)
    {
        TextBox _txtCEP = (TextBox)sender;
        RepeaterItem item = (RepeaterItem)_txtCEP.NamingContainer;
        bool _validaGlobal = _VS_Obrigatorio;
        try
        {
            if (!String.IsNullOrEmpty(_txtCEP.Text.Trim()))
            {
                item.FindControl("btnLimparEndereco").Visible = true;

                DataTable dt = END_EnderecoBO.GetSelect(Guid.Empty, Guid.Empty, Guid.Empty,
                        Guid.Empty, _txtCEP.Text, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, false, 0, 1);

                if (dt.Rows.Count == 1)
                {
                    // Preenche os dados do endereço.
                    PreencherDados(dt.Rows[0], item, false);

                    HabilitaCamposEndereco(item, false);

                    item.FindControl("txtNumero").Focus();
                }
                else
                {
                    LimpaDadosLinha(item, false);

                    HabilitaCamposEndereco(item, true);

                    item.FindControl("txtLogradouro").Focus();
                }

                ((RequiredFieldValidator)item.FindControl("rfvCEP")).Enabled = true;
                ((RequiredFieldValidator)item.FindControl("rfvLogradouro")).Enabled = true;
                ((RequiredFieldValidator)item.FindControl("rfvNumero")).Enabled = true;
                ((RequiredFieldValidator)item.FindControl("rfvBairro")).Enabled = true;
                ((RequiredFieldValidator)item.FindControl("rfvCidade")).Enabled = true;

                TextBox txtLogradouro = (TextBox)item.FindControl("txtLogradouro");
                TextBox txtNumero = (TextBox)item.FindControl("txtNumero");
                TextBox txtBairro = (TextBox)item.FindControl("txtBairro");
                TextBox txtCidade = (TextBox)item.FindControl("txtCidade");

                Button btnLocalizarPorCoordenadas = (Button)item.FindControl("btnLocalizarPorCoordenadas");
                if (btnLocalizarPorCoordenadas != null)
                {
                    btnLocalizarPorCoordenadas.Visible = VS_LocalizacaoGeografica;
                    btnLocalizarPorCoordenadas.Enabled = true;
                }
                Button btnLocalizarPorEndereco = (Button)item.FindControl("btnLocalizarPorEndereco");
                if (btnLocalizarPorEndereco != null)
                {
                    btnLocalizarPorEndereco.Visible = VS_LocalizacaoGeografica;
                    btnLocalizarPorEndereco.Enabled = true;
                }
                HtmlGenericControl divLatLongitude = (HtmlGenericControl)item.FindControl("divLatLongitude");
                if (divLatLongitude != null)
                {
                    divLatLongitude.Visible = VS_LocalizacaoGeografica;

                    TextBox txtLatitude = (TextBox)item.FindControl("txtLatitude");
                    TextBox txtLongitude = (TextBox)item.FindControl("txtLongitude");
                    if (divLatLongitude.Visible && txtLatitude != null)
                        txtLatitude.Text = "";
                    if (divLatLongitude.Visible && txtLongitude != null)
                        txtLongitude.Text = "";
                }

                // Se o CEP estiver preenchido, mostra os * de obrigatório nos labels do repeater item (validação por item).
                _VS_Obrigatorio = true;
                item.DataBind();
            }
            else
            {
                Button btnLocalizarPorEndereco = (Button)item.FindControl("btnLocalizarPorEndereco");
                if (btnLocalizarPorEndereco != null)
                {
                    btnLocalizarPorEndereco.Visible = false;
                }
                
                HtmlGenericControl divLatLongitude = (HtmlGenericControl)item.FindControl("divLatLongitude");
                if (divLatLongitude != null)
                {
                    divLatLongitude.Visible = false;
                }

                item.FindControl("btnLimparEndereco").Visible = false;

                LimpaDadosLinha(item, false);

                HabilitaCamposEndereco(item, true);

                item.FindControl("txtLogradouro").Focus();

                ((RequiredFieldValidator)item.FindControl("rfvCEP")).Enabled = false;
                ((RequiredFieldValidator)item.FindControl("rfvLogradouro")).Enabled = false;
                ((RequiredFieldValidator)item.FindControl("rfvNumero")).Enabled = false;
                ((RequiredFieldValidator)item.FindControl("rfvBairro")).Enabled = false;
                ((RequiredFieldValidator)item.FindControl("rfvCidade")).Enabled = false;

                // Se o CEP não estiver preenchido, não mostra os * de obrigatório nos labels do repeater item (validação por item).
                _VS_Obrigatorio = false;
                item.DataBind();
            }

            // Mantém o mesmo valor - _VS_Obrigatorio - inicializado pelo user control
            _VS_Obrigatorio = _validaGlobal;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            ((Label)item.FindControl("lblMessage")).Text =
                UtilBO.GetErroMessage("Erro ao tentar carregar o endereço.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void rptEndereco_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if ((e.Item.ItemType == ListItemType.AlternatingItem) ||
            (e.Item.ItemType == ListItemType.Item))
        {
            if (Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "excluido")))
            {
                e.Item.Visible = false;
            }
            else
            {
                if (divAdaptado.Visible)
                    _chkAdaptado.Checked = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "prd_adaptado_especial").ToString());

                TextBox txtCEP = (TextBox)e.Item.FindControl("txtCEP");
                TextBox txtLogradouro = (TextBox)e.Item.FindControl("txtLogradouro");
                TextBox txtNumero = (TextBox)e.Item.FindControl("txtNumero");
                TextBox txtBairro = (TextBox)e.Item.FindControl("txtBairro");
                TextBox txtCidade = (TextBox)e.Item.FindControl("txtCidade");

                Button btnLocalizarPorCoordenadas = (Button)e.Item.FindControl("btnLocalizarPorCoordenadas");
                if (btnLocalizarPorCoordenadas != null)
                {
                    btnLocalizarPorCoordenadas.Visible = VS_LocalizacaoGeografica && !string.IsNullOrEmpty(txtCEP.Text);
                }
                Button btnLocalizarPorEndereco = (Button)e.Item.FindControl("btnLocalizarPorEndereco");
                if (btnLocalizarPorEndereco != null)
                {
                    btnLocalizarPorEndereco.Visible = VS_LocalizacaoGeografica && !string.IsNullOrEmpty(txtCEP.Text);
                }
                HtmlGenericControl divLatLongitude = (HtmlGenericControl)e.Item.FindControl("divLatLongitude");
                if (divLatLongitude != null)
                {
                    divLatLongitude.Visible = VS_LocalizacaoGeografica && !string.IsNullOrEmpty(txtCEP.Text);
                    
                    TextBox txtLatitude = (TextBox)e.Item.FindControl("txtLatitude");
                    TextBox txtLongitude = (TextBox)e.Item.FindControl("txtLongitude");
                    decimal longitude = 0;
                    if (!string.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "longitude").ToString()))
                        Decimal.TryParse(DataBinder.Eval(e.Item.DataItem, "longitude").ToString().Replace(".", ","), out longitude);

                    if (divLatLongitude.Visible && txtLatitude != null)
                        txtLatitude.Text = longitude == 0 ? string.Empty : DataBinder.Eval(e.Item.DataItem, "latitude").ToString().Replace(",", ".");
                    if (divLatLongitude.Visible && txtLongitude != null)
                        txtLongitude.Text = longitude == 0 ? string.Empty : DataBinder.Eval(e.Item.DataItem, "longitude").ToString().Replace(",", ".");
                }
                
                // Habilita/desabilita validator de acordo com o preenchimento de outros campos obrigatórios.
                string ids = "'" + txtCEP.ClientID + "','"
                    + txtLogradouro.ClientID + "','"
                    + txtNumero.ClientID + "','"
                    + txtBairro.ClientID + "','"
                    + txtCidade.ClientID + "','"
                    + ((RequiredFieldValidator)e.Item.FindControl("rfvCEP")).ClientID + "','"
                    + ((RequiredFieldValidator)e.Item.FindControl("rfvLogradouro")).ClientID + "','"
                    + ((RequiredFieldValidator)e.Item.FindControl("rfvNumero")).ClientID + "','"
                    + ((RequiredFieldValidator)e.Item.FindControl("rfvBairro")).ClientID + "','"
                    + ((RequiredFieldValidator)e.Item.FindControl("rfvCidade")).ClientID + "'";

                txtCEP.Attributes.Add("onchange", "configuraValidacao(" + ids + ");");
                txtLogradouro.Attributes.Add("onchange", "configuraValidacao(" + ids + ");");
                txtNumero.Attributes.Add("onchange", "configuraValidacao(" + ids + ");");
                txtBairro.Attributes.Add("onchange", "configuraValidacao(" + ids + ");");
                txtCidade.Attributes.Add("onchange", "configuraValidacao(" + ids + ");");

                if (!String.IsNullOrEmpty(Convert.ToString(DataBinder.Eval(e.Item.DataItem, "end_id"))))
                {
                    // Deixa os campos como ReadyOnly.
                    HabilitaCamposEndereco(e.Item, false);
                    e.Item.FindControl("btnLimparEndereco").Visible = true;
                }

                Button btnExcluir = (Button)e.Item.FindControl("btnExcluir");
                btnExcluir.Visible = !_VS_CadastroUnico;

                // Inicializando combo de zona.
                WebControls_Combos_UCComboZona UCComboZona1 = (WebControls_Combos_UCComboZona)e.Item.FindControl("UCComboZona1");

                if (!String.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "end_zona").ToString()) &&
                    (Convert.ToByte(DataBinder.Eval(e.Item.DataItem, "end_zona").ToString()) > 0))
                {
                    // Selecionar Zona.
                    UCComboZona1._Combo.SelectedValue = DataBinder.Eval(e.Item.DataItem, "end_zona").ToString();
                }

                indice++;
                Panel pnlEndereco = (Panel)e.Item.FindControl("pnlEndereco");
                pnlEndereco.GroupingText = _VS_CadastroUnico ? string.Empty : "Endereço " + indice;

                // Exibe checkbox de endereço principal de acordo com configuração.
                if (_VS_EnderecoPrincipal)
                {
                    CheckBox chkPrincipal = (CheckBox)e.Item.FindControl("chkPrincipal");
                    if(chkPrincipal != null)
                        chkPrincipal.Visible = true;

                    if(!String.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "principal").ToString()))
                        chkPrincipal.Checked = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "principal").ToString());
                }
            }
        }
    }

    protected void btnLimparEndereco_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btnLimparEndereco = (ImageButton)sender;
        RepeaterItem item = (RepeaterItem)btnLimparEndereco.NamingContainer;

        try
        {
            ExcluirEndereco(item.ItemIndex);

            rptEndereco.Visible = rptEndereco.Items.Count > 0 && VerificaItemVisivel();

            NovoEndereco();

            if (rptEndereco.Visible == false)
            {
                rptEndereco.Visible = true;
            }

            rptEndereco.Items[rptEndereco.Items.Count - 1].FindControl("txtCEP").Focus();

            HabilitaCamposEndereco(item, true);

            item.FindControl("txtCEP").Focus();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            ((Label)item.FindControl("lblMessage")).Text =
                UtilBO.GetErroMessage("Erro ao tentar limpar os campos do endereço.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void btnNovo_Click(object sender, EventArgs e)
    {
        try
        {
            NovoEndereco();

            if (rptEndereco.Visible == false)
            {
                rptEndereco.Visible = true;
            }

            rptEndereco.Items[rptEndereco.Items.Count - 1].FindControl("txtCEP").Focus();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar incluir novo endereço.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void btnExcluir_Click(object sender, EventArgs e)
    {
        Button btnExcluir = (Button)sender;
        RepeaterItem item = (RepeaterItem)btnExcluir.NamingContainer;

        try
        {
            ExcluirEndereco(item.ItemIndex);

            rptEndereco.Visible = rptEndereco.Items.Count > 0 && VerificaItemVisivel();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            ((Label)item.FindControl("lblMessage")).Text =
                UtilBO.GetErroMessage("Erro ao tentar excluir o endereço.", UtilBO.TipoMensagem.Erro);
        }
    }

    #region Localização geográfica

    protected void ValidarLatitude_ServerValidate(object source, ServerValidateEventArgs args)
    {
        RepeaterItem item = (RepeaterItem)((CustomValidator)source).NamingContainer;
        if (item != null)
        {
            TextBox txtLatitude = (TextBox)item.FindControl("txtLatitude");
            if (txtLatitude != null && !string.IsNullOrEmpty(txtLatitude.Text))
            {
                double latitude;
                if (double.TryParse(txtLatitude.Text.Replace(".", ","), out latitude) && ((latitude >= 5.272222) || (-33.750833 >= latitude)))
                {
                    args.IsValid = false;
                }
            }
        }
    }

    protected void ValidarLongitude_ServerValidate(object source, ServerValidateEventArgs args)
    {
        RepeaterItem item = (RepeaterItem)((CustomValidator)source).NamingContainer;
        if (item != null)
        {
            TextBox txtLongitude = (TextBox)item.FindControl("txtLongitude");
            if (txtLongitude != null && !string.IsNullOrEmpty(txtLongitude.Text))
            {
                double longitude;
                if (double.TryParse(txtLongitude.Text.Replace(".", ","), out longitude) && ((longitude >= -28.85) || (-73.992222 >= longitude)))
                {
                    args.IsValid = false;
                }
            }
        }
    }

    #endregion Localização geográfica

    #endregion Eventos
}