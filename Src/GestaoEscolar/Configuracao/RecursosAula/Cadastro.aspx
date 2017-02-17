<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Configuracao.RecursosAula.Cadastro" %>

<%@ PreviousPageType VirtualPath="~/Configuracao/RecursosAula/Busca.aspx" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
    <fieldset>
        <legend>Cadastro de recurso de aula</legend>
        <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" />
        <asp:Label ID="_lblTipoMovimentacao" runat="server" Text="Recurso de aula *" AssociatedControlID="txtRecursoNome"></asp:Label>
        <asp:TextBox ID="txtRecursoNome" runat="server" MaxLength="100" SkinID="text60C"></asp:TextBox>
        <asp:RequiredFieldValidator ID="_rfvTipoMovimentacao" runat="server" ControlToValidate="txtRecursoNome"
            Display="Dynamic" ErrorMessage="Recurso de aula é obrigatório.">*</asp:RequiredFieldValidator>
        <div align="right">
            <asp:Button ID="_btnSalvar" runat="server" Text="Salvar" OnClick="_btnSalvar_Click" />
            <asp:Button ID="_btnCancelar" runat="server" Text="Cancelar" CausesValidation="false"
                OnClick="_btnCancelar_Click" />
        </div>
    </fieldset>
</asp:Content>
