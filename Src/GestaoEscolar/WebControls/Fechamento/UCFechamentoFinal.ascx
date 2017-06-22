<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCFechamentoFinal.ascx.cs" Inherits="GestaoEscolar.WebControls.Fechamento.UCFechamentoFinal" %>

<%@ Register Src="../../WebControls/Combos/UCComboOrdenacao.ascx" TagName="UCComboOrdenacao" TagPrefix="uc2" %>

<asp:UpdatePanel ID="uppAlunos" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Label ID="lblMessageInfo" runat="server" EnableViewState="False"></asp:Label>
        <div style="clear:both; padding-top:15px;">
            <uc2:UCComboOrdenacao ID="_UCComboOrdenacao1" runat="server" />
        </div>
        <asp:Label ID="lblMsgRepeater" runat="server" CssClass="summary" Text ="Não foram encontrados alunos na turma selecionada."></asp:Label>
        <asp:Repeater ID="rptAlunos" runat="server" OnItemDataBound="rptAlunos_ItemDataBound" OnItemCommand="rptAlunos_ItemCommand">
            <HeaderTemplate>
		        <div class="grid-collapse">
			        <table id="tabela" class="grid grid-responsive-list" cellspacing="0">
				        <thead>
					        <tr class="gridHeader" style="height: 60px;">
                                <th id="thStatus" runat="server"></th>
						        <th class="center">
                                    <asp:Literal ID="litChamada" runat="server" Text="<%$ Resources:UserControl, Fechamento.UCFechamentoPadrao.gvAlunos.ColunaNumeroChamada %>"></asp:Literal>
						        </th>
						        <th>
							        <asp:Literal ID="litNomeAluno" runat="server" Text="<%$ Resources:UserControl, Fechamento.UCFechamentoPadrao.gvAlunos.ColunaNomeAluno %>"></asp:Literal>
						        </th>
                                <asp:Repeater ID="rptHeaderPeriodos" runat="server" OnItemDataBound="rptHeaderPeriodos_ItemDataBound">
                                    <ItemTemplate>
                                        <th class="center {sorter :false} .sorterFalse">
                                            <asp:Literal ID="litHeadPeriodo" runat="server"></asp:Literal>
                                        </th>
                                    </ItemTemplate>
                                </asp:Repeater>
						        <th id="thNotaRegencia" runat="server" class="center {sorter :false}">
                                    <asp:Literal ID="litHeadNotaRegencia" runat="server"></asp:Literal>
                                    <asp:LinkButton ID="btnExpandir" runat="server" ToolTip="" style="display:block; margin:auto;"
                                        CssClass="ui-icon ui-icon-circle-triangle-s" OnClientClick="ExpandCollapseAll(this); return false;" />
                                    <input id="hfExpandidoTodos"  runat="server" type="hidden" value="0" />
						        </th>
                                <th id="thFrequenciaFinal" runat="server" class="center {sorter :false}">
                                    <asp:Literal ID="litFrequenciaFinalAjustada" runat="server" Text="<%$ Resources:UserControl, Fechamento.UCFechamentoPadrao.gvAlunos.ColunaFrequenciaFinalAjustada %>"></asp:Literal>
						        </th>
                                <th id="thNotaFinal" runat="server" class="center {sorter :false}" style="min-width:95px">
                                    <asp:Literal ID="litHeadNotaFinal" runat="server"></asp:Literal>
						        </th>
                                <th id="thParecerFinal" runat="server" class="center {sorter :false}">
                                    <asp:Literal ID="litHeadParecerFinal" runat="server" Text="<%$ Resources:UserControl, EfetivacaoNotas.UCEfetivacaoNotas.ParecerFinal %>"></asp:Literal>
						        </th>
                                <th id="thParecerConclusivo" runat="server" class="center {sorter :false}">
                                    <asp:Literal ID="litHeadParecerConclusivo" runat="server" Text="<%$ Resources:UserControl, Fechamento.UCFechamentoPadrao.gvAlunos.ColunaRegistroConselho %>"></asp:Literal>
						        </th>                        
                                <th id="thBoletim" runat="server" class="center {sorter :false}">
                                    <asp:Literal ID="litHeadBoletim" runat="server" Text="<%$ Resources:UserControl, Fechamento.UCFechamentoPadrao.gvAlunos.ColunaBoletim %>"></asp:Literal>
						        </th>
					        </tr>
				        </thead>
				        <tbody>
	        </HeaderTemplate>
            <ItemTemplate>
                <tr class="gridRow collapsibleContainer">
                    <td id="tdStatus" runat="server" style="text-align: center; border-top:0px;">
                        <asp:Image ID="imgStatusFechamento" runat="server" Width="18" />
                    </td>

			        <td id="tdNumeroChamada" runat="server" style="text-align: center; border-top:0px;">
				        <asp:Label ID="lblChamada" runat="server" Text='<%#Bind("mtd_numeroChamada") %>'></asp:Label>
                        <asp:HiddenField ID="hfAluId" runat="server" />
                        <asp:HiddenField ID="hfMtuId" runat="server" />
                        <asp:HiddenField ID="hfMtdId" runat="server" />
                        <asp:HiddenField ID="hfAvaliacaoId" runat="server" />
                        <asp:HiddenField ID="hfAlcMatricula" runat="server" />
			        </td>
			        <td id="tdNomeAluno" runat="server" style="border-top:0px;" class="td-relatorio">
				        <asp:Label ID="lblAluno" runat="server" Text='<%#Bind("pes_nome") %>' CssClass="tamanho-lbl-aluno"></asp:Label>
                        <asp:Label ID="lblNomeAluno" runat="server" Text='<%#Bind("pes_nome") %>' Visible="false"></asp:Label>
                        <div class="dropdown-relatorio">
                            <asp:LinkButton ID="btnRelatorioRP" runat="server" CausesValidation="False" CommandName="RelatorioRP"
                                ToolTip="<%$ Resources:Academico, ControleTurma.Alunos.btnRelatorioRP.ToolTip %>" SkinID="btRelatorioRP" Visible="false" />
                            <asp:LinkButton ID="btnRelatorioAEE" runat="server" CausesValidation="False" CommandName="RelatorioAEE"
                                ToolTip="<%$ Resources:Academico, ControleTurma.Alunos.btnRelatorioAEE.ToolTip %>" SkinID="btRelatorioAEE" Visible="false" />
                            <!-- botao dropdown -->
                            <button title="Seleção de relatório" class="btn-dropdown-relatorio"></button>
                        </div>
                    </td>
                        
                    <%-- Notas/Frequencia de todos os periodos --%>
                    <asp:Repeater ID="rptItemPeriodos" runat="server" OnItemDataBound="rptItemPeriodos_ItemDataBound">
                        <ItemTemplate>
                            <td style="text-align: center; border-top:0px;" runat="server" id="tdPeriodos">
                                <asp:HiddenField ID="hfAvaId" runat="server" />
                                <asp:HiddenField ID="hfQtFaltas" runat="server" />
                                <asp:HiddenField ID="hfQtAulas" runat="server" />
                                <asp:HiddenField ID="hfQtAusenciasCompensadas" runat="server" />
                                <asp:HiddenField ID="hdnFrequencia" runat="server" Value='<%# Bind("Frequencia") %>' />
                                <asp:HiddenField ID="hdnQtAulasReposicao" runat="server" Value='<%# Bind("QtAulasAlunoReposicao") %>' />
                                <asp:HiddenField ID="hdnQtFaltasReposicao" runat="server" Value='<%# Bind("QtFaltasAlunoReposicao") %>'></asp:HiddenField>
                                <div runat="server" id="divNotaPeriodo" style="padding:10px;">
                                    <asp:Label ID="lblFrequencia" runat="server" style="margin:0px;"></asp:Label>
                                    <asp:Label ID="lblNota" runat="server" style="margin:0px;"></asp:Label>
                                </div>
                                <div id="divLancarRelatorio" style="display: inline; padding: 0 0 0 0;">
                                    <asp:ImageButton ID="btnRelatorio" runat="server" SkinID="btDetalhar" Style="margin-bottom: -1px !important;"
                                        CommandName="Relatorio" CausesValidation="false" />
                                    <asp:Image ID="imgSituacao" runat="server" SkinID="imgConfirmar" ToolTip="Relatório lançado"
                                        Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                                    <asp:HyperLink ID="hplAnexo" runat="server" SkinID="hplAnexo" ToolTip="Relatório anexo"
                                        Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                                </div>
                            </td>
                        </ItemTemplate>
                    </asp:Repeater>

                    <%-- Notas de todos os periodos para disciplina de Regência --%>
			        <td id="tdNotaRegencia" runat="server" style="text-align: center; border-top:0px;" class="grid-responsive-item-bottom">
				        <asp:LinkButton ID="btnExpandir" runat="server" ToolTip="" style="display:block; margin:auto;"
                            CssClass="ui-icon ui-icon-circle-triangle-s" OnClientClick="ExpandCollapse('.trExpandir', this); return false;" />
                        <input id="hfExpandido"  runat="server" type="hidden" value="0" />
			        </td>

                    <%-- Frequencia final --%>
                    <td id="tdFrequenciaFinal" runat="server" style="text-align: center; border-top:0px;">
                        <asp:Label ID="lblFrequenciaFinalAjustada" runat="server" ></asp:Label>
			        </td>

                    <%-- Nota final --%>
                    <td id="tdNotaFinal" runat="server" style="text-align: center; border-top:0px;" class="colunaNota">
				        <div>
                            <asp:TextBox ID="txtNotaFinal" runat="server" SkinID="Decimal" MaxLength="6"></asp:TextBox>
                            <asp:CustomValidator ID="cvNotaMaxima" runat="server" ControlToValidate="txtNotaFinal"
                                Display="Dynamic" OnServerValidate="cvNotaMaxima_Validar" Visible="false" ErrorMessage="">*</asp:CustomValidator>
                            <asp:DropDownList ID="ddlPareceresFinal" runat="server">
                            </asp:DropDownList>
                            <asp:CustomValidator ID="cvParecerMaximo" runat="server" ControlToValidate="ddlPareceresFinal"
                                Display="Dynamic" OnServerValidate="cvParecerMaximo_Validar" Visible="false"
                                ErrorMessage="">*</asp:CustomValidator>
                        </div>
			        </td>

                    <%-- Parecer final --%>
                    <td id="tdParecerFinal" runat="server" style="text-align: center; border-top:0px;">
				        <asp:DropDownList ID="ddlResultado" runat="server"></asp:DropDownList>
			        </td>

                    <%-- Registro do Conselho de Classe --%>
                    <td id="tdParecerConclusivo" runat="server" style="text-align: center; border-top:0px;">
				        <div id="divObservacaoConselho" style="display: inline; padding: 0 0 0 0;">
                            <asp:ImageButton ID="btnObservacaoConselho" runat="server" SkinID="btDetalhar" Style="margin-bottom: -1px !important;"
                                CommandName="ObservacaoConselho" CausesValidation="false" ToolTip="Preencher informações para o boletim do aluno" />
                            <asp:Image ID="imgObservacaoConselhoSituacao" runat="server" ToolTip="Observação preenchida"
                                Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                        </div>
			        </td>

                    <%-- Boletim --%>
                    <td id="tdBoletim" runat="server" style="text-align: center; border-top:0px;">
				        <asp:ImageButton ID="btnBoletim" runat="server" CausesValidation="False" CommandName="Boletim" SkinID="btRelatorio" />
			        </td>
		        </tr>
                <%-- Notas/Frequencia de todos os periodos para disciplina de Regência expandido --%>
                <tr class="trExpandir" style="display: none;">
                    <td align="left" colspan="100%" class="divExpandeLinha" style="border-top:0px;">
                        <asp:Repeater ID="rptComponenteRegencia" runat="server" OnItemDataBound="rptComponenteRegencia_ItemDataBound" OnItemCommand="rptComponenteRegencia_ItemCommand">
                            <HeaderTemplate>
                                <table style="margin:0px;" class="grid-responsive-list">
                                    <tr class="gridHeader">
                                        <th>                                                            
                                            <asp:Literal ID="litHeadNomeDisciplina" runat="server" Text="<%$ Resources:UserControl, EfetivacaoNotas.UCEfetivacaoNotas.litHeadNomeDisciplina.Text %>"></asp:Literal>
                                        </th>
                                        <asp:Repeater ID="rptHeaderPeriodos" runat="server" OnItemDataBound="rptHeaderPeriodos_ItemDataBound">
                                            <ItemTemplate>
                                                <th class="center">
                                                    <asp:Literal ID="litHeadPeriodo" runat="server"></asp:Literal>
                                                </th>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <th id="thNotaFinal" runat="server" class="center">
                                            <asp:Literal ID="litHeadNotaFinal" runat="server"></asp:Literal>
						                </th>
                                    </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr class="gridRow">
                                    <td style="border-top:0px;">
                                        <asp:Label ID="lblNomeDisciplina" runat="server" Text='<%#Bind("dis_nome") %>' Font-Bold="true"></asp:Label>
                                        <asp:HiddenField ID="hfTudId" runat="server" />
                                        <asp:HiddenField ID="hfMtdId" runat="server" />
                                        <asp:HiddenField ID="hfAvaliacaoId" runat="server" />
                                    </td>

                                    <%-- Notas de todos os periodos --%>
                                    <asp:Repeater ID="rptItemPeriodos" runat="server" OnItemDataBound="rptItemPeriodos_ItemDataBound">
                                        <ItemTemplate>
                                            <td style="text-align: center; border-top:0px;" class="colunaNota grid-responsive-item-inline grid-responsive-col4">
                                                <asp:HiddenField ID="hfAvaId" runat="server" />
                                                <div runat="server" id="divNotaPeriodo" style="padding:10px;">
                                                    <asp:Label ID="lblNota" runat="server" style="margin:0px;"></asp:Label>
                                                </div>
                                                <div id="divLancarRelatorio" style="display: inline; padding: 0 0 0 0;">
                                                    <asp:ImageButton ID="btnRelatorio" runat="server" SkinID="btDetalhar" Style="margin-bottom: -1px !important;"
                                                        CommandName="Relatorio" CausesValidation="false" />
                                                    <asp:Image ID="imgSituacao" runat="server" SkinID="imgConfirmar" ToolTip="Relatório lançado"
                                                        Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                                                    <asp:HyperLink ID="hplAnexo" runat="server" SkinID="hplAnexo" ToolTip="Relatório anexo"
                                                        Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                                                </div>
                                            </td>
                                        </ItemTemplate>
                                    </asp:Repeater>

                                    <%-- Nota final --%>
                                    <td id="tdNotaFinal" runat="server" style="text-align: center; border-top:0px;" class="colunaNota">
                                        <div>
                                            <asp:TextBox ID="txtNotaFinal" runat="server" SkinID="Decimal" MaxLength="6"></asp:TextBox>
                                            <asp:CustomValidator ID="cvNotaMaxima" runat="server" ControlToValidate="txtNotaFinal"
                                                Display="Dynamic" OnServerValidate="cvNotaMaxima_Validar" Visible="false" ErrorMessage="">*</asp:CustomValidator>
                                            <asp:DropDownList ID="ddlPareceresFinal" runat="server">
                                            </asp:DropDownList>
                                            <asp:CustomValidator ID="cvParecerMaximo" runat="server" ControlToValidate="ddlPareceresFinal"
                                                Display="Dynamic" OnServerValidate="cvParecerMaximo_Validar" Visible="false"
                                                ErrorMessage="">*</asp:CustomValidator>
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>    
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                        </tbody>
                    </table>
                </div>
            </FooterTemplate>
        </asp:Repeater>
        <br />
        <br />
        <div id="divLegenda" runat="server" visible="false">
            <b>Legenda:</b>
            <div style="border-style: solid; border-width: thin;">
                <table id="tbLegenda" runat="server" style="border-collapse: separate !important; border-spacing: 2px !important;">
                    <tr id="lnNotaConselho" runat="server">
                        <td style="border-style: solid; border-width: thin; width: 25px; height: 15px;"></td>
                        <td><asp:Literal runat="server" ID="litLegendaNotaConselho" ></asp:Literal></td>
                    </tr>
                    <tr id="lnAlunoForaDaRede">
                        <td style="border-style: solid; border-width: thin; width: 25px; height: 15px;"></td>
                        <td><asp:Literal runat="server" ID="litLegendaAlunoForaDaRede" ></asp:Literal></td>
                    </tr>
                </table>
            </div>
        </div>
        <asp:HiddenField ID="hdnLocalImgCheckSituacao" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>