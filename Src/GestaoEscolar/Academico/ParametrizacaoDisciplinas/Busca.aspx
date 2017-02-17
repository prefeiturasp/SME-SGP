<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Academico.ParametrizacaoDisciplinas.Busca" %>

<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="ComboUAEscola"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboCursoCurriculo.ascx" TagName="ComboCursoCurriculo"
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboCurriculoPeriodo.ascx" TagName="ComboCurriculoPeriodo"
    TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/UCComboCalendario.ascx" TagName="ComboCalendario"
    TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoDisciplina.ascx" TagName="UCComboTipoDisciplina"
    TagPrefix="uc5" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc6" %>
<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc7" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="_updMessage" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="false" Text=""></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="ParametrosDisciplinas" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <fieldset class="msgInfo">
        <legend>Consulta de parâmetros de configuração de disciplinas</legend>
        <uc8:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" />
        <div id="divPesquisa" runat="server">
            <uc1:ComboUAEscola ID="ucComboUAEscola" runat="server" CarregarEscolaAutomatico="true"
                MostrarMessageSelecioneEscola="true" AsteriscoObg="true" ObrigatorioEscola="true"
                ObrigatorioUA="true" MostrarMessageSelecioneUA="true" ValidationGroup="ParametrosDisciplinas" />
            <uc2:ComboCursoCurriculo ID="ucComboCursoCurriculo" runat="server" Obrigatorio="true" PermiteEditar="false"
                MostrarMessageSelecione="true" ValidationGroup="ParametrosDisciplinas" />
            <uc3:ComboCurriculoPeriodo ID="ucComboCurriculoPeriodo" runat="server" CancelSelect="true" PermiteEditar="false"
                Obrigatorio="true" _MostrarMessageSelecione="true" ValidationGroup="ParametrosDisciplinas" />
            <uc4:ComboCalendario ID="ucComboCalendario" runat="server" Obrigatorio="true" ValidationGroup="ParametrosDisciplinas" />
            <uc5:UCComboTipoDisciplina ID="UCComboTipoDisciplina1" runat="server" MostrarMensagemSelecione="true" PermiteEditar="false"
                Obrigatorio="true" ValidationGroup="ParametrosDisciplinas" />
            <asp:Label ID="lblCodigoTurma" runat="server" Text="Código da turma" AssociatedControlID="txtCodigoTurma"
                EnableViewState="false"></asp:Label>
            <asp:TextBox ID="txtCodigoTurma" runat="server" MaxLength="30" SkinID="text30C"></asp:TextBox>
        </div>
        <div class="right">
            <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click"
                ValidationGroup="ParametrosDisciplinas" />
            <asp:Button ID="btnLimparPesquisa" runat="server" Text="Limpar pesquisa" OnClick="btnLimparPesquisa_Click" />
            <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click" />
        </div>
    </fieldset>
    <%--Resultado da consulta--%>
    <fieldset id="fdsResultado" runat="server" visible="false">
        <legend>Resultados</legend>
        <uc6:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged"
            ComboDefaultValue="true" />
        <asp:GridView ID="grvParametrosDisciplinas" runat="server" AutoGenerateColumns="False" DataSourceID="odsParametrosDisciplinas"
            DataKeyNames="tud_id, tud_codigo, tud_nome" AllowPaging="True"
            EmptyDataText="A pesquisa não encontrou resultados." AllowSorting="True" OnDataBound="grvParametrosDisciplinas_DataBound">
            <Columns>
                <asp:BoundField HeaderText="Código da turma" DataField="tud_codigo" SortExpression="tud_codigo" />
                <asp:BoundField HeaderText="Disciplina" DataField="tud_nome" 
                    SortExpression="tud_nome">
                </asp:BoundField>
                <asp:BoundField HeaderText="Curso - Período" DataField="cur_nome" SortExpression="cur_nome" />
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkNaoLancarFrequencia" runat="server" Text="Não Lançar frequência"
                            ToolTip="Bloqueia lançamento de frequência." />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkItemNaoLancarFrequencia" runat="server" Checked='<%# Bind("tud_naoLancarFrequencia") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkNaoLancarNota" runat="server" Text="Não Lançar nota"
                            ToolTip="Bloqueia lançamento de nota." />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkItemNaoLancarNota" runat="server" Checked='<%# Bind("tud_naoLancarNota") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkNaoExibirFrequencia" runat="server" Text="Não Exibir frequência"
                            ToolTip="Não exibe frequência no boletim." />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkItemNaoExibirFrequencia" runat="server" Checked='<%# Bind("tud_naoExibirFrequencia") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkNaoExibirNota" runat="server" Text="Não Exibir nota"
                            ToolTip="Não exibe frequência no boletim." />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkItemNaoExibirNota" runat="server" Checked='<%# Bind("tud_naoExibirNota") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                </asp:TemplateField>                
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkNaoExibirBoletim" runat="server" Text="Não Exibir no boletim"
                            ToolTip="Não exibe no boletim." />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkItemNaoExibirBoletim" runat="server" Checked='<%# Bind("tud_naoExibirBoletim") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkNaoLancarPlanejamento" runat="server" Text="Não Lançar planejamento"
                            ToolTip="Bloqueia lançamento de planejamento." />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkItemNaoLancarPlanejamento" runat="server" Checked='<%# Bind("tud_naoLancarPlanejamento") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkPermitirLancarAbonoFalta" runat="server" Text="Permitir lançar abono de falta"
                            ToolTip="Permitir lançar abono de falta." />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkItemPermitirLancarAbonoFalta" runat="server" Checked='<%# Bind("tud_permitirLancarAbonoFalta") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                </asp:TemplateField>
            </Columns>
            <HeaderStyle HorizontalAlign="Center" />
        </asp:GridView>
        <uc7:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="grvParametrosDisciplinas" />
        <asp:ObjectDataSource ID="odsParametrosDisciplinas" runat="server" TypeName="MSTech.GestaoEscolar.BLL.TUR_TurmaDisciplinaBO"
            SelectMethod="SelectBy_ConfiguracaoParametrosDisciplinas" MaximumRowsParameterName="pageSize" SelectCountMethod="GetTotalRecords"
            StartRowIndexParameterName="currentPage" EnablePaging="true" OnSelecting="odsParametrosDisciplinas_Selecting" >
        </asp:ObjectDataSource>
    </fieldset>
</asp:Content>
