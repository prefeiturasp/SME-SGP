using System;
using System.Web;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using CFG_RelatorioBO = MSTech.GestaoEscolar.BLL.CFG_RelatorioBO;
using CFG_ServidorRelatorioBO = MSTech.GestaoEscolar.BLL.CFG_ServidorRelatorioBO;
using MSTech.CoreSSO.BLL;

namespace GestaoEscolar.Configuracao.ServidorRelatorios
{
    public partial class Cadastro : MotherPageLogado
    {
        #region Propriedadades
        private int _VS_srr_id
        {
            get
            {
                if (ViewState["_VS_srr_id"] != null)
                {
                    return Convert.ToInt32(ViewState["_VS_srr_id"]);
                }

                return 1;
            }

            set
            {
                ViewState["_VS_srr_id"] = value;
            }
        }

        private Guid _VS_ent_id
        {
            get
            {
                if (ViewState["_VS_ent_id"] != null)
                {
                    return (Guid)(ViewState["_VS_ent_id"]);
                }

                return new Guid();
            }

            set
            {
                ViewState["_VS_ent_id"] = value;
            }
        }
        #endregion Propriedades

        #region Eventos Page Life Cycle
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    VerificarPermissaoUsuario();
                    Inicializar();
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMensagemErro.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema", UtilBO.TipoMensagem.Erro);
                }

