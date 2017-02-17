<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Academico_FormatoAvaliacao_Busca" Codebehind="Busca.aspx.cs" %>

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
    <fieldset id="fdsFormatoAvaliacao" runat="server">
        <legend>Consulta de formatos de avaliação</legend>
        <div id="_divPesquisa" runat="server">
            <asp:Label ID="_lblNome" runat="server" Text="Nome do formato de avaliação" AssociatedControlID="_txtNome"></asp:Label>
            <asp:TextBox ID="_txtNome" runat="server" SkinID="text60C"></asp:TextBox>
        </div>
        <div class="right">
            <asp:Button ID="_btnPesquisar" runat="server" Text="Pesquisar" OnClick="_btnPesquisar_Click" />
            <asp:Button ID="_btnLimparPesquisa" runat="server" Text="Limpar pesquisa" OnClick="_btnLimparPesquisa_Click" />
    </fieldset>
    <fieldset id="fdsResultados" runat="server">
        <legend>Resultados</legend>
        <uc3:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
        <asp:GridView ID="_dgvFormatoAvaliacao" runat="server" AllowPaging="true" DataKeyNames="fav_id"
            AutoGenerateColumns="False" DataSourceID="odsFormatoAvaliacao" EmptyDataText="A pesquisa não encontrou resultados."
            OnRowDataBound="_dgvFormatoAvaliacao_RowDataBound" 
            ondatabound="_dgvFormatoAvaliacao_DataBound" AllowSorting="true">
            <Columns>
                <asp:BoundField DataField="fav_id" HeaderText="fav_id" InsertVisible="False" Visible="false"
                    SortExpression="fav_id" ReadOnly="True" />
                <asp:TemplateField HeaderText="Nome do formato de avaliação" SortExpression="fav_nome">
                    <ItemTemplate>
                        <asp:LinkButton ID="_btnAlterar" runat="server" CommandName="Edit" Text='<%# Bind("fav_nome") %>'
                            PostBackUrl="~/Academico/FormatoAvaliacao/Cadastro.aspx"></asp:LinkButton>
                        <asp:Label ID="_lblAlterar" runat="server" Text='<%# Bind("fav_nome") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="fav_situacao" HeaderText="Bloqueado" SortExpression="fav_situacao">
                    <HeaderStyle CssClass="center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
            </Columns>
            <HeaderStyle HorizontalAlign="Center" />
        </asp:GridView>
        <uc2:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="_dgvFormatoAvaliacao" />
    </fieldset>
    <asp:ObjectDataSource ID="odsFormatoAvaliacao" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_FormatoAvaliacao"
        DeleteMethod="Delete" SelectMethod="GetSelectBy_esc_id_uni_id_fav_nome" TypeName="MSTech.GestaoEscolar.BLL.ACA_FormatoAvaliacaoBO"
        >
    </asp:ObjectDataSource>
</asp:Content>
