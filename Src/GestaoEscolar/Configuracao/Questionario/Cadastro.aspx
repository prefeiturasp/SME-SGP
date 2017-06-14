<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Configuracao.Questionario.Cadastro"
    ValidateRequest="false" %>
<%@ PreviousPageType VirtualPath="~/Configuracao/Questionario/Busca.aspx" %>
<%@ Register Src="../../WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc1" %>
<%@ Register Src="../../WebControls/Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="_updMessage" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="ValidationSummary" runat="server" ValidationGroup="Questionario" />
    <fieldset>
        <legend>Questionário</legend>
        <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" />
        <uc2:UCLoader ID="UCLoader1" runat="server" />
        <asp:Label ID="_lblTitulo" runat="server" Text="Título do questionário *" AssociatedControlID="_txtTitulo"></asp:Label>
        <asp:TextBox ID="_txtTitulo" runat="server" CssClass="wrap150px" SkinID="text30C"></asp:TextBox>
        <asp:RequiredFieldValidator ID="_rfvTitulo" runat="server" ErrorMessage="Título do questionário é obrigatório."
            ControlToValidate="_txtTitulo" ValidationGroup="Questionario">*</asp:RequiredFieldValidator>
        <div class="right">
            <asp:Button ID="_btnSalvar" runat="server" Text="Salvar" OnClick="_btnSalvar_Click" ValidationGroup="Questionario"/>
            <asp:Button ID="_btnCancelar" runat="server" Text="Cancelar" CausesValidation="false"
                OnClick="_btnCancelar_Click" />
        </div>
    </fieldset>
</asp:Content>