                Page.Form.DefaultFocus = txtNomeServidor.ClientID;
                Page.Form.DefaultButton = btnSalvar.UniqueID;
            }
        }

        #endregion Eventos Page Life Cycle

        #region Métodos

        /// <summary>
        /// Iniciliaza os componentes da tela.
        /// </summary>
        private void Inicializar()
        {
            // Adiciona os asteriscos de obrigatórios
            AdicionaAsteriscoObrigatorio(lblNomeServidor);
            AdicionaAsteriscoObrigatorio(lblUsuario);
            AdicionaAsteriscoObrigatorio(lblUrlRelatorios);
            AdicionaAsteriscoObrigatorio(lblPastaRelatorios);
            AdicionaAsteriscoObrigatorio(lblSenha);
            AlterarCheckSenha(false);
            // Inicializa combo de local de processamento            
            ddlLocalProcessamento.Items.Add(new ListItem("Local", "0"));
            ddlLocalProcessamento.Items.Add(new ListItem("Remoto", "1"));
            // Inicializa combo de situação            
            ddlSituacao.Items.Add(new ListItem("Ativo", "0"));
            ddlSituacao.Items.Add(new ListItem("Bloqueado", "1"));
            // Carrega os dados
            CarregarEntidade();
            // Inicializa o grid com todos os relatórios ativos            
            chkRelatorios.DataSource = CFG_RelatorioBO.RelatoriosAtivos_SelectBy_EntidadeServidor(_VS_ent_id, _VS_srr_id);
            chkRelatorios.DataBind();
        }

        /// <summary>
        /// Carrega os dados do servidor de relatórios
        /// </summary>
        private void CarregarEntidade()
        {
            _VS_ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id;

            CFG_ServidorRelatorio srr = CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(_VS_ent_id);

            if (srr != null)
            {
                _VS_srr_id = srr.srr_id;
                txtNomeServidor.Text = srr.srr_nome;
                txtDescricaoServidor.Text = srr.srr_descricao;
                ddlLocalProcessamento.SelectedIndex = srr.srr_remoteServer ? 1 : 0;
                txtUsuario.Text = srr.srr_usuario;
                txtDominio.Text = srr.srr_dominio;
                txtUrlRelatorios.Text = srr.srr_diretorioRelatorios;
                txtPastaRelatorios.Text = srr.srr_pastaRelatorios;
                ddlSituacao.SelectedIndex = (srr.srr_situacao - 1);
                AlterarSelecaoComboLocalProcessamento(srr.srr_remoteServer);
            }
            else
            {                
                AlterarCheckSenha(true);
            }
        }

        /// <summary>
        /// Verifica se usuário tem permissão de acesso à página.
        /// </summary>
        private void VerificarPermissaoUsuario()
        {
            if (!__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage("Você não possui permissão para acessar a página solicitada.", UtilBO.TipoMensagem.Alerta);
                Response.Redirect("~/Index.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        /// <summary>
        /// Salva a configuração de servidor de relatórios
        /// </summary>
        private void Salvar()
        {
            try
            {
                CFG_ServidorRelatorio srr = new CFG_ServidorRelatorio()
                {
                    ent_id = _VS_ent_id
                    ,
                    srr_id = _VS_srr_id
                    ,
                    srr_nome = txtNomeServidor.Text
                    ,
                    srr_descricao = txtDescricaoServidor.Text
                    ,
                    srr_remoteServer = (ddlLocalProcessamento.SelectedIndex == 1)
                    ,
                    srr_usuario = txtUsuario.Text
                    ,
                    srr_dominio = txtDominio.Text
                    ,
                    srr_senha = chkAlterarSenha.Checked ? UtilBO.CriptografarSenha(txtSenha.Text, eCriptografa.TripleDES) : String.Empty
                    ,
                    srr_diretorioRelatorios = txtUrlRelatorios.Text
                    ,
                    srr_pastaRelatorios = txtPastaRelatorios.Text
                    ,
                    srr_situacao = Convert.ToInt16(ddlSituacao.SelectedIndex + 1)
                };

                List<CFG_RelatorioServidorRelatorio> relatorios =
                       (from ListItem row in chkRelatorios.Items
                        let rlt = row.Value.ToString().Split(';')
                        where row.Selected
                        select new CFG_RelatorioServidorRelatorio
                        {
                            ent_id = srr.ent_id
                          ,
                            srr_id = srr.srr_id
                          ,
                            rlt_id = Convert.ToInt32(rlt[0])
                          ,
                            IsNew = true
                        }
                       ).ToList();

                if (CFG_ServidorRelatorioBO.SalvarServidorRelatorio(srr, relatorios))
                {
                    lblMensagemErro.Text = UtilBO.GetErroMessage("Servidor de relatórios salvo com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    AlterarCheckSenha(false);
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "ent_id: " + _VS_ent_id.ToString() + " , srr_id: " + _VS_srr_id);
                }

            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagemErro.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o servidor de relatórios.",
                    UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Altera a visualização dos campos de senha
        /// </summary>
        /// <param name="valor"></param>
        private void AlterarCheckSenha(bool valor)
        {
            rfvSenha.Enabled = valor;
            lblSenha.Visible = valor;
            txtSenha.Visible = valor;
            lblConfirmarSenha.Visible = valor;
            txtConfirmarSenha.Visible = valor;
            chkAlterarSenha.Checked = valor;
        }

        /// <summary>
        /// Altera a visualização dos campos mediante ao combo ddlLocalProcessamento
        /// </summary>
        /// <param name="valor"></param>
        private void AlterarSelecaoComboLocalProcessamento(bool valor)
        {
            lblUsuario.Visible = valor;
            txtUsuario.Visible = valor;
            rfvUsuario.Enabled = valor;
            AlterarCheckSenha(false);
            chkAlterarSenha.Visible = valor;
            lblDominio.Visible = valor;
            txtDominio.Visible = valor;
            lblUrlRelatorios.Visible = valor;
            txtUrlRelatorios.Visible = valor;
            rfvUrlRelatorios.Enabled = valor;
        }

        #endregion Métodos

        #region Eventos
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Index.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
                Salvar();
        }

        protected void chkRelatorios_DataBound(object sender, EventArgs e)
        {
            CheckBoxList lista = (CheckBoxList)sender;

            foreach (ListItem item in lista.Items)
            {
                var valor = item.Value.ToString().Split(';');
                item.Selected = Convert.ToBoolean(Convert.ToByte(valor[1]));
            }
        }

        protected void chkAlterarSenha_CheckedChanged(object sender, EventArgs e)
        {
            AlterarCheckSenha(((CheckBox)sender).Checked);
        }

        protected void ddlLocalProcessamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            AlterarSelecaoComboLocalProcessamento(((DropDownList)sender).SelectedIndex == 1);
        }
        #endregion Eventos

    }
}