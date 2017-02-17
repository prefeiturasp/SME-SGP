<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Configuracao_TipoNivelEnsino_Busca" CodeBehind="Busca.aspx.cs" %>

<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <fieldset>
        <legend>Listagem de tipos de nível de ensino</legend>
        <div>
            <uc1:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
            <asp:GridView ID="_dgvTipoNivelEnsino" runat="server" AutoGenerateColumns="False"
                DataKeyNames="tne_id,tne_ordem" DataSourceID="_odsTipoNivelEnsino" EmptyDataText="Não existem tipos de nível de ensino cadastrados."
                AllowPaging="True" OnRowCommand="_dgvTipoNivelEnsino_RowCommand" OnRowDataBound="_dgvTipoNivelEnsino_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Tipo de nível de ensino">
                        <ItemTemplate>
                            <asp:Label ID="_lblAlterar" runat="server" Text='<%# Bind("tne_nome") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ordem" SortExpression="tpc_ordem">
                        <ItemTemplate>
                            <asp:ImageButton ID="_btnSubir" runat="server" CausesValidation="false" CommandName="Subir"
                                Height="16" Width="16" />
                            <asp:ImageButton ID="_btnDescer" runat="server" CausesValidation="false" CommandName="Descer"
                                Height="16" Width="16" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="center" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle HorizontalAlign="Center" />
            </asp:GridView>
            <asp:ObjectDataSource ID="_odsTipoNivelEnsino" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_TipoNivelEnsino"
                SelectMethod="SelecionaTipoNivelEnsinoPaginado" TypeName="MSTech.GestaoEscolar.BLL.ACA_TipoNivelEnsinoBO"
                OnSelecting="_odsTipoNivelEnsino_Selecting" EnablePaging="True" MaximumRowsParameterName="pageSize"
                SelectCountMethod="GetTotalRecords" StartRowIndexParameterName="currentPage"></asp:ObjectDataSource>
        </div>
    </fieldset>
</asp:Content>