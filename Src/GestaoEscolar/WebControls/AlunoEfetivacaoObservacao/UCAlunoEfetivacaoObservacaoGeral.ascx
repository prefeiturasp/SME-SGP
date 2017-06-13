<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCAlunoEfetivacaoObservacaoGeral.ascx.cs" Inherits="GestaoEscolar.WebControls.AlunoEfetivacaoObservacao.UCAlunoEfetivacaoObservacaoGeral" %>

<script type="text/javascript">
    var idbtnParecerConclusivo = '#<%=btnParecerConclusivo.ClientID%>';
    var idbtnJustificativaPosConselho = '#<%=btnJustificativaPosConselho.ClientID%>';
    var idbtnDesempenho = '#<%=btnDesempenho.ClientID%>';
    var idbtnRecomendacaoAluno = '#<%=btnRecomendacaoAluno.ClientID%>';
    var idbtnRecomendacaoResponsavel = '#<%=btnRecomendacaoResponsavel.ClientID%>';
    var idbtnAnotacao = '#<%=btnAnotacao.ClientID%>';
</script>

<asp:UpdatePanel ID="updObservacao" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <asp:Label ID="lblMensagem" runat="server"></asp:Label>
        <asp:Label ID="lblMensagemResultadoInvalido" runat="server" CssClass="hide"></asp:Label>
        <asp:Label ID="lblMensagemResultadoErro" runat="server" CssClass="hide"></asp:Label>
        <asp:Panel ID="pnlObservacao" runat="server">
            <div class="dados-aluno clearfix">
                <div class="div-inline">
                    <asp:HiddenField ID="hdnTipoFechamento" runat="server" />
                    <asp:HiddenField ID="hdnCodigoEOLTurma" runat="server" />
                    <asp:HiddenField ID="hdnCodigoEOLAluno" runat="server" />
                    <asp:Image runat="server" ID="imgFotoAluno" />
                </div>
                <div class="div-inline">
                    <asp:Label ID="lblNomeAlunoTitulo" runat="server" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.lblNomeAlunoTitulo.Text %>" CssClass="lbl-negrito"></asp:Label>
                    <asp:Label ID="lblNomeAluno" runat="server"></asp:Label>
                    <br />
                    <asp:Label ID="lblNumeroChamadaTitulo" runat="server" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.lblNumeroChamadaTitulo.Text %>" CssClass="lbl-negrito"></asp:Label>
                    <asp:Label ID="lblNumeroChamada" runat="server"></asp:Label>
                    <br />
                    <asp:Label ID="lblCodigoEolTitulo" runat="server" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.lblCodigoEolTitulo.Text %>" CssClass="lbl-negrito"></asp:Label>
                    <asp:Label ID="lblCodigoEol" runat="server"></asp:Label>
                    <br />
                    <asp:Label ID="lblSituacaoMatriculaTitulo" runat="server" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.lblSituacaoMatriculaTitulo.Text %>" CssClass="lbl-negrito"></asp:Label>
                    <div class="div-inline">
                        <asp:Label ID="lblSituacaoMatriculaEntrada" runat="server"></asp:Label>
                        <br />
                        <asp:Label ID="lblSituacaoMatriculaSaida" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="div-inline div-atualizar">
                    <span class="btn-atualizar">
                        <asp:ImageButton ID="btnAtualizar" runat="server" SkinID="btAtualizar" ToolTip="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.btnAtualizar.ToolTip %>" CausesValidation="false" OnClick="btnAtualizar_Click" />
                        <asp:Label ID="lblAtualizarDados" runat="server" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.lblAtualizarDados.Text %>" Font-Bold="true" AssociatedControlID="btnAtualizar" Style="display: inline; cursor: pointer"></asp:Label>
                    </span>
                </div>
            </div>
            <hr />
            <asp:Button ID="btnParecerConclusivo" ClientIDMode="Static" runat="server" OnClick="btnParecerConclusivo_Click" Style="display: none;" />
            <asp:Button ID="btnJustificativaPosConselho" ClientIDMode="Static" runat="server" OnClick="aJusstificativa_ServerClick" Style="display: none;" />
            <asp:Button ID="btnDesempenho" runat="server" ClientIDMode="Static" OnClick="aDesempenho_ServerClick" Style="display: none;" />
            <asp:Button ID="btnRecomendacaoAluno" ClientIDMode="Static" runat="server" OnClick="aRecomendacaoAluno_ServerClick" Style="display: none;" />
            <asp:Button ID="btnRecomendacaoResponsavel" ClientIDMode="Static" runat="server" OnClick="aRecomendacaoResponsavel_ServerClick" Style="display: none;" />
            <asp:Button ID="btnAnotacao" ClientIDMode="Static" runat="server" OnClick="aAnotacao_ServerClick" Style="display: none;" />
            <div class="area-form">
                <div id="divTabs">
                <ul class="hide">
                    <li id="liParecerConclusivo" runat="server">
                        <a href="#divTabs-0" onclick="$(idbtnParecerConclusivo).click();">
                            <asp:Literal ID="litAbaParecerConclusivo" runat="server" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.litAbaParecerConclusivo.Text %>"></asp:Literal>
                        </a>
                    </li>
                    <li id="liJustificativaPosConselho" runat="server">
                        <a href="#divTabs-1" onclick="$(idbtnJustificativaPosConselho).click();">
                            <asp:Literal ID="litJustificativaPosConselho" runat="server" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.litJustificativaPosConselho.Text %>"></asp:Literal>
                        </a>
                    </li>
                    <li id="liDesempenhoAprendizagem" runat="server">
                        <a href="#divTabs-2" onclick="$(idbtnDesempenho).click();">
                            <asp:Label ID="lblDesempenhoAprendizagem" runat="server" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.lblDesempenhoAprendizagem.Text %>"></asp:Label></a></li>
                    <li id="liRecomendacaoAluno" runat="server">
                        <a href="#divTabs-3" onclick="$(idbtnRecomendacaoAluno).click();">
                            <asp:Label ID="lblRecomendacaoAluno" runat="server" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.lblRecomendacaoAluno.Text %>"></asp:Label></a></li>
                    <li id="liRecomendacaoResponsavel" runat="server">
                        <a href="#divTabs-4" onclick="$(idbtnRecomendacaoResponsavel).click(); ">
                            <asp:Label ID="lblRecomendacaoResponsavel" runat="server" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.lblRecomendacaoResponsavel.Text %>"></asp:Label></a></li>
                    <li id="liAnotacoesAluno" runat="server"><a href="#divTabs-5" onclick="$(idbtnAnotacao).click();">
                        <asp:Label ID="lblAnotacaoAluno" runat="server" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.lblAnotacaoAluno.Text %>"></asp:Label>
                        <asp:HiddenField ID="hdfAbaAnotacaoAlunoVisivel" runat="server" />
                    </a></li>
                </ul>
                <div id="divTabs-0">
                    <fieldset id="fdsBoletim" runat="server">
                        <asp:Label ID="lblMensagemSemDados" runat="server" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.Cadastro.lblMensagemSemDados.Text %>"></asp:Label>
                        <br />
                        <div id="divFrequenciaGlobal" class="div-inline align-middle" runat="server">
                            <asp:Label ID="lblFrequenciaGlobalTitulo" runat="server" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.lblFrequenciaGlobalTitulo.Text %>" CssClass="lbl-negrito"></asp:Label>
                            <asp:Label ID="lblFrequenciaGlobal" runat="server"></asp:Label>
                        </div>
                        <div id="divParecerConclusivo" class="div-inline align-middle div-parecer-conclusivo" runat="server">
                            <asp:HiddenField ID="hfDataUltimaAlteracaoParecerConclusivo" runat="server" />
                            <asp:Label ID="lblParecerConclusivo" runat="server" Text="<%$ Resources:Mensagens, MSG_RESULTADOEFETIVACAO %>" Font-Bold="true" AssociatedControlID="ddlResultado"></asp:Label>
                            <asp:DropDownList ID="ddlResultado" runat="server" CssClass="ddlResultadoParecerConclusivo"></asp:DropDownList>
                        </div><br />
                        <br />
                        <div id="divInseridoPor" runat="server">
                            <asp:Label ID="lblHistoricoParecer" runat="server" Text=""></asp:Label>
                            <br />
                            <br />
                        </div>

                        <div id="divBoletim" runat="server">
                            <asp:Label ID="lblMensagemFrequenciaExterna" runat="server" Visible="false" ></asp:Label>

                            <div class="div-gestor-scroll" id="divDisciplinas" runat="server">
                                <table class="table-boletim">
                                    <thead>
                                        <tr>
                                            <th rowspan="2" class="th-disciplina">
                                                <asp:Label runat="server" ID="lblDisp" Text="<%$ Resources:Mensagens, MSG_DISCIPLINA %>"></asp:Label>
                                            </th>
                                            <asp:Repeater ID="rptPeriodosNomes" runat="server">
                                                <ItemTemplate>
                                                    <th class="th-periodo" colspan="4" runat="server" visible='<%#mostraConceitoGlobal %>'>
                                                        <span><%#Eval("tpc_nome") %></span>
                                                    </th>
                                                    <th class="th-periodo" colspan="3" runat="server" visible='<%#!mostraConceitoGlobal %>'>
                                                        <span><%#Eval("tpc_nome") %></span>
                                                    </th>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            <th rowspan="2" class="th-ca" id="thTotalComp" runat="server" visible="false">
                                                <asp:Label runat="server" ID="lblCompAusencia" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.lblCompAusencia.Text %>"></asp:Label>
                                            </th>
                                            <th rowspan="2" class="th-faltas-externas" id="thFaltasExternas" runat="server" visible="false">
                                                <asp:Label runat="server" ID="lblFaltasExternas" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.lblFaltasExternas.Text %>"></asp:Label>
                                            </th>
                                            <th rowspan="2" class="th-frequencia" id="thFreqFinal" runat="server" visible="false">
                                                <asp:Label runat="server" ID="lblPorcentagemFreq" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.lblPorcentagemFreq.Text %>"></asp:Label>
                                            </th>
                                            <th rowspan="2" class="th-parecer-final" id="thNotaFinal" runat="server">
                                                <asp:Label runat="server" ID="lblNotaFinal" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.lblNotaFinal.Text %>"></asp:Label>
                                            </th>
                                            <%--COLUNA CONCEITO FINAL--%>
                                            <%-- Nota final, é setada no código --%>
                                        </tr>
                                        <tr>
                                            <asp:Repeater ID="rptPeriodosColunasFixas" runat="server">
                                                <ItemTemplate>
                                                    <th class="th-conceito" runat="server" visible='<%#mostraConceitoGlobal %>'>Conc.
                                                    </th>
                                                    <th class="th-nota"><%#nomeNota %>
                                                    </th>
                                                    <th class="th-pos-conselho">
                                                        <asp:Label runat="server" ID="lblNotaPosConselho" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.lblNotaPosConselho.Text %>"></asp:Label>
                                                    </th>
                                                    <th class="th-faltas">
                                                        <asp:Label runat="server" ID="lblQtdFaltas" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.lblQtdFaltas.Text %>"></asp:Label>
                                                    </th>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptDisciplinas" runat="server" OnItemDataBound="rptDisciplinas_ItemDataBound">
                                            <ItemTemplate>
                                                <tr class='tr-disciplina'>
                                                    <th id="tdNomeDiciplina" runat="server" class="th-disciplina">
                                                        <asp:Literal ID="litNomeDisciplina" runat="server" Text='<%#Eval("Disciplina")%>'></asp:Literal>
                                                    </th>
                                                    <asp:Repeater ID="rptNotasDisciplina" OnItemDataBound="rptNotasDisciplina_ItemDataBound" runat="server" DataSource='<%#Eval("notas") %>'>
                                                        <ItemTemplate>
                                                            <td id="tdConceito" class="td-notas" runat="server" visible='<%#mostraConceitoGlobal %>'>
                                                                <%#Eval("nota.Conceito") %>
                                                            </td>
                                                            <td id="tdNota" runat="server" class="td-notas">
                                                                <%#Eval("nota.Nota")%>
                                                            </td>
                                                            <td id="tdNotaPosConselho" runat="server" class="td-notas">
                                                                <div>
                                                                    <asp:HiddenField ID="hfTudId" runat="server" Value='<%# Eval("nota.tud_id") %>' />
                                                                    <asp:HiddenField ID="hfMtuId" runat="server" Value='<%# Eval("nota.mtu_id") %>' />
                                                                    <asp:HiddenField ID="hfMtdId" runat="server" Value='<%# Eval("nota.mtd_id") %>' />
                                                                    <asp:HiddenField ID="hfAtdId" runat="server" Value='<%# Eval("nota.atd_id") %>' />
                                                                    <asp:HiddenField ID="hfFavId" runat="server" Value='<%# Eval("nota.fav_id") %>' />
                                                                    <asp:HiddenField ID="hfAvaId" runat="server" Value='<%# Eval("nota.ava_id") %>' />
                                                                    <asp:HiddenField ID="hfTpcId" runat="server" Value='<%# Eval("nota.tpc_id") %>' />
                                                                    <asp:Label ID="lblNotaPosConselho" runat="server" Visible="false"></asp:Label>
                                                                    <asp:TextBox ID="txtNotaFinal" runat="server" SkinID="Decimal" MaxLength="6"></asp:TextBox>
                                                                    <asp:CustomValidator ID="cvNotaMaxima" runat="server" ControlToValidate="txtNota"
                                                                        Display="Dynamic" OnServerValidate="cvNotaMaxima_Validar" Visible="false" ErrorMessage="">*</asp:CustomValidator>
                                                                    <asp:DropDownList ID="ddlParecerFinal" runat="server" Style="margin: 0px;">
                                                                    </asp:DropDownList>
                                                                    <asp:CustomValidator ID="cvParecerMaximo" runat="server" ControlToValidate="ddlPareceres"
                                                                        Display="Dynamic" OnServerValidate="cvParecerMaximo_Validar" Visible="false"
                                                                        ErrorMessage="">*</asp:CustomValidator>
                                                                </div>
                                                            </td>
                                                            <td class="td-faltas" id="tdFaltas" runat="server">
                                                                <%#Eval("nota.numeroFaltas")%>
                                                            </td>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                    <td class="td-notas" id="tdTotAusenciasCompensadas" runat="server" visible="false">
                                                        <asp:Literal runat="server" ID="litAusenciasCompensadas" Text='<%#Eval("ausenciasCompensadas")%>'></asp:Literal>
                                                    </td>
                                                    <td class="td-notas" id="tdTotFaltasExternas" runat="server" visible="false">
                                                        <asp:Literal runat="server" ID="litFaltasExternas" Text='<%#Eval("faltasExternas")%>'></asp:Literal>
                                                    </td>
                                                    <td class="td-notas" id="tdTotFrequenciaAjustada" runat="server" visible="false">
                                                        <asp:Literal runat="server" ID="litFrequenciaAjustada"
                                                            Text='<%# Eval("FrequenciaFinalAjustada") %>'></asp:Literal>
                                                    </td>
                                                    <td class="td-notas" id="tdNotaFinal" runat="server">
                                                        <div>
                                                            <asp:HiddenField ID="hfTudId" runat="server" Value='<%# Eval("tud_idResultado") %>' />
                                                            <asp:HiddenField ID="hfMtuId" runat="server" Value='<%# Eval("mtu_idResultado") %>' />
                                                            <asp:HiddenField ID="hfMtdId" runat="server" Value='<%# Eval("mtd_idResultado") %>' />
                                                            <asp:HiddenField ID="hfAtdId" runat="server" Value='<%# Eval("atd_idResultado") %>' />
                                                            <asp:HiddenField ID="hfFavId" runat="server" Value='<%# Eval("fav_idResultado") %>' />
                                                            <asp:HiddenField ID="hfAvaId" runat="server" Value='<%# Eval("ava_idResultado") %>' />
                                                            <asp:Label ID="lblNotaFinal" runat="server" Visible="false"></asp:Label>
                                                            <asp:TextBox ID="txtNotaFinal" runat="server" SkinID="Decimal" MaxLength="6"></asp:TextBox>
                                                            <asp:CustomValidator ID="cvNotaMaxima" runat="server" ControlToValidate="txtNotaFinal"
                                                                Display="Dynamic" OnServerValidate="cvNotaMaxima_Validar" Visible="false" ErrorMessage="">*</asp:CustomValidator>
                                                            <asp:DropDownList ID="ddlParecerFinal" runat="server" Style="margin: 0px;">
                                                            </asp:DropDownList>
                                                            <asp:CustomValidator ID="cvParecerMaximo" runat="server" ControlToValidate="ddlParecerFinal"
                                                                Display="Dynamic" OnServerValidate="cvParecerMaximo_Validar" Visible="false"
                                                                ErrorMessage="">*</asp:CustomValidator>
                                                        </div>

                                                    </td>
                                                    <%--COLUNA CONCEITO FINAL--%>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr class='tr-disciplina alternada'>
                                                    <th id="tdNomeDiciplina" runat="server" class="th-disciplina">
                                                        <asp:Literal ID="litNomeDisciplina" runat="server" Text='<%#Eval("Disciplina")%>'></asp:Literal>
                                                    </th>
                                                    <asp:Repeater ID="rptNotasDisciplina" OnItemDataBound="rptNotasDisciplina_ItemDataBound" runat="server" DataSource='<%#Eval("notas") %>'>
                                                        <ItemTemplate>
                                                            <td id="tdConceito" class="td-notas" runat="server" visible='<%#mostraConceitoGlobal %>'>
                                                                <%#Eval("nota.Conceito") %>
                                                            </td>
                                                            <td id="tdNota" runat="server" class="td-notas">
                                                                <%#Eval("nota.Nota")%>
                                                            </td>
                                                            <td id="tdNotaPosConselho" runat="server" class="td-notas">
                                                                <div>
                                                                    <asp:HiddenField ID="hfTudId" runat="server" Value='<%# Eval("nota.tud_id") %>' />
                                                                    <asp:HiddenField ID="hfMtuId" runat="server" Value='<%# Eval("nota.mtu_id") %>' />
                                                                    <asp:HiddenField ID="hfMtdId" runat="server" Value='<%# Eval("nota.mtd_id") %>' />
                                                                    <asp:HiddenField ID="hfAtdId" runat="server" Value='<%# Eval("nota.atd_id") %>' />
                                                                    <asp:HiddenField ID="hfFavId" runat="server" Value='<%# Eval("nota.fav_id") %>' />
                                                                    <asp:HiddenField ID="hfAvaId" runat="server" Value='<%# Eval("nota.ava_id") %>' />
                                                                    <asp:HiddenField ID="hfTpcId" runat="server" Value='<%# Eval("nota.tpc_id") %>' />
                                                                    <asp:Label ID="lblNotaPosConselho" runat="server" Visible="false"></asp:Label>
                                                                    <asp:TextBox ID="txtNotaFinal" runat="server" SkinID="Decimal" MaxLength="6"></asp:TextBox>
                                                                    <asp:CustomValidator ID="cvNotaMaxima" runat="server" ControlToValidate="txtNota"
                                                                        Display="Dynamic" OnServerValidate="cvNotaMaxima_Validar" Visible="false" ErrorMessage="">*</asp:CustomValidator>
                                                                    <asp:DropDownList ID="ddlParecerFinal" runat="server" Style="margin: 0px;">
                                                                    </asp:DropDownList>
                                                                    <asp:CustomValidator ID="cvParecerMaximo" runat="server" ControlToValidate="ddlPareceres"
                                                                        Display="Dynamic" OnServerValidate="cvParecerMaximo_Validar" Visible="false"
                                                                        ErrorMessage="">*</asp:CustomValidator>
                                                                </div>
                                                            </td>
                                                            <td class="td-faltas" id="tdFaltas" runat="server">
                                                                <%#Eval("nota.numeroFaltas")%>
                                                            </td>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                    <td class="td-notas" id="tdTotAusenciasCompensadas" runat="server" visible="false">
                                                        <asp:Literal runat="server" ID="litAusenciasCompensadas" Text='<%#Eval("ausenciasCompensadas")%>'></asp:Literal>
                                                    </td>
                                                    <td class="td-notas" id="tdTotFaltasExternas" runat="server" visible="false">
                                                        <asp:Literal runat="server" ID="litFaltasExternas" Text='<%#Eval("faltasExternas")%>'></asp:Literal>
                                                    </td>
                                                    <td class="td-notas" id="tdTotFrequenciaAjustada" runat="server" visible="false">
                                                        <asp:Literal runat="server" ID="litFrequenciaAjustada"
                                                            Text='<%# Eval("FrequenciaFinalAjustada") %>'></asp:Literal>
                                                    </td>
                                                    <td class="td-notas" id="tdNotaFinal" runat="server">
                                                        <div>
                                                            <asp:HiddenField ID="hfTudId" runat="server" Value='<%# Eval("tud_idResultado") %>' />
                                                            <asp:HiddenField ID="hfMtuId" runat="server" Value='<%# Eval("mtu_idResultado") %>' />
                                                            <asp:HiddenField ID="hfMtdId" runat="server" Value='<%# Eval("mtd_idResultado") %>' />
                                                            <asp:HiddenField ID="hfAtdId" runat="server" Value='<%# Eval("atd_idResultado") %>' />
                                                            <asp:HiddenField ID="hfFavId" runat="server" Value='<%# Eval("fav_idResultado") %>' />
                                                            <asp:HiddenField ID="hfAvaId" runat="server" Value='<%# Eval("ava_idResultado") %>' />
                                                            <asp:Label ID="lblNotaFinal" runat="server" Visible="false"></asp:Label>
                                                            <asp:TextBox ID="txtNotaFinal" runat="server" SkinID="Decimal" MaxLength="6"></asp:TextBox>
                                                            <asp:CustomValidator ID="cvNotaMaxima" runat="server" ControlToValidate="txtNotaFinal"
                                                                Display="Dynamic" OnServerValidate="cvNotaMaxima_Validar" Visible="false" ErrorMessage="">*</asp:CustomValidator>
                                                            <asp:DropDownList ID="ddlParecerFinal" runat="server" Style="margin: 0px;">
                                                            </asp:DropDownList>
                                                            <asp:CustomValidator ID="cvParecerMaximo" runat="server" ControlToValidate="ddlParecerFinal"
                                                                Display="Dynamic" OnServerValidate="cvParecerMaximo_Validar" Visible="false"
                                                                ErrorMessage="">*</asp:CustomValidator>
                                                        </div>

                                                    </td>
                                                    <%--COLUNA CONCEITO FINAL--%>
                                                </tr>
                                            </AlternatingItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>
                            <div id="divEnriquecimentoCurricular" runat="server">
                                <hr />
                                <div class="div-gestor-scroll">
                                    <table class="table-boletim">
                                        <thead>
                                            <tr>
                                                <th rowspan="2" class="th-disciplina">
                                                    <asp:Label runat="server" ID="lblEnriquecimento" Text="<%$ Resources:UserControl, UCDadosBoletim.lblEnriquecimento.Text %>"></asp:Label>
                                                </th>
                                                <asp:Repeater ID="rptPeriodosNomesEnriquecimento" runat="server">
                                                    <ItemTemplate>
                                                        <th id="Th1" class="th-periodo" runat="server" visible='<%#mostraConceitoGlobal %>'>
                                                            <span><%#Eval("tpc_nome") %></span>
                                                        </th>
                                                        <th id="Th2" class="th-periodo" runat="server" visible='<%#!mostraConceitoGlobal %>'>
                                                            <span><%#Eval("tpc_nome") %></span>
                                                        </th>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                                <th rowspan="2" class="th-faltas-externas" id="thFaltasExternasEnriquecimento" runat="server" visible="false">
                                                    <asp:Label runat="server" ID="lblFaltasExternasEnriquecimento" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.lblFaltasExternas.Text %>"></asp:Label>
                                                </th>
                                                <th rowspan="2" class="th-frequencia" id="thFreqFinalEnriquecimento" runat="server" visible="false">
                                                    <asp:Label runat="server" ID="lblPorcentagemFreqEnriquecimento" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.lblPorcentagemFreqEnriquecimento.Text %>"></asp:Label>
                                                </th>
                                                <th rowspan="2" class="th-parecer-final" id="thParecerFinal" runat="server" visible="false">
                                                    <asp:Label runat="server" ID="lblParecerFinal" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.lblParecerFinal.Text %>"></asp:Label>
                                                </th>
                                            </tr>
                                            <tr>
                                                <asp:Repeater ID="rptPeriodosColunasFixasEnriquecimento" runat="server">
                                                    <ItemTemplate>
                                                        <th class="th-faltas">
                                                            <asp:Label runat="server" ID="lblQtdFaltas" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.lblQtdFaltas.Text %>"></asp:Label>
                                                        </th>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rptDisciplinasEnriquecimentoCurricular" runat="server" OnItemDataBound="rptDisciplinasEnriquecimentoCurricular_ItemDataBound">
                                                <ItemTemplate>
                                                    <tr class='tr-disciplina'>
                                                        <th id="tdNomeDiciplina" runat="server" class="th-disciplina">
                                                            <asp:Literal ID="litNomeDisciplina" runat="server" Text='<%#Eval("Disciplina")%>'></asp:Literal>
                                                        </th>
                                                        <asp:Repeater ID="rptNotasDisciplina" OnItemDataBound="rptNotasDisciplina_ItemDataBound" runat="server" DataSource='<%#Eval("notas") %>'>
                                                            <ItemTemplate>
                                                                <td class="td-faltas" id="tdFaltas" runat="server">
                                                                    <%#Eval("nota.numeroFaltas")%>
                                                                </td>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                        <td class="td-notas" id="tdTotFaltasExternas" runat="server" visible="false">
                                                            <asp:Literal runat="server" ID="litFaltasExternas" Text='<%#Eval("faltasExternas")%>'></asp:Literal>
                                                        </td>
                                                        <td class="td-notas" id="tdTotFrequenciaAjustada" runat="server">
                                                            <asp:Literal runat="server" ID="litFrequenciaAjustadaEnriquec" Text='<%#Eval("FrequenciaFinalAjustada")%>'></asp:Literal>
                                                        </td>
                                                        <td class="td-notas" id="tdParecerFinal" runat="server">
                                                            <asp:HiddenField ID="hfTudId" runat="server" Value='<%# Eval("tud_idResultado") %>' />
                                                            <asp:HiddenField ID="hfMtuId" runat="server" Value='<%# Eval("mtu_idResultado") %>' />
                                                            <asp:HiddenField ID="hfMtdId" runat="server" Value='<%# Eval("mtd_idResultado") %>' />
                                                            <asp:HiddenField ID="hfAtdId" runat="server" Value='<%# Eval("atd_idResultado") %>' />
                                                            <asp:HiddenField ID="hfFavId" runat="server" Value='<%# Eval("fav_idResultado") %>' />
                                                            <asp:HiddenField ID="hfAvaId" runat="server" Value='<%# Eval("ava_idResultado") %>' />
                                                            <asp:Label ID="lblParecerFinal" runat="server"></asp:Label>
                                                            <asp:DropDownList ID="ddlParecerFinal" runat="server" Style="margin: 0px;"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <tr class='tr-disciplina alternada'>
                                                        <th id="tdNomeDiciplina" runat="server" class="th-disciplina">
                                                            <asp:Literal ID="litNomeDisciplina" runat="server" Text='<%#Eval("Disciplina")%>'></asp:Literal>
                                                        </th>
                                                        <asp:Repeater ID="rptNotasDisciplina" OnItemDataBound="rptNotasDisciplina_ItemDataBound" runat="server" DataSource='<%#Eval("notas") %>'>
                                                            <ItemTemplate>
                                                                <td class="td-faltas" id="tdFaltas" runat="server">
                                                                    <%#Eval("nota.numeroFaltas")%>
                                                                </td>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                        <td class="td-notas" id="tdTotFaltasExternas" runat="server" visible="false">
                                                            <asp:Literal runat="server" ID="litFaltasExternas" Text='<%#Eval("faltasExternas")%>'></asp:Literal>
                                                        </td>
                                                        <td class="td-notas" id="tdTotFrequenciaAjustada" runat="server">
                                                            <asp:Literal runat="server" ID="litFrequenciaAjustadaEnriquec" Text='<%#Eval("FrequenciaFinalAjustada")%>'></asp:Literal>
                                                        </td>
                                                        <td class="td-notas" id="tdParecerFinal" runat="server">
                                                            <asp:HiddenField ID="hfTudId" runat="server" Value='<%# Eval("tud_idResultado") %>' />
                                                            <asp:HiddenField ID="hfMtuId" runat="server" Value='<%# Eval("mtu_idResultado") %>' />
                                                            <asp:HiddenField ID="hfMtdId" runat="server" Value='<%# Eval("mtd_idResultado") %>' />
                                                            <asp:HiddenField ID="hfAtdId" runat="server" Value='<%# Eval("atd_idResultado") %>' />
                                                            <asp:HiddenField ID="hfFavId" runat="server" Value='<%# Eval("fav_idResultado") %>' />
                                                            <asp:HiddenField ID="hfAvaId" runat="server" Value='<%# Eval("ava_idResultado") %>' />
                                                            <asp:Label ID="lblParecerFinal" runat="server"></asp:Label>
                                                            <asp:DropDownList ID="ddlParecerFinal" runat="server" Style="margin: 0px;"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </AlternatingItemTemplate>
                                            </asp:Repeater>

                                            <asp:Repeater ID="rptDisciplinasExperiencias" runat="server" OnItemDataBound="rptDisciplinasEnriquecimentoCurricular_ItemDataBound">
                                                <ItemTemplate>
                                                    <tr class='tr-disciplina'>
                                                        <th id="tdNomeDiciplina" runat="server" class="th-disciplina">
                                                            <asp:Literal ID="litNomeDisciplina" runat="server" Text='<%#Eval("Disciplina")%>'></asp:Literal>
                                                        </th>
                                                        <asp:Repeater ID="rptNotasDisciplina" OnItemDataBound="rptNotasDisciplina_ItemDataBound" runat="server" DataSource='<%#Eval("notas") %>'>
                                                            <ItemTemplate>
                                                                <td class="td-faltas" id="tdFaltas" runat="server">
                                                                    <%#Eval("nota.numeroFaltas")%>
                                                                </td>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                        <td class="td-notas" id="tdTotFaltasExternas" runat="server" visible="false">
                                                            <asp:Literal runat="server" ID="litFaltasExternas" Text='<%#Eval("faltasExternas")%>'></asp:Literal>
                                                        </td>
                                                        <td class="td-notas" id="tdTotFrequenciaAjustada" runat="server">
                                                            <asp:Literal runat="server" ID="litFrequenciaAjustadaEnriquec" Text='<%#Eval("FrequenciaFinalAjustada")%>'></asp:Literal>
                                                        </td>
                                                        <td class="td-notas" id="tdParecerFinal" runat="server">
                                                            <asp:HiddenField ID="hfTudId" runat="server" Value='<%# Eval("tud_idResultado") %>' />
                                                            <asp:HiddenField ID="hfMtuId" runat="server" Value='<%# Eval("mtu_idResultado") %>' />
                                                            <asp:HiddenField ID="hfMtdId" runat="server" Value='<%# Eval("mtd_idResultado") %>' />
                                                            <asp:HiddenField ID="hfAtdId" runat="server" Value='<%# Eval("atd_idResultado") %>' />
                                                            <asp:HiddenField ID="hfFavId" runat="server" Value='<%# Eval("fav_idResultado") %>' />
                                                            <asp:HiddenField ID="hfAvaId" runat="server" Value='<%# Eval("ava_idResultado") %>' />
                                                            <asp:Label ID="lblParecerFinal" runat="server"></asp:Label>
                                                            <asp:DropDownList ID="ddlParecerFinal" runat="server" Style="margin: 0px;"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <tr class='tr-disciplina alternada'>
                                                        <th id="tdNomeDiciplina" runat="server" class="th-disciplina">
                                                            <asp:Literal ID="litNomeDisciplina" runat="server" Text='<%#Eval("Disciplina")%>'></asp:Literal>
                                                        </th>
                                                        <asp:Repeater ID="rptNotasDisciplina" OnItemDataBound="rptNotasDisciplina_ItemDataBound" runat="server" DataSource='<%#Eval("notas") %>'>
                                                            <ItemTemplate>
                                                                <td class="td-faltas" id="tdFaltas" runat="server">
                                                                    <%#Eval("nota.numeroFaltas")%>
                                                                </td>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                        <td class="td-notas" id="tdTotFaltasExternas" runat="server" visible="false">
                                                            <asp:Literal runat="server" ID="litFaltasExternas" Text='<%#Eval("faltasExternas")%>'></asp:Literal>
                                                        </td>
                                                        <td class="td-notas" id="tdTotFrequenciaAjustada" runat="server">
                                                            <asp:Literal runat="server" ID="litFrequenciaAjustadaEnriquec" Text='<%#Eval("FrequenciaFinalAjustada")%>'></asp:Literal>
                                                        </td>
                                                        <td class="td-notas" id="tdParecerFinal" runat="server">
                                                            <asp:HiddenField ID="hfTudId" runat="server" Value='<%# Eval("tud_idResultado") %>' />
                                                            <asp:HiddenField ID="hfMtuId" runat="server" Value='<%# Eval("mtu_idResultado") %>' />
                                                            <asp:HiddenField ID="hfMtdId" runat="server" Value='<%# Eval("mtd_idResultado") %>' />
                                                            <asp:HiddenField ID="hfAtdId" runat="server" Value='<%# Eval("atd_idResultado") %>' />
                                                            <asp:HiddenField ID="hfFavId" runat="server" Value='<%# Eval("fav_idResultado") %>' />
                                                            <asp:HiddenField ID="hfAvaId" runat="server" Value='<%# Eval("ava_idResultado") %>' />
                                                            <asp:Label ID="lblParecerFinal" runat="server"></asp:Label>
                                                            <asp:DropDownList ID="ddlParecerFinal" runat="server" Style="margin: 0px;"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </AlternatingItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                            <div id="divEnsinoInfantil" runat="server">
                                <hr />
                                <div class="div-gestor-scroll">
                                    <table class="table-boletim">
                                        <thead>
                                            <tr>
                                                <th rowspan="2" class="th-disciplina">
                                                    <asp:Label runat="server" ID="lblEI" Text="<%$ Resources:UserControl, UCDadosBoletim.lblEI.Text %>"></asp:Label>
                                                </th>
                                                <asp:Repeater ID="rptPeriodosNomesEI" runat="server">
                                                    <ItemTemplate>
                                                        <th id="Th1" class="th-periodo" runat="server" visible='<%#mostraConceitoGlobal %>'>
                                                            <span><%#Eval("tpc_nome") %></span>
                                                        </th>
                                                        <th id="Th2" class="th-periodo" runat="server" visible='<%#!mostraConceitoGlobal %>'>
                                                            <span><%#Eval("tpc_nome") %></span>
                                                        </th>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                                <th rowspan="2" class="th-faltas-externas" id="thFaltasExternasEI" runat="server" visible="false">
                                                    <asp:Label runat="server" ID="lblFaltasExternasEI" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.lblFaltasExternas.Text %>"></asp:Label>
                                                </th>
                                                <th rowspan="2" class="th-frequencia" id="thFreqFinalEI" runat="server" visible="false">
                                                    <asp:Label runat="server" ID="lblPorcentagemFreqEI" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.lblPorcentagemFreqEI.Text %>"></asp:Label>
                                                </th>
                                                <th rowspan="2" class="th-parecer-final" id="thParecerFinalEI" runat="server" visible="false">
                                                    <asp:Label runat="server" ID="lblParecerFinalEI" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.lblParecerFinal.Text %>"></asp:Label>
                                                </th>
                                            </tr>
                                            <tr>
                                                <asp:Repeater ID="rptPeriodosColunasFixasEI" runat="server">
                                                    <ItemTemplate>
                                                        <th class="th-faltas">
                                                            <asp:Label runat="server" ID="lblQtdFaltas" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.lblQtdFaltas.Text %>"></asp:Label>
                                                        </th>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rptDisciplinasEnsinoInfantil" runat="server" OnItemDataBound="rptDisciplinasEnsinoInfantil_ItemDataBound">
                                                <ItemTemplate>
                                                    <tr class='tr-disciplina'>
                                                        <th id="tdNomeDiciplina" runat="server" class="th-disciplina">
                                                            <asp:Literal ID="litNomeDisciplina" runat="server" Text='<%#Eval("Disciplina")%>'></asp:Literal>
                                                        </th>
                                                        <asp:Repeater ID="rptNotasDisciplina" OnItemDataBound="rptNotasDisciplina_ItemDataBound" runat="server" DataSource='<%#Eval("notas") %>'>
                                                            <ItemTemplate>
                                                                <td class="td-faltas" id="tdFaltas" runat="server">
                                                                    <%#Eval("nota.numeroFaltas")%>
                                                                </td>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                         <td class="td-notas" id="tdTotFaltasExternas" runat="server" visible="false">
                                                            <asp:Literal runat="server" ID="litFaltasExternasEI" Text='<%#Eval("faltasExternas")%>'></asp:Literal>
                                                        </td>
                                                        <td class="td-notas" id="tdTotFrequenciaAjustada" runat="server">
                                                            <asp:Literal runat="server" ID="litFrequenciaAjustadaEI" Text='<%#Eval("FrequenciaFinalAjustada")%>'></asp:Literal>
                                                        </td>
                                                        <td class="td-notas" id="tdParecerFinal" runat="server">
                                                            <asp:HiddenField ID="hfTudId" runat="server" Value='<%# Eval("tud_idResultado") %>' />
                                                            <asp:HiddenField ID="hfMtuId" runat="server" Value='<%# Eval("mtu_idResultado") %>' />
                                                            <asp:HiddenField ID="hfMtdId" runat="server" Value='<%# Eval("mtd_idResultado") %>' />
                                                            <asp:HiddenField ID="hfAtdId" runat="server" Value='<%# Eval("atd_idResultado") %>' />
                                                            <asp:HiddenField ID="hfFavId" runat="server" Value='<%# Eval("fav_idResultado") %>' />
                                                            <asp:HiddenField ID="hfAvaId" runat="server" Value='<%# Eval("ava_idResultado") %>' />
                                                            <asp:Label ID="lblParecerFinal" runat="server"></asp:Label>
                                                            <asp:DropDownList ID="ddlParecerFinal" runat="server" Style="margin: 0px;"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <tr class='tr-disciplina alternada'>
                                                        <th id="tdNomeDiciplina" runat="server" class="th-disciplina">
                                                            <asp:Literal ID="litNomeDisciplina" runat="server" Text='<%#Eval("Disciplina")%>'></asp:Literal>
                                                        </th>
                                                        <asp:Repeater ID="rptNotasDisciplina" OnItemDataBound="rptNotasDisciplina_ItemDataBound" runat="server" DataSource='<%#Eval("notas") %>'>
                                                            <ItemTemplate>
                                                                <td class="td-faltas" id="tdFaltas" runat="server">
                                                                    <%#Eval("nota.numeroFaltas")%>
                                                                </td>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                         <td class="td-notas" id="tdTotFaltasExternas" runat="server" visible="false">
                                                            <asp:Literal runat="server" ID="litFaltasExternasEI" Text='<%#Eval("faltasExternas")%>'></asp:Literal>
                                                        </td>
                                                        <td class="td-notas" id="tdTotFrequenciaAjustada" runat="server">
                                                            <asp:Literal runat="server" ID="litFrequenciaAjustadaEI" Text='<%#Eval("FrequenciaFinalAjustada")%>'></asp:Literal>
                                                        </td>
                                                        <td class="td-notas" id="tdParecerFinal" runat="server">
                                                            <asp:HiddenField ID="hfTudId" runat="server" Value='<%# Eval("tud_idResultado") %>' />
                                                            <asp:HiddenField ID="hfMtuId" runat="server" Value='<%# Eval("mtu_idResultado") %>' />
                                                            <asp:HiddenField ID="hfMtdId" runat="server" Value='<%# Eval("mtd_idResultado") %>' />
                                                            <asp:HiddenField ID="hfAtdId" runat="server" Value='<%# Eval("atd_idResultado") %>' />
                                                            <asp:HiddenField ID="hfFavId" runat="server" Value='<%# Eval("fav_idResultado") %>' />
                                                            <asp:HiddenField ID="hfAvaId" runat="server" Value='<%# Eval("ava_idResultado") %>' />
                                                            <asp:Label ID="lblParecerFinal" runat="server"></asp:Label>
                                                            <asp:DropDownList ID="ddlParecerFinal" runat="server" Style="margin: 0px;"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </AlternatingItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                            <div id="divRecuperacao" runat="server">
                                <hr />
                                <div class="div-gestor-scroll">
                                    <table class="table-boletim">
                                        <thead>
                                            <tr>
                                                <th rowspan="2" class="th-disciplina">
                                                    <asp:Label runat="server" ID="lblRecuperacaoTitulo" Text="<%$ Resources:UserControl, UCDadosBoletim.lblRecuperacaoTitulo.Text %>"></asp:Label>
                                                </th>
                                                <asp:Repeater ID="rptPeriodosNomesRecuperacao" runat="server">
                                                    <ItemTemplate>
                                                        <th id="Th1" class="th-periodo" runat="server" visible='<%#mostraConceitoGlobal %>'>
                                                            <span><%#Eval("tpc_nome") %></span>
                                                        </th>
                                                        <th id="Th2" class="th-periodo" runat="server" visible='<%#!mostraConceitoGlobal %>'>
                                                            <span><%#Eval("tpc_nome") %></span>
                                                        </th>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                                <th rowspan="2" class="th-frequencia">
                                                    <asp:Label runat="server" ID="lblPorcentagemFreqRecuperacao" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.lblPorcentagemFreqRecuperacao.Text %>"></asp:Label>
                                                </th>
                                                <th rowspan="2" class="th-parecer-final" id="thFreqFinalRecuperacao" runat="server" visible="false">
                                                    <asp:Label runat="server" ID="lblParecerFinalRecuperacao" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.lblParecerFinalRecuperacao.Text %>"></asp:Label>
                                                </th>
                                            </tr>
                                            <tr>
                                                <asp:Repeater ID="rptPeriodosColunasFixasRecuperacao" runat="server">
                                                    <ItemTemplate>
                                                        <th class="th-faltas">
                                                            <asp:Label runat="server" ID="lblQtdFaltas" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.lblQtdFaltas.Text %>"></asp:Label>
                                                        </th>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rptDisciplinasRecuperacao" runat="server" OnItemDataBound="rptDisciplinasRecuperacao_ItemDataBound">
                                                <ItemTemplate>
                                                    <tr class='tr-disciplina'>
                                                        <th id="tdNomeDiciplina" runat="server" class="th-disciplina">
                                                            <asp:Literal ID="litNomeDisciplina" runat="server" Text='<%#Eval("Disciplina")%>'></asp:Literal>
                                                        </th>
                                                        <asp:Repeater ID="rptNotasDisciplina" OnItemDataBound="rptNotasDisciplina_ItemDataBound" runat="server" DataSource='<%#Eval("notas") %>'>
                                                            <ItemTemplate>

                                                                <td class="td-notas" id="tdFaltas" runat="server">
                                                                    <%#Eval("nota.numeroFaltas")%>
                                                                </td>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                        <td class="td-notas" id="tdTotFrequenciaAjustada" runat="server">
                                                            <asp:Literal runat="server" ID="litFrequenciaAjustada" Text='<%#Eval("FrequenciaFinalAjustada") %>'></asp:Literal>
                                                        </td>

                                                        <td class="td-notas" id="tdParecerFinal" runat="server">
                                                            <asp:HiddenField ID="hfTudId" runat="server" Value='<%# Eval("tud_idResultado") %>' />
                                                            <asp:HiddenField ID="hfMtuId" runat="server" Value='<%# Eval("mtu_idResultado") %>' />
                                                            <asp:HiddenField ID="hfMtdId" runat="server" Value='<%# Eval("mtd_idResultado") %>' />
                                                            <asp:HiddenField ID="hfAtdId" runat="server" Value='<%# Eval("atd_idResultado") %>' />
                                                            <asp:HiddenField ID="hfFavId" runat="server" Value='<%# Eval("fav_idResultado") %>' />
                                                            <asp:HiddenField ID="hfAvaId" runat="server" Value='<%# Eval("ava_idResultado") %>' />
                                                            <asp:Label ID="lblParecerFinal" runat="server"></asp:Label>
                                                            <asp:DropDownList ID="ddlParecerFinal" runat="server" Style="margin: 0px;"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <tr class='tr-disciplina'>
                                                        <th id="tdNomeDiciplina" runat="server" class="th-disciplina">
                                                            <asp:Literal ID="litNomeDisciplina" runat="server" Text='<%#Eval("Disciplina")%>'></asp:Literal>
                                                        </th>
                                                        <asp:Repeater ID="rptNotasDisciplina" OnItemDataBound="rptNotasDisciplina_ItemDataBound" runat="server" DataSource='<%#Eval("notas") %>'>
                                                            <ItemTemplate>

                                                                <td class="td-notas" id="tdFaltas" runat="server">
                                                                    <%#Eval("nota.numeroFaltas")%>
                                                                </td>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                        <td class="td-notas" id="tdTotFrequenciaAjustada" runat="server">
                                                            <asp:Literal runat="server" ID="litFrequenciaAjustada" Text='<%#Eval("FrequenciaFinalAjustada") %>'></asp:Literal>
                                                        </td>

                                                        <td class="td-notas" id="tdParecerFinal" runat="server">
                                                            <asp:HiddenField ID="hfTudId" runat="server" Value='<%# Eval("tud_idResultado") %>' />
                                                            <asp:HiddenField ID="hfMtuId" runat="server" Value='<%# Eval("mtu_idResultado") %>' />
                                                            <asp:HiddenField ID="hfMtdId" runat="server" Value='<%# Eval("mtd_idResultado") %>' />
                                                            <asp:HiddenField ID="hfAtdId" runat="server" Value='<%# Eval("atd_idResultado") %>' />
                                                            <asp:HiddenField ID="hfFavId" runat="server" Value='<%# Eval("fav_idResultado") %>' />
                                                            <asp:HiddenField ID="hfAvaId" runat="server" Value='<%# Eval("ava_idResultado") %>' />
                                                            <asp:Label ID="lblParecerFinal" runat="server"></asp:Label>
                                                            <asp:DropDownList ID="ddlParecerFinal" runat="server" Style="margin: 0px;"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </AlternatingItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>

                        <div id="divLegenda" runat="server" visible="false" class="legenda">
                            <hr />
                            <b>
                                <asp:Label runat="server" ID="lblLegenda" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.lblLegenda.Text %>"></asp:Label></b>

                            <div class="borda-legenda">
                                <ul id="tbLegenda" runat="server">
                                    <li><i runat="server" id="lnAlunoPendencia"></i>
                                        <asp:Label runat="server" ID="litPendencia" Text="<%$ Resources:Mensagens, MSG_ALUNO_NAO_PENDENCIA %>"></asp:Label></li>
                                    <li><i runat="server" id="lnInativos"></i>
                                        <asp:Label runat="server" ID="litInativo" Text="<%$ Resources:Mensagens, MSG_ALUNO_INATIVO %>"></asp:Label></li>
                                    <li><i runat="server" id="lnAlunoForaRede"></i>
                                        <asp:Label runat="server" ID="litAlunoForaRede" Text="<%$ Resources:Mensagens, MSG_ALUNO_FORA_REDE %>"></asp:Label></li>
                                    <li><i runat="server" id="lnAlunoFrequencia"></i>
                                        <asp:Label runat="server" ID="litBaixaFreq" Text="<%$ Resources:Mensagens, MSG_ALUNO_BAIXA_FREQ %>"></asp:Label></li>
                                </ul>
                            </div>
                        </div>
                    </fieldset>
                </div>
                <div id="divTabs-1">
                    <fieldset id="fdsJustificativaPosConselho" runat="server" class="resumoDesempenho">
                        <div class="summaryMensagem">
                            <asp:Literal ID="litMensagemJustificativaPosConselho" runat="server" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.litMensagemJustificativaPosConselho.Text %>"></asp:Literal>
                        </div>
                        <asp:Repeater ID="rptJustificativaPosConselho" runat="server" OnItemDataBound="rptJustificativaPosConselho_ItemDataBound">
                            <HeaderTemplate>
                                <div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div runat="server" id="divPeriodoCalendario">
                                    <asp:Label ID="lblPeriodoCalendario" runat="server" Text='<%# Eval("cap_descricao") %>' Font-Bold="true"></asp:Label>
                                    <asp:ImageButton ID="btnTextoGrandeBimestre" runat="server" SkinID="btnExpandirCampo" />
                                    <asp:ImageButton ID="btnVoltaEstadoAnteriorTextoBimestre" runat="server" SkinID="btnComprimirCampo" Style="display: none" />
                                    <div class="textareaComInfo">
                                        <div id="divJustificativaPosConselho" runat="server" class="responsive-block">
                                            <asp:Label ID="lblJustificativaPosConselho" runat="server" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.lblJustificativaPosConselho.Text %>"
                                                AssociatedControlID="txtJustificativaPosConselho"></asp:Label>
                                            <asp:TextBox ID="txtJustificativaPosConselho" runat="server" TextMode="MultiLine" SkinID="text60c" Text='<%# Eval("aat_justificativaPosConselho").ToString() %>'></asp:TextBox>
                                            <asp:HiddenField ID="hdnTpcId" runat="server" Value='<%# Eval("tpc_id") %>' />
                                            <asp:HiddenField ID="hdnTurId" runat="server" Value='<%# Eval("tur_id") %>' />
                                            <asp:HiddenField ID="hdnMtuId" runat="server" Value='<%# Eval("mtu_id") %>' />
                                            <asp:HiddenField ID="hdnFavId" runat="server" Value='<%# Eval("fav_id") %>' />
                                            <asp:HiddenField ID="hdnAvaId" runat="server" Value='<%# Eval("ava_id") %>' />
                                            <asp:HiddenField ID="hdnAatId" runat="server" Value='<%# Eval("aat_id") %>' />
                                        </div>
                                    </div>
                                    <hr />
                                </div>
                            </ItemTemplate>
                            <FooterTemplate></div></FooterTemplate>
                        </asp:Repeater>
                    </fieldset>
                </div>
                <div id="divTabs-2">
                    <fieldset id="fdsDesempenhoAprendizado" runat="server" class="resumoDesempenho">
                        <asp:Repeater ID="rptResumoDesempenho" runat="server" OnItemDataBound="rptResumoDesempenho_ItemDataBound">
                            <HeaderTemplate>
                                <div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div runat="server" id="divPeriodoCalendario">
                                    <asp:Label ID="lblPeriodoCalendario" runat="server" Text='<%# Eval("cap_descricao") %>' Font-Bold="true"></asp:Label>
                                    <asp:ImageButton ID="btnTextoGrandeBimestre" runat="server" SkinID="btnExpandirCampo" />
                                    <asp:ImageButton ID="btnVoltaEstadoAnteriorTextoBimestre" runat="server" SkinID="btnComprimirCampo" Style="display: none" />
                                    <div class="textareaComInfo">
                                        <div id="divResumoDesempenho" runat="server" class="responsive-block">
                                            <asp:Label ID="lblResumoDesempenho" runat="server" Text="<%$ Resources:UserControl, AlunoEfetivacaoObservacao.UCAlunoEfetivacaoObservacao.lblResumoDesempenho.Text %>"
                                                AssociatedControlID="txtResumoDesempenho"></asp:Label>
                                            <asp:TextBox ID="txtResumoDesempenho" runat="server" TextMode="MultiLine" SkinID="text60c" Text='<%# Eval("ato_desempenhoAprendizado").ToString() %>' Visible="true"></asp:TextBox>
                                            <asp:HiddenField ID="hdnTpcId" runat="server" Value='<%# Eval("tpc_id") %>' />
                                            <asp:HiddenField ID="hdnTurId" runat="server" Value='<%# Eval("tur_id") %>' />
                                            <asp:HiddenField ID="hdnMtuId" runat="server" Value='<%# Eval("mtu_id") %>' />
                                            <asp:HiddenField ID="hdnFavId" runat="server" Value='<%# Eval("fav_id") %>' />
                                            <asp:HiddenField ID="hdnAvaId" runat="server" Value='<%# Eval("ava_id") %>' />
                                        </div>
                                    </div>
                                    <hr />
                                </div>
                            </ItemTemplate>
                            <FooterTemplate></div></FooterTemplate>
                        </asp:Repeater>
                        <br />
                        <div class="recomendacoes" id="divDesempenho" runat="server">
                            <h4>
                                <asp:Label ID="lblListaDesempenho" runat="server" Text="<%$ Resources:Mensagens, MSG_DESEMPENHOAPRENDIZADO %>"></asp:Label></h4>

                            <asp:UpdatePanel ID="updDesempenho" runat="server">
                                <ContentTemplate>
                                    <asp:Repeater ID="rptDesempenho" runat="server" OnItemDataBound="rptDesempenho_ItemDataBound">
                                        <HeaderTemplate>
                                            <ul class="lista-recomendacoes">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <li>
                                                <asp:LinkButton ID="lnkDesempenho" runat="server" Text='<%# Eval("tda_descricao").ToString() %>' Visible='<%# (!string.IsNullOrEmpty(Eval("tda_id").ToString()))? true: false %>'></asp:LinkButton>
                                            </li>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </ul>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:Label ID="lblInfoDesempenho" runat="server"></asp:Label>
                        </div>
                    </fieldset>
                </div>
                <div id="divTabs-3">
                    <fieldset id="fdsRecomendacoesAluno" runat="server" class="recomendacaoAluno">
                        <asp:Repeater ID="rptRecomendacaoAluno" runat="server" OnItemDataBound="rptRecomendacaoAluno_ItemDataBound">
                            <HeaderTemplate>
                                <div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div runat="server" id="divPeriodoCalendario">
                                    <asp:Label ID="lblPeriodoCalendario" runat="server" Text='<%# Eval("cap_descricao") %>' Font-Bold="true"></asp:Label>
                                    <asp:ImageButton ID="btnTextoGrandeBimestre" runat="server" SkinID="btnExpandirCampo" />
                                    <asp:ImageButton ID="btnVoltaEstadoAnteriorTextoBimestre" runat="server" SkinID="btnComprimirCampo" Style="display: none" />
                                    <div class="textareaComInfo">
                                        <div id="divRecomendacaoAluno" runat="server" class="responsive-block">
                                            <asp:Label ID="lblRecomendacaoAluno" runat="server" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.lblRecomendacaoAluno.Text %>" AssociatedControlID="txtRecomendacaoAluno"></asp:Label>
                                            <asp:TextBox ID="txtRecomendacaoAluno" runat="server" TextMode="MultiLine" SkinID="text60c" Text='<%# Eval("ato_recomendacaoAluno").ToString() %>'></asp:TextBox>
                                            <asp:HiddenField ID="hdnTpcId" runat="server" Value='<%# Eval("tpc_id") %>' />
                                            <asp:HiddenField ID="hdnTurId" runat="server" Value='<%# Eval("tur_id") %>' />
                                            <asp:HiddenField ID="hdnMtuId" runat="server" Value='<%# Eval("mtu_id") %>' />
                                            <asp:HiddenField ID="hdnFavId" runat="server" Value='<%# Eval("fav_id") %>' />
                                            <asp:HiddenField ID="hdnAvaId" runat="server" Value='<%# Eval("ava_id") %>' />
                                        </div>
                                    </div>
                                    <hr />
                                </div>
                            </ItemTemplate>
                            <FooterTemplate></div></FooterTemplate>
                        </asp:Repeater>
                        <br />
                        <div class="recomendacoes" id="divListaRecomendacaoAluno" runat="server">
                            <h4>
                                <asp:Literal ID="lblListaRecomendacaoAluno" runat="server" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.lblListaRecomendacaoAluno.Text %>"></asp:Literal></h4>

                            <asp:UpdatePanel ID="updRecomendacaoAluno" runat="server">
                                <ContentTemplate>
                                    <asp:Repeater ID="rptRecomendacao" runat="server" OnItemDataBound="rptRecomendacao_ItemDataBound">
                                        <HeaderTemplate>
                                            <ul class="lista-recomendacoes">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <li>
                                                <asp:LinkButton ID="lnkRecomendacaoAluno" Text='<%# Eval("rar_descricao").ToString() %>' Visible='<%# (!string.IsNullOrEmpty(Eval("rar_id").ToString()))? true: false %>' runat="server"></asp:LinkButton>
                                            </li>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </ul>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:Label ID="lblInfoRecomendacaoAluno" runat="server"></asp:Label>
                        </div>
                    </fieldset>
                </div>
                <div id="divTabs-4">
                    <fieldset id="fdsRecomendacoesPais" runat="server" class="recomendacaoResp">
                        <asp:Repeater ID="rptRecomendacaoResponsavel" runat="server" OnItemDataBound="rptRecomendacaoResponsavel_ItemDataBound">
                            <HeaderTemplate>
                                <div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div runat="server" id="divPeriodoCalendario">
                                    <asp:Label ID="lblPeriodoCalendario" runat="server" Text='<%# Eval("cap_descricao") %>' Font-Bold="true"></asp:Label>
                                    <asp:ImageButton ID="btnTextoGrandeBimestre" runat="server" SkinID="btnExpandirCampo" />
                                    <asp:ImageButton ID="btnVoltaEstadoAnteriorTextoBimestre" runat="server" SkinID="btnComprimirCampo" Style="display: none" />
                                    <div class="textareaComInfo">
                                        <div id="divRecomendacaoResp" runat="server" class="responsive-block">
                                            <asp:Label ID="lblRecomendacaoResp" runat="server" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.lblRecomendacaoResp.Text %>" AssociatedControlID="txtRecomendacaoResp"></asp:Label>
                                            <asp:TextBox ID="txtRecomendacaoResp" runat="server" TextMode="MultiLine" SkinID="text60c" Text='<%# Eval("ato_recomendacaoResponsavel").ToString() %>'></asp:TextBox>
                                            <asp:HiddenField ID="hdnTpcId" runat="server" Value='<%# Eval("tpc_id") %>' />
                                            <asp:HiddenField ID="hdnTurId" runat="server" Value='<%# Eval("tur_id") %>' />
                                            <asp:HiddenField ID="hdnMtuId" runat="server" Value='<%# Eval("mtu_id") %>' />
                                            <asp:HiddenField ID="hdnFavId" runat="server" Value='<%# Eval("fav_id") %>' />
                                            <asp:HiddenField ID="hdnAvaId" runat="server" Value='<%# Eval("ava_id") %>' />
                                        </div>
                                    </div>
                                    <hr />
                                </div>
                            </ItemTemplate>
                            <FooterTemplate></div></FooterTemplate>
                        </asp:Repeater>
                        <br />
                        <div class="recomendacoes" id="divListaRecomendacaoResp" runat="server">
                            <h4>
                                <asp:Literal ID="lblListaRecomendacaoResp" runat="server" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.lblListaRecomendacaoResp.Text %>"></asp:Literal></h4>

                            <asp:UpdatePanel ID="updRecomendacaoResp" runat="server">
                                <ContentTemplate>
                                    <asp:Repeater ID="rptRecomendacaoResp" runat="server" OnItemDataBound="rptRecomendacaoResp_ItemDataBound">
                                        <HeaderTemplate>
                                            <ul class="lista-recomendacoes">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <li>
                                                <asp:LinkButton ID="lnkRecomendacaoResp" runat="server" Text='<%# Eval("rar_descricao").ToString() %>' Visible='<%# (!string.IsNullOrEmpty(Eval("rar_id").ToString()))? true: false %>'></asp:LinkButton>
                                            </li>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </ul>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:Label ID="lblInfoRecomendacaoResp" runat="server"></asp:Label>
                        </div>
                    </fieldset>
                </div>
                <div id="divTabs-5">
                    <fieldset id="fdsAnotacoes" class="fieldset-anotacoes" runat="server">
                        <br />
                        <%--<legend>
                            <asp:Label ID="lblLegendAnotacao" runat="server" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.lblLegendAnotacao.Text %>"></asp:Label>
                        </legend>--%>
                        <fieldset>
                            <legend>
                                <asp:Label ID="lblLgdAnotacoesAulas" runat="server" Text="<%$ Resources:UserControl, UCAlunoAnotacoes.lblLgdAnotacoesAulas.Text %>"></asp:Label></legend>
                            <div class="fieldset-gestor-content">
                                <asp:GridView ID="grvAnotacoes" runat="server" AutoGenerateColumns="False"
                                    EmptyDataText="<%$ Resources:UserControl, UCAlunoAnotacoes.grvAnotacoes.EmptyDataText %>">
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.HeaderEscola.Text %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEscola" runat="server" Text='<%# Bind("esc_nome") %>' CssClass="wrap250px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="17%" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="tur_codigo" HeaderText="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.HeaderTurma.Text %>" ItemStyle-Width="5%" />
                                        <asp:BoundField DataField="tud_nome" HeaderText="<%$ Resources:Mensagens, MSG_DISCIPLINA %>" ItemStyle-Width="13%" />
                                        <asp:BoundField DataField="doc_id" HeaderText="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.HeaderDocente.Text %>" ItemStyle-Width="16%" />
                                        <asp:BoundField DataField="tau_data" HeaderText="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.HeaderDataAula.Text %>" DataFormatString="{0: dd/MM/yyyy}" ItemStyle-Width="80px" />
                                        <asp:TemplateField HeaderText="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.HeaderAnotacaoAluno.Text %>" HeaderStyle-CssClass="alinharHeader">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAnotacoes" runat="server" Text='<%# Bind("taa_anotacao") %>' CssClass="wrap600px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>
                        <div id="divAnotacoesGerais" runat="server">
                            <fieldset>
                                <legend>
                                    <asp:Label ID="lblLgdAnotacoesGerais" runat="server" Text="<%$ Resources:UserControl, UCAlunoAnotacoes.lblLgdAnotacoesGerais.Text %>"></asp:Label></legend>
                                <div class="fieldset-gestor-content">
                                    <asp:GridView ID="grvAnotacoesGerais" runat="server" AutoGenerateColumns="False"
                                        EmptyDataText="<%$ Resources:UserControl, UCAlunoAnotacoes.grvAnotacoesGerais.EmptyDataText %>" DataKeyNames="ano_id"
                                        OnRowDataBound="grvAnotacoesGerais_RowDataBound" OnRowCommand="grvAnotacoesGerais_RowCommand">
                                        <Columns>
                                            <asp:BoundField DataField="ano_dataAnotacao" HeaderText="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.HeaderDataAnotacao.Text %>" DataFormatString="{0: dd/MM/yyyy}" ItemStyle-Width="10%" />
                                            <asp:BoundField DataField="gru_nome" HeaderText="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.HeaderGrupo.Text %>" ItemStyle-Width="30%" />
                                            <asp:TemplateField HeaderText="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.HeaderAnotacao.Text %>" HeaderStyle-CssClass="alinharHeader">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAnotacoes" runat="server" Text='<%# Bind("ano_anotacao") %>' CssClass="wrap600px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.HeaderEditar.Text %>" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="70px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnEditar" SkinID="btEditar" runat="server" CommandName="Editar" />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.HeaderExcluir.Text %>" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="70px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnExcluir" SkinID="btExcluir" runat="server" CommandName="Deletar" />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div class="right">
                                    <asp:Button ID="btnAddAnotacao" runat="server" Text="<%$ Resources:UserControl, UCAlunoAnotacoes.btnAddAnotacao.Text %>" CausesValidation="False"
                                        OnClick="btnAddAnotacao_Click" />
                                </div>
                            </fieldset>
                        </div>
                    </fieldset>
                </div>
            </div>

                <input id="txtSelectedTab" type="hidden" runat="server" />
                <br />
                <asp:Label ID="lblHistoricoObservacao" runat="server" Text=""></asp:Label>
            </div>
            <div class="button-bar right area-botoes-bottom">
                <asp:Button ID="btnSalvar" runat="server" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.btnSalvar.Text %>" OnClick="btnSalvar_Click" />
            </div>
            <asp:HiddenField ID="hdnTpcIdFechamento" runat="server" Value="-1" />
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>

