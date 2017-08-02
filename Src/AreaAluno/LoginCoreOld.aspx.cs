using System;
using System.Data;
using System.Web;
using System.Web.UI;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;

namespace AreaAluno
{
    using System.Collections.Generic;

    using MSTech.Validation.Exceptions;
    using System.Web.Services;
    using MSTech.GestaoEscolar.Entities;
    public partial class LoginCoreOld : MotherPage
    {
        #region Métodos

        /// <summary>
        /// Valida campos na tela obrigatórios para o login.
        /// </summary>
        /// <returns></returns>
        private bool ValidarLogin()
        {

            if (string.IsNullOrEmpty(txtLogin.Text.Trim()))
            {
                lblMessage.Text = UtilBO.GetErroMessage("Login é obrigatório.", UtilBO.TipoMensagem.Alerta);
                return false;
            }

            if (string.IsNullOrEmpty(txtSenha.Text.Trim()))
            {
                lblMessage.Text = UtilBO.GetErroMessage("Senha é obrigatório.", UtilBO.TipoMensagem.Alerta);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Valida campos necessários para recuperar a senha.
        /// </summary>
        /// <returns></returns>
        private bool ValidarEsqueciSenha()
        {
            if (string.IsNullOrEmpty(txtEmail.Text.Trim()))
            {
                lblMessageEsqueciSenha.Text = UtilBO.GetErroMessage("E-mail é obrigatório.", UtilBO.TipoMensagem.Alerta);
                return false;
            }

            return true;
        }

        private void LoadSession(SYS_Usuario entityUsuario)
        {
            __SessionWEB.__UsuarioWEB.Usuario = entityUsuario;
            __SessionWEB.__UsuarioWEB.responsavel = RadioButtonList1.SelectedIndex == 1;
            if (__SessionWEB.__UsuarioWEB.responsavel)
            {
                SYS_Usuario entityUsuarioAluno = new SYS_Usuario
                {
                    ent_id = UCComboEntidade1.Valor
                    ,
                    usu_login = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.PREFIXO_LOGIN_ALUNO_AREA_ALUNO, __SessionWEB.__UsuarioWEB.Usuario.ent_id) + txtLogin.Text
                };

                SYS_UsuarioBO.GetSelectBy_ent_id_usu_login(entityUsuarioAluno);
                __SessionWEB.__UsuarioWEB.pes_idAluno = entityUsuarioAluno.pes_id;
            }
            
            // Carrega grupos do usuário
            IList<SYS_Grupo> list = SYS_GrupoBO.GetSelectBySis_idAndUsu_id(__SessionWEB.__UsuarioWEB.Usuario.usu_id, ApplicationWEB.AreaAlunoSistemaID);

            // Verifica se foi carregado os grupos do usuário
            if (list.Count > 0)
            {
                __SessionWEB.__UsuarioWEB.Grupo = list[0];
            }
            else
            {
                throw new ValidationException("Não foi possível atender a solicitação, nenhum grupo de usuário encontrado.");
            }

            // Armazena o cid_id referente a entidade do usuário na Session
            Guid ene_id = SYS_EntidadeEnderecoBO.Select_ene_idBy_ent_id(__SessionWEB.__UsuarioWEB.Usuario.ent_id);
            SYS_EntidadeEndereco entityEntidadeEndereco = new SYS_EntidadeEndereco { ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id, ene_id = ene_id };
            SYS_EntidadeEnderecoBO.GetEntity(entityEntidadeEndereco);

            END_Endereco entityEndereco = new END_Endereco { end_id = entityEntidadeEndereco.end_id };
            END_EnderecoBO.GetEntity(entityEndereco);
            __SessionWEB._cid_id = entityEndereco.cid_id;

            // Armazena o nome da pessoa ou o login do usuário na Session
            PES_Pessoa EntityPessoa = new PES_Pessoa { pes_id = __SessionWEB.__UsuarioWEB.Usuario.pes_id };
            PES_PessoaBO.GetEntity(EntityPessoa);
            __SessionWEB.UsuarioLogado = string.IsNullOrEmpty(EntityPessoa.pes_nome) ? __SessionWEB.__UsuarioWEB.Usuario.usu_login : EntityPessoa.pes_nome;
        }

        /// <summary>
        /// Redireciona para a tela de seleção do sistema
        /// </summary>
        private void RedirecionarLogin(bool RedirecionaIndexSelecaoAluno)
        {
            if (RedirecionaIndexSelecaoAluno)
            {
                Response.Redirect("~/IndexSelecaoAluno.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else
            {
                Response.Redirect("~/Index.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        /// <summary>
        /// Configura pop up de alteração de senha expirada.
        /// </summary>
        /// <param name="entityUsuario"></param>
        private void ConfigurarTelaSenhaExpirada(SYS_Usuario entityUsuario)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this.Page);
            if (sm != null)
            {
                string script = String.Format(@"var usu_id = '{0}';", entityUsuario.usu_id);

                if (sm.IsInAsyncPostBack)
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Usuario", script, true);
                }
                else
                {
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Usuario", script, true);
                }
            }

            // Mensagem de validação da senha atual.
            string mensagem = SYS_MensagemSistemaBO.RetornaValor(SYS_MensagemSistemaChave.MeusDadosMensagemSenhaAtualIncorreta);
            if (!string.IsNullOrEmpty(mensagem))
            {
                cvSenhaAtual.ErrorMessage = mensagem;
            }

            // Mensagem de validação de confirmação da nova senha.
            mensagem = SYS_MensagemSistemaBO.RetornaValor(SYS_MensagemSistemaChave.MeusDadosMensagemConfirmarSenhaNaoIdentico);

            if (!string.IsNullOrEmpty(mensagem))
            {
                _cpvConfirmarSenha.ErrorMessage = mensagem;
            }

            mensagem = SYS_MensagemSistemaBO.RetornaValor(SYS_MensagemSistemaChave.MeusDadosMensagemSenhaAtualSenhaNovaDiferenca);

            if (!string.IsNullOrEmpty(mensagem))
            {
                CompareValidator1.ErrorMessage = mensagem;
            }

            _txtSenhaAtual.Text = string.Empty;
            _txtNovaSenha.Text = string.Empty;
            _txtConfNovaSenha.Text = string.Empty;
            _txtSenhaAtual.Focus();


            ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Login, "Senha expirada." + entityUsuario.usu_login);
            ScriptManager.RegisterStartupScript(this, GetType(), "AlterarSenha",
                                                "$(document).ready(function(){ $('#divAlterarSenha').dialog('open'); }); ",
                                                true);
        }

        /// <summary>
        /// Valida campos necessários para alterar a senha.
        /// </summary>
        /// <returns></returns>
        private bool ValidarAlterarSenha()
        {
            if (string.IsNullOrEmpty(_txtSenhaAtual.Text.Trim()))
            {
                _lblMessageAlterarSenha.Text = UtilBO.GetErroMessage("Senha atual é obrigatório.", UtilBO.TipoMensagem.Alerta);
                return false;
            }

            if (string.IsNullOrEmpty(_txtNovaSenha.Text.Trim()))
            {
                _lblMessageAlterarSenha.Text = UtilBO.GetErroMessage("Nova senha é obrigatório.", UtilBO.TipoMensagem.Alerta);
                return false;
            }

            if (string.IsNullOrEmpty(_txtConfNovaSenha.Text.Trim()))
            {
                _lblMessageAlterarSenha.Text = UtilBO.GetErroMessage("Confirmar nova senha é obrigatório.", UtilBO.TipoMensagem.Alerta);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Valida se a senha atual está correta.
        /// </summary>
        /// <param name="senhaAtual">Senha atual.</param>
        /// <param name="usu_id">ID do usuário.</param>
        /// <returns></returns>
        [WebMethod]
        public static bool ValidarSenhaAtual(string senhaAtual, Guid usu_id)
        {
            SYS_Usuario entityUsuario = new SYS_Usuario { usu_id = usu_id };
            SYS_UsuarioBO.GetEntity(entityUsuario);

            // Configura criptografia da senha
            eCriptografa criptografia = (eCriptografa)Enum.Parse(typeof(eCriptografa), Convert.ToString(entityUsuario.usu_criptografia), true);
            if (!Enum.IsDefined(typeof(eCriptografa), criptografia))
                criptografia = eCriptografa.TripleDES;

            return UtilBO.EqualsSenha(entityUsuario.usu_senha, UtilBO.CriptografarSenha(senhaAtual, criptografia), criptografia);
        }

        /// <summary>
        /// Verifica se está acessando a área do aluno pela URL configurada do Ensino Intantil
        /// </summary>
        public void VerificaAcessoInfantil()
        {
            IDictionary<string, ICFG_Configuracao> configuracao;
            MSTech.GestaoEscolar.BLL.CFG_ConfiguracaoBO.Consultar(eConfig.Academico, out configuracao);
            if (configuracao.ContainsKey("AppURLAreaAlunoInfantil") && !string.IsNullOrEmpty(configuracao["AppURLAreaAlunoInfantil"].cfg_valor))
            {
                string url = HttpContext.Current.Request.Url.AbsoluteUri;
                string configInfantil = configuracao["AppURLAreaAlunoInfantil"].cfg_valor;
                
                if (url.Contains(configInfantil) && RadioButtonList1.Items.Count > 1)
                {
                    RadioButtonList1.SelectedIndex = 1;
                    RadioButtonList1.Items.RemoveAt(0);
                }
            }
        }

        #endregion

        #region Eventos Life Cycle

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Exibe linkbutton somente se não há parâmetro ou se seu valor é 'false'
                btnEsqueceuSenha.Visible = String.IsNullOrEmpty(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.REMOVER_OPCAO_ESQUECISENHA)) || !Boolean.Parse(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.REMOVER_OPCAO_ESQUECISENHA));
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference("~/includes/jsLogin.js"));
            }

