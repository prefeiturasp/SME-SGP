<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="CadastroResposta.aspx.cs" Inherits="GestaoEscolar.Configuracao.Questionario.CadastroResposta" %>

<%@ PreviousPageType VirtualPath="~/Configuracao/Questionario/BuscaResposta.aspx" %>
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
    <asp:ValidationSummary ID="ValidationSummary" runat="server" ValidationGroup="Resposta" />
    <fieldset>
        <legend>Conteúdo</legend>
        <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" />
        <uc2:UCLoader ID="UCLoader1" runat="server" />
        <asp:Label ID="_lblTexto" runat="server" Text="Texto da resposta *" AssociatedControlID="_txtTexto"></asp:Label>
        <asp:TextBox ID="_txtTexto" runat="server" CssClass="wrap150px" SkinID="text30C"></asp:TextBox>
        <asp:RequiredFieldValidator ID="_rfvTexto" runat="server" ErrorMessage="Texto da resposta é obrigatório."
            ControlToValidate="_txtTexto" ValidationGroup="Resposta">*</asp:RequiredFieldValidator>
        <br />
        <asp:CheckBox ID="_chkPermiteAdicionarTexto" runat="server" Text="Permite adicionar texto"/>
        <div class="right">
            <asp:Button ID="_btnSalvar" runat="server" Text="Salvar" OnClick="_btnSalvar_Click" ValidationGroup="Resposta" />
            <asp:Button ID="_btnCancelar" runat="server" Text="Cancelar" CausesValidation="false"
                OnClick="_btnCancelar_Click" />
        </div>
    </fieldset>
</asp:Content>