<div id="divCadastroAnotacao" title="Cadastro de anotação" class="hide divCadastroAnotacao">
    <asp:UpdatePanel ID="updCadastro" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMessageCadastro" runat="server" EnableViewState="false"></asp:Label>
            <asp:ValidationSummary runat="server" ID="vsAnotacao" ValidationGroup="Anotacao" />
            <asp:Label ID="lblDataAnotacao" runat="server" Text="<%$ Resources:UserControl, UCAlunoAnotacoes.lblDataAnotacao.Text %>"
                AssociatedControlID="txtDataAnotacao"></asp:Label>
            <asp:TextBox ID="txtDataAnotacao" runat="server" CssClass="maskData" SkinID="Data"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvDataAnotacao" runat="server" ErrorMessage="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.rfvDataAnotacao.ErrorMessage %>"
                ControlToValidate="txtDataAnotacao" ValidationGroup="Anotacao" Display="Dynamic">*</asp:RequiredFieldValidator>
            <asp:CustomValidator ID="cvDataAnotacao" runat="server" ControlToValidate="txtDataAnotacao"
                ValidationGroup="Anotacao" Display="Dynamic" ErrorMessage="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.cvDataAnotacao.ErrorMessage %>"
                OnServerValidate="ValidarData_ServerValidate">*</asp:CustomValidator>
            <asp:Label ID="lblAnotacao" runat="server" Text="<%$ Resources:UserControl, UCAlunoAnotacoes.lblAnotacao.Text %>"
                AssociatedControlID="txtAnotacao"></asp:Label>
            <%--skin text60C é o mesmo tamanho que o limite4000 mas não tem os eventos onkeypress e onkeyup usados no contador de caractere--%>
            <asp:TextBox ID="txtAnotacao" runat="server" TextMode="MultiLine" MaxLength="4000" SkinID="text60c"
                Text="" CssClass="wrap250px" onkeypress="LimitarCaracter(this,'contadesc3','4000');"
                onkeyup="LimitarCaracter(this,'contadesc3','4000');"></asp:TextBox>
            <span id="contadesc3" style="display: inline; font-size: 85%; position: relative; top: -8px;">0/4000</span>
            <asp:RequiredFieldValidator ID="rfvAnotacao" runat="server" ErrorMessage="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.rfvAnotacao.ErrorMessage %>"
                ControlToValidate="txtAnotacao" ValidationGroup="Anotacao" Display="Dynamic">*</asp:RequiredFieldValidator>
            <div class="right">
                <asp:Button ID="btnSalvarAnotacao" runat="server" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.btnSalvarAnotacao.Text %>" OnClick="btnSalvarAnotacao_Click"
                    ValidationGroup="Anotacao" />
                <asp:Button ID="btnCancelar" runat="server" Text="<%$ Resources:UserControl, UCAlunoEfetivacaoObservacaoGeral.btnCancelar.Text %>" CausesValidation="False" OnClientClick="var exibirMensagemConfirmacao=true;$('.divCadastroAnotacao').dialog('close'); return false;" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
