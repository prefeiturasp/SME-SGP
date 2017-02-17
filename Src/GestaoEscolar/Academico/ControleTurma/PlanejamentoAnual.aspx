<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="PlanejamentoAnual.aspx.cs" Inherits="GestaoEscolar.Academico.ControleTurma.PlanejamentoAnual" %>
<%@ PreviousPageType VirtualPath="~/Academico/ControleTurma/Busca.aspx" %>

<%@ Register Src="~/WebControls/ControleTurma/UCControleTurma.ascx" TagName="UCControleTurma" TagPrefix="uc10" %>
<%@ Register Src="~/WebControls/NavegacaoTelaPeriodo/UCNavegacaoTelaPeriodo.ascx" TagName="UCNavegacaoTelaPeriodo" TagPrefix="uc13" %>
<%@ Register src="~/WebControls/PlanejamentoAnual/UCPlanejamentoAnual.ascx" tagname="UCPlanejamentoAnual" tagprefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCPosicaoDocente.ascx" TagName="UCCPosicaoDocente" TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/PlanejamentoAnual/UCPlanejamentoProjetos.ascx" TagName="UCPlanejamentoProjetos" TagPrefix="uc3" %>
<%@ Register src="~/WebControls/ControleTurma/UCSelecaoDisciplinaCompartilhada.ascx" tagname="UCSelecaoDisciplinaCompartilhada" tagprefix="uc10" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .ui-widget
        {
            font-size: 1em !important;
        }
        .ui-widget ul.ui-tabs-nav
        {
            font-size: 1.2em !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:UpdatePanel ID="updMensagem" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <fieldset>
        <uc10:UCControleTurma ID="UCControleTurma1" runat="server"/>

        <div runat="server" id="divMessageTurmaAnterior"
            class="summaryMsgAnosAnteriores" style="<%$ Resources: Academico, ControleTurma.Busca.divMessageTurmaAnterior.Style %>">
            <asp:Label runat="server" ID="lblMessageTurmaAnterior" Text="<%$ Resources:Academico, ControleTurma.Busca.lblMessageTurmaAnterior.Text %>"
                Style="<%$ Resources: Academico, ControleTurma.Busca.lblMessageTurmaAnterior.Style %>"></asp:Label>
        </div>

        <!-- Botões de navegação -->
        <uc13:ucnavegacaotelaperiodo id="UCNavegacaoTelaPeriodo" runat="server" />
        
        <div style="margin-top: 10px;">
            
            <!-- Guia Planejamento anual -->
            <asp:Panel ID="pnlPlanejamentoAnual" runat="server">
                <fieldset>
                    <asp:Panel ID="pnlTurmaDisciplinaPlanAnual" runat="server">
                        <asp:Label ID="lblTurmaDisciplinaPlanAnual" runat="server" Text="<%$ Resources:Academico, ControleTurma.PlanejamentoAnual.lblTurmaDisciplinaPlanAnual.Text %>" AssociatedControlID="ddlTurmaDisciplinaPlanAnual"></asp:Label>
                        <asp:DropDownList ID="ddlTurmaDisciplinaPlanAnual" runat="server" AppendDataBoundItems="True"
                            AutoPostBack="true" DataTextField="tur_tud_nome" DataValueField="tur_tud_id"
                            SkinID="text60C" OnSelectedIndexChanged="ddlTurmaDisciplinaPlanAnual_SelectedIndexChanged">
                        </asp:DropDownList>
                        <br /><br />
                    </asp:Panel>
                    <div class="right area-botoes-top">
                        <asp:Button ID="btnSalvarPlanejamentoAnualCima" runat="server" Text="Salvar" OnClick="btnSalvarPlanejamentoAnual_Click" />
                        <asp:Button ID="btnCancelarPlanejamentoAnualCima" runat="server" Text="Cancelar" OnClick="btnCancelarPlanejamentoAnual_Click" />
                        <asp:Button ID="btnImprimirPlanejamentoAnualCima" runat="server" Text="Imprimir" ToolTip="Imprimir o relatório de planejamento de aula da turma" CausesValidation="false" OnClick="btnImprimirPlanejamentoAnual_Click" />
                    </div>
                </fieldset>
                <fieldset class="area-form">
                    <uc1:UCPlanejamentoAnual ID="UCPlanejamentoAnual" runat="server" />
                    <uc3:UCPlanejamentoProjetos ID="UCPlanejamentoProjetos" runat="server" />
                </fieldset>
                <fieldset id="divBotoes" class="area-botoes-bottom">
                    <div class="right">
                        <asp:Button ID="btnSalvarPlanejamentoAnual" runat="server" Text="Salvar" OnClick="btnSalvarPlanejamentoAnual_Click" />
                        <asp:Button ID="btnCancelarPlanejamentoAnual" runat="server" Text="Cancelar" OnClick="btnCancelarPlanejamentoAnual_Click" />
                        <asp:Button ID="btnImprimirPlanejamentoAnual" runat="server" Text="Imprimir" ToolTip="Imprimir o relatório de planejamento de aula da turma" CausesValidation="false" OnClick="btnImprimirPlanejamentoAnual_Click" />
                    </div>
                </fieldset>
            </asp:Panel>            
        </div>

    </fieldset>
    <!-- Filtro disciplina relatório -->
    <div id="divSelecionaDisciplina" runat="server" class="hide divSelecionaDisciplina">
        <asp:UpdatePanel ID="updSelecionaDisciplina" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset>
                    <asp:GridView ID="grvDisciplinas" runat="server" AutoGenerateColumns="False" DataKeyNames="tud_id"
                        EmptyDataText="Não existem disciplinas cadastradas."
                        OnRowCommand="grvDisciplinas_RowCommand" OnRowDataBound="grvDisciplinas_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ Resources:Mensagens, MSG_DISCIPLINA %>">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("tud_nome") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnSelecionar" runat="server" CommandName="Imprimir" Text='<%# Bind("tud_nome") %>'
                                        CausesValidation="False"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="pes_nome" HeaderText="Docente" />
                        </Columns>
                    </asp:GridView>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>    
    <div id="divFiltrosRelatorio" title="Selecione pelo menos uma opção que deseja imprimir"
        class="hide">
        <asp:UpdatePanel ID="uppFiltrosRelatorio" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset>
                    <asp:Label ID="lblMensagemRelatorio" runat="server"></asp:Label>
                    <div>
                        <uc2:UCCPosicaoDocente ID="UCCPosicaoDocente1" runat="server" MostrarMensagemSelecione="false" />
                        <br />
                        <asp:CheckBox ID="ckbPlanejamentoAnualRel" runat="server" Text="Diagnóstico inicial / Proposta"
                            Value="0"></asp:CheckBox>
                        <asp:CheckBox ID="ckbPlanejamentoPeriodoRel" runat="server" Text="Planejamento periódico"
                            Value="1"></asp:CheckBox>
                        <asp:CheckBox ID="ckbAulasRel" runat="server" Text="Aulas" Value="2" OnClick="javascript:MostraCkbPeriodo(this.checked);"></asp:CheckBox>
                        <div id="divCkb" class="paddingDiv">
                            <div id="divCkbServer" runat="server" style="display: none">
                                <asp:CheckBox ID="ckbTodasAulas" runat="server" Text="Todas as aulas" OnClick="javascript:CheckTodasAulas(this.checked);"
                                    Class="uncheck"></asp:CheckBox>
                                <asp:CheckBox ID="ckbPeriodoRel" runat="server" Text="Informar período" OnClick="javascript:MostraTextBox(this.checked);"
                                    Class="uncheck"></asp:CheckBox>
                                <div id="divInicioFim" class="paddingDiv">
                                    <div id="divInicioFimServer" runat="server" style="display: none">
                                                    <asp:Label ID="lblInicioRel" Text="Início" runat="server" AssociatedControlID="txtInicioRel"></asp:Label>
                                                    <asp:TextBox ID="txtInicioRel" runat="server" SkinID="Data"></asp:TextBox>
                                                    <asp:CustomValidator ID="cvInicioRel" runat="server" ControlToValidate="txtInicioRel"
                                                        ValidationGroup="DataAulas" Display="Dynamic" ErrorMessage="">* </asp:CustomValidator>

                                                    <asp:Label ID="lblFimRel" Text="Fim" runat="server" AssociatedControlID="txtFimRel"></asp:Label>
                                                    <asp:TextBox ID="txtFimRel" runat="server" SkinID="Data"></asp:TextBox>
                                                    <asp:CustomValidator ID="cvFimRel" runat="server" ControlToValidate="txtFimRel" ValidationGroup="DataAulas"
                                                        Display="Dynamic" ErrorMessage="">*</asp:CustomValidator>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <asp:CheckBox ID="ckbAvaliacoesRel" runat="server" Text="Atividades" Value="3"></asp:CheckBox>
                    </div>
                    <div class="right">
                        <asp:Button ID="btnGerar" runat="server" Text="Imprimir" ValidationGroup="DataAulas"
                            OnClick="btnGerar_Click" CausesValidation="true" />
                        <asp:Button ID="Button2" runat="server" CausesValidation="false" Text="Cancelar"
                            OnClientClick="$('#divFiltrosRelatorio').dialog('close'); return false;" />
                    </div>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divReplicarPlanejamento" title="Replicar planejamento anual" class="hide">
        <fieldset>
            <asp:Label ID="lblMensagemReplicacao" runat="server" Text="Selecione abaixo a(s) turma(s) para as quais esse planejamento deve ser replicado:"></asp:Label>
            <br />
            <br />
            <asp:CheckBoxList ID="chkTurmas" runat="server" DataTextField="tur_codigo" DataValueField="tur_id"></asp:CheckBoxList>
            <div class="right">
                <asp:Button ID="btnReplicar" runat="server" Text="Salvar" OnClick="btnReplicar_Click" />
                <asp:Button ID="btnCancelarReplicar" runat="server" Text="Cancelar" CausesValidation="false" OnClientClick='$("#divReplicarPlanejamento").dialog("close"); return false;' />
            </div>
        </fieldset>
    </div>

    <%-- Disciplinas compartilhadas --%>
    <uc10:UCSelecaoDisciplinaCompartilhada ID="UCSelecaoDisciplinaCompartilhada1" runat="server"></uc10:UCSelecaoDisciplinaCompartilhada>
    <asp:HiddenField ID="hdnValorTurmas" runat="server" />

</asp:Content>
