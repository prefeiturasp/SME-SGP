<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Classe.RelatorioAtendimento.Busca" %>

<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="UCCUAEscola" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCursoCurriculo.ascx" TagName="UCCCursoCurriculo" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCurriculoPeriodo.ascx" TagName="UCCCurriculoPeriodo" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCalendario.ascx" TagName="UCCCalendario" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCTurma.ascx" TagName="UCCTurma" TagPrefix="uc" %>
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
            <asp:ValidationSummary ID="vsSummary" runat="server" ValidationGroup="BuscaAlunos" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="updFiltros" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlBusca" runat="server" GroupingText="Consulta de relatórios de AEE">
                <uc:UCCObrigatorios ID="UCCObrigatorios" runat="server" />
                <uc:UCCUAEscola ID="UCCUAEscola" runat="server" CarregarEscolaAutomatico="true" MostrarMessageSelecioneEscola="true" MostrarMessageSelecioneUA="true"
                    ObrigatorioUA="true" ObrigatorioEscola="true" ValidationGroup="BuscaAlunos" />
                <uc:UCCCursoCurriculo ID="UCCCursoCurriculo" MostrarMensagemSelecione="true" Obrigatorio="true" ValidationGroup="BuscaAlunos"
                    runat="server" />
                <uc:UCCCurriculoPeriodo ID="UCCCurriculoPeriodo" MostrarMensagemSelecione="true" Obrigatorio="true" ValidationGroup="BuscaAlunos"
                    runat="server" />
                <uc:UCCCalendario ID="UCCCalendario" runat="server" MostrarMensagemSelecione="true" Obrigatorio="true" ValidationGroup="BuscaAlunos" />
                <uc:UCCTurma ID="UCCTurma" runat="server" MostrarMessageSelecione="true" Obrigatorio="true" ValidationGroup="BuscaAlunos" />
                <div id="divBuscaAvancadaAluno" runat="server" class="divBuscaAvancadaAluno">
                    <uc:UCCBuscaAluno ID="UCCBuscaAluno" runat="server" />
                </div>
                <div class="right">
                    <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click" ValidationGroup="BuscaAlunos" />
                    <asp:Button ID="btnLimparPesquisa" runat="server" Text="Limpar pesquisa" OnClick="btnLimparPesquisa_Click" CausesValidation="false" />
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updResultados" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlResultados" runat="server" GroupingText="Resultados">
                <uc:UCCQtdePaginacao ID="UCCQtdePaginacao" runat="server" />
                <asp:GridView ID="grvResultados" runat="server" AutoGenerateColumns="false" OnDataBound="grvResultados_DataBound" 
                    OnPageIndexChanging="grvResultados_PageIndexChanging" AllowPaging="true" AllowSorting="true"
                    EmptyDataText="A pesquisa não encontrou resultados." OnDataBinding="grvResultados_DataBinding"
                    OnSorting="grvResultados_Sorting" OnRowEditing="grvResultados_RowEditing" DataKeyNames="alu_id,cal_id">
                    <Columns>
                        <asp:BoundField HeaderText="Nome" DataField="pes_nome" SortExpression="pes_nome" />
                        <asp:BoundField HeaderText="Idade" DataField="pes_idade" SortExpression="pes_idade" />
                        <asp:BoundField HeaderText="Escola" DataField="tur_escolaUnidade" SortExpression="tur_escolaUnidade"/>
                        <asp:BoundField HeaderText="Curso" DataField="tur_curso" SortExpression="tur_curso" />
                        <asp:BoundField HeaderText="Turma" DataField="tur_codigo" SortExpression="tur_codigo"/>
                        <asp:TemplateField HeaderText="Lançar relatório" HeaderStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnResponder" runat="server" SkinID="btRelatorio" CommandName="Edit" PostBackUrl="~/Classe/RelatorioAtendimento/Cadastro.aspx" />
                            </ItemTemplate>
                            <ItemStyle CssClass="center" HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <uc:UCTotalRegistros ID="UCTotalRegistros" runat="server" AssociatedGridViewID="grvResultados" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
