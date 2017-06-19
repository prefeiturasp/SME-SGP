<%@ Page Language="C#" MasterPageFile="~/MasterPageAreaAberta.Master" AutoEventWireup="true" CodeBehind="LoginCoreOld.aspx.cs" Inherits="AreaAluno.LoginCoreOld" %>

<%@ Register Src="WebControls/Combos/UCComboEntidade.ascx" TagName="UCComboEntidade" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="login">
        <fieldset id="fdsMensagem" runat="server" class="fdsMensagem" visible="false" enableviewstate="false">
            <div class="nossaDiv">
                <br />
                <span id="spnMensagemUsuario" runat="server" enableviewstate="false"></span>
                <br />
                <br />
            </div>
            <div class="right">
                <asp:Button ID="btnFechar" runat="server" Text="Continuar login" EnableViewState="false" />
            </div>
        </fieldset>
        <div id="divAlterarSenha" title="Senha expirada - alterar senha" class="hide">
            <asp:UpdatePanel ID="_updAlterarSenha" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:UpdateProgress ID="UpdateProgress3" runat="server" DisplayAfter="10" AssociatedUpdatePanelID="_updAlterarSenha">
                        <ProgressTemplate>
                            <div class="loader">
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <asp:Label ID="_lblMessageAlterarSenha" runat="server" EnableViewState="False"></asp:Label>
                    <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="AlterarSenha" />
                    <fieldset>
                        <asp:Label ID="_lblSenhaAtual" runat="server" Text="Senha atual" AssociatedControlID="_txtSenhaAtual"></asp:Label>
                        <asp:TextBox ID="_txtSenhaAtual" runat="server" TextMode="Password" SkinID="text20C"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="_rfvSenhaAtual" ControlToValidate="_txtSenhaAtual"
                            ValidationGroup="AlterarSenha" runat="server" ErrorMessage="Senha atual é obrigatório.">*</asp:RequiredFieldValidator>
                         <asp:CustomValidator ID="cvSenhaAtual" ControlToValidate="_txtSenhaAtual" Display="Dynamic"
                                runat="server" ErrorMessage="Senha atual inválida." ValidationGroup="AlterarSenha" ClientValidationFunction="cvSenhaAtual_ClientValidate">*</asp:CustomValidator>
                        <asp:Label ID="_lblNovaSenha" runat="server" Text="Nova senha" AssociatedControlID="_txtNovaSenha"></asp:Label>
                        <asp:TextBox ID="_txtNovaSenha" runat="server" TextMode="Password" SkinID="text20C"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="_rfvNovaSenha" runat="server" ControlToValidate="_txtNovaSenha"
                            ValidationGroup="AlterarSenha" ErrorMessage="Nova senha é obrigatório." Display="Dynamic">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revSenhaTamanho" runat="server" ControlToValidate="_txtNovaSenha"
                            Display="Dynamic" ValidationGroup="AlterarSenha" ErrorMessage="A senha deve conter {0}."
                            Enabled="false">*</asp:RegularExpressionValidator>
                        <asp:RegularExpressionValidator ID="revSenhaFormato" runat="server" ControlToValidate="_txtNovaSenha"
                            ValidationGroup="AlterarSenha" Display="Dynamic" ErrorMessage="A senha não pode conter espaços em branco."
                            ValidationExpression="[^\s]+" Enabled="false">*</asp:RegularExpressionValidator>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Senha atual e nova senha devem ser diferentes"
                            ValidationGroup="AlterarSenha" Operator="NotEqual" ControlToCompare="_txtNovaSenha"
                            ControlToValidate="_txtSenhaAtual">*</asp:CompareValidator>
                        <%--<br />
                        <asp:Label ID="lblMsnNovaSenha" runat="server" Text="({0}, utilizando letras e números)."></asp:Label>
                        <br />--%>
                        <asp:Label ID="_lblConfNovaSenha" runat="server" Text="Confirmar nova senha" AssociatedControlID="_txtConfNovaSenha"></asp:Label>
                        <asp:TextBox ID="_txtConfNovaSenha" runat="server" TextMode="Password" SkinID="text20C"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="_rfvConfirmarSenha" runat="server" ControlToValidate="_txtConfNovaSenha"
                            ValidationGroup="AlterarSenha" ErrorMessage="Confirmar nova senha é obrigatório."
                            Display="Dynamic">*</asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="_cpvConfirmarSenha" runat="server" ControlToCompare="_txtNovaSenha"
                            ValidationGroup="AlterarSenha" ControlToValidate="_txtConfNovaSenha" ErrorMessage="Senha não confere."
                            Display="Dynamic">*</asp:CompareValidator>
                        <div class="right">
                            <asp:Button ID="_btnSalvar" runat="server" Text="Alterar senha" OnClick="_btnSalvar_Click"
                                ValidationGroup="AlterarSenha" />
                            <asp:Button ID="_btnCancelarAlterarSenha" runat="server" Text="Cancelar" CausesValidation='false'
                                OnClientClick="$('#divAlterarSenha').dialog('close');" />
                        </div>
                    </fieldset>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <fieldset id="fdsLogin" runat="server">
            <legend>Login</legend>

            <div id="msgLogin">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Login" />
            </div>

            <div class="container tipoLogin">
                <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Selected="True" Value="1">Aluno</asp:ListItem>
                    <asp:ListItem Value="2">Respons&#225;vel</asp:ListItem>
                </asp:RadioButtonList>
            </div>

            <div class="container entidade">
                <uc1:UCComboEntidade ID="UCComboEntidade1" runat="server" />
            </div>

            <div class="container usuario">
                <asp:Label ID="Label1" runat="server" Text="Usuário *" AssociatedControlID="txtLogin"
                    EnableViewState="False"></asp:Label>
                <asp:TextBox ID="txtLogin" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvLogin" runat="server" ErrorMessage="Login é obrigatório."
                    ControlToValidate="txtLogin" Display="Dynamic" ValidationGroup="Login">*</asp:RequiredFieldValidator>
            </div>

            <div class="container senha">
                <asp:Label ID="Label2" runat="server" Text="Senha *" AssociatedControlID="txtSenha"
                    EnableViewState="False"></asp:Label>
                <asp:TextBox ID="txtSenha" runat="server" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvSenha" runat="server" ErrorMessage="Senha é obrigatório."
                    ControlToValidate="txtSenha" Display="Dynamic" ValidationGroup="Login">*</asp:RequiredFieldValidator>
            </div>

            <div id="divEsqueciSenha" title="Esqueceu sua senha?" class="hide">
                <asp:UpdatePanel ID="updEsqueciSenha" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="10" AssociatedUpdatePanelID="updEsqueciSenha">
                            <ProgressTemplate>
                                <div class="loader">
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:Label ID="lblMessageEsqueciSenha" runat="server" EnableViewState="False"></asp:Label>
                        
                        <div class="sombra" ></div>
                        <fieldset>
                            <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="EsqueciSenha" />
                            <uc1:UCComboEntidade id="UCComboEntidade2" runat="server" />
                            <asp:Label ID="Label3" runat="server" Text="E-mail *" AssociatedControlID="txtEmail"></asp:Label>
                            <asp:TextBox ID="txtEmail" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                ValidationGroup="EsqueciSenha" ErrorMessage="E-mail é obrigatório." Display="Dynamic">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                ValidationGroup="EsqueciSenha" ErrorMessage="E-mail está fora do padrão ( seuEmail@seuProvedor )."
                                Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:RegularExpressionValidator>
                            <div class="right">
                                <asp:Button ID="btnEnviar" runat="server" Text="Enviar" OnClick="btnEnviar_Click"
                                    ValidationGroup="EsqueciSenha" />
                                <asp:Button ID="btnCancelar" runat="server" CausesValidation="False" Text="Cancelar"
                                    OnClientClick="$('#divEsqueciSenha').dialog('close');" />                                
                            </div>
                        </fieldset>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <asp:Button ID="btnEntrar" runat="server" Text="Entrar" ValidationGroup="Login"
                CssClass="btn" EnableViewState="False" OnClick="btnEntrar_Click"/>
            <asp:UpdatePanel ID="updLogin" runat="server">
                <ContentTemplate>
                    <asp:LinkButton ID="btnEsqueceuSenha" runat="server" CausesValidation="False" OnClick="btnEsqueceuSenha_Click"
                        TabIndex="1">Esqueceu sua senha?</asp:LinkButton>
                </ContentTemplate>
            </asp:UpdatePanel>
        </fieldset>
    </div>
</asp:Content>
