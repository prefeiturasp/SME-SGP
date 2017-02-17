<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Configuracao.FilaFechamento.Busca" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblMensagem" runat="server" EnableViewState="false"></asp:Label>
    <asp:Panel ID="pnlConsultaFila" runat="server" GroupingText="Consulta de fila de fechamento">
        <asp:GridView ID="grvQtFila" runat="server" AutoGenerateColumns="true" EmptyDataText="Não existem registros na fila."></asp:GridView>
        <div class="right">
            <asp:Button ID="btnAtualizar" runat="server" Text="Atualizar" OnClick="btnAtualizar_Click" />
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlConsultaExecucoes" runat="server" GroupingText="Consulta de execuções">
        <asp:CheckBox ID="chkSomenteCompleta" runat="server" Text="Somente execuções completas" />
        <asp:Label ID="lbl1" runat="server" Text="Quantidade de linhas" AssociatedControlID="txtQtdeRegistros"></asp:Label>
        <asp:TextBox ID="txtQtdeRegistros" runat="server" Text="20" SkinID="Numerico"></asp:TextBox>
        <asp:GridView ID="grvQtLogs" runat="server" AutoGenerateColumns="true" EmptyDataText="Não foram encontradas execuções."></asp:GridView>
    </asp:Panel>
</asp:Content>
