<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Configuracao.RelatorioAtendimento.Busca" %>
<%@ Register Src="../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc1" %>   

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
            <asp:Panel ID="pnlBusca" runat="server" GroupingText="<%$ Resources:GestaoEscolar.Classe.RelatorioAtendimento.Busca, pnlBusca.Text %>">
                <uc:UCCObrigatorios ID="UCCObrigatorios" runat="server" />
                <uc:UCCUAEscola ID="UCCUAEscola" runat="server" CarregarEscolaAutomatico="true" MostrarMessageSelecioneEscola="true" MostrarMessageSelecioneUA="true"
                    ObrigatorioUA="true" ObrigatorioEscola="true"  />
                <uc:UCCCursoCurriculo ID="UCCCursoCurriculo" MostrarMensagemSelecione="true" Obrigatorio="true"
                    runat="server" />
                <uc:UCCCurriculoPeriodo ID="UCCCurriculoPeriodo" MostrarMensagemSelecione="true" Obrigatorio="true"
                    runat="server" />
                <uc:UCCCalendario ID="UCCCalendario" runat="server" MostrarMensagemSelecione="true" Obrigatorio="true"  />
                <uc:UCCTurma ID="UCCTurma" runat="server" MostrarMessageSelecione="true" Obrigatorio="true" />
                <div id="divBuscaAvancadaAluno" runat="server" class="divBuscaAvancadaAluno">
                    <uc:UCCBuscaAluno ID="UCCBuscaAluno" runat="server"  />
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
                <uc:UCCQtdePaginacao ID="UCCQtdePaginacao" runat="server" />
                <asp:GridView ID="grvResultados" runat="server" AutoGenerateColumns="false" OnDataBound="grvResultados_DataBound"
                    OnPageIndexChanging="grvResultados_PageIndexChanging" AllowPaging="true" AllowSorting="true"
                    EmptyDataText="<%$ Resources:Padrao, Padrao.SemResultado.Text %>" OnDataBinding="grvResultados_DataBinding"
                    OnSorting="grvResultados_Sorting" OnRowEditing="grvResultados_RowEditing" DataKeyNames="alu_id,cal_id,tur_id">
                    <Columns>
                        <asp:BoundField HeaderText="<%$ Resources:Padrao, Padrao.Nome.Text %>" DataField="pes_nome" SortExpression="pes_nome" />
                        <asp:BoundField HeaderText="<%$ Resources:Padrao, Padrao.Idade.Text %>" DataField="pes_idade" SortExpression="pes_idade" />
                        <asp:BoundField HeaderText="<%$ Resources:Padrao, Padrao.Escola.Text %>" DataField="tur_escolaUnidade" SortExpression="tur_escolaUnidade"/>
                        <asp:BoundField HeaderText="<%$ Resources:Padrao, Padrao.Curso.Text %>" DataField="tur_curso" SortExpression="tur_curso" />
                        <asp:BoundField HeaderText="<%$ Resources:Padrao, Padrao.Turma.Text %>" DataField="tur_codigo" SortExpression="tur_codigo"/>
                        <asp:TemplateField HeaderText="<%$ Resources:Padrao, Padrao.LancarRelatorio.Text %>" HeaderStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnResponder" runat="server" SkinID="btRelatorio" CommandName="Edit" PostBackUrl="~/Classe/RelatorioAtendimento/Cadastro.aspx"
                                    ToolTip="<%$ Resources:GestaoEscolar.Classe.RelatorioAtendimento.Busca, ctrl_61.ToolTip %>" />
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
