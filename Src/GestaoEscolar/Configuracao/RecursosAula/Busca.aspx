<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Configuracao.RecursosAula.Busca" %>

<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <fieldset>
        <legend>Listagem de recursos de aula</legend>
        <div>
            <asp:Button ID="_btnNovo" runat="server" Text="Incluir novo recurso de aula" OnClick="_btnNovo_Click" />
        </div>
        <div>
            <uc1:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
            <asp:GridView ID="dgvRecurso" runat="server" AutoGenerateColumns="False" DataKeyNames="rsa_id"
                DataSourceID="odsDados" AllowPaging="True" OnRowCommand="dgvRecurso_RowCommand"
                EmptyDataText="Não existem recursos de aula cadastrados." OnRowDataBound="dgvRecurso_RowDataBound"
                EnableModelValidation="True">
                <Columns>
                    <asp:TemplateField HeaderText="Recursos de aula">
                        <ItemTemplate>
                            <asp:LinkButton ID="_btnAlterar" runat="server" Text='<%# Eval("rsa_nome") %>' PostBackUrl="~/Configuracao/RecursosAula/Cadastro.aspx"
                                CommandName="Edit">
                            </asp:LinkButton>
                            <asp:Label ID="_lblAlterar" runat="server" Text='<%# Eval("rsa_nome") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
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
            <asp:ObjectDataSource ID="odsDados" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_RecursosAula"
                SelectMethod="GetRecursoAulaBy_All_Paginado" TypeName="MSTech.GestaoEscolar.BLL.ACA_RecursosAulaBO"
                EnablePaging="True" MaximumRowsParameterName="pageSize" SelectCountMethod="GetTotalRecords"
                StartRowIndexParameterName="currentPage" OnSelecting="odsDados_Selecting" DeleteMethod="Delete"
                OldValuesParameterFormatString="original_{0}" UpdateMethod="Save">
                <DeleteParameters>
                    <asp:Parameter Name="entity" Type="Object" />
                    <asp:Parameter Name="banco" Type="Object" />
                </DeleteParameters>
                <SelectParameters>
                    <asp:Parameter Name="currentPage" Type="Int32" />
                    <asp:Parameter Name="pageSize" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </fieldset>
</asp:Content>
