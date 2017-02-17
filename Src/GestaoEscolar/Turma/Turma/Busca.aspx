<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Academico_Turma_Busca" CodeBehind="Busca.aspx.cs" %>

<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="ComboUAEscola"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboCalendario.ascx" TagName="ComboCalendario"
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboCursoCurriculo.ascx" TagName="ComboCursoCurriculo"
    TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/UCComboTurno.ascx" TagName="ComboTurno" TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Combos/UCComboDocente.ascx" TagName="ComboDocente"
    TagPrefix="uc5" %>
<%@ Register Src="~/WebControls/Combos/UCComboCurriculoPeriodo.ascx" TagName="ComboCurriculoPeriodo"
    TagPrefix="uc6" %>
<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc7" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc9" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <fieldset class="msgInfo">
        <legend>Consulta de turmas</legend>
        <div id="divPesquisa" runat="server">
            <asp:UpdatePanel ID="updPesquisa" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc1:ComboUAEscola ID="ucComboUAEscola" runat="server" CarregarEscolaAutomatico="true"
                        ObrigatorioEscola="false" ObrigatorioUA="false" MostrarMessageSelecioneEscola="true"
                        MostrarMessageSelecioneUA="true" />
                    <uc3:ComboCursoCurriculo ID="ucComboCursoCurriculo" runat="server" />
                    <uc6:ComboCurriculoPeriodo ID="ucComboCurriculoPeriodo" runat="server" CancelSelect="true"
                        _MostrarMessageSelecione="true" PermiteEditar="false" />
                    <uc2:ComboCalendario ID="ucComboCalendario" runat="server" />
                    <uc4:ComboTurno ID="ucComboTurno" runat="server" />
                    <uc5:ComboDocente ID="ucComboDocente" runat="server" _CancelaSelect="true" _MostrarMessageSelecione="true"
                        PermiteEditar="false" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:Label ID="lblCodigoTurma" runat="server" Text="Código da turma" AssociatedControlID="txtCodigoTurma"
                EnableViewState="false"></asp:Label>
            <asp:TextBox ID="txtCodigoTurma" runat="server" MaxLength="30" SkinID="text30C"></asp:TextBox>
        </div>
        <div class="right">
            <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click" />
            <asp:Button ID="btnLimparPesquisa" runat="server" Text="Limpar pesquisa" OnClick="btnLimparPesquisa_Click" />
        </div>
    </fieldset>
    <!-- Resultado consulta de turmas -->
    <fieldset id="fdsResultado" runat="server" visible="false">
        <legend>Resultados</legend>
        <uc9:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged"
            ComboDefaultValue="true" />
        <asp:GridView ID="grvTurma" runat="server" AutoGenerateColumns="False" AllowCustomPaging="true"
            DataKeyNames="tur_id,cur_id,crr_id,crp_id,tur_situacao" AllowPaging="True" EmptyDataText="A pesquisa não encontrou resultados."
            OnDataBound="grvTurma_DataBound" AllowSorting="True" OnSorting="grvTurma_Sorting" OnPageIndexChanging="grvTurma_PageIndexChanging">
            <Columns>
                <asp:TemplateField HeaderText="Código da turma" SortExpression="tur_codigo">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnAlterar" runat="server" CommandName="Select" Text='<%# Bind("tur_codigo") %>'
                            PostBackUrl="~/Turma/Turma/Cadastro.aspx" CssClass="wrap100px"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Escola" DataField="tur_escolaUnidade" ControlStyle-CssClass="wrap150px"
                    SortExpression="tur_escolaUnidade" />
                <asp:BoundField HeaderText="Calendário" DataField="tur_calendario" SortExpression="tur_calendario" />
                <asp:BoundField HeaderText="Curso" DataField="tur_curso" ControlStyle-CssClass="wrap150px"
                    SortExpression="tur_curso" />
                <asp:BoundField HeaderText="Turno" DataField="tur_turno" ControlStyle-CssClass="wrap100px"
                    SortExpression="tur_turno" />
                <asp:BoundField DataField="tur_situacao" HeaderText="Situação" SortExpression="tur_situacao" />
                <asp:TemplateField HeaderText="Alunos">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnAlunosTurma" SkinID="btDetalhar" runat="server" CommandName="Select"
                            PostBackUrl="~/Turma/Turma/AlunosTurma.aspx" ToolTip="Exibe alunos matriculados na turma" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="center"></ItemStyle>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle HorizontalAlign="Center" />
        </asp:GridView>
        <uc7:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="grvTurma" />
    </fieldset>
</asp:Content>