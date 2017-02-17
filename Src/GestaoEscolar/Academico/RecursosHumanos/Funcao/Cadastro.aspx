<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Academico_RecursosHumanos_Funcao_Cadastro" Title="Untitled Page" CodeBehind="Cadastro.aspx.cs" %>

<%@ PreviousPageType VirtualPath="~/Academico/RecursosHumanos/Funcao/Busca.aspx" %>
<%@ Register Src="../../../WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc1" %>
<%@ Register Src="../../../WebControls/Combos/UCComboParametroGrupoPerfil.ascx" TagName="UCComboParametroGrupoPerfil"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
    <fieldset>
        <legend>Cadastro de funções</legend>
        <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" />
        <asp:Label ID="_lblFuncao" runat="server" Text="Nome da função *" AssociatedControlID="_txtFuncao"></asp:Label>
        <asp:TextBox ID="_txtFuncao" runat="server" SkinID="text60C" MaxLength="100"></asp:TextBox>
        <asp:RequiredFieldValidator ID="_rfvFuncao" runat="server" ControlToValidate="_txtFuncao"
            ErrorMessage="Nome da função é obrigatório." Display="Dynamic">*</asp:RequiredFieldValidator>
        <asp:Label ID="_lblCodigo" runat="server" Text="Código da função" AssociatedControlID="_txtCodigo"></asp:Label>
        <asp:TextBox ID="_txtCodigo" runat="server" SkinID="text10C" MaxLength="20"></asp:TextBox>
        <asp:Label ID="_lblDescricao" runat="server" Text="Descrição" AssociatedControlID="_txtDescricao"></asp:Label>
        <asp:TextBox ID="_txtDescricao" runat="server" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox>
        <asp:Label ID="_lblCodIntegracao" runat="server" Text="Código de integração" AssociatedControlID="_txtCodIntegracao"></asp:Label>
        <asp:TextBox ID="_txtCodIntegracao" runat="server" SkinID="text10C" MaxLength="20"></asp:TextBox>
        <uc2:UCComboParametroGrupoPerfil ID="UCComboParametroGrupoPerfil1" runat="server" />
        <asp:CheckBox ID="_ckbBloqueado" Text="Bloqueado" runat="server" />
        <div class="right">
            <asp:Button ID="_btnSalvar" runat="server" Text="Salvar" OnClick="_btnSalvar_Click" />
            <asp:Button ID="_btnCancelar" runat="server" Text="Cancelar" OnClick="_btnCancelar_Click"
                CausesValidation="false" /></div>
    </fieldset>
</asp:Content>
