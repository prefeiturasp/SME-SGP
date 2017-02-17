<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Configuracao_TipoJustificativaExclusaoAulas_Busca" CodeBehind="Busca.aspx.cs" %>

<%@ Register Src="../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <fieldset>
        <legend>Listagem de tipos de justificativa para exclusão de aulas</legend>
        <div></div>
        <asp:Button ID="btnNovo" runat="server" Text="<%$ Resources:Configuracao, TipoJustificativaExclusaoAulas.Busca.btnNovo.Text %>"
            OnClick="btnNovoTipoJustificativaExclusaoAulas_Click" />
        <uc2:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
        <asp:GridView ID="grvTipoJustificativaExclusaoAulas" runat="server" AutoGenerateColumns="False"
            DataKeyNames="tje_id" DataSourceID="odsTipoJustificativaExclusaoAulas" AllowPaging="True"
            EmptyDataText="Não existem tipos de justificativa para exclusão de aulas." HeaderStyle-HorizontalAlign="Center"
            OnRowCommand="grvTipoJustificativaExclusaoAulas_RowCommand" OnRowDataBound="grvTipoJustificativaExclusaoAulas_RowDataBound"
            OnDataBound="grvTipoJustificativaExclusaoAulas_DataBound">
            <Columns>
                <asp:BoundField DataField="tje_nome" HeaderText="<%$ Resources:Configuracao, TipoJustificativaExclusaoAulas.Busca.grvTipoJustificativaExclusaoAulas.tje_nome.HeaderText %>"
                    SortExpression="tje_nome" Visible="false" />
                <asp:TemplateField HeaderText="<%$ Resources:Configuracao, TipoJustificativaExclusaoAulas.Busca.grvTipoJustificativaExclusaoAulas.tje_nome.HeaderText %>">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnAlterar" runat="server" CommandName="Edit" Text='<%# Bind("tje_nome") %>'
                            PostBackUrl="~/Configuracao/TipoJustificativaExclusaoAulas/Cadastro.aspx" CssClass="wrap600px"></asp:LinkButton>
                        <asp:Label ID="lblAlterar" runat="server" Text='<%# Bind("tje_nome") %>' CssClass="wrap600px"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="tje_codigo" HeaderText="<%$ Resources:Configuracao, TipoJustificativaExclusaoAulas.Busca.grvTipoJustificativaExclusaoAulas.tje_codigo.HeaderText %>" SortExpression="tje_codigo" />
                <asp:BoundField DataField="tje_situacaoTipoJustificativaExclusaoAulas" HeaderText="<%$ Resources:Configuracao, TipoJustificativaExclusaoAulas.Busca.grvTipoJustificativaExclusaoAulas.tje_situacao.HeaderText %>"
                    SortExpression="tje_situacaoTipoJustificativaExclusaoAulas" ItemStyle-HorizontalAlign="Center">
                    <HeaderStyle CssClass="center" />
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:TemplateField HeaderText="<%$ Resources:Configuracao, TipoJustificativaExclusaoAulas.Busca.grvTipoJustificativaExclusaoAulas.btnExcluir.HeaderText %>">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnExcluir" SkinID="btExcluir" runat="server" CommandName="Deletar" />
                    </ItemTemplate>
                    <HeaderStyle CssClass="center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
        </asp:GridView>
        <uc1:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="grvTipoJustificativaExclusaoAulas" />
    </fieldset>
    <asp:ObjectDataSource ID="odsTipoJustificativaExclusaoAulas" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_TipoJustificativaExclusaoAulas"
        EnablePaging="True" MaximumRowsParameterName="PageSize" SelectCountMethod="GetTotalRecords"
        SelectMethod="SelecionaTipoJustificativaExclusaoAulas" StartRowIndexParameterName="currentPage"
        TypeName="MSTech.GestaoEscolar.BLL.ACA_TipoJustificativaExclusaoAulasBO" OnSelecting="odsTipoJustificativaExclusaoAulas_Selecting">
        <SelectParameters>
            <asp:Parameter Name="paginado" Type="Boolean" DefaultValue="true" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