            // Limpa a mensagem caso seja postback.
            fdsMensagem.Visible = false;
            fdsLogin.Attributes.Remove("style");

            if (!IsPostBack)
            {
                try
                {
                    // Setar a mensagem na frente do login.
                    if (!String.IsNullOrEmpty((SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.MENSAGEM_ALERTA_PRELOGIN))))
                    {
                        spnMensagemUsuario.InnerHtml = HttpUtility.HtmlDecode(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.MENSAGEM_ALERTA_PRELOGIN));
                        fdsMensagem.Visible = true;
                        fdsLogin.Attributes["style"] = "display:none;";
                        btnFechar.OnClientClick = "$('.fdsMensagem').hide();" +
                                                  "$('#" + fdsLogin.ClientID + "').show();" +
                                                  "$('#login').find('select,input').first().focus();" +
                                                  "return false;";
                        btnFechar.Focus();
                    }
                    else
                    {
                        Page.Form.DefaultButton = btnEntrar.UniqueID;
                        fdsMensagem.Visible = false;
                    }

                    UCComboEntidade1.ValidationGroup = "Login";
                    UCComboEntidade1.Obrigatorio = true;
                    UCComboEntidade1.CarregarPorEntidadeSituacao(Guid.Empty, 1);
                    UCComboEntidade1.Visible = UCComboEntidade1.MostrarCombo;

