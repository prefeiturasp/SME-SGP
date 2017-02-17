<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Avaliacao.aspx.cs" Inherits="GestaoEscolar.Academico.ControleTurma.Avaliacao" %>

<%@ PreviousPageType VirtualPath="~/Academico/ControleTurma/Busca.aspx" %>

<%@ Register Src="~/WebControls/Combos/UCComboOrdenacao.ascx" TagName="UCComboOrdenacao" TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Habilidades/UCHabilidades.ascx" TagName="UCHabilidades" TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/ControleTurma/UCControleTurma.ascx" TagName="UCControleTurma" TagPrefix="uc10" %>
<%@ Register Src="~/WebControls/NavegacaoTelaPeriodo/UCNavegacaoTelaPeriodo.ascx" TagName="UCNavegacaoTelaPeriodo" TagPrefix="uc13" %>
<%@ Register Src="~/WebControls/LancamentoAvaliacao/UCCadastroAvaliacao.ascx" TagName="UCCadastroAvaliacao" TagPrefix="uc14" %>
<%@ Register Src="~/WebControls/Mensagens/UCConfirmacaoOperacao.ascx" TagName="UCConfirmacaoOperacao" TagPrefix="uc5" %>
<%@ Register Src="~/WebControls/ControleTurma/UCSelecaoDisciplinaCompartilhada.ascx" TagName="UCSelecaoDisciplinaCompartilhada" TagPrefix="uc10" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var idbtnCompensacaoAusencia = "";
        var idhdbtnCompensacaoAusenciaVisible = "";
        var idDdlOrdenacaoFrequencia = "";
        var idDdlOrdenacaoAvaliacao = '#<%=UCComboOrdenacaoAvaliacao.ComboClientID%>';
        var idhdnOrdenacaoFrequencia = "";
        var idhdnOrdenacaoAvaliacao = '#<%=hdnOrdenacaoAvaliacao.ClientID%>';
        var commandoAvaliacao = false;

        function ClickAvaliacao(argumento) {
            if (!commandoAvaliacao) {
                __doPostBack('pnlAvaliacao', argumento);
            }
            commandoAvaliacao = false;
        }
    </script>
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

        <asp:HiddenField ID="hdnOrdenacaoAvaliacao" runat="server" />

        <div runat="server" id="divMessageTurmaAnterior"
            class="summaryMsgAnosAnteriores" style="<%$ Resources: Academico, ControleTurma.Busca.divMessageTurmaAnterior.Style %>">
            <asp:Label runat="server" ID="lblMessageTurmaAnterior" Text="<%$ Resources:Academico, ControleTurma.Busca.lblMessageTurmaAnterior.Text %>"
                Style="<%$ Resources: Academico, ControleTurma.Busca.lblMessageTurmaAnterior.Style %>"></asp:Label>
        </div>

        <!-- Botões de navegação -->
        <uc13:UCNavegacaoTelaPeriodo ID="UCNavegacaoTelaPeriodo" runat="server" />
    </fieldset>

            <asp:Panel ID="pnlListao" runat="server" DefaultButton="btnSalvar">
                <uc5:UCConfirmacaoOperacao ID="UCConfirmacaoOperacao1" runat="server" ObservacaoVisivel="false" ObservacaoObrigatorio="false" />
        <fieldset id="fdsPesquisa" class="fieldset-filtro-avaliacao" runat="server">
            <asp:Label ID="litPesquisa" runat="server" class="titulo-filtro" Text="<%$ Resources:Academico, ControleTurma.Avaliacao.litPesquisa.Text %>"></asp:Label>
            <asp:CheckBoxList ID="cblQualificadorAtividade" runat="server" DataTextField="Value" DataValueField="Key" AutoPostBack="false" RepeatLayout="Flow" RepeatDirection="Horizontal"></asp:CheckBoxList>
        </fieldset>
        <fieldset>
            <asp:UpdatePanel ID="updListao" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnSalvarCima" />
                    <asp:PostBackTrigger ControlID="btnSalvar" />
                    <asp:PostBackTrigger ControlID="btnNovaAvaliacao" />
                </Triggers>
                    <ContentTemplate>
                        <asp:HiddenField ID="hdnConfirmArguments" runat="server" />
                        <asp:HiddenField ID="hdnListaoSelecionado" runat="server" />
                        <asp:Label ID="lblMessage2" runat="server" EnableViewState="false"></asp:Label>
                        <asp:Label ID="_lblMsgRepeater" runat="server" Visible="false"></asp:Label>
                        <div id="divListao" runat="server">
                            <div class="right" style="border-bottom-right-radius: 0px; border-bottom-left-radius: 0px;">
                                <asp:Button ID="btnSalvarCima" runat="server" Text="Salvar" OnClick="btnSalvar_Click" />
                                <asp:Button ID="btnNovaAvaliacao" runat="server" Text="<%$ Resources:Academico, ControleTurma.Avaliacao.btnNovaAvaliacao.Text %>" OnClick="btnNovaAvaliacao_Click" />
                            </div>
                                                
                        <asp:Panel ID="pnlListTurmaDisciplina" runat="server" Visible="false">
                                <asp:DropDownList ID="ddlTurmaDisciplinaListao" runat="server" AppendDataBoundItems="True"
                                    AutoPostBack="true" DataTextField="tur_tud_nome" DataValueField="tur_tud_id"
                                    SkinID="text60C" OnSelectedIndexChanged="ddlTurmaDisciplinaListao_SelectedIndexChanged">
                                </asp:DropDownList>
                            <br />
                            </asp:Panel>

                            <asp:Panel ID="pnlLancamentoAvaliacao" runat="server" Visible="false">
                            <div>
                                <asp:Label ID="_lblMsgRepeaterAvaliacao" runat="server"></asp:Label>
                            </div>
                            <div>
                                <asp:Label ID="lblComponenteListao" runat="server" Text="<%$ Resources:Mensagens, MSG_DISCIPLINA %>" AssociatedControlID="ddlComponenteListao"></asp:Label>
                                <asp:DropDownList ID="ddlComponenteListao" runat="server" AppendDataBoundItems="True"
                                    AutoPostBack="true" DataTextField="tur_tud_nome" DataValueField="tur_tud_id" SkinID="text60C" OnSelectedIndexChanged="ddlComponenteListao_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <uc2:UCComboOrdenacao ID="UCComboOrdenacaoAvaliacao" runat="server" />
                            </div>
                                <asp:Repeater ID="rptAlunosAvaliacao" runat="server" OnItemDataBound="rptAlunosAvaliacao_ItemDataBound">
                                    <HeaderTemplate>
                                    <div class="fixedLeftColumnWrapper">
											<div class="marginLeft">
                                        <div class="fixedLeftColumnInnerWrapper">
                                            <table id="tabela" class="grid tbLancamentoAvaliacoes sortableAvaliacoes" cellspacing="0">
                                                <thead>
                                                    <tr class="gridHeader" style="height: 60px;">
                                                        <th class="fixedLeftColumn">
                                                            <asp:Label ID="_lblNumChamada" runat="server" Text='Nº Chamada'></asp:Label>
                                                        </th>
                                                        <th class="fixedLeftColumn">
                                                            <asp:Label ID="lblNome" runat="server" Text='Nome do aluno'></asp:Label>
                                                        </th>
                                                        <asp:Repeater ID="rptAtividadesAvaliacao" runat="server" OnItemDataBound="rptAtividadesHeader_ItemDataBound"
                                                            OnItemCommand="rptAtividadesAvaliacao_ItemCommand">
                                                            <ItemTemplate>
                                                                <th class="center {sorter :false}" style="border-left: 0.1em dotted #FFFFFF; padding-right: 3px; visibility: hidden;">
                                                                    <asp:Panel ID="pnlAvaliacao" runat="server" Width="100%" Height="100%">
                                                                        <asp:Label ID="lbltau_id" runat="server" Text='<%#Bind("tau_id") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lbltud_id" runat="server" Text='<%#Bind("tud_id") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblPosicao" runat="server" Text='<%#Bind("tdt_posicao") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblDataAula" runat="server" Text='<%#Bind("tnt_data") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblAtividadeExclusiva" runat="server" Text='<%#Bind("tnt_exclusiva") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblUsuIdAtiv" runat="server" Text='<%#Bind("usu_id") %>' Visible="false"></asp:Label>
                                                                        <asp:HiddenField ID="hdnTavId" runat="server" Value='<%#Bind("tav_id") %>' />
                                                                        <asp:HiddenField ID="hdnQatId" runat="server" Value='<%#Bind("qat_id") %>' />
                                                                        <asp:HiddenField ID="hdnTntId" runat="server" Value='<%#Bind("tnt_id") %>' />
                                                                        <div style="display: inline-block; width: 100%;">
                                                                            <asp:ImageButton ID="btnExcluirAvaliacao" runat="server"
                                                                                ToolTip='<%$ Resources:Academico, ControleTurma.Avaliacao.btnExcluirAvaliacao.Text %>'
                                                                                SkinID="btExcluirSemMensagem" Style="float: right;"
                                                                                OnClick="btnExcluirAvaliacao_Click"
                                                                                OnClientClick="commandoAvaliacao=true;" />   
                                                                            <asp:Label ID="lblAtividade" runat="server" Text='<%#Bind("nome") %>'></asp:Label>
                                                                            <br />
                                                                            <asp:Label ID="lbltnt_data" runat="server" Text='<%#Bind("tnt_data") %>'></asp:Label>
                                                                            <asp:ImageButton ID="btnAtualizarAvaliacaoAutomatica" runat="server" SkinID="btAtualizar" CommandName="AtualizarNotas"
                                                                                CommandArgument='<%# Eval("tnt_id") %>' ToolTip="<%$ Resources:Academico, ControleTurma.Listao.btnAtualizarAvaliacaoAutomatica.ToolTip %>"
                                                                                OnClientClick="commandoAvaliacao=true;" />
                                                                        </div>
                                                                    </asp:Panel>
                                                                </th>
                                                            </ItemTemplate>
                                                        </asp:Repeater>  
                                                    </tr>
                                                </thead>
                                                <tbody>
                                    </HeaderTemplate>
                                    <AlternatingItemTemplate>
                                        <tr class="gridAlternatingRow">
                                            <td runat="server" id="tdNumChamadaAvaliacao" class="center fixedLeftColumn" style="text-align: center;">
                                                <asp:Label ID="lblalu_id" runat="server" Text='<%#Bind("alu_id") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblmtu_id" runat="server" Text='<%#Bind("mtu_id") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblmtd_id" runat="server" Text='<%#Bind("mtd_id") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblava_id" runat="server" Text='<%#Bind("ava_id") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblAtividade" runat="server" Text='<%#Bind("numeroChamada") %>'></asp:Label>
                                            </td>
                                            <td runat="server" id="tdNomeAvaliacao" class="fixedLeftColumn">
                                                <asp:Label ID="lblNome" runat="server" Text='<%#Bind("pes_nome") %>'></asp:Label>
                                                <asp:Label ID="lblNomeOficial" runat="server" Text='<%#Bind("pes_nome") %>' Visible="false">
                                                </asp:Label>
                                            </td>
                                            <asp:Repeater ID="rptAtividadesAvaliacao" runat="server" OnItemDataBound="rptAtividades_ItemDataBound">
                                                <ItemTemplate>
                                                <td runat="server" id="tdAtividadesAtivAva" class="center" style="text-align: center; visibility: hidden">
                                                        <div id="divAtividades" runat="server" style="display: inline-block; width: 100%;">
                                                            <asp:Label ID="lbltau_id" runat="server" Text='<%#Bind("tau_id") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblPosicao" runat="server" Text='<%#Bind("tdt_posicao") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblUsuIdAtiv" runat="server" Text='<%#Bind("usu_id") %>' Visible="false"></asp:Label>
                                                            <asp:HiddenField ID="hdnQatId" runat="server" Value='<%#Bind("qat_id") %>' />
                                                            <asp:HiddenField ID="hdnTntId" runat="server" Value='<%#Bind("tnt_id") %>' />
                                                            <asp:HiddenField ID="hdnTntIdPai" runat="server" Value='<%#Bind("tnt_idRelacionadaPai") %>' />
                                                            <asp:CheckBox ID="chkParticipante" runat="server" Text="Participante" Style="display: inline-block;" /><br />
                                                        <asp:TextBox ID="txtNota" runat="server" SkinID="Decimal" MaxLength="6"></asp:TextBox>
                                                            <asp:DropDownList ID="ddlPareceres" runat="server" DataTextField="descricao" DataValueField="eap_valor">
                                                            </asp:DropDownList>
                                                            <asp:ImageButton ID="btnRelatorio" runat="server" SkinID="btDetalhar" OnClick="btnRelatorio_Click"
                                                                ToolTip="Lançar relatório" Width="16px" Height="16px" />
                                                            <asp:Image ID="imgSituacao" runat="server" SkinID="imgConfirmar" ToolTip="Relatório lançado"
                                                                Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                                                        <asp:ImageButton ID="btnHabilidade" runat="server" SkinID="btHabilidadesAluno" OnClick="btnHabilidade_Click" Style="vertical-align: bottom;"
                                                                ToolTip="<%$ Resources:Academico, ControleTurma.Avaliacao.btnHabilidade.ToolTip %>" />
                                                        <asp:CheckBox ID="chkFalta" runat="server" SkinID="chkFalta" Text="&nbsp;" ToolTip="<%$ Resources:Academico, ControleTurma.Listao.chkFalta.ToolTip %>" />
                                                        </div>
                                                    </td>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <ItemTemplate>
                                        <tr class="gridRow">
                                            <td runat="server" id="tdNumChamadaAvaliacao" class="center fixedLeftColumn" style="text-align: center;">
                                                    <asp:Label ID="lblalu_id" runat="server" Text='<%#Bind("alu_id") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblmtu_id" runat="server" Text='<%#Bind("mtu_id") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblmtd_id" runat="server" Text='<%#Bind("mtd_id") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblava_id" runat="server" Text='<%#Bind("ava_id") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblAtividade" runat="server" Text='<%#Bind("numeroChamada") %>'></asp:Label>
                                            </td>
                                            <td runat="server" id="tdNomeAvaliacao" class="fixedLeftColumn">
                                                    <asp:Label ID="lblNome" runat="server" Text='<%#Bind("pes_nome") %>'></asp:Label>
                                                    <asp:Label ID="lblNomeOficial" runat="server" Text='<%#Bind("pes_nome") %>' Visible="false">
                                                    </asp:Label>
                                            </td>
                                            <asp:Repeater ID="rptAtividadesAvaliacao" runat="server" OnItemDataBound="rptAtividades_ItemDataBound">
                                                <ItemTemplate>
                                                <td runat="server" id="tdAtividadesAtivAva" class="center" style="text-align: center; visibility: hidden">
                                                        <div id="divAtividades" runat="server" style="display: inline-block; width: 100%;">
                                                            <asp:Label ID="lblPosicao" runat="server" Text='<%#Bind("tdt_posicao") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblUsuIdAtiv" runat="server" Text='<%#Bind("usu_id") %>' Visible="false"></asp:Label>
                                                            <asp:HiddenField ID="hdnQatId" runat="server" Value='<%#Bind("qat_id") %>' />
                                                            <asp:HiddenField ID="hdnTntId" runat="server" Value='<%#Bind("tnt_id") %>' />
                                                            <asp:HiddenField ID="hdnTntIdPai" runat="server" Value='<%#Bind("tnt_idRelacionadaPai") %>' />
                                                            <asp:CheckBox ID="chkParticipante" runat="server" Text="Participante" Style="display: inline-block;" /><br />
                                                        <asp:TextBox ID="txtNota" runat="server" SkinID="Decimal" MaxLength="6"></asp:TextBox>
                                                            <asp:DropDownList ID="ddlPareceres" runat="server" DataTextField="descricao" DataValueField="eap_valor">
                                                            </asp:DropDownList>
                                                            <asp:ImageButton ID="btnRelatorio" runat="server" SkinID="btDetalhar" OnClick="btnRelatorio_Click"
                                                                ToolTip="Lançar relatório" />
                                                            <asp:Image ID="imgSituacao" runat="server" SkinID="imgConfirmar" ToolTip="Relatório lançado"
                                                                Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                                                        <asp:ImageButton ID="btnHabilidade" runat="server" SkinID="btHabilidadesAluno" OnClick="btnHabilidade_Click" Style="vertical-align: bottom;"
                                                                ToolTip="<%$ Resources:Academico, ControleTurma.Avaliacao.btnHabilidade.ToolTip %>" />
                                                        <asp:CheckBox ID="chkFalta" runat="server" SkinID="chkFalta" Text="&nbsp;" ToolTip="<%$ Resources:Academico, ControleTurma.Listao.chkFalta.ToolTip %>" />
                                                        </div>
                                                    </td>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>
                                        </table></div></div></div>
                                    </FooterTemplate>
                                </asp:Repeater>
                                <br />
                                <b>Legenda:</b>
                                <table id="tbLegendaListao" runat="server" style="border-style: solid; border-width: thin; width: 265px; border-collapse: separate !important; border-spacing: 2px !important;">
                                    <tr id="trExibirAlunoDispensadoListao" runat="server">
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
                                <div id="divUsuarioAlteracaoMedia" runat="server" visible="false">
                                    <br />
                                    <asp:Label ID="lblAlteracaoMedia" runat="server" />
                                    <br />
                                </div>
                            </asp:Panel>
                            <div class="right divBtnCadastro">
                                <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click" />
                            </div>
                        </div>
                        <asp:HiddenField runat="server" ID="hdnPermiteRecuperacaoQualquerNota" Value="" />
                        <asp:HiddenField runat="server" ID="hdnMinimoAprovacaoDocente" Value="" />
                    </ContentTemplate>
                </asp:UpdatePanel>
        </fieldset>
        <fieldset class="fieldset-FixedLeftColumn" style="visibility: hidden"></fieldset>
            </asp:Panel>

    <input id="txtSelectedTab" type="hidden" runat="server" />

    <div id="divHabilidadesRelacionadas" title="Habilidades Relacionadas" class="hide">
        <asp:UpdatePanel ID="udpHabilidades" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblMensagemHabilidade" runat="server" Text=""></asp:Label>
                <asp:Literal runat="server" ID="litHabilidades"></asp:Literal>
                <uc4:UCHabilidades runat="server" ID="UCHabilidades" TituloFildSet="Expectativa de aprendizagem" LegendaCheck="Não alcançada" bHabilidaEdicao="True" />
                <div class="right">
                    <asp:Button ID="btnSalvarHabilidadesRelacionadas" runat="server" Text="Salvar" OnClick="btnSalvarHabilidadesRelacionadas_Click" />
                    <asp:Button ID="btnCancelarHabilidadesRelacionadas" runat="server" Text="Cancelar" CausesValidation="false"
                        OnClientClick="$('#divHabilidadesRelacionadas').dialog('close');" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div id="divRelatorio" title="Relatório de avaliação" class="hide">
        <asp:UpdatePanel ID="updRelatorio" runat="server">
            <ContentTemplate>
                <fieldset id="fdsRelatorio" runat="server">
                    <asp:Label ID="lblDadosRelatorio" runat="server"></asp:Label>
                    <br />
                    <br />
                    <asp:TextBox ID="txtRelatorio" runat="server" SkinID="text60C" TextMode="MultiLine"></asp:TextBox>
                    <asp:HiddenField ID="hdnIds" runat="server" />
                    <div class="right">
                        <asp:Button ID="btnSalvarRelatorio" runat="server" Text="Salvar" OnClick="btnSalvarRelatorio_Click" />
                        <asp:Button ID="btnCancelarRelatorio" runat="server" Text="Cancelar" CausesValidation="false"
                            OnClientClick="$('#divRelatorio').dialog('close');" />
                    </div>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div id="divAtividadeAvaliativa" title="Lançar atividade avaliativa" class="hide">    
        <uc14:UCCadastroAvaliacao runat="server" ID="UCCadastroAvaliacao"></uc14:UCCadastroAvaliacao>           
    </div>

    <%-- Disciplinas compartilhadas --%>
    <uc10:UCSelecaoDisciplinaCompartilhada ID="UCSelecaoDisciplinaCompartilhada1" runat="server"></uc10:UCSelecaoDisciplinaCompartilhada>
    <asp:HiddenField ID="hdnValorTurmas" runat="server" />

</asp:Content>
