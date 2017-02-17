<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCAlunoEfetivacaoObservacao.ascx.cs" Inherits="GestaoEscolar.WebControls.AlunoEfetivacaoObservacao.UCAlunoEfetivacaoObservacao" %>

<asp:Label ID="lblMensagem" runat="server"></asp:Label>
<asp:Label ID="lblMensagem2" runat="server"></asp:Label>
<asp:Panel ID="pnlObservacao" runat="server">
    <br />
    <asp:Label ID="lblDadosAluno" runat="server"></asp:Label>
    <br />
    <br />
    <div id="divTabs">
        <ul class="hide">
            <li id="liDesempenhoAprendizado" runat="server">
                <a href="#divTabs-1">
                    <asp:Label ID="lblDesempenhoAprendizagem" runat="server"></asp:Label>
                </a>
            </li>
            <li><a href="#divTabs-2">Recomendações ao aluno</a></li>
            <li><a href="#divTabs-3">Recomendações aos pais/responsáveis</a></li>
            <li id="liParecerConclusivo" runat="server">
                <a href="#divTabs-4">
                    <asp:Literal ID="litAbaParecerConclusivo" runat="server" Text="<%$ Resources:Mensagens, MSG_RESULTADOEFETIVACAO %>"></asp:Literal>
                </a>
            </li>
        </ul>
        <div id="divTabs-1">
            <fieldset id="fdsDesempenhoAprendizado" runat="server">
                <asp:Label ID="lblResumoDesempenho" runat="server" Text="<%$ Resources:UserControl, AlunoEfetivacaoObservacao.UCAlunoEfetivacaoObservacao.lblResumoDesempenho.Text %>"
                    AssociatedControlID="txtResumoDesempenho"></asp:Label>
                <asp:TextBox ID="txtResumoDesempenho" runat="server" TextMode="MultiLine" SkinID="text60c"></asp:TextBox>
                <br />
                <br />
                <asp:Label ID="lblListaDesempenho" runat="server" Text="<%$ Resources:Mensagens, MSG_DESEMPENHOAPRENDIZADO %>"></asp:Label>

                <asp:UpdatePanel ID="updDesempenho" runat="server">
                    <ContentTemplate>
                        <asp:Repeater ID="rptDesempenho" runat="server">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnConfirmaDesempenho" runat="server" SkinID="btConfirmar"
                                    Visible='<%# (!string.IsNullOrEmpty(Eval("tda_id").ToString()))? true: false %>' />
                                <asp:Label ID="lblDesempenho" runat="server" Text='<%# Eval("tda_descricao").ToString() %>'
                                    Visible='<%# (!string.IsNullOrEmpty(Eval("tda_id").ToString()))? true: false %>'></asp:Label><br />
                            </ItemTemplate>
                        </asp:Repeater>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Label ID="lblInfoDesempenho" runat="server"></asp:Label>
            </fieldset>
        </div>
        <div id="divTabs-2">
            <fieldset id="fdsRecomendacoesAluno" runat="server">
                <asp:Label ID="lblRecomendacaoAluno" runat="server" Text="Resumo de recomendações" AssociatedControlID="txtRecomendacaoAluno"></asp:Label>
                <asp:TextBox ID="txtRecomendacaoAluno" runat="server" TextMode="MultiLine" SkinID="text60c"></asp:TextBox>
                <br />
                <br />
                <asp:Label ID="lblListaRecomendacaoAluno" runat="server" Text="Recomendações"></asp:Label>

                <asp:UpdatePanel ID="updRecomendacaoAluno" runat="server">
                    <ContentTemplate>
                        <asp:Repeater ID="rptRecomendacao" runat="server">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnConfirmaRecomendacao" runat="server" SkinID="btConfirmar"
                                    Visible='<%# (!string.IsNullOrEmpty(Eval("rar_id").ToString()))? true: false %>' />
                                <asp:Label ID="lblRecomendAluno" runat="server" Text='<%# Eval("rar_descricao").ToString() %>'
                                    Visible='<%# (!string.IsNullOrEmpty(Eval("rar_id").ToString()))? true: false %>'></asp:Label><br />
                            </ItemTemplate>
                        </asp:Repeater>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Label ID="lblInfoRecomendacaoAluno" runat="server"></asp:Label>
            </fieldset>
        </div>
        <div id="divTabs-3">
            <fieldset id="fdsRecomendacoesPais" runat="server">
                <asp:Label ID="lblRecomendacaoResp" runat="server" Text="Resumo de recomendações" AssociatedControlID="txtRecomendacaoResp"></asp:Label>
                <asp:TextBox ID="txtRecomendacaoResp" runat="server" TextMode="MultiLine" SkinID="text60c"></asp:TextBox>
                <br />
                <br />
                <asp:Label ID="lblListaRecomendacaoResp" runat="server" Text="Recomendações"></asp:Label>

                <asp:UpdatePanel ID="updRecomendacaoResp" runat="server">
                    <ContentTemplate>
                        <asp:Repeater ID="rptRecomendacaoResp" runat="server">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnConfirmaRecomendacaoResp" runat="server" SkinID="btConfirmar"
                                    Visible='<%# (!string.IsNullOrEmpty(Eval("rar_id").ToString()))? true: false %>' />
                                <asp:Label ID="lblRecomendResp" runat="server" Text='<%# Eval("rar_descricao").ToString() %>'
                                    Visible='<%# (!string.IsNullOrEmpty(Eval("rar_id").ToString()))? true: false %>'></asp:Label><br />
                            </ItemTemplate>
                        </asp:Repeater>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Label ID="lblInfoRecomendacaoResp" runat="server"></asp:Label>
            </fieldset>
        </div>
        <div id="divTabs-4">
            <fieldset id="fdsParecerConclusivo" runat="server">
                <asp:UpdatePanel ID="uppNotaDisciplinas" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div style="float: right; margin-bottom: 5px;">
                            <asp:ImageButton ID="btnAtualizar" runat="server" SkinID="btAtualizar" ToolTip="Atualizar"
                                CausesValidation="false" OnClick="btnAtualizar_Click" />
                            <asp:Label ID="lblAtualizarDados" runat="server" Text="Atualizar" Font-Bold="true" AssociatedControlID="btnAtualizar" Style="display: inline; cursor: pointer"></asp:Label>
                            <asp:HiddenField ID="hfDataUltimaAlteracaoNotaFinal" runat="server" />
                            <asp:HiddenField ID="hfDataUltimaAlteracaoParecerConclusivo" runat="server" />
                            <asp:HiddenField ID="hdnVariacaoFrequencia" runat="server" />
                        </div>
                        <table style="width:100%">
                            <tr>
                                <td>
                                    <asp:Repeater ID="rptNotaDisciplinas" runat="server" OnItemDataBound="rptNotaDisciplinas_ItemDataBound" OnItemCommand="rptNotaDisciplinas_ItemCommand">
                                        <HeaderTemplate>
                                            <table id="tabela" class="grid" cellspacing="0" style="margin: 0px;">
                                                <thead>
                                                    <tr class="gridHeader" style="height: 60px;">
                                                        <th>
                                                            <asp:Literal ID="litHeadNomeDisciplina" runat="server" Text="<%$ Resources:UserControl, AlunoEfetivacaoObservacao.UCAlunoEfetivacaoObservacao.litHeadNomeDisciplina.Text %>"></asp:Literal>
                                                        </th>
                                                        <asp:Repeater ID="rptHeaderPeriodos" runat="server" OnItemDataBound="rptHeaderPeriodos_ItemDataBound">
                                                            <ItemTemplate>
                                                                <th class="center {sorter :false} .sorterFalse">
                                                                    <asp:Literal ID="litHeadPeriodo" runat="server"></asp:Literal>
                                                                </th>
                                                            </ItemTemplate>
                                                        </asp:Repeater>                                                     
                                                        <th id="thFrequenciaFinal" runat="server" class="center {sorter :false}">Freq. final (%)
                                                        </th>
                                                        <th id="thNotaFinal" runat="server" class="center {sorter :false}" style="min-width:100px;">
                                                            <asp:Literal ID="litHeadNotaFinal" runat="server"></asp:Literal>
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr class="gridHeader" style="height: 60px;">
                                                <th>
                                                    <asp:Literal ID="litHeadNomeDisciplinaEnriqCurr" runat="server" Text="<%$ Resources:UserControl,AlunoEfetivacaoObservacao.UCAlunoEfetivacaoObservacao.litHeadNomeDisciplinaEnriqCurr.Text %>"></asp:Literal>
                                                    <asp:Literal ID="litHeadNomeDisciplinaRecPar" runat="server" Text="<%$ Resources:UserControl,AlunoEfetivacaoObservacao.UCAlunoEfetivacaoObservacao.litHeadNomeDisciplinaRecPar.Text %>"></asp:Literal>
                                                </th>
                                                <asp:Repeater ID="rptHeaderPeriodosEnriqCurr" runat="server" OnItemDataBound="rptHeaderPeriodosEnriqCurr_ItemDataBound">
                                                    <ItemTemplate>
                                                        <th class="center {sorter :false} .sorterFalse">
                                                            <asp:Literal ID="litHeadPeriodo" runat="server"></asp:Literal>
                                                        </th>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                                <asp:Repeater ID="rptHeaderPeriodosRecPar" runat="server" OnItemDataBound="rptHeaderPeriodosEnriqCurr_ItemDataBound">
                                                    <ItemTemplate>
                                                        <th class="center {sorter :false} .sorterFalse">
                                                            <asp:Literal ID="litHeadPeriodo" runat="server"></asp:Literal>
                                                        </th>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                                <th id="thFrequenciaFinal" runat="server" class="center {sorter :false}">Freq. final (%)
                                                </th>
                                                <th id="thNotaFinal" runat="server" class="center {sorter :false}" style="min-width:100px;">Parecer final
                                                </th>
                                            </tr>
                                            <tr class="gridRow">
                                                <td style="border-top: 0px;">
                                                    <asp:Label ID="lblNomeDisciplina" runat="server" Text='<%#Bind("tud_nome") %>' Font-Bold="true"></asp:Label>
                                                    <asp:HiddenField ID="hfTudId" runat="server" />
                                                    <asp:HiddenField ID="hfMtdId" runat="server" />
                                                    <asp:HiddenField ID="hfAvaliacaoId" runat="server" />
                                                    <asp:HiddenField ID="hfAvaId" runat="server" />
                                                    <asp:HiddenField ID="hfIsHead" runat="server" Value="0" />
                                                </td>

                                                <%-- Notas/Frequencia de todos os periodos e da avaliacao final --%>
                                                <asp:Repeater ID="rptItemPeriodos" runat="server" OnItemDataBound="rptItemPeriodos_ItemDataBound">
                                                    <ItemTemplate>
                                                        <td style="text-align: center; border-top: 0px;" class="colunaNota">
                                                            <div id="divNotaPeriodo" runat="server" style="padding:10px;">
                                                                <asp:Label ID="lblFrequencia" runat="server" style="margin:0px;"></asp:Label>
                                                                <asp:Label ID="lblNota" runat="server" style="margin:0px;"></asp:Label>
                                                            </div>
                                                            <div id="divLancarRelatorio" style="display: inline; padding: 0 0 0 0;">
                                                                <asp:ImageButton ID="btnRelatorio" runat="server" SkinID="btDetalhar" Style="margin-bottom: -1px !important;"
                                                                    CommandName="Relatorio" CausesValidation="false" Enabled="false" />
                                                                <asp:Image ID="imgSituacao" runat="server" SkinID="imgConfirmar" ToolTip="Relatório lançado"
                                                                    Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                                                                <asp:HyperLink ID="hplAnexo" runat="server" SkinID="hplAnexo" ToolTip="Relatório anexo"
                                                                    Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                                                            </div>
                                                        </td>
                                                    </ItemTemplate>
                                                </asp:Repeater>

                                                <%-- Frequencia final --%>
                                                <td id="tdFrequenciaFinal" runat="server" style="text-align: center; border-top: 0px;" class="frequencia">
                                                    <asp:Label ID="lblFrequenciaFinalAjustada" runat="server" Style="margin: 0px;"></asp:Label>
                                                </td>

                                                <%-- Nota final --%>
                                                <td id="tdNotaFinal" runat="server" style="text-align: center; border-top: 0px;" class="colunaNota">
                                                    <div>
                                                        <asp:TextBox ID="txtNotaFinal" runat="server" SkinID="Decimal" MaxLength="6" Style="margin: 0px;"></asp:TextBox>
                                                        <asp:CustomValidator ID="cvNotaMaxima" runat="server" ControlToValidate="txtNotaFinal"
                                                            Display="Dynamic" OnServerValidate="cvNotaMaxima_Validar" Visible="false" ErrorMessage="">*</asp:CustomValidator>
                                                        <asp:DropDownList ID="ddlPareceresFinal" runat="server" Style="margin: 0px;">
                                                        </asp:DropDownList>
                                                        <asp:CustomValidator ID="cvParecerMaximo" runat="server" ControlToValidate="ddlPareceresFinal"
                                                            Display="Dynamic" OnServerValidate="cvParecerMaximo_Validar" Visible="false"
                                                            ErrorMessage="">*</asp:CustomValidator>
                                                        <asp:ImageButton ID="btnJustificativaNotaFinal" runat="server" SkinID="btDetalhar" Style="margin-bottom: -1px !important;"
                                                            CommandName="JustificativaNotaFinal" CausesValidation="false" />
                                                        <asp:Image ID="imgJustificativaNotaFinalSituacao" runat="server" SkinID="imgConfirmar" ToolTip="Justificativa preenchida"
                                                            Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                                                    </div>
                                                </td> 

                                                <%-- Parecer final --%>
                                                <td id="tdParecerFinal" runat="server" style="text-align: center; border-top: 0px;">
                                                    <asp:DropDownList ID="ddlParecerFinal" runat="server" Style="margin: 0px;"></asp:DropDownList>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </td>
                                <td style="padding: 0px 15px; background-color: white;">
                                    <asp:Label ID="lblParecerConclusivo" runat="server" Text="<%$ Resources:Mensagens, MSG_RESULTADOEFETIVACAO %>" Font-Bold="true"></asp:Label>
                                    <br />
                                    <br />
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlResultado" runat="server"></asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="imgJustificativaParecerConclusivo" runat="server" SkinID="btDetalhar" OnClick="imgJustificativaParecerConclusivo_Click"
                                                    ToolTip="Informar justificativa do parecer conclusivo" style="margin-left:5px;" />
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <br />
                                    <asp:Label ID="lblInseridoPor" runat="server" Text="Inserido por:" Font-Bold="true"></asp:Label>
                                    <br />
                                    <asp:Literal ID="litNomeDocente" runat="server"></asp:Literal>
                                    <br />
                                    <asp:Label ID="lblDataAlteracao" runat="server" Text="Em:" Font-Bold="true"></asp:Label>
                                    <br />
                                    <asp:Literal ID="litDataAlteracao" runat="server"></asp:Literal>
                                </td>
                            </tr>
                        </table>
                        <asp:HiddenField ID="hdnLocalImgCheckSituacao" runat="server" />
                        <asp:HiddenField ID="hdnTipoFechamento" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </fieldset>
        </div>
    </div>
    <div class="right">
        <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click" />
        <asp:Button ID="btnLimpar" runat="server" Text="Limpar" CausesValidation="false" OnClick="btnLimpar_Click" />
    </div>
