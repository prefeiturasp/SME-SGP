<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Academico.ControleTurma.Busca" %>

<%@ Register Src="~/WebControls/Combos/Novos/UCComboGenerico.ascx" TagName="UCCComboGenerico" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="UCComboUAEscola" TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCursoCurriculo.ascx" TagName="UCCCursoCurriculo" TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCurriculoPeriodo.ascx" TagName="UCCCurriculoPeriodo" TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoDocente.ascx" TagName="UCComboTipoDocente" TagPrefix="uc5" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao" TagPrefix="uc6" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoCiclo.ascx" TagName="UCComboTipoCiclo" TagPrefix="uc7" %>
<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros" TagPrefix="uc8" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCalendario.ascx" TagName="UCCCalendario" TagPrefix="uc9" %>
<%@ Register Src="~/WebControls/ControleTurma/UCSelecaoDisciplinaCompartilhada.ascx" TagName="UCSelecaoDisciplinaCompartilhada" TagPrefix="uc10" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="loader" style="display: none;">
        <asp:Image SkinID="imgLoader" runat="server" />
    </div>
    <asp:UpdatePanel ID="upnMensagem" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Label ID="lblMensagem" runat="server" EnableViewState="false"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Label ID="lblMensagemAlertaDocente" runat="server"></asp:Label>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="vsMinhasTurmas" />
    <asp:Panel ID="pnlTurmas" runat="server" GroupingText="Minhas turmas">
        <div id="divAgenda" runat="server">
            <asp:Button ID="btnGerarAula" runat="server" Text="<%$ Resources:Academico, ControleTurma.Busca.btnGerarAula.text %>" OnClick="btnGerarAula_Click" />&nbsp;&nbsp;
            <asp:Button ID="btnHistorico" runat="server" Text="<%$ Resources:Academico, ControleTurma.Busca.btnHistorico.text %>" OnClick="btnHistorico_Click" />
            <br />
            <br />
        </div>
        <asp:Label ID="lblMensagem1" runat="server" EnableViewState="false"></asp:Label>
        <div class="clear"></div>
        <%-- Filtros de pesquisa --%>
        <div id="divFiltros" runat="server">
            <asp:UpdatePanel ID="upnFiltros" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <uc2:UCComboUAEscola ID="UCComboUAEscola1" runat="server" CarregarEscolaAutomatico="true"
                        MostrarMessageSelecioneEscola="true" MostrarMessageSelecioneUA="true"
                        ValidationGroup="vsMinhasTurmas" ObrigatorioUA="true" ObrigatorioEscola="true" />
                    <uc9:UCCCalendario ID="UCCCalendario1" runat="server" MostrarMensagemSelecione="true"
                        Obrigatorio="true" ValidationGroup="vsMinhasTurmas" PermiteEditar="false" />
                    <uc3:UCCCursoCurriculo ID="UCComboCursoCurriculo1" runat="server"
                        MostrarMensagemSelecione="true" PermiteEditar="false" Obrigatorio="true"
                        ValidationGroup="vsMinhasTurmas" />
                    <uc7:UCComboTipoCiclo ID="UCComboTipoCiclo" runat="server" Obrigatorio="false" 
                        Titulo="Ciclo" Enabled="false" />
                    <uc4:UCCCurriculoPeriodo ID="UCCCurriculoPeriodo1" runat="server" 
                        MostrarMensagemSelecione="true" PermiteEditar="false" />
                    <asp:Label ID="lblCodigoTurma" runat="server" Text="Código da turma" AssociatedControlID="txtCodigoTurma"></asp:Label>
                    <asp:TextBox ID="txtCodigoTurma" runat="server" MaxLength="30" SkinID="text30C"></asp:TextBox>
                    <div class="right">
                        <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click"
                            ValidationGroup="vsMinhasTurmas" />
                        <asp:Button ID="btnLimparPesquisa" runat="server" Text="Limpar pesquisa" OnClick="btnLimparPesquisa_Click"
                            CausesValidation="false" />
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnPesquisar" />
                    <asp:PostBackTrigger ControlID="btnLimparPesquisa" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="clear"></div>
    </asp:Panel>
    <asp:UpdatePanel ID="upnResultado" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <%-- Resultado da pesquisa 'Docente' --%>
            <div id="divResultadoDocente" runat="server" visible="false" class="divResultadosDocente">
                <asp:Repeater ID="rptTurmas" runat="server" OnItemDataBound="rptTurmas_ItemDataBound">
                    <ItemTemplate>
                        <asp:HiddenField ID="hdnUadSuperior" runat="server" Value='<%# Eval("uad_idSuperior") %>' />
                        <asp:HiddenField ID="hdnEscola" runat="server" Value='<%# Eval("esc_id") %>' />
                        <asp:HiddenField ID="hdnUnidadeEscola" runat="server" Value='<%# Eval("uni_id") %>' />
                        <asp:HiddenField ID="hdnCalendario" runat="server" Value='<%# Eval("cal_id") %>' />
                        <asp:HiddenField ID="hdnCalendarioAno" runat="server" Value='<%# Eval("cal_ano") %>' />
                        <fieldset>
                            <legend class="legendMinhasTurmas" runat="server" id="legMinhasTurmas">
                                <asp:Label runat="server" ID="txtLegendMinhasTurmas" Text='<%#Eval("lengendTitulo") %>'
                                    ForeColor='<%# ((Convert.ToInt32(Eval("cal_ano").ToString()) < DateTime.Now.Year && Convert.ToBoolean(Eval("turmasAnoAtual").ToString()) == true) ? System.Drawing.ColorTranslator.FromHtml("#A52A2A") : System.Drawing.Color.Black) %>'></asp:Label>
                                <asp:Label runat="server" ID="lblDataProcessamento" class="dataProcessamentoPendencia"></asp:Label>
                            </legend>
                            <div runat="server" id="divMessageTurmaAnterior" visible='<%# ((Convert.ToInt32(Eval("cal_ano").ToString()) < DateTime.Now.Year) && (Convert.ToBoolean(Eval("turmasAnoAtual").ToString()) == true)) %>'
                                class="summaryMsgAnosAnteriores" style="<%$ Resources: Academico, ControleTurma.Busca.divMessageTurmaAnterior.Style %>">
                                <asp:Label runat="server" ID="lblMessageTurmaAnterior" Text="<%$ Resources:Academico, ControleTurma.Busca.lblMessageTurmaAnterior.Text %>"
                                    Style="<%$ Resources: Academico, ControleTurma.Busca.lblMessageTurmaAnterior.Style %>"></asp:Label>
                            </div>
                            <div class="divScrollResponsivo">
                                <div class="clear"></div>
                                <div id="mensagemPendenciaFechamentoMinhasTurmas" class="mensagemPendenciaFechamentoMinhasTurmas" runat="server" visible="false">
                                    <asp:LinkButton ID="lkbMensagemPendenciaFechamento" runat="server" Text="<%$ Resources:Academico, ControleTurma.Busca.lkbMensagemPendenciaFechamentoDocente.Text %>" OnClick="lkbMensagemPendenciaFechamento_Click"></asp:LinkButton>
                                </div>
                                <div id="mensagemSemPendenciaFechamento" class="mensagemSemPendenciaFechamento" runat="server" visible="false">
                                    <asp:Literal ID="litMensagemSemPendenciaFechamento" runat="server" Text="<%$ Resources:Academico, ControleTurma.Busca.litMensagemSemPendenciaFechamento.Text %>"></asp:Literal>
                                </div>
                            <asp:GridView ID="grvTurma" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" PageSize="10"
                                OnRowCommand="grvMinhasTurmas_RowCommand"
                                DataKeyNames="tur_id,tur_escolaUnidade,tur_codigo,tud_nome,tud_id,tdt_posicao,cal_id,esc_id,uni_id,tud_naoLancarNota,tud_naoLancarFrequencia,tud_disciplinaEspecial,EscolaTurmaDisciplina,tud_tipo,tur_dataEncerramento,tciIds,disciplinaAtiva,tur_tipo,tud_idAluno,tur_idNormal,fav_id"
                                OnPageIndexChanging="grvTurma_PageIndexChanging" OnRowDataBound="grvTurmas_RowDataBound" OnDataBound="grvTurmas_DataBound" SkinID="GridResponsive">
                                <Columns>
                                    <asp:TemplateField HeaderText="Turma">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTurma" runat="server" Text='<%# Eval("tur_codigo") + " - " + Eval("tud_nome") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Curso">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_curso" runat="server" Text='<%# Eval("tur_curso") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Turno" DataField="tur_turno" />
                                    <asp:BoundField HeaderText="Tipo de docência" DataField="TipoDocencia" />
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
                                            <asp:Image ID="imgDivergenciaAulaPrevista" runat="server" SkinID="imgAviso" Width="16px" Height="16px"
                                                ToolTip="<%$ Resources:Academico, ControleTurma.Busca.imgDivergenciaAulaPrevista.ToolTip %>"
                                                Visible='<%# Convert.ToBoolean(Eval("divergenciasAulasPrevistas")) %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%--Planejamento--%>
                                    <asp:TemplateField HeaderText="Planejamento" HeaderStyle-CssClass="center"
                                        ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-responsive-item-inline grid-responsive-center">
                                        <ItemTemplate>
                                            <span class="ico-font ico-planejamento">
                                                <asp:ImageButton ID="btnPlanejamento"
                                                    CommandArgument='<%# Container.DataItemIndex %>'
                                                    runat="server" SkinID="btPlanejamentoGestor" CommandName="Planejamento"
                                                    ToolTip="Planejamento" />
                                                <asp:ImageButton ID="imgPendenciaPlanejamento" runat="server" SkinID="btStatusAlertaPendencia" Visible="false"
                                                    CommandArgument='<%# Container.DataItemIndex %>' 
                                                    CommandName="PendenciaPlanejamento" />
                                            </span>
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
                                            <span class="ico-font ico-listao">
                                                <asp:ImageButton ID="btnListao"
                                                    CommandArgument='<%# Container.DataItemIndex %>'
                                                    runat="server" SkinID="btListaoGestor" CommandName="Listao"
                                                    ToolTip="<%$ Resources:Mensagens, MSG_Listao %>" /></span>
                                                <asp:ImageButton ID="imgPendenciaPlanoAula" runat="server" SkinID="btStatusAlertaPendencia" Visible="false" 
                                                    CommandArgument='<%# Container.DataItemIndex %>' 
                                                    CommandName="PendenciaPlanoAula" />
                                            </span>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--Frequência --%>
                                    <asp:TemplateField HeaderText="<%$ Resources:WebControls, UCNavegacaoTelaPeriodo.btnFrequencia.Text %>" HeaderStyle-CssClass="center"
                                        ItemStyle-HorizontalAlign="Center"  ItemStyle-CssClass="grid-responsive-item-inline grid-responsive-center">
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
                                            <asp:HiddenField ID="hdnTudId" runat="server" />
                                            <asp:HiddenField ID="hdnTurId" runat="server" />
                                            <asp:HiddenField ID="hdnTpcId" runat="server" />
                                            <asp:HiddenField ID="hdnAvaId" runat="server" />
                                            <asp:HiddenField ID="hdnFavId" runat="server" />
                                            <asp:HiddenField ID="hdnTipoAvaliacao" runat="server" />
                                            <asp:HiddenField ID="hdnEsaId" runat="server" />
                                            <asp:HiddenField ID="hdnTipoEscala" runat="server" />
                                            <asp:HiddenField ID="hdnTipoEscalaDocente" runat="server" />
                                            <asp:HiddenField ID="hdnNotaMinima" runat="server" />
                                            <asp:HiddenField ID="hdnParecerMinimo" runat="server" />
                                            <asp:HiddenField ID="hdnTipoLancamento" runat="server" />
                                            <asp:HiddenField ID="hdnCalculoQtAulasDadas" runat="server" />
                                            <asp:HiddenField ID="hdnTurTipo" runat="server" />
                                            <asp:HiddenField ID="hdnCalId" runat="server" />
                                            <asp:HiddenField ID="hdnTudTipo" runat="server" />
                                            <asp:HiddenField ID="hdnTpcOrdem" runat="server" />
                                            <asp:HiddenField ID="hdnVariacao" runat="server" />
                                            <asp:HiddenField ID="hdnTipoDocente" runat="server" />
                                            <asp:HiddenField ID="hdnDisciplinaEspecial" runat="server" />
                                            <asp:HiddenField ID="hdnFechamentoAutomatico" runat="server" />
                                            <span class="ico-font ico-fechamento">
                                                <asp:ImageButton ID="btnFechamento"
                                                    CommandArgument='<%# Container.DataItemIndex %>'
                                                    runat="server" SkinID="btFechamentoGestor" CommandName="Fechamento"
                                                    ToolTip="<%$ Resources:Mensagens, MSG_EFETIVACAO %>"
                                                    Visible='<%# Convert.ToByte(Eval("tud_tipo")) != (byte)MSTech.GestaoEscolar.BLL.ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada %>' />
                                                <asp:ImageButton ID="imgPendenciaFechamento" runat="server" SkinID="btStatusAlertaPendencia" Visible="false"
                                                    CommandArgument='<%# Container.DataItemIndex %>' 
                                                    CommandName="PendenciaFechamento" />
                                            </span>
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
                                </div>
                            <uc8:UCTotalRegistros ID="UCTotalRegistros2" runat="server" AssociatedGridViewID="grvTurma" />
                        </fieldset>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
            <%-- Resultado da pesquisa 'Visão Superior' --%>
            <div id="divResultadoVisaoSuperior" runat="server" visible="false">
                <fieldset id="fdsResultado" runat="server">
                    <legend>
                        Resultados
                        <asp:Label runat="server" ID="lblDataProcessamentoAdm" class="dataProcessamentoPendencia"></asp:Label>
                    </legend>
                    <uc6:UCComboQtdePaginacao ID="UCComboQtdePaginacao" runat="server"
                        OnIndexChanged="UCComboQtdePaginacao_IndexChanged" ComboDefaultValue="true" />
                    <div class="clear"></div>
                    <div id="mensagemPendenciaFechamentoMinhasTurmas" class="mensagemPendenciaFechamentoMinhasTurmas" runat="server" visible="false">
                        <asp:LinkButton ID="lkbMensagemPendenciaFechamento" runat="server" Text="<%$ Resources:Academico, ControleTurma.Busca.lkbMensagemPendenciaFechamentoGestor.Text %>" OnClick="lkbMensagemPendenciaFechamento_Click"></asp:LinkButton>
                    </div>
                    <div id="mensagemSemPendenciaFechamento" class="mensagemSemPendenciaFechamento" runat="server" visible="false">
                        <asp:Literal ID="litMensagemSemPendenciaFechamento" runat="server" Text="<%$ Resources:Academico, ControleTurma.Busca.litMensagemSemPendenciaFechamento.Text %>"></asp:Literal>
                    </div>
                    <asp:GridView ID="grvTurmas" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true"
                        OnRowCommand="grvMinhasTurmas_RowCommand" DataSourceID='odsMinhasTurmas' OnRowDataBound="grvTurmas_RowDataBound"
                        EmptyDataText="A pesquisa não encontrou resultados." OnDataBound="grvTurmas_DataBound" SkinID="GridResponsive"
                        DataKeyNames="tur_id,tur_escolaUnidade,tur_codigo,tud_nome,tud_id,tdt_posicao,cal_id,esc_id,uni_id,tud_naoLancarNota,tud_naoLancarFrequencia,tud_disciplinaEspecial,EscolaTurmaDisciplina,tud_tipo,tur_dataEncerramento,tciIds,disciplinaAtiva,tur_tipo,tud_idAluno,tur_idNormal,fav_id">
                        <Columns>
                            <asp:TemplateField HeaderText="Turma" SortExpression="tur_codigo">
                                <ItemTemplate>
                                    <asp:Label ID="lblTurma" runat="server" Text='<%# Eval("tur_codigo") + " - " + Eval("tud_nome") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Curso" SortExpression="tur_curso">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_curso" runat="server" Text='<%# Eval("tur_curso") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Turno" SortExpression="tur_turno">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_turno" runat="server" Text='<%# Eval("tur_turno") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tipo de docência" SortExpression="TipoDocencia">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_TipoDocencia" runat="server" Text='<%# Eval("TipoDocencia") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Mensagens, MSG_AulasDadas %>" HeaderStyle-CssClass="center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-responsive-item-inline grid-responsive-center">
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
                                    <asp:Image ID="imgDivergenciaAulaPrevista" runat="server" SkinID="imgAviso" Width="16px" Height="16px"
                                        ToolTip="<%$ Resources:Academico, ControleTurma.Busca.imgDivergenciaAulaPrevista.ToolTip %>"
                                        Visible="false" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%--Planejamento--%>
                            <asp:TemplateField HeaderText="Planejamento" HeaderStyle-CssClass="center"
                                ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-responsive-item-inline grid-responsive-center">
                                <ItemTemplate>
                                    <span class="ico-font ico-planejamento">
                                        <asp:ImageButton ID="btnPlanejamento"
                                            CommandArgument='<%# Container.DataItemIndex %>'
                                            runat="server" SkinID="btPlanejamentoGestor" CommandName="Planejamento"
                                            ToolTip="Planejamento" />
                                    </span>
                                    <asp:ImageButton ID="imgPendenciaPlanejamento" runat="server" SkinID="btStatusAlertaPendencia" Visible="false"
                                        CommandArgument='<%# Container.DataItemIndex %>' 
                                        CommandName="PendenciaPlanejamento" />
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
                                    <span class="ico-font ico-listao">
                                        <asp:ImageButton ID="btnListao"
                                            CommandArgument='<%# Container.DataItemIndex %>'
                                            runat="server" SkinID="btListaoGestor" CommandName="Listao"
                                            ToolTip="<%$ Resources:Mensagens, MSG_Listao %>" />
                                    </span>
                                    <asp:ImageButton ID="imgPendenciaPlanoAula" runat="server" SkinID="btStatusAlertaPendencia" Visible="false"
                                        CommandArgument='<%# Container.DataItemIndex %>' 
                                        CommandName="PendenciaPlanoAula" />
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
                                    <asp:HiddenField ID="hdnTudId" runat="server" />
                                    <asp:HiddenField ID="hdnTurId" runat="server" />
                                    <asp:HiddenField ID="hdnTpcId" runat="server" />
                                    <asp:HiddenField ID="hdnAvaId" runat="server" />
                                    <asp:HiddenField ID="hdnFavId" runat="server" />
                                    <asp:HiddenField ID="hdnTipoAvaliacao" runat="server" />
                                    <asp:HiddenField ID="hdnEsaId" runat="server" />
                                    <asp:HiddenField ID="hdnTipoEscala" runat="server" />
                                    <asp:HiddenField ID="hdnTipoEscalaDocente" runat="server" />
                                    <asp:HiddenField ID="hdnNotaMinima" runat="server" />
                                    <asp:HiddenField ID="hdnParecerMinimo" runat="server" />
                                    <asp:HiddenField ID="hdnTipoLancamento" runat="server" />
                                    <asp:HiddenField ID="hdnCalculoQtAulasDadas" runat="server" />
                                    <asp:HiddenField ID="hdnTurTipo" runat="server" />
                                    <asp:HiddenField ID="hdnCalId" runat="server" />
                                    <asp:HiddenField ID="hdnTudTipo" runat="server" />
                                    <asp:HiddenField ID="hdnTpcOrdem" runat="server" />
                                    <asp:HiddenField ID="hdnVariacao" runat="server" />
                                    <asp:HiddenField ID="hdnTipoDocente" runat="server" />
                                    <asp:HiddenField ID="hdnDisciplinaEspecial" runat="server" />
                                    <asp:HiddenField ID="hdnFechamentoAutomatico" runat="server" />
                                    <span class="ico-font ico-fechamento">
                                        <asp:ImageButton ID="btnFechamento"
                                            CommandArgument='<%# Container.DataItemIndex %>'
                                            runat="server" SkinID="btFechamentoGestor" CommandName="Fechamento"
                                            ToolTip="<%$ Resources:Mensagens, MSG_EFETIVACAO %>"
                                            Visible='<%# Convert.ToByte(Eval("tud_tipo")) != (byte)MSTech.GestaoEscolar.BLL.ACA_CurriculoDisciplinaTipo.DocenciaCompartilhada %>' />
                                    </span>
                                    <asp:ImageButton ID="imgPendenciaFechamento" runat="server" SkinID="btStatusAlertaPendencia" Visible="false"
                                        CommandArgument='<%# Container.DataItemIndex %>' 
                                        CommandName="PendenciaFechamento" />
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
                    <uc8:UCTotalRegistros ID="UCTotalRegistros" runat="server" AssociatedGridViewID="grvTurmas" />
                    <asp:ObjectDataSource ID="odsMinhasTurmas" runat="server" TypeName="MSTech.GestaoEscolar.BLL.TUR_TurmaBO"
                        SelectMethod="SelecionaPorFiltrosMinhasTurmasPaginado"></asp:ObjectDataSource>
                </fieldset>
            </div>
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
                    <div>
                        <uc5:UCComboTipoDocente ID="UCComboTipoDocente1" runat="server"></uc5:UCComboTipoDocente>
                        <uc1:UCCComboGenerico ID="uccTurmaDisciplina" runat="server"
                            MostrarMensagemSelecione="false" Obrigatorio="false"
                            TituloCombo="<%$ Resources:Academico, ControleTurma.Busca.uccTurmaDisciplina.TituloCombo %>"
                            ValorItemVazio="-1;-1;-1;-1"></uc1:UCCComboGenerico>
                        <asp:HiddenField ID="hdnEscId" runat="server" Value="-1" />
                        <asp:GridView ID="grvPeriodosAulas" runat="server" AutoGenerateColumns="false" DataKeyNames="tud_id, tud_tipo, tpc_id, cap_dataInicio, cap_dataFim, cap_descricao, fav_fechamentoAutomatico" ShowFooter="True"
                            EmptyDataText="Não existem dados a serem lançados em bimestres para a turma selecionada."
                            OnRowDataBound="grvPeriodosAulas_RowDataBound" OnRowCreated="grvPeriodosAulas_RowCreated" SkinID="GridResponsive">
                            <Columns>
                                <asp:BoundField HeaderText="Bimestre" DataField="cap_descricao" FooterText="Total" />
                                <asp:BoundField HeaderText="Período" DataField="periodo" />
                                <asp:TemplateField HeaderText="<%$ Resources:Academico, ControleTurma.Busca.grvPeriodosAulas.ColunaSugestao %>">
                                    <ItemTemplate>
                                        <asp:HyperLink runat="server" ID="lnkSugestao" Text='<%# Bind("aulasSugestao") %>' style="cursor:pointer"></asp:HyperLink>
                                        <asp:Label runat="server" ID="lblSugestao" Text='<%# Bind("aulasSugestao") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label runat="server" ID="lblTotalSugestao" ></asp:Label>
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <FooterStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <span class="responsive-hide">
                                            <asp:Button ID="btnSugestao" runat="server" Text=" > " OnClientClick="AplicarSugestaoAulasPrevistas(this); return false;" />
                                        <span>
                                   </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Previstas *">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtPrevistas" Text='<%# Bind("aulasPrevistas") %>'
                                            SkinID="Numerico2c" MaxLength="3"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rvPrevistas" runat="server" ControlToValidate="txtPrevistas"
                                            ErrorMessage="Quantidade de aulas previstas do {0} é obrigatório."
                                            ValidationGroup="AulasPrevistas">*</asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cvPrevistas" runat="server" Text="*"
                                            ErrorMessage="Quantidade de aulas previstas deve ser maior que 0 (zero)."
                                            ControlToValidate="txtPrevistas" Type="Integer" Operator="GreaterThan"
                                            ValueToCompare="0" Display="Dynamic" ValidationGroup="AulasPrevistas">*</asp:CompareValidator>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label runat="server" ID="lblTotalPrevistas" CssClass="lblPrevistas" Text=""></asp:Label>
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <FooterStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Academico, ControleTurma.Busca.grvPeriodosAulas.ColunaAulasCriadas %>">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblCriadas" Text='<%# Bind("aulasCriadas") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cumpridas">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblDadas" Text='<%# Bind("aulasDadas") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label runat="server" ID="lblTotalDadas"></asp:Label>
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <FooterStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reposições">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblReposicoes" Text='<%# Bind("aulasRepostas") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label runat="server" ID="lblTotalReposicoes"></asp:Label>
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <FooterStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="right">
                        <asp:Button ID="btnSalvar" runat="server" Text="Salvar"
                            ValidationGroup="AulasPrevistas"
                            OnClick="btnSalvar_Click" />
                        <asp:Button ID="btnFecharJanela" runat="server" Text="Fechar" CausesValidation="false"
                            OnClientClick="$('.divIndicadores').dialog('close');return false;" />
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <%-- Tipo de docencia --%>
    <div id="divSelecionaTipoDocencia" title="Tipos de docência" class="hide">
        <br />
        <asp:LinkButton ID="lkRegular" runat="server" Text="Visualizar como professor regular"
            OnClick="lkRegular_Click"></asp:LinkButton>
        <br />
        <br />
        <asp:LinkButton ID="lkEspecial" runat="server" Text="Visualizar como professor de disciplina especial"
            OnClick="lkEspecial_Click"></asp:LinkButton>
        <br />
        <br />
        <div class="right">
            <asp:Button ID="btnFecharTipoDoc" runat="server" Text="Fechar"
                OnClientClick="$('#divSelecionaTipoDocencia').dialog('close');return false;" />
        </div>
    </div>
    <%-- Histórico de Turmas --%>
    <div id="divHistorico" title="Histórico de Turmas" class="hide">
        <asp:UpdatePanel ID="updHistorico" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label ID="lblMensagemHistorico" runat="server" EnableViewState="false"></asp:Label>
                <div id="divHistoricoTurmas" runat="server">
                    <br />

                    <uc2:UCComboUAEscola ID="UCComboUAEscola2" runat="server" />
                    <br />
                    <br />


                    <asp:HiddenField ID="hdnTabSelecionado" runat="server" />
                    <div id="divMostrarAbas" runat="server">
                        <div id="divTabs">
                            <ul class="hide">
                                <asp:Repeater ID="rptCiclos" runat="server">
                                    <ItemTemplate>
                                        <li><a href='#<%# RetornaTabID((int)Eval("tci_id"))%>'><%# Eval("tci_nome") %></a></li>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <li id="liTurmasEx" runat="server">
                                    <a href="#divTabsTurmasEx">
                                    <asp:Label ID="lblTabTurmasExtintas" runat="server"
                                        Text="<%$ Resources:Academico, ControleTurma.Busca.lblTabsTurmasEx.text %>" />
                                    </a>
                                </li>
                                <li id="liTurmasInat" runat="server">
                                    <a href="#divTabsTurmasInat">
                                        <asp:Label ID="lblTabTurmasInativas" runat="server"
                                            Text="<%$ Resources:Academico, ControleTurma.Busca.lblTabsTurmasAnosAnteriores.text %>" />
                                    </a>
                                </li>
                            </ul>
                            <asp:Repeater ID="rptCiclosAbas" runat="server" OnItemDataBound="rptCiclosAbas_ItemDataBound">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnCiclo" runat="server" Value='<%# Eval("tci_id") %>' />
                                    <div id='<%# RetornaTabID((int)Eval("tci_id"))%>'>
                                        <fieldset id="fsAba" runat="server">
                                            <asp:GridView ID="grvHistorico" runat="server" AutoGenerateColumns="false"
                                                DataKeyNames="tud_id,tdt_posicao,Turma" OnRowCommand="grvHistorico_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Turma">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lkbTurmaHistorico" runat="server"
                                                                Text='<%# Eval("Turma") %>' CommandName="Turma"
                                                                CommandArgument='<%# Eval("esc_id") 
                                                                                        + "," + Eval("tur_id") 
                                                                                        + "," + Eval("tud_id") 
                                                                                        + "," + Eval("cal_id") 
                                                                                        + "," + Eval("tdt_posicao") 
                                                                                        + "," + Eval("tud_tipo") 
                                                                                        + "," + Eval("tdt_situacao") 
                                                                                        + "," + Container.DataItemIndex
                                                                                        + "," + (String.IsNullOrEmpty(Eval("docenciaCompartilhada").ToString()) ? "0" : "1") %>'></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Tipo de docência" DataField="tdc_nome">
                                                        <HeaderStyle CssClass="center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="<%$ Resources:Academico, ControleTurma.Busca.grvHistorico.DisciplinaCompartilhada %>" DataField="docenciaCompartilhada">
                                                        <HeaderStyle CssClass="center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                </Columns>
                                            </asp:GridView>
                                        </fieldset>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <div id="divTabsTurmasEx" class="hide">
                                <fieldset id="fdsTurmasExtintas" runat="server">
                                    <asp:GridView ID="grvHistoricoTurmasExtintas" runat="server" AutoGenerateColumns="false"
                                        DataKeyNames="tud_id,tdt_posicao,Turma"
                                        OnRowCommand="grvHistoricoTurmasExtintas_RowCommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Turma">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lkbTurmaHistorico" runat="server"
                                                        Text='<%# Eval("Turma") %>'
                                                        CommandName="Turma"
                                                        CommandArgument='<%# Eval("esc_id") 
                                                                                + "," + Eval("tur_id") 
                                                                                + "," + Eval("tud_id") 
                                                                                + "," + Eval("cal_id") 
                                                                                + "," + Eval("tdt_posicao")
                                                                                + "," + Container.DataItemIndex
                                                                                + "," + Eval("tud_tipo")
                                                                                + "," + (String.IsNullOrEmpty(Eval("docenciaCompartilhada").ToString()) ? "0" : "1") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Tipo de docência" DataField="tdc_nome">
                                                <HeaderStyle CssClass="center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Mensagens, MSG_AulasDadas %>" HeaderStyle-CssClass="center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnIndicadores"
                                                        CommandArgument='<%# Eval("esc_id") 
                                                                            + "," + Eval("tur_id") 
                                                                            + "," + Eval("tud_id") 
                                                                            + "," + Eval("cal_id") 
                                                                            + "," + Eval("tdt_posicao") %>'
                                                        runat="server" SkinID="btIndicadores" CommandName="Indicadores"
                                                        ToolTip="Visualizar indicadores de aulas da turma" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </fieldset>
                            </div>
                            <div id="divTabsTurmasInat" class="hide">
                                <fieldset id="fdsTurmasInativas" runat="server">
                                    <asp:Label runat="server" ID="lblAnoInativos" AssociatedControlID="ddlAnoInativos" Text="Ano"></asp:Label>
                                    <asp:DropDownList runat="server" ID="ddlAnoInativos" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlAnoInativos_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:GridView ID="grvHistoricoTurmasInativas" runat="server" AutoGenerateColumns="false"
                                        DataKeyNames="tud_id,tdt_posicao,Turma"
                                        OnRowCommand="grvHistoricoTurmasInativas_RowCommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Turma">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lkbTurmaHistorico" runat="server"
                                                        Text='<%# Eval("Turma") %>'
                                                        CommandName="Turma"
                                                        CommandArgument='<%# Eval("esc_id") 
                                                                                + "," + Eval("tur_id") 
                                                                                + "," + Eval("tud_id") 
                                                                                + "," + Eval("cal_id") 
                                                                                + "," + Eval("tdt_posicao")
                                                                                + "," + Container.DataItemIndex
                                                                                + "," + Eval("tud_tipo") 
                                                                                + "," + (String.IsNullOrEmpty(Eval("docenciaCompartilhada").ToString()) ? "0" : "1") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Tipo de docência" DataField="tdc_nome">
                                                <HeaderStyle CssClass="center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Mensagens, MSG_AulasDadas %>" HeaderStyle-CssClass="center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnIndicadores"
                                                        CommandArgument='<%# Eval("esc_id") 
                                                                                + "," + Eval("tur_id")
                                                                                + "," + Eval("tud_id") 
                                                                                + "," + Eval("cal_id") 
                                                                                + "," + Eval("tdt_posicao") %>'
                                                        runat="server" SkinID="btIndicadores" CommandName="Indicadores"
                                                        ToolTip="Visualizar indicadores de aulas da turma" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </fieldset>
                            </div>
                        </div>
                    </div>
                    <div class="right">
                        <asp:Button ID="btnAtribuirTurma" runat="server" Text="Atribuir nova turma"
                            OnClick="btnAtribuirTurma_Click" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <%-- Disciplinas compartilhadas --%>
    <uc10:UCSelecaoDisciplinaCompartilhada ID="UCSelecaoDisciplinaCompartilhada1" runat="server"></uc10:UCSelecaoDisciplinaCompartilhada>
</asp:Content>
