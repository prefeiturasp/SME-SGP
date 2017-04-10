<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Academico.Sondagem.Cadastro" %>

<%@ PreviousPageType VirtualPath="~/Academico/Sondagem/Busca.aspx" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
    <fieldset>
        <legend><asp:Label runat="server" ID="lblLegend" Text="<%$ Resources:Academico, Sondagem.Cadastro.lblLegend.Text %>" /></legend>
        <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios3" runat="server" />
        <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:Academico, Sondagem.Cadastro.lblTitulo.Text %>" AssociatedControlID="txtTitulo" />
        <asp:TextBox ID="txtTitulo" runat="server" SkinID="text60C" MaxLength="200"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvTitulo" runat="server" ControlToValidate="txtTitulo"
            Display="Dynamic" ErrorMessage="<%$ Resources:Academico, Sondagem.Cadastro.rfvTitulo.ErrorMessage %>" Text="*" />
        <asp:Label ID="lblDescricao" runat="server" Text="<%$ Resources:Academico, Sondagem.Cadastro.lblDescricao.Text %>" AssociatedControlID="txtDescricao" />
        <asp:TextBox ID="txtDescricao" runat="server" SkinID="text60C" MaxLength="200"></asp:TextBox>
        <asp:CheckBox ID="ckbBloqueado" runat="server" Visible="False" Text="Bloqueado" />
        <div class="right">
            <asp:Button ID="bntSalvar" runat="server" Text="<%$ Resources:Academico, Sondagem.Cadastro.bntSalvar.Text %>" OnClick="bntSalvar_Click" />
            <asp:Button ID="btnCancelar" runat="server" Text="<%$ Resources:Academico, Sondagem.Cadastro.btnCancelar.Text %>" CausesValidation="false"
                OnClick="btnCancelar_Click" />
        </div>
    </fieldset>
</asp:Content>