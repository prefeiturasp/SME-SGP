<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Configuracao.ServidorRelatorios.Cadastro" %>

<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMessage" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMensagemErro" runat="server" EnableViewState="false"></asp:Label>
            <asp:ValidationSummary ID="vsSumarioErro" runat="server" ValidationGroup="vlgValidaServidorRelatorios" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <fieldset>
        <legend>Cadastro de servidor de relatórios</legend>
        <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" />
        <asp:Label ID="lblNomeServidor" runat="server" Text="Nome do servidor" AssociatedControlID="txtNomeServidor"></asp:Label>
        <asp:TextBox ID="txtNomeServidor" runat="server" MaxLength="100" CssClass="text60C"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvNomeServidor" runat="server" ControlToValidate="txtNomeServidor"
            ValidationGroup="vlgValidaServidorRelatorios" ErrorMessage="Nome do servidor é obrigatório."
            Display="Dynamic">*</asp:RequiredFieldValidator>
        <asp:Label ID="lblDescricaoServidor" runat="server" Text="Descrição" AssociatedControlID="txtDescricaoServidor"
            EnableViewState="False"></asp:Label>
        <asp:TextBox ID="txtDescricaoServidor" runat="server" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox>
        <asp:Label ID="lblLocalProcessamento" runat="server" Text="Local do Processamento" AssociatedControlID="ddlLocalProcessamento" EnableViewState="False"></asp:Label>
        <asp:DropDownList ID="ddlLocalProcessamento" runat="server" CssClass="text20C" AutoPostBack="true" OnSelectedIndexChanged="ddlLocalProcessamento_SelectedIndexChanged"></asp:DropDownList>
        <asp:Label ID="lblUsuario" runat="server" Text="Usuário" AssociatedControlID="txtUsuario"></asp:Label>
        <asp:TextBox ID="txtUsuario" runat="server" MaxLength="100" CssClass="text20C"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvUsuario" runat="server" ControlToValidate="txtUsuario"
            ValidationGroup="vlgValidaServidorRelatorios" ErrorMessage="Nome do usuário é obrigatório."
            Display="Dynamic">*</asp:RequiredFieldValidator>
        <asp:CheckBox ID="chkAlterarSenha" runat="server" Text="Alterar senha" AutoPostBack="true" OnCheckedChanged="chkAlterarSenha_CheckedChanged" />
        <asp:Label ID="lblSenha" runat="server" Text="Senha" AssociatedControlID="txtSenha" EnableViewState="False"></asp:Label>
        <asp:TextBox ID="txtSenha" runat="server" MaxLength="512" TextMode="Password" SkinID="text20C"></asp:TextBox>
        <asp:Label ID="lblConfirmarSenha" class="lblConfirmarSenha" runat="server" Text="Confirmar senha" AssociatedControlID="txtConfirmarSenha"
            EnableViewState="False"></asp:Label>
        <asp:TextBox ID="txtConfirmarSenha" runat="server" MaxLength="512" TextMode="Password" SkinID="text20C"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvSenha" runat="server" ControlToValidate="txtSenha"
            ValidationGroup="vlgValidaServidorRelatorios" ErrorMessage="Senha é obrigatório."
            Display="Dynamic">*</asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="revSenha" runat="server" ControlToValidate="txtSenha"
            ValidationGroup="vlgValidaServidorRelatorios" Display="Dynamic" ErrorMessage="A senha não pode conter espaços em branco."
            ValidationExpression="[^\s]+" Enabled="false">*</asp:RegularExpressionValidator>
        <asp:CompareValidator ID="cpvConfirmarSenha" runat="server" ControlToCompare="txtSenha"
            ValidationGroup="vlgValidaServidorRelatorios" ControlToValidate="txtConfirmarSenha" ErrorMessage="Senha não confere."
            Display="Dynamic">*</asp:CompareValidator>
        <asp:Label ID="lblDominio" runat="server" Text="Domínio" AssociatedControlID="txtDominio"></asp:Label>
        <asp:TextBox ID="txtDominio" runat="server" MaxLength="512" CssClass="text20C"></asp:TextBox>
        <asp:Label ID="lblUrlRelatorios" runat="server" Text="Url relatórios" AssociatedControlID="txtUrlRelatorios"></asp:Label>
        <asp:TextBox ID="txtUrlRelatorios" runat="server" MaxLength="100" CssClass="text60C"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvUrlRelatorios" runat="server" ControlToValidate="txtUrlRelatorios"
            ValidationGroup="vlgValidaServidorRelatorios" ErrorMessage="Url de relatórios é obrigatório."
            Display="Dynamic">*</asp:RequiredFieldValidator>
        <asp:Label ID="lblPastaRelatorios" runat="server" Text="Pasta do relatórios" AssociatedControlID="txtPastaRelatorios"></asp:Label>
        <asp:TextBox ID="txtPastaRelatorios" runat="server" MaxLength="100" CssClass="text60C"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvPastaRelatorios" runat="server" ControlToValidate="txtPastaRelatorios"
            ValidationGroup="vlgValidaServidorRelatorios" ErrorMessage="Pasta de relatórios é obrigatório."
            Display="Dynamic">*</asp:RequiredFieldValidator>
        <asp:Label ID="lblSituacao" runat="server" Text="Situação" AssociatedControlID="ddlSituacao" EnableViewState="False"></asp:Label>
        <asp:DropDownList ID="ddlSituacao" runat="server" CssClass="text20C"></asp:DropDownList>
        <br />
        <br />
        <%--Sub Fieldset de Relatórios--%>
        <fieldset>
            <legend>Selecionar relatórios do servidor</legend>
            <asp:CheckBoxList ID="chkRelatorios" runat="server" DataTextField="rlt_nome" DataValueField="rlt_ativo" AutoPostBack="false"
                RepeatColumns="3" CellSpacing="5" OnDataBound="chkRelatorios_DataBound">
            </asp:CheckBoxList>
        </fieldset>
        <div align="right">
            <asp:Button ID="btnSalvar" runat="server" Text="Salvar" CausesValidation="True" ValidationGroup="vlgValidaServidorRelatorios" OnClick="btnSalvar_Click" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CausesValidation="false" OnClick="btnCancelar_Click" />
        </div>
    </fieldset>

</asp:Content>
