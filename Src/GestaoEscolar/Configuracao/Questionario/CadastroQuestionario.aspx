<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="CadastroQuestionario.aspx.cs" Inherits="GestaoEscolar.Configuracao.Questionario.Cadastro"
    ValidateRequest="false" %>
<%@ PreviousPageType VirtualPath="~/Configuracao/Questionario/BuscaQuestionario.aspx" %>
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
    <asp:ValidationSummary ID="ValidationSummary" runat="server" ValidationGroup="Questionario" />
    <fieldset>
        <legend>Questionário</legend>
        <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" />
        <uc2:UCLoader ID="UCLoader1" runat="server" />
        <asp:Label ID="_lblTitulo" runat="server" Text="Título do questionário *" AssociatedControlID="_txtTitulo"></asp:Label>
        <asp:TextBox ID="_txtTitulo" runat="server" CssClass="wrap150px" SkinID="limite500" MaxLength="500"></asp:TextBox>
        <asp:RequiredFieldValidator ID="_rfvTitulo" runat="server" ErrorMessage="Título do questionário é obrigatório."
            ControlToValidate="_txtTitulo" ValidationGroup="Questionario">*</asp:RequiredFieldValidator>
        <asp:Label ID="lblTipoCalculo" runat="server" Text="Tipo de cálculo *" AssociatedControlID="_ddlTipoCalculo"></asp:Label>
        <asp:DropDownList ID="_ddlTipoCalculo" runat="server"  
            OnSelectedIndexChanged="_ddlTipoCalculo_SelectedIndexChanged" AutoPostBack="true">
            <asp:ListItem Text="-- Selecione --" Value="0"></asp:ListItem>
            <asp:ListItem Text="Sem cálculo" Value="1"></asp:ListItem>
            <asp:ListItem Text="Soma" Value="2"></asp:ListItem>
        </asp:DropDownList>
        <div runat="server" id="divCalculo" visible="false">
            <asp:Label ID="lblTituloCalculo" runat="server" Text="Título do cálculo *" AssociatedControlID="_txtTituloCalculo"></asp:Label>
            <asp:TextBox ID="_txtTituloCalculo" runat="server" CssClass="wrap150px" SkinID="limite500" MaxLength="500"></asp:TextBox>
            <asp:RequiredFieldValidator ID="_rfvTituloCalculo" runat="server" ErrorMessage="Título do cálculo é obrigatório."
            ControlToValidate="_txtTituloCalculo" ValidationGroup="Questionario" Visible="false">*</asp:RequiredFieldValidator>
        </div>
        <asp:CompareValidator ID="_cpvTipoCalculo" runat="server" ErrorMessage="Tipo de cálculo é obrigatório."
            ControlToValidate="_ddlTipoCalculo" Operator="GreaterThan" ValueToCompare="0"
            Display="Dynamic" ValidationGroup="Questionario">*</asp:CompareValidator>
        <div class="right">
            <asp:Button ID="_btnSalvar" runat="server" Text="Salvar" OnClick="_btnSalvar_Click" ValidationGroup="Questionario"/>
            <asp:Button ID="_btnCancelar" runat="server" Text="Cancelar" CausesValidation="false"
                OnClick="_btnCancelar_Click" />
        </div>
    </fieldset>
</asp:Content>
