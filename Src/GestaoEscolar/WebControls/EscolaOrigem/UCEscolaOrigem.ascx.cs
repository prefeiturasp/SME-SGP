using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;

namespace GestaoEscolar.WebControls.EscolaOrigem
{
    public partial class UCEscolaOrigem : MotherUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetarTipoEscola();
               
            ScriptManager sm = ScriptManager.GetCurrent(Page);
            if (sm != null)
            {                
                sm.Scripts.Add(new ScriptReference("~/Includes/jsUCCadastroEndereco.js"));
                sm.Services.Add(new ServiceReference("~/WSServicos.asmx"));              
            }

        }

        #region DELEGATES

        public delegate void onSeleciona(string eco_nome);
        public event onSeleciona Selecionar;

        public delegate void AbreJanelaCadastroCidade();
        public event AbreJanelaCadastroCidade _AbreJanelaCadastroCidade; 
        
        public void SelecionarEscola(string eco_nome)
        {
            if (Selecionar != null)
                Selecionar(eco_nome);
        }

       
        #endregion

        #region PROPRIEDADES

        /// <summary>
        /// Guarda o tipo de escola
        /// 1 - Movimentação - Origem
        /// 2 - Movimentação - Destino
        /// </summary>
        public byte VS_tipoEscola
        {
            get { return Convert.ToByte(ViewState["VS_tipoEscola"]); }
            set { ViewState["VS_tipoEscola"] = value; }
        }

        /// <summary>
        /// Guarda o tipo de rede de ensino para pesquisa e inclusão
        /// Se for menor ou igual a zero, o usuário escolhe
        /// </summary>
        public int VS_tre_id
        {
            get { return Convert.ToInt32(ViewState["VS_tre_id"]); }
            set { ViewState["VS_tre_id"] = value; }
        }

        /// <summary>
        /// Guarda o ID da escola
        /// </summary>
        public long VS_eco_id
        {
            get { return Convert.ToInt64(ViewState["VS_eco_id"]); }
            set { ViewState["VS_eco_id"] = value; }
        }

        /// <summary>
        /// Armazena o nome da escola.
        /// </summary>
        private string VS_eco_nome
        {
            get
            {
                return (ViewState["VS_eco_nome"] ?? string.Empty).ToString();
            }

            set
            {
                ViewState["VS_eco_nome"] = value;
            }
        }

        /// <summary>
        /// Armazena o código Inep da escola.
        /// </summary>
        private string VS_eco_codigoInep
        {
            get
            {
                return (ViewState["VS_eco_codigoInep"] ?? string.Empty).ToString();
            }

            set
            {
                ViewState["VS_eco_codigoInep"] = value;
            }
        }

        /// <summary>
        /// Armazena o ID da cidade origem/destino.
        /// </summary>
        private Guid VS_cid_id
        {
            get
            {
                return new Guid((ViewState["VS_cid_id"] ?? Guid.Empty).ToString());
            }

            set
            {
                ViewState["VS_cid_id"] = value;
            }
        }

        #endregion

        #region METODOS

        /// <summary>
        /// Limpa os campos da busca de escola de origem
        /// </summary>
        public void LimparBuscaEscolaOrigem()
        {
            UCComboTipoRedeEnsinoBusca.Valor = -1;
            txtBuscaEscolaOrigemDestino.Text = string.Empty;

            fdsResultadosEscolaOrigemDestino.Visible = false;            
        }

        /// <summary>
        /// Limpa os campos do cadastro de escola de origem
        /// </summary>
        public void LimparCadastroEscolaOrigem()
        {
            UCComboTipoRedeEnsino1.Valor = -1;
            txtNomeEscolaOrigemDestino.Text = string.Empty;
            txtCodigoInepEscolaOrigemDestino.Text = string.Empty;            
        }

        /// <summary>
        /// Seta o nome dos label's da tela 
        /// </summary>
        public void SetarTipoEscola()
        {
            // Seta os nomes dos labels de acordo com o tipo de escola
            if (VS_tipoEscola == 1)
            {
                //Busca
                lblMessageBuscaEscolaOrigemDestino.Text = "Escola de origem";
                btnNovoEscolaOrigemDestino.Text = "Incluir nova escola de origem";
                grvEscolaOrigemDestino.Columns[0].HeaderText = "Escola de origem";

                //Cadastro                    
                lblNomeEscolaOrigemDestino.Text = "Escola de origem *";
                rfvNomeEscolaOrigemDestino.ErrorMessage = "Escola de origem é obrigatório.";
            }
            else if (VS_tipoEscola == 2)
            {
                //Busca
                lblMessageBuscaEscolaOrigemDestino.Text = "Escola de destino";
                btnNovoEscolaOrigemDestino.Text = "Incluir nova escola de destino";
                grvEscolaOrigemDestino.Columns[0].HeaderText = "Escola de destino";

                //Cadastro                    
                lblNomeEscolaOrigemDestino.Text = "Escola de destino *";
                rfvNomeEscolaOrigemDestino.ErrorMessage = "Escola de destino é obrigatório.";
            }
           
            // Mostra ou não os filtros de tipo de rede de ensino
            if (VS_tre_id > 0)
            {
                UCComboTipoRedeEnsinoBusca.Visible = false;
                UCComboTipoRedeEnsino1.Visible = false;  
              
                txtBuscaEscolaOrigemDestino.Focus();
            }
            else
            {
                if (!UCComboTipoRedeEnsino1.Carregado())
                {
                    UCComboTipoRedeEnsino1.CarregarTipoRedeEnsino();
                }
                if (!UCComboTipoRedeEnsinoBusca.Carregado())
                {
                    UCComboTipoRedeEnsinoBusca.CarregarTipoRedeEnsino();
                }

                UCComboTipoRedeEnsinoBusca.Visible = true;
                UCComboTipoRedeEnsino1.Visible = true;     

                UCComboTipoRedeEnsinoBusca.SetarFoco();
            }
        }

        /// <summary>
        /// Cria e retorna a entidade de Cadastro de Escola de Origem
        /// </summary>
        /// <returns>Estrutura ACA_AlunoEscolaOrigem_Cadastro</returns>
        public ACA_AlunoEscolaOrigem_Cadastro CriarEntityCadastroEscolaOrigem()
        {
            ACA_AlunoEscolaOrigem_Cadastro cad = new ACA_AlunoEscolaOrigem_Cadastro
                                                     {
                                                         entEscolaOrigem = new ACA_AlunoEscolaOrigem { eco_id = VS_eco_id },
                                                         entEndereco = new END_Endereco()
                                                     };           

            try
            {                
                if (VS_eco_id > 0)
                {   
                    // Carrega as escolas de origens cadastradas
                    ACA_AlunoEscolaOrigemBO.GetEntity(cad.entEscolaOrigem);

                    // Carrega o endereço da escola de origem cadastrada
                    cad.entEndereco.end_id = cad.entEscolaOrigem.end_id;
                    END_EnderecoBO.GetEntity(cad.entEndereco);
                }
                else
                {
                    // Recupera os campos do UserControl de endereço
                    END_Endereco entityEndereco = new END_Endereco();
                    string numero = string.Empty;
                    string complemento = string.Empty;
                    //string msg;
                    //UCEnderecos1.RetornaEnderecoCadastrado(out entityEndereco, out numero, out complemento, out msg);
                    entityEndereco.end_situacao = 1;

                    // Armazena os dados da escola de origem informados pelo usuário 
                    cad.entEscolaOrigem.tre_id = VS_tre_id > 0 ? VS_tre_id : UCComboTipoRedeEnsino1.Valor;
                    cad.entEscolaOrigem.eco_nome = !string.IsNullOrEmpty(VS_eco_nome) ? VS_eco_nome : txtNomeEscolaOrigemDestino.Text;
                    cad.entEscolaOrigem.eco_codigoInep = !string.IsNullOrEmpty(VS_eco_codigoInep) ? VS_eco_codigoInep : txtCodigoInepEscolaOrigemDestino.Text;
                    cad.entEscolaOrigem.cid_id = !string.IsNullOrEmpty(VS_cid_id.ToString()) ? VS_cid_id : (string.IsNullOrEmpty(txtCid_idMunicipio.Value) ? Guid.Empty : new Guid(txtCid_idMunicipio.Value));
                    cad.entEscolaOrigem.eco_numero = numero;
                    cad.entEscolaOrigem.eco_complemento = complemento;
                    cad.entEscolaOrigem.eco_situacao = 1;

                    // Armazena os dados do endereço informados pelo usuário
                    cad.entEndereco = entityEndereco;
                }

                return cad;
            }
            catch (Exception)
            {
                return cad;
            }
        }

        /// <summary>
        /// Valida os campos do cadastro de escola
        /// </summary>
        /// <returns></returns>
        private bool ValidaCadastroEscolaOrigemDestino()
        {
            if (string.IsNullOrEmpty(txtNomeEscolaOrigemDestino.Text.Trim()))
            {
                lblMessageCadastroEscolaOrigemDestino.Text = UtilBO.GetErroMessage(lblNomeEscolaOrigemDestino.Text.Replace("*","") + " é obrigatório.", UtilBO.TipoMensagem.Alerta);
                return false;
            }

            //END_Endereco entityEndereco;
            //string numero;
            //string complemento;
            //string msg;

            //if (!UCEnderecos1.RetornaEnderecoCadastrado(out entityEndereco, out numero, out complemento, out msg))
            //{
            //    lblMessageCadastroEscolaOrigemDestino.Text = UtilBO.GetErroMessage(msg, UtilBO.TipoMensagem.Alerta);
            //    return false;
            //}

            lblMessageCadastroEscolaOrigemDestino.Visible = false;
            return true;
        }

        #endregion

        #region EVENTOS

        protected void btnPesquisarEscolaOrigemDestino_Click(object sender, EventArgs e)
        {
            try
            {
                grvEscolaOrigemDestino.PageIndex = 0;

                odsEscolaOrigemDestino.SelectParameters.Clear();
                odsEscolaOrigemDestino.SelectParameters.Add("eco_nome", txtBuscaEscolaOrigemDestino.Text.Trim());
                odsEscolaOrigemDestino.SelectParameters.Add("tre_id", VS_tre_id > 0 ? VS_tre_id.ToString() : UCComboTipoRedeEnsinoBusca.Valor.ToString());
                odsEscolaOrigemDestino.SelectParameters.Add("paginado", "true");

                grvEscolaOrigemDestino.DataBind();

                fdsResultadosEscolaOrigemDestino.Visible = true;
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessageBuscaEscolaOrigemDestino.Text = UtilBO.GetErroMessage("Erro ao tentar carregar " + lblNomeEscolaOrigemDestino.Text.Replace("*", "").ToLower() + ".", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnNovoEscolaOrigemDestino_Click(object sender, EventArgs e)
        {
            if (VS_tre_id > 0)
                txtNomeEscolaOrigemDestino.Focus();
            else
                UCComboTipoRedeEnsino1.SetarFoco();

            txtNomeEscolaOrigemDestino.Text = txtBuscaEscolaOrigemDestino.Text;
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "CadastroEscolaOrigemDestinoNovo", "$('#divCadastroEscolaOrigemDestino').dialog('open');", true);
        }

        protected void btnIncluirEscolaOrigemDestino_Click(object sender, EventArgs e)
        {
            if (ValidaCadastroEscolaOrigemDestino())
            {                
                VS_eco_id = -1;
                VS_eco_nome = txtNomeEscolaOrigemDestino.Text;
                VS_eco_codigoInep = txtCodigoInepEscolaOrigemDestino.Text;
                VS_cid_id = string.IsNullOrEmpty(txtCid_idMunicipio.Value) ? Guid.Empty : new Guid(txtCid_idMunicipio.Value);

                SelecionarEscola(VS_eco_nome);

                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "CadastroEscolaOrigemDestinoFechar", "$('#divCadastroEscolaOrigemDestino').dialog('close');", true);
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "BuscaEscolaOrigemDestinoFechar", "$('#divBuscaEscolaOrigemDestino').dialog('close');", true);                
            }
            else
            {
                lblMessageCadastroEscolaOrigemDestino.Visible = true;
            }
        }

        protected void grvEscolaOrigemDestino_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            string cid_id = grvEscolaOrigemDestino.DataKeys[e.NewSelectedIndex].Values["cid_id"].ToString();
            VS_eco_id = Convert.ToInt32(grvEscolaOrigemDestino.DataKeys[e.NewSelectedIndex].Values["eco_id"].ToString());
            VS_eco_nome = grvEscolaOrigemDestino.DataKeys[e.NewSelectedIndex].Values["eco_nome"].ToString();
            VS_cid_id = string.IsNullOrEmpty(cid_id) ? Guid.Empty : new Guid(cid_id);
            VS_eco_codigoInep = grvEscolaOrigemDestino.DataKeys[e.NewSelectedIndex].Values["eco_codigoInep"].ToString();

            SelecionarEscola(VS_eco_nome);

            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "BuscaEscolaOrigemDestinoFechar", "$('#divBuscaEscolaOrigemDestino').dialog('close');", true);            
        }

        protected void odsEscolaOrigemDestino_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (e.ExecutingSelectCount)
                e.InputParameters.Clear();
        }

        protected void btnCadastraCidade_Click(object sender, ImageClickEventArgs e)
        {
           if (_AbreJanelaCadastroCidade != null)
                _AbreJanelaCadastroCidade();
        }

        #endregion   
    }
}