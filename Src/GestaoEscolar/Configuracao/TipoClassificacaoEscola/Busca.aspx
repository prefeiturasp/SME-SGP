<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Configuracao_TipoClassificacaoEscola_Busca" CodeBehind="Busca.aspx.cs" %>
 
 <%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros" TagPrefix="uc1" %>
 <%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao" TagPrefix="uc2" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
    <fieldset>
        <legend>Listagem de tipos de classificação de escola</legend>
        <div></div>
        <uc2:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" ComboDefaultValue="true" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
        <asp:GridView ID="grvTipoClassificacaoEscola" runat="server" AutoGenerateColumns="False"
            DataKeyNames="tce_id, tce_nome" DataSourceID="odsTipoClassificacaoEscola" AllowPaging="True"
            EmptyDataText="Não existem tipo de classificação de escola." 
            HeaderStyle-HorizontalAlign="Center" OnDataBound="grvTipoClassificacaoEscola_DataBound" >
            <Columns> 
                <asp:BoundField DataField="tce_nome" HeaderText="Tipo de classificação de escola" SortExpression="tce_nome"
                    Visible="false" />
                <asp:TemplateField HeaderText="Tipo de classificação de escola">
                    <ItemTemplate>
                        <asp:Label ID="lblAlterar" runat="server" Text='<%# Bind("tce_nome") %>' CssClass="wrap600px"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Cargos">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnCargos" runat="server" CommandName="Edit" SkinID="btDetalhar"
                            PostBackUrl="~/Configuracao/TipoClassificacaoEscola/Cargos.aspx" CssClass="wrap600px"></asp:ImageButton>                        
                    </ItemTemplate>
                    <HeaderStyle CssClass="center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
        </asp:GridView>
        <uc1:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="grvTipoClassificacaoEscola" />
    </fieldset>
    <asp:ObjectDataSource ID="odsTipoClassificacaoEscola" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ESC_TipoClassificacaoEscola"
        EnablePaging="True" MaximumRowsParameterName="PageSize" SelectCountMethod="GetTotalRecords"
        SelectMethod="SelecionaTipoClassificacaoEscolaPaginado" StartRowIndexParameterName="currentPage"
        TypeName="MSTech.GestaoEscolar.BLL.ESC_TipoClassificacaoEscolaBO" 
        OnSelecting="odsTipoClassificacaoEscola_Selecting">
    </asp:ObjectDataSource>
</asp:Content>

