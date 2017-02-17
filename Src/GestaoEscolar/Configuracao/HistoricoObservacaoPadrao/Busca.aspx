<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Configuracao_HistoricoObservacaoPadrao_Busca" CodeBehind="Busca.aspx.cs" %>

<%@ Register Src="../../WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <fieldset>
        <legend>Listagem de observações do histórico escolar</legend>
        <div>
            <asp:Button ID="_btnNovo" runat="server" Text="Incluir nova observação do histórico escolar"
                OnClick="_btnNovo_Click" />
        </div>
        <div>
            <div align="right">
                <uc1:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
            </div>
            <asp:GridView ID="_dgvHistoricoEscolar" runat="server" AutoGenerateColumns="False"
                DataKeyNames="hop_id" AllowPaging="True" DataSourceID="_odsHistoricoEscolar"
                OnRowCommand="_dgvHistoricoEscolar_RowCommand" EmptyDataText="Não existem observações do histórico escolar cadastradas."
                OnRowDataBound="_dgvHistoricoEscolar_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Nome da observação">
                        <ItemTemplate>
                            <asp:LinkButton ID="_btnAlterar" runat="server" Text='<%# Eval("hop_nome") %>' PostBackUrl="~/Configuracao/HistoricoObservacaoPadrao/Cadastro.aspx"
                                CommandName="Edit">
                            </asp:LinkButton>
                            <asp:Label ID="_lblAlterar" runat="server" Text='<%# Eval("hop_nome") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="hop_descricao" HeaderText="Descrição da observação" SortExpression="hop_descricao"
                        ItemStyle-HorizontalAlign="Left">
                        <HeaderStyle CssClass="Left" />
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="hop_tipo" HeaderText="Tipo de observação" SortExpression="hop_tipo"
                        ItemStyle-HorizontalAlign="Left">
                        <HeaderStyle CssClass="Left" />
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Excluir" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <asp:ImageButton ID="_btnExcluir" SkinID="btExcluir" runat="server" CommandName="Deletar" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="_odsHistoricoEscolar" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_HistoricoObservacaoPadrao"
                DeleteMethod="Delete" OldValuesParameterFormatString="original_{0}" SelectMethod="SelecionaTodosGrid"
                TypeName="MSTech.GestaoEscolar.BLL.ACA_HistoricoObservacaoPadraoBO" UpdateMethod="Save"
                EnablePaging="True" MaximumRowsParameterName="pageSize" SelectCountMethod="GetTotalRecords"
                StartRowIndexParameterName="currentPage" OnSelecting="_odsHistoricoEscolar_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="paginado" Type="Boolean" DefaultValue="true" />
                    <asp:Parameter Name="currentPage" Type="Int32" DefaultValue="1" />
                    <asp:Parameter Name="pageSize" Type="Int32" DefaultValue="1" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </fieldset>
</asp:Content>
