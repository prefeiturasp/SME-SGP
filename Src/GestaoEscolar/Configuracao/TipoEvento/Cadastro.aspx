<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Configuracao_TipoEvento_Cadastro" Codebehind="Cadastro.aspx.cs" %>

<%@ PreviousPageType VirtualPath="~/Configuracao/TipoEvento/Busca.aspx" %>
<%@ Register Src="../../WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
    <fieldset>
        <legend>Cadastro de tipo de evento</legend>
        <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios3" runat="server" />
        <asp:Label ID="_lblTipoEvento" runat="server" Text="Tipo de evento *" AssociatedControlID="_txtTipoEvento" />
        <asp:TextBox ID="_txtTipoEvento" runat="server" SkinID="text60C" MaxLength="200"></asp:TextBox>
        <asp:RequiredFieldValidator ID="_rfvTipoEvento" runat="server" ControlToValidate="_txtTipoEvento"
            Display="Dynamic" ErrorMessage="Tipo de evento é obrigatório." Text="*" />
        <asp:CheckBox ID="_ckbPeriodoCalendario" runat="server" Text="Relacionado a um tipo de período de calendário" />
        <asp:RadioButtonList ID="_rdlLiberacao" runat="server">
            <asp:ListItem Value="1" Text="Não exibir na liberação de eventos" />
            <asp:ListItem Value="2" Text="Exibir na liberação de eventos e não ser obrigatório" />
            <asp:ListItem Value="3" Text="Obrigatório ter liberação de evento" />
        </asp:RadioButtonList>
        <asp:RequiredFieldValidator ID="_rfvLiberacao" runat="server" ControlToValidate="_rdlLiberacao"
            Display="Dynamic" Text="*" ErrorMessage="É necessário selecionar a configuração de liberação para cadastro de eventos deste tipo." />
        <asp:CheckBox ID="_ckbBloqueado" runat="server" Visible="False" Text="Bloqueado" />
        <div class="right">
            <asp:Button ID="_bntSalvar" runat="server" Text="Salvar" OnClick="_bntSalvar_Click" />
            <asp:Button ID="_btnCancelar" runat="server" Text="Cancelar" CausesValidation="false"
                OnClick="_btnCancelar_Click" />
        </div>
    </fieldset>
</asp:Content>
