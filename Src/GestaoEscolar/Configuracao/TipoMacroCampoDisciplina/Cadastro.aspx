<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Configuracao.TipoMacroCampoDisciplina.Cadastro" %>

<%@ PreviousPageType VirtualPath="~/Configuracao/TipoMacroCampoDisciplina/Busca.aspx" %>
<%@ Register Src="../../WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
    <fieldset>
        <legend><asp:Label runat="server" ID="lblLegend" Text="<%$ Resources:Configuracao, TipoMacroCampoDisciplina.Cadastro.lblLegend.Text %>"></asp:Label> eletivo(a)</legend>
        <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" />
        <asp:Label ID="lblTipoMacroCampo" runat="server" Text="<%$ Resources:Configuracao, TipoMacroCampoDisciplina.Cadastro.lblTipoMacroCampo.Text %>"
            AssociatedControlID="txtTipoMacroCampo"></asp:Label>
        <asp:TextBox ID="txtTipoMacroCampo" runat="server" SkinID="text60C" MaxLength="100"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvTipoMacroCampo" runat="server"
            Display="Dynamic" ControlToValidate="txtTipoMacroCampo">*</asp:RequiredFieldValidator>
        <asp:Label ID="lblSigla" runat="server" Text="Sigla *" AssociatedControlID="txtSigla"></asp:Label>
        <asp:TextBox ID="txtSigla" runat="server" MaxLength="10"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvSigla" runat="server" ErrorMessage="Sigla é obrigatório."
            Display="Dynamic" ControlToValidate="txtSigla">*</asp:RequiredFieldValidator>
        <div class="right">
            <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CausesValidation="false"
                OnClick="btnCancelar_Click" />
        </div>
    </fieldset>
</asp:Content>
