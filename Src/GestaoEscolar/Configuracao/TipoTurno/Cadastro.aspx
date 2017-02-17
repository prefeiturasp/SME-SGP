<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Configuracao_TipoTurno_Cadastro" CodeBehind="Cadastro.aspx.cs" %>

<%@ PreviousPageType VirtualPath="~/Configuracao/TipoTurno/Busca.aspx" %>
<%@ Register Src="../../WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc36" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
    <fieldset>
        <legend>Cadastro de tipo de turno</legend>
        <uc36:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" />
        <asp:Label ID="_lblTipoTurno" runat="server" Text="Tipo de turno *" AssociatedControlID="_txtTipoTurno"></asp:Label>
        <asp:TextBox ID="_txtTipoTurno" runat="server" MaxLength="100" SkinID="text60C" Enabled="false"></asp:TextBox>
        <asp:RequiredFieldValidator ID="_rfvTipoTurno" runat="server" ControlToValidate="_txtTipoTurno"
            ErrorMessage="Tipo de turno é obrigatório." Display="Dynamic">*</asp:RequiredFieldValidator>
        <asp:Label ID="_lblTipo" runat="server" Text="Tipo" AssociatedControlID="_ddlTipoTurno"></asp:Label>
        <asp:DropDownList ID="_ddlTipoTurno" runat="server" Enabled="false">
            <asp:ListItem Value="-1" Text="-- Selecione o tipo do turno --"></asp:ListItem>
            <asp:ListItem Value="1" Text="Manhã"></asp:ListItem>
            <asp:ListItem Value="2" Text="Tarde"></asp:ListItem>
            <asp:ListItem Value="3" Text="Noite"></asp:ListItem>
            <asp:ListItem Value="4" Text="Integral"></asp:ListItem>
            <asp:ListItem Value="5" Text="Intermediário"></asp:ListItem>
        </asp:DropDownList>
        <asp:CheckBox ID="_ckbBloqueado" runat="server" Text="Bloqueado" Enabled="false"/>
        <div align="right">
            <asp:Button ID="_btnCancelar" runat="server" Text="Voltar" OnClick="_btnCancelar_Click"
                CausesValidation="false" />
        </div>
    </fieldset>
</asp:Content>