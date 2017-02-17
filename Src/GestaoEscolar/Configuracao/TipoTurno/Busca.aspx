<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Configuracao_TipoTurno_Busca" Codebehind="Busca.aspx.cs" %>

<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <fieldset>
        <legend>Listagem de turnos</legend>
        <div>
            <uc1:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
            <asp:GridView ID="_dgvTipoTurno" runat="server" AutoGenerateColumns="False" DataKeyNames="ttn_id"
                DataSourceID="_odsTipoTurno" AllowPaging="True" EmptyDataText="Não existem tipos de turno cadastrados.">
                <Columns>
                    <asp:TemplateField HeaderText="Tipo de turno">
                        <ItemTemplate>
                            <asp:LinkButton ID="_btnAlterar" runat="server" CommandName="Edit" Text='<%# Bind("ttn_nome") %>'
                                PostBackUrl="~/Configuracao/TipoTurno/Cadastro.aspx"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ttn_situacao" HeaderText="Bloqueado" SortExpression="ttn_situacao"
                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <HeaderStyle CssClass="center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                </Columns>
                <HeaderStyle HorizontalAlign="Center" />
            </asp:GridView>
        </div>
        <div>
            <asp:ObjectDataSource ID="_odsTipoTurno" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_TipoTurno"
                EnablePaging="True" StartRowIndexParameterName="currentPage" MaximumRowsParameterName="pageSize"
                SelectCountMethod="GetTotalRecords" SelectMethod="SelecionaTipoTurnoPaginado"
                TypeName="MSTech.GestaoEscolar.BLL.ACA_TipoTurnoBO" OnSelecting="_odsTipoTurno_Selecting">
            </asp:ObjectDataSource>
        </div>
    </fieldset>
</asp:Content>
