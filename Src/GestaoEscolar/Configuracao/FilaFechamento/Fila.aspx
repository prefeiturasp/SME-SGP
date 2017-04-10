<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Fila.aspx.cs" Inherits="GestaoEscolar.Configuracao.FilaFechamento.Fila" %>

<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagPrefix="uc1" TagName="UCComboUAEscola" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCalendario.ascx" TagPrefix="uc1" TagName="UCCCalendario" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCursoCurriculo.ascx" TagPrefix="uc1" TagName="UCCCursoCurriculo" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCTurma.ascx" TagPrefix="uc1" TagName="UCComboTurma" %>
<%@ Register Src="~/WebControls/Combos/UCComboCurriculoPeriodo.ascx" TagPrefix="uc1" TagName="UCComboCurriculoPeriodo" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoCiclo.ascx" TagPrefix="uc1" TagName="UCComboTipoCiclo" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCPeriodoCalendario.ascx" TagPrefix="uc1" TagName="UCCPeriodoCalendario" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios" TagPrefix="uc9" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao" TagPrefix="uc6" %>
<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros" TagPrefix="uc7" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMensagem" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary" runat="server" ValidationGroup="" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <fieldset runat="server" style="margin-left: 10px;">
        <legend><%= GetGlobalResourceObject("Padrao", "Padrao.ParametrosBusca.Text").ToString() %></legend>
        <uc9:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" />
        <div id="divPesquisa" class="divPesquisa area-form" runat="server">
            <asp:Label ID="lblAvisoMensagem" runat="server"></asp:Label>
            <uc1:UCComboUAEscola runat="server" ID="UCComboUAEscola" AsteriscoObg="true" ObrigatorioEscola="true" ObrigatorioUA="true"
                CarregarEscolaAutomatico="true" MostraApenasAtivas="true" MostrarMessageSelecioneEscola="true" MostrarMessageSelecioneUA="true" />
            <uc1:UCCCalendario runat="server" ID="UCCCalendario" Obrigatorio="true" MostrarMensagemSelecione="true" PermiteEditar="false" />
            <uc1:UCCPeriodoCalendario runat="server" ID="UCCPeriodoCalendario" MostrarMensagemSelecione="true" Obrigatorio="true" PermiteEditar="false" />
            <uc1:UCCCursoCurriculo runat="server" ID="UCCCursoCurriculo" Obrigatorio="false" MostrarMensagemSelecione="true" PermiteEditar="false" />
            <uc1:UCComboCurriculoPeriodo runat="server" ID="UCComboCurriculoPeriodo" Obrigatorio="false" _MostrarMessageSelecione="true" PermiteEditar="false" />
            <uc1:UCComboTurma runat="server" ID="UCComboTurma" Obrigatorio="false" _MostrarMessageSelecione="true" PermiteEditar="false" />
            </div>
        <div class="right area-botoes-bottom">
            <asp:Button ID="btnPesquisar" runat="server" Text="<%$ Resources:Padrao, Padrao.Pesquisar.Text %>" OnClick="btnPesquisar_Click"
                ValidationGroup="" />
            <asp:Button ID="btnLimparPesquisa" runat="server" Text="<%$ Resources:Padrao, Padrao.LimparPesquisa.Text %>" OnClick="btnLimparPesquisa_Click" />
            <asp:Button ID="btnGerar" runat="server" Text="<%$ Resources:GestaoEscolar.Configuracao.FilaFechamento.Fila, btnGerar.Text %>" OnClick="btnGerar_Click" />
        </div>
    </fieldset>
    <%--Resultado da consulta--%>
    <fieldset id="fdsResultado" runat="server" visible="false">
        <legend><%= GetGlobalResourceObject("Padrao", "Padrao.Resultados.Text").ToString() %></legend>
        <uc6:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged"
            ComboDefaultValue="true" />
        <asp:GridView ID="grvFilaFechamento" runat="server" AutoGenerateColumns="False" DataSourceID="odsFilaFechamento"
            DataKeyNames="tud_id" AllowPaging="True"
            EmptyDataText="<%$ Resources:Padrao, Padrao.SemResultado.Text %>" AllowSorting="True" OnDataBound="grvFilaFechamento_DataBound">
                    <HeaderStyle HorizontalAlign="Left" />
            <Columns>
                <asp:BoundField HeaderText="<%$ Resources:GestaoEscolar.Configuracao.FilaFechamento.Fila, ctrl_52.HeaderText %>" DataField="tud_nome" SortExpression="tud_nome"></asp:BoundField>
                <asp:TemplateField HeaderStyle-CssClass="center" ItemStyle-CssClass="center">
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkGerarFilaNota" runat="server" Text="<%$ Resources:GestaoEscolar.Configuracao.FilaFechamento.Fila, chkGerarFilaNota.Text %>" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox style="text-align:center" ID="chkItemGerarFilaNota" runat="server" Visible='<%# Convert.ToBoolean(Eval("tud_naoLancarNota")) ? false : true %>'/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-CssClass="center" ItemStyle-CssClass="center">
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkGerarFilaFrequencia" runat="server" Text="<%$ Resources:GestaoEscolar.Configuracao.FilaFechamento.Fila, chkGerarFilaFrequencia.Text %>" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox style="text-align:center" ID="chkItemGerarFilaFrequencia" runat="server" Visible='<%# Convert.ToBoolean(Eval("tud_naoLancarFrequencia")) ? false : true %>'/>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <uc7:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="grvFilaFechamento" />
        <asp:ObjectDataSource ID="odsFilaFechamento" runat="server" TypeName="MSTech.GestaoEscolar.BLL.TUR_TurmaDisciplinaBO"
            SelectMethod="GetSelectBy_EscolaCalendarioTurma" MaximumRowsParameterName="pageSize" SelectCountMethod="GetTotalRecords"
            StartRowIndexParameterName="currentPage" EnablePaging="true" OnSelecting="odsFilaFechamento_Selecting"></asp:ObjectDataSource>
    </fieldset>
</asp:Content>
