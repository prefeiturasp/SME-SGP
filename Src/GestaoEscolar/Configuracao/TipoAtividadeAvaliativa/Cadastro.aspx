<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Configuracao_TipoAtividadeAvaliativa_Cadastro" Codebehind="Cadastro.aspx.cs" %>

<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCComboGenerico.ascx" TagName="UCComboGenerico"
    TagPrefix="uc2" %>
<%@ PreviousPageType VirtualPath="~/Configuracao/TipoAtividadeAvaliativa/Busca.aspx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
    <fieldset>
        <legend>Cadastro de tipo de atividade avaliativa</legend>
        <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" />
        <asp:Label ID="lblTipoAtividadeAvaliativa" runat="server" Text="Tipo de atividade avaliativa *"
            AssociatedControlID="txtTipoAtividadeAvaliativa"></asp:Label>
        <asp:TextBox ID="txtTipoAtividadeAvaliativa" runat="server" MaxLength="100" SkinID="text60C"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvTipoAtividadeAvaliativa" runat="server" ControlToValidate="txtTipoAtividadeAvaliativa"
            Display="Dynamic" ErrorMessage="Tipo de atividade avaliativa é obrigatório.">*</asp:RequiredFieldValidator>
        <asp:Label ID="lblQualificador" Text="<%$ Resources:Configuracao, TipoAtividadeAvaliativa.Cadastro.lblQualificador.Text %>" runat="server" AssociatedControlID="ddlQualificador" Visible="false"></asp:Label>
        <asp:DropDownList ID="ddlQualificador" runat="server" AppendDataBoundItems="True" SkinID="text60C" Visible="false">
        </asp:DropDownList>
        <div align="right">
            <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CausesValidation="false"
                OnClick="btnCancelar_Click" />
        </div>
    </fieldset>
</asp:Content>