                    UCComboEntidade2.ValidationGroup = "EsqueciSenha";
                    UCComboEntidade2.Obrigatorio = true;
                    UCComboEntidade2.CarregarPorEntidadeSituacao(Guid.Empty, 1);
                    UCComboEntidade2.Visible = UCComboEntidade2.MostrarCombo;

                    VerificaAcessoInfantil();

                    // Verifica se usuário está autenticado, caso esteja redireciona para o sistema
                    if (UserIsAuthenticated())
                    {
                        RedirecionarLogin(false);
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);

                    ApplicationWEB._GravaErro(ex);
                }

                // Configuração utilizando o parâmetro "Expressão regular para validar o tamanho da senha do usuário"
                revSenhaTamanho.ValidationExpression = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TAMANHO_SENHA_USUARIO);
                revSenhaTamanho.ErrorMessage = String.Format(revSenhaTamanho.ErrorMessage, UtilBO.GetMessageTamanhoByRegex(revSenhaTamanho.ValidationExpression));
            }

            // Registra o GATC para a página de Login.
            UtilBO.RegistraGATC(this.Page);
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            // Setar o foco no txt de login, se no combo já estiver selecionado um valor (quando só
            // tem um valor no combo, seleciona automaticamente).
            if ((!IsPostBack) && (UCComboEntidade1.Valor != Guid.Empty))
            {
                Page.Form.DefaultFocus = txtLogin.ClientID;

                // Caso exista apenas uma entidade o Combo é escondido
                UCComboEntidade1.Visible = false;
            }
            else
            {
                Page.Form.DefaultFocus = UCComboEntidade1.ClientID;
            }
        }

        #endregion Eventos Life Cycle

        #region Eventos

        protected void btnEntrar_Click(object sender, EventArgs e)
        {
            if (ValidarLogin())
            {
                try
                {
                    // Carrega os dados do usuário necessário para o login
                    SYS_Usuario entityUsuario = new SYS_Usuario
                    {
                        ent_id = UCComboEntidade1.Valor
                        ,
                        usu_login = (RadioButtonList1.SelectedIndex == 0 
                                        ? ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.PREFIXO_LOGIN_ALUNO_AREA_ALUNO, Guid.Empty )
                                        : ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.PREFIXO_LOGIN_RESPONSAVEL_AREA_ALUNO, Guid.Empty)) + txtLogin.Text
                        ,
                        usu_senha = txtSenha.Text
                    };

                    // Checa as credenciais do usuário
                    LoginStatus status = SYS_UsuarioBO.LoginWEB(entityUsuario);

                    switch (status)
                    {
                        case LoginStatus.Erro:
                            {
                                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Login, "Erro ao tentar entrar no sistema." + entityUsuario.usu_login);
                                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar entrar no sistema.", UtilBO.TipoMensagem.Erro);
                                txtLogin.Focus();
                                break;
                            }
                        case LoginStatus.Bloqueado:
                            {
                                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Login, "Usuário bloqueado." + entityUsuario.usu_login);
                                lblMessage.Text = UtilBO.GetErroMessage("Usuário bloqueado.", UtilBO.TipoMensagem.Alerta);
                                txtLogin.Focus();
                                break;
                            }
                        case LoginStatus.NaoEncontrado:
                            {
                                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Login, "Usuário não encontrado." + entityUsuario.usu_login);

                                //mostra para o usuário a mensagem abaixo
                                lblMessage.Text = UtilBO.GetErroMessage("Usuário e/ou senha inválidos.", UtilBO.TipoMensagem.Alerta);
                                txtLogin.Focus();
                                break;
                            }
                        case LoginStatus.SenhaInvalida:
                            {
                                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Login, "Senha inválida." + entityUsuario.usu_login);

                                //mostra para o usuário a mensagem abaixo
                                lblMessage.Text = UtilBO.GetErroMessage("Usuário e/ou senha inválidos.", UtilBO.TipoMensagem.Alerta);
                                txtLogin.Focus();

                                break;
                            }
                        case LoginStatus.Expirado:
                            {
                                ConfigurarTelaSenhaExpirada(entityUsuario);
                                break;
                            }

                        case LoginStatus.Sucesso:
                            {
                                // Zera a quantidade de falhas de autenticação para o usuário.
                                SYS_UsuarioFalhaAutenticacaoBO.ZeraFalhaAutenticacaoUsuario(entityUsuario.usu_id);

                                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Login, "Login efetuado com sucesso.");

                                // Autenticação SAML.
                                SYS_UsuarioBO.AutenticarUsuario(entityUsuario);

                                // Configura usuário na Session
                                LoadSession(entityUsuario);

                                // Se selecionou para logar como responsável, verifica se esse ele é responsável por um aluno só, 
                                //  ou caso tenha mais, redireciona para uma tela de selação de alunos
                                if (RadioButtonList1.SelectedIndex == 1)
                                {
                                    DataTable dtAlunosDoResponsavel = ACA_AlunoResponsavelBO.SelecionaAlunosPorResponsavel(entityUsuario.pes_id);

                                    Session["Pes_Id_Responsavel"] = entityUsuario.pes_id.ToString();

                                    Session["Qtde_Filhos_Responsavel"] = dtAlunosDoResponsavel.Rows.Count;  

                                    if (dtAlunosDoResponsavel.Rows.Count > 1)
                                    {
                                        //Session["Pes_Id_Responsavel"] = entityUsuario.pes_id.ToString();
                                        RedirecionarLogin(true);
                                        break;
                                    }
                                }                                

                                RedirecionarLogin(false);

                                break;
                            }
                    }
                    
                }
                catch (ValidationException ex)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar entrar no sistema.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void btnEsqueceuSenha_Click(object sender, EventArgs e)
        {
            try
            {
                // Setar o foco no txt de login, se no combo já estiver selecionado um valor (quando só
                // tem um valor no combo, seleciona automaticamente).
                if (UCComboEntidade2.Valor != Guid.Empty)
                {
                    txtEmail.Focus();

                    // Caso exista apenas uma entidade o Combo é escondido
                    UCComboEntidade2.Visible = false;
                }
                else
                {
                    UCComboEntidade2.Focus();
                }

                txtEmail.Text = string.Empty;
                lblMessageEsqueciSenha.Visible = false;
                updEsqueciSenha.Update();
                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Login, "Usuário solicitou uma nova senha.");
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "EsqueciSenhaAbrir", "$('#divEsqueciSenha').dialog('open'); ", true);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            if (ValidarEsqueciSenha())
            {
                try
                {
                    DataTable dt = SYS_UsuarioBO.GetSelectBy_ent_id_usu_email(UCComboEntidade2.Valor, txtEmail.Text);

                    if (dt.Rows.Count == 0)
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Login, "Erro no envio de senha. Usuário não encontrado.");
                        lblMessage.Text = UtilBO.GetErroMessage("Usuário não encontrado.", UtilBO.TipoMensagem.Alerta);
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(dt.Rows[0]["usu_dominio"].ToString()))
                        {
                            ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Login, "Não é possível recuperar a senha pois o usuário solicitado está ligado no Active Directory.");
                            lblMessage.Text = UtilBO.GetErroMessage("Não é possível recuperar a senha pois o usuário solicitado está ligado no Active Directory, contate o administrador de rede do seu domínio.", UtilBO.TipoMensagem.Alerta);
                        }
                        else if (dt.Rows[0]["usu_situacao"].ToString() == "1" || dt.Rows[0]["usu_situacao"].ToString() == "5")
                        {
                            try
                            {
                                SYS_Usuario usu = new SYS_Usuario { usu_id = new Guid(dt.Rows[0]["usu_id"].ToString()) };
                                SYS_UsuarioBO.GetEntity(usu);

                                PES_Pessoa pes = new PES_Pessoa { pes_id = usu.pes_id };
                                PES_PessoaBO.GetEntity(pes);

                                usu.usu_situacao = 5;
                                SYS_UsuarioBO.Save(usu, pes.pes_nome, __SessionWEB.TituloGeral, ApplicationWEB._EmailHost, ApplicationWEB._EmailSuporte);

                                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Login, "Senha enviada para o e-mail com sucesso.");
                                lblMessage.Text = UtilBO.GetErroMessage("Senha enviada para o e-mail com sucesso.", UtilBO.TipoMensagem.Sucesso);
                            }
                            catch (DuplicateNameException ex)
                            {
                                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                                ScriptManager.RegisterClientScriptBlock(this, GetType(), "EsqueciSenhaErro", "$('#divEsqueciSenha').dialog('close');", true);
                            }
                            catch (Exception ex)
                            {
                                ApplicationWEB._GravaErro(ex);
                                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar enviar e-mail com a senha para o usuário.", UtilBO.TipoMensagem.Erro);
                                ScriptManager.RegisterClientScriptBlock(this, GetType(), "EsqueciSenhaErro", "$('#divEsqueciSenha').dialog('close');", true);
                            }
                        }
                        else if (dt.Rows[0]["usu_situacao"].ToString() == "4")
                        {
                            ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Login, "Erro no envio de senha. Usuário padrão.");
                            lblMessage.Text = UtilBO.GetErroMessage("Usuário padrão.", UtilBO.TipoMensagem.Alerta);
                        }
                        else
                        {
                            ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Login, "Erro no envio de senha. Usuário bloqueado.");
                            lblMessage.Text = UtilBO.GetErroMessage("Usuário bloqueado.", UtilBO.TipoMensagem.Alerta);
                        }
                    }

                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "EsqueciSenha", "$('#divEsqueciSenha').dialog('close');", true);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessageEsqueciSenha.Text = UtilBO.GetErroMessage("Erro ao tentar enviar e-mail com a senha para o usuário.", UtilBO.TipoMensagem.Erro);
                }
            }
            else
            {
                updEsqueciSenha.Update();
            }
        }

        protected void _btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidarAlterarSenha())
            {
                try
                {
                    // Carrega os dados do usuário
                    SYS_Usuario entityUsuario = new SYS_Usuario
                    {
                        ent_id = UCComboEntidade1.Valor
                        ,
                        usu_login = (RadioButtonList1.SelectedIndex == 0
                                        ? ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.PREFIXO_LOGIN_ALUNO_AREA_ALUNO, Guid.Empty)
                                        : ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.PREFIXO_LOGIN_RESPONSAVEL_AREA_ALUNO, Guid.Empty)) + txtLogin.Text
                    };
                    SYS_UsuarioBO.GetSelectBy_ent_id_usu_login(entityUsuario);

                    // Configura criptografia da senha
                    eCriptografa criptografia = (eCriptografa)Enum.Parse(typeof(eCriptografa), Convert.ToString(entityUsuario.usu_criptografia), true);
                    if (!Enum.IsDefined(typeof(eCriptografa), criptografia))
                        criptografia = eCriptografa.TripleDES;

                    // Verifica a senha do usuário comparando com a senha atual
                    if (!UtilBO.EqualsSenha(entityUsuario.usu_senha, UtilBO.CriptografarSenha(_txtSenhaAtual.Text, criptografia), criptografia))
                    {
                        string mensagemSenhaAtualInvalida = SYS_MensagemSistemaBO.RetornaValor(SYS_MensagemSistemaChave.MeusDadosMensagemSenhaAtualIncorreta);

                        _lblMessageAlterarSenha.Text = UtilBO.GetErroMessage(string.IsNullOrEmpty(mensagemSenhaAtualInvalida) ? "Senha atual inválida." : mensagemSenhaAtualInvalida, UtilBO.TipoMensagem.Erro);
                        _updAlterarSenha.Update();

                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Login, "Erro ao alterar senha. Senha atual inválida.");
                    }
                    else
                    {
                        // Atualiza dados do usuário
                        entityUsuario.usu_situacao = 1;
                        entityUsuario.usu_senha = _txtNovaSenha.Text;
                        entityUsuario.usu_dataAlteracao = DateTime.Now;
                        SYS_UsuarioBO.AlterarSenhaAtualizarUsuario(entityUsuario, entityUsuario.usu_integracaoAD == (byte)SYS_UsuarioBO.eIntegracaoAD.IntegradoADReplicacaoSenha);

                        // Configura usuário na Session
                        LoadSession(entityUsuario);
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Login, "Senha alterada com sucesso.");

                        string mensagemSenhaAlterada = SYS_MensagemSistemaBO.RetornaValor(SYS_MensagemSistemaChave.LoginMensagemSenhaAlteradaSucesso);

                        __SessionWEB.PostMessages = UtilBO.GetErroMessage(string.IsNullOrEmpty(mensagemSenhaAlterada) ? "Senha alterada com sucesso." : mensagemSenhaAlterada, UtilBO.TipoMensagem.Sucesso);

                        // Autenticação SAML.
                        SYS_UsuarioBO.AutenticarUsuario(entityUsuario);

                        // Configura usuário na Session
                        LoadSession(entityUsuario);

                        // Se selecionou para logar como responsável, verifica se esse ele é responsável por um aluno só, 
                        //  ou caso tenha mais, redireciona para uma tela de selação de alunos
                        if (RadioButtonList1.SelectedIndex == 1)
                        {
                            DataTable dtAlunosDoResponsavel = ACA_AlunoResponsavelBO.SelecionaAlunosPorResponsavel(entityUsuario.pes_id);

                            Session["Pes_Id_Responsavel"] = entityUsuario.pes_id.ToString();

                            Session["Qtde_Filhos_Responsavel"] = dtAlunosDoResponsavel.Rows.Count;

                            if (dtAlunosDoResponsavel.Rows.Count > 1)
                            {
                                //Session["Pes_Id_Responsavel"] = entityUsuario.pes_id.ToString();
                                RedirecionarLogin(true);
                                return;
                            }
                        }

                        RedirecionarLogin(false);
                    }
                }
                catch (DuplicateNameException ex)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "AlterarSenhaErro", "$('#divAlterarSenha').dialog('close');", true);
                }
                catch (ArgumentException ex)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "AlterarSenhaErro", "$('#divAlterarSenha').dialog('close');", true);
                }
                catch (ValidationException ex)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "AlterarSenhaErro", "$('#divAlterarSenha').dialog('close');", true);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);

                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar alterar a senha do usuário.", UtilBO.TipoMensagem.Erro);
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "AlterarSenhaErro", "$('#divAlterarSenha').dialog('close');", true);
                }
            }
            else
            {
                _updAlterarSenha.Update();
            }
        }

        #endregion Eventos
    }
}