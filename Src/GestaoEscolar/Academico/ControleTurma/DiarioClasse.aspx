<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeBehind="DiarioClasse.aspx.cs"
    Inherits="GestaoEscolar.Academico.ControleTurma.DiarioClasse" %>

<%@ PreviousPageType VirtualPath="~/Academico/ControleTurma/Busca.aspx" %>

<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboOrdenacao.ascx" TagName="UCComboOrdenacao" TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoAtividadeAvaliativa.ascx" TagName="UCComboTipoAtividadeAvaliativa" TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/ControleTurma/UCControleTurma.ascx" TagName="UCControleTurma" TagPrefix="uc10" %>
<%@ Register Src="~/WebControls/NavegacaoTelaPeriodo/UCNavegacaoTelaPeriodo.ascx" TagName="UCNavegacaoTelaPeriodo" TagPrefix="uc13" %>
<%@ Register Src="~/WebControls/Habilidades/UCHabilidades.ascx" TagName="UCHabilidades" TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Mensagens/UCConfirmacaoOperacao.ascx" TagName="UCConfirmacaoOperacao" TagPrefix="uc5" %>
<%@ Register Src="~/WebControls/ControleTurma/UCSelecaoDisciplinaCompartilhada.ascx" TagName="UCSelecaoDisciplinaCompartilhada" TagPrefix="uc10" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var idlblMessage3 = '#<%=lblMessage3.ClientID %>';
        var idDivAtividadeCasa = '#<%=divAtividadeCasa.ClientID%>';
        var idChkRecursos = '#<%=chkRecursos.ClientID%>';
        var idtxtOutroRecurso = '#<%=txtOutroRecurso.ClientID%>';
        var idDdlOrdenacaoFrequencia = '#<%=UCComboOrdenacao.ComboClientID%>';
        var idDdlOrdenacaoAvaliacao = '#<%=UCComboOrdenacao2.ComboClientID%>';

        function setaDisplayCss(id, mostra) {
            $(id).css({ 'display': mostra ? '' : 'none' });
        }

        function SetaAtividadePraCasa() {
            var valor = $('#<%=chkAtividadeCasa.ClientID%>').attr('checked');

            setaDisplayCss(idDivAtividadeCasa, valor);

            if (valor) {
                $(idDivAtividadeCasa).find('textarea').attr('value', '').focus();
            }
        }
    </script>
    <style type="text/css">
        #divAlunos td {
            padding: 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            <asp:Label ID="lblPeriodoEfetivado" runat="server" EnableViewState="false" Visible="false"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <fieldset>
        <uc10:UCControleTurma ID="UCControleTurma1" runat="server" />

        <div runat="server" id="divMessageTurmaAnterior"
            class="summaryMsgAnosAnteriores" style="<%$ Resources: Academico, ControleTurma.Busca.divMessageTurmaAnterior.Style %>">
            <asp:Label runat="server" ID="lblMessageTurmaAnterior" Text="<%$ Resources:Academico, ControleTurma.Busca.lblMessageTurmaAnterior.Text %>"
                Style="<%$ Resources: Academico, ControleTurma.Busca.lblMessageTurmaAnterior.Style %>"></asp:Label>
        </div>

        <!-- Botões de navegação -->
        <uc13:UCNavegacaoTelaPeriodo ID="UCNavegacaoTelaPeriodo" runat="server" />

        <div style="margin-top: 10px;">
            <asp:UpdatePanel ID="upnDiarioClasse" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <asp:Panel ID="pnlDiarioClasse" runat="server">
                        <asp:Label ID="lblMessage2" runat="server" EnableViewState="false"></asp:Label>
                        <div>
                            <br />
                            <asp:Button ID="btnIncluirAula" runat="server" Text="Incluir aula" OnClick="btnIncluirAula_Click" />
                            <asp:Button ID="btnRelatorioFrequencia" runat="server" Text="DC - frequência" OnClick="btnRelatorioFrequencia_Click" />
                            <asp:Button ID="btnRelatorioAvaliacao" runat="server" Text="DC - avaliação" OnClick="btnRelatorioAvaliacao_Click" />
                        </div>
                        <br />
                        <asp:GridView ID="grvAulas" runat="server" AutoGenerateColumns="false" OnRowCommand="grvAulas_RowCommand"
                            OnRowDataBound="grvAulas_RowDataBound" OnDataBound="grvAulas_DataBound" SkinID="GridResponsive"
                            EmptyDataText="Não foram encontradas aulas."
                            DataKeyNames="tau_id, tau_data, tdt_posicao, tdc_corDestaque, pes_nome, tdc_id, tau_efetivado,
                                          permissaoAlteracao, usu_id,  EventoSemAtividade, tau_statusFrequencia, tau_statusAtividadeAvaliativa
                                          , tau_statusAnotacoes, tau_statusPlanoAula, tud_id, tud_tipo, NomeDisciplinaRelacionada, tud_global
                                          , tau_reposicao, semPlanoAula">
                            <Columns>
                                <asp:TemplateField HeaderText="Data da aula">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDataAulaAlterar" runat="server" CommandArgument='<%# Container.DataItemIndex %>' CommandName="EditarAula" CausesValidation="False"
                                            Text='<%# Bind("tau_data") %>'></asp:LinkButton>
                                        <asp:Label ID="lblAula" runat="server" Text='<%# Bind("tau_data") %>' Visible="false"></asp:Label>
                                        <asp:Image ID="imgEventoSemAtividade" runat="server" SkinID="imgStatusAlertaPendencia" Visible="false"
                                            ToolTip="Existe um evento cadastrado sem atividade discente para esse dia." Width="16px" Height="16px" ImageAlign="Top" />
                                        <asp:Image ID="imgAvisoSubstituto" runat="server" SkinID="imgAviso" Visible="false" Width="16px" Height="16px" ImageAlign="Top" />
                                        <asp:Label ID="lblAulaReposicao" runat="server" Text=' - Aula de reposição' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="center" HorizontalAlign="Center" />
                                    <ItemStyle CssClass="center" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="<%$ Resources:Academico, ControleTurma.DiarioClasse.lblQtdeAulasHeader.Text %>" DataField="tau_numeroAulas" />
                                <asp:TemplateField HeaderText="Frequência">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnFrequencia" runat="server" SkinID="btDetalhar" CommandName="LancarFrequencia"
                                            CommandArgument='<%# Container.DataItemIndex %>'
                                            ToolTip="Lançar frequência da aula para os alunos" />
                                        <asp:Image ID="imgFrequenciaSituacaoEfetivada" runat="server" SkinID="imgConfirmar" ToolTip="Frequência efetivada"
                                            Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                                        <asp:Image ID="imgFrequenciaSituacao" runat="server" SkinID="imgConfirmarAmarelo" ToolTip="Frequência preenchida"
                                            Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="center" HorizontalAlign="Center" />
                                    <ItemStyle CssClass="center grid-responsive-item-inline grid-responsive-center" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ativ. avaliativa">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnAtividade" runat="server" SkinID="btRelatorio" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Atividade" ToolTip='<%# "Adicionar ou editar " + MSTech.GestaoEscolar.BLL.GestaoEscolarUtilBO.nomePadraoAtividadeAvaliativa(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() %>' />
                                        <asp:Image ID="imgAtividadeSituacaoEfetivada" runat="server" SkinID="imgConfirmar" ToolTip="Atividade efetivada"
                                            Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                                        <asp:Image ID="imgAtividadeSituacao" runat="server" SkinID="imgConfirmarAmarelo" ToolTip="Atividade preenchida"
                                            Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="center" HorizontalAlign="Center" />
                                    <ItemStyle CssClass="center grid-responsive-item-inline grid-responsive-center" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Anotações">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnAnotacao" runat="server" SkinID="btFormulario" CommandName="AnotacoesAluno"
                                            CommandArgument='<%# Container.DataItemIndex %>'
                                            ToolTip="Adicionar ou editar anotações da aula para os alunos" />
                                        <asp:Image ID="imgAnotacaoSituacao" runat="server" SkinID="imgConfirmar" ToolTip="Anotações preenchidas"
                                            Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="center" HorizontalAlign="Center" />
                                    <ItemStyle CssClass="center grid-responsive-item-inline grid-responsive-center" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Plano de aula">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnPlanoAula" runat="server" SkinID="btAulas" CommandArgument='<%# Container.DataItemIndex %>' CommandName="PlanoAula" ToolTip="Adicionar ou editar o plano de aula" />
                                        <asp:Image ID="imgPlanoAulaSituacao" runat="server" SkinID="imgConfirmar" ToolTip="<%$ Resources:Academico, ControleTurma.DiarioClasse.imgPlanoAulaSituacao %>"
                                            Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                                        <asp:Image ID="imgPlanoAulaSituacaoIncompleta" runat="server" SkinID="imgConfirmarAmarelo" Visible="false"
                                            ToolTip="<%$ Resources:Academico, ControleTurma.DiarioClasse.imgPlanoAulaSituacaoIncompleta %>" Width="16px" Height="16px" ImageAlign="Top" />
                                        <asp:Image ID="imgSemPlanoAula" runat="server" SkinID="imgStatusAlertaAulaSemPlano" Visible="false" ImageAlign="Top" Width="20px" Height="20px" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="center" HorizontalAlign="Center" />
                                    <ItemStyle CssClass="center grid-responsive-item-inline grid-responsive-center" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Excluir aula">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnExcluir" runat="server" SkinID="btExcluirSemMensagem" CommandArgument='<%# Container.DataItemIndex %>'
                                            CommandName="ExcluirAula" ToolTip="Excluir aula" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="center" HorizontalAlign="Center" />
                                    <ItemStyle CssClass="center" HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <div id="divLegendaDiario" runat="server" class="legenda" visible="false">
                            <b>Legenda:</b>
                            <div class="borda-legenda">
                                <asp:Repeater ID="rptLegendaDiario" runat="server" OnItemDataBound="rptLegendaDiario_ItemDataBound">
                                    <HeaderTemplate>
                                        <ul style="">
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <li>
                                            <i id="tdCorLegendaDiario" runat="server"></i>
                                            <asp:Label ID="lblLegendaDiario" runat="server"></asp:Label>
                                        </li>
                                    </ItemTemplate>
                                    <FooterTemplate></ul></FooterTemplate>
                                </asp:Repeater>
                            </div>
                            <br />
                        </div>
                        <div id="divEventoSemAtividade" runat="server" style="float: left; width: 100%;" visible="false">
                            <asp:Image ID="imgLegendaEventoSemAtividade" runat="server" SkinID="imgStatusAlertaPendencia"
                                ToolTip="Existe um evento cadastrado sem atividade discente para esse dia." Width="16px" Height="16px" ImageAlign="Top" />
                            <asp:Literal ID="lit" runat="server" Text="<%$ Resources:Academico, ControleTurma.DiarioClasse.MensagemEventoSemAtivDiscente %>"></asp:Literal>
                        </div>
                        <div id="divAvisoSubstituto" runat="server" style="float: left; width: 100%;" visible="false">
                            <asp:Image ID="imgLegendaAvisoSubstituto" runat="server" SkinID="imgAviso" Width="16px" Height="16px" ImageAlign="Top"
                                ToolTip="<%$ Resources:Academico, ControleTurma.DiarioClasse.MensagemSubstitutoRegencia %>" />
                            <asp:Literal ID="lit2" runat="server" Text="<%$ Resources:Academico, ControleTurma.DiarioClasse.MensagemSubstitutoRegencia %>"></asp:Literal>
                        </div>
                        <div id="divAvisoAulaSemPlano" runat="server" style="float: left; width: 100%;" visible="false">
                            <asp:Image ID="imgLegendaAvisoAulaSemPlano" runat="server" SkinID="imgStatusAlertaAulaSemPlano"
                                ToolTip="<%$ Resources:Academico, ControleTurma.DiarioClasse.MensagemAulaSemPlanoAula %>" Width="18px" Height="18px" ImageAlign="AbsMiddle" />
                            <asp:Literal ID="lit3" runat="server" Text="<%$ Resources:Academico, ControleTurma.DiarioClasse.MensagemAulaSemPlanoAula %>"></asp:Literal>
                        </div>

                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </fieldset>

    <div id="divCadastroAula" class="hide" title="Incluir Aula">
        <asp:UpdatePanel ID="updCadastroAula" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset style="top: 0px; left: 0px">
                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSalvarAula">
                        <asp:ValidationSummary ID="vsAulas" runat="server" ValidationGroup="IncluiAula" />
                        <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios3" runat="server" />

                        <asp:Label ID="lblMessage3" runat="server" EnableViewState="false" />

                        <asp:Label ID="lblData" runat="server" Text="Data da aula *" AssociatedControlID="txtDataAula"></asp:Label>
                        <asp:TextBox ID="txtDataAula" runat="server" SkinID="Data"></asp:TextBox>

                        <div id="divQtdeAulas">
                            <asp:Label ID="lblQtdeAulas" runat="server" Text="<%$ Resources:Academico, ControleTurma.DiarioClasse.lblQtdeAulas.Text %>" AssociatedControlID="txtQtdeAulas"></asp:Label>
                            <asp:TextBox ID="txtQtdeAulas" runat="server" SkinID="Numerico" MaxLength="4"></asp:TextBox><br />
                        </div>
                        <asp:CheckBox ID="chkReposicao" runat="server" Text="Aula de reposição" />
                        <div class="right">
                            <asp:Button ID="btnSalvarAula" runat="server" Text="Salvar" ValidationGroup="IncluiAula" OnClick="btnSalvarAula_Click" OnClientClick="$('#divCadastroAula').scrollTo(0,0);javascript:scroll(0,0);" />
                            <asp:Button ID="btnCancelarAula" runat="server" Text="Cancelar" OnClientClick="var exibirMensagemConfirmacao=true;$('#divCadastroAula').dialog('close'); return false;" />
                        </div>
                        <asp:HiddenField runat="server" ID="hdfIsNewAula" Value="true" />
                    </asp:Panel>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div id="divLancamentoFrequencia" title="Lançamento de frequência" class="hide">
        <asp:UpdatePanel ID="updMessagesFrequencia" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblMessageFrequencia" runat="server" EnableViewState="False"></asp:Label>
                <asp:ValidationSummary ID="vsFrequencia" runat="server" ValidationGroup="Frequencia" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updFrequencia" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset id="fdsLancamentoFrequencia" runat="server">
                    <asp:Panel ID="pnlLancamentoFrequencia" runat="server" DefaultButton="btnSalvarFrequencia">
                        <uc2:UCComboOrdenacao ID="UCComboOrdenacao" runat="server" />
                        <asp:Label ID="lblInformacaoFrequencia" runat="server"></asp:Label>
                        <asp:Label ID="lblDataAula" runat="server" Text="Aula"></asp:Label>
                        <div class="right">
                            <asp:Button ID="btnSalvarFrequenciaCima" runat="server" ValidationGroup="Frequencia"
                                Text="Salvar" OnClick="btnSalvarFrequencia_Click" OnClientClick="$('#divLancamentoFrequencia').scrollTo(0,0);javascript:scroll(0,0);" />
                            <asp:Button ID="btnCancelarFrequenciaCima" runat="server" Text="Cancelar" OnClientClick="var exibirMensagemConfirmacao=true;$('#divLancamentoFrequencia').dialog('close'); return false;" />
                        </div>
                        <br />
                        <asp:Repeater ID="rptDiarioAlunosFrequencia" runat="server" OnItemDataBound="rptDiarioAlunosFrequencia_ItemDataBound">
                            <HeaderTemplate>
                                <div>
                                    <table id="tabela" class="grid grid-responsive-list" cellspacing="0">
                                        <thead>
                                            <tr class="gridHeader" style="height: 30px;">
                                                <th class="center">
                                                    <asp:Label ID="_lblNumChamada" runat="server" Text='Nº Chamada'></asp:Label>
                                                </th>
                                                <th>
                                                    <asp:Label ID="lblNome" runat="server" Text='Nome do aluno'></asp:Label>
                                                </th>
                                                <asp:Repeater ID="rptDiarioAulas" runat="server" OnItemDataBound="rpDiarioAulasHeader_ItemDataBound">
                                                    <ItemTemplate>
                                                        <th class="center {sorter:false}">
                                                            <asp:Label ID="lbltau_id" runat="server" Text='<%#Bind("tau_id") %>' Visible="false">
                                                            </asp:Label>
                                                            <asp:HiddenField ID="hdnPosicao" runat="server" Value='<%#Bind("tdt_posicao") %>' />
                                                            <asp:Label ID="lbltnt_data" runat="server" Text='Ausência'>
                                                            </asp:Label>
                                                        </th>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tr>
                                            <tr class="gridRow grid-linha-destaque">
                                                <td class="grid-responsive-no-header"></td>
                                                <td class="grid-responsive-no-header"></td>
                                                <asp:Repeater ID="rptDiarioAulasEfetivado" runat="server" OnItemDataBound="rpDiarioAulasHeader_ItemDataBound">
                                                    <ItemTemplate>
                                                        <td class="center {sorter:false} grid-responsive-item-inline grid-responsive-center">
                                                            <asp:Label ID="lbltau_id" runat="server" Text='<%#Bind("tau_id") %>' Visible="false">
                                                            </asp:Label>
                                                            <asp:HiddenField ID="hdnPosicao" runat="server" Value='<%#Bind("tdt_posicao") %>' />
                                                            <asp:Label ID="lbltnt_data" runat="server" Text='Ausência' style="display:none;">
                                                            </asp:Label>
                                                            <asp:CheckBox ID="chkEfetivado" runat="server" Text="Efetivado" style="text-align:center;" />
                                                        </td>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tr>
                                        </thead>
                                        <tbody>
                            </HeaderTemplate>
                            <AlternatingItemTemplate>
                                <tr class="gridAlternatingRow">
                                    <td style="text-align: center;" runat="server" id="tdNumeroChamada">
                                        <asp:Label ID="lblalu_id" runat="server" Text='<%#Bind("alu_id") %>' Visible="false">
                                        </asp:Label>
                                        <asp:Label ID="lblmtu_id" runat="server" Text='<%#Bind("mtu_id") %>' Visible="false">
                                        </asp:Label>
                                        <asp:Label ID="lblmtd_id" runat="server" Text='<%#Bind("mtd_id") %>' Visible="false">
                                        </asp:Label>
                                        <asp:Label ID="lblAtividade" runat="server" Text='<%#Bind("numeroChamada") %>'>
                                        </asp:Label>
                                    </td>
                                    <td id="tdNomeAluno" runat="server">
                                        <asp:Label ID="lblNome" runat="server" Text='<%#Bind("pes_nome") %>'>
                                        </asp:Label>
                                    </td>

                                    <asp:Repeater ID="rptDiarioAulas" runat="server" OnItemDataBound="rptDiarioAulas_ItemDataBound">
                                        <ItemTemplate>
                                            <td style="text-align: center;" runat="server" id="tdAulas">
                                                <div id="divAulasAluno" runat="server" style="display: inline-block; width: 100%;" class="responsive-width-auto">
                                                    <asp:Label ID="lbltau_id" runat="server" Text='<%#Bind("tau_id") %>' Visible="false">
                                                    </asp:Label>
                                                    <asp:HiddenField ID="hdnPosicao" runat="server" Value='<%#Bind("tdt_posicao") %>' />
                                                    <asp:CheckBoxList ID="cblFrequencia" runat="server" RepeatDirection="Horizontal"
                                                        RepeatLayout="Flow">
                                                    </asp:CheckBoxList>
                                                </div>
                                            </td>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tr>
                            </AlternatingItemTemplate>
                            <ItemTemplate>
                                <tr class="gridRow">
                                    <td style="text-align: center;" runat="server" id="tdNumeroChamada">
                                        <asp:Label ID="lblalu_id" runat="server" Text='<%#Bind("alu_id") %>' Visible="false">
                                        </asp:Label>
                                        <asp:Label ID="lblmtu_id" runat="server" Text='<%#Bind("mtu_id") %>' Visible="false">
                                        </asp:Label>
                                        <asp:Label ID="lblmtd_id" runat="server" Text='<%#Bind("mtd_id") %>' Visible="false">
                                        </asp:Label>
                                        <asp:Label ID="lblAtividade" runat="server" Text='<%#Bind("numeroChamada") %>'>
                                        </asp:Label>
                                    </td>
                                    <td id="tdNomeAluno" runat="server">
                                        <asp:Label ID="lblNome" runat="server" Text='<%#Bind("pes_nome") %>'>
                                        </asp:Label>
                                    </td>

                                    <asp:Repeater ID="rptDiarioAulas" runat="server" OnItemDataBound="rptDiarioAulas_ItemDataBound">
                                        <ItemTemplate>
                                            <td style="text-align: center;" runat="server" id="tdAulas">
                                                <div id="divAulasAluno" runat="server" style="display: inline-block; width: 100%;" class="responsive-width-auto">
                                                    <asp:Label ID="lbltau_id" runat="server" Text='<%#Bind("tau_id") %>' Visible="false">
                                                    </asp:Label>
                                                    <asp:HiddenField ID="hdnPosicao" runat="server" Value='<%#Bind("tdt_posicao") %>' />
                                                    <asp:CheckBoxList ID="cblFrequencia" runat="server" RepeatDirection="Horizontal"
                                                        RepeatLayout="Flow">
                                                    </asp:CheckBoxList>
                                                </div>
                                            </td>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </tbody>
                                </table>
                                                    </div>
                            </FooterTemplate>
                        </asp:Repeater>

                        <asp:Repeater ID="rptDiarioAlunosFrequenciaTerriorio" runat="server" OnItemDataBound="rptDiarioAlunosFrequenciaTerriorio_ItemDataBound">
                            <HeaderTemplate>
                                <div>
                                    <table id="tabela" class="grid" cellspacing="0">
                                        <thead>
                                            <tr class="gridHeader" style="height: 30px;">
                                                <th class="center">
                                                    <asp:Label ID="_lblNumChamada" runat="server" Text='Nº Chamada'></asp:Label>
                                                </th>
                                                <th>
                                                    <asp:Label ID="lblNome" runat="server" Text='Nome do aluno'></asp:Label>
                                                </th>
                                                <asp:Repeater ID="rptDiarioAulas" runat="server" OnItemDataBound="rpDiarioAulasHeader_ItemDataBound">
                                                    <ItemTemplate>
                                                        <th class="center {sorter:false}">
                                                            <asp:Label ID="lbltau_id" runat="server" Text='<%#Bind("tau_id") %>' Visible="false">
                                                            </asp:Label>
                                                            <asp:HiddenField ID="hdnPosicao" runat="server" Value='<%#Bind("tdt_posicao") %>' />
                                                            <asp:Label ID="lbltnt_data" runat="server" Text='Ausência'>
                                                            </asp:Label>
                                                        </th>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tr>
                                            <tr class="gridRow" style="height: 30px;">
                                                <td colspan="2"></td>
                                                <asp:Repeater ID="rptDiarioAulasEfetivado" runat="server" OnItemDataBound="rpDiarioAulasHeader_ItemDataBound">
                                                    <ItemTemplate>
                                                        <th class="center {sorter:false}">
                                                            <asp:Label ID="lbltau_id" runat="server" Text='<%#Bind("tau_id") %>' Visible="false">
                                                            </asp:Label>
                                                            <asp:HiddenField ID="hdnPosicao" runat="server" Value='<%#Bind("tdt_posicao") %>' />
                                                            <asp:Label ID="lbltnt_data" runat="server" Text='Ausência' style="display:none;">
                                                            </asp:Label>
                                                            <asp:CheckBox ID="chkEfetivado" runat="server" Text="Efetivado" style="text-align:center;"/>
                                                        </th>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tr>
                                        </thead>
                                        <tbody>
                            </HeaderTemplate>
                            <AlternatingItemTemplate>
                                <tr class="gridAlternatingRow">
                                    <td style="text-align: center;" runat="server" id="tdNumeroChamada">
                                        <asp:Label ID="lblalu_id" runat="server" Text='<%#Bind("alu_id") %>' Visible="false">
                                        </asp:Label>
                                        <asp:Label ID="lblmtu_id" runat="server" Text='<%#Bind("mtu_id") %>' Visible="false">
                                        </asp:Label>
                                        <asp:Label ID="lblAtividade" runat="server" Text='<%#Bind("numeroChamada") %>'>
                                        </asp:Label>
                                    </td>
                                    <td id="tdNomeAluno" runat="server">
                                        <asp:Label ID="lblNome" runat="server" Text='<%#Bind("pes_nome") %>'>
                                        </asp:Label>
                                    </td>

                                    <asp:Repeater ID="rptDiarioAulas" runat="server" OnItemDataBound="rptDiarioAulasTerritorio_ItemDataBound">
                                        <ItemTemplate>
                                            <td style="text-align: center;" runat="server" id="tdAulas">
                                                <div id="divAulasAluno" runat="server" style="display: flex; width: 100%;">
                                                    <asp:HiddenField ID="hdnPosicao" runat="server" Value='<%#Bind("tdt_posicao") %>' />
                                                    <table>
                                                        <tr style="display: flex">
                                                            <asp:Repeater ID="rptAulasTerritorio" runat="server" OnItemDataBound="rptAulasTerritorio_ItemDataBound">
                                                                <ItemTemplate>
                                                                    <td style="padding: 0">
                                                                        <asp:CheckBoxList ID="cblFrequencia" runat="server" RepeatDirection="Horizontal"
                                                                            RepeatLayout="Flow">
                                                                        </asp:CheckBoxList>
                                                                    </td>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tr>
                            </AlternatingItemTemplate>
                            <ItemTemplate>
                                <tr class="gridRow">
                                    <td style="text-align: center;" runat="server" id="tdNumeroChamada">
                                        <asp:Label ID="lblalu_id" runat="server" Text='<%#Bind("alu_id") %>' Visible="false">
                                        </asp:Label>
                                        <asp:Label ID="lblmtu_id" runat="server" Text='<%#Bind("mtu_id") %>' Visible="false">
                                        </asp:Label>
                                        <asp:Label ID="lblAtividade" runat="server" Text='<%#Bind("numeroChamada") %>'>
                                        </asp:Label>
                                    </td>
                                    <td id="tdNomeAluno" runat="server">
                                        <asp:Label ID="lblNome" runat="server" Text='<%#Bind("pes_nome") %>'>
                                        </asp:Label>
                                    </td>

                                    <asp:Repeater ID="rptDiarioAulas" runat="server" OnItemDataBound="rptDiarioAulasTerritorio_ItemDataBound">
                                        <ItemTemplate>
                                            <td style="text-align: center;" runat="server" id="tdAulas">
                                                <div id="divAulasAluno" runat="server" style="display: flex; width: 100%;">
                                                    <asp:HiddenField ID="hdnPosicao" runat="server" Value='<%#Bind("tdt_posicao") %>' />
                                                    <table>
                                                        <tr style="display: flex">
                                                            <asp:Repeater ID="rptAulasTerritorio" runat="server" OnItemDataBound="rptAulasTerritorio_ItemDataBound">
                                                                <ItemTemplate>
                                                                    <td style="padding: 0">
                                                                        <asp:CheckBoxList ID="cblFrequencia" runat="server" RepeatDirection="Horizontal"
                                                                            RepeatLayout="Flow">
                                                                        </asp:CheckBoxList>
                                                                    </td>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </tbody>
                                </table>
                                                    </div>
                            </FooterTemplate>
                        </asp:Repeater>

                        <br />
                        <div>
                            <b>Legenda:</b>
                            <div style="border-style: solid; border-width: thin; width: 230px;">
                                <table id="tbLegenda" runat="server" style="border-collapse: separate !important; border-spacing: 2px !important;">
                                    <tr id="trExibirAlunoDispensadoFrequencia" runat="server">
                                        <td style="border-style: solid; border-width: thin; width: 25px; height: 15px;"></td>
                                        <td>
                                            <asp:Literal runat="server" ID="litDispensado" Text="Aluno dispensado"></asp:Literal>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="border-style: solid; border-width: thin; width: 25px; height: 15px;"></td>
                                        <td>
                                            <asp:Literal runat="server" ID="litInativo" Text="<%$ Resources:Mensagens, MSG_ALUNO_INATIVO %>"></asp:Literal>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>

                        <div id="divUsuarioAlteracaoFrequencia" runat="server" visible="false">
                            <br />
                            <asp:Label ID="lblAlteracaoFreq" runat="server" />
                            <br />
                        </div>

                        <div class="right">
                            <asp:Button ID="btnSalvarFrequencia" ValidationGroup="Frequencia" runat="server"
                                Text="Salvar" OnClick="btnSalvarFrequencia_Click" OnClientClick="$('#divLancamentoFrequencia').scrollTo(0,0);javascript:scroll(0,0);" />
                            <asp:Button ID="btnCancelarFrequencia" runat="server" Text="Cancelar" OnClientClick="var exibirMensagemConfirmacao=true;$('#divLancamentoFrequencia').dialog('close'); return false;" />
                        </div>
                    </asp:Panel>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div id="divConfirmarLancamento" title="Confirmar lançamento" class="hide">
        <asp:UpdatePanel ID="updConfirmarLancamento" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset>
                    <asp:Label ID="lblConfirmarLancamento" runat="server" Text="Sr(a). Docente, caso tenha concluído o lançamento de frequência desta aula, clique em efetivar. Caso queira gravar as informações e continuar o lançamento posteriormente, clique em salvar."></asp:Label>
                    <div class="right">
                        <asp:Button ID="btnEfetivarLancamento" runat="server" CausesValidation="False" Text="Efetivar" OnClick="btnEfetivarLancamento_Click" />
                        <asp:Button ID="btnSalvarLancamento" runat="server" CausesValidation="False" Text="Salvar" OnClick="btnSalvarLancamento_Click" OnClientClick="javascript:scroll(0,0);" />
                    </div>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div id="divAnotacoesAluno" title="Anotações sobre os alunos" class="hide">
        <asp:UpdatePanel ID="updMessageAnotacoes" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblMessageAnotacoes" runat="server" EnableViewState="False"></asp:Label>
                <asp:ValidationSummary ID="vsAnotacoes" runat="server" ValidationGroup="Anotacao" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updAnotacoes" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset>
                    <asp:Panel ID="pnlAnotacoesAluno" runat="server" DefaultButton="btnSalvarAnotacoes">
                        <asp:GridView ID="grvAnotacaoAluno" runat="server" AutoGenerateColumns="False" EmptyDataText="Não existem alunos cadastrados."
                            OnRowDataBound="grvAnotacaoAluno_RowDataBound" OnDataBinding="grvAnotacaoAluno_DataBinding" SkinID="GridResponsive">
                            <Columns>
                                <asp:TemplateField HeaderText="Aluno" HeaderStyle-Width="150px">
                                    <ItemTemplate>

                                        <asp:Label ID="lblIdsAnotAlu" runat="server" Text='<%# Bind("alu_mtu_mtd_id") %>'
                                            CssClass="hide"></asp:Label>
                                        <asp:Label ID="lblNomeAluno" Text='<%# Bind("pes_nome") %>' runat="server"></asp:Label>
                                        <asp:DropDownList ID="ddlAnotacaoAluno" runat="server" OnSelectedIndexChanged="ddlAnotacaoAluno_SelectedIndexChanged"
                                            DataTextField="Nome" DataValueField="alu_mtu_mtd_id" AppendDataBoundItems="True"
                                            SkinID="ddlAlunoAnotacao" CssClass="wrap100px">
                                        </asp:DropDownList>
                                        <asp:CustomValidator ID="cpvAluno" runat="server" ErrorMessage="Aluno é obrigatório."
                                            ValidationGroup="Anotacao" ControlToValidate="ddlAnotacaoAluno" Operator="NotEqual"
                                            OnServerValidate="cpvAluno_ServerValidate" Display="Dynamic">*</asp:CustomValidator>

                                        <asp:HiddenField ID="hdnUsuAleracao" runat="server" Value='<%# Eval("nomeUsuAlteracao") %>' />

                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tipos de anotação do aluno">
                                    <ItemTemplate>
                                        <asp:CheckBoxList ID="cblAnotacoesPredefinidas" runat="server"
                                            DataTextField="tia_nome" DataValueField="tia_id" AppendDataBoundItems="True" RepeatLayout="Flow" RepeatDirection="Vertical">
                                        </asp:CheckBoxList>

                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Anotações sobre o aluno">

                                    <ItemTemplate>
                                        <div class="textareaComInfo">
                                            <asp:Label ID="lblAnotacaoInfo" CssClass="textareaInfo" runat="server"
                                                Text="<%$ Resources:Mensagens, AULAS_ANOTACOESALUNOS %>"></asp:Label>
                                            <%--skin text60C é o mesmo tamanho que o limite4000 mas não tem os eventos onkeypress e onkeyup usados no contador de caractere--%>
                                            <asp:TextBox ID="txtAnotacao" runat="server" TextMode="MultiLine"
                                                Text='<%# Bind("taa_anotacao") %>' onkeypress="LimitarCaracter(this,'contadesc3','4000');"
                                                SkinID="txtAlunoAnotacao"
                                                onkeyup="LimitarCaracter(this,'contadesc3','4000');" Style="margin-bottom: 0; min-height: 80px"></asp:TextBox>
                                            <asp:Label ID="contadesc3" Style="font-size: 85%;" Text="0/4000" runat="server" />
                                            <asp:CustomValidator ID="cpvAnotacao" runat="server" ErrorMessage="Anotação ou um tipo de anotação é obrigatório."
                                                ValidationGroup="Anotacao" ControlToValidate="txtAnotacao" Operator="NotEqual" ValidateEmptyText="true"
                                                OnServerValidate="cpvAnotacao_ServerValidate" Display="Dynamic">*</asp:CustomValidator>

                                            <asp:Label ID="lblAlteracaoAnotacoes" runat="server" Visible="false" />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ações" HeaderStyle-CssClass="center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnAdicionar" runat="server" ValidationGroup="Anotacao" SkinID="btNovo"
                                            ToolTip="Adicionar anotação" OnClick="btnAdicionar_Click" Visible="False" />&nbsp;
                                        <asp:ImageButton ID="btnCancelar" runat="server" SkinID="btExcluir" ToolTip="Excluir anotação"
                                            CausesValidation="false" OnClick="btnCancelar_Click" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemStyle VerticalAlign="Middle" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <div class="right">
                            <asp:Button ID="btnSalvarAnotacoes" runat="server" Text="Salvar" OnClick="btnSalvarAnotacoes_Click" ValidationGroup="Anotacao" OnClientClick="$('#divAnotacoesAluno').scrollTo(0,0);javascript:scroll(0,0);" />
                            <asp:Button ID="btnCancelarAnotacoes" runat="server" Text="Cancelar" OnClientClick="var exibirMensagemConfirmacao=true;$('#divAnotacoesAluno').dialog('close'); return false;" />
                            <asp:Button ID="btnAdicionarMaisdeUmAluno" runat="server" Text="Adicionar anotação para mais de um aluno" Visible="false" OnClick="btnAdicionarMaisdeUmAluno_Click" />
                        </div>
                    </asp:Panel>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div id="divAnotacoesMaisdeUmAluno" title="Anotações sobre os alunos" class="hide">
        <asp:UpdatePanel ID="uppMensagemAnotacoesMaisdeUmAluno" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblMessageAnotacoesMaisdeUmAluno" runat="server" EnableViewState="False"></asp:Label>
                <asp:ValidationSummary ID="vsAnotacoesMaisdeUmAluno" runat="server" ValidationGroup="AnotacaoMaisdeUmAluno" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updAnotacoesMaisdeUmAluno" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset>
                    <asp:Panel ID="pnlAnotacoesMaisdeUmAluno" runat="server" DefaultButton="btnSalvarAnotacoesMaisdeUmAluno">
                        <asp:GridView ID="grvAnotacoesMaisdeUmAluno" runat="server" AutoGenerateColumns="False" EmptyDataText="Não existem alunos cadastrados."
                            OnRowDataBound="grvAnotacoesMaisdeUmAluno_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Aluno" HeaderStyle-Width="200px">
                                    <ItemTemplate>
                                        <div id="divAlunos" style="width: 200px; height: 270px; overflow-y: scroll; overflow-x: hidden;">
                                            <asp:CheckBoxList ID="cblAnotacaoAluno" runat="server" RepeatLayout="Flow" RepeatDirection="Vertical"
                                                DataTextField="Nome" DataValueField="alu_mtu_mtd_id" AppendDataBoundItems="True">
                                            </asp:CheckBoxList>
                                            <%--   <asp:CustomValidator ID="cpvAluno" runat="server" ErrorMessage="Aluno é obrigatório."
                                            ValidationGroup="AnotacoesMaisdeUmAluno" ControlToValidate="lbAnotacaoAluno" Operator="NotEqual"
                                            OnServerValidate="cpvAluno_ServerValidate" Display="Dynamic">*</asp:CustomValidator>--%>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tipos de anotação do aluno" HeaderStyle-Width="200px">
                                    <ItemTemplate>
                                        <div style="width: 200px;">
                                            <asp:CheckBoxList ID="cblAnotacoesPredefinidas" runat="server" RepeatLayout="Flow" RepeatDirection="Vertical"
                                                DataTextField="tia_nome" DataValueField="tia_id" AppendDataBoundItems="True">
                                            </asp:CheckBoxList>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Anotações sobre o aluno">
                                    <ItemTemplate>

                                        <div class="textareaComInfo">
                                            <asp:Label ID="lblAnotacaoInfo" CssClass="textareaInfo" runat="server"
                                                Text="<%$ Resources:Mensagens, AULAS_ANOTACOESALUNOS %>"></asp:Label>
                                            <%--skin text60C é o mesmo tamanho que o limite4000 mas não tem os eventos onkeypress e onkeyup usados no contador de caractere--%>
                                            <asp:TextBox ID="txtAnotacao" runat="server" TextMode="MultiLine" onkeypress="LimitarCaracter(this,'contadesc3','4000');" onkeyup="LimitarCaracter(this,'contadesc3','4000');" Style="margin-bottom: 0; min-height: 80px;"></asp:TextBox>
                                            <asp:Label ID="contadesc3" Style="font-size: 85%;" Text="0/4000" runat="server" />
                                            <asp:CustomValidator ID="cpvAnotacao" runat="server" ErrorMessage="Anotação ou um tipo de anotação é obrigatório."
                                                ValidationGroup="AnotacoesMaisdeUmAluno" ControlToValidate="txtAnotacao" Operator="NotEqual" ValidateEmptyText="true"
                                                OnServerValidate="cpvAnotacao_ServerValidate" Display="Dynamic">*</asp:CustomValidator>

                                            <asp:Label ID="lblAlteracaoAnotacoes" runat="server" Visible="false" />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <div class="right">
                            <asp:Button ID="btnSalvarAnotacoesMaisdeUmAluno" runat="server" Text="Salvar" OnClick="btnSalvarAnotacoesMaisdeUmAluno_Click" ValidationGroup="AnotacoesMaisdeUmAluno" OnClientClick="$('#divAnotacoesMaisdeUmAluno').scrollTo(0,0);javascript:scroll(0,0);" />
                            <asp:Button ID="btnCancelarAnotacoesMaisdeUmAluno" runat="server" Text="Cancelar" OnClientClick="var exibirMensagemConfirmacao=true;$('#divAnotacoesMaisdeUmAluno').dialog('close'); return false;" />
                        </div>
                    </asp:Panel>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <uc5:UCConfirmacaoOperacao ID="UCConfirmacaoOperacao1" runat="server" />
    <div id="divAtividadeAvaliativa" title="Lançar atividade avaliativa" class="hide">
        <asp:UpdatePanel ID="updMessageAtividade" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblMessageAtividade" runat="server" EnableViewState="False"></asp:Label>
                <asp:ValidationSummary ID="vsAtividade" runat="server" ValidationGroup="Atividade" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updAtividade" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset runat="server" id="fdsAtividadeAvaliativa">
                    <asp:Panel ID="pnlAtividadeAvaliativa" runat="server" DefaultButton="btnSalvarNota">
                        <asp:Panel ID="pnlCadastroAtividade" runat="server" GroupingText="Nova avaliação">
                            <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios2" runat="server" />
                            <div>
                                <asp:Label ID="lblComponenteAtAvaliativa" runat="server" Text="<%$ Resources:Mensagens, MSG_DISCIPLINA %>" AssociatedControlID="ddlComponenteAtAvaliativa"></asp:Label>
                                <asp:DropDownList ID="ddlComponenteAtAvaliativa" runat="server" AppendDataBoundItems="True"
                                    AutoPostBack="true" DataTextField="tur_tud_nome" DataValueField="tur_tud_id" SkinID="text20C" OnSelectedIndexChanged="ddlComponenteAtAvaliativa_SelectedIndexChanged">
                                </asp:DropDownList>
                                <div id="divItensAtividade" runat="server">
                                    <uc3:UCComboTipoAtividadeAvaliativa ID="UCComboTipoAtividadeAvaliativa" runat="server"
                                        ValidationGroup="Atividade" Obrigatorio="true" Validator_IsValid="true" MostrarMessageOutros="true" />
                                    <asp:DropDownList ID="ddlTurmaDisciplinaAtAvaliativa" runat="server" AppendDataBoundItems="True"
                                        AutoPostBack="true" DataTextField="tur_tud_nome" DataValueField="tur_tud_id"
                                        SkinID="text60C" Visible="false">
                                    </asp:DropDownList>
                                    <div id="divNomeAtividade" runat="server">
                                        <asp:Label ID="lblNomeAtividade" runat="server" Text="Nome da atividade avaliativa" AssociatedControlID="txtNomeAtividade"></asp:Label>
                                        <asp:TextBox ID="txtNomeAtividade" runat="server" SkinID="text60C"></asp:TextBox>
                                    </div>
                                    <asp:Label ID="lblConteudoAtividade" runat="server" Text="<%$ Resources:Mensagens, MSG_CONTEUDOATIVIDADE %>" AssociatedControlID="txtConteudoAtividade">
                                    </asp:Label>
                                    <asp:TextBox ID="txtConteudoAtividade" runat="server" TextMode="MultiLine" SkinID="limite4000">
                                    </asp:TextBox>
                                    <fieldset runat="server" id="fdsHabilidadesRelacionadas">
                                        <legend>Habilidades relacionadas</legend>
                                        <div></div>
                                        <uc4:UCHabilidades runat="server" ID="UCHabilidades" TituloFildSet="Expectativa de aprendizagem" LegendaCheck="Não alcançada" bHabilidaEdicao="True" />
                                    </fieldset>
                                </div>
                            </div>
                            <div id="divConfirmAtividadeAvaliativa" style="float: left; padding-left: 60px;" runat="server" visible="false">
                                <asp:Label ID="lbllblAtividadeAvaliativa" runat="server" Text="Atividade avaliativa?" AssociatedControlID="rblAtividadeAvaliativa"></asp:Label>
                                <asp:RadioButtonList ID="rblAtividadeAvaliativa" runat="server">
                                    <asp:ListItem Text="Sim" Value="True" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Não" Value="False"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div id="divAtividadeExclusiva" runat="server" style="width: 365px; float: left; clear: none; margin-top: 5px; margin-bottom: 5px;" visible="false">
                                <asp:CheckBox ID="chkAtividadeExclusiva" runat="server" Text="Atividade extraordinária" />
                            </div>
                            <div class="right" id="divBotaoAcaoAtividade" runat="server">
                                <asp:Button ID="btnNovaAtividade" runat="server" Text="Adicionar nova avaliação" OnClick="btnNovaAtividade_Click" ValidationGroup="Atividade" OnClientClick="$('#divAtividadeAvaliativa').scrollTo(0,0);javascript:scroll(0,0);" />
                                <asp:Button ID="btnEditarAtividade" runat="server" Text="Salvar" OnClick="btnEditarAtividade_Click" ValidationGroup="Atividade" Visible="false" />
                                <asp:Button ID="btnCancelarAtividade" runat="server" Text="Cancelar" OnClick="btnCancelarAtividade_Click" Visible="false" />
                            </div>
                        </asp:Panel>
                        <br />
                        <div class="right" id="divSalvarAvaliacaoCima" runat="server">
                            <asp:Button ID="btnSalvarNotaCima" runat="server" Text="Salvar"
                                OnClick="btnSalvarNota_Click" OnClientClick="$('#divAtividadeAvaliativa').scrollTo(0,0);javascript:scroll(0,0);" />
                            <asp:Button ID="btnCancelarNotaCima" runat="server" Text="Cancelar" OnClientClick="var exibirMensagemConfirmacao=true;$('#divAtividadeAvaliativa').dialog('close'); return false;" />
                        </div>
                        <br />
                        <br />
                        <asp:Panel ID="pnlAvaliacao" runat="server" GroupingText="Avaliações cadastradas">
                            <uc2:UCComboOrdenacao ID="UCComboOrdenacao2" runat="server" />
                            <asp:Label ID="lblInfoAtividade" runat="server"></asp:Label>
                            <asp:Repeater ID="rptAlunos" runat="server" OnItemDataBound="rptAlunos_ItemDataBound">
                                <HeaderTemplate>
                                    <div class="divScrollResponsivo">
                                        <table class="grid tbAtividadeAvaliativa grid-responsive-list" id="tabelaAtividade">
                                            <thead>
                                                <tr class="gridHeader">
                                                    <th class="center">
                                                        <asp:Label ID="lblHeaderNumChamada" runat="server" Text='Nº Chamada'></asp:Label>
                                                    </th>
                                                    <th>
                                                        <asp:Label ID="lblHeaderNome" runat="server" Text='Nome do aluno'></asp:Label>
                                                    </th>
                                                    <asp:Repeater ID="rptAtividades" runat="server" OnItemDataBound="rptAtividadesHeader_ItemDataBound">
                                                        <ItemTemplate>
                                                            <th class="center {sorter:false}" style="border-left: 0.1em dotted #FFFFFF; padding-right: 3px;">
                                                                <asp:Label ID="lbltud_id" runat="server" Text='<%#Bind("tud_id") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lbltnt_id" runat="server" Text='<%#Bind("tnt_id") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblPosicao" runat="server" Text='<%#Bind("tdt_posicao") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblAtividadeExclusiva" runat="server" Text='<%#Bind("tnt_exclusiva") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblUsuIdAtiv" runat="server" Text='<%#Bind("usu_id") %>' Visible="false"></asp:Label>

                                                                <div style="display: inline-block; width: 100%;">
                                                                    <asp:Label ID="lblAtividade" runat="server" Text='<%#Bind("tnt_nome") %>'></asp:Label><br />
                                                                    <asp:Label ID="lbltnt_data" runat="server" Text='<%#Bind("tnt_data") %>'></asp:Label>
                                                                    <br />

                                                                    <div style="text-align: center">
                                                                        <asp:ImageButton ID="btnExcluirAtividadePopup" runat="server" CausesValidation="false"
                                                                            ToolTip='<%#"Excluir " + MSTech.GestaoEscolar.BLL.GestaoEscolarUtilBO.nomePadraoAtividadeAvaliativa(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " \"" + Eval("tnt_nome") + "\"" %>'
                                                                            SkinID="btExcluirSemMensagem" OnClick="btnExcluirAtividadeListao_Click" />

                                                                        <asp:ImageButton ID="btnEditarAtividadePopup" runat="server"
                                                                            ToolTip='<%#"Editar " + MSTech.GestaoEscolar.BLL.GestaoEscolarUtilBO.nomePadraoAtividadeAvaliativa(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " \"" + Eval("tnt_nome") + "\"" %>'
                                                                            SkinID="btEditar" Style="display: inline-block; vertical-align: middle;" OnClick="btnEditarAtividadePopup_Click" Visible="false" />
                                                                    </div>
                                                                </div>
                                                            </th>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tr>
                                                <tr class="gridRow grid-linha-destaque">
                                                    <td class="grid-responsive-no-header"></td>
                                                    <td class="grid-responsive-no-header"></td>
                                                    <asp:Repeater ID="rptAtividadesEfetivado" runat="server" OnItemDataBound="rptAtividadesHeader_ItemDataBound">
                                                        <ItemTemplate>
                                                            <td class="center {sorter:false} .sorterFalse grid-responsive-item-inline grid-responsive-center" style="border-left: 0.1em dotted #FFFFFF; padding-right: 3px;">
                                                                <asp:Label ID="lbltud_id" runat="server" Text='<%#Bind("tud_id") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lbltnt_id" runat="server" Text='<%#Bind("tnt_id") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblPosicao" runat="server" Text='<%#Bind("tdt_posicao") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblAtividadeExclusiva" runat="server" Text='<%#Bind("tnt_exclusiva") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblUsuIdAtiv" runat="server" Text='<%#Bind("usu_id") %>' Visible="false"></asp:Label>

                                                                <div style="display: inline-block; width: 100%;">
                                                                    <asp:Label ID="lblAtividade" runat="server" Text='<%#Bind("tnt_nome") %>' style="display:none;"></asp:Label>
                                                                    <asp:Label ID="lbltnt_data" runat="server" Text='<%#Bind("tnt_data") %>' style="display:none;"></asp:Label>
                                                                    <div style="text-align: center; display: none;">
                                                                        <asp:ImageButton ID="btnExcluirAtividadePopup" runat="server" CausesValidation="false"
                                                                            ToolTip='<%#"Excluir " + MSTech.GestaoEscolar.BLL.GestaoEscolarUtilBO.nomePadraoAtividadeAvaliativa(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " \"" + Eval("tnt_nome") + "\"" %>'
                                                                            SkinID="btExcluirSemMensagem" OnClick="btnExcluirAtividadeListao_Click" />

                                                                        <asp:ImageButton ID="btnEditarAtividadePopup" runat="server"
                                                                            ToolTip='<%#"Editar " + MSTech.GestaoEscolar.BLL.GestaoEscolarUtilBO.nomePadraoAtividadeAvaliativa(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + " \"" + Eval("tnt_nome") + "\"" %>'
                                                                            SkinID="btEditar" Style="display: inline-block; vertical-align: middle;" OnClick="btnEditarAtividadePopup_Click" Visible="false" />
                                                                    </div>
                                                                    <asp:CheckBox ID="chkEfetivado" runat="server" Text="Efetivado" style="text-align: center;" />
                                                                </div>
                                                            </td>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tr>
                                            </thead>
                                            <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr class="gridRow">
                                        <td runat="server" id="tdNumChamadaAtivAva" class="center" style="text-align: center;">
                                            <asp:Label ID="lblalu_id" runat="server" Text='<%#Bind("alu_id") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblmtu_id" runat="server" Text='<%#Bind("mtu_id") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblmtd_id" runat="server" Text='<%#Bind("mtd_id") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblNumeroChamada" runat="server" Text='<%#Bind("numeroChamada") %>'></asp:Label>
                                        </td>
                                        <td runat="server" id="tdNomeAtivAva">
                                            <asp:Label ID="lblNome" runat="server" Text='<%#Bind("pes_nome") %>'></asp:Label>
                                        </td>
                                        <asp:Repeater ID="rptAtividades" runat="server" OnItemDataBound="rptAtividades_ItemDataBound">
                                            <ItemTemplate>
                                                <td runat="server" id="tdAtividadesAtivAva" class="center" style="text-align: center;">
                                                    <div id="divAtividades" runat="server" style="display: inline-block; width: 100%;">
                                                        <asp:Label ID="lbltnt_id" runat="server" Text='<%#Bind("tnt_id") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblPosicao" runat="server" Text='<%#Bind("tdt_posicao") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblUsuIdAtiv2" runat="server" Text='<%#Bind("usu_id") %>' Visible="false"></asp:Label>

                                                        <asp:CheckBox ID="chkParticipante" runat="server" Text="Participante" Style="display: inline-block;" /><br class="responsive-hide"/>
                                                        <asp:TextBox ID="txtNota" runat="server" SkinID="Decimal" Width="50"></asp:TextBox>
                                                        <asp:CheckBox ID="chkDesconsiderar" runat="server" Text="D" ToolTip="Desconsiderar nota no cálculo da média" />
                                                        <asp:DropDownList ID="ddlPareceres" runat="server" DataTextField="descricao" DataValueField="eap_valor">
                                                        </asp:DropDownList>
                                                        <asp:ImageButton ID="btnRelatorio" runat="server" SkinID="btDetalhar"
                                                            ToolTip="Lançar relatório" OnClick="btnRelatorio_Click" />
                                                        <asp:Image ID="imgSituacao" runat="server" SkinID="imgConfirmar" ToolTip="Relatório lançado"
                                                            Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                                                    </div>
                                                </td>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>
                                    </table>
                                    </div>
                                </FooterTemplate>
                            </asp:Repeater>

                            <div>
                                <b>Legenda:</b>
                                <div style="border-style: solid; border-width: thin; width: 230px;">
                                    <table id="tbLegendaAtiv" runat="server">
                                        <tr id="trExibirAlunoDispensadoAtividade" runat="server">
                                            <td style="border-style: solid; border-width: thin; width: 25px; height: 15px;"></td>
                                            <td>
                                                <asp:Literal runat="server" ID="litDispensado2" Text="<%$ Resources:Mensagens, MSG_ALUNO_DISPENSADO %>"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-style: solid; border-width: thin; width: 25px; height: 15px;"></td>
                                            <td>
                                                <asp:Literal runat="server" ID="litAusente" Text="<%$ Resources:Mensagens, MSG_ALUNO_AUSENTE %>"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-style: solid; border-width: thin; width: 25px; height: 15px;"></td>
                                            <td>
                                                <asp:Literal runat="server" ID="litInativo2" Text="<%$ Resources:Mensagens, MSG_ALUNO_INATIVO %>"></asp:Literal>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>

                            <div id="divUsuarioAlteracaoAtividadeAvaliativa" runat="server" visible="false">
                                <br />
                                <asp:Label ID="lblAlteracaoAtividadeAvaliativa" runat="server" />
                                <br />
                            </div>
                        </asp:Panel>

                        <div class="right">
                            <asp:Button ID="btnSalvarNota" runat="server" Text="Salvar" OnClick="btnSalvarNota_Click" OnClientClick="$('#divAtividadeAvaliativa').scrollTo(0,0);javascript:scroll(0,0);" />
                            <asp:Button ID="btnCancelarNota" runat="server" Text="Cancelar" OnClientClick="var exibirMensagemConfirmacao=true;$('#divAtividadeAvaliativa').dialog('close'); return false;" />
                        </div>
                    </asp:Panel>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div id="divPlanoAula" title="Plano de aula" class="hide">
        <asp:UpdatePanel ID="updMessagePlanoAula" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblMessagePlanoAula" runat="server" EnableViewState="False"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updPlanejamento" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="divTabsPlanejamento">
                    <asp:DropDownList ID="ddlTurmaDisciplina" runat="server" AppendDataBoundItems="True"
                        AutoPostBack="true" DataTextField="tur_tud_nome" DataValueField="tur_tud_id"
                        SkinID="text60C" OnSelectedIndexChanged="ddlTurmaDisciplina_SelectedIndexChanged"
                        Visible="false">
                    </asp:DropDownList>
                    <asp:Label ID="lblTurmaDisciplinaComponente" runat="server" Text="<%$ Resources:Mensagens, MSG_DISCIPLINA %>" AssociatedControlID="ddlTurmaDisciplinaComponente"></asp:Label>
                    <asp:DropDownList ID="ddlTurmaDisciplinaComponente" runat="server" AppendDataBoundItems="True"
                        AutoPostBack="true" DataTextField="tur_tud_nome" DataValueField="tur_tud_id" SkinID="text60C"
                        OnSelectedIndexChanged="ddlTurmaDisciplinaComponente_SelectedIndexChanged">
                    </asp:DropDownList>
                    <br />
                    <br />
                    <fieldset class="area-botoes-top">
                        <div class="right">
                            <asp:Button ID="btnSalvarPlanoAulaCima" runat="server" Text="Salvar" ValidationGroup="Aula" OnClick="btnSalvarPlanoAulaCima_Click" OnClientClick="$('#divPlanoAula').scrollTo(0,0);javascript:scroll(0,0);" />
                            <asp:Button ID="btnCancelarPlanoAulaCima" runat="server" Text="Cancelar" OnClientClick="var exibirMensagemConfirmacao=true;$('#divPlanoAula').dialog('close'); return false;" />
                        </div>
                    </fieldset>
                    <div id="divCadastroPlanoAula" runat="server">
                        <br />
                        <asp:ValidationSummary ID="ValidationSummaryAula" runat="server" ValidationGroup="Aula" />
                        <ul class="hide">
                            <li><a href="#divTabsPlanejamento-0" id="aPlanoAula" runat="server">Plano de aula</a> </li>
                            <li class="tabPlanejamento"><a href="#divTabsPlanejamento-1" id="aPlanejamentoBimestre" runat="server">Planejamento</a></li>
                            <%--<li style="visibility: hidden"><a href="#divTabsPlanejamento-Anterior" id="aPlanoAnterior" runat="server">Planejamento Anterior</a></li>--%>
                        </ul>
                        <div id="divTabsPlanejamento-0">
                            <fieldset>
                                <asp:Panel ID="pnlPanoAulaDados" runat="server">
                                    <div id="divDados" runat="server">

                                        <fieldset id="fdsHabilidadesAula" runat="server" class="fsdTreeview" visible="false">
                                            <asp:Label ID="lblMensagemHabilidadeAula" runat="server" CssClass="Info"></asp:Label>
                                            <legend>
                                                <span id="spanLegend" runat="server" title="<%$ Resources:Classe, DiarioClasse.fdsHabilidadesAula.spanLegend %>"></span>
                                                <span style="position: absolute; right: 0; top: 4px;">
                                                    <span id="spanTrabalhado" runat="server" style="display: inline-block; width: 150px; text-align: center; border-right: 1px solid white; border-left: 1px solid #fff;" title="<%$ Resources:Classe, DiarioClasse.fdsHabilidadesAula.spanTrabalhado %>"></span>
                                                </span>

                                            </legend>
                                            <div style="clear: both;">
                                                <div>
                                                </div>
                                                <div class="divTreeviewScrollAula">
                                                    <asp:Repeater ID="rptHabilidadesAula" runat="server" OnItemDataBound="rptHabilidadesAula_ItemDataBound">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litCabecalho" runat="server"></asp:Literal>
                                                            <asp:HiddenField ID="hdnPermiteLancamento" runat="server" Value='<%# Eval("PermiteLancamento") %>' />
                                                            <asp:Literal ID="litConteudo" runat="server"></asp:Literal>
                                                            <asp:HiddenField ID="hdnOcrId" runat="server" Value='<%# Eval("ocr_id") %>' />
                                                            <div style="display: table-row;" id="divHabilidade" runat="server">
                                                                <span style="display: table-cell; text-align: left; vertical-align: top;">
                                                                    <asp:Literal ID="lblHabilidade" runat="server"></asp:Literal>
                                                                </span>
                                                                <span style="display: table-cell; width: 150px; text-align: center; vertical-align: top; vertical-align: middle;">
                                                                    <asp:CheckBox ID="chkTrabalhado" runat="server" Checked='<%# Eval("Trabalhado") %>'></asp:CheckBox><br />
                                                                    <asp:Label CssClass="lblLegenda" ID="lblLegendaTrabalhado" runat="server" Text="" Visible="false"></asp:Label>
                                                                </span>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </div>
                                            </div>
                                        </fieldset>

                                        <asp:Label ID="lblCampoObrigatorioDocumento" runat="server" Text="<%$ Resources:Academico, ControleTurma.DiarioClasse.lblCampoObrigatorioDocumento.Text %>"
                                            Font-Size="Small" class="campoObrigatorio"></asp:Label><br />
                                        <br />
                                        <div class="clear">
                                        </div>

                                        <div id="divPlanoAulas" runat="server">
                                            <asp:HiddenField ID="hdnOperacao" runat="server" />
                                            <asp:CheckBoxList ID="cblComponentesRegencia" runat="server" Visible="false"
                                                DataTextField="tud_nome" DataValueField="tur_tud_id" RepeatDirection="Horizontal" CssClass="checkBoxListHorizontal">
                                            </asp:CheckBoxList>
                                            <div id="divPlanoAulaInfo" runat="server">
                                                <asp:Label ID="LabelPlanoAula" runat="server" Text="<%$ Resources:Mensagens, MSG_PLANODEAULA %>"
                                                    AssociatedControlID="txtPlanoAula" Width="480px">
                                                </asp:Label>
                                                <div class="textareaComInfo">
                                                    <asp:Label ID="lblPlanoAulaInfo" CssClass="textareaInfo" runat="server"
                                                        Text="<%$ Resources:Mensagens, AULAS_PLANOAULA %>">
                                                    </asp:Label>
                                                    <asp:TextBox ID="txtPlanoAula" runat="server" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div id="divSinteseDaAula" runat="server">
                                                <asp:Label ID="LabelSinteseAula" runat="server" Text="<%$ Resources:Mensagens, MSG_SINTESEDAAULA %>"
                                                    AssociatedControlID="txtSinteseAula" Width="480px" Font-Bold="true"></asp:Label>
                                                <div class="textareaComInfo">
                                                    <asp:Label ID="lblSinteseAula" CssClass="textareaInfo" runat="server"
                                                        Text="<%$ Resources:Mensagens, AULAS_SINTESE %>"></asp:Label>
                                                    <asp:TextBox ID="txtSinteseAula" runat="server" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div id="divRegistroAula" runat="server">
                                                <asp:Label ID="LabelRegistroAula" runat="server" Text="<%$ Resources:Mensagens, MSG_PLANOAULA_REGISTROAULA %>"
                                                    AssociatedControlID="txtRegistroAula" Width="480px"></asp:Label>
                                                <div class="textareaComInfo">
                                                    <asp:Label ID="lblRegistroAulaInfo" CssClass="textareaInfo" runat="server"
                                                        Text="<%$ Resources:Mensagens, AULAS_AVALIACAOAULA %>"></asp:Label>
                                                    <asp:TextBox ID="txtRegistroAula" runat="server" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox>
                                                </div>
                                            </div>
                                            <asp:UpdatePanel ID="updAtividadeCasa" UpdateMode="Always" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBox ID="chkAtividadeCasa" Text="<%$ Resources:Academico, ControleTurma.DiarioClasse.chkAtividadeCasa.Text %>" runat="server"
                                                        EnableViewState="false" onclick="SetaAtividadePraCasa();" />
                                                    <div id="divAtividadeCasa" runat="server" style="display: none">
                                                        <asp:Label ID="lblAtividadeCasa" runat="server" Text="<%$ Resources:Academico, ControleTurma.DiarioClasse.lblAtividadeCasa.Text %>" AssociatedControlID="txtRegistroAula"
                                                            Width="480px"></asp:Label>
                                                        <div class="textareaComInfo">
                                                            <asp:Label ID="lbltividadeCasaInfo" CssClass="textareaInfo" runat="server"
                                                                Text="<%$ Resources:Mensagens, AULAS_ATIVIDADECASA %>"></asp:Label>
                                                            <asp:TextBox ID="txtAtividadeCasa" runat="server" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <div id="divObjetosAprendizagem" runat="server">
                                                <asp:UpdatePanel ID="updCalendario" runat="server" UpdateMode="Always">
                                                    <ContentTemplate>
                                                        <fieldset>
                                                            <legend><asp:Label ID="lblLgdObjAprendizagem" CssClass="textareaInfo" runat="server"
                                                                    Text="<%$ Resources:Academico, ControleTurma.DiarioClasse.lblLgdObjAprendizagem.Text %>"></asp:Label></legend>
                                                            <div></div>
                                                            <asp:Repeater ID="rptCampos" runat="server">
                                                                <HeaderTemplate>
                                                                    <div class="checkboxlist-columns">
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:HiddenField ID="hdnId" runat="server" Value='<%# Eval("oap_id") %>' />
                                                                    <asp:CheckBox ID="ckbCampo" runat="server" Text='<%# Eval("oap_descricao") %>' />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    </div> 
                                                                </FooterTemplate>
                                                            </asp:Repeater>

                                                        </fieldset>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <fieldset runat="server" id="fsRecursosUtilizados">
                                            <legend>Recursos utilizados</legend>
                                            <div>
                                                <asp:CheckBoxList ID="chkRecursos" runat="server" DataTextField="rsa_nome" DataValueField="rsa_id"
                                                    OnDataBound="chkRecursos_DataBound" AutoPostBack="false">
                                                </asp:CheckBoxList>
                                                <asp:TextBox ID="txtOutroRecurso" runat="server" SkinID="text20C" Style="display: none"
                                                    MaxLength="100"></asp:TextBox>
                                            </div>
                                        </fieldset>

                                    </div>
                                </asp:Panel>
                            </fieldset>
                        </div>
                        <div id="divTabsPlanejamento-1">
                            <asp:Panel ID="pnlPanoAulaPlanejamento" runat="server">
                                <asp:HiddenField ID="hdnTpcId" runat="server" Value='<%# Eval("tpc_id") %>' />
                                <asp:HiddenField ID="hdnTpcOrdem" runat="server" Value='<%# Eval("tpc_ordem") %>' />
                                <fieldset id="fdsCOC" runat="server">
                                    <div id="divCOC" runat="server">
                                        <div id="divTurmaDisciplinaComponentePlanejamento" runat="server">
                                            <asp:Label ID="lblTurmaDisciplinaComponentePlanejamento" runat="server" Text="<%$ Resources:Mensagens, MSG_DISCIPLINA %>" AssociatedControlID="ddlTurmaDisciplinaComponentePlanejamento"></asp:Label>
                                            <asp:DropDownList ID="ddlTurmaDisciplinaComponentePlanejamento" runat="server" AppendDataBoundItems="True"
                                                AutoPostBack="true" DataTextField="tur_tud_nome" DataValueField="tur_tud_id" SkinID="text60C"
                                                OnSelectedIndexChanged="ddlTurmaDisciplinaComponentePlanejamento_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <br />
                                            <br />
                                        </div>
                                        <fieldset id="fdsHabilidadesCOC" runat="server" class="fsdTreeview">
                                            <asp:Label ID="lblMensagemHabilidade" runat="server" CssClass="Info"></asp:Label>
                                            <legend style="position: relative; font-size: 90%"><span id="spanOrientacao" runat="server">Objetivos, conteúdos e habilidades das orientações
                                    curriculares</span> <span style="position: absolute; right: 8px; top: 4px;">
                                        <span style="display: inline-block; width: 70px; text-align: center; border-right: 1px solid white; border-left: 1px solid #fff; overflow: hidden; text-overflow: ellipsis; white-space: nowrap;" title="Planejada">Planejada</span>
                                        <span style="display: inline-block; width: 70px; text-align: center; border-right: 1px solid white; overflow: hidden; text-overflow: ellipsis; white-space: nowrap;" title="Trabalhada">Trabalhada</span>
                                    </span></legend>
                                            <div style="clear: both;">
                                                <div>
                                                </div>
                                                <div class="divTreeviewScrollCOC">
                                                    <asp:Repeater ID="rptHabilidadesCOC" runat="server" OnItemDataBound="rptHabilidades_ItemDataBound">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litCabecalho" runat="server"></asp:Literal>
                                                            <asp:HiddenField ID="hdnPermiteLancamento" runat="server" Value='<%# Eval("PermiteLancamento") %>' />
                                                            <asp:Literal ID="litConteudo" runat="server"></asp:Literal>
                                                            <div style="display: table-row;" id="divHabilidade" runat="server">
                                                                <asp:HiddenField ID="hdnChave" runat="server" />
                                                                <span style="display: table-cell; text-align: left; vertical-align: top;">
                                                                    <asp:Literal ID="lblHabilidade" runat="server"></asp:Literal>
                                                                </span>
                                                                <span style="display: table-cell; width: 70px; text-align: center; vertical-align: top; vertical-align: middle;">
                                                                    <asp:CheckBox ID="chkPlanejado" runat="server" Checked='<%# Eval("Planejado") %>'></asp:CheckBox><br />
                                                                    <asp:Label CssClass="lblLegenda" ID="lblLegendaPlanejado" runat="server" Text="" Visible="false"></asp:Label>
                                                                </span><span style="display: table-cell; width: 70px; text-align: center; vertical-align: top; vertical-align: middle;">
                                                                    <asp:CheckBox ID="chkTrabalhado" runat="server" Checked='<%# Eval("Trabalhado") %>'></asp:CheckBox><br />
                                                                    <asp:Label CssClass="lblLegenda" ID="lblLegendaTrabalhado" runat="server" Text="" Visible="false"></asp:Label>
                                                                </span>
                                                            </div>
                                                            <asp:Literal ID="litRodape" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </div>
                                            </div>
                                        </fieldset>
                                        <asp:Label ID="lblMsgMarcarHabilidades" runat="server" Text='<%# MSTech.CoreSSO.BLL.UtilBO.GetErroMessage("Marcar as habilidades planejadas a serem trabalhadas durante o COC.", MSTech.CoreSSO.BLL.UtilBO.TipoMensagem.Informacao) %>'></asp:Label>
                                        <fieldset id="fdsLegendaNaoAlcancadasCOC" runat="server">

                                            <div id="divLegendaNivelAprendizado" runat="server" style="width: 300px; padding: 6px;" visible="false">
                                                <b>Níveis de aprendizado:</b>
                                                <div style="border-style: solid; border-width: thin;">
                                                    <table id="tbLegendaNivelAprendizado" style="border-collapse: separate !important; border-spacing: 2px !important;">
                                                        <asp:Repeater ID="rptLegendaNivelAprendizado" runat="server">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblLegendaNivelAprendizado" runat="server" Text='<%# Bind("nivelAprendizado") %>'></asp:Label></td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </table>
                                                </div>
                                                <br />
                                            </div>

                                            <div style="margin: 5px;">
                                                <b>Legenda:</b>
                                                <div id="divLegendaHabilidades" runat="server" style="border-style: solid; border-width: thin; width: 400px;">
                                                    <table id="tbLegendaHabilidades" runat="server" style="border-collapse: separate !important; border-spacing: 2px !important;">
                                                        <tr>
                                                            <td height="15px" width="25px"></td>
                                                            <td>
                                                                <asp:Label ID="lblHabilidadePlanej" runat="server" Text="<%$ Resources:Academico, ControleTurma.DiarioClasse.lblHabilidadePlanej.Text %>"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td height="15px" width="25px"></td>
                                                            <td>
                                                                <asp:Label ID="lblHabilidadeTrab" runat="server" Text="<%$ Resources:Academico, ControleTurma.DiarioClasse.lblHabilidadeTrab.Text %>"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </fieldset>
                                        <fieldset>
                                            <div id="div2" runat="server" visible="false">
                                                <asp:Label ID="lblQuantidadeAulas" runat="server" Text="Quantidade de aulas previstas" EnableViewState="false" AssociatedControlID="txtQuantidadeAulas"></asp:Label>
                                                <asp:Label ID="txtQuantidadeAulas" runat="server"></asp:Label>
                                            </div>
                                            <br />
                                            <div class="clear"></div>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDiagnosticoCOC" runat="server"></asp:Label>
                                                        <div class="textareaComInfo">
                                                            <asp:Label ID="lblDiagnosticoCOCInfo" CssClass="textareaInfo" runat="server"
                                                                Text="<%$ Resources:Mensagens, PLANEJAMENTO_COC_AVALIACAO %>"></asp:Label>
                                                            <asp:TextBox ID="txtDiagnosticoCOC" runat="server" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox>
                                                        </div>
                                                        <asp:Label ID="lblTdp_id_COC" runat="server" Visible="false" Text='<%# Eval("tdp_id") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                        <fieldset>
                                            <asp:Label ID="lblPlanejamentoCOC" runat="server"></asp:Label>
                                            <div class="textareaComInfo">
                                                <asp:Label ID="lblPlanejamentoCOCInfo" CssClass="textareaInfo" runat="server"
                                                    Text="<%$ Resources:Mensagens, PLANEJAMENTO_PLANEJAMENTOANUAL %>"></asp:Label>
                                                <asp:TextBox ID="txtPlanejamentoCOC" runat="server" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox><br />
                                            </div>
                                        </fieldset>
                                        <fieldset>
                                            <asp:Label ID="lblRecursosCOC" runat="server" Text="<%$ Resources:Mensagens, MSG_RECURSOSBIMESTRE %>"></asp:Label>
                                            <div class="textareaComInfo">
                                                <asp:Label ID="lblRecursosCOCInfo" CssClass="textareaInfo" runat="server"
                                                    Text="<%$ Resources:Mensagens, PLANEJAMENTO_RECURSOS %>"></asp:Label>
                                                <asp:TextBox ID="txtRecursosCOC" runat="server" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox><br />
                                            </div>
                                        </fieldset>
                                        <fieldset>
                                            <asp:Label ID="lblIntervencoesPedagogicasCOC" runat="server" Text="<%$ Resources:Mensagens, MSG_INTERVENCOESPEDAGOGICASBIMESTRE %>"></asp:Label>
                                            <div class="textareaComInfo">
                                                <asp:Label ID="lblIntervencoesPedagogicasCOCInfo" CssClass="textareaInfo" runat="server"></asp:Label>
                                                <asp:TextBox ID="txtIntervencoesPedagogicasCOC" runat="server" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox><br />
                                            </div>
                                        </fieldset>
                                        <fieldset>
                                            <asp:Label ID="lblRegistroIntervencoesCOC" runat="server" Text="<%$ Resources:Mensagens, MSG_REGISTROINTERVENCOESBIMESTRE %>"></asp:Label>
                                            <div class="textareaComInfo">
                                                <asp:Label ID="lblRegistroIntervencoesCOCInfo" CssClass="textareaInfo" runat="server"></asp:Label>
                                                <asp:TextBox ID="txtRegistroIntervencoesCOC" runat="server" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox><br />
                                            </div>
                                        </fieldset>
                                    </div>
                                </fieldset>
                            </asp:Panel>
                        </div>

                        <div id="divUsuarioAlteracaoPlanoAula" runat="server" visible="false">
                            <br />
                            <asp:Label ID="lblAlteracaoPlanoAula" runat="server" />
                            <br />
                            <br />
                        </div>
                    </div>

                    <fieldset>
                        <div class="right">
                            <asp:Button ID="btnSalvarPlanoAula" runat="server" Text="Salvar" ValidationGroup="Aula" OnClick="btnSalvarPlanoAula_Click" OnClientClick="$('#divPlanoAula').scrollTo(0,0);javascript:scroll(0,0);" />
                            <asp:Button ID="btnCancelarPlanoAula" runat="server" Text="Cancelar" OnClientClick="var exibirMensagemConfirmacao=true;$('#divPlanoAula').dialog('close'); return false;" />
                        </div>
                    </fieldset>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div id="divAvaliacao" runat="server" title="Avaliação do aluno na turma"
        class="hide">
        <asp:UpdatePanel ID="updAvaliacao" runat="server">
            <ContentTemplate>
                <asp:Label ID="Label1" runat="server" Text="Avaliação" AssociatedControlID="txtAvaliacao"></asp:Label>
                <asp:TextBox ID="txtAvaliacao" runat="server" TextMode="MultiLine" MaxLength="4000"
                    ReadOnly="true" SkinID="limite4000"></asp:TextBox>
                <asp:HiddenField ID="hdnAlu_id" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="right">
            <asp:Button ID="btnCancelarAvaliacaoAlunoNaTurma" runat="server" Text="Voltar" OnClientClick="var exibirMensagemConfirmacao=true;$('#divAvaliacao').dialog('close'); return false;" />
        </div>
    </div>

    <div id="divRelatorio" title="Relatório de avaliação" class="hide">
        <asp:UpdatePanel ID="updRelatorio" runat="server">
            <ContentTemplate>
                <fieldset id="fdsRelatorio" runat="server">
                    <asp:TextBox ID="txtRelatorio" runat="server" SkinID="text60C" TextMode="MultiLine"></asp:TextBox>
                    <asp:HiddenField ID="hdnIds" runat="server" />
                    <div class="right">
                        <asp:Button ID="btnSalvarRelatorio" runat="server" Text="Salvar" OnClick="btnSalvarRelatorio_Click" OnClientClick="$('#divRelatorio').scrollTo(0,0);javascript:scroll(0,0);" />
                        <asp:Button ID="btnCancelarRelatorio" runat="server" Text="Cancelar" CausesValidation="false"
                            OnClientClick="var exibirMensagemConfirmacao=true;$('#divRelatorio').dialog('close');" />
                    </div>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div id="divConfirmacaoExclusaoAulaDiretor" title="Confirmação" class="hide">
        <asp:UpdatePanel ID="updConfirmacaoExclusaoAulaDiretor" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="ConfirmacaoExclusaoAulaDiretor" />
                <fieldset>
                    <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" Visible="false" />
                    <div id="divJustificativa" runat="server">
                        <asp:Label ID="lblTipoJustificativaExclusaoAula" runat="server" Text="<%$ Resources:Academico, ControleTurma.DiarioClasse.lblTipoJustificativaExclusaoAula.Text %>"></asp:Label>
                        <asp:DropDownList ID="ddlTipoJustificativaExclusaoAula" runat="server" DataValueField="tje_id" SkinID="text60C"
                            DataTextField="tje_nome" AppendDataBoundItems="True">
                        </asp:DropDownList>
                        <asp:CompareValidator ID="cpvTipoJustificativaExclusaoAula" runat="server" ErrorMessage="<%$ Resources:Academico, ControleTurma.DiarioClasse.cpvTipoJustificativaExclusaoAula.ErrorMessage %>"
                            ControlToValidate="ddlTipoJustificativaExclusaoAula" Operator="NotEqual" ValueToCompare="-1"
                            Display="Dynamic" ValidationGroup="ConfirmacaoExclusaoAulaDiretor">*</asp:CompareValidator>
                    </div>
                    <br />
                    <div id="divObservacao" runat="server">
                        <asp:Label ID="lblObservacaoExclusaoAula" runat="server" Text="<%$ Resources:Academico, ControleTurma.DiarioClasse.lblObservacaoExclusaoAula.Text %>"></asp:Label>
                        <asp:TextBox ID="txtObservacaoExclusaoAula" runat="server" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox>
                    </div>
                    <div class="right">
                        <asp:Button ID="btnSalvarJustificativa" runat="server" ValidationGroup="ConfirmacaoExclusaoAulaDiretor" OnClick="btnSalvarJustificativa_Click"
                            Text="<%$ Resources:Academico, ControleTurma.DiarioClasse.btnSalvarJustificativa.Text %>" 
                            ToolTip="<%$ Resources:Academico, ControleTurma.DiarioClasse.btnSalvarJustificativa.ToolTip %>"/>
                        <asp:Button ID="btnCancelarJustificativa" runat="server" CausesValidation="False" OnClientClick="$('#divConfirmacaoExclusaoAulaDiretor').dialog('close'); return false;"
                            Text="<%$ Resources:Academico, ControleTurma.DiarioClasse.btnCancelarJustificativa.Text %>"
                            ToolTip="<%$ Resources:Academico, ControleTurma.DiarioClasse.btnCancelarJustificativa.ToolTip %>" />
                    </div>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <%-- Disciplinas compartilhadas --%>
    <uc10:UCSelecaoDisciplinaCompartilhada ID="UCSelecaoDisciplinaCompartilhada1" runat="server"></uc10:UCSelecaoDisciplinaCompartilhada>
    <asp:HiddenField ID="hdnValorTurmas" runat="server" />

    <input id="txtSelectedTab" type="hidden" runat="server" />
    <asp:HiddenField ID="hdnSituacaoTurmaDisciplina" runat="server" />
</asp:Content>
