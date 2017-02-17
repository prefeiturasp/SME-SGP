<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Academico_EscalaAvaliacao_Busca" CodeBehind="Busca.aspx.cs" %>

<%@ Register Src="../../WebControls/FiltroEscolas/UCFiltroEscolas.ascx" TagName="UCFiltroEscolas"
    TagPrefix="uc1" %>
<%@ Register Src="../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <fieldset id="fdsEscalaAvaliacao" runat="server">
        <legend>Consulta de escalas de avaliação</legend>
        <div id="_divPesquisa" runat="server">
            <asp:Label ID="_lblNome" runat="server" Text="Nome da escala de avaliação" AssociatedControlID="_txtNome"></asp:Label>
            <asp:TextBox ID="_txtNome" runat="server" SkinID="text60C"></asp:TextBox>
        </div>
        <div class="right">
            <asp:Button ID="_btnPesquisar" runat="server" Text="Pesquisar" OnClick="_btnPesquisar_Click" />
            <asp:Button ID="_btnLimparPesquisa" runat="server" Text="Limpar pesquisa" OnClick="_btnLimparPesquisa_Click" />
    </fieldset>
    <fieldset id="fdsResultados" runat="server">
        <legend>Resultados</legend>
        <uc3:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
        <asp:GridView ID="_dgvEscala" runat="server" AllowPaging="True" DataKeyNames="esa_id"
            AutoGenerateColumns="False" DataSourceID="odsEscala" EmptyDataText="A pesquisa não encontrou resultados."
            HeaderStyle-HorizontalAlign="Center" OnRowDataBound="_dgvEscala_RowDataBound"
            OnDataBound="_dgvEscala_DataBound" AllowSorting="true">
            <Columns>
                <asp:TemplateField HeaderText="Nome da escala de avaliação" SortExpression="esa_nome">
                    <ItemTemplate>
                        <asp:LinkButton ID="_btnAlterar" runat="server" CommandName="Edit" Text='<%# Bind("esa_nome") %>'
                            PostBackUrl="../EscalaAvaliacao/Cadastro.aspx"></asp:LinkButton>
                        <asp:Label ID="_lblAlterar" runat="server" Text='<%# Bind("esa_nome") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="esa_tipo" HeaderText="Tipo da escala" SortExpression="esa_tipo">
                    <HeaderStyle CssClass="center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="esa_situacao" HeaderText="Bloqueado" SortExpression="esa_situacao"
                    ItemStyle-HorizontalAlign="Center">
                    <HeaderStyle CssClass="center" />
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
            </Columns>
            <HeaderStyle HorizontalAlign="Center" />
        </asp:GridView>
        <uc2:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="_dgvEscala" />
    </fieldset>
    <asp:ObjectDataSource ID="odsEscala" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_EscalaAvaliacao"
        DeleteMethod="Delete" SelectMethod="SelecionaEscalaAvaliacao" TypeName="MSTech.GestaoEscolar.BLL.ACA_EscalaAvaliacaoBO"
        OnSelecting="_odsEscala_Selecting"></asp:ObjectDataSource>
</asp:Content>
