<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Classe_Efetivacao_Busca" CodeBehind="Busca.aspx.cs" enableEventValidation="false" %>

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
        GroupingText="Consulta de lançamento de notas" />
    <asp:Panel ID="pnlResultado" runat="server" GroupingText="Resultados">
        <uc8:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
        <asp:GridView ID="_dgvTurma" runat="server" AutoGenerateColumns="False" DataSourceID="_odsTurma"
            DataKeyNames="tur_id,fav_id, cal_id" AllowPaging="True" OnRowDataBound="_dgvTurma_RowDataBound"
            EmptyDataText="A pesquisa não encontrou resultados." OnRowCommand="_dgvTurma_RowCommand"
            OnDataBound="_dgvTurma_DataBound" AllowSorting="true" OnPageIndexChanging="_dgvTurma_PageIndexChanging"
            OnSorting="_dgvTurma_Sorting">
            <Columns>
                <asp:TemplateField HeaderText="Código da turma" SortExpression="tur_codigo">
                    <ItemTemplate>
                        <asp:LinkButton ID="_btnAlterar" runat="server" CommandName="Selecionar" Text='<%# Bind("tur_codigo") %>'
                            CausesValidation="False" CssClass="wrap100px"></asp:LinkButton>
                        <asp:Label ID="_lblAlterar" runat="server" Text='<%# Bind("tur_codigo") %>' CssClass="wrap100px"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Escola" SortExpression="tur_escolaUnidade">
                    <ItemTemplate>
                        <asp:Label ID="lblEscola" runat="server" Text='<%# Bind("tur_escolaUnidade") %>'
                            CssClass="wrap200px"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Calendário" SortExpression="tur_calendario">
                    <ItemTemplate>
                        <asp:Label ID="_lblCalendario" runat="server" Text='<%# Bind("tur_calendario") %>'
                            CssClass="wrap100px"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Curso" SortExpression="tur_curso">
                    <ItemTemplate>
                        <asp:Label ID="lblCurso" runat="server" Text='<%# Bind("tur_curso") %>' CssClass="wrap200px"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Turno" SortExpression="tur_turno">
                    <ItemTemplate>
                        <asp:Label ID="lblTurno" runat="server" Text='<%# Bind("tur_turno") %>' CssClass="wrap100px"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <uc7:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="_dgvTurma" />
    </asp:Panel>
    <asp:ObjectDataSource ID="_odsTurma" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.TUR_Turma"
        TypeName="MSTech.GestaoEscolar.BLL.TUR_TurmaBO" SelectMethod="GetSelectBy_Pesquisa_TodosTipos" OnSelecting="_odsTurma_Selecting">
    </asp:ObjectDataSource>
    <div id="divAvaliacoes" title="Selecionar avaliação" class="hide">
        <asp:GridView ID="gvAvaliacoes" runat="server" AutoGenerateColumns="false" OnRowCommand="gv_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="Nome">
                    <ItemTemplate>
                        <asp:LinkButton ID="lkbSelecionar" runat="server" CommandName="SelecionarAvaliacao"
                            Text='<%#Bind("ava_nome") %>' CommandArgument='<%#Bind("ava_id") %>' ToolTip="Selecionar avaliação">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Tipo" DataField="ava_tipo_periodo_grid" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
