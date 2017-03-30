<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Academico.ControleSemanal.Busca" %>

<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="UCComboUAEscola" TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCursoCurriculo.ascx" TagName="UCCCursoCurriculo" TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCurriculoPeriodo.ascx" TagName="UCCCurriculoPeriodo" TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao" TagPrefix="uc6" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoCiclo.ascx" TagName="UCComboTipoCiclo" TagPrefix="uc7" %>
<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros" TagPrefix="uc8" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCalendario.ascx" TagName="UCCCalendario" TagPrefix="uc9" %>

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
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="vsMinhasTurmas" />
    <asp:Panel ID="pnlTurmas" runat="server" GroupingText="Planejamento semanal">
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
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <%--Planejamento semanal--%>
                                        <asp:TemplateField HeaderText="Planejamento semanal" HeaderStyle-CssClass="center"
                                            ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-responsive-item-inline grid-responsive-center">
                                            <ItemTemplate>
                                                <span class="ico-font ico-planejamento">
                                                    <asp:ImageButton ID="btnPlanejamentosemanal" CommandArgument='<%# Container.DataItemIndex %>'
                                                        runat="server" SkinID="btPlanejamentoGestor" CommandName="PlanejamentoSemanal"
                                                        ToolTip="Planejamento semanal" />
                                                </span>
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
                    <asp:GridView ID="grvTurmas" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true"
                        OnRowCommand="grvMinhasTurmas_RowCommand" DataSourceID='odsplanejamentoSemanal' OnRowDataBound="grvTurmas_RowDataBound"
                        EmptyDataText="A pesquisa não encontrou resultados." OnDatound="grvTurmas_DataBound" SkinID="GridResponsive"
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
                            <%--Planejamento semanal--%>
                            <asp:TemplateField HeaderText="Planejamento semanal" HeaderStyle-CssClass="center"
                                ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-responsive-item-inline grid-responsive-center">
                                <ItemTemplate>
                                    <span class="ico-font ico-planejamento"><asp:ImageButton ID="btnPlanejamentoSemanal"
                                            CommandArgument='<%# Container.DataItemIndex %>'
                                            runat="server" SkinID="btPlanejamentoGestor" CommandName="PlanejamentoSemanal"
                                            ToolTip="Planejamento semanal" />
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <uc8:UCTotalRegistros ID="UCTotalRegistros" runat="server" AssociatedGridViewID="grvTurmas" />
                    <asp:ObjectDataSource ID="odsplanejamentoSemanal" runat="server" TypeName="MSTech.GestaoEscolar.BLL.TUR_TurmaBO"
                        SelectMethod="SelecionaPorFiltrosPlanejamentoSemanalPaginado"></asp:ObjectDataSource>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
