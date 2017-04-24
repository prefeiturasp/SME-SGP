<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="MinhaEscolaGestor.aspx.cs" Inherits="GestaoEscolar.Academico.ControleTurma.MinhaEscolaGestor" %>

<%@ Register Src="~/WebControls/Combos/Novos/UCCCursoCurriculo.ascx" TagName="UCCCursoCurriculo" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCurriculoPeriodo.ascx" TagName="UCCCurriculoPeriodo" TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoDisciplina.ascx" TagName="UCComboTipoDisciplina" TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="UCComboUAEscola" TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCComboGenerico.ascx" TagName="UCComboGenerico" TagPrefix="uc5" %>
<%@ Register Src="~/WebControls/Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc6" %>
<%@ Register Src="~/WebControls/ControleTurma/UCSelecaoDisciplinaCompartilhada.ascx" TagName="UCSelecaoDisciplinaCompartilhada" TagPrefix="uc10" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="upnMensagem" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Label ID="lblMensagem" runat="server" EnableViewState="false"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="vsMinhasTurmas" />
    <asp:UpdatePanel ID="upnResultado" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc6:UCLoader ID="UCLoader1" runat="server" AssociatedUpdatePanelID="upnResultado" />
            <div id="divFiltros" runat="server">
                <fieldset>
                    <legend>Minha escola</legend>
                    <uc4:UCComboUAEscola ID="UCComboUAEscola1" runat="server" FiltroEscolasControladas="true"
                        ValidationGroup="vsMinhasTurmas" ObrigatorioUA="true" ObrigatorioEscola="true"
                        MostrarMessageSelecioneEscola="true" MostrarMessageSelecioneUA="true" />
                    <div class="right">
                        <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click"
                            ValidationGroup="vsMinhasTurmas" />
                        <asp:Button ID="btnLimparPesquisa" runat="server" Text="Limpar pesquisa" OnClick="btnLimparPesquisa_Click"
                            CausesValidation="false" />
                    </div>
                    <br />
                </fieldset>
            </div>
            <asp:Repeater ID="rptTurmas" runat="server" OnItemDataBound="rptTurmas_ItemDataBound">
                <ItemTemplate>
                    <asp:HiddenField ID="hdnUadSuperior" runat="server" Value='<%# Eval("uad_idSuperior") %>' />
                    <asp:HiddenField ID="hdnEscola" runat="server" Value='<%# Eval("esc_id") %>' />
                    <asp:HiddenField ID="hdnUnidadeEscola" runat="server" Value='<%# Eval("uni_id") %>' />
                    <asp:HiddenField ID="hdnCalendario" runat="server" Value='<%# Eval("cal_id") %>' />
                    <asp:HiddenField ID="hdnCalendarioAno" runat="server" Value='<%# Eval("cal_ano") %>' />
                    <fieldset class="fdsTurmas">
                        <legend class="legendMinhasTurmas" runat="server" id="legMinhaEscola">
                            <asp:Label runat="server" ID="txtLegendMinhasTurmas" Text='<%#Eval("lengendTitulo") %>'
                                ForeColor='<%# ((Convert.ToInt32(Eval("cal_ano").ToString()) < DateTime.Now.Year && Convert.ToBoolean(Eval("turmasAnoAtual").ToString()) == true) ? System.Drawing.ColorTranslator.FromHtml("#A52A2A") : System.Drawing.Color.Black) %>'></asp:Label>
                            <%--<asp:Label runat="server" ID="lblLegendTitulo" Text="<%#Eval("lengendTitulo") %>"
                                ForeColor='<%# ((Convert.ToInt32(Eval("cal_ano").ToString()) < DateTime.Now.Year && Convert.ToBoolean(Eval("turmasAnoAtual").ToString()) == true) ? System.Drawing.ColorTranslator.FromHtml("#A52A2A") : System.Drawing.Color.Black) %>'></asp:Label>--%>
                            <asp:Label runat="server" ID="lblDataProcessamento" class="dataProcessamentoPendencia"></asp:Label>
                        </legend>
                        <div runat="server" id="divMessageTurmaAnterior" visible='<%# ((Convert.ToInt32(Eval("cal_ano").ToString()) < DateTime.Now.Year) && (Convert.ToBoolean(Eval("turmasAnoAtual").ToString()) == true)) %>'
                            class="summaryMsgAnosAnteriores" style="<%$ Resources: Academico, ControleTurma.Busca.divMessageTurmaAnterior.Style %>">
                            <asp:Label runat="server" ID="lblMessageTurmaAnterior" Text="<%$ Resources:Academico, ControleTurma.Busca.lblMessageTurmaAnterior.Text %>"
                                Style="<%$ Resources: Academico, ControleTurma.Busca.lblMessageTurmaAnterior.Style %>"></asp:Label>
                        </div>
                        <div id="mensagemPendenciaFechamentoAbas" class="mensagemSemPendenciaFechamento" runat="server" visible="false">
                            <asp:Literal ID="lblMensagemPendenciaFechamento" runat="server"></asp:Literal>
                        </div>
                        <input id="txtSelectedTab" type="hidden" runat="server" />
                        <div id="divTabs" style="clear:both; padding-top:10px;">
                            <ul class="hide">
                                <asp:Repeater ID="rptCiclos" runat="server">
                                    <ItemTemplate>
                                        <li><a href='#<%# RetornaTabID((int)Eval("tci_id"))%>'><%# Eval("tci_nome") %></a></li>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <li id="liTurmasEx"><a href="#divTabsTurmasEx">
                                    <asp:Label ID="lbl1" runat="server"
                                        Text="<%$ Resources:Academico, ControleTurma.BuscaMinhaEscola.lblTabsTurmasEx.text %>" />
                                </a></li>
                                <li id="liProjetos" runat="server"><a href="#divTabsProjetos">
                                    <asp:Literal ID="litProjetos" runat="server" Text="<%$ Resources:Academico, ControleTurma.MinhaEscolaGestor.litProjetos.Text %>"></asp:Literal>
                                </a></li>
                            </ul>
                            <asp:Repeater ID="rptCiclosAbas" runat="server" OnItemDataBound="rptCiclosAbas_ItemDataBound">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnCiclo" runat="server" Value='<%# Eval("tci_id") %>' />
                                    <asp:HiddenField ID="hdnNomeCiclo" runat="server" Value='<%# Eval("tci_nome") %>' />
                                    <div id='<%# RetornaTabID((int)Eval("tci_id"))%>'>
                                        <fieldset id="fsAba" runat="server" style="font-size: 0.9em;">
                                            <legend class="legendMinhasTurmas" runat="server" id="legMinhaEscola">
                                                <asp:Label runat="server" ID="lblDataProcessamento" class="dataProcessamentoPendencia"></asp:Label>
                                            </legend>
                                            <uc1:UCCCursoCurriculo ID="UCCCursoCurriculo1" runat="server" />
                                            <uc2:UCCCurriculoPeriodo ID="UCCCurriculoPeriodo1" runat="server" />
                                            <uc3:UCComboTipoDisciplina ID="UCComboTipoDisciplina1" runat="server" />
                                            <br />
                                            <br />
                                            <div class="clear"></div>
                                            <div id="mensagemPendenciaFechamentoMinhaEscolaGestor" class="mensagemPendenciaFechamentoMinhaEscolaGestor" runat="server" visible="false">
                                                <asp:LinkButton ID="lkbMensagemPendenciaFechamento" runat="server" Text="<%$ Resources:Academico, ControleTurma.MinhaEscolaGestor.lkbMensagemPendenciaFechamento.Text %>" OnClick="lkbMensagemPendenciaFechamentoTurma_Click"></asp:LinkButton>
                                            </div>
                                            <div id="mensagemSemPendenciaFechamento" class="mensagemSemPendenciaFechamento" runat="server" visible="false">
                                                <asp:Literal ID="litMensagemSemPendenciaFechamento" runat="server" Text="<%$ Resources:Academico, ControleTurma.MinhaEscolaGestor.litMensagemSemPendenciaFechamento.Text %>"></asp:Literal>
                                            </div>
                                            <asp:GridView ID="grvTurma" runat="server" AutoGenerateColumns="false"
                                                DataKeyNames="tud_id,esc_id,uni_id,tur_id,tud_naoLancarNota,tud_naoLancarFrequencia,cal_id,EscolaTurmaDisciplina,tur_dataEncerramento,tciIds,tud_tipo,tur_tipo,tud_idAluno,tur_idNormal,fav_id"
                                                OnRowCommand="grvMinhasTurmas_RowCommand" EmptyDataText="A pesquisa não encontrou resultados." OnDataBound="grvTurmas_DataBound" OnRowDataBound="grvTurma_RowDataBound" SkinID="GridResponsive">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Turma">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTurma" runat="server"
                                                                Text='<%# Eval("tur_codigo") + " - " + Eval("tud_nome") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--Aulas Dadas--%>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Mensagens, MSG_AulasDadas %>" HeaderStyle-CssClass="center"
                                                        ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-responsive-item-inline grid-responsive-center">
                                                        <ItemTemplate>
                                                            <span class="ico-font ico-aulas-dadas"><asp:ImageButton ID="btnIndicadores"
                                                                CommandArgument='<%# Eval("esc_id") + "," + Eval("tur_id") + "," 
                                                                        + Eval("tud_id") + "," + Eval("cal_id") + "," + Eval("tdt_posicao") %>'
                                                                runat="server" SkinID="btIndicadores" CommandName="Indicadores"
                                                                ToolTip="Visualizar indicadores de aulas da turma"
                                                                Visible='<%# Convert.ToByte(Eval("tud_tipo")) != (byte)MSTech.GestaoEscolar.BLL.ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada 
                                                                            && Convert.ToByte(Eval("tud_tipo")) != (byte)MSTech.GestaoEscolar.BLL.ACA_CurriculoDisciplinaTipo.Experiencia %>' /></span>
                                                            <asp:Image ID="imgSituacaoAulasDadas" runat="server" SkinID="imgConfirmar" ToolTip="Aulas lançadas"
                                                                Width="16px" Height="16px" Visible='<%# Convert.ToByte(Eval("tud_tipo")) != (byte)MSTech.GestaoEscolar.BLL.ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada
                                                                                        && Convert.ToByte(Eval("tud_tipo")) != (byte)MSTech.GestaoEscolar.BLL.ACA_CurriculoDisciplinaTipo.Experiencia
                                                                                        && Convert.ToBoolean(Eval("aulasPrevistasPreenchida")) %>'
                                                                ImageAlign="Top" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--Planejamento--%>
                                                    <asp:TemplateField HeaderText="Planejamento" HeaderStyle-CssClass="center"
                                                        ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-responsive-item-inline grid-responsive-center">
                                                        <ItemTemplate>
                                                            <span class="ico-font ico-planejamento"><asp:ImageButton ID="btnPlanejamento"
                                                                    CommandArgument='<%# Container.DataItemIndex %>'
                                                                    runat="server" SkinID="btPlanejamentoGestor" CommandName="Planejamento"
                                                                    ToolTip="Planejamento" /></span>
                                                            <asp:Image ID="imgPendenciaPlanejamento" runat="server" SkinID="imgStatusAlertaPendencia" Width="16px" Height="16px" ImageAlign="Top"
                                                                Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--Diário de Classe--%>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Mensagens, MSG_DiarioClasse %>" HeaderStyle-CssClass="center"
                                                        ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-responsive-item-inline grid-responsive-center">
                                                        <ItemTemplate>
                                                            <span class="ico-font ico-diario-classe"><asp:ImageButton ID="btnDiarioClasse"
                                                                    CommandArgument='<%# Container.DataItemIndex %>'
                                                                    runat="server" SkinID="btDiarioGestor" CommandName="DiarioClasse"
                                                                    ToolTip="<%$ Resources:Mensagens, MSG_DiarioClasse %>" /></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--Listão--%>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Mensagens, MSG_Listao %>" HeaderStyle-CssClass="center"
                                                        ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-responsive-item-inline grid-responsive-center">
                                                        <ItemTemplate>
                                                            <span class="ico-font ico-listao"><asp:ImageButton ID="btnListao"
                                                                    CommandArgument='<%# Container.DataItemIndex %>'
                                                                    runat="server" SkinID="btListaoGestor" CommandName="Listao"
                                                                    ToolTip="<%$ Resources:Mensagens, MSG_Listao %>" /></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--Frequência --%>
                                                    <asp:TemplateField HeaderText="<%$ Resources:WebControls, UCNavegacaoTelaPeriodo.btnFrequencia.Text %>" HeaderStyle-CssClass="center"
                                                        ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-responsive-item-inline grid-responsive-center">
                                                        <ItemTemplate>
                                                            <span class="ico-font ico-frequencia"><asp:ImageButton ID="btnFrequencia"
                                                                    CommandArgument='<%# Container.DataItemIndex %>'
                                                                    runat="server" SkinID="btFrequenciaGestor" CommandName="Frequencia"
                                                                    ToolTip="<%$ Resources:WebControls, UCNavegacaoTelaPeriodo.btnFrequencia.Text %>" /></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--Avaliação --%>
                                                    <asp:TemplateField HeaderText="<%$ Resources:WebControls, UCNavegacaoTelaPeriodo.btnAvaliacao.Text %>" HeaderStyle-CssClass="center"
                                                        ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-responsive-item-inline grid-responsive-center">
                                                        <ItemTemplate>
                                                            <span class="ico-font ico-avaliacao"><asp:ImageButton ID="btnAvaliacao"
                                                                    CommandArgument='<%# Container.DataItemIndex %>'
                                                                    runat="server" SkinID="btAvaliacaoGestor" CommandName="Avaliacao"
                                                                    ToolTip="<%$ Resources:WebControls, UCNavegacaoTelaPeriodo.btnAvaliacao.Text %>" /></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--Fechamento--%>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Mensagens, MSG_EFETIVACAO %>" HeaderStyle-CssClass="center"
                                                        ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-responsive-item-inline grid-responsive-center">
                                                        <ItemTemplate>
                                                            <asp:HiddenField ID="hdnDadosFechamento" runat="server" />
                                                            <span class="ico-font ico-fechamento"><asp:ImageButton ID="btnFechamento"
                                                                    CommandArgument='<%# Container.DataItemIndex %>'
                                                                    runat="server" SkinID="btFechamentoGestor" CommandName="Fechamento"
                                                                    ToolTip="<%$ Resources:Mensagens, MSG_EFETIVACAO %>"
                                                                    Visible='<%# Convert.ToByte(Eval("tud_tipo")) != (byte)MSTech.GestaoEscolar.BLL.ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada %>' /></span> 
                                                           <asp:Image ID="imgPendenciaFechamento" runat="server" SkinID="imgStatusAlertaPendencia" Width="16px" Height="16px" ImageAlign="Top"
                                                                Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--Alunos--%>
                                                    <asp:TemplateField HeaderText="Alunos" HeaderStyle-CssClass="center"
                                                        ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-responsive-item-inline grid-responsive-center">
                                                        <ItemTemplate>
                                                            <span class="ico-font ico-alunos"><asp:ImageButton ID="btnAlunos"
                                                                    CommandArgument='<%# Container.DataItemIndex %>'
                                                                    runat="server" SkinID="btAlunoGestor" CommandName="Alunos"
                                                                    ToolTip="Alunos" /></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </fieldset>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <div id="divTabsTurmasEx">
                                <asp:Panel ID="fdsTurmasExtintas" runat="server">
                                    <fieldset id="fsTurExtintas" runat="server" style="font-size: 0.9em;">
                                        <legend class="legendMinhasTurmas" runat="server" id="legMinhaEscolaExtintas">
                                            <asp:Label runat="server" ID="lblDataProcessamentoExtintas" class="dataProcessamentoPendencia"></asp:Label>
                                        </legend>
                                        <div class="clear"></div>
                                        <div id="mensagemPendenciaFechamentoMinhaEscolaGestorExtintas" class="mensagemPendenciaFechamentoMinhaEscolaGestor" runat="server" visible="false">
                                            <asp:LinkButton ID="lkbMensagemPendenciaFechamentoExtintas" runat="server" Text="<%$ Resources:Academico, ControleTurma.MinhaEscolaGestor.lkbMensagemPendenciaFechamento.Text %>" OnClick="lkbMensagemPendenciaFechamentoExtintas_Click"></asp:LinkButton>
                                        </div>
                                        <div id="mensagemSemPendenciaFechamentoExtintas" class="mensagemSemPendenciaFechamento" runat="server" visible="false">
                                            <asp:Literal ID="litMensagemSemPendenciaFechamentoExtintas" runat="server" Text="<%$ Resources:Academico, ControleTurma.MinhaEscolaGestor.litMensagemSemPendenciaFechamento.Text %>"></asp:Literal>
                                        </div>
                                        <asp:GridView ID="grvTurmasExtintas" runat="server" AutoGenerateColumns="false"
                                            DataKeyNames="tud_id,esc_id,uni_id,tur_id,tud_naoLancarNota,tud_naoLancarFrequencia,cal_id,EscolaTurmaDisciplina,tur_dataEncerramento,tciIds,tud_tipo,tur_tipo,tud_idAluno,tur_idNormal,fav_id"
                                            OnRowCommand="grvTurmasExtintas_RowCommand" OnDataBound="grvTurmas_DataBound" OnRowDataBound="grvTurma_RowDataBound" SkinID="GridResponsive">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Turma" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTurma" runat="server"
                                                            Text='<%# Eval("tur_codigo") + " - " + Eval("tud_nome") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <%--Aulas Dadas--%>
                                                <asp:TemplateField HeaderText="<%$ Resources:Mensagens, MSG_AulasDadas %>" HeaderStyle-CssClass="center"
                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-responsive-item-inline grid-responsive-center">
                                                    <ItemTemplate>
                                                        <span class="ico-font ico-aulas-dadas"><asp:ImageButton ID="btnIndicadores"
                                                                CommandArgument='<%# Eval("esc_id") + "," + Eval("tur_id") + "," 
                                                                        + Eval("tud_id") + "," + Eval("cal_id") + "," + Eval("tdt_posicao") %>'
                                                                runat="server" SkinID="btIndicadores" CommandName="Indicadores"
                                                                ToolTip="Visualizar indicadores de aulas da turma"
                                                                Visible='<%# Convert.ToByte(Eval("tud_tipo")) != (byte)MSTech.GestaoEscolar.BLL.ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada %>' /></span>
                                                        <asp:Image ID="imgSituacaoAulasDadas" runat="server" SkinID="imgConfirmar" ToolTip="Aulas lançadas"
                                                                Width="16px" Height="16px" Visible='<%# Convert.ToByte(Eval("tud_tipo")) != (byte)MSTech.GestaoEscolar.BLL.ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada
                                                                                        && Convert.ToBoolean(Eval("aulasPrevistasPreenchida")) %>'
                                                                ImageAlign="Top" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--Planejamento--%>
                                                <asp:TemplateField HeaderText="Planejamento" HeaderStyle-CssClass="center"
                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-responsive-item-inline grid-responsive-center">
                                                    <ItemTemplate>
                                                        <span class="ico-font ico-planejamento"><asp:ImageButton ID="btnPlanejamento"
                                                                CommandArgument='<%# Container.DataItemIndex %>'
                                                                runat="server" SkinID="btPlanejamentoGestor" CommandName="Planejamento"
                                                                ToolTip="Planejamento" /></span>
                                                        <asp:Image ID="imgPendenciaPlanejamento" runat="server" SkinID="imgStatusAlertaPendencia" Width="16px" Height="16px" ImageAlign="Top"
                                                            Visible="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--Diário de Classe--%>
                                                <asp:TemplateField HeaderText="<%$ Resources:Mensagens, MSG_DiarioClasse %>" HeaderStyle-CssClass="center"
                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-responsive-item-inline grid-responsive-center">
                                                    <ItemTemplate>
                                                        <span class="ico-font ico-diario-classe"><asp:ImageButton ID="btnDiarioClasse"
                                                                CommandArgument='<%# Container.DataItemIndex %>'
                                                                runat="server" SkinID="btDiarioGestor" CommandName="DiarioClasse"
                                                                ToolTip="<%$ Resources:Mensagens, MSG_DiarioClasse %>" /></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--Listão--%>
                                                <asp:TemplateField HeaderText="<%$ Resources:Mensagens, MSG_Listao %>" HeaderStyle-CssClass="center"
                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-responsive-item-inline grid-responsive-center">
                                                    <ItemTemplate>
                                                        <span class="ico-font ico-listao"><asp:ImageButton ID="btnListao"
                                                                CommandArgument='<%# Container.DataItemIndex %>'
                                                                runat="server" SkinID="btListaoGestor" CommandName="Listao"
                                                                ToolTip="<%$ Resources:Mensagens, MSG_Listao %>" /></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--Frequência --%>
                                                <asp:TemplateField HeaderText="<%$ Resources:WebControls, UCNavegacaoTelaPeriodo.btnFrequencia.Text %>" HeaderStyle-CssClass="center"
                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-responsive-item-inline grid-responsive-center">
                                                    <ItemTemplate>
                                                        <span class="ico-font ico-frequencia"><asp:ImageButton ID="btnFrequencia"
                                                                CommandArgument='<%# Container.DataItemIndex %>'
                                                                runat="server" SkinID="btFrequenciaGestor" CommandName="Frequencia"
                                                                ToolTip="<%$ Resources:WebControls, UCNavegacaoTelaPeriodo.btnFrequencia.Text %>" /></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--Avaliação --%>
                                                <asp:TemplateField HeaderText="<%$ Resources:WebControls, UCNavegacaoTelaPeriodo.btnAvaliacao.Text %>" HeaderStyle-CssClass="center"
                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-responsive-item-inline grid-responsive-center">
                                                    <ItemTemplate>
                                                        <span class="ico-font ico-avaliacao"><asp:ImageButton ID="btnAvaliacao"
                                                                CommandArgument='<%# Container.DataItemIndex %>'
                                                                runat="server" SkinID="btAvaliacaoGestor" CommandName="Avaliacao"
                                                                ToolTip="<%$ Resources:WebControls, UCNavegacaoTelaPeriodo.btnAvaliacao.Text %>" /></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--Fechamento--%>
                                                <asp:TemplateField HeaderText="<%$ Resources:Mensagens, MSG_EFETIVACAO %>" HeaderStyle-CssClass="center"
                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-responsive-item-inline grid-responsive-center">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnDadosFechamento" runat="server" />
                                                        <span class="ico-font ico-fechamento"><asp:ImageButton ID="btnFechamento"
                                                                CommandArgument='<%# Container.DataItemIndex %>'
                                                                runat="server" SkinID="btFechamentoGestor" CommandName="Fechamento"
                                                                ToolTip="<%$ Resources:Mensagens, MSG_EFETIVACAO %>"
                                                                Visible='<%# Convert.ToByte(Eval("tud_tipo")) != (byte)MSTech.GestaoEscolar.BLL.ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada %>' /></span>
                                                        <asp:Image ID="imgPendenciaFechamento" runat="server" SkinID="imgStatusAlertaPendencia" Width="16px" Height="16px" ImageAlign="Top"
                                                            Visible="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--Alunos--%>
                                                <asp:TemplateField HeaderText="Alunos" HeaderStyle-CssClass="center"
                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-responsive-item-inline grid-responsive-center">
                                                    <ItemTemplate>
                                                        <span class="ico-font ico-alunos"><asp:ImageButton ID="btnAlunos"
                                                                CommandArgument='<%# Container.DataItemIndex %>'
                                                                runat="server" SkinID="btAlunoGestor" CommandName="Alunos"
                                                                ToolTip="Alunos" /></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </fieldset>
                                </asp:Panel>
                            </div>
                            <div id="divTabsProjetos">
                                <asp:Panel ID="fdsProjetos" runat="server">
                                    <fieldset id="fsProjetos" runat="server" style="font-size: 0.9em;">
                                        <legend class="legendMinhasTurmas" runat="server" id="legMinhaEscolaProjeto">
                                            <asp:Label runat="server" ID="lblDataProcessamentoProjeto" class="dataProcessamentoPendencia"></asp:Label>
                                        </legend>
                                        <div class="clear"></div>
                                        <div id="mensagemPendenciaFechamentoMinhaEscolaGestorProjeto" class="mensagemPendenciaFechamentoMinhaEscolaGestor" runat="server" visible="false">
                                            <asp:LinkButton ID="lkbMensagemPendenciaFechamento" runat="server" Text="<%$ Resources:Academico, ControleTurma.MinhaEscolaGestor.lkbMensagemPendenciaFechamento.Text %>" OnClick="lkbMensagemPendenciaFechamentoProjeto_Click"></asp:LinkButton>
                                        </div>
                                        <div id="mensagemSemPendenciaFechamentoProjeto" class="mensagemSemPendenciaFechamento" runat="server" visible="false">
                                            <asp:Literal ID="litMensagemSemPendenciaFechamentoProjeto" runat="server" Text="<%$ Resources:Academico, ControleTurma.MinhaEscolaGestor.litMensagemSemPendenciaFechamento.Text %>"></asp:Literal>
                                        </div>
                                        <asp:GridView ID="grvProjetosRecParalela" runat="server" AutoGenerateColumns="false"
                                            DataKeyNames="tud_id,esc_id,uni_id,tur_id,tud_naoLancarNota,tud_naoLancarFrequencia,cal_id,EscolaTurmaDisciplina,tur_dataEncerramento,tciIds,tud_tipo,tur_tipo,tud_idAluno,tur_idNormal,fav_id"
                                            OnRowCommand="grvProjetosRecParalela_RowCommand" OnDataBound="grvTurmas_DataBound" OnRowDataBound="grvTurma_RowDataBound" SkinID="GridResponsive">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Turma" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTurma" runat="server"
                                                            Text='<%# Eval("tur_codigo") + " - " + Eval("tud_nome") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Mensagens, MSG_AulasDadas %>" HeaderStyle-CssClass="center"
                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-responsive-item-inline grid-responsive-center">
                                                    <ItemTemplate>
                                                        <span class="ico-font ico-aulas-dadas"><asp:ImageButton ID="btnIndicadores"
                                                                CommandArgument='<%# Eval("esc_id") + "," + Eval("tur_id") + "," 
                                                                        + Eval("tud_id") + "," + Eval("cal_id") + "," + Eval("tdt_posicao") %>'
                                                                runat="server" SkinID="btIndicadores" CommandName="Indicadores"
                                                                ToolTip="Visualizar indicadores de aulas da turma"
                                                                Visible='<%# Convert.ToByte(Eval("tud_tipo")) != (byte)MSTech.GestaoEscolar.BLL.ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada %>' /></span>
                                                        <asp:Image ID="imgSituacaoAulasDadas" runat="server" SkinID="imgConfirmar" ToolTip="Aulas lançadas"
                                                                Width="16px" Height="16px" Visible='<%# Convert.ToByte(Eval("tud_tipo")) != (byte)MSTech.GestaoEscolar.BLL.ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada
                                                                                        && Convert.ToBoolean(Eval("aulasPrevistasPreenchida")) %>'
                                                                ImageAlign="Top" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--Planejamento--%>
                                                <asp:TemplateField HeaderText="Planejamento" HeaderStyle-CssClass="center"
                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-responsive-item-inline grid-responsive-center">
                                                    <ItemTemplate>
                                                        <span class="ico-font ico-planejamento"><asp:ImageButton ID="btnPlanejamento"
                                                                CommandArgument='<%# Container.DataItemIndex %>'
                                                                runat="server" SkinID="btPlanejamentoGestor" CommandName="Planejamento"
                                                                ToolTip="Planejamento" /></span>
                                                        <asp:Image ID="imgPendenciaPlanejamento" runat="server" SkinID="imgStatusAlertaPendencia" Width="16px" Height="16px" ImageAlign="Top"
                                                            Visible="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--Diário de Classe--%>
                                                <asp:TemplateField HeaderText="<%$ Resources:Mensagens, MSG_DiarioClasse %>" HeaderStyle-CssClass="center"
                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-responsive-item-inline grid-responsive-center">
                                                    <ItemTemplate>
                                                        <span class="ico-font ico-diario-classe"><asp:ImageButton ID="btnDiarioClasse"
                                                                CommandArgument='<%# Container.DataItemIndex %>'
                                                                runat="server" SkinID="btDiarioGestor" CommandName="DiarioClasse"
                                                                ToolTip="<%$ Resources:Mensagens, MSG_DiarioClasse %>" /></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--Listão--%>
                                                <asp:TemplateField HeaderText="<%$ Resources:Mensagens, MSG_Listao %>" HeaderStyle-CssClass="center"
                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-responsive-item-inline grid-responsive-center">
                                                    <ItemTemplate>
                                                        <span class="ico-font ico-listao"><asp:ImageButton ID="btnListao"
                                                                CommandArgument='<%# Container.DataItemIndex %>'
                                                                runat="server" SkinID="btListaoGestor" CommandName="Listao"
                                                                ToolTip="<%$ Resources:Mensagens, MSG_Listao %>" /></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--Frequência --%>
                                                <asp:TemplateField HeaderText="<%$ Resources:WebControls, UCNavegacaoTelaPeriodo.btnFrequencia.Text %>" HeaderStyle-CssClass="center"
                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-responsive-item-inline grid-responsive-center">
                                                    <ItemTemplate>
                                                        <span class="ico-font ico-frequencia"><asp:ImageButton ID="btnFrequencia"
                                                                CommandArgument='<%# Container.DataItemIndex %>'
                                                                runat="server" SkinID="btFrequenciaGestor" CommandName="Frequencia"
                                                                ToolTip="<%$ Resources:WebControls, UCNavegacaoTelaPeriodo.btnFrequencia.Text %>" /></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--Avaliação --%>
                                                <asp:TemplateField HeaderText="<%$ Resources:WebControls, UCNavegacaoTelaPeriodo.btnAvaliacao.Text %>" HeaderStyle-CssClass="center"
                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-responsive-item-inline grid-responsive-center">
                                                    <ItemTemplate>
                                                        <span class="ico-font ico-avaliacao"><asp:ImageButton ID="btnAvaliacao"
                                                                CommandArgument='<%# Container.DataItemIndex %>'
                                                                runat="server" SkinID="btAvaliacaoGestor" CommandName="Avaliacao"
                                                                ToolTip="<%$ Resources:WebControls, UCNavegacaoTelaPeriodo.btnAvaliacao.Text %>" /></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--Fechamento--%>
                                                <asp:TemplateField HeaderText="<%$ Resources:Mensagens, MSG_EFETIVACAO %>" HeaderStyle-CssClass="center"
                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-responsive-item-inline grid-responsive-center">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnDadosFechamento" runat="server" />
                                                        <span class="ico-font ico-fechamento"><asp:ImageButton ID="btnFechamento"
                                                            CommandArgument='<%# Container.DataItemIndex %>'
                                                            runat="server" SkinID="btFechamentoGestor" CommandName="Fechamento"
                                                            ToolTip="<%$ Resources:Mensagens, MSG_EFETIVACAO %>"
                                                            Visible='<%# Convert.ToByte(Eval("tud_tipo")) != (byte)MSTech.GestaoEscolar.BLL.ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada %>' /></span>
                                                        <asp:Image ID="imgPendenciaFechamento" runat="server" SkinID="imgStatusAlertaPendencia" Width="16px" Height="16px" ImageAlign="Top"
                                                            Visible="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--Alunos--%>
                                                <asp:TemplateField HeaderText="Alunos" HeaderStyle-CssClass="center"
                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-responsive-item-inline grid-responsive-center">
                                                    <ItemTemplate>
                                                        <span class="ico-font ico-alunos"><asp:ImageButton ID="btnAlunos"
                                                                CommandArgument='<%# Container.DataItemIndex %>'
                                                                runat="server" SkinID="btAlunoGestor" CommandName="Alunos"
                                                                ToolTip="Alunos" /></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </fieldset>
                                </asp:Panel>
                            </div>
                        </div>
                    </fieldset>
                </ItemTemplate>
            </asp:Repeater>
            <asp:HiddenField ID="hdnProcessarFilaFechamentoTela" runat="server" Value="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <%-- Indicadores --%>
    <div id="divIndicadores" runat="server" title="<%$ Resources:Mensagens, MSG_AulasDadas %>" class="hide divIndicadores">
        <asp:UpdatePanel ID="upnIndicadores" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label ID="lblPeriodoEfetivado" runat="server" EnableViewState="false" Visible="false"></asp:Label>
                <asp:Label ID="lblMensagemBloqueio" runat="server" Text=""></asp:Label>
                <asp:Panel ID="pnlIndicadores" runat="server" GroupingText="<%$ Resources:Mensagens, MSG_AulasDadas %>">
                    <asp:Label ID="lblMensagemIndicador" runat="server" Text="" EnableViewState="false"></asp:Label>
                    <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="AulasPrevistas" />
                    <div style="display: inline-block;">
                        <uc5:UCComboGenerico ID="uccTurmaDisciplina" runat="server" MostrarMensagemSelecione="false" Obrigatorio="false"
                            TituloCombo="<%$ Resources:Academico, ControleTurma.Busca.uccTurmaDisciplina.TituloCombo %>"
                            ValorItemVazio="-1;-1;-1;-1" />
                        <asp:HiddenField ID="hdnEscId" runat="server" Value="-1" />
                        <asp:GridView ID="grvPeriodosAulas" runat="server" AutoGenerateColumns="false" Width="740" DataKeyNames="tud_id, tud_tipo, tpc_id, cap_dataInicio, cap_dataFim, cap_descricao, fav_fechamentoAutomatico" ShowFooter="True"
                            OnRowDataBound="grvPeriodosAulas_RowDataBound" SkinID="GridResponsive">
                            <Columns>
                                <asp:BoundField HeaderText="Bimestre" DataField="cap_descricao" FooterText="Total" />
                                <asp:BoundField HeaderText="Período" DataField="periodo" />
                                <asp:TemplateField HeaderText="Previstas *">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtPrevistas" Text='<%# Bind("aulasPrevistas") %>'
                                            SkinID="Numerico" MaxLength="3" Width="30px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rvPrevistas" runat="server" ControlToValidate="txtPrevistas"
                                            ErrorMessage="Quantidade de aulas previstas do {0} é obrigatório."
                                            ValidationGroup="AulasPrevistas">*</asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cvPrevistas" runat="server" Text="*"
                                            ErrorMessage="Quantidade de aulas previstas deve ser maior que 0 (zero)."
                                            ControlToValidate="txtPrevistas" Type="Integer" Operator="GreaterThan"
                                            ValueToCompare="0" Display="Dynamic" ValidationGroup="AulasPrevistas">*</asp:CompareValidator>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label runat="server" ID="lblTotalPrevistas" CssClass="lblPrevistas" Text="sdadas"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cumpridas">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblDadas" Text='<%# Bind("aulasDadas") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label runat="server" ID="lblTotalDadas"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reposições">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblReposicoes" Text='<%# Bind("aulasRepostas") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label runat="server" ID="lblTotalReposicoes"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="right">
                        <asp:Button ID="btnSalvarAulasPrevistas" runat="server" Text="Salvar"
                            ValidationGroup="AulasPrevistas"
                            OnClick="btnSalvarAulasPrevistas_Click" />
                        <asp:Button ID="btnFecharJanela" runat="server" Text="Fechar" CausesValidation="false"
                            OnClientClick="$('.divIndicadores').dialog('close');return false;" />
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <%-- Disciplinas compartilhadas --%>
    <uc10:UCSelecaoDisciplinaCompartilhada ID="UCSelecaoDisciplinaCompartilhada1" runat="server"></uc10:UCSelecaoDisciplinaCompartilhada>
</asp:Content>
