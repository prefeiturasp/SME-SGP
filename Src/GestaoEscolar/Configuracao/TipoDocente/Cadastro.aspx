<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Configuracao_TipoDocente_Cadastro" CodeBehind="Cadastro.aspx.cs" %>

<%@ PreviousPageType VirtualPath="~/Configuracao/TipoDocente/Busca.aspx" %>
<%@ Register Src="../../WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc36" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="grpTipoDocente" />

    <fieldset>
        <legend>Cadastro de tipo de docente</legend>
        <uc36:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" />

        <asp:Label ID="lblTipoDocente" runat="server" Text="Tipo de docente *" AssociatedControlID="ddlTipoDocente"></asp:Label>
        <asp:DropDownList ID="ddlTipoDocente" runat="server" />
        <asp:RequiredFieldValidator ID="rfvTipoDocente" runat="server" ControlToValidate="ddlTipoDocente" ValidationGroup="grpTipoDocente"
            ErrorMessage="Tipo de docente é obrigatório." Display="Dynamic">*</asp:RequiredFieldValidator>

        <asp:Label ID="lblDescricao" runat="server" Text="Descrição *" AssociatedControlID="txtDescricao"></asp:Label>
        <asp:TextBox ID="txtDescricao" runat="server" MaxLength="100" SkinID="text60C"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvDescricao" runat="server" ControlToValidate="txtDescricao"
            ValidationGroup="grpTipoDocente" Display="Dynamic" ErrorMessage="Campo descrição é obrigatório.">*</asp:RequiredFieldValidator>
        
        <asp:Label ID="lblNome" runat="server" Text="Nome *" AssociatedControlID="txtNome"></asp:Label>
        <asp:TextBox ID="txtNome" runat="server" MaxLength="100" SkinID="text30C"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvNome" runat="server" ControlToValidate="txtNome"
            ValidationGroup="grpTipoDocente" Display="Dynamic" ErrorMessage="Campo nome é obrigatório.">*</asp:RequiredFieldValidator>

        <asp:Label ID="lblPosicao" runat="server" Text="Posição *" AssociatedControlID="txtPosicao"></asp:Label>
        <asp:TextBox ID="txtPosicao" runat="server" MaxLength="1" SkinID="text10C"></asp:TextBox>
        <asp:RequiredFieldValidator ID="_rfvPosicao" runat="server" ControlToValidate="txtPosicao"
            ValidationGroup="grpTipoDocente" Display="Dynamic" ErrorMessage="Campo posição é obrigatório.">*</asp:RequiredFieldValidator>

        <asp:Label ID="lblCorDestaque" runat="server" Text="Cor" AssociatedControlID="txtCorDestaque"></asp:Label>
        <asp:TextBox ID="txtCorDestaque" runat="server" MaxLength="10" SkinID="text10C"></asp:TextBox>

        <div align="right">
            <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click"
                CausesValidation="false" />
        </div>

    </fieldset>
</asp:Content>
