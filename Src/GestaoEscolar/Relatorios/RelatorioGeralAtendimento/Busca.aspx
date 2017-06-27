<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Relatorios.RelatorioGeralAtendimento.Busca" %>

<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagPrefix="uc1" TagName="UCCamposObrigatorios" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoRelatorioAtendimento.ascx" TagPrefix="uc2" TagName="UCComboTipoRelatorioAtendimento" %>
<%@ Register Src="~/WebControls/Combos/UCComboRelatorioAtendimento.ascx" TagPrefix="uc3" TagName="UCCRelatorioAtendimento" %>
<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="UCComboUAEscola" TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCalendario.ascx" TagName="UCCCalendario" TagPrefix="uc5" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCurriculoPeriodo.ascx" TagName="UCComboCurriculoPeriodo" TagPrefix="uc6" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCursoCurriculo.ascx" TagName="UCComboCursoCurriculo" TagPrefix="uc7" %>
<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros" TagPrefix="uc8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="_updMessage" runat="server">
        <ContentTemplate>
            <asp:Label ID="_lblMessage" runat="server" EnableViewState="false"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="_updPesquisa" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset id="_fdsPesquisa" runat="server" style="margin-left: 10px;">
                <legend>Parâmetros de busca</legend>
                <uc1:UCCamposObrigatorios ID="_UCCamposObrigatorios" runat="server" />
                <uc2:UCComboTipoRelatorioAtendimento runat="server" ID="_UCComboTipoRelatorioAtendimento" 
                    MostrarMensagemSelecione="true" Obrigatorio="true" Titulo="Tipo de relatório"/>
                <uc3:UCCRelatorioAtendimento ID="_UCCRelatorioAtendimento" runat="server" Obrigatorio="true"/>
                <uc4:UCComboUAEscola ID="_UCComboUAEscola" runat="server" MostrarMessageSelecioneUA="true" 
                    MostrarMessageSelecioneEscola="true" CarregarEscolaAutomatico="true" ObrigatorioUA="true" ObrigatorioEscola="true" />
                <uc7:UCComboCursoCurriculo ID="_UCComboCursoCurriculo" MostrarMensagemSelecione="true" runat="server" 
                    Obrigatorio="true"/>
                <uc6:UCComboCurriculoPeriodo ID="_UCComboCurriculoPeriodo" MostrarMensagemSelecione="true" runat="server" 
                    Obrigatorio="true"/>
                <uc5:UCCCalendario ID="_UCComboCalendario" runat="server" MostrarMensagemSelecione="true" 
                    Obrigatorio="true"/>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="right area-botoes-bottom">
        <asp:Button ID="_btnPesquisar" runat="server" Text="" OnClick="_btnPesquisar_Click"
            CausesValidation="false" />
        <asp:Button ID="_btnLimparPesquisa" runat="server" Text="" OnClick="_btnLimparPesquisa_Click"
            CausesValidation="false" />
    </div>
    <asp:UpdatePanel ID="_updResultado" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset id="_fdsResultado" runat="server">
                <legend>Resultados</legend>
                <div class="right area-botoes-top">
                    <asp:Button ID="_btnGerarRelatorioCima" runat="server" Text="Gerar relatório" OnClick="_btnGerarRelatorioCima_Click" />
                </div>
                <div class="area-form">
                    <br />
                    <br />
                    <div id="DivSelecionaTodos" runat="server">
                        <div style="float: left; width: 50%">
                            <asp:CheckBox ID="_chkTodos" SkinID="chkTodos" Text="Selecionar todos os alunos"
                                runat="server" />
                        </div>
                        <div align="right" id="divQtdPaginacao" runat="server">
                            <asp:Label ID="_lblPag" runat="server" Text="Itens por página"></asp:Label>
                            <asp:DropDownList ID="_ddlQtPaginado" runat="server" AutoPostBack="True" OnSelectedIndexChanged="_ddlQtPaginado_SelectedIndexChanged1">
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>20</asp:ListItem>
                                <asp:ListItem>50</asp:ListItem>
                                <asp:ListItem>100</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <br />
                    <asp:GridView ID="_grvAlunos" runat="server" AllowPaging="True" AllowCustomPaging="true" AutoGenerateColumns="False"
                        BorderStyle="None" DataKeyNames=""
                        EmptyDataText="A pesquisa não encontrou resultados."
                        OnRowDataBound="_grvAlunos_RowDataBound" OnDataBound="_grvAlunos_DataBound"
                        AllowSorting="True" OnPageIndexChanging="_grvAlunos_PageIndexChanging" OnSorting="_grvAlunos_Sorting"
                        EnableModelValidation="True" SkinID="GridResponsive">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="_chkSelecionar" runat="server" alu_id='<%# Eval("alu_id") %>' cal_id='<%# Eval("cal_id") %>'
                                        esc_id='<%# Eval("esc_id") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%-- Fields --%>
                        </Columns>
                    </asp:GridView>
                    <uc8:UCTotalRegistros id="_UCTotalRegistros" runat="server" associatedgridviewid="_grvAlunos"
                        class="clTotalReg" />
                </div>
                <div class="right area-botoes-bottom">
                    <asp:Button ID="_btnGerarRelatorio" runat="server" Text="Gerar relatório" OnClick="_btnGerarRelatorio_Click" />
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
