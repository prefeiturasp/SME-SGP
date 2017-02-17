<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Configuracao.AreaConhecimento.Cadastro" %>

<%@ PreviousPageType VirtualPath="~/Configuracao/AreaConhecimento/Busca.aspx" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="AreaConhecimento" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updCadastroQualidade" runat="server">
        <ContentTemplate>
            <fieldset>
                <legend>Cadastro de área de conhecimento</legend>
                <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios4" runat="server" />
                <asp:Label ID="lblAreaConhecimento" runat="server" Text="Área de conhecimento*"
                    AssociatedControlID="txtAreaConhecimento"></asp:Label>
                <asp:TextBox ID="txtAreaConhecimento" runat="server" MaxLength="150" SkinID="text60C"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvAreaConhecimento" runat="server" ControlToValidate="txtAreaConhecimento"
                    Display="Dynamic" ValidationGroup="AreaConhecimento" ErrorMessage="Área de conhecimento é obrigatória.">*</asp:RequiredFieldValidator>
                <asp:Label ID="lblTipoBaseGeral" runat="server" Text="Tipo base geral"
                    AssociatedControlID="ddlTipoBaseGeral"></asp:Label>
                <asp:DropDownList runat="server" ID="ddlTipoBaseGeral"></asp:DropDownList>
                <asp:Label ID="lblTipoBase" runat="server" Text="Tipo base"
                    AssociatedControlID="ddlTipoBase"></asp:Label>
                <asp:DropDownList runat="server" ID="ddlTipoBase"></asp:DropDownList>
                <div align="right">
                    <asp:Button ID="bntSalvar" runat="server" Text="Salvar" ValidationGroup="AreaConhecimento" OnClick="bntSalvar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CausesValidation="false"
                        OnClick="btnCancelar_Click" />
                </div>                
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>