<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="CadastroConteudo.aspx.cs" Inherits="GestaoEscolar.Configuracao.Questionario.CadastroConteudo" %>

<%@ PreviousPageType VirtualPath="~/Configuracao/Questionario/BuscaConteudo.aspx" %>
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
    <asp:ValidationSummary ID="ValidationSummary" runat="server" ValidationGroup="Conteudo" />
    <fieldset>
        <legend>Conteúdo</legend>
        <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" />
        <uc2:UCLoader ID="UCLoader1" runat="server" />
        <asp:Label ID="_lblTexto" runat="server" Text="Texto do conteúdo *" AssociatedControlID="_txtTexto"></asp:Label>
        <asp:TextBox ID="_txtTexto" runat="server" CssClass="wrap150px" SkinID="text30C"></asp:TextBox>
        <asp:RequiredFieldValidator ID="_rfvTexto" runat="server" ErrorMessage="Texto do conteúdo é obrigatório."
            ControlToValidate="_txtTexto" ValidationGroup="Conteudo">*</asp:RequiredFieldValidator>
        <asp:Label ID="lblTipoConteudo" runat="server" Text="Tipo de conteúdo *" AssociatedControlID="_ddlTipoConteudo"></asp:Label>
        <asp:DropDownList ID="_ddlTipoConteudo" runat="server" CssClass="wrap150px" 
            OnSelectedIndexChanged="_ddlTipoConteudo_SelectedIndexChanged" AutoPostBack="true">
            <asp:ListItem Text="-- Selecione --" Value="0"></asp:ListItem>
            <asp:ListItem Text="Título 1" Value="1"></asp:ListItem>
            <asp:ListItem Text="Título 2" Value="2"></asp:ListItem>
            <asp:ListItem Text="Texto" Value="3"></asp:ListItem>
            <asp:ListItem Text="Pergunta" Value="4"></asp:ListItem>
        </asp:DropDownList>
        <asp:CompareValidator ID="_cpvTipoConteudo" runat="server" ErrorMessage="Tipo de conteúdo é obrigatório."
            ControlToValidate="_ddlTipoConteudo" Operator="GreaterThan" ValueToCompare="0"
            Display="Dynamic" ValidationGroup="Conteudo">*</asp:CompareValidator>
        <asp:Label ID="lblTipoResposta" runat="server" Text="Tipo de resposta" AssociatedControlID="_ddlTipoResposta"></asp:Label>
        <asp:DropDownList ID="_ddlTipoResposta" runat="server" CssClass="wrap150px" Enabled="false" AppendDataBoundItems="True">
            <asp:ListItem Text="-- Selecione --" Value="0"></asp:ListItem>
            <asp:ListItem Text="Múltipla seleção" Value="1"></asp:ListItem>
            <asp:ListItem Text="Seleção única" Value="2"></asp:ListItem>
            <asp:ListItem Text="Texto aberto" Value="3"></asp:ListItem>
        </asp:DropDownList>
        <asp:CompareValidator ID="_cpvTipoResposta" runat="server" ErrorMessage="Tipo de resposta é obrigatório."
            ControlToValidate="_ddlTipoResposta" Operator="GreaterThan" ValueToCompare="0"
            Display="Dynamic" ValidationGroup="Conteudo" Visible="false">*</asp:CompareValidator>
        <div class="right">
            <asp:Button ID="_btnSalvar" runat="server" Text="Salvar" OnClick="_btnSalvar_Click" ValidationGroup="Conteudo" />
            <asp:Button ID="_btnCancelar" runat="server" Text="Cancelar" CausesValidation="false"
                OnClick="_btnCancelar_Click" />
        </div>
    </fieldset>
</asp:Content>
