<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Academico.Areas.Cadastro" %>

<%@ PreviousPageType VirtualPath="~/Academico/Areas/Busca.aspx" %>

<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblMensagem" runat="server" EnableViewState="false"></asp:Label>
    <asp:ValidationSummary ID="ValidationSummary" runat="server" ValidationGroup="Area" />
    <asp:Panel ID="pnlCadastro" runat="server" GroupingText="<%$ Resources:Academico, Areas.Cadastro.pnlCadastro.GroupingText %>">
        <div class="area-form">
            <asp:Label ID="lblMsgInfo" runat="server"></asp:Label>
            <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" />
            <asp:Label ID="lblNome" runat="server" Text="<%$ Resources:Academico, Areas.Cadastro.lblNome.Text %>" AssociatedControlID="txtNome"></asp:Label>
            <asp:TextBox ID="txtNome" runat="server" SkinID="text60C"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvNome" runat="server" ControlToValidate="txtNome" Display="Dynamic" ValidationGroup="Area"
                ErrorMessage="<%$ Resources:Academico, Areas.Cadastro.rfvNome.ErrorMessage %>">*</asp:RequiredFieldValidator>
            <asp:CheckBox ID="chkCadastroEscola" runat="server" Text="<%$ Resources:Academico, Areas.Cadastro.chkCadastroEscola.Text %>" />
        </div>
        <div class="right area-botoes-bottom">
            <asp:Button ID="btnSalvar" runat="server" Text="<%$ Resources:Academico, Areas.Cadastro.btnSalvar.Text %>" OnClick="btnSalvar_Click" ValidationGroup="Area" />
            <asp:Button ID="btnCancelar" runat="server" Text="<%$ Resources:Academico, Areas.Cadastro.btnCancelar.Text %>" OnClick="btnCancelar_Click" CausesValidation="false" />
        </div>
    </asp:Panel>
</asp:Content>