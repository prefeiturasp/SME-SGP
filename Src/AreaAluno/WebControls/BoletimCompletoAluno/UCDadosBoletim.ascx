<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCDadosBoletim.ascx.cs" Inherits="AreaAluno.WebControls.BoletimCompletoAluno.UCDadosBoletim" %>

<asp:UpdatePanel ID="upnBoletim" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
        <div id="divBoletim" runat="server" visible="false">
            <asp:Label ID="lblMensagem" runat="server" Text="" EnableViewState="false"></asp:Label>
            <div style="overflow-x:auto;">
            <table rules="none" style="width: 100%;" class="boletimDefault">
                <tbody>
                    <tr>
                        <td>
                            <div class="divDados">
                                <table class="tblBoletim tblBoletimDetalhes" width="100%" rules="none">
                                    <tbody>
                                        <tr>
                                            <th class="linhaConceitoGlobal" colspan="3"><span>Boletim Escolar</span>
                                            </th>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <strong>Escola: </strong>
                                                <asp:Literal ID="litEscola" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <strong>Nome: </strong>
                                                <asp:Literal ID="litNome" runat="server"></asp:Literal>
                                            </td>
                                            <td>
                                                <strong>Número: </strong>
                                                <asp:Literal ID="litNumero" runat="server">00</asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 70%">
                                                <strong>
                                                    <%=MSTech.GestaoEscolar.BLL.GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id)%>: </strong>
                                                <asp:Literal ID="litCurso" runat="server"></asp:Literal>
                                            </td>
                                            <td style="width: 15%">
                                                <strong>Turma: </strong>
                                                <asp:Literal ID="litTurma" runat="server"></asp:Literal>
                                            </td>
                                            <td style="width: 15%">
                                                <strong>Ano: </strong>
                                                <asp:Literal ID="litAno" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="divResultados">
                                <div>
                                    <asp:Label runat="server" ID="lblNotasFaltas" Text="Notas e Faltas" class="itemBoletim"></asp:Label>
                                    <%--<span id="spnNotasFaltas" class="itemBoletim"><%#nomeNotas %> e Faltas</span>--%>
                                </div>
                                <table class="tblBoletim" rules="none">
                                    <thead>
                                        <tr>
                                            <th rowspan="2" class="nomePeriodo colPrincipal">
                                                <asp:Label runat="server" ID="lblDisp" Text="<%$ Resources:Mensagens, MSG_DISCIPLINA %>"></asp:Label>
                                            </th>
                                            <asp:Repeater ID="rptPeriodosNomes" runat="server">
                                                <ItemTemplate>
                                                    <th class="nomePeriodo colBimestre" colspan="3" title='<%#Eval("MatriculaPeriodo") %>' runat="server" visible='<%#mostraConceitoGlobal %>'>
                                                        <span><%#Eval("tpc_nome") %></span>
                                                    </th>
                                                    <th class="nomePeriodo colBimestre" colspan="2" title='<%#Eval("MatriculaPeriodo") %>' runat="server" visible='<%#!mostraConceitoGlobal %>'>
                                                        <span><%#Eval("tpc_nome") %></span>
                                                    </th>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            <th rowspan="2" class="nomePeriodo" id="thNotaFinal" runat="server">Síntese Final
                                            </th>
                                            <th rowspan="2" class="nomePeriodo">Total de Ausências
                                            </th>
                                            <th rowspan="2" class="nomePeriodo" id="thTotalComp" runat="server" visible="false">Total de Compensações
                                            </th>
                                            <th rowspan="2" class="nomePeriodo" id="thFreqFinal" runat="server" visible="false">Frequência Final(%)
                                            </th>
                                            <%--COLUNA CONCEITO FINAL--%>
                                            <%-- Nota final, é setada no código --%>
                                        </tr>
                                        <tr>
                                            <asp:Repeater ID="rptPeriodosColunasFixas" runat="server">
                                                <ItemTemplate>
                                                    <th class="nomePeriodoColunas" runat="server" visible='<%#mostraConceitoGlobal %>'>Conc.
                                                    </th>
                                                    <th class="nomePeriodoColunas"><%#nomeNota %>
                                                    </th>
                                                    <th class="nomePeriodoColunas">Faltas
                                                    </th>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptDisciplinas" runat="server" OnItemDataBound="rptDisciplinas_ItemDataBound">
                                            <ItemTemplate>
                                                <tr class='trDisciplina'>
                                                    <td id="tdNomeDiciplina" runat="server" class="nomeDisciplina">
                                                        <%#Eval("Disciplina")%>
                                                    </td>
                                                    <asp:Repeater ID="rptNotasDisciplina" OnItemDataBound="rptNotasDisciplina_ItemDataBound" runat="server" DataSource='<%#Eval("notas") %>'>
                                                        <ItemTemplate>
                                                            <td class="nota" runat="server" visible='<%#mostraConceitoGlobal %>'>
                                                                <%#Eval("nota.Conceito") %>
                                                            </td>
                                                            <td class="nota">
                                                                <%#Eval("nota.Nota")%>
                                                            </td>
                                                            <td class="nota" id="tdFaltas" runat="server">
                                                                <%#Eval("nota.numeroFaltas")%>
                                                            </td>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                    <td class="nota" id="tdNotaFinal" runat="server">
                                                        <asp:Literal runat="server" ID="litMediaFinal" Text='<%#Eval("MediaFinal")%>'></asp:Literal>
                                                    </td>
                                                    <td class="nota" id="tdTotFaltas" runat="server">
                                                        <asp:Literal runat="server" ID="litTotalFaltas" Text='<%#Eval("totalFaltas") %>'></asp:Literal>
                                                    </td>
                                                    <td class="nota" id="tdTotAusenciasCompensadas" runat="server" visible="false">
                                                        <asp:Literal runat="server" ID="litAusenciasCompensadas" Text='<%#Eval("ausenciasCompensadas")%>'></asp:Literal>
                                                    </td>
                                                    <td class="nota" id="tdTotFrequenciaAjustada" runat="server" visible="false">
                                                        <asp:Literal runat="server" ID="litFrequenciaAjustada"
                                                            Text='<%# Eval("FrequenciaFinalAjustada") %>'></asp:Literal>
                                                    </td>
                                                    <%--COLUNA CONCEITO FINAL--%>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <tr id="trParecerConclusivo" runat="server" class='trDisciplina'>
                                            <td class="nota tdParecerConclusivo" colspan="20">
                                                <asp:Literal ID="lblParecerConclusivo" runat="server" Text="Parecer conclusivo: " EnableViewState="false"></asp:Literal>
                                                <asp:Literal ID="lblParecerConclusivoResultado" runat="server" Text=""></asp:Literal>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>

                                <div id="divEnriquecimentoCurricular" runat="server">
                                    <table class="tblBoletim" rules="none">
                                        <thead>
                                            <tr>
                                                <th rowspan="2" class="nomePeriodo colPrincipal">
                                                    <asp:Label runat="server" ID="lblEnriquecimento" Text="<%$ Resources:UserControl, UCDadosBoletim.lblEnriquecimento.Text %>"></asp:Label>
                                                </th>
                                                <asp:Repeater ID="rptPeriodosNomesEnriquecimento" runat="server">
                                                    <ItemTemplate>
                                                        <th id="Th1" class="nomePeriodo colBimestre" title='<%#Eval("MatriculaPeriodo") %>' runat="server" visible='<%#mostraConceitoGlobal %>'>
                                                            <span><%#Eval("tpc_nome") %></span>
                                                        </th>
                                                        <th id="Th2" class="nomePeriodo colBimestre" title='<%#Eval("MatriculaPeriodo") %>' runat="server" visible='<%#!mostraConceitoGlobal %>'>
                                                            <span><%#Eval("tpc_nome") %></span>
                                                        </th>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                                <th rowspan="2" class="nomePeriodo">Total de Ausências
                                                </th>
                                                <th rowspan="2" class="nomePeriodo" id="thTotalCompEnriquecimento" runat="server" visible="false">Total de Compensações
                                                </th>
                                                <th rowspan="2" class="nomePeriodo" id="thFreqFinalEnriquecimento" runat="server" visible="false">Parecer Final
                                                </th>
                                            </tr>
                                            <tr>
                                                <asp:Repeater ID="rptPeriodosColunasFixasEnriquecimento" runat="server">
                                                    <ItemTemplate>

                                                        <th class="nomePeriodoColunas">Faltas
                                                        </th>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rptDisciplinasEnriquecimentoCurricular" runat="server" OnItemDataBound="rptDisciplinasEnriquecimentoCurricular_ItemDataBound">
                                                <ItemTemplate>
                                                    <tr class='trDisciplina'>
                                                        <td id="tdNomeDiciplina" runat="server" class="nomeDisciplina">
                                                            <asp:Literal ID="litDiscEnrCurricular" runat="server" Text='<%#Eval("Disciplina")%>'></asp:Literal>
                                                        </td>
                                                        <asp:Repeater ID="rptNotasDisciplina" OnItemDataBound="rptNotasDisciplina_ItemDataBound" runat="server" DataSource='<%#Eval("notas") %>'>
                                                            <ItemTemplate>

                                                                <td class="nota" id="tdFaltas" runat="server">
                                                                    <%#Eval("nota.numeroFaltas")%>
                                                                </td>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                        <td class="nota" id="tdTotFaltasEnriquec" runat="server">
                                                            <asp:Literal runat="server" ID="litTotalFaltas" Text='<%#Eval("totalFaltas") %>'></asp:Literal>
                                                        </td>
                                                        <td class="nota" id="tdTotAusenciasCompensadasEnriquec" runat="server" visible="false">
                                                            <asp:Literal runat="server" ID="litAusenciasCompensadasEnriquec" Text='<%#Eval("ausenciasCompensadas")%>'></asp:Literal>
                                                        </td>
                                                        <td class="nota" id="tdTotFrequenciaAjustadaEnriquec" runat="server">
                                                            <asp:Literal runat="server" ID="litFrequenciaAjustadaEnriquec" Text='<%#Eval("ParecerFinal")%>'></asp:Literal>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                                </div>
                                <div id="divTerritorioSaber" runat="server">
                                    <table class="tblBoletim" rules="none">
                                        <thead>
                                            <tr>
                                                <th runat="server" id="thTerritorioSaber" class="nomePeriodo colPrincipal">
                                                    <asp:Label runat="server" ID="lblTerritorioSaber" Text="Experiências pedagógicas"></asp:Label>
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rptLinhasTerritorio" runat="server">
                                                <ItemTemplate>
                                                    <tr class='trDisciplina'>
                                                        <asp:Repeater ID="rptItemTerritorio" runat="server" DataSource='<%#Eval("itemTerritorio") %>'>
                                                            <ItemTemplate>
                                                                <td id="tdNomeDiciplina" runat="server" class="nota" style="width:20%">
                                                                    <asp:Literal ID="litTerritorio" runat="server" Text='<%#Eval("territorio")%>'></asp:Literal>
                                                                </td>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                                </div>
                                <div id="divRecuperacao" runat="server">
                                    <table class="tblBoletim" rules="none">
                                        <thead>
                                            <tr>
                                                <th rowspan="2" class="nomePeriodo colPrincipal">
                                                    <asp:Label runat="server" ID="lblRecuperacaoTitulo" Text="<%$ Resources:UserControl, UCDadosBoletim.lblRecuperacaoTitulo.Text %>"></asp:Label>
                                                </th>
                                                <asp:Repeater ID="rptPeriodosNomesRecuperacao" runat="server">
                                                    <ItemTemplate>
                                                        <th id="Th1" class="nomePeriodo colBimestre" title='<%#Eval("MatriculaPeriodo") %>' runat="server" visible='<%#mostraConceitoGlobal %>'>
                                                            <span><%#Eval("tpc_nome") %></span>
                                                        </th>
                                                        <th id="Th2" class="nomePeriodo colBimestre" title='<%#Eval("MatriculaPeriodo") %>' runat="server" visible='<%#!mostraConceitoGlobal %>'>
                                                            <span><%#Eval("tpc_nome") %></span>
                                                        </th>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                                <th rowspan="2" class="nomePeriodo">Total de Ausências
                                                </th>

                                                <th rowspan="2" class="nomePeriodo" id="thFreqFinalRecuperacao" runat="server" visible="false">Parecer Final
                                                </th>
                                            </tr>
                                            <tr>
                                                <asp:Repeater ID="rptPeriodosColunasFixasRecuperacao" runat="server">
                                                    <ItemTemplate>

                                                        <th class="nomePeriodoColunas">Faltas
                                                        </th>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rptDisciplinasRecuperacao" runat="server">
                                                <ItemTemplate>
                                                    <tr class='trDisciplina'>
                                                        <td id="tdNomeDiciplina" runat="server" class="nomeDisciplina">
                                                            <%#Eval("Disciplina")%>
                                                        </td>
                                                        <asp:Repeater ID="rptNotasDisciplina" OnItemDataBound="rptNotasDisciplina_ItemDataBound" runat="server" DataSource='<%#Eval("notas") %>'>
                                                            <ItemTemplate>

                                                                <td class="nota" id="tdFaltas" runat="server">
                                                                    <%#Eval("nota.numeroFaltas")%>
                                                                </td>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                        <td class="nota" id="tdTotFaltasRecuperacao" runat="server">
                                                            <asp:Literal runat="server" ID="litTotalFaltas" Text='<%#Eval("totalFaltas") %>'></asp:Literal>
                                                        </td>

                                                        <td class="nota" id="tdTotFrequenciaAjustadaRecuperacao" runat="server">
                                                            <asp:Literal runat="server" ID="litFrequenciaAjustada" Text='<%#Eval("ParecerFinal")%>'></asp:Literal>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                            <div id="divDocenciaCompartilhada" runat="server">
                                <asp:Repeater ID="rptTudDocenciaCompartilhada" runat="server" OnItemDataBound="rptTudDocenciaCompartilhada_ItemDataBound">
                                    <ItemTemplate>
                                        <tr class='trDisciplina'>
                                            <td id="tdNomeDiciplina" runat="server" class="nomeDisciplina">
                                                <asp:Literal ID="litDocenciaCompartilhada" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
                </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>