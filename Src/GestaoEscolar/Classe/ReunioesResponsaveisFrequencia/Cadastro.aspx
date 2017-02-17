<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="Cadastro.aspx.cs" Inherits="Classe_ReunioesResponsaveisFrequencia_Cadastro" %>

<%@ PreviousPageType VirtualPath="~/Classe/ReunioesResponsaveisFrequencia/Busca.aspx" %>
<%@ Register Src="~/WebControls/Combos/UCComboPeriodoCalendario.ascx" TagName="UCComboPeriodoCalendario"
    TagPrefix="uc2" %>
<%@ Register Src="../../WebControls/Combos/UCComboOrdenacao.ascx" TagName="UCComboOrdenacao"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="_uppMessage" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="vsFrequencia" runat="server" ValidationGroup="Frequencia" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <fieldset>
        <legend>Dados da turma</legend>
        <asp:Label ID="lblTurma" runat="server"></asp:Label>
        <br />
        <div style="width: 900px; word-wrap: break-word">
            <asp:Label ID="lblCurso" runat="server"></asp:Label>
        </div>
        <br />
        <uc2:UCComboPeriodoCalendario ID="UCComboPeriodoCalendario1" SkinID_Combo="text30C"
            runat="server" _MostrarMessageSelecione="true" />
        <asp:Label ID="lblPeriodoCalendario" runat="server"></asp:Label>
        <div class="right" class="divBtnCadastro">
            <asp:Button ID="_btnImprimir" runat="server" Text="Imprimir declaração para todos responsáveis"
                ValidationGroup="Frequencia" OnClick="_btnImprimir_Click" />
            <asp:Button ID="_btnSalvar2" runat="server" Text="Salvar" ValidationGroup="Frequencia"
                OnClick="_btnSalvar_Click" />
            <asp:Button ID="_btnCancelar2" runat="server" Text="Cancelar" CausesValidation="False"
                OnClick="_btnCancelar_Click" />
        </div>
    </fieldset>
    <asp:Panel ID="pnlLancamentoFrequencias" runat="server" GroupingText="Lançamento de frequência em reuniões de responsáveis">
        <asp:UpdatePanel ID="upnLancamento" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:Label Style="display: block" ID="lblMsgParecer" runat="server" Text=""></asp:Label>
                <asp:Label ID="_lblMsgRepeater" runat="server"></asp:Label>
                <uc3:UCComboOrdenacao ID="_UCComboOrdenacao1" runat="server" />
                <div class="chkTodas">
                    <asp:CheckBox ID="chkTodas" runat="server" Text="Selecionar todos os registros">
                    </asp:CheckBox>
                </div>
                <asp:Repeater ID="rptAlunos" runat="server" OnItemDataBound="rptAlunos_ItemDataBound"
                    OnItemCommand="rptAlunos_ItemCommand">
                    <HeaderTemplate>
                        <div class="tabelaLancamentoFrequencia" style="height:400px;">
                        <table id="tblNotas" class="grid" cellspacing="0" style="width: 100%">
                            <tr class="gridHeader">
                                <th class="center">
                                    <asp:Label ID="_lblNumChamada" runat="server" Text="Nº Chamada"></asp:Label>
                                </th>
                                <th class="thLeft" align="left">
                                    <asp:Label ID="lblNome" runat="server" Text="Nome"></asp:Label>
                                </th>
                                <th runat="server" ID="thAvaliacaoAluno">
                                    <asp:Label runat="server" ID="lblAvaliacaoAluno"></asp:Label>
                                </th>
                                <asp:Repeater ID="rptReunioes" runat="server">
                                    <ItemTemplate>
                                        <th class="center">
                                            <asp:Label ID="lblreu_id" runat="server" Text='<%#Bind("reu_id") %>' Visible="false">
                                            </asp:Label>
                                            <asp:Label ID="lblreu_titulo" runat="server" Text='<%#Bind("reu_titulo") %>'>
                                            </asp:Label>
                                            <asp:CheckBox ID="chkEfetivado" runat="server" Text="Efetivado" Checked='<%#Convert.ToBoolean(Eval("reu_efetivado"))%>' />
                                        </th>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <th class="center">
                                    <asp:Label ID="lblCheckAll" runat="server" Text="Todas"></asp:Label>
                                </th>
                                <th class="center">
                                    <asp:Label ID="lblDeclaracao" runat="server" Text="Declaração"></asp:Label>
                                </th>
                            </tr>
                    </HeaderTemplate>
                    <AlternatingItemTemplate>
                        <tr class="gridAlternatingRow">
                            <td style="text-align: right;">
                                <asp:Label ID="lblalu_id" runat="server" Text='<%#Bind("alu_id") %>' Visible="false">
                                </asp:Label>
                                <asp:Label ID="lblAtividade" runat="server" Text='<%#Bind("mtu_numeroChamada") %>'>
                                </asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblNome" runat="server" Text='<%#Bind("pes_nome") %>'>
                                </asp:Label>
                            </td>
                            <td runat="server" ID="tdAvaliacaoAluno">
                                <asp:Label runat="server" ID="lblAvaliacaoAluno" Text='<%# Bind("tca_numeroAvaliacao") %>'></asp:Label>
                            </td>
                            <asp:Repeater ID="rptReunioes" runat="server">
                                <ItemTemplate>
                                    <td style="text-align: center;">
                                        <asp:Label ID="lblreu_id" runat="server" Text='<%#Bind("reu_id") %>' Visible="false">
                                        </asp:Label>
                                        <div class="chkFrequencia">
                                            <asp:CheckBox ID="chkFrequencia" runat="server"></asp:CheckBox>
                                        </div>
                                    </td>
                                </ItemTemplate>
                            </asp:Repeater>
                            <td style="text-align: center;">
                                <div class="checkAll">
                                    <asp:CheckBox ID="checkAll" runat="server"></asp:CheckBox>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center">
                                    <asp:ImageButton ID="btnImprimir" runat="server" CommandArgument='<%#Bind("alu_id")%>'
                                        SkinID="btImprimir" />
                                </div>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                    <ItemTemplate>
                        <tr class="gridRow">
                            <td style="text-align: right;">
                                <asp:Label ID="lblalu_id" runat="server" Text='<%#Bind("alu_id") %>' Visible="false">
                                </asp:Label>
                                <asp:Label ID="lblAtividade" runat="server" Text='<%#Bind("mtu_numeroChamada") %>'>
                                </asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblNome" runat="server" Text='<%#Bind("pes_nome") %>'>
                                </asp:Label>
                            </td>
                            <td runat="server" ID="tdAvaliacaoAluno">
                                <asp:Label runat="server" ID="lblAvaliacaoAluno" Text='<%# Bind("tca_numeroAvaliacao") %>'></asp:Label>
                            </td>
                            <asp:Repeater ID="rptReunioes" runat="server">
                                <ItemTemplate>
                                    <td style="text-align: center;">
                                        <asp:Label ID="lblreu_id" runat="server" Text='<%#Bind("reu_id") %>' Visible="false">
                                        </asp:Label>
                                        <div class="chkFrequencia">
                                            <asp:CheckBox ID="chkFrequencia" runat="server"></asp:CheckBox>
                                        </div>
                                    </td>
                                </ItemTemplate>
                            </asp:Repeater>
                            <td style="text-align: center;">
                                <div class="checkAll">
                                    <asp:CheckBox ID="checkAll" runat="server"></asp:CheckBox>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center">
                                    <asp:ImageButton ID="btnImprimir" runat="server" CommandArgument='<%#Bind("alu_id")%>'
                                        SkinID="btImprimir" />
                                </div>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table></div>
                    </FooterTemplate>
                </asp:Repeater>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <fieldset>
        <div class="right" class="divBtnCadastro">
            <asp:Button ID="_btnSalvar" runat="server" Text="Salvar" ValidationGroup="Frequencia"
                OnClick="_btnSalvar_Click" />
            <asp:Button ID="_btnCancelar" runat="server" Text="Cancelar" CausesValidation="False"
                OnClick="_btnCancelar_Click" />
        </div>
    </fieldset>
    <div id="divCompComparecimento" title="Comprovante de comparecimento" class="hide">
        <fieldset id="fdsRelatorio" runat="server">
            <asp:UpdatePanel ID="_updRelatorio" runat="server">
                <ContentTemplate>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="comparecimento" />
                    <asp:Label ID="lblComComparecimento" runat="server"></asp:Label>
                    <div>
                        <div style="float: left;">
                            <asp:Label ID="Label6" runat="server" Text="Data de comparecimento *" AssociatedControlID="txtdata"></asp:Label>
                            <asp:TextBox ID="txtData" runat="server" Enabled="true" MaxLength="10" SkinID="Data"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="_rfvData" runat="server" ControlToValidate="txtData"
                                ErrorMessage="Data de comparecimento é obrigatório." ValidationGroup="comparecimento">*</asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cpvData" runat="server" ControlToValidate="txtData" ValidationGroup="comparecimento"
                                Display="Dynamic" ErrorMessage="Data de comparecimento não está no formato dd/mm/aaaa ou é inexistente."
                                Operator="DataTypeCheck" Type="Date" SetFocusOnError="true">*</asp:CompareValidator>
                        </div>
                        <div class="clear">
                        </div>
                        <div style="float: left;">
                            <asp:Label ID="Label5" runat="server" Text="Hora inicial *" AssociatedControlID="ddlHoraInicial"></asp:Label>
                            <asp:DropDownList ID="ddlHoraInicial" runat="server" Enabled="true">
                            </asp:DropDownList>
                            <asp:CompareValidator ID="_cpvValidadeHoraInicial" runat="server" ControlToValidate="ddlHoraInicial"
                                ValueToCompare="-1" Operator="GreaterThan" Type="Integer" ErrorMessage="Hora inicial é obrigatório."
                                ValidationGroup="comparecimento" Display="Dynamic">*</asp:CompareValidator>
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        </div>
                        <div style="float: left;">
                            <asp:Label ID="Label7" runat="server" Text="Minuto inicial *" AssociatedControlID="ddlMinutosInicial"></asp:Label>
                            <asp:DropDownList ID="ddlMinutosInicial" runat="server" Enabled="true">
                            </asp:DropDownList>
                            <asp:CompareValidator ID="_cpvValidatorMinutoInicial" runat="server" ValueToCompare="-1"
                                Operator="GreaterThan" Type="Integer" ErrorMessage="Minuto inicial é obrigatório."
                                ValidationGroup="comparecimento" Display="Dynamic" ControlToValidate="ddlMinutosInicial">*</asp:CompareValidator>
                        </div>
                        <div class="clear">
                        </div>
                        <div style="float: left;">
                            <asp:Label ID="Label2" runat="server" Text="Hora final *  " AssociatedControlID="ddlHoraFinal"></asp:Label>
                            <asp:DropDownList ID="ddlHoraFinal" runat="server" Enabled="true">
                            </asp:DropDownList>
                            <asp:CompareValidator ID="_cpvValidadeHoraFinal" runat="server" ControlToValidate="ddlHoraFinal"
                                ValueToCompare="-1" Operator="GreaterThan" Type="Integer" ErrorMessage="Hora final é obrigatório."
                                ValidationGroup="comparecimento" Display="Dynamic">*</asp:CompareValidator>
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        </div>
                        <div style="float: left;">
                            <asp:Label ID="Label3" runat="server" Text="Minuto final *" AssociatedControlID="ddlMinutosFinal"></asp:Label>
                            <asp:DropDownList ID="ddlMinutosFinal" runat="server" Enabled="true">
                            </asp:DropDownList>
                            <asp:CompareValidator ID="_cpvValidadeMinutoFinal" runat="server" ValueToCompare="-1"
                                Operator="GreaterThan" Type="Integer" ErrorMessage="Minuto final é obrigatório."
                                ValidationGroup="comparecimento" Display="Dynamic" ControlToValidate="ddlMinutosFinal">*</asp:CompareValidator>
                            <asp:CustomValidator runat="server" ID="cvVadationTime" ControlToValidate="ddlMinutosFinal"
                                OnServerValidate="cvVadationTime_ServerValidate" ValidationGroup="comparecimento"
                                Display="Dynamic" ErrorMessage="Hora final e minuto final deve ser maior que a hora inicial e minuto inicial.">*</asp:CustomValidator>
                        </div>
                        <div class="clear">
                        </div>
                        <div style="float: left;">
                            <asp:Label ID="lblParticipante" runat="server" Text="Selecione o participante na reunião *"
                                AssociatedControlID="rblParticipante"></asp:Label>
                            <div style="float: left;">
                                <asp:RadioButtonList ID="rblParticipante" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1">Pai</asp:ListItem>
                                    <asp:ListItem Value="2">Mãe</asp:ListItem>
                                    <asp:ListItem Value="3">Responsável</asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div style="float: left;">
                                <asp:RequiredFieldValidator ID="rfvParticipante" runat="server" ErrorMessage="Participante na reunião é obrigatório."
                                    ControlToValidate="rblParticipante" Display="Dynamic" ValidationGroup="comparecimento">*</asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvParticipante" runat="server" ErrorMessage="Participante na reunião não é um tipo de responsável cadastrado para o aluno selecionado."
                                    ControlToValidate="rblParticipante" Display="Dynamic" OnServerValidate="cvParticipante_ServerValidate"
                                    ValidationGroup="comparecimento">*</asp:CustomValidator>
                            </div>
                        </div>
                        <br />
                        <br />
                    </div>
                    <br />
                    <div class="right">
                        <asp:Button ID="_btnGerarRelatorio" runat="server" Text="Imprimir declaração" ValidationGroup="comparecimento"
                            OnClick="_btnGerarRelatorio_Click" />
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="_btnImprimir" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </fieldset>
    </div>
</asp:Content>