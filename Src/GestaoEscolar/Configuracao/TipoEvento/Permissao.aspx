<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Permissao.aspx.cs" Inherits="GestaoEscolar.Configuracao.TipoEvento.Permissao" %>

<%@ PreviousPageType VirtualPath="~/Configuracao/TipoEvento/Busca.aspx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <legend>Permissões do tipo de evento</legend>

        <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
        <asp:Label ID="lblMensagem" runat="server"></asp:Label>
        <asp:CheckBoxList ID="cblGrupos" RepeatColumns="5" RepeatDirection="Vertical" runat="server"></asp:CheckBoxList>

        <div class="right">
            <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"/>
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CausesValidation="false"
                OnClick="btnCancelar_Click" />
        </div>

    </fieldset>
</asp:Content>
