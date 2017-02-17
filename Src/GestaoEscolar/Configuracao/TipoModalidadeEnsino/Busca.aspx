<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Configuracao_TipoModalidadeEnsino_Busca" CodeBehind="Busca.aspx.cs" %>

<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <fieldset>
        <legend>Listagem de tipos de modalidade de ensino</legend>
        <div>
            <uc1:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
            <asp:GridView ID="_dgvTipoModalidadeEnsino" runat="server" AutoGenerateColumns="False"
                DataKeyNames="tme_id" DataSourceID="_odsTipoModalidadeEnsino" EmptyDataText="Não existem tipos de modalidade de ensino cadastrados."
                AllowPaging="True">
                <Columns>
                    <asp:BoundField DataField="tme_nome" HeaderText="Tipo de modalidade de ensino" SortExpression="tme_nome" />
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="_odsTipoModalidadeEnsino" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_TipoModalidadeEnsino"
                EnablePaging="True" MaximumRowsParameterName="pageSize" SelectCountMethod="GetTotalRecords"
                SelectMethod="SelecionaTipoModalidadeEnsinoPaginado" StartRowIndexParameterName="currentPage"
                TypeName="MSTech.GestaoEscolar.BLL.ACA_TipoModalidadeEnsinoBO" OnSelecting="_odsTipoModalidadeEnsino_Selecting">
            </asp:ObjectDataSource>
        </div>
    </fieldset>
</asp:Content>