</asp:Panel>

<div id="divJustificativaParecerConclusivo" title='Justificativa do parecer conclusivo' class="hide">
    <asp:UpdatePanel ID="updJustificativaParecerConclusivo" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <fieldset>
                <asp:Label ID="lblNomeAlunoJustificativa" runat="server"></asp:Label>
                <br />
                <br />
                <asp:Label ID="lblMsgJustificativaParecerConclusivo" runat="server" EnableViewState="False"></asp:Label>
                <asp:Label ID="lblJustificativaParecerConclusivo" runat="server" AssociatedControlID="txtJustificativaParecerConclusivo" Text="Justificativa do parecer conclusivo" EnableViewState="false"></asp:Label>
                <asp:TextBox ID="txtJustificativaParecerConclusivo" runat="server" TextMode="MultiLine" MaxLength="4000" SkinID="limite4000"></asp:TextBox>
                <div class="right">
                    <asp:Label ID="lblUsuarioJustificativa" runat="server" Style="float: left;"></asp:Label>
                    <asp:Button ID="btnSalvarJustificativaParecerConclusivo" runat="server" CausesValidation="false" Text="Salvar" OnClick="btnSalvarJustificativaParecerConclusivo_Click" />
                    <asp:Button ID="btnCancelarJustificativaParecerConclusivo" runat="server" CausesValidation="false" Text="Cancelar" OnClientClick='$("#divJustificativaParecerConclusivo").dialog("close"); return false;' />
                </div>
                <asp:Label ID="hdnJustificativa" runat="server" Visible="false" />
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
