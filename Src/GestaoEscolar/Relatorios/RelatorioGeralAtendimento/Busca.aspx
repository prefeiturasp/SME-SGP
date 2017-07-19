<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Relatorios.RelatorioGeralAtendimento.Busca" %>

<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="UCCUAEscola" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCursoCurriculo.ascx" TagName="UCCCursoCurriculo" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCurriculoPeriodo.ascx" TagName="UCCCurriculoPeriodo" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCalendario.ascx" TagName="UCCCalendario" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCTurma.ascx" TagName="UCCTurma" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoRelatorioAtendimento.ascx" TagPrefix="uc" TagName="UCCTipoRelatorioAtendimento" %>
<%@ Register Src="~/WebControls/Combos/UCComboRelatorioAtendimento.ascx" TagPrefix="uc" TagName="UCCRelatorioAtendimento" %>
<%@ Register Src="~/WebControls/BuscaAluno/UCCamposBuscaAluno.ascx" TagName="UCCBuscaAluno" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCObrigatorios" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCCQtdePaginacao" TagPrefix="uc" %>

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
            <asp:Panel ID="pnlBusca" runat="server" GroupingText="Relatório geral de atendimento">
                <uc:UCCObrigatorios ID="UCCObrigatorios" runat="server" />
                <uc:UCCTipoRelatorioAtendimento runat="server" ID="UCCTipoRelatorioAtendimento" 
                    MostrarMensagemSelecione="true" Obrigatorio="true" Titulo="Tipo de relatório *"/>
                <uc:UCCRelatorioAtendimento ID="UCCRelatorioAtendimento" runat="server" Obrigatorio="true" PermiteEditar="false"/>
                <uc:UCCUAEscola ID="UCCUAEscola" runat="server" CarregarEscolaAutomatico="true" MostrarMessageSelecioneEscola="true" MostrarMessageSelecioneUA="true"
                    ObrigatorioUA="true" ObrigatorioEscola="true" />
                <uc:UCCCursoCurriculo ID="UCCCursoCurriculo" MostrarMensagemSelecione="true" Obrigatorio="true"
                    runat="server" />
                <uc:UCCCurriculoPeriodo ID="UCCCurriculoPeriodo" MostrarMensagemSelecione="true" Obrigatorio="true"
                    runat="server" />
                <uc:UCCCalendario ID="UCCCalendario" runat="server" MostrarMensagemSelecione="true" Obrigatorio="true" />
                <uc:UCCTurma ID="UCCTurma" runat="server" MostrarMessageSelecione="true" />
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
            <fieldset id="fdsResultados" runat="server">
                <legend>Resultados</legend>
                <div class="right area-botoes-top">
                    <asp:Button ID="btnGerarRelatorioCima" runat="server" Text="Gerar relatório" OnClick="btnGerarRelatorioCima_Click" />
                </div>
                <div class="area-form">
                    <br />
                    <br />
                    <div style="float: left; width: 50%">
                        <asp:CheckBox ID="chkTodos" SkinID="chkTodos" Text="Selecionar todos os alunos"
                            runat="server" />
                    </div>
                    <uc:UCCQtdePaginacao ID="UCCQtdePaginacao" runat="server" />                    
                    <br /> 
                    <asp:GridView ID="grvResultados" runat="server" AutoGenerateColumns="false" OnDataBound="grvResultados_DataBound"
                        OnPageIndexChanging="grvResultados_PageIndexChanging" AllowPaging="true" AllowSorting="true" OnRowDataBound="grvResultados_RowDataBound"
                        EmptyDataText="<%$ Resources:Padrao, Padrao.SemResultado.Text %>" OnDataBinding="grvResultados_DataBinding"
                        OnSorting="grvResultados_Sorting" OnRowEditing="grvResultados_RowEditing" DataKeyNames="alu_id,cal_id,tur_id">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelecionar" runat="server" alu_id='<%# Eval("alu_id") %>' cal_id='<%# Eval("cal_id") %>'
                                        tur_id='<%# Eval("tur_id") %>' esc_id='<%# Eval("esc_id") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="<%$ Resources:Padrao, Padrao.Nome.Text %>" DataField="pes_nome" SortExpression="pes_nome" />
                            <asp:BoundField HeaderText="<%$ Resources:Padrao, Padrao.Idade.Text %>" DataField="pes_idade" SortExpression="pes_idade" />
                            <asp:BoundField HeaderText="<%$ Resources:Padrao, Padrao.Escola.Text %>" DataField="tur_escolaUnidade" SortExpression="tur_escolaUnidade" />
                            <asp:BoundField HeaderText="<%$ Resources:Padrao, Padrao.Curso.Text %>" DataField="tur_curso" SortExpression="tur_curso" />
                            <asp:BoundField HeaderText="<%$ Resources:Padrao, Padrao.Turma.Text %>" DataField="tur_codigo" SortExpression="tur_codigo" />
                        </Columns>
                    </asp:GridView>
                    <uc:UCTotalRegistros ID="UCTotalRegistros" runat="server" AssociatedGridViewID="grvResultados" />
                </div>
                <div class="right area-botoes-bottom">
                    <asp:Button ID="btnGerarRelatorio" runat="server" Text="Gerar relatório" OnClick="btnGerarRelatorio_Click" />
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
