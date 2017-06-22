<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCLancamentoFrequenciaTerritorio.ascx.cs" Inherits="GestaoEscolar.WebControls.LancamentoFrequencia.UCLancamentoFrequenciaTerritorio" %>

<%@ Register Src="~/WebControls/Combos/UCComboOrdenacao.ascx" TagName="UCComboOrdenacao" TagPrefix="uc2" %>

<div id="divListaoLancamentoFrequencia" runat="server" class="divListaoLancamentoFrequencia">
    <asp:Panel ID="pnlLancamentoFrequencias" runat="server">
        <asp:HiddenField ID="hdnOrdenacaoFrequencia" runat="server" />
        <table align="center" style="margin: 0 auto;">
            <tr>
                <td style="padding-top: 50px; width: 20px; vertical-align: bottom;" id="td_lkbAnterior" runat="server">
                    <asp:LinkButton Style="zoom: 140%; -moz-transform: scale(1.40);" ID="lkbAnterior" Text="|<" runat="server" OnClick="lkbAnterior_Click"
                        CssClass="ui-icon ui-icon-circle-triangle-w"></asp:LinkButton>
                </td>
                <td style="padding-top: 30px; vertical-align: bottom;">
                    <asp:Label ID="lblInicio" runat="server" Text="DD/MM/YYYY" AssociatedControlID="lkbProximo"
                        Font-Bold="True" class="lblInicio" />
                </td>
                <td style="padding-top: 45px; vertical-align: bottom;">
                    <asp:Label ID="Label3" runat="server" Text=" - " AssociatedControlID="lkbProximo"
                        Font-Bold="True" />
                </td>
                <td class="clear"></td>
                <td style="padding-top: 44px; vertical-align: bottom;">
                    <asp:Label ID="lblFim" runat="server" Text="DD/MM/YYYY" AssociatedControlID="lkbProximo"
                        Font-Bold="True" class="lblFim" />
                </td>
                <td style="padding-top: 50px; width: 50px; vertical-align: bottom;" id="td_lkbProximo" runat="server">
                    <asp:LinkButton Style="zoom: 140%; -moz-transform: scale(1.40);" ID="lkbProximo" Text=">|" runat="server" OnClick="lkbProximo_Click"
                        CssClass="ui-icon ui-icon-circle-triangle-e" />
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="upnFrequencia" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:Label Style="display: block" ID="lblMsgParecer" runat="server">
                </asp:Label>
                <asp:Label ID="_lblMsgRepeater" runat="server"></asp:Label>
                <%-- CssClass="summary"></asp:Label>--%>
                <uc2:UCComboOrdenacao ID="_UCComboOrdenacao1" runat="server" />
                <asp:Repeater ID="rptAlunosFrequencia" runat="server" OnItemDataBound="rptAlunosFrequencia_ItemDataBound" OnItemCommand="rptAlunosFrequencia_ItemCommand">
                    <HeaderTemplate>
                        <div class="divScrollResponsivo">
                            <table id="tabela" class="grid sortableFrequenciaTerritorio grid-responsive-list" cellspacing="0">
                                <thead>
                                    <tr class="gridHeader" style="height: 30px;">
                                        <th class="center">
                                            <asp:Label ID="_lblNumChamada" runat="server" Text='Nº Chamada'></asp:Label>
                                        </th>
                                        <th>
                                            <asp:Label ID="lblNome" runat="server" Text='Nome do aluno'></asp:Label>
                                        </th>
                                        <asp:Repeater ID="rptAulas" runat="server" OnItemDataBound="rptAulasHeader_ItemDataBound">
                                            <ItemTemplate>
                                                <th class="center {sorter :false} .sorterFalse">
                                                    <asp:Label ID="lbltau_id" runat="server" Text='<%#Bind("tau_id") %>' Visible="false">
                                                    </asp:Label>
                                                    <asp:Label ID="lblUsuId" runat="server" Text='<%#Bind("usu_id") %>' Visible="false"></asp:Label>
                                                    <asp:HiddenField ID="hdnPosicao"
                                                        runat="server" Value='<%#Bind("tdt_posicao") %>' />
                                                    <asp:HiddenField ID="hdnPermissaoAlteracao" runat="server" Value='<%#Bind("permissaoAlteracao") %>' />
                                                    <asp:Label ID="lblAulaReposicao" runat="server"></asp:Label>
                                                    <asp:Label ID="lbltnt_data" runat="server" Text='<%#Bind("tau_data","{0:dd/MM/yyyy}") %>'>
                                                    </asp:Label>                                                    
                                                </th>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <th id="thCompensacao" runat="server" class="{sorter :false}"
                                            style="width: 30px;">Compensação
                                        </th>
                                    </tr>
                                    <tr class="gridRow grid-linha-destaque">
                                        <td class="grid-responsive-no-header"></td>
                                        <td class="grid-responsive-no-header"></td>
                                        <asp:Repeater ID="rptAulasEfetivado" runat="server" OnItemDataBound="rptAulasHeader_ItemDataBound">
                                            <ItemTemplate>
                                                <td class="center {sorter :false} .sorterFalse grid-responsive-item-inline grid-responsive-center">
                                                    <asp:Label ID="lbltau_id" runat="server" Text='<%#Bind("tau_id") %>' Visible="false">
                                                    </asp:Label>
                                                    <asp:Label ID="lblUsuId" runat="server" Text='<%#Bind("usu_id") %>' Visible="false"></asp:Label>
                                                    <asp:HiddenField ID="hdnPosicao"
                                                        runat="server" Value='<%#Bind("tdt_posicao") %>' />
                                                    <asp:HiddenField ID="hdnPermissaoAlteracao" runat="server" Value='<%#Bind("permissaoAlteracao") %>' />
                                                    <asp:Label ID="lbltnt_data" runat="server" Text='<%#Bind("tau_data","{0:dd/MM/yyyy}") %>' style="display:none;">
                                                    </asp:Label>
                                                    <asp:Label ID="lblAulaReposicao" runat="server" style="display:none;"></asp:Label>
                                                    <asp:CheckBox ID="chkEfetivado" runat="server" Text="Efetivado" style="text-align:center;" />
                                                </td>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <td class="grid-responsive-no-header"></td>
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
                            <td id="tdNomeAluno" runat="server" class="td-relatorio">
                                <asp:Label ID="lblNome" runat="server" Text='<%#Bind("pes_nome") %>'>
                                </asp:Label>
                                <div class="dropdown-relatorio">
                                    <asp:LinkButton ID="btnRelatorioRP" runat="server" CausesValidation="False" CommandName="RelatorioRP"
                                        ToolTip="<%$ Resources:Academico, ControleTurma.Alunos.btnRelatorioRP.ToolTip %>" SkinID="btRelatorioRP" Visible="false" />
                                    <asp:LinkButton ID="btnRelatorioAEE" runat="server" CausesValidation="False" CommandName="RelatorioAEE"
                                        ToolTip="<%$ Resources:Academico, ControleTurma.Alunos.btnRelatorioAEE.ToolTip %>" SkinID="btRelatorioAEE" Visible="false" />
                                    <!-- botao dropdown -->
                                    <button title="Seleção de relatório" class="btn-dropdown-relatorio"></button>
                                </div>
                            </td>
                            <asp:Repeater ID="rptAulas" runat="server" OnItemDataBound="rptAulas_ItemDataBound">
                                <ItemTemplate>
                                    <td style="text-align: center;" runat="server" id="tdAulas" class="grid-responsive-item-inline grid-responsive-center">
                                        <div id="divAulasAluno" runat="server" class="divAulasAlunoTerritorio">
                                            <asp:Label ID="lbltau_id" runat="server" Text='<%#Bind("tau_id") %>' Visible="false">
                                            </asp:Label>
                                            <asp:Label ID="lblUsuId" runat="server" Text='<%#Bind("usu_id") %>' Visible="false"></asp:Label>
                                            <asp:HiddenField ID="hdnPosicao" runat="server" Value='<%#Bind("tdt_posicao") %>' />
                                            <asp:HiddenField ID="hdnPermissaoAlteracao" runat="server" Value='<%#Bind("permissaoAlteracao") %>' />
                                            <asp:CheckBoxList ID="cblFrequencia" runat="server" RepeatDirection="Horizontal"
                                                RepeatLayout="Flow" CssClass="cblFrequenciaTerritorios">
                                            </asp:CheckBoxList>
                                        </div>
                                    </td>
                                </ItemTemplate>
                            </asp:Repeater>
                            <td id="tdCompensacao" runat="server" style="text-align: center;">
                                <asp:ImageButton ID="btnDetalharCompensacao" runat="server" SkinID="btDetalhar" CommandName="DetalharCompensacao"
                                    CommandArgument='<%#Eval("alu_id")+";"+ Eval("mtu_id")+";"+Eval("mtd_id")%>' ToolTip="Detalhar compensação" />
                                <asp:Image ID="imgDetalharCompensacaoSituacao" runat="server" SkinID="imgConfirmar" ToolTip="Possui compensação"
                                    Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
                            </td>
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
                            <td id="tdNomeAluno" runat="server" class="td-relatorio">
                                <asp:Label ID="lblNome" runat="server" Text='<%#Bind("pes_nome") %>'>
                                </asp:Label>
                                <div class="dropdown-relatorio">
                                    <asp:LinkButton ID="btnRelatorioRP" runat="server" CausesValidation="False" CommandName="RelatorioRP"
                                        ToolTip="<%$ Resources:Academico, ControleTurma.Alunos.btnRelatorioRP.ToolTip %>" SkinID="btRelatorioRP" Visible="false" />
                                    <asp:LinkButton ID="btnRelatorioAEE" runat="server" CausesValidation="False" CommandName="RelatorioAEE"
                                        ToolTip="<%$ Resources:Academico, ControleTurma.Alunos.btnRelatorioAEE.ToolTip %>" SkinID="btRelatorioAEE" Visible="false" />
                                    <!-- botao dropdown -->
                                    <button title="Seleção de relatório" class="btn-dropdown-relatorio"></button>
                                </div>
                            </td>
                            <asp:Repeater ID="rptAulas" runat="server" OnItemDataBound="rptAulas_ItemDataBound">
                                <ItemTemplate>
                                    <td style="text-align: center;" runat="server" id="tdAulas" class="grid-responsive-item-inline grid-responsive-center">
                                        <div id="divAulasAluno" runat="server" class="divAulasAlunoTerritorio">
                                            <asp:Label ID="lbltau_id" runat="server" Text='<%#Bind("tau_id") %>' Visible="false">
                                            </asp:Label>
                                            <asp:Label ID="lblUsuId" runat="server" Text='<%#Bind("usu_id") %>' Visible="false"></asp:Label>
                                            <asp:HiddenField ID="hdnPosicao" runat="server" Value='<%#Bind("tdt_posicao") %>' />
                                            <asp:HiddenField ID="hdnPermissaoAlteracao" runat="server" Value='<%#Bind("permissaoAlteracao") %>' />
                                            <asp:CheckBoxList ID="cblFrequencia" runat="server" RepeatDirection="Horizontal"
                                                RepeatLayout="Flow" CssClass="cblFrequenciaTerritorios">
                                            </asp:CheckBoxList>
                                        </div>
                                    </td>
                                </ItemTemplate>
                            </asp:Repeater>
                            <td id="tdCompensacao" runat="server" style="text-align: center;">
                                <asp:ImageButton ID="btnDetalharCompensacao" runat="server" SkinID="btDetalhar" CommandName="DetalharCompensacao"
                                    CommandArgument='<%#Eval("alu_id")+";"+ Eval("mtu_id")+";"+Eval("mtd_id")%>' ToolTip="Detalhar compensação" />
                                <asp:Image ID="imgDetalharCompensacaoSituacao" runat="server" SkinID="imgConfirmar" ToolTip="Possui compensação"
                                    Width="16px" Height="16px" Visible="false" ImageAlign="Top" />
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

                <div>
                    <b>Legenda:</b>
                    <div style="border-style: solid; border-width: thin; width: 230px;">
                        <table id="tbLegendaFrequencia" runat="server" style="border-collapse: separate !important; border-spacing: 2px !important;">
                            <tr id="trExibirAlunoDispensadoListaoFreq" runat="server">
                                <td style="border-style: solid; border-width: thin; width: 25px; height: 15px;"></td>
                                <td>
                                    <asp:Literal runat="server" ID="litDispensado" Text="<%$ Resources:Mensagens, MSG_ALUNO_DISPENSADO %>"></asp:Literal>
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</div>
