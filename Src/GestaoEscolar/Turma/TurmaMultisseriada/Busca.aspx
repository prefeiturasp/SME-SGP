<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Turma.TurmaMultisseriada.Busca" %>

<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="UCComboUAEscola"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboCursoCurriculo.ascx" TagName="UCComboCursoCurriculo"
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboDisciplina.ascx" TagName="UCComboDisciplina"
    TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/UCComboCalendario.ascx" TagName="UCComboCalendario"
    TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Combos/UCComboTurno.ascx" TagName="UCComboTurno"
    TagPrefix="uc5" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc6" %>
<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc7" %>
<%@ Register Src="~/WebControls/Combos/UCComboDocente.ascx" TagName="UCComboDocente"
    TagPrefix="uc8" %>
<%@ Register Src="~/WebControls/Combos/UCComboCurriculoPeriodo.ascx" TagName="UCComboCurriculoPeriodo"
    TagPrefix="uc9" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <asp:Panel ID="pnlTurma" runat="server" GroupingText="Consulta de turmas multisseriadas">
        <div id="divPesquisa" runat="server">
            <asp:UpdatePanel ID="upnPesquisa" runat="server">
                <ContentTemplate>
                    <uc1:UCComboUAEscola ID="uccFiltroEscola" runat="server" CarregarEscolaAutomatico="true"
                        MostrarMessageSelecioneEscola="true" />
                    <uc4:UCComboCalendario ID="uccCalendario" runat="server" MostrarMensagemSelecione="true" PermiteEditar="true" />
                    <uc2:UCComboCursoCurriculo ID="uccCursoCurriculo" runat="server" MostrarMessageSelecione="true" PermiteEditar="true"/>
                    <uc9:UCComboCurriculoPeriodo ID="uccCurriculoPeriodo" runat="server" _MostrarMessageSelecione="true"
                        PermiteEditar="false" />
                    <uc3:UCComboDisciplina ID="uccDisciplina" runat="server" MostrarMensagemSelecione="true"
                        PermiteEditar="false" />
                    <uc5:UCComboTurno ID="uccTurno" runat="server" CancelSelect="false" />
                    <uc8:UCComboDocente ID="uccDocente" runat="server" _MostrarMessageSelecione="true"
                        _CancelaSelect="true" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:Label ID="lblCodigoTurma" runat="server" Text="Código da turma" AssociatedControlID="txtCodigoTurma">
            </asp:Label>
            <asp:TextBox ID="txtCodigoTurma" runat="server" MaxLength="30" SkinID="text30C">
            </asp:TextBox>
        </div>
        <div class="right">
            <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click"
                CausesValidation="false" />
            <asp:Button ID="btnLimparPesquisa" runat="server" Text="Limpar pesquisa" OnClick="btnLimparPesquisa_Click"
                CausesValidation="false" />
        </div>
    </asp:Panel>
    <fieldset id="fdsResultado" runat="server" visible="false">
        <legend>Resultados</legend>
        <uc6:UCComboQtdePaginacao ID="uccQtdePaginacao" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
        <asp:GridView ID="gvTurma" runat="server" AutoGenerateColumns="False" DataSourceID="odsTurma"
            AllowPaging="True" EmptyDataText="A pesquisa não encontrou resultados." DataKeyNames="tur_id,tur_situacao"
            OnDataBound="gvTurma_DataBound" PageSize="10" AllowSorting="True">
            <Columns>
                <asp:TemplateField HeaderText="Código da turma" SortExpression="tur_codigo">
                    <ItemTemplate>
                        <asp:LinkButton ID="_btnAlterar" CausesValidation="false" runat="server" CommandName="Edit"
                            Text='<%# Bind("tur_codigo") %>' PostBackUrl="~/Turma/TurmaMultisseriada/Cadastro.aspx"
                            CssClass="wrap100px"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Escola" SortExpression="esc_nome">
                    <ItemTemplate>
                        <asp:Label ID="lblEscola" runat="server" Text='<%# Bind("esc_nome") %>' CssClass="wrap150px"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Calendário" SortExpression="Calendario">
                    <ItemTemplate>
                        <asp:Label ID="_lblCalendario" runat="server" Text='<%# Bind("Calendario") %>' CssClass="wrap100px"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Curso" SortExpression="cur_nome">
                    <ItemTemplate>
                        <asp:Label ID="lblCurso" runat="server" Text='<%# Bind("cur_nome") %>' CssClass="wrap200px"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Turno" SortExpression="Turno">
                    <ItemTemplate>
                        <asp:Label ID="lblTurno" runat="server" Text='<%# Bind("Turno") %>' CssClass="wrap100px"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="tud_nome" HeaderText="<%$ Resources:Turma, TurmaMultisseriada.Busca.gvTurma.Coluna6 %>" SortExpression="tud_nome" />
                <asp:BoundField DataField="tur_situacao" HeaderText="Situação" SortExpression="tur_situacao" />
            </Columns>
            <HeaderStyle HorizontalAlign="Center" />
        </asp:GridView>
        <uc7:UCTotalRegistros ID="ucTotalRegistros" runat="server" AssociatedGridViewID="gvTurma" />
        <asp:ObjectDataSource ID="odsTurma" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.TUR_Turma"
            TypeName="MSTech.GestaoEscolar.BLL.TUR_TurmaBO" DeleteMethod="Delete" SelectMethod="SelectBy_Pesquisa_TurmasMultisseriadas">
        </asp:ObjectDataSource>
    </fieldset>
</asp:Content>