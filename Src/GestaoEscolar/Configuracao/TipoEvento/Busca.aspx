<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Configuracao_TipoEvento_Busca" CodeBehind="Busca.aspx.cs" %>

<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <fieldset>
        <legend>Listagem de tipos de evento</legend>
        <div>
            <div>
                <asp:Button ID="_btnNovo" runat="server" Text="Incluir novo tipo de evento" OnClick="_btnNovo_Click" />
            </div>
        </div>
        <div>
            <uc1:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
            <asp:GridView ID="_dgvTipoEvento" runat="server" AutoGenerateColumns="False" DataKeyNames="tev_id"
                DataSourceID="_odsTipoEvento" AllowPaging="True" EmptyDataText="Não existem tipos de eventos cadastrados."
                HeaderStyle-HorizontalAlign="Center" OnRowCommand="_dgvTipoEvento_RowCommand"
                OnRowDataBound="_dgvTipoEvento_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="tev_nome" HeaderText="Tipo de evento" SortExpression="tev_nome"
                        Visible="false" />
                    <asp:TemplateField HeaderText="Tipo de evento">
                        <ItemTemplate>
                            <asp:LinkButton ID="_btnAlterar" runat="server" CommandName="Edit" Text='<%# Bind("tev_nome") %>'
                                PostBackUrl="~/Configuracao/TipoEvento/Cadastro.aspx" CssClass="wrap600px"></asp:LinkButton>
                            <asp:Label ID="_lblAlterar" runat="server" Text='<%# Bind("tev_nome") %>' CssClass="wrap600px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="tev_periodoCalendario" HeaderText="Relacionado a um tipo de período de calendário"
                        SortExpression="tev_periodoCalendario" ItemStyle-HorizontalAlign="Center">
                        <HeaderStyle CssClass="center" />
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <%--<asp:BoundField DataField="tev_liberacao" HeaderText="Liberação de eventos" SortExpression="tev_liberacao"
                        ItemStyle-HorizontalAlign="Center">
                        <HeaderStyle CssClass="center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>--%>
                    <asp:BoundField DataField="tev_situacao" HeaderText="Bloqueado" SortExpression="tev_situacao"
                        ItemStyle-HorizontalAlign="Center">
                        <HeaderStyle CssClass="center" />
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Permissões">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnPermissoes" SkinID="btDetalhar" runat="server" PostBackUrl="~/Configuracao/TipoEvento/Permissao.aspx"
                                CommandName="Edit" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Excluir">
                        <ItemTemplate>
                            <asp:ImageButton ID="_btnExcluir" SkinID="btExcluir" runat="server" CommandName="Deletar" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </asp:GridView>
        </div>
    </fieldset>
    <asp:ObjectDataSource ID="_odsTipoEvento" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_TipoEvento"
        EnablePaging="True" MaximumRowsParameterName="PageSize" SelectCountMethod="GetTotalRecords"
        SelectMethod="SelecionaTipoEventoPaginado" StartRowIndexParameterName="currentPage"
        TypeName="MSTech.GestaoEscolar.BLL.ACA_TipoEventoBO" OnSelecting="_odsTipoEvento_Selecting">
        <SelectParameters>
            <asp:Parameter Name="tev_situacao" Type="Byte" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
