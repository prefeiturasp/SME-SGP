<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Relatorios.AcoesRealizadas.Busca" %>

<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="UCCUAEscola" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCursoCurriculo.ascx" TagName="UCCCursoCurriculo" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCurriculoPeriodo.ascx" TagName="UCCCurriculoPeriodo" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCalendario.ascx" TagName="UCCCalendario" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCTurma.ascx" TagName="UCCTurma" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/BuscaAluno/UCCamposBuscaAluno.ascx" TagName="UCCBuscaAluno" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCObrigatorios" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCCQtdePaginacao" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/Combos/UCComboRelatorioAtendimento.ascx" TagName="UCCRelatorioAtendimento" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMensagem" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="lblMensagem" runat="server" EnableViewState="false"></asp:Label>
            <asp:ValidationSummary ID="vsSummary" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="updFiltros" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlBusca" runat="server" GroupingText="<%$ Resources:GestaoEscolar.Relatorios.AcoesRealizadas.Busca, pnlBusca.Text %>">
                <uc:UCCObrigatorios ID="UCCObrigatorios" runat="server" />
                <uc:UCCUAEscola ID="UCCUAEscola" runat="server" CarregarEscolaAutomatico="true" MostrarMessageSelecioneEscola="true" MostrarMessageSelecioneUA="true"
                    ObrigatorioUA="true" ObrigatorioEscola="true" />
                <uc:UCCCursoCurriculo ID="UCCCursoCurriculo" MostrarMensagemSelecione="true" Obrigatorio="true"
                    runat="server" />
                <uc:UCCCurriculoPeriodo ID="UCCCurriculoPeriodo" MostrarMensagemSelecione="true" Obrigatorio="true"
                    runat="server" />
                <uc:UCCCalendario ID="UCCCalendario" runat="server" MostrarMensagemSelecione="true" Obrigatorio="true" />
                <uc:UCCTurma ID="UCCTurma" runat="server" MostrarMessageSelecione="true" Obrigatorio="false" />
                <uc:UCCRelatorioAtendimento ID="UCCRelatorioAtendimento" runat="server" MostrarMensagemSelecione="true" Obrigatorio="false" />
                <div id="divBuscaAvancadaAluno" runat="server" class="divBuscaAvancadaAluno">
                    <uc:UCCBuscaAluno ID="UCCBuscaAluno" runat="server" />
                </div>
                <div class="right">
                    <asp:Button ID="btnPesquisar" runat="server" Text="<%$ Resources:Padrao, Padrao.Pesquisar.Text %>" OnClick="btnPesquisar_Click" />
                    <asp:Button ID="btnLimparPesquisa" runat="server" Text="<%$ Resources:Padrao, Padrao.LimparPesquisa.Text %>" OnClick="btnLimparPesquisa_Click" CausesValidation="false" />
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updResultados" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlResultados" runat="server" GroupingText="<%$ Resources:Padrao, Padrao.Resultados.Text %>">
                <div id="DivSelecionaTodos" runat="server">
                    <div style="float: left; width: 50%">
                        <asp:CheckBox ID="_chkTodos" SkinID="chkTodos" Text="Selecionar todos os alunos" todososcursospeja='0'
                            runat="server" />
                    </div>
                    <uc:UCCQtdePaginacao ID="UCCQtdePaginacao" runat="server" />
                </div>
                <asp:GridView ID="grvResultados" runat="server" AutoGenerateColumns="false" OnDataBound="grvResultados_DataBound"
                    OnPageIndexChanging="grvResultados_PageIndexChanging" AllowPaging="true" AllowCustomPaging="true" AllowSorting="true"
                    EmptyDataText="<%$ Resources:Padrao, Padrao.SemResultado.Text %>"
                    OnSorting="grvResultados_Sorting" OnRowEditing="grvResultados_RowEditing" DataKeyNames="alu_id,cal_id,tur_id">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="_chkSelecionar" runat="server" alu_id='<%# Eval("alu_id") %>'  cursopeja='<%# Eval("CursoPeja") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="<%$ Resources:Mensagens, MSG_NUMEROMATRICULA %>" DataField="alc_matricula" SortExpression="alc_matricula" />
                        <asp:BoundField HeaderText="<%$ Resources:Padrao, Padrao.Nome.Text %>" DataField="pes_nome" SortExpression="pes_nome" />
                        <asp:BoundField HeaderText="<%$ Resources:Padrao, Padrao.Escola.Text %>" DataField="tur_escolaUnidade" SortExpression="tur_escolaUnidade" />
                        <asp:BoundField HeaderText="<%$ Resources:Padrao, Padrao.Turma.Text %>" DataField="tur_codigo" SortExpression="tur_codigo" />
                        <asp:BoundField HeaderText="<%$ Resources:Padrao, Padrao.Curso.Text %>" DataField="tur_curso" SortExpression="tur_curso" />
                        <asp:BoundField HeaderText="<%$ Resources:Padrao, Padrao.Calendario.Text %>" DataField="tur_calendario" SortExpression="tur_calendario" />
                    </Columns>
                </asp:GridView>
                <uc:UCTotalRegistros ID="UCTotalRegistros" runat="server" AssociatedGridViewID="grvResultados" />
                <div class="right">
                    <asp:Button ID="btnGerarRelatrorioAcoesRealizadas" runat="server" Text="Gerar relatório" OnClick="btnGerarRelatrorioAcoesRealizadas_Click" />
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
