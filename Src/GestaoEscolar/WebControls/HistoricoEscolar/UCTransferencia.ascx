<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCTransferencia.ascx.cs" Inherits="GestaoEscolar.WebControls.HistoricoEscolar.UCTransferencia" %>

<asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>

<asp:Panel ID="pnlNotasBimestrais" runat="server" GroupingText="<%$ Resources:UserControl, UCTransferencia.pnlNotasBimestrais.GroupingText %>">
    <asp:Label ID="lblMessagePanel" runat="server" EnableViewState="false"></asp:Label>
    <div id="divConteudoNotasBimestrais" runat="server">
        <!-- Notas/Frequencia Disciplinas -->
        <asp:HiddenField ID="hdnVariacaoFrequencia" runat="server" />
        <br />
        <table class="tblBoletim" rules="none">
            <thead>
                <tr>
                    <th rowspan="2" class="nomePeriodo colPrincipal">
                        <asp:Label runat="server" ID="lblDisp" Text="<%$ Resources:Mensagens, MSG_DISCIPLINA %>"></asp:Label>
                    </th>
                    <asp:Repeater ID="rptPeriodosNomes" runat="server">
                        <ItemTemplate>
                            <th id="Th1" class="nomePeriodo" colspan="3" title='<%#Eval("MatriculaPeriodo") %>' runat="server" visible='<%#mostraConceitoGlobal %>'>
                                <span><%#Eval("tpc_nome") %></span>
                            </th>
                            <th id="Th2" class="nomePeriodo" colspan="2" title='<%#Eval("MatriculaPeriodo") %>' runat="server" visible='<%#!mostraConceitoGlobal %>'>
                                <span><%#Eval("tpc_nome") %></span>
                            </th>
                        </ItemTemplate>
                    </asp:Repeater>
                </tr>
                <tr>
                    <asp:Repeater ID="rptPeriodosColunasFixas" runat="server">
                        <ItemTemplate>
                            <th id="Th3" class="nomePeriodoColunas" runat="server" visible='<%#mostraConceitoGlobal %>'>Conc.
                            </th>
                            <th class="nomePeriodoColunas"><%#nomeNota %>
                            </th>
                            <th class="nomePeriodoColunas">% Freq.
                            </th>
                        </ItemTemplate>
                    </asp:Repeater>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rptDisciplinas" runat="server" OnItemDataBound="rptDisciplinas_ItemDataBound">
                    <ItemTemplate>
                        <asp:HiddenField ID="hdnAluId" runat="server" Value='<%#Eval("alu_id") %>' />
                        <asp:HiddenField ID="hdnMtuId" runat="server" Value='<%#Eval("mtu_id") %>' />
                        <asp:HiddenField ID="hdnMtdId" runat="server" Value='<%#Eval("mtd_id") %>' />
                        <asp:HiddenField ID="hdnTudId" runat="server" Value='<%#Eval("tud_id") %>' />
                        <tr class='trDisciplina'>
                            <td id="tdNomeDiciplina" runat="server" class="nomeDisciplina">
                                <%#Eval("Disciplina")%>
                            </td>
                            <asp:Repeater ID="rptNotasDisciplina" OnItemDataBound="rptNotasDisciplina_ItemDataBound" runat="server" DataSource='<%#Eval("notas") %>'>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnFavId" runat="server" Value='<%#Eval("fav_id") %>' />
                                    <asp:HiddenField ID="hdnAvaId" runat="server" Value='<%#Eval("ava_id") %>' />
                                    <asp:HiddenField ID="hdnNotaId" runat="server" Value='<%#Eval("nota.NotaId") %>' />
                                    <asp:HiddenField ID="hdnEsaTipo" runat="server" Value='<%#Eval("nota.esa_tipo") %>' />
                                    <asp:HiddenField ID="hdnPermiteEditar" runat="server" Value='<%#Eval("nota.PermiteEditar") %>' />
                                    <asp:HiddenField ID="hdnTudIdRegencia" runat="server" Value='<%#Eval("nota.TudIdRegencia") %>' />
                                    <asp:HiddenField ID="hdnMtdIdRegencia" runat="server" Value='<%#Eval("nota.MtdIdRegencia") %>' />
                                    <asp:HiddenField ID="hdnAtdIdRegencia" runat="server" Value='<%#Eval("nota.AtdIdRegencia") %>' />
                                    <td id="Td1" class="nota" runat="server" visible='<%#mostraConceitoGlobal %>'>
                                        <%#Eval("nota.Conceito") %>
                                    </td>
                                    <td class="nota">
                                        <asp:Literal ID="litNota" runat="server" Text='<%#Eval("nota.Nota") %>'></asp:Literal>
                                        <div class="notaNumericaTransferencia">
                                            <asp:TextBox ID="txtNota" runat="server" Text='<%#Eval("nota.Nota") %>' Visible="false" SkinID="Decimal" MaxLength="6"></asp:TextBox>
                                        </div>
                                        <asp:DropDownList ID="ddlNota" runat="server" Visible="false" SkinID="text30C"></asp:DropDownList>
                                    </td>
                                    <td class="nota" id="tdFrequencia" runat="server">
                                        <asp:Literal ID="litFrequencia" runat="server" Text='<%#Eval("nota.Frequencia")%>'></asp:Literal>
                                        <asp:HiddenField ID="hdnNumeroFaltas" runat="server" Value='<%#Eval("nota.numeroFaltas")%>' />
                                        <asp:HiddenField ID="hdnNumeroAulas" runat="server" Value='<%#Eval("nota.numeroAulas")%>' />
                                        <asp:HiddenField ID="hdnAusenciasCompensadas" runat="server" Value='<%#Eval("nota.ausenciasCompensadas")%>' />
                                        <asp:HiddenField ID="hdnFrequenciaFinalAjustada" runat="server" Value='<%#Eval("nota.FrequenciaFinalAjustada")%>' />
                                    </td>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <br /><br />
        <!-- Disciplinas de enriquecimento curricular -->
        <div id="divEnriquecimentoCurricular" runat="server">
            <table class="tblBoletim" rules="none">
                <thead>
                    <tr>
                        <th rowspan="2" class="nomePeriodo colPrincipal">
                            <asp:Label runat="server" ID="lblEnriquecimento" Text="<%$ Resources:UserControl, UCTransferencia.lblEnriquecimento.Text %>"></asp:Label>
                        </th>
                        <asp:Repeater ID="rptPeriodosNomesEnriquecimento" runat="server">
                            <ItemTemplate>
                                <th id="Th1" class="nomePeriodo" title='<%#Eval("MatriculaPeriodo") %>' runat="server" visible='<%#mostraConceitoGlobal %>'>
                                    <span><%#Eval("tpc_nome") %></span>
                                </th>
                                <th id="Th2" class="nomePeriodo" title='<%#Eval("MatriculaPeriodo") %>' runat="server" visible='<%#!mostraConceitoGlobal %>'>
                                    <span><%#Eval("tpc_nome") %></span>
                                </th>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tr>
                    <tr>
                        <asp:Repeater ID="rptPeriodosColunasFixasEnriquecimento" runat="server">
                            <ItemTemplate>

                                <th class="nomePeriodoColunas">% Freq.
                                </th>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptDisciplinasEnriquecimentoCurricular" runat="server" OnItemDataBound="rptDisciplinasEnriquecimentoCurricular_ItemDataBound">
                        <ItemTemplate>
                            <asp:HiddenField ID="hdnAluId" runat="server" Value='<%#Eval("alu_id") %>' />
                            <asp:HiddenField ID="hdnMtuId" runat="server" Value='<%#Eval("mtu_id") %>' />
                            <asp:HiddenField ID="hdnMtdId" runat="server" Value='<%#Eval("mtd_id") %>' />
                            <asp:HiddenField ID="hdnTudId" runat="server" Value='<%#Eval("tud_id") %>' />
                            <tr class='trDisciplina'>
                                <td id="tdNomeDiciplina" runat="server" class="nomeDisciplina">
                                    <%#Eval("Disciplina")%>
                                </td>
                                <asp:Repeater ID="rptNotasDisciplina" OnItemDataBound="rptNotasDisciplina_ItemDataBound" runat="server" DataSource='<%#Eval("notas") %>'>
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnFavId" runat="server" Value='<%#Eval("fav_id") %>' />
                                        <asp:HiddenField ID="hdnAvaId" runat="server" Value='<%#Eval("ava_id") %>' />
                                        <asp:HiddenField ID="hdnNotaId" runat="server" Value='<%#Eval("nota.NotaId") %>' />
                                        <asp:HiddenField ID="hdnPermiteEditar" runat="server" Value='<%# Convert.ToBoolean(Eval("nota.PermiteEditar")) && Convert.ToBoolean(Eval("nota.PermiteEdicaoDocente")) %>' />
                                        <td class="nota" id="tdFrequencia" runat="server">
                                            <asp:Literal ID="litFrequencia" runat="server" Text='<%#Eval("nota.Frequencia")%>'></asp:Literal>
                                            <asp:HiddenField ID="hdnNumeroFaltas" runat="server" Value='<%#Eval("nota.numeroFaltas")%>' />
                                            <asp:HiddenField ID="hdnNumeroAulas" runat="server" Value='<%#Eval("nota.numeroAulas")%>' />
                                            <asp:HiddenField ID="hdnAusenciasCompensadas" runat="server" Value='<%#Eval("nota.ausenciasCompensadas")%>' />
                                            <asp:HiddenField ID="hdnFrequenciaFinalAjustada" runat="server" Value='<%#Eval("nota.FrequenciaFinalAjustada")%>' />
                                        </td>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>

        <br /><br />
        <!-- Projetos/atividades complementares -->
        <div id="divProjetos" runat="server">
            <table class="tblBoletim" rules="none">
                <thead>
                    <tr>
                        <th rowspan="2" class="nomePeriodo colPrincipal">
                            <asp:Label runat="server" ID="lblProjetos" Text="<%$ Resources:UserControl, UCTransferencia.lblProjetos.Text %>"></asp:Label>
                        </th>
                        <asp:Repeater ID="rptPeriodosNomesProjeto" runat="server">
                            <ItemTemplate>
                                <th id="Th1" class="nomePeriodo" title='<%#Eval("MatriculaPeriodo") %>' runat="server" visible='<%#mostraConceitoGlobal %>'>
                                    <span><%#Eval("tpc_nome") %></span>
                                </th>
                                <th id="Th2" class="nomePeriodo" title='<%#Eval("MatriculaPeriodo") %>' runat="server" visible='<%#!mostraConceitoGlobal %>'>
                                    <span><%#Eval("tpc_nome") %></span>
                                </th>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tr>
                    <tr>
                        <asp:Repeater ID="rptPeriodosColunasFixasProjeto" runat="server">
                            <ItemTemplate>

                                <th class="nomePeriodoColunas">% Freq.
                                </th>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptDisciplinasProjeto" runat="server">
                        <ItemTemplate>
                            <asp:HiddenField ID="hdnAluId" runat="server" Value='<%#Eval("alu_id") %>' />
                            <tr class='trDisciplina'>
                                <td id="tdNomeDiciplina" runat="server" class="nomeDisciplina">
                                    <%#Eval("Disciplina")%>
                                </td>
                                <asp:Repeater ID="rptNotasDisciplina" OnItemDataBound="rptNotasDisciplinaProjeto_ItemDataBound" runat="server" DataSource='<%#Eval("notas") %>'>
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnProjetoId" runat="server" Value='<%#Eval("nota.ProjetoId") %>' />
                                        <asp:HiddenField ID="hdnNotaProjetoId" runat="server" Value='<%#Eval("nota.NotaProjetoId") %>' />
                                        <asp:HiddenField ID="hdnTpcId" runat="server" Value='<%#Eval("tpc_id") %>' />
                                        <td class="frequenciaTransferencia" id="tdFrequencia" runat="server" align="center">
                                            <asp:TextBox ID="txtFrequencia" runat="server" Text='<%#Eval("nota.Frequencia") %>' SkinID="Decimal" CssClass="frequenciaTransferencia" ></asp:TextBox>
                                        </td>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
    </div>
    <div class="right">
        <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click" />
        <asp:Button ID="btnVisualizar" runat="server" Text="Visualizar histórico" OnClick="btnVisualizar_Click" CausesValidation="false" />
        <asp:Button ID="btnVoltar" runat="server" Text="Voltar" OnClick="btnVoltar_Click" CausesValidation="false" />
    </div>
</asp:Panel>
