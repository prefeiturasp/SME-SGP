<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCAlunoBoletimEscolar.ascx.cs"
    Inherits="GestaoEscolar.WebControls.AlunoBoletimEscolar.UCAlunoBoletimEscolar" %>
<style type="text/css">
    th.linhaConceitoGlobal
    {
        background: #ddd;
        font-weight: bold;
        text-align: center;
    }
    
    th.nomePeriodo
    {
        font-weight: bold;
        text-align: center;
        background: #EEE;
    }
    
    th.nomePeriodoColunas
    {
        text-align: center;
        background: #EEE;
    }
    
    tr.trDisciplina td.nota
    {
        text-align: center;
    }
    
    tr.trDisciplina.global td
    {
        background: #ddd;
    }
</style>
<fieldset id="fsSemBoletim" runat="server" visible="false">
    <asp:Label ID="lblSemBoletim" runat="server" EnableViewState="false"></asp:Label>
</fieldset>
<fieldset id="fdsBoletim" style="margin-right: 10px;" runat="server">
    <legend>Boletim escolar</legend>
    <asp:Label ID="lblMensagem" runat="server" Text="" EnableViewState="false"></asp:Label>
    <table class="tblBoletim" rules="none">
        <tbody>
            <tr>
                <th colspan="100%" class="linhaConceitoGlobal">
                    BOLETIM ESCOLAR
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
                    <strong>Nome do aluno: </strong>
                    <asp:Literal ID="litNome" runat="server"></asp:Literal>
                </td>
                <td>
                    <strong>Número: </strong>
                    <asp:Literal ID="litNumero" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td>
                    <strong>
                        <%=MSTech.GestaoEscolar.BLL.GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id) %>: </strong>
                    <asp:Literal ID="litCurso" runat="server"></asp:Literal>
                </td>
                <td>
                    <strong>Turma: </strong>
                    <asp:Literal ID="litTurma" runat="server"></asp:Literal>
                </td>
                <td>
                    <strong>Ano: </strong>
                    <asp:Literal ID="litAno" runat="server"></asp:Literal>
                </td>
            </tr>
        </tbody>
    </table>
    <br />
    <table class="tblBoletim" rules="none">
        <thead>
            <tr>
                <th colspan="100%" class="linhaConceitoGlobal">
                    MÉDIAS BIMESTRAIS
                </th>
            </tr>
            <tr>
                <th rowspan="2" class="nomePeriodo" style="min-width: 110px;">
                    <asp:Label runat="server" ID="lblDisp" Text="<%$ Resources:Mensagens, MSG_DISCIPLINA %>"></asp:Label>
                </th>
                <asp:Repeater ID="rptPeriodosNomes" runat="server">
                    <ItemTemplate>
                        <th class="nomePeriodo" colspan="4" title='<%#Eval("MatriculaPeriodo") %>'>
                            <%#Eval("tpc_nome") %>
                        </th>
                    </ItemTemplate>
                </asp:Repeater>
                <th runat="server" id="coluna5COC" class="nomePeriodo">
                    5º
                    <%=MSTech.GestaoEscolar.BLL.GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id) %>
                </th>
                <th rowspan="2" style="width: 50px;" class="nomePeriodo">
                    Total de faltas
                </th>
            </tr>
            <tr>
                <asp:Repeater ID="rptPeriodosColunasFixas" runat="server">
                    <ItemTemplate>
                        <th class="nomePeriodoColunas">
                            Conc.
                        </th>
                        <th class="nomePeriodoColunas">
                            Nota
                        </th>
                        <th class="nomePeriodoColunas">
                            Faltas
                        </th>
                        <th class="nomePeriodoColunas">
                            Rec.
                        </th>
                    </ItemTemplate>
                </asp:Repeater>
                <th class="nomePeriodoColunas" style="width: 50px;" id="coluna5COCNota" runat="server">
                    Nota
                </th>
            </tr>
        </thead>
        <tbody>
            <asp:Repeater ID="rptDisciplinas" runat="server">
                <ItemTemplate>
                    <tr class='<%#classeLinhaDisciplina((bool)Eval("tud_global")) %>'>
                        <td id="tdNomeDiciplina" runat="server" class="nomeDisciplina">
                            <%#Eval("nomeDisciplina")%>
                        </td>
                        <asp:Repeater ID="rptNotasDisciplina" runat="server" DataSource='<%#Eval("notas") %>'>
                            <ItemTemplate>
                                <td class="nota">
                                    <%#Eval("nota.Conceito") %>
                                </td>
                                <td class="nota">
                                    <%#Eval("nota.Nota")%>
                                </td>
                                <td class="nota">
                                    <%#Eval("nota.numeroFaltas")%>
                                </td>
                                <td class="nota">
                                    <%#Eval("nota.NotaRP")%>
                                </td>
                            </ItemTemplate>
                        </asp:Repeater>
                        <td id="Td1" class="nota" runat="server" visible='<%#mostra5COC %>'>
                            <%#Eval("NotaRecEsp")%>
                        </td>
                        </td>
                        <td class="nota">
                            <%#Eval("totalFaltas") %>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
        <tfoot>
            <tr>
                <td colspan="100%">
                </td>
            </tr>
            <tr>
                <td colspan="100%" style="text-align: center;">
                    <asp:Literal ID="litFreqFrase" runat="server" Text="O aluno obteve uma frequência de "/>
                    <asp:Literal ID="litFrequenciaAcumulada" runat="server"></asp:Literal>
                    <asp:Literal ID="litFreqPorc" runat="server" Text=" %"/>
                </td>
            </tr>
        </tfoot>
    </table>
    <br />
    <br />
    <div id="divAvaliacoesSecretaria" runat="server">
        <table class="tblBoletim" rules="none">
            <thead>
                <tr>
                    <th colspan="100%" class="linhaConceitoGlobal">
                        PROVA BIMESTRAL
                    </th>
                </tr>
                <tr>
                    <th class="nomePeriodo" style="width: 110px;">
                    </th>
                    <asp:Repeater ID="rptPeridosSecretaria" runat="server">
                        <ItemTemplate>
                            <th class="nomePeriodo" style="width: 110px;">
                                <%#Eval("tpc_nome") %>
                            </th>
                        </ItemTemplate>
                    </asp:Repeater>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rptAvaliacoesSecretaria" runat="server">
                    <ItemTemplate>
                        <tr class='<%#classeLinhaDisciplina(false) %>'>
                            <td id="tdNomeDiciplina" runat="server" class="nomeDisciplina">
                                <%#Eval("avs_nome")%>
                            </td>
                            <asp:Repeater ID="rptNotasDisciplina" runat="server" DataSource='<%#Eval("notas") %>'>
                                <ItemTemplate>
                                    <td class="nota">
                                        <%#Eval("nota.Nota")%>
                                    </td>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>
</fieldset>
