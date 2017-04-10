<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro" %>

<%@ PreviousPageType VirtualPath="~/Classe/LancamentoFrequenciaExterna/Busca.aspx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblMensagem" runat="server" EnableViewState="false"></asp:Label>
    <fieldset class="fieldset-fechamento-gestor">
        <legend><%= GetGlobalResourceObject("GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro", "lit_9.Text").ToString() %></legend>
        <div class="dados-aluno clearfix">
            <br />
            <div class="div-inline">
                <asp:Image runat="server" ID="imgFotoAluno" />
            </div>
            <div class="div-inline">
                <asp:Label ID="lblNomeAlunoTitulo" runat="server" Text="<%$ Resources:GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro, lblNomeAlunoTitulo.Text %>" CssClass="lbl-negrito"></asp:Label>
                <asp:Label ID="lblNomeAluno" runat="server"></asp:Label>
                <br />
                <asp:Label ID="lblTurmaTitulo" runat="server" Text="<%$ Resources:GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro, lblTurmaTitulo.Text %>" CssClass="lbl-negrito"></asp:Label>
                <asp:Label ID="lblTurma" runat="server"></asp:Label>
                <br />
                <asp:Label ID="lblNumeroChamadaTitulo" runat="server" Text="<%$ Resources:GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro, lblNumeroChamadaTitulo.Text %>" CssClass="lbl-negrito"></asp:Label>
                <asp:Label ID="lblNumeroChamada" runat="server"></asp:Label>
                <br />
                <asp:Label ID="lblCodigoEolTitulo" runat="server" Text="<%$ Resources:GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro, lblCodigoEolTitulo.Text %>" CssClass="lbl-negrito"></asp:Label>
                <asp:Label ID="lblCodigoEol" runat="server"></asp:Label>
                <br />
                <br />
            </div>
        </div>
        <div class="area-form">
            <fieldset id="fdsBoletim" runat="server">
                <div id="divBoletim" runat="server">
                    <div class="div-gestor-scroll" id="divDisciplinas" runat="server">
                        <table class="table-boletim">
                            <thead>
                                <tr>
                                    <th rowspan="2" class="th-disciplina">
                                        <asp:Label runat="server" ID="lblDisp" Text="<%$ Resources:Mensagens, MSG_DISCIPLINA %>"></asp:Label>
                                    </th>
                                    <asp:Repeater ID="rptPeriodosNomes" runat="server">
                                        <ItemTemplate>
                                            <th class="th-periodo" runat="server" colspan="3">
                                                <span><%#Eval("tpc_nome") %></span>
                                            </th>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tr>
                                <tr>
                                    <asp:Repeater ID="rptPeriodosColunasFixas" runat="server">
                                        <ItemTemplate>
                                            <th class="th-aula-prevista">
                                                <asp:Label runat="server" ID="lblQtdAulasPrevistas" Text="<%$ Resources:GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro, lblQtdAulasPrevistas.Text %>"></asp:Label>
                                            </th>
                                            <th class="th-aulas">
                                                <asp:Label runat="server" ID="lblQtdAulas" Text="<%$ Resources:GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro, lblQtdAulas.Text %>"></asp:Label>
                                            </th>
                                            <th class="th-faltas">
                                                <asp:Label runat="server" ID="lblQtdFaltas" Text="<%$ Resources:GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro, lblQtdFaltas.Text %>"></asp:Label>
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
                                            <asp:Repeater ID="rptFrequenciaDisciplina" runat="server" OnItemDataBound="rptFrequenciaDisciplina_ItemDataBound" DataSource='<%#Eval("frequencias") %>'>
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnMtuId" runat="server" Value='<%#Eval("frequencia.mtu_id") %>' />
                                                    <asp:HiddenField ID="hdnMtdId" runat="server" Value='<%#Eval("frequencia.mtd_id") %>' />
                                                    <asp:HiddenField ID="hdnMtdIdReg" runat="server" Value='<%#Eval("frequencia.mtd_idRegencia") %>' />
                                                    <asp:HiddenField ID="hdnTpc" runat="server" Value='<%#Eval("tpc_id") %>' />
                                                    <asp:HiddenField ID="hdnAfx" runat="server" Value='<%#Eval("frequencia.afx_id") %>' />
                                                    <asp:HiddenField ID="hdnTudId" runat="server" Value='<%#Eval("frequencia.tud_id") %>' />
                                                    <asp:HiddenField ID="hdnTudIdRegencia" runat="server" Value='<%#Eval("frequencia.tud_idRegencia") %>' />
                                                    <asp:HiddenField ID="hdnTudTipo" runat="server" Value='<%#Eval("frequencia.tud_tipo") %>' />
                                                    <asp:HiddenField ID="hdnPossuiAulasPrev" runat="server" Value='<%#Eval("frequencia.possuiLancamentoAulasPrevistas") %>' />
                                                    <asp:HiddenField ID="hdnAulasPrev" runat="server" Value='<%#Eval("frequencia.numeroAulasPrevistas") %>' />
                                                    <td class="td-aulasPrevistas" id="tdAulasPrevistas" runat="server">
                                                        <%#Eval("frequencia.numeroAulasPrevistas")%>
                                                    </td>
                                                    <td class="td-aulas" id="tdAulas" runat="server">
                                                        <asp:TextBox ID="txtAulas" runat="server" Text='<%#Eval("frequencia.numeroAulas")%>' MaxLength="5" SkinID="Numerico2c"></asp:TextBox>
                                                    </td>
                                                    <td class="td-faltas" id="tdFaltas" runat="server">
                                                        <asp:TextBox ID="txtFaltas" runat="server" Text='<%#Eval("frequencia.numeroFaltas")%>' MaxLength="5" SkinID="Numerico2c"></asp:TextBox>
                                                        <asp:Image ID="imgAvisoAulasPrevistas" runat="server" SkinID="imgAviso" CssClass="hide" Width="16px" Height="16px" ImageAlign="AbsMiddle" ToolTip="<%$ Resources:GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro, imgAvisoAulasPrevistas.ToolTip %>" />
                                                    </td>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr class='tr-disciplina alternada'>
                                            <th id="tdNomeDiciplina" runat="server" class="th-disciplina">
                                                <asp:Literal ID="litNomeDisciplina" runat="server" Text='<%#Eval("Disciplina")%>'></asp:Literal>
                                            </th>
                                            <asp:Repeater ID="rptFrequenciaDisciplina" runat="server" OnItemDataBound="rptFrequenciaDisciplina_ItemDataBound" DataSource='<%#Eval("frequencias") %>'>
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnMtuId" runat="server" Value='<%#Eval("frequencia.mtu_id") %>' />
                                                    <asp:HiddenField ID="hdnMtdId" runat="server" Value='<%#Eval("frequencia.mtd_id") %>' />
                                                    <asp:HiddenField ID="hdnMtdIdReg" runat="server" Value='<%#Eval("frequencia.mtd_idRegencia") %>' />
                                                    <asp:HiddenField ID="hdnTpc" runat="server" Value='<%#Eval("tpc_id") %>' />
                                                    <asp:HiddenField ID="hdnAfx" runat="server" Value='<%#Eval("frequencia.afx_id") %>' />
                                                    <asp:HiddenField ID="hdnTudId" runat="server" Value='<%#Eval("frequencia.tud_id") %>' />
                                                    <asp:HiddenField ID="hdnTudIdRegencia" runat="server" Value='<%#Eval("frequencia.tud_idRegencia") %>' />
                                                    <asp:HiddenField ID="hdnTudTipo" runat="server" Value='<%#Eval("frequencia.tud_tipo") %>' />
                                                    <asp:HiddenField ID="hdnPossuiAulasPrev" runat="server" Value='<%#Eval("frequencia.possuiLancamentoAulasPrevistas") %>' />
                                                    <asp:HiddenField ID="hdnAulasPrev" runat="server" Value='<%#Eval("frequencia.numeroAulasPrevistas") %>' />
                                                    <td class="td-aulasPrevistas" id="tdAulasPrevistas" runat="server">
                                                        <%#Eval("frequencia.numeroAulasPrevistas")%>
                                                    </td>
                                                    <td class="td-aulas" id="tdAulas" runat="server">
                                                        <asp:TextBox ID="txtAulas" runat="server" Text='<%#Eval("frequencia.numeroAulas")%>' MaxLength="5" SkinID="Numerico2c"></asp:TextBox>
                                                    </td>
                                                    <td class="td-faltas" id="tdFaltas" runat="server">
                                                        <asp:TextBox ID="txtFaltas" runat="server" Text='<%#Eval("frequencia.numeroFaltas")%>' MaxLength="5" SkinID="Numerico2c"></asp:TextBox>
                                                        <asp:Image ID="imgAvisoAulasPrevistas" runat="server" SkinID="imgAviso" CssClass="hide" Width="16px" Height="16px" ImageAlign="AbsMiddle" ToolTip="<%$ Resources:GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro, imgAvisoAulasPrevistas.ToolTip %>" />
                                                    </td>
                                                </ItemTemplate>
                                            </asp:Repeater>
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
                                                <th id="Th2" class="th-periodo" runat="server" colspan="3">
                                                    <span><%#Eval("tpc_nome") %></span>
                                                </th>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tr>
                                    <tr>
                                        <asp:Repeater ID="rptPeriodosColunasFixasEnriquecimento" runat="server">
                                            <ItemTemplate>
                                                <th class="th-aula-prevista">
                                                    <asp:Label runat="server" ID="lblQtdAulasPrevistas" Text="<%$ Resources:GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro, lblQtdAulasPrevistas.Text %>"></asp:Label>
                                                </th>
                                                <th class="th-aulas">
                                                    <asp:Label runat="server" ID="lblQtdAulas" Text="<%$ Resources:GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro, lblQtdAulas.Text %>"></asp:Label>
                                                </th>
                                                <th class="th-faltas">
                                                    <asp:Label runat="server" ID="lblQtdFaltas" Text="<%$ Resources:GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro, lblQtdFaltas.Text %>"></asp:Label>
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
                                                <asp:Repeater ID="rptFrequenciaDisciplina" OnItemDataBound="rptFrequenciaDisciplina_ItemDataBound" runat="server" DataSource='<%#Eval("frequencias") %>'>
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnMtuId" runat="server" Value='<%#Eval("frequencia.mtu_id") %>' />
                                                        <asp:HiddenField ID="hdnMtdId" runat="server" Value='<%#Eval("frequencia.mtd_id") %>' />
                                                        <asp:HiddenField ID="hdnTpc" runat="server" Value='<%#Eval("tpc_id") %>' />
                                                        <asp:HiddenField ID="hdnAfx" runat="server" Value='<%#Eval("frequencia.afx_id") %>' />
                                                        <asp:HiddenField ID="hdnTudId" runat="server" Value='<%#Eval("frequencia.tud_id") %>' />
                                                        <asp:HiddenField ID="hdnPossuiAulasPrev" runat="server" Value='<%#Eval("frequencia.possuiLancamentoAulasPrevistas") %>' />
                                                        <asp:HiddenField ID="hdnAulasPrev" runat="server" Value='<%#Eval("frequencia.numeroAulasPrevistas") %>' />
                                                        <td class="td-aulasPrevistas" id="tdAulasPrevistas" runat="server">
                                                            <%#Eval("frequencia.numeroAulasPrevistas")%>
                                                        </td>
                                                        <td class="td-aulas" id="tdAulas" runat="server">
                                                            <asp:TextBox ID="txtAulas" runat="server" Text='<%#Eval("frequencia.numeroAulas")%>' MaxLength="5" SkinID="Numerico2c"></asp:TextBox>
                                                        </td>
                                                        <td class="td-faltas" id="tdFaltas" runat="server">
                                                            <asp:TextBox ID="txtFaltas" runat="server" Text='<%#Eval("frequencia.numeroFaltas")%>' MaxLength="5" SkinID="Numerico2c"></asp:TextBox>
                                                            <asp:Image ID="imgAvisoAulasPrevistas" runat="server" SkinID="imgAviso" CssClass="hide" Width="16px" Height="16px" ImageAlign="AbsMiddle" ToolTip="<%$ Resources:GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro, imgAvisoAulasPrevistas.ToolTip %>" />
                                                        </td>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr class='tr-disciplina alternada'>
                                                <th id="tdNomeDiciplina" runat="server" class="th-disciplina">
                                                    <asp:Literal ID="litNomeDisciplina" runat="server" Text='<%#Eval("Disciplina")%>'></asp:Literal>
                                                </th>
                                                <asp:Repeater ID="rptFrequenciaDisciplina" OnItemDataBound="rptFrequenciaDisciplina_ItemDataBound" runat="server" DataSource='<%#Eval("frequencias") %>'>
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnMtuId" runat="server" Value='<%#Eval("frequencia.mtu_id") %>' />
                                                        <asp:HiddenField ID="hdnMtdId" runat="server" Value='<%#Eval("frequencia.mtd_id") %>' />
                                                        <asp:HiddenField ID="hdnTpc" runat="server" Value='<%#Eval("tpc_id") %>' />
                                                        <asp:HiddenField ID="hdnAfx" runat="server" Value='<%#Eval("frequencia.afx_id") %>' />
                                                        <asp:HiddenField ID="hdnTudId" runat="server" Value='<%#Eval("frequencia.tud_id") %>' />
                                                        <asp:HiddenField ID="hdnPossuiAulasPrev" runat="server" Value='<%#Eval("frequencia.possuiLancamentoAulasPrevistas") %>' />
                                                        <asp:HiddenField ID="hdnAulasPrev" runat="server" Value='<%#Eval("frequencia.numeroAulasPrevistas") %>' />
                                                        <td class="td-aulasPrevistas" id="tdAulasPrevistas" runat="server">
                                                            <%#Eval("frequencia.numeroAulasPrevistas")%>
                                                        </td>
                                                        <td class="td-aulas" id="tdAulas" runat="server">
                                                            <asp:TextBox ID="txtAulas" runat="server" Text='<%#Eval("frequencia.numeroAulas")%>' MaxLength="5" SkinID="Numerico2c"></asp:TextBox>
                                                        </td>
                                                        <td class="td-faltas" id="tdFaltas" runat="server">
                                                            <asp:TextBox ID="txtFaltas" runat="server" Text='<%#Eval("frequencia.numeroFaltas")%>' MaxLength="5" SkinID="Numerico2c"></asp:TextBox>
                                                            <asp:Image ID="imgAvisoAulasPrevistas" runat="server" SkinID="imgAviso" CssClass="hide" Width="16px" Height="16px" ImageAlign="AbsMiddle" ToolTip="<%$ Resources:GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro, imgAvisoAulasPrevistas.ToolTip %>" />
                                                        </td>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tr>
                                        </AlternatingItemTemplate>
                                    </asp:Repeater>

                                    <asp:Repeater ID="rptDisciplinasExperiencias" runat="server" OnItemDataBound="rptDisciplinasEnriquecimentoCurricular_ItemDataBound">
                                        <ItemTemplate>
                                            <tr class='tr-disciplina'>
                                                <th id="tdNomeDiciplina" runat="server" class="th-disciplina">
                                                    <asp:Literal ID="litNomeDisciplina" runat="server" Text='<%#Eval("Disciplina")%>'></asp:Literal>
                                                </th>
                                                <asp:Repeater ID="rptFrequenciaDisciplina" OnItemDataBound="rptFrequenciaDisciplina_ItemDataBound" runat="server" DataSource='<%#Eval("frequencias") %>'>
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnMtuId" runat="server" Value='<%#Eval("frequencia.mtu_id") %>' />
                                                        <asp:HiddenField ID="hdnMtdId" runat="server" Value='<%#Eval("frequencia.mtd_id") %>' />
                                                        <asp:HiddenField ID="hdnTpc" runat="server" Value='<%#Eval("tpc_id") %>' />
                                                        <asp:HiddenField ID="hdnAfx" runat="server" Value='<%#Eval("frequencia.afx_id") %>' />
                                                        <asp:HiddenField ID="hdnTudId" runat="server" Value='<%#Eval("frequencia.tud_id") %>' />
                                                        <asp:HiddenField ID="hdnPossuiAulasPrev" runat="server" Value='<%#Eval("frequencia.possuiLancamentoAulasPrevistas") %>' />
                                                        <asp:HiddenField ID="hdnAulasPrev" runat="server" Value='<%#Eval("frequencia.numeroAulasPrevistas") %>' />
                                                        <td class="td-aulasPrevistas" id="tdAulasPrevistas" runat="server">
                                                            <%#Eval("frequencia.numeroAulasPrevistas")%>
                                                        </td>
                                                        <td class="td-aulas" id="tdAulas" runat="server">
                                                            <asp:TextBox ID="txtAulas" runat="server" Text='<%#Eval("frequencia.numeroAulas")%>' MaxLength="5" SkinID="Numerico2c"></asp:TextBox>
                                                        </td>
                                                        <td class="td-faltas" id="tdFaltas" runat="server">
                                                            <asp:TextBox ID="txtFaltas" runat="server" Text='<%#Eval("frequencia.numeroFaltas")%>' MaxLength="5" SkinID="Numerico2c"></asp:TextBox>
                                                            <asp:Image ID="imgAvisoAulasPrevistas" runat="server" SkinID="imgAviso" CssClass="hide" Width="16px" Height="16px" ImageAlign="AbsMiddle" ToolTip="<%$ Resources:GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro, imgAvisoAulasPrevistas.ToolTip %>" />
                                                        </td>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr class='tr-disciplina alternada'>
                                                <th id="tdNomeDiciplina" runat="server" class="th-disciplina">
                                                    <asp:Literal ID="litNomeDisciplina" runat="server" Text='<%#Eval("Disciplina")%>'></asp:Literal>
                                                </th>
                                                <asp:Repeater ID="rptFrequenciaDisciplina" OnItemDataBound="rptFrequenciaDisciplina_ItemDataBound" runat="server" DataSource='<%#Eval("frequencias") %>'>
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnMtuId" runat="server" Value='<%#Eval("frequencia.mtu_id") %>' />
                                                        <asp:HiddenField ID="hdnMtdId" runat="server" Value='<%#Eval("frequencia.mtd_id") %>' />
                                                        <asp:HiddenField ID="hdnTpc" runat="server" Value='<%#Eval("tpc_id") %>' />
                                                        <asp:HiddenField ID="hdnAfx" runat="server" Value='<%#Eval("frequencia.afx_id") %>' />
                                                        <asp:HiddenField ID="hdnTudId" runat="server" Value='<%#Eval("frequencia.tud_id") %>' />
                                                        <asp:HiddenField ID="hdnPossuiAulasPrev" runat="server" Value='<%#Eval("frequencia.possuiLancamentoAulasPrevistas") %>' />
                                                        <asp:HiddenField ID="hdnAulasPrev" runat="server" Value='<%#Eval("frequencia.numeroAulasPrevistas") %>' />
                                                        <td class="td-aulasPrevistas" id="tdAulasPrevistas" runat="server">
                                                            <%#Eval("frequencia.numeroAulasPrevistas")%>
                                                        </td>
                                                        <td class="td-aulas" id="tdAulas" runat="server">
                                                            <asp:TextBox ID="txtAulas" runat="server" Text='<%#Eval("frequencia.numeroAulas")%>' MaxLength="5" SkinID="Numerico2c"></asp:TextBox>
                                                        </td>
                                                        <td class="td-faltas" id="tdFaltas" runat="server">
                                                            <asp:TextBox ID="txtFaltas" runat="server" Text='<%#Eval("frequencia.numeroFaltas")%>' MaxLength="5" SkinID="Numerico2c"></asp:TextBox>
                                                            <asp:Image ID="imgAvisoAulasPrevistas" runat="server" SkinID="imgAviso" CssClass="hide" Width="16px" Height="16px" ImageAlign="AbsMiddle" ToolTip="<%$ Resources:GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro, imgAvisoAulasPrevistas.ToolTip %>" />
                                                        </td>
                                                    </ItemTemplate>
                                                </asp:Repeater>
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
                                            <asp:Label runat="server" ID="lblEI" Text="<%$ Resources:GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro, lblEI.Text %>"></asp:Label>
                                        </th>
                                        <asp:Repeater ID="rptPeriodosNomesEI" runat="server">
                                            <ItemTemplate>
                                                <th id="Th2" class="th-periodo" runat="server" colspan="3">
                                                    <span><%#Eval("tpc_nome") %></span>
                                                </th>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tr>
                                    <tr>
                                        <asp:Repeater ID="rptPeriodosColunasFixasEI" runat="server">
                                            <ItemTemplate>
                                                <th class="th-aula-prevista">
                                                    <asp:Label runat="server" ID="lblQtdAulasPrevistas" Text="<%$ Resources:GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro, lblQtdAulasPrevistas.Text %>"></asp:Label>
                                                </th>
                                                <th class="th-aulas">
                                                    <asp:Label runat="server" ID="lblQtdAulas" Text="<%$ Resources:GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro, lblQtdAulas.Text %>"></asp:Label>
                                                </th>
                                                <th class="th-faltas">
                                                    <asp:Label runat="server" ID="lblQtdFaltas" Text="<%$ Resources:GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro, lblQtdFaltas.Text %>"></asp:Label>
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
                                                <asp:Repeater ID="rptNotasDisciplina" OnItemDataBound="rptFrequenciaDisciplina_ItemDataBound" runat="server" DataSource='<%#Eval("frequencias") %>'>
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnMtuId" runat="server" Value='<%#Eval("frequencia.mtu_id") %>' />
                                                        <asp:HiddenField ID="hdnMtdId" runat="server" Value='<%#Eval("frequencia.mtd_id") %>' />
                                                        <asp:HiddenField ID="hdnMtdIdReg" runat="server" Value='<%#Eval("frequencia.mtd_idRegencia") %>' />
                                                        <asp:HiddenField ID="hdnTpc" runat="server" Value='<%#Eval("tpc_id") %>' />
                                                        <asp:HiddenField ID="hdnAfx" runat="server" Value='<%#Eval("frequencia.afx_id") %>' />
                                                        <asp:HiddenField ID="hdnTudId" runat="server" Value='<%#Eval("frequencia.tud_id") %>' />
                                                        <asp:HiddenField ID="hdnTudIdRegencia" runat="server" Value='<%#Eval("frequencia.tud_idRegencia") %>' />
                                                        <asp:HiddenField ID="hdnTudTipo" runat="server" Value='<%#Eval("frequencia.tud_tipo") %>' />
                                                        <asp:HiddenField ID="hdnPossuiAulasPrev" runat="server" Value='<%#Eval("frequencia.possuiLancamentoAulasPrevistas") %>' />
                                                        <asp:HiddenField ID="hdnAulasPrev" runat="server" Value='<%#Eval("frequencia.numeroAulasPrevistas") %>' />
                                                        <td class="td-aulasPrevistas" id="tdAulasPrevistas" runat="server">
                                                            <%#Eval("frequencia.numeroAulasPrevistas")%>
                                                        </td>
                                                        <td class="td-aulas" id="tdAulas" runat="server">
                                                            <asp:TextBox ID="txtAulas" runat="server" Text='<%#Eval("frequencia.numeroAulas")%>' MaxLength="5" SkinID="Numerico2c"></asp:TextBox>
                                                        </td>
                                                        <td class="td-faltas" id="tdFaltas" runat="server">
                                                            <asp:TextBox ID="txtFaltas" runat="server" Text='<%#Eval("frequencia.numeroFaltas")%>' MaxLength="5" SkinID="Numerico2c"></asp:TextBox>
                                                            <asp:Image ID="imgAvisoAulasPrevistas" runat="server" SkinID="imgAviso" CssClass="hide" Width="16px" Height="16px" ImageAlign="AbsMiddle" ToolTip="<%$ Resources:GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro, imgAvisoAulasPrevistas.ToolTip %>" />
                                                        </td>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr class='tr-disciplina alternada'>
                                                <th id="tdNomeDiciplina" runat="server" class="th-disciplina">
                                                    <asp:Literal ID="litNomeDisciplina" runat="server" Text='<%#Eval("Disciplina")%>'></asp:Literal>
                                                </th>
                                                <asp:Repeater ID="rptNotasDisciplina" OnItemDataBound="rptFrequenciaDisciplina_ItemDataBound" runat="server" DataSource='<%#Eval("frequencias") %>'>
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnMtuId" runat="server" Value='<%#Eval("frequencia.mtu_id") %>' />
                                                        <asp:HiddenField ID="hdnMtdId" runat="server" Value='<%#Eval("frequencia.mtd_id") %>' />
                                                        <asp:HiddenField ID="hdnMtdIdReg" runat="server" Value='<%#Eval("frequencia.mtd_idRegencia") %>' />
                                                        <asp:HiddenField ID="hdnTpc" runat="server" Value='<%#Eval("tpc_id") %>' />
                                                        <asp:HiddenField ID="hdnAfx" runat="server" Value='<%#Eval("frequencia.afx_id") %>' />
                                                        <asp:HiddenField ID="hdnTudId" runat="server" Value='<%#Eval("frequencia.tud_id") %>' />
                                                        <asp:HiddenField ID="hdnTudIdRegencia" runat="server" Value='<%#Eval("frequencia.tud_idRegencia") %>' />
                                                        <asp:HiddenField ID="hdnTudTipo" runat="server" Value='<%#Eval("frequencia.tud_tipo") %>' />
                                                        <asp:HiddenField ID="hdnPossuiAulasPrev" runat="server" Value='<%#Eval("frequencia.possuiLancamentoAulasPrevistas") %>' />
                                                        <asp:HiddenField ID="hdnAulasPrev" runat="server" Value='<%#Eval("frequencia.numeroAulasPrevistas") %>' />
                                                        <td class="td-aulasPrevistas" id="tdAulasPrevistas" runat="server">
                                                            <%#Eval("frequencia.numeroAulasPrevistas")%>
                                                        </td>
                                                        <td class="td-aulas" id="tdAulas" runat="server">
                                                            <asp:TextBox ID="txtAulas" runat="server" Text='<%#Eval("frequencia.numeroAulas")%>' MaxLength="5" SkinID="Numerico2c"></asp:TextBox>
                                                        </td>
                                                        <td class="td-faltas" id="tdFaltas" runat="server">
                                                            <asp:TextBox ID="txtFaltas" runat="server" Text='<%#Eval("frequencia.numeroFaltas")%>' MaxLength="5" SkinID="Numerico2c"></asp:TextBox>
                                                            <asp:Image ID="imgAvisoAulasPrevistas" runat="server" SkinID="imgAviso" CssClass="hide" Width="16px" Height="16px" ImageAlign="AbsMiddle" ToolTip="<%$ Resources:GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro, imgAvisoAulasPrevistas.ToolTip %>" />
                                                        </td>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tr>
                                        </AlternatingItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
                <hr />
                <div id="divAvisoAulasPrevistas" runat="server" style="float: left; width: 100%;">
                    <asp:Image ID="imgLegendaAvisoAulasPrevistas" runat="server" SkinID="imgAviso" Width="16px" Height="16px" ImageAlign="Top"
                        ToolTip="<%$ Resources:GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro, ctrl_394.ToolTip %>" />
                    <asp:Literal ID="lit2" runat="server" Text="<%$ Resources:GestaoEscolar.Classe.LancamentoFrequenciaExterna.Cadastro, lit2.Text %>"></asp:Literal>
                </div>
            </fieldset>
        </div>
        <div class="button-bar right area-botoes-bottom">
            <asp:Button ID="btnSalvar" runat="server" Text="<%$ Resources:Padrao, Padrao.Salvar.Text %>" OnClick="btnSalvar_Click" />
            <asp:Button ID="btnCancelar" runat="server" Text="<%$ Resources:Padrao, Padrao.Cancelar.Text %>" CausesValidation="false" OnClick="btnCancelar_Click" />
        </div>
    </fieldset>
</asp:Content>
