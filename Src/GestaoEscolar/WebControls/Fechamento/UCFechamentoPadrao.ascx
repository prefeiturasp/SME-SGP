<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCFechamentoPadrao.ascx.cs" Inherits="GestaoEscolar.WebControls.Fechamento.UCFechamentoPadrao" %>

<%@ Register Src="../../WebControls/Combos/UCComboOrdenacao.ascx" TagName="UCComboOrdenacao" TagPrefix="uc2" %>

<asp:UpdatePanel ID="uppGridAlunos" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Label ID="lblMessageInfo" runat="server" EnableViewState="False"></asp:Label>       
        <div style="clear:both; padding-top:15px;">
            <div style="float: left; padding-top:10px;">
                <uc2:UCComboOrdenacao ID="_UCComboOrdenacao1" runat="server" />
            </div>
            <div style="float: right;" class="responsive-float-left">
                <asp:Label ID="lblQtdeAulasPrevistas" runat="server" Visible="false" style="display: inline-block;"></asp:Label>
                <asp:Label ID="lblTotalAulasExperiencia" runat="server" Visible="false" style="display: inline-block;"></asp:Label>
                <asp:Label ID="lblQtdeAulasDadas" runat="server" Visible="false" style="display: inline-block;"></asp:Label>
            </div>
        </div>
        <asp:Label ID="lblMessageInfo2" runat="server" EnableViewState="True" SkinID="SummaryRed"></asp:Label>
        <div class="divScrollResponsivo grid-collapse">
            <asp:GridView ID="gvAlunos" runat="server" AutoGenerateColumns="False" EmptyDataText="Não foram encontrados alunos na turma selecionada."
                OnRowDataBound="gvAlunos_RowDataBound" DataKeyNames="tur_id,tud_id,alu_id,mtu_id,mtd_id,AvaliacaoID,dispensadisciplina,alc_matricula,tur_codigo,mtd_numeroChamada,pes_nome"
                OnRowCommand="gvAlunos_RowCommand" SkinID="GridResponsive">
                <Columns>
                    <asp:TemplateField HeaderText="" HeaderStyle-CssClass="center" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Image ID="imgStatusFechamento" runat="server" Width="18" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$ Resources:UserControl, Fechamento.UCFechamentoPadrao.gvAlunos.ColunaNumeroChamada %>" HeaderStyle-CssClass="center" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblChamada" runat="server" Text='<%#Bind("mtd_numeroChamada") %>'>
                            </asp:Label>
                            <asp:HiddenField ID="hdnSituacao" runat="server" Value='<%# Bind("situacaoMatriculaAluno") %>' />
                            <asp:HiddenField ID="hdnDataMatricula" runat="server" Value='<%# Bind("dataMatricula") %>' />
                            <asp:HiddenField ID="hdnDataSaida" runat="server" Value='<%# Bind("dataSaida") %>' />
                            <asp:HiddenField ID="hdnDispensaDisciplina" runat="server" Value='<%# Bind("dispensadisciplina") %>' />
                            <asp:HiddenField ID="hdnFrequencia" runat="server" Value='<%# Bind("Frequencia") %>' />
                            <asp:HiddenField ID="hdnFrequenciaAjustada" runat="server" Value='<%# Bind("FrequenciaFinalAjustada") %>' />
                            <asp:HiddenField ID="hdnQtAulas" runat="server" Value='<%# Bind("QtAulasAluno") %>' />
                            <asp:HiddenField ID="hdnQtAulasReposicao" runat="server" Value='<%# Bind("QtAulasAlunoReposicao") %>' />
                            <asp:HiddenField ID="hdnQtFaltasReposicao" runat="server" Value='<%# Bind("QtFaltasAlunoReposicao") %>'></asp:HiddenField>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$ Resources:UserControl, Fechamento.UCFechamentoPadrao.gvAlunos.ColunaNomeAluno %>">
                        <ItemTemplate>
                            <asp:Label ID="lblAluno" runat="server" Text='<%#Bind("pes_nome") %>' CssClass="tamanho-lbl-aluno">
                            </asp:Label>
                            <asp:Label ID="lblNomeAluno" runat="server" Text='<%#Bind("pes_nome") %>' Visible="false">
                            </asp:Label>
                            <asp:LinkButton ID="btnRelatorioRP" runat="server" CausesValidation="False" CommandName="RelatorioRP"
                                ToolTip="<%$ Resources:Academico, ControleTurma.Alunos.btnRelatorioRP.ToolTip %>" SkinID="btRelatorioRP" Visible="false" />
                            <asp:LinkButton ID="btnRelatorioAEE" runat="server" CausesValidation="False" CommandName="RelatorioAEE"
                                ToolTip="<%$ Resources:Academico, ControleTurma.Alunos.btnRelatorioAEE.ToolTip %>" SkinID="btRelatorioAEE" Visible="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <%--Nota da avaliação normal--%>
                    <asp:TemplateField HeaderStyle-CssClass="center" ItemStyle-HorizontalAlign="Center">
                        <ItemStyle CssClass="colunaNota grid-responsive-item-inline" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <div>
                                <asp:TextBox ID="txtNota" runat="server" SkinID="Decimal" MaxLength="6"></asp:TextBox>
                                <asp:CustomValidator ID="cvNotaMaxima" runat="server" ControlToValidate="txtNota"
                                    Display="Dynamic" OnServerValidate="cvNotaMaxima_Validar" Visible="false" ErrorMessage="">*</asp:CustomValidator>
                                <asp:DropDownList ID="ddlPareceres" runat="server">
                                </asp:DropDownList>
                                <asp:CustomValidator ID="cvParecerMaximo" runat="server" ControlToValidate="ddlPareceres"
                                    Display="Dynamic" OnServerValidate="cvParecerMaximo_Validar" Visible="false"
                                    ErrorMessage="">*</asp:CustomValidator>
                            </div>
                            <div id="divLancarRelatorio" style="display: inline; padding: 0 0 0 0;">
                                <asp:ImageButton ID="btnRelatorio" runat="server" SkinID="btDetalhar" Style="margin-bottom: -1px !important;"
                                    CommandName="Relatorio" CausesValidation="false" />
                                <asp:Image ID="imgSituacao" runat="server" SkinID="imgConfirmar" ToolTip="Relatório lançado"
                                    Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                                <asp:HyperLink ID="hplAnexo" runat="server" SkinID="hplAnexo" ToolTip="Relatório anexo"
                                    Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                            </div>
                            <asp:HiddenField ID="hdnNota" runat="server" Value='<%# Bind("Avaliacao") %>' />
                        </ItemTemplate>
                        <HeaderStyle CssClass="center" HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <%-- Nota pós-conselho --%>
                    <asp:TemplateField HeaderText="Nota pós-conselho" HeaderStyle-CssClass="center" ItemStyle-HorizontalAlign="Center">
                        <ItemStyle CssClass="colunaNota grid-responsive-item-inline" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <div>
                                <asp:TextBox ID="txtNotaPosConselho" runat="server" SkinID="Decimal" MaxLength="6"></asp:TextBox>
                                <asp:CustomValidator ID="cvNotaPosConselhoMaxima" runat="server" ControlToValidate="txtNotaPosConselho"
                                    Display="Dynamic" OnServerValidate="cvNotaMaxima_Validar" Visible="false" ErrorMessage="">*</asp:CustomValidator>
                                <asp:DropDownList ID="ddlPareceresPosConselho" runat="server">
                                </asp:DropDownList>
                                <asp:CustomValidator ID="cvParecerPosConselhoMaximo" runat="server" ControlToValidate="ddlPareceresPosConselho"
                                    Display="Dynamic" OnServerValidate="cvParecerMaximo_Validar" Visible="false"
                                    ErrorMessage="">*</asp:CustomValidator>
                            </div>
                            <div id="divLancarRelatorioPosConselho" style="display: inline; padding: 0 0 0 0;">
                                <asp:ImageButton ID="btnRelatorioPosConselho" runat="server" SkinID="btDetalhar" Style="margin-bottom: -1px !important;"
                                    CommandName="Relatorio" CausesValidation="false" />
                                <asp:Image ID="imgSituacaoPosConselho" runat="server" SkinID="imgConfirmar" ToolTip="Relatório lançado"
                                    Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                                <asp:HyperLink ID="hplAnexoPosConselho" runat="server" SkinID="hplAnexo" ToolTip="Relatório anexo"
                                    Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                            </div>
                            <asp:HiddenField ID="hdnNotaPosConselho" runat="server" Value='<%# Bind("avaliacaoPosConselho") %>' />
                        </ItemTemplate>
                        <HeaderStyle CssClass="center" HorizontalAlign="Center" Width="200px" />
                    </asp:TemplateField>

                    <%-- Nota da avaliação normal e pós-conselho para disciplina de Regência --%>
                    <asp:TemplateField HeaderStyle-CssClass="center" ItemStyle-HorizontalAlign="Center">
                        <ItemStyle CssClass="grid-responsive-item-bottom"/>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnExpandir" runat="server" ToolTip=""
                                CssClass="ui-icon ui-icon-circle-triangle-s" OnClientClick="ExpandCollapse('.trExpandir', this); return false;" />
                            <input id="hfExpandido" runat="server" type="hidden" value="0" />
                        </ItemTemplate>
                        <HeaderTemplate>
                            <asp:Literal runat="server" ID="litNotaRegencia"></asp:Literal>
                            <asp:LinkButton ID="btnExpandir" runat="server" ToolTip="" Style="display: block; margin: auto;"
                                            CssClass="ui-icon ui-icon-circle-triangle-s" OnClientClick="ExpandCollapseAll(this); return false;" />
                            <input id="hfExpandidoTodos" runat="server" type="hidden" value="0" />
                        </HeaderTemplate>
                        <HeaderStyle CssClass="center" HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$ Resources:UserControl, Fechamento.UCFechamentoPadrao.gvAlunos.ColunaFaltas %>" HeaderStyle-CssClass="center" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:TextBox ID="txtQtdeFalta" runat="server" MaxLength="5" SkinID="Numerico2c">
                            </asp:TextBox>
                            <asp:ImageButton ID="btnFaltasExternas" runat="server" Visible="false" ToolTip="Exibir ausências de outras redes" SkinID="btDetalhar" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="center" />
                        <ItemStyle HorizontalAlign="Center" CssClass="colunaQtdeFalta grid-responsive-item-inline grid-responsive-header-block" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$ Resources:UserControl, Fechamento.UCFechamentoPadrao.gvAlunos.ColunaCompensacoes %>" HeaderStyle-CssClass="center"
                        ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:TextBox ID="txtAusenciasCompensadas" MaxLength="5" runat="server" SkinID="Numerico2c"
                                Text='<%#Bind("ausenciasCompensadas") %>'>
                            </asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle CssClass="center" />
                        <ItemStyle CssClass="colunaQtdeCompensacao grid-responsive-item-inline grid-responsive-header-block" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$ Resources:UserControl, Fechamento.UCFechamentoPadrao.gvAlunos.ColunaFrequenciaFinalAjustada %>" HeaderStyle-CssClass="center" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:TextBox ID="txtFrequenciaFinalAjustada" runat="server" SkinID="Decimal" MaxLength="6" Enabled="false"
                                Text='<%# string.Format(VS_FormatacaoDecimaisFrequencia , Convert.ToDecimal(Eval("FrequenciaFinalAjustada"))) %>'></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle CssClass="center" />
                        <ItemStyle HorizontalAlign="Center" CssClass="colunaFrequenciaAjustada grid-responsive-item-inline grid-responsive-header-block" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$ Resources:UserControl, Fechamento.UCFechamentoPadrao.gvAlunos.ColunaRegistroConselho %>" HeaderStyle-CssClass="center" ItemStyle-HorizontalAlign="Center">   
                        <ItemTemplate>
                            <div id="divObservacaoConselho" style="display: inline; padding: 0 0 0 0;">
                                <asp:ImageButton ID="btnObservacaoConselho" runat="server" SkinID="btDetalhar" Style="margin-bottom: -1px !important;"
                                    CommandName="ObservacaoConselho" CausesValidation="false" ToolTip="Preencher informações para o boletim do aluno" />
                                <asp:Image ID="imgObservacaoConselhoSituacao" runat="server" ToolTip="Observação preenchida"
                                    Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                            </div>
                        </ItemTemplate>
                        <HeaderStyle CssClass="center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderStyle-CssClass="center" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlResultado" runat="server"></asp:DropDownList>
                            <asp:HiddenField ID="hdnResultado" runat="server" Value='<%# Bind("AvaliacaoResultado") %>' />
                        </ItemTemplate>
                        <HeaderTemplate>
                            <asp:Label ID="lblTituloResultadoEfetivacao" runat="server" Text=""></asp:Label>
                        </HeaderTemplate>
                        <HeaderStyle CssClass="center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$ Resources:UserControl, Fechamento.UCFechamentoPadrao.gvAlunos.ColunaBoletim %>" HeaderStyle-CssClass="center" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnBoletim" runat="server" CausesValidation="false" CommandName="Boletim" 
                                SkinID="btRelatorio" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:TemplateField>
                        <ItemTemplate>
                            <tr class="trExpandir" style="display: none;">
                                <td align="left" colspan="100%" class="divExpandeLinha">
                                    <asp:Repeater ID="rptComponenteRegencia" runat="server" OnItemDataBound="rptComponenteRegencia_ItemDataBound" OnItemCommand="rptComponenteRegencia_ItemCommand">
                                        <HeaderTemplate>
                                            <table style="margin: 0px;" class="grid-responsive-list">
                                                <tr class="gridHeader" id="tr0" runat="server">
                                                    <th>
                                                        <asp:Literal ID="litHeadNomeDisciplina" runat="server"></asp:Literal></th>
                                                    <th>
                                                        <asp:Literal ID="litHeadNota" runat="server"></asp:Literal></th>
                                                    <th>
                                                        <asp:Literal ID="litHeadNotaPosConselho" runat="server"></asp:Literal></th>
                                                </tr>
                                        </HeaderTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <tr id="tr1" runat="server" class="gridRow">
                                                <td style="border-top: 0px;">
                                                    <asp:Label ID="lblNomeDisciplina" runat="server" Text='<%#Bind("dis_nome") %>' Font-Bold="true"></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hfDataKeys" />
                                                </td>
                                                
                                                <%-- Nota --%>
                                                <td class="colunaNota" style="border-top: 0px;">
                                                    <div>
                                                        <asp:TextBox ID="txtNota" runat="server" SkinID="Decimal" MaxLength="6"></asp:TextBox>
                                                        <asp:CustomValidator ID="cvNotaMaxima" runat="server" ControlToValidate="txtNota"
                                                            Display="Dynamic" OnServerValidate="cvNotaMaxima_Validar" Visible="false" ErrorMessage="">*</asp:CustomValidator>
                                                        <asp:DropDownList ID="ddlPareceres" runat="server">
                                                        </asp:DropDownList>
                                                        <asp:CustomValidator ID="cvParecerMaximo" runat="server" ControlToValidate="ddlPareceres"
                                                            Display="Dynamic" OnServerValidate="cvParecerMaximo_Validar" Visible="false"
                                                            ErrorMessage="">*</asp:CustomValidator>
                                                    </div>
                                                    <div id="divLancarRelatorio" style="display: inline; padding: 0 0 0 0;">
                                                        <asp:ImageButton ID="btnRelatorio" runat="server" SkinID="btDetalhar" Style="margin-bottom: -1px !important;"
                                                            CommandName="Relatorio" CausesValidation="false" />
                                                        <asp:Image ID="imgSituacao" runat="server" SkinID="imgConfirmar" ToolTip="Relatório lançado"
                                                            Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                                                        <asp:HyperLink ID="hplAnexo" runat="server" SkinID="hplAnexo" ToolTip="Relatório anexo"
                                                            Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                                                    </div>
                                                    <asp:HiddenField runat="server" ID="hdnAvaliacao" Value='<%# Bind("Avaliacao") %>' />
                                                </td>
                                                
                                                <%-- NotaPosConselho --%>
                                                <td class="colunaNota" style="border-top: 0px;">
                                                    <div>
                                                        <asp:TextBox ID="txtNotaPosConselho" runat="server" SkinID="Decimal" MaxLength="6"></asp:TextBox>
                                                        <asp:CustomValidator ID="cvNotaPosConselhoMaxima" runat="server" ControlToValidate="txtNotaPosConselho"
                                                            Display="Dynamic" OnServerValidate="cvNotaMaxima_Validar" Visible="false" ErrorMessage="">*</asp:CustomValidator>
                                                        <asp:DropDownList ID="ddlPareceresPosConselho" runat="server">
                                                        </asp:DropDownList>
                                                        <asp:CustomValidator ID="cvParecerPosConselhoMaximo" runat="server" ControlToValidate="ddlPareceresPosConselho"
                                                            Display="Dynamic" OnServerValidate="cvParecerMaximo_Validar" Visible="false"
                                                            ErrorMessage="">*</asp:CustomValidator>
                                                    </div>
                                                    <div id="divLancarRelatorioPosConselho" style="display: inline; padding: 0 0 0 0;">
                                                        <asp:ImageButton ID="btnRelatorioPosConselho" runat="server" SkinID="btDetalhar" Style="margin-bottom: -1px !important;"
                                                            CommandName="Relatorio" CausesValidation="false" />
                                                        <asp:Image ID="imgSituacaoPosConselho" runat="server" SkinID="imgConfirmar" ToolTip="Relatório lançado"
                                                            Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                                                        <asp:HyperLink ID="hplAnexoPosConselho" runat="server" SkinID="hplAnexo" ToolTip="Relatório anexo"
                                                            Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                                                    </div>
                                                    <asp:HiddenField runat="server" ID="hdnAvaliacaoPosConselho" Value='<%# Bind("avaliacaoPosConselho") %>' />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <HeaderStyle CssClass="hide" />
                        <ItemStyle CssClass="hide" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <br />
        <div id="divLegenda" runat="server" visible="false">
            <b>Legenda:</b>
            <div style="border-style: solid; border-width: thin;">
                <table id="tbLegenda" runat="server" style="border-collapse: separate !important; border-spacing: 2px !important;">
                    <tr runat="server" id="lnAlunoFrequencia">
                        <td style="border-style: solid; border-width: thin; width: 25px; height: 15px;"></td>
                        <td><asp:Literal runat="server" ID="litBaixaFreq" Text="<%$ Resources:Mensagens, MSG_ALUNO_BAIXA_FREQ %>"></asp:Literal>
                        </td>
                    </tr>
                    <tr runat="server" id="lnAlunoProximoBaixaFrequencia">
                        <td style="border-style: solid; border-width: thin; width: 25px; height: 15px;"></td>
                        <td><asp:Literal runat="server" ID="litProxBaixaFreq" Text="<%$ Resources:Mensagens, MSG_ALUNO_PROXIMO_BAIXA_FREQ %>"></asp:Literal>
                        </td>
                    </tr>
                    <tr runat="server" id="lnInativos">
                        <td style="border-style: solid; border-width: thin; width: 25px; height: 15px;"></td>
                        <td><asp:Literal runat="server" ID="litInativo" Text="<%$ Resources:Mensagens, MSG_ALUNO_INATIVO %>"></asp:Literal>
                        </td>
                    </tr>
                    <tr runat="server" id="lnAlunoDispensado">
                        <td style="border-style: solid; border-width: thin; width: 25px; height: 15px;"></td>
                        <td><asp:Literal runat="server" ID="litDispensado" Text="<%$ Resources:Mensagens, MSG_ALUNO_DISPENSADO %>"></asp:Literal>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <asp:HiddenField ID="hdnLocalImgCheckSituacao" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>


<!-- Confirma quais campos deseja atualizar -->
<div id="divFrequenciaExterna" runat="server" title="<%$ Resources:UserControl, EfetivacaoNotas.UCEfetivacaoNotas.divFrequenciaExterna.title %>" 
    class="hide divFrequenciaExterna">
    <fieldset>
        <asp:Label ID="Label2" Text="<b>Quantidade de aulas: </b>" runat="server"></asp:Label>
        <asp:Label ID="lblQtAulasExterna" Text="" runat="server"></asp:Label>
        <br />
        <br />
        <asp:Label ID="Label3" Text="<b>Quantidade de faltas: </b>" runat="server"></asp:Label>
        <asp:Label ID="lblQtFaltasExterna" Text="" runat="server"></asp:Label>
        <br />
        <div class="right">
            <asp:Button ID="btnFecharFreqExt" runat="server" Text="Voltar" OnClientClick="$('.divFrequenciaExterna').dialog('close'); return false;" />
        </div>
    </fieldset>
</div>
