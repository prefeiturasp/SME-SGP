<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Configuracao_TipoJustificativaFalta_Busca" CodeBehind="Busca.aspx.cs" %>

<%@ Register Src="../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <fieldset>
        <legend>Listagem de tipos de justificativa de falta</legend>
        <div></div>
        <asp:Button ID="btnNovo" runat="server" Text="Incluir novo tipo de justificativa de falta"
            OnClick="btnNovoTipoJustificativaFalta_Click" />
        <uc2:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
        <asp:GridView ID="grvTipoJustificativaFalta" runat="server" AutoGenerateColumns="False"
            DataKeyNames="tjf_id" DataSourceID="odsTipoJustificativaFalta" AllowPaging="True"
            EmptyDataText="Não existem tipos de justificativa de falta." HeaderStyle-HorizontalAlign="Center"
            OnRowCommand="grvTipoJustificativaFalta_RowCommand" OnRowDataBound="grvTipoJustificativaFalta_RowDataBound"
            OnDataBound="grvTipoJustificativaFalta_DataBound">
            <Columns>
                <asp:BoundField DataField="tjf_nome" HeaderText="Tipo de justificativa de falta"
                    SortExpression="tjf_nome" Visible="false" />
                <asp:TemplateField HeaderText="Tipo de justificativa de falta">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnAlterar" runat="server" CommandName="Edit" Text='<%# Bind("tjf_nome") %>'
                            PostBackUrl="~/Configuracao/TipoJustificativaFalta/Cadastro.aspx" CssClass="wrap600px"></asp:LinkButton>
                        <asp:Label ID="lblAlterar" runat="server" Text='<%# Bind("tjf_nome") %>' CssClass="wrap600px"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="tjf_codigo" HeaderText="Código" SortExpression="tjf_codigo" />
                <asp:BoundField DataField="tjf_abonaFaltaDesc" HeaderText="Abona faltas" SortExpression="tjf_abonaFaltaDesc"
                    ItemStyle-HorizontalAlign="Center">
                    <HeaderStyle CssClass="center" />
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="tjf_situacaoTipoJustificativaFalta" HeaderText="Situação"
                    SortExpression="tjf_situacaoTipoJustificativaFalta" ItemStyle-HorizontalAlign="Center">
                    <HeaderStyle CssClass="center" />
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:TemplateField HeaderText="Excluir">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnExcluir" SkinID="btExcluir" runat="server" CommandName="Deletar" />
                    </ItemTemplate>
                    <HeaderStyle CssClass="center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
        </asp:GridView>
        <uc1:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="grvTipoJustificativaFalta" />
    </fieldset>
    <asp:ObjectDataSource ID="odsTipoJustificativaFalta" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_TipoJustificativaFalta"
        EnablePaging="True" MaximumRowsParameterName="PageSize" SelectCountMethod="GetTotalRecords"
        SelectMethod="SelecionaTipoJustificativaFalta" StartRowIndexParameterName="currentPage"
        TypeName="MSTech.GestaoEscolar.BLL.ACA_TipoJustificativaFaltaBO" OnSelecting="odsTipoJustificativaFalta_Selecting">
        <SelectParameters>
            <asp:Parameter Name="paginado" Type="Boolean" DefaultValue="true" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
