<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="CadastroGestor.aspx.cs" Inherits="GestaoEscolar.Classe.Efetivacao.CadastroGestor" %>

<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="UCComboUAEscola" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboCalendario.ascx" TagName="UCComboCalendario" TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCTurma.ascx" TagName="UCComboTurma" TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/AlunoEfetivacaoObservacao/UCAlunoEfetivacaoObservacaoGeral.ascx" TagName="UCAlunoEfetivacaoObservacaoGeral" TagPrefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="upnMensagem" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Label ID="lblMensagem" runat="server" EnableViewState="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="vsMensagemValidacao" runat="server" ValidationGroup="FechamentoGestor" />
    <fieldset>
        <legend id="lgdTituloPesquisa" runat="server">
            <asp:Label ID="lblLegendFechamentoGestor" runat="server" Text="<%$ Resources:Fechamento, FechamentoGestor.Cadastro.lblLegendFechamentoGestor.Text %>"></asp:Label>
        </legend>
        <div id="divPesquisa" runat="server">
            <asp:UpdatePanel ID="upnBusca" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:CheckBox ID="chkTurmaExtinta" runat="server" Text="<%$ Resources:Fechamento, FechamentoGestor.Cadastro.chkTurmaExtinta.Text %>" AutoPostBack="true" OnCheckedChanged="chkTurmaExtinta_CheckedChanged" />
                    <uc1:UCComboUAEscola ID="UCComboUAEscola1" runat="server" CarregarEscolaAutomatico="true"
                        ObrigatorioEscola="false" ObrigatorioUA="false" MostrarMessageSelecioneEscola="true"
                        MostrarMessageSelecioneUA="true" />
                    <uc2:UCComboCalendario ID="UCComboCalendario1" runat="server" />
                    <uc3:UCComboTurma ID="UCComboTurma1" runat="server" MostrarMessageSelecione="true" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="right">
            <asp:Button ID="btnExibir" runat="server" Text="Exibir" OnClick="btnExibir_Click" ValidationGroup="FechamentoGestor" />
            <asp:HiddenField ID="hdnFormatacaoNota" runat="server" />
        </div>
    </fieldset>
    <fieldset id="fdsSemAlunos" runat="server" visible="false">
        <asp:Label ID="lblMensagemSemAlunos" runat="server" Text="<%$ Resources:Fechamento, FechamentoGestor.Cadastro.lblMensagemSemAlunos.Text %>"></asp:Label>
    </fieldset>
    <div id="divResultados" runat="server" visible="false">
        <fieldset class="fieldset-fechamento-gestor">
            <legend>
                <asp:UpdatePanel ID="updLegendDadosTurma" runat="server">
                    <ContentTemplate>
                        <small>
                            <asp:LinkButton ID="lkbDadosTurma" runat="server" Text="<%$ Resources:Fechamento, FechamentoGestor.Cadastro.lkbDadosTurma.Text %>" OnClick="lkbDadosTurma_Click"></asp:LinkButton>
                        </small>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </legend>
            <div class="fieldset-gestor-content area-gestor-fechamento">
                <div class="row collapse">
                    <div class="small-12 medium-3 columns">
                        <asp:UpdatePanel ID="updDadosAluno" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Repeater ID="rptAlunos" runat="server" OnItemDataBound="rptAlunos_ItemDataBound">
                                    <HeaderTemplate>
                                        <table class="table-alunos">
                                            <thead>
                                                <tr>
                                                    <th>
                                                        <abbr runat="server" ID="lblNumeroChamada" title="<%$ Resources:Fechamento, FechamentoGestor.Cadastro.lblNumeroChamada.Text %>">Nº.</abbr>
                                                    </th>
                                                    <th>
                                                        <asp:Label ID="lblAlunos" runat="server" Text="<%$ Resources:Fechamento, FechamentoGestor.Cadastro.lblAlunos.Text %>"></asp:Label>
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr id="trNomeAluno" runat="server">
                                            <td style="text-align: center;">
                                                <asp:Label ID="lblLegendaAluno" runat="server" CssClass="label-legenda-aluno"></asp:Label>
                                                <asp:Literal ID="ltrNumeroChamada" runat="server" Text='<%#Bind("mtu_numeroChamada") %>'></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:HiddenField ID="hdnAluId" runat="server" Value='<%#Bind("alu_id") %>' />
                                                <asp:HiddenField ID="hdnMtuId" runat="server" Value='<%#Bind("mtu_id") %>' />
                                                <asp:ImageButton ID="imgStatusFechamento" runat="server" Visible="false" ToolTip='<%#Bind("DisciplinaPendencia") %>' CssClass="imagem-status-fechamento" />
                                                <asp:LinkButton ID="lblNomeAluno" runat="server" Text='<%#Bind("pes_nome_infoCompl") %>' OnClick="lblNomeAluno_Click"></asp:LinkButton>

                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>
                                    </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="small-12 medium-9 columns">
                        <div class="div-gestor-content divScrollResponsivo">
                            <asp:UpdatePanel ID="updMensagemSelecionarAluno" runat="server">
                                <ContentTemplate>
                                    <div id="spanMensagemSelecionarAluno" runat="server" class="mensagem-informativa">
                                        <asp:Literal ID="litMensagemSelecionarAluno" runat="server" Text="<%$ Resources:Fechamento, FechamentoGestor.Cadastro.litMensagemSelecionarAluno.Text %>"></asp:Literal>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <uc4:UCAlunoEfetivacaoObservacaoGeral ID="UCAlunoEfetivacaoObservacaoGeral1" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
    </div>

    <div id="divDadosTurma" class="hide fieldset-fechamento-gestor" title="Informações da turma">
        <asp:UpdatePanel ID="updDadosTurma" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Repeater ID="rptDadosTurma" runat="server" OnItemDataBound="rptDadosTurma_ItemDataBound">
                    <HeaderTemplate>
                        <table class="table-boletim">
                            <tr>
                                <th class="th-disciplina" rowspan="2">
                                    <asp:Label runat="server" ID="lblDisp" Text="<%$ Resources:Mensagens, MSG_DISCIPLINA %>"></asp:Label>
                                </th>
                                <asp:Repeater ID="rptPeriodoCalendario" runat="server">
                                    <ItemTemplate>
                                        <th class="th-periodo" colspan="2">
                                            <asp:Label ID="lblNomeTpc" runat="server" Text='<%#Bind("cap_descricao") %>'></asp:Label></th>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tr>
                            <tr>
                                <asp:Repeater ID="rptPeriodoCalendarioColunasFixas" runat="server">
                                    <ItemTemplate>
                                        <th class="th-aula-prevista">
                                            <asp:Label ID="lblTituloAulasPrevistas" runat="server" Text="<%$ Resources:Fechamento, FechamentoGestor.Cadastro.lblTituloAulasPrevistas.Text %>" Font-Size="Small"></asp:Label></th>
                                        <th class="th-aula-dada">
                                            <asp:Label ID="lblTituloAulasDadas" runat="server" Text="<%$ Resources:Fechamento, FechamentoGestor.Cadastro.lblTituloAulasDadas.Text %>" Font-Size="Small"></asp:Label></th>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr class="tr-disciplina">
                            <th class="th-disciplina">
                                <asp:Label ID="lblDisciplina" runat="server" Text='<%#Bind("tud_nome") %>'></asp:Label>
                            </th>
                            <asp:Repeater ID="rptAulas" runat="server">
                                <ItemTemplate>
                                    <td class="td-aulas">
                                        <asp:Label ID="lblAulasPrevistas" runat="server" Text='<%#Bind("aulasPrevistas") %>'></asp:Label></td>
                                    <td class="td-aulas">
                                        <asp:Label ID="lblAulasDadas" runat="server" Text='<%#Bind("aulasDadas") %>'></asp:Label></td>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
