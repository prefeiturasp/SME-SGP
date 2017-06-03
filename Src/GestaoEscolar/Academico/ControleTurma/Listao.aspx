<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Listao.aspx.cs" Inherits="GestaoEscolar.Academico.ControleTurma.Listao" %>

<%@ PreviousPageType VirtualPath="~/Academico/ControleTurma/Busca.aspx" %>

<%@ Register Src="~/WebControls/Combos/UCComboOrdenacao.ascx" TagName="UCComboOrdenacao" TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Habilidades/UCHabilidades.ascx" TagName="UCHabilidades" TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros" TagPrefix="uc8" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao" TagPrefix="uc9" %>
<%@ Register Src="~/WebControls/ControleTurma/UCControleTurma.ascx" TagName="UCControleTurma" TagPrefix="uc10" %>
<%@ Register Src="~/WebControls/NavegacaoTelaPeriodo/UCNavegacaoTelaPeriodo.ascx" TagName="UCNavegacaoTelaPeriodo" TagPrefix="uc13" %>
<%@ Register Src="~/WebControls/LancamentoFrequencia/UCLancamentoFrequencia.ascx" TagName="UCLancamentoFrequencia" TagPrefix="uc1" %>
<%@ Register src="~/WebControls/ControleTurma/UCSelecaoDisciplinaCompartilhada.ascx" tagname="UCSelecaoDisciplinaCompartilhada" tagprefix="uc10" %>
<%@ Register Src="~/WebControls/LancamentoFrequencia/UCLancamentoFrequenciaTerritorio.ascx" TagName="UCLancamentoFrequenciaTerritorio" TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoAtividadeAvaliativa.ascx" TagName="UCComboTipoAtividadeAvaliativa" TagPrefix="uc11" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios" TagPrefix="uc12" %>
<%@ Register Src="~/WebControls/Mensagens/UCConfirmacaoOperacao.ascx" TagName="UCConfirmacaoOperacao" TagPrefix="uc13" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var idbtnCompensacaoAusencia = '#<%=btnCompensacaoAusencia.ClientID%>';
        var idhdbtnCompensacaoAusenciaVisible = '#<%=hdbtnCompensacaoAusenciaVisible.ClientID%>';
        var idDdlOrdenacaoFrequenciaTerritorio = '#<%=UCLancamentoFrequenciaTerritorio.ClientIdComboOrdenacao%>';
        var idDdlOrdenacaoFrequencia = '#<%= UCLancamentoFrequencia.ClientIdComboOrdenacao%>';
        var idDdlOrdenacaoAvaliacao = '#<%=UCComboOrdenacaoAvaliacao.ComboClientID%>';
        var idDdlOrdenacaoAtivExtra = '#<%=UCComboOrdenacaoAtivExtra.ComboClientID%>';
        var idhdnOrdenacaoFrequenciaTerritorio = '#<%=UCLancamentoFrequenciaTerritorio.ClientIdHdnOrdenacao%>';
        var idhdnOrdenacaoFrequencia = '#<%=UCLancamentoFrequencia.ClientIdHdnOrdenacao%>';
        var idhdnOrdenacaoAvaliacao = '#<%=hdnOrdenacaoAvaliacao.ClientID%>';
        var idhdnOrdenacaoAtivExtra = '#<%=hdnOrdenacaoAtivExtra.ClientID%>';
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            <asp:Label ID="lblPeriodoEfetivado" runat="server" EnableViewState="false" Visible="false"></asp:Label>
            <asp:Label ID="lblAulasSemPlano" runat="server" Visible="false"></asp:Label>
            <asp:ValidationSummary ID="vsAtividadeExtra" runat="server" ValidationGroup="AtividadeExtraclasse" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <fieldset>
        <uc10:UCControleTurma ID="UCControleTurma1" runat="server" />
        <asp:HiddenField ID="hdnOrdenacaoAvaliacao" runat="server" />
        <asp:HiddenField ID="hdnOrdenacaoAtivExtra" runat="server" />

        <div runat="server" id="divMessageTurmaAnterior"
            class="summaryMsgAnosAnteriores" style="<%$ Resources: Academico, ControleTurma.Busca.divMessageTurmaAnterior.Style %>">
            <asp:Label runat="server" ID="lblMessageTurmaAnterior" Text="<%$ Resources:Academico, ControleTurma.Busca.lblMessageTurmaAnterior.Text %>"
                Style="<%$ Resources: Academico, ControleTurma.Busca.lblMessageTurmaAnterior.Style %>"></asp:Label>
        </div>

        <!-- Botões de navegação -->
        <uc13:UCNavegacaoTelaPeriodo ID="UCNavegacaoTelaPeriodo" runat="server" />

        <div style="margin-top: 10px;">
            <asp:Panel ID="pnlListao" runat="server">
                <asp:UpdatePanel ID="updListao" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:HiddenField ID="hdnAlterouFrequencia" runat="server" />
                        <asp:HiddenField ID="hdnAlterouNota" runat="server" />
                        <asp:HiddenField ID="hdnAlterouPlanoAula" runat="server" />
                        <asp:HiddenField ID="hdnAlterouAtividadeExtra" runat="server" />
                        <asp:HiddenField ID="hdnListaoSelecionado" runat="server" />
                        <asp:Label ID="lblMessage2" runat="server" EnableViewState="false"></asp:Label>
                        <div id="divListao" runat="server">
                            <div class="right area-botoes-top" style="border-bottom-right-radius: 0px; border-bottom-left-radius: 0px;">
                                <asp:Button ID="btnSalvarCima" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
                                    ValidationGroup="Frequencia" />
                            </div>
                            <br />
                            <div id="divTabsListao" style="font-size: 1em;" class="area-form">
                                <asp:Panel ID="pnlListTurmaDisciplina" runat="server" Visible ="false">
                                    <asp:DropDownList ID="ddlTurmaDisciplinaListao" runat="server" AppendDataBoundItems="True"
                                        AutoPostBack="true" DataTextField="tur_tud_nome" DataValueField="tur_tud_id"
                                        SkinID="text60C" OnSelectedIndexChanged="ddlTurmaDisciplinaListao_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <br /><br />
                                </asp:Panel>
                                <ul class="hide">
                                    <li><a href="#divTabsListao-0" id="aFrequencia" runat="server" visible="false" onclick="if ($(idhdbtnCompensacaoAusenciaVisible).val() == 'True') $(idbtnCompensacaoAusencia).show();">
                                        <asp:Label runat="server" ID="lblFrequencia" Text="<%$ Resources:Academico, ControleTurma.Listao.lblFrequencia.Text %>"></asp:Label> </a> </li>
                                    <li><a href="#divTabsListao-1" id="aAvaliacao" runat="server" visible="false" onclick="$(idbtnCompensacaoAusencia).hide();">
                                        <asp:Label runat="server" ID="lblAvaliacao" Text="<%$ Resources:Academico, ControleTurma.Listao.lblAvaliacao.Text %>"></asp:Label> </a></li>
                                    <li><a href="#divTabsListao-2" id="aPlanoAula" runat="server" visible="false" onclick="$(idbtnCompensacaoAusencia).hide();">
                                        <asp:Label runat="server" ID="lblPlanoAula" Text="<%$ Resources:Academico, ControleTurma.Listao.lblPlanoAula.Text %>"></asp:Label> </a></li>
                                    <li><a href="#divTabsListao-3" id="aAtividadeExtraClasse" runat="server" visible="false">
                                        <asp:Label runat="server" ID="lblAtividadeExtraClasse" Text="Listão de atividades extraclasse"></asp:Label></a></li>
                                    <%--<div id="msgTabs" class="msgTabs">Navegue entre as abas utilizando as setas.</div>--%>
                                </ul>
                                <div id="divTabsListao-0">
                                    <asp:Panel ID="pnlListaoLancamentoFrequencias" runat="server" Visible="false">
                                        <uc1:UCLancamentoFrequencia ID="UCLancamentoFrequencia" runat="server" ></uc1:UCLancamentoFrequencia>
                                        <uc3:UCLancamentoFrequenciaTerritorio ID="UCLancamentoFrequenciaTerritorio" runat="server"></uc3:UCLancamentoFrequenciaTerritorio>
                                    </asp:Panel>
                                </div>
                                <div id="divTabsListao-1">
                                    <asp:Panel ID="pnlLancamentoAvaliacao" runat="server" Visible="false">
                                        <asp:UpdatePanel ID="upnAvaliacao" runat="server">
                                            <ContentTemplate>
                                                <br />
                                                <asp:Literal ID="lblMsgParecerAvaliacao" runat="server" Text="Marque a opção Efetivado para indicar que o lançamento de notas da atividade foi finalizado.">
                                                </asp:Literal>
                                                <asp:Label Style="display: block" ID="lblMsgInfo" runat="server" Text=""></asp:Label>
                                                <asp:Label Style="display: block" ID="lblMsgTab" runat="server" Text=""></asp:Label>
                                                <asp:Label Style="display: block" ID="lblMsgAvaliacoes" runat="server" Text=""></asp:Label>
                                                <asp:Label ID="_lblMsgRepeaterAvaliacao" runat="server"></asp:Label>
                                                <%--CssClass="summary"></asp:Label>--%>
                                                <br />

                                                <asp:Label ID="lblComponenteListao" runat="server" Text="<%$ Resources:Mensagens, MSG_DISCIPLINA %>" AssociatedControlID="ddlComponenteListao"></asp:Label>
                                                <asp:DropDownList ID="ddlComponenteListao" runat="server" AppendDataBoundItems="True"
                                                    AutoPostBack="true" DataTextField="tur_tud_nome" DataValueField="tur_tud_id" SkinID="text60C" OnSelectedIndexChanged="ddlComponenteListao_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <br />
                                                <table>
                                                    <tr>
                                                        <td style="width: 90%; padding-top: 35px">
                                                            <uc2:UCComboOrdenacao ID="UCComboOrdenacaoAvaliacao" runat="server" />
                                                        </td>
                                                        <td>
                                                            <div style="width: 310px; float: right;" runat="server" id="divFormatoCalculo">
                                                                <fieldset>
                                                                    <asp:Label ID="lblFormatoCalculoMedia" runat="server" Text="Calcular nota final"></asp:Label>
                                                                    <div style="display: inline-block;">
                                                                        <asp:Button ID="btnCalculaMediaAritmetica" runat="server" Text="Média aritmética"
                                                                            OnClientClick="CalculaNotaFinal('Media');return false;" />
                                                                        <asp:Button ID="btnCalculaSoma" runat="server" Text="Soma das notas"
                                                                            OnClientClick="CalculaNotaFinal('Soma');return false;" />
                                                                    </div>
                                                                </fieldset>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:Repeater ID="rptAlunosAvaliacao" runat="server" OnItemDataBound="rptAlunosAvaliacao_ItemDataBound">
                                                    <HeaderTemplate>
                                                        <div>
                                                            <table id="tabela" class="grid tbLancamentoAvaliacoes sortableAvaliacoes grid-responsive-list" cellspacing="0">
                                                                <thead>
                                                                    <tr class="gridHeader" style="height: 30px;">
                                                                        <th class="center">
                                                                            <asp:Label ID="_lblNumChamada" runat="server" Text='Nº Chamada'></asp:Label>
                                                                        </th>
                                                                        <th>
                                                                            <asp:Label ID="lblNome" runat="server" Text='Nome do aluno'></asp:Label>
                                                                        </th>
                                                                        <asp:Repeater ID="rptAtividadesAvaliacao" runat="server" OnItemDataBound="rptAtividadesHeader_ItemDataBound"
                                                                            OnItemCommand="rptAtividadesAvaliacao_ItemCommand">
                                                                            <ItemTemplate>
                                                                                <th class="center {sorter :false}" style="border-left: 0.1em dotted #FFFFFF; padding-right: 3px;">
                                                                                    <asp:Label ID="lbltnt_id" runat="server" Text='<%#Bind("tnt_id") %>' Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lbltau_id" runat="server" Text='<%#Bind("tau_id") %>' Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lbltud_id" runat="server" Text='<%#Bind("tud_id") %>' Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lblPosicao" runat="server" Text='<%#Bind("tdt_posicao") %>' Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lblDataAula" runat="server" Text='<%#Bind("tnt_data") %>' Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lblAtividadeExclusiva" runat="server" Text='<%#Bind("tnt_exclusiva") %>' Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lblUsuIdAtiv" runat="server" Text='<%#Bind("usu_id") %>' Visible="false"></asp:Label>
                                                                                    <div style="display: inline-block; width: 100%;">
                                                                                        <asp:Label ID="lblAtividade" runat="server" Text='<%#Bind("nome") %>'></asp:Label>
                                                                                        <br />
                                                                                        <asp:Label ID="lbltnt_data" runat="server" Text='<%#Bind("tnt_data") %>'></asp:Label>
                                                                                        <div id="divDetalharHabilidades" runat="server" Style="display: none;">
                                                                                            <asp:ImageButton ID="btnDetalharHabilidades" runat="server" SkinID="btDetalhar" CommandName="DetalharHabilidades"
                                                                                                CommandArgument='<%#Eval("tnt_id")+";"+ Eval("tud_id")%>' ToolTip="Detalhar habilidades relacionadas" Style="display: none;" />
                                                                                        </div>
                                                                                    </div>
                                                                                </th>
                                                                            </ItemTemplate>
                                                                        </asp:Repeater>
                                                                        <th id="tdMedia" runat="server" class="center {sorter :false}" visible="false" style="border-left: 1px dotted #FFFFFF;">
                                                                            <asp:Label ID="lblMedia" runat="server" Text='Média'></asp:Label>
                                                                        </th>
                                                                    </tr>
                                                                    <tr class="gridRow grid-linha-destaque">
                                                                        <td class="grid-responsive-no-header"></td>
                                                                        <td class="grid-responsive-no-header"></td>
                                                                        <asp:Repeater ID="rptAtividadesAvaliacaoEfetivado" runat="server" OnItemDataBound="rptAtividadesHeader_ItemDataBound"
                                                                            OnItemCommand="rptAtividadesAvaliacao_ItemCommand">
                                                                            <ItemTemplate>
                                                                                <td class="center {sorter :false} grid-responsive-item-inline grid-responsive-center" style="border-left: 0.1em dotted #FFFFFF; padding-right: 3px;">
                                                                                    <asp:Label ID="lbltnt_id" runat="server" Text='<%#Bind("tnt_id") %>' Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lbltau_id" runat="server" Text='<%#Bind("tau_id") %>' Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lbltud_id" runat="server" Text='<%#Bind("tud_id") %>' Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lblPosicao" runat="server" Text='<%#Bind("tdt_posicao") %>' Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lblDataAula" runat="server" Text='<%#Bind("tnt_data") %>' Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lblAtividadeExclusiva" runat="server" Text='<%#Bind("tnt_exclusiva") %>' Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lblUsuIdAtiv" runat="server" Text='<%#Bind("usu_id") %>' Visible="false"></asp:Label>
                                                                                    <div style="display: inline-block; width: 100%; text-align: center;">
                                                                                        <asp:Label ID="lblAtividade" runat="server" Text='<%#Bind("nome") %>' Style="display: none;"></asp:Label>
                                                                                        <asp:Label ID="lbltnt_data" runat="server" Text='<%#Bind("tnt_data") %>' Style="display: none;"></asp:Label>
                                                                                        <div id="divDetalharHabilidades" runat="server">
                                                                                            <asp:ImageButton ID="btnDetalharHabilidades" runat="server" SkinID="btDetalhar" CommandName="DetalharHabilidades"
                                                                                                CommandArgument='<%#Eval("tnt_id")+";"+ Eval("tud_id")%>' ToolTip="Detalhar habilidades relacionadas" />
                                                                                            <br />
                                                                                        </div>
                                                                                        <asp:CheckBox ID="chkEfetivado" runat="server" Text="Efetivado" Style="display: inline-block;" />
                                                                                    </div>
                                                                                </td>
                                                                            </ItemTemplate>
                                                                        </asp:Repeater>
                                                                        <td id="tdMediaResponsivo" runat="server" class="grid-responsive-no-header" visible="false"></td>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                    </HeaderTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr class="gridAlternatingRow">
                                                            <td runat="server" id="tdNumChamadaAvaliacao" class="center" style="text-align: center;">
                                                                <asp:Label ID="lblalu_id" runat="server" Text='<%#Bind("alu_id") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblmtu_id" runat="server" Text='<%#Bind("mtu_id") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblmtd_id" runat="server" Text='<%#Bind("mtd_id") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblava_id" runat="server" Text='<%#Bind("ava_id") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblAtividade" runat="server" Text='<%#Bind("numeroChamada") %>'></asp:Label>
                                                            </td>
                                                            <td runat="server" id="tdNomeAvaliacao">
                                                                <asp:Label ID="lblNome" runat="server" Text='<%#Bind("pes_nome") %>'></asp:Label>
                                                                <asp:Label ID="lblNomeOficial" runat="server" Text='<%#Bind("pes_nome") %>' Visible="false">
                                                                </asp:Label>
                                                            </td>
                                                            <asp:Repeater ID="rptAtividadesAvaliacao" runat="server" OnItemDataBound="rptAtividades_ItemDataBound">
                                                                <ItemTemplate>
                                                                    <td runat="server" id="tdAtividadesAtivAva" class="center grid-responsive-item-inline grid-responsive-center" style="text-align: center;">
                                                                        <div id="divAtividades" runat="server" style="display: inline-block; width: 100%;">
                                                                            <asp:Label ID="lbltnt_id" runat="server" Text='<%#Bind("tnt_id") %>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="lbltau_id" runat="server" Text='<%#Bind("tau_id") %>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="lblPosicao" runat="server" Text='<%#Bind("tdt_posicao") %>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="lblUsuIdAtiv" runat="server" Text='<%#Bind("usu_id") %>' Visible="false"></asp:Label>
                                                                            <div class="media">
                                                                                <asp:HiddenField ID="lblNaoConstaMedia" runat="server" Value="false"></asp:HiddenField>
                                                                            </div>
                                                                            <asp:CheckBox ID="chkParticipante" runat="server" Text="Participante" Style="display: inline-block;" /><br class="responsive-hide"/>
                                                                            <asp:TextBox ID="txtNota" runat="server" SkinID="Decimal" Width="50" MaxLength="6"></asp:TextBox>
                                                                            <asp:CheckBox ID="chkDesconsiderar" runat="server" Text="D" ToolTip="Desconsiderar nota no cálculo da média" Style="display: inline-block;" />
                                                                            <asp:DropDownList ID="ddlPareceres" runat="server" DataTextField="descricao" DataValueField="eap_valor">
                                                                            </asp:DropDownList>
                                                                            <asp:ImageButton ID="btnRelatorio" runat="server" SkinID="btDetalhar" OnClick="btnRelatorio_Click"
                                                                                ToolTip="Lançar relatório" Width="16px" Height="16px" />
                                                                            <asp:Image ID="imgSituacao" runat="server" SkinID="imgConfirmar" ToolTip="Relatório lançado"
                                                                                Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                                                                        </div>
                                                                    </td>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                            <td id="tdMedia" runat="server" visible="false" class="center" style="text-align: center;">
                                                                <asp:Label ID="lblMedia" runat="server" Text="" EnableViewState="false" CssClass="spMedia"></asp:Label>
                                                                <asp:DropDownList ID="ddlParecerFinal" runat="server" DataTextField="descricao" DataValueField="eap_valor">
                                                                </asp:DropDownList>
                                                                <asp:TextBox ID="txtNotaFinal" runat="server" CssClass="notaFinal" SkinID="Decimal" Width="50px" MaxLength="6"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </AlternatingItemTemplate>
                                                    <ItemTemplate>
                                                        <tr class="gridRow">
                                                            <td runat="server" id="tdNumChamadaAvaliacao" class="center" style="text-align: center;">
                                                                <asp:Label ID="lblalu_id" runat="server" Text='<%#Bind("alu_id") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblmtu_id" runat="server" Text='<%#Bind("mtu_id") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblmtd_id" runat="server" Text='<%#Bind("mtd_id") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblava_id" runat="server" Text='<%#Bind("ava_id") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblAtividade" runat="server" Text='<%#Bind("numeroChamada") %>'></asp:Label>
                                                            </td>
                                                            <td runat="server" id="tdNomeAvaliacao">
                                                                <asp:Label ID="lblNome" runat="server" Text='<%#Bind("pes_nome") %>'></asp:Label>
                                                                <asp:Label ID="lblNomeOficial" runat="server" Text='<%#Bind("pes_nome") %>' Visible="false">
                                                                </asp:Label>
                                                            </td>
                                                            <asp:Repeater ID="rptAtividadesAvaliacao" runat="server" OnItemDataBound="rptAtividades_ItemDataBound">
                                                                <ItemTemplate>
                                                                    <td runat="server" id="tdAtividadesAtivAva" class="center grid-responsive-item-inline grid-responsive-center" style="text-align: center;">
                                                                        <div id="divAtividades" runat="server" style="display: inline-block; width: 100%;">
                                                                            <asp:Label ID="lbltnt_id" runat="server" Text='<%#Bind("tnt_id") %>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="lblPosicao" runat="server" Text='<%#Bind("tdt_posicao") %>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="lblUsuIdAtiv" runat="server" Text='<%#Bind("usu_id") %>' Visible="false"></asp:Label>
                                                                            <div class="media">
                                                                                <asp:HiddenField ID="lblNaoConstaMedia" runat="server" Value="false"></asp:HiddenField>
                                                                            </div>
                                                                            <asp:CheckBox ID="chkParticipante" runat="server" Text="Participante" Style="display: inline-block;" /><br class="responsive-hide"/>
                                                                            <asp:TextBox ID="txtNota" runat="server" SkinID="Decimal" Width="50" MaxLength="6"></asp:TextBox>
                                                                            <asp:CheckBox ID="chkDesconsiderar" runat="server" Text="D" ToolTip="Desconsiderar nota no cálculo da média" Style="display: inline-block;" />
                                                                            <asp:DropDownList ID="ddlPareceres" runat="server" DataTextField="descricao" DataValueField="eap_valor">
                                                                            </asp:DropDownList>
                                                                            <asp:ImageButton ID="btnRelatorio" runat="server" SkinID="btDetalhar" OnClick="btnRelatorio_Click"
                                                                                ToolTip="Lançar relatório" />
                                                                            <asp:Image ID="imgSituacao" runat="server" SkinID="imgConfirmar" ToolTip="Relatório lançado"
                                                                                Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                                                                        </div>
                                                                    </td>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                            <td id="tdMedia" runat="server" visible="false" class="center" style="text-align: center;">
                                                                <asp:Label ID="lblMedia" runat="server" Text="" EnableViewState="false" CssClass="spMedia"></asp:Label>
                                                                <asp:DropDownList ID="ddlParecerFinal" runat="server" DataTextField="descricao" DataValueField="eap_valor">
                                                                </asp:DropDownList>
                                                                <asp:TextBox ID="txtNotaFinal" runat="server" CssClass="notaFinal" SkinID="Decimal" Width="50px" MaxLength="6"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        </tbody>
                                                        </table></div>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                                <br />
                                                <b>Legenda:</b>
                                                <table id="tbLegendaListao" runat="server" style="border-style: solid; border-width: thin; width: 265px; border-collapse: separate !important; border-spacing: 2px !important;">
                                                    <tr>
                                                        <td style="border-style: solid; border-width: thin; width: 25px; height: 15px;"></td>
                                                        <td><asp:Literal runat="server" ID="litAusente" Text="<%$ Resources:Mensagens, MSG_ALUNO_AUSENTE %>"></asp:Literal>
                                                        </td>
                                                    </tr>
                                                    <tr id="trExibirAlunoDispensadoListao" runat="server">
                                                        <td style="border-style: solid; border-width: thin; width: 25px; height: 15px;"></td>
                                                        <td><asp:Literal runat="server" ID="litDispensado" Text="<%$ Resources:Mensagens, MSG_ALUNO_DISPENSADO %>"></asp:Literal>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="border-style: solid; border-width: thin; width: 25px; height: 15px;"></td>
                                                        <td><asp:Literal runat="server" ID="litInativo" Text="<%$ Resources:Mensagens, MSG_ALUNO_INATIVO %>"></asp:Literal>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div id="divUsuarioAlteracaoMedia" runat="server" visible="false">
                                                    <br />
                                                    <asp:Label ID="lblAlteracaoMedia" runat="server" />
                                                    <br />
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </asp:Panel>
                                </div>
                                <div id="divTabsListao-2">
                                    <asp:Panel ID="pnlPlanoAula" runat="server" Visible="false">
                                        <asp:UpdatePanel ID="upnPlanoAula" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblDadoPlanoAula" runat="server" Visible="false"></asp:Label>
                                                <asp:Repeater ID="rptPlanoAula" runat="server" OnItemDataBound="rptPlanoAula_ItemDataBound">
                                                    <HeaderTemplate>
                                                        <div>
                                                            <table cellspacing="0" class="grid grid-responsive-list">
                                                                <thead>
                                                                    <tr class="gridHeader">
                                                                        <th class="center">
                                                                            <asp:Label ID="lblDataAula" runat="server" Text='Data da aula'></asp:Label>
                                                                        </th>
                                                                        <th class="center">
                                                                            <asp:Label ID="lblQtdeAula" runat="server" Text='Quant. de aulas'></asp:Label>
                                                                        </th>
                                                                        <th id="thComponenteCurricular" runat="server">
                                                                            <asp:Label ID="lblComponenteCurricular" runat="server" Text='<%$ Resources:Mensagens, MSG_DISCIPLINA %>'></asp:Label>
                                                                        </th>
                                                                        <th class="center">
                                                                            <asp:Label ID="lblPlanoAula" runat="server" Text='<%$ Resources:Mensagens, MSG_PLANODEAULA %>'></asp:Label>
                                                                        </th>
                                                                        <th class="center"></th>
                                                                        <th class="center">
                                                                            <asp:Label ID="lblResumo" runat="server" Text='<%$ Resources:Mensagens, MSG_SINTESEDAAULA %>'></asp:Label>
                                                                        </th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                    </HeaderTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr class="gridRow">
                                                            <td class="center grid-responsive-no-header" style="text-align: center;">
                                                               
                                                            </td>
                                                            <td class="center grid-responsive-no-header" style="text-align: center;">
                                                               
                                                            </td>
                                                            <td id="tdComponenteCurricularLayout" runat="server" style="text-align: center;" class="grid-responsive-no-header">
                                                                
                                                            </td>
                                                            <td class="center grid-responsive-no-header" style="text-align: center;">
                                                                <asp:Button ID="btnTrocaPlano" runat="server" Text=" V "/>
                                                            </td>
                                                            <td class="grid-responsive-no-header">
                                                              
                                                            </td>
                                                            <td class="center grid-responsive-no-header" style="text-align: center;">
                                                                <asp:Button ID="btnTrocaResumo" runat="server" Text=" V "/>
                                                            </td>
                                                        </tr>
                                                        <tr class="gridAlternatingRow tdSemBordaInferior">
                                                            <td class="center" style="text-align: center;">
                                                                <asp:Literal ID="litTud_id" runat="server" Text='<%#Bind("tud_id") %>' Visible="false"></asp:Literal>
                                                                <asp:Literal ID="litTau_id" runat="server" Text='<%#Bind("tau_id") %>' Visible="false"></asp:Literal>
                                                                <asp:Literal ID="litTud_idFilho" runat="server" Text='<%#Bind("tud_idFilho") %>' Visible="false"></asp:Literal>
                                                                <asp:Label ID="lblData" runat="server" Text='<%#Bind("data") %>'></asp:Label>
                                                                <asp:HiddenField ID="hdnPermissaoAlteracao" runat="server" Value='<%#Bind("permissaoAlteracao") %>' />
                                                            </td>
                                                            <td class="center" style="text-align: center;">
                                                                <asp:Label ID="lblNumeroAulas" runat="server" Text='<%#Bind("numeroAulas") %>'></asp:Label>
                                                            </td>
                                                            <td id="tdComponenteCurricular" runat="server">
                                                                <div class="tdSemBordaInferior">
                                                                    <asp:CheckBoxList ID="chlComponenteCurricular" runat="server" DataTextField="tud_nome"
                                                                        DataValueField="tur_tud_id" CssClass="checkBoxListVertical">
                                                                    </asp:CheckBoxList>
                                                                </div>
                                                            </td>
                                                            <td class="center" style="text-align: center;">
                                                                <asp:HiddenField ID="hdfSemPlanoAula" runat="server" Value='<%#Bind("semPlanoAula") %>' />
                                                                <asp:Image ID="imgSemPlanoAula" runat="server" Visible="false" SkinID="imgStatusAlertaAulaSemPlano" Width="16px" Height="16px" ImageAlign="Top" />
                                                                <asp:TextBox ID="txtPlanoAula" runat="server" Text='<%#Bind("planoAula") %>' TextMode="MultiLine" SkinID="limite4000" Width="90%"></asp:TextBox>
                                                            </td>
                                                            <td class="center grid-responsive-no-header" style="text-align: center;">
                                                                <asp:Button ID="btnTrocaPlanoResumo" runat="server" Text=" > " />
                                                            </td>
                                                            <td class="center" style="text-align: center;">
                                                                <asp:TextBox ID="txtSinteseAula" runat="server" Text='<%#Bind("sintese") %>' TextMode="MultiLine" SkinID="limite4000" Width="90%"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="gridAlternatingRow">
                                                        </tr>
                                                    </AlternatingItemTemplate>
                                                    <ItemTemplate>
                                                        <tr class="gridAlternatingRow">
                                                            <td class="center grid-responsive-no-header" style="text-align: center;">
                                                               
                                                            </td>
                                                            <td class="center grid-responsive-no-header" style="text-align: center;">
                                                               
                                                            </td>
                                                            <td id="tdComponenteCurricularLayout" runat="server" style="text-align: center;" class="grid-responsive-no-header">
                                                                
                                                            </td>
                                                            <td class="center grid-responsive-no-header" style="text-align: center;">
                                                                <asp:Button ID="btnTrocaPlano" runat="server" Text=" V "/>
                                                            </td>
                                                            <td class="grid-responsive-no-header">
                                                              
                                                            </td>
                                                            <td class="center grid-responsive-no-header" style="text-align: center;">
                                                                <asp:Button ID="btnTrocaResumo" runat="server" Text=" V "/>
                                                            </td>
                                                        </tr>
                                                        <tr class="gridRow tdSemBordaInferior">
                                                            <td class="center" style="text-align: center;">
                                                                <asp:Literal ID="litTud_id" runat="server" Text='<%#Bind("tud_id") %>' Visible="false"></asp:Literal>
                                                                <asp:Literal ID="litTau_id" runat="server" Text='<%#Bind("tau_id") %>' Visible="false"></asp:Literal>
                                                                <asp:Literal ID="litTud_idFilho" runat="server" Text='<%#Bind("tud_idFilho") %>' Visible="false"></asp:Literal>
                                                                <asp:Label ID="lblData" runat="server" Text='<%#Bind("data") %>'></asp:Label>
                                                                <asp:HiddenField ID="hdnPermissaoAlteracao" runat="server" Value='<%#Bind("permissaoAlteracao") %>' />
                                                            </td>
                                                            <td class="center" style="text-align: center;">
                                                                <asp:Label ID="lblNumeroAulas" runat="server" Text='<%#Bind("numeroAulas") %>'></asp:Label>
                                                            </td>
                                                            <td id="tdComponenteCurricular" runat="server">
                                                                <div class="tdSemBordaInferior">
                                                                    <asp:CheckBoxList ID="chlComponenteCurricular" runat="server" DataTextField="tud_nome"
                                                                        DataValueField="tur_tud_id" CssClass="checkBoxListVertical">
                                                                    </asp:CheckBoxList>
                                                                </div>
                                                            </td>
                                                            <td class="center" style="text-align: center;">
                                                                <asp:HiddenField ID="hdfSemPlanoAula" runat="server" Value='<%#Bind("semPlanoAula") %>' />
                                                                <asp:Image ID="imgSemPlanoAula" runat="server" Visible="false" SkinID="imgAviso" Width="16px" Height="16px" ImageAlign="Top" />
                                                                <asp:TextBox ID="txtPlanoAula" runat="server" Text='<%#Bind("planoAula") %>' TextMode="MultiLine" SkinID="limite4000" Width="90%"></asp:TextBox>
                                                            </td>
                                                            <td class="center grid-responsive-no-header" style="text-align: center;">
                                                                <asp:Button ID="btnTrocaPlanoResumo" runat="server" Text=" > " />
                                                            </td>
                                                            <td class="center" style="text-align: center;">
                                                                <asp:TextBox ID="txtSinteseAula" runat="server" Text='<%#Bind("sintese") %>' TextMode="MultiLine" SkinID="limite4000" Width="90%"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="gridRow">
                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        </tbody>
                                                        </table></div>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                                <div id="divAvisoAulaSemPlano" runat="server" style="float: left; width: 100%;" visible="false">
                                                    <br />
                                                    <asp:Image ID="imgLegendaAvisoAulaSemPlano" runat="server" SkinID="imgAviso"
                                                        ToolTip="<%$ Resources:Academico, ControleTurma.DiarioClasse.MensagemAulaSemPlanoAula %>" Width="18px" Height="18px" ImageAlign="AbsMiddle" />
                                                    <asp:Literal ID="lit3" runat="server" Text="<%$ Resources:Academico, ControleTurma.DiarioClasse.MensagemAulaSemPlanoAula %>"></asp:Literal>
                                                    <br />
                                                    <br />
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </asp:Panel>
                                </div>
                                <div id="divTabsListao-3">
                                    <asp:Panel ID="pnlAtividadesExtraClasse" runat="server">
                                        
                                                <uc13:UCConfirmacaoOperacao ID="UCConfirmacaoOperacao" runat="server" ObservacaoVisivel="false" ObservacaoObrigatorio="false" />
                                        <asp:UpdatePanel ID="updAtiExtra" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <fieldset id="fdsCadastroAtiExtra" runat="server">
                                                    <asp:HiddenField ID="hdnTaeId" runat="server" />
                                                    <uc12:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" />
                                                    <uc11:UCComboTipoAtividadeAvaliativa ID="UCComboTipoAtividadeAvaliativa" runat="server" Obrigatorio="true" ValidationGroup="AtividadeExtraclasse" />
                                                    <asp:Label ID="lblNomeAtiExtra" runat="server" Text="Nome da atividade extraclasse *" AssociatedControlID="txtNomeAtiExtra"></asp:Label>
                                                    <asp:TextBox ID="txtNomeAtiExtra" runat="server" SkinID="text60C" MaxLength="100"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvNomeAtiExtra" runat="server" ControlToValidate="txtNomeAtiExtra" Display="Dynamic"
                                                        ErrorMessage="Nome da atividade extraclasse é obrigatório." ValidationGroup="AtividadeExtraclasse">*</asp:RequiredFieldValidator>
                                                    <asp:Label ID="lblDescricaoAtiExtra" runat="server" Text="Descrição da atividade extraclasse" AssociatedControlID="txtDescricaoAtiExtra"></asp:Label>
                                                    <asp:TextBox ID="txtDescricaoAtiExtra" runat="server" TextMode="MultiLine" SkinID="limite2000"></asp:TextBox>
                                                    <asp:Label ID="lblCargaAtiExtra" runat="server" Text="Carga horária da atividade extraclasse *" AssociatedControlID="txtCargaAtiExtra"></asp:Label>
                                                    <asp:TextBox ID="txtCargaAtiExtra" runat="server" SkinID="Numerico" MaxLength="4"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvCargaAtiExtra" runat="server" ControlToValidate="txtCargaAtiExtra" Display="Dynamic"
                                                        ErrorMessage="Carga horária da atividade extraclasse é obrigatório." ValidationGroup="AtividadeExtraclasse">*</asp:RequiredFieldValidator>
                                                    <div class="right">
                                                        <asp:Button ID="btnAdicionarAtiExtra" runat="server" Text="Salvar atividade extraclasse" OnClick="btnAdicionarAtiExtra_Click" ValidationGroup="AtividadeExtraclasse" />
                                                        <asp:Button ID="btnLimparCamposAtiExtra" runat="server" Text="Limpar cadastro de atividade extraclasse" OnClick="btnLimparCamposAtiExtra_Click" CausesValidation="false" />
                                                    </div>
                                                </fieldset>
                                                <fieldset>
                                                    <asp:Label ID="lblSemAtividadeExtra" runat="server"></asp:Label>
                                                    <uc2:UCComboOrdenacao ID="UCComboOrdenacaoAtivExtra" runat="server" />
                                                    <asp:Repeater ID="rptAlunoAtivExtra" runat="server" OnItemDataBound="rptAlunoAtivExtra_ItemDataBound">
                                                        <HeaderTemplate>
                                                            <div>
                                                                <table id="tabela" class="grid tbLancamentoAvaliacoes sortableAtividadeExtra grid-responsive-list" cellspacing="0">
                                                                    <thead>
                                                                        <tr class="gridHeader" style="height: 30px;">
                                                                            <th class="center">
                                                                                <asp:Label ID="lblNumChamada" runat="server" Text='Nº Chamada'></asp:Label>
                                                                            </th>
                                                                            <th>
                                                                                <asp:Label ID="lblNome" runat="server" Text='Nome do aluno'></asp:Label>
                                                                            </th>
                                                                            <asp:Repeater ID="rptAtividades" runat="server" OnItemDataBound="rptAtividadesExtraClasseHeader_ItemDataBound">
                                                                                <ItemTemplate>
                                                                                    <th class="center {sorter :false}" style="border-left: 0.1em dotted #FFFFFF; padding-right: 3px;">
                                                                                        <asp:Label ID="lbltae_id" runat="server" Text='<%#Bind("tae_id") %>' Visible="false"></asp:Label>
                                                                                        <asp:Label ID="lbltud_id" runat="server" Text='<%#Bind("tud_id") %>' Visible="false"></asp:Label>
                                                                                        <asp:Label ID="lblAtividade" runat="server" Text='<%#Bind("nome") %>'></asp:Label>
                                                                                        <div style="display: block; margin-bottom: 5px;">
                                                                                            <asp:ImageButton ID="btnEditarAtiExtra" runat="server" SkinID="btEditar" 
                                                                                                ToolTip="Editar atividade extraclasse" OnClick="btnEditarAtiExtra_Click"
                                                                                                CausesValidation="false" />
                                                                                            <asp:ImageButton ID="btnExcluirAtiExtra" runat="server" SkinID="btExcluir"
                                                                                                ToolTip="Excluir atividade extraclasse" OnClick="btnExcluirAtiExtra_Click" />
                                                                                        </div>
                                                                                    </th>
                                                                                </ItemTemplate>
                                                                            </asp:Repeater>
                                                                        </tr>
                                                                    </thead>
                                                                    <tbody>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <tr class="gridRow">
                                                                <td runat="server" id="tdNumChamadaAvaliacao" class="center" style="text-align: center;">
                                                                    <asp:Label ID="lblalu_id" runat="server" Text='<%#Bind("alu_id") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblmtu_id" runat="server" Text='<%#Bind("mtu_id") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblmtd_id" runat="server" Text='<%#Bind("mtd_id") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblava_id" runat="server" Text='<%#Bind("ava_id") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblAtividade" runat="server" Text='<%#Bind("numeroChamada") %>'></asp:Label>
                                                                </td>
                                                                <td runat="server" id="tdNomeAvaliacao">
                                                                    <asp:Label ID="lblNome" runat="server" Text='<%#Bind("pes_nome") %>'></asp:Label>
                                                                    <asp:Label ID="lblNomeOficial" runat="server" Text='<%#Bind("pes_nome") %>' Visible="false">
                                                                    </asp:Label>
                                                                </td>
                                                                <asp:Repeater ID="rptAtividades" runat="server" OnItemDataBound="rptAtividadesExtraClasse_ItemDataBound">
                                                                    <ItemTemplate>
                                                                        <td runat="server" id="tdAtividadesAtivAva" class="center grid-responsive-item-inline grid-responsive-center" style="text-align: center;">
                                                                            <div id="divAtividades" runat="server" style="display: inline-block; width: 100%;">
                                                                                <asp:Label ID="lbltae_id" runat="server" Text='<%#Bind("tae_id") %>' Visible="false"></asp:Label>
                                                                                <asp:Label ID="lbltud_id" runat="server" Text='<%#Bind("tud_id") %>' Visible="false"></asp:Label>
                                                                                <asp:TextBox ID="txtNota" runat="server" SkinID="Decimal" Width="50" MaxLength="6"></asp:TextBox>
                                                                                <asp:DropDownList ID="ddlPareceres" runat="server" DataTextField="descricao" DataValueField="eap_valor">
                                                                                </asp:DropDownList>
                                                                                <asp:ImageButton ID="btnRelatorio" runat="server" SkinID="btDetalhar" OnClick="btnRelatorioAtiExtra_Click"
                                                                                    ToolTip="Lançar relatório" />
                                                                                <asp:Image ID="imgSituacao" runat="server" SkinID="imgConfirmar" ToolTip="Relatório lançado"
                                                                                    Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                                                                                <asp:CheckBox ID="chkEntregou" runat="server" Text="Entregue" Style="display: inline-block;" /><br class="responsive-hide" />
                                                                            </div>
                                                                        </td>
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            </tbody>
                                                    </table></div>
                                                        </FooterTemplate>
                                                    </asp:Repeater>
                                                </fieldset>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </asp:Panel>
                                </div>
                            </div>
                            <div class="right divBtnCadastro area-botoes-bottom">
                                <asp:HiddenField runat="server" ID="hdbtnCompensacaoAusenciaVisible" Value="True" />
                                <asp:Button ID="btnCompensacaoAusencia" runat="server" Text="Incluir nova compensação de ausência"
                                    OnClick="btnCompensacaoAusencia_Click" />
                                <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
                                    ValidationGroup="Frequencia" />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </div>
    </fieldset>
    <input id="txtSelectedTab" type="hidden" runat="server" />

    <div id="divHabilidadesRelacionadas" title="Habilidades Relacionadas" class="hide">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <fieldset id="fsdHabilidadesRelacionadas" runat="server">
                    
                        <legend>Habilidades relacionadas</legend>
                        <div></div>
                        <uc4:UCHabilidades runat="server" ID="UCHabilidades" TituloFildSet="Expectativa de aprendizagem" LegendaCheck="Não alcançada" bHabilidaEdicao="True" />
                    

                    <div class="right">
                        <asp:Button ID="btnSalvarHabilidadesRelacionadas" runat="server" Text="Salvar" OnClick="btnSalvarHabilidadesRelacionadas_Click" />
                        <asp:Button ID="btnCancelarHabilidadesRelacionadas" runat="server" Text="Cancelar" CausesValidation="false"
                            OnClientClick="$('#divHabilidadesRelacionadas').dialog('close');" />
                    </div>
                </fieldset>
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

    <div id="divCompensacao" title="Compensação de ausência" class="hide">
        <asp:UpdatePanel ID="upnCompensacao" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset id="fdsResultados" runat="server" visible="false">
                    <uc9:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
                    <asp:GridView ID="gvCompAusencia" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                        EmptyDataText="A pesquisa não encontrou resultados." OnDataBound="gvCompAusencia_DataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Atividades desenvolvidas" SortExpression="atividadesDesenv">
                                <ItemTemplate>
                                    <asp:Label ID="lblAtividadesDesenv" runat="server" Text='<%# Bind("cpa_atividadesDesenvolvidas") %>'
                                        ToolTip='<%# Bind("cpa_atividadesDesenvolvidas") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="cpa_quantidadeAulasCompensadas" HeaderText="Quant. de aulas compensadas">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="tpc_nome" HeaderText="">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource ID="odsCompAusencia" runat="server" OldValuesParameterFormatString="original_{0}"
                        SelectMethod="SelecionaPorAluno" TypeName="MSTech.GestaoEscolar.BLL.CLS_CompensacaoAusenciaBO"></asp:ObjectDataSource>
                    <uc8:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="gvCompAusencia" />
                </fieldset>
                <div class="right">
                    <asp:Button ID="btnFecharConsultaCompensacao" runat="server" Text="Voltar" OnClientClick="$('#divCompensacao').dialog('close'); return false;" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <%-- Disciplinas compartilhadas --%>
    <uc10:UCSelecaoDisciplinaCompartilhada ID="UCSelecaoDisciplinaCompartilhada1" runat="server"></uc10:UCSelecaoDisciplinaCompartilhada>
    <asp:HiddenField ID="hdnValorTurmas" runat="server" />

</asp:Content>
