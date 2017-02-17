<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Classe_EfetivacaoGestor_Busca" CodeBehind="Busca.aspx.cs" enableEventValidation="false" %>

<%@ Register Src="../../WebControls/BuscaLancamentoClasse/UCBuscaLancamentoClasse.ascx"
    TagName="UCBuscaLancamentoClasse" TagPrefix="uc1" %>
<%@ Register Src="../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc7" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <uc1:UCBuscaLancamentoClasse ID="UCBuscaLancamentoClasse1" runat="server" Visible="false"
        GroupingText="Consulta de lançamento de notas" VisualizacaoGestor="true" />
    <asp:Panel ID="pnlResultado" runat="server" GroupingText="Resultados">
        <uc8:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
        <asp:GridView ID="_dgvEscola" runat="server" AutoGenerateColumns="False" DataSourceID="_odsEscola"
            DataKeyNames="esc_id, uni_id, cal_id" AllowPaging="True" OnRowDataBound="_dgvEscola_RowDataBound"
            EmptyDataText="A pesquisa não encontrou resultados." OnRowCommand="_dgvEscola_RowCommand"
            OnDataBound="_dgvEscola_DataBound" AllowSorting="true" OnPageIndexChanging="_dgvEscola_PageIndexChanging"
            OnSorting="_dgvEscola_Sorting">
            <Columns>
                <asp:TemplateField HeaderText="Escola" SortExpression="tur_escolaUnidade">
                    <ItemTemplate>
                        <asp:LinkButton ID="_btnAlterar" runat="server" CommandName="Selecionar" Text='<%# Bind("esc_escolaUnidade") %>'
                            CausesValidation="False" CssClass="wrap200px"></asp:LinkButton>
                        <asp:Label ID="_lblAlterar" runat="server" Text='<%# Bind("esc_escolaUnidade") %>' CssClass="wrap100px"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Calendário" SortExpression="tur_calendario">
                    <ItemTemplate>
                        <asp:Label ID="_lblCalendario" runat="server" Text='<%# Bind("esc_calendario") %>'
                            CssClass="wrap100px"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <uc7:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="_dgvEscola" />
    </asp:Panel>
    <asp:ObjectDataSource ID="_odsEscola" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ESC_Escola"
        TypeName="MSTech.GestaoEscolar.BLL.ESC_EscolaBO" SelectMethod="GetSelectBy_Pesquisa_TodosTipos" OnSelecting="_odsEscola_Selecting">
    </asp:ObjectDataSource>
</asp:Content>