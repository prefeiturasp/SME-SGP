<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master"
    EnableEventValidation="false" Inherits="Documentos_DocumentoAluno_Busca" CodeBehind="Busca.aspx.cs" %>

<%@ Register Src="~/WebControls/Combos/Novos/UCCCursoCurriculo.ascx" TagName="UCComboCursoCurriculo"
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="UCComboUAEscola"
    TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCTurma.ascx" TagName="UCComboTurma"
    TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCalendario.ascx" TagName="UCComboCalendario"
    TagPrefix="uc5" %>
<%@ Register Src="~/WebControls/BuscaAluno/UCCamposBuscaAluno.ascx" TagName="UCCamposBuscaAluno"
    TagPrefix="uc6" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCurriculoPeriodo.ascx" TagName="UCComboCurriculoPeriodo"
    TagPrefix="uc7" %>
<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc8" %>
<%@ Register Src="~/WebControls/Busca/UCAluno.ascx" TagName="UCAluno" 
    TagPrefix="uc9" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc10" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoResponsavelAluno.ascx" TagName="UCComboTipoResponsavelAluno"
    TagPrefix="uc11" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="divBuscaAluno" title="Busca de alunos" class="hide">
        <asp:UpdatePanel ID="uppBuscaAluno" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc9:UCAluno ID="UCAluno1" BuscaExcedentes="false" runat="server" OnReturnValues="UCAluno1_ReturnValues" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnBuscaAluno" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <!-- POPUP - seleção do Período de Avaliação (bimestres/coc) para listar o boletim correspondente  -->
    <div id="divPeriodoDeAvaliacao" title="Período de avaliação" class="hide">
        <br />
        <asp:UpdatePanel ID="updPeriodoDeAvaliacao" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset id="fsPeriodoDeAvaliacao" runat="server">
                    <legend>Selecione o período desejado</legend>

                    <asp:GridView ID="grvPeriodoDeAvaliacao" runat="server" AutoGenerateColumns="False"
                        DataKeyNames="tpc_id" AllowPaging="True" BorderStyle="None" OnRowCommand="grvPeriodoAvaliacao_RowCommand"
                        OnRowDataBound="grvPeriodoAvaliacao_RowDataBound"
                        EmptyDataText="Não existe período de avaliação cadastrado." AllowSorting="true">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbkPeriodoAvaliacao" runat="server" Text='<%# Eval("cap_descricao")%>'
                                        CommandName="lbkPeriodoAvaliacao_Select"></asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                    <div class="right">
                        <asp:Button ID="btnVoltarPesquisa" runat="server" Text="Voltar"
                            OnClick="btnVoltarPesquisa_Click" />
                    </div>
                </fieldset>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="_btnGerarRelatorio" />
                <asp:AsyncPostBackTrigger ControlID="btnGerarRelatorioCima" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div id="divDeclaracaoComparecimento" title="Declaração de comparecimento" class="hide">
        <br />
        <asp:UpdatePanel ID="updDeclaracaoComparecimento" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label ID="_lblMessageDeclaracao" runat="server" EnableViewState="False"></asp:Label>
                <asp:ValidationSummary ID="ValidationSummary5" runat="server" ValidationGroup="Declaracao" />
                <fieldset id="fdsDeclaracaoComparecimento" runat="server">
                    <legend>Informe os dados da declaração</legend>
                    <uc11:UCComboTipoResponsavelAluno ID="UCComboTipoResponsavelAluno1" runat="server"
                        MostrarMessageSelecione="true" Obrigatorio="true" ValidationGroup="Declaracao" />
                    <div style="display: inline-block">
                        <asp:Label ID="lblDataDeclaracao" runat="server" Text="Data *" AssociatedControlID="txtDataDeclaracao"></asp:Label>
                        <asp:TextBox ID="txtDataDeclaracao" runat="server" Enabled="true" MaxLength="10"
                            SkinID="Data"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvDataDeclaracao" runat="server" ControlToValidate="txtDataDeclaracao"
                            ValidationGroup="Declaracao" ErrorMessage="Data é obrigatório.">*</asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cpvDataDeclaracao" runat="server" ControlToValidate="txtDataDeclaracao"
                            ValidationGroup="Declaracao" Display="Dynamic" ErrorMessage="Data é inválida ou inexistente (DD/MM/AAAA)."
                            Operator="DataTypeCheck" Type="Date" SetFocusOnError="true">*</asp:CompareValidator>
                    </div>
                    <div style="display: inline-block">
                        <asp:Label ID="lblHoraInicioDeclaracao" runat="server" Text="Horário Inicial *" AssociatedControlID="txtHoraInicioDeclaracao"></asp:Label>
                        <asp:TextBox ID="txtHoraInicioDeclaracao" runat="server" SkinID="Hora" ValidationGroup="Declaracao"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvHoraInicioDeclaracao" runat="server" ControlToValidate="txtHoraInicioDeclaracao"
                            Display="Dynamic" ErrorMessage="Horário Inicial é obrigatório." ValidationGroup="Declaracao">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revHoraInicioDeclaracao" runat="server" ControlToValidate="txtHoraInicioDeclaracao"
                            ValidationExpression="^([0-1][0-9]|[2][0-3])(:([0-5][0-9])){1,2}$" ErrorMessage="Horário Inicial deve estar entre 00:00 e 23:59 no formato HH:mm."
                            ValidationGroup="Declaracao">*</asp:RegularExpressionValidator>
                    </div>
                    <div style="display: inline-block">
                        <asp:Label ID="lblHoraFimDeclaracao" runat="server" Text="Horário final *" AssociatedControlID="txtHoraFimDeclaracao"></asp:Label>
                        <asp:TextBox ID="txtHoraFimDeclaracao" runat="server" SkinID="Hora"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvHoraFimDeclaracao" runat="server" ControlToValidate="txtHoraFimDeclaracao"
                            Display="Dynamic" ErrorMessage="Horário final é obrigatório." ValidationGroup="Declaracao">*</asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cpvHoraFimDeclaracao" runat="server" ErrorMessage="Horário final deve ser maior que o Horário Inicial."
                            ControlToCompare="txtHoraFimDeclaracao" ControlToValidate="txtHoraFinalEnc" Operator="GreaterThan"
                            Type="String" ValidationGroup="Declaracao">*</asp:CompareValidator>
                        <asp:RegularExpressionValidator ID="revHoraFimDeclaracao" runat="server" ControlToValidate="txtHoraFimDeclaracao"
                            ValidationExpression="^([0-1][0-9]|[2][0-3])(:([0-5][0-9])){1,2}$" ErrorMessage="Horário final deve estar entre 00:00 e 23:59 no formato HH:mm."
                            ValidationGroup="Declaracao">*</asp:RegularExpressionValidator>
                    </div>
                    <div class="right">
                        <asp:Button ID="btnGerarDeclaracao" runat="server" Text="Gerar declaração"
                            OnClick="btnGerarDeclaracao_Click" ValidationGroup="Declaracao" />
                        <asp:Button ID="btnVoltarPesquisaDec" runat="server" Text="Voltar"
                            OnClick="btnVoltarPesquisaDec_Click" CausesValidation="false" />
                    </div>
                </fieldset>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="_btnGerarRelatorio" />
                <asp:AsyncPostBackTrigger ControlID="btnGerarRelatorioCima" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <div id="divGrupamento" title="Gerar documento" class="hide">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label ID="_lblMessageGrupamento" runat="server" EnableViewState="False"></asp:Label>
                <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="Grupamento" />
                <fieldset>
                    <asp:Label ID="Label1" runat="server" Text="Grupamento" AssociatedControlID="_rblGrupamento"
                        EnableViewState="False"></asp:Label>
                    <asp:RadioButtonList ID="_rblGrupamento" runat="server" RepeatDirection="Horizontal"
                        OnSelectedIndexChanged="_rblGrupamento_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Value="1" Selected="True">Cursando</asp:ListItem>
                        <asp:ListItem Value="2">Conclusão</asp:ListItem>
                        <asp:ListItem Value="3">Sem conclusão</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:Label ID="_lblDestinatario" runat="server" Text="Destinatário" AssociatedControlID="_txtDestinatario"
                        EnableViewState="False"></asp:Label>
                    <asp:TextBox ID="_txtDestinatario" runat="server" SkinID="text30C"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="_rfvGrupamento" runat="server" ErrorMessage="Destinatário é obrigatório."
                        ControlToValidate="_txtDestinatario" ValidationGroup="Grupamento">*</asp:RequiredFieldValidator>
                    <div id="divGrupamentoFrequencia">
                        <asp:Label ID="lblExibirFrequencia" runat="server" Text="Exibir frequência do aluno?"
                            AssociatedControlID="rdbExibirFrequencia"></asp:Label>
                        <asp:RadioButtonList ID="rdbExibirFrequencia" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Selected="True" Value="true">Sim</asp:ListItem>
                            <asp:ListItem Value="false">Não</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </fieldset>
                <div class="right">
                    <asp:Button ID="_btnGerarRelatorio2" runat="server" Text="Gerar relatório" OnClick="_btnGerarRelatorio2_Click"
                        ValidationGroup="Grupamento" />
                    <asp:Button ID="_btnGrupamentoFechar" runat="server" Text="Fechar" OnClientClick="$(&quot;#divGrupamento&quot;).dialog(&quot;close&quot;); return false;"
                        CausesValidation="False" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divEncaminhamentoRemanejado" title="Gerar relatório" class="hide">
        <asp:UpdatePanel ID="UpdEncRemanej" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblMensagemRemanej" runat="server" EnableViewState="False"></asp:Label>
                <asp:ValidationSummary ID="ValidationSummary4" runat="server" ValidationGroup="EncaminhamentoRemanejado" />
                <asp:Label ID="lblDadosAlunoRemanej" runat="server" EnableViewState="False"></asp:Label>
                <fieldset>
                    <div style="display: inline-block">
                        <asp:Label ID="lblDataEncam" runat="server" Text="Data *" AssociatedControlID="txtDataEncaminhamento"></asp:Label>
                        <asp:TextBox ID="txtDataEncaminhamento" runat="server" Enabled="true" MaxLength="10"
                            SkinID="Data"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvDataEncaminhamento" runat="server" ControlToValidate="txtDataEncaminhamento"
                            ValidationGroup="EncaminhamentoRemanejado" ErrorMessage="Data é obrigatório.">*</asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cvDataEncaminhamento" runat="server" ControlToValidate="txtDataEncaminhamento"
                            ValidationGroup="EncaminhamentoRemanejado" Display="Dynamic" ErrorMessage="Data é inválida ou inexistente (DD/MM/AAAA)."
                            Operator="DataTypeCheck" Type="Date" SetFocusOnError="true">*</asp:CompareValidator>
                    </div>
                    <div style="display: inline-block">
                        <asp:Label ID="lblHoraInicialEnc" runat="server" Text="Horário Inicial *" AssociatedControlID="txtHoraInicialEnc"></asp:Label>
                        <asp:TextBox ID="txtHoraInicialEnc" runat="server" SkinID="Hora" ValidationGroup="EncaminhamentoRemanejado"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvHoraInicialEnc" runat="server" ControlToValidate="txtHoraInicialEnc"
                            Display="Dynamic" ErrorMessage="Horário Inicial é obrigatório." ValidationGroup="EncaminhamentoRemanejado">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revHoraInicial" runat="server" ControlToValidate="txtHoraInicialEnc"
                            ValidationExpression="^([0-1][0-9]|[2][0-3])(:([0-5][0-9])){1,2}$" ErrorMessage="Horário Inicial deve estar entre 00:00 e 23:59 no formato HH:mm."
                            ValidationGroup="EncaminhamentoRemanejado">*</asp:RegularExpressionValidator>
                    </div>
                    <div style="display: inline-block">
                        <asp:Label ID="lblHoraFinalEnc" runat="server" Text="Horário final *" AssociatedControlID="txtHoraFinalEnc"></asp:Label>
                        <asp:TextBox ID="txtHoraFinalEnc" runat="server" SkinID="Hora"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvHoraFinalEnc" runat="server" ControlToValidate="txtHoraFinalEnc"
                            Display="Dynamic" ErrorMessage="Horário final é obrigatório." ValidationGroup="EncaminhamentoRemanejado">*</asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cpvHorario" runat="server" ErrorMessage="Horário final deve ser maior que o Horário Inicial."
                            ControlToCompare="txtHoraInicialEnc" ControlToValidate="txtHoraFinalEnc" Operator="GreaterThan"
                            Type="String" ValidationGroup="EncaminhamentoRemanejado">*</asp:CompareValidator>
                        <asp:RegularExpressionValidator ID="revHoraFinal" runat="server" ControlToValidate="txtHoraFinalEnc"
                            ValidationExpression="^([0-1][0-9]|[2][0-3])(:([0-5][0-9])){1,2}$" ErrorMessage="Horário final deve estar entre 00:00 e 23:59 no formato HH:mm."
                            ValidationGroup="EncaminhamentoRemanejado">*</asp:RegularExpressionValidator>
                    </div>
                </fieldset>
                <div class="right">
                    <asp:Button ID="btnGerarEncaminhamentoRemanej" runat="server" Text="Gerar relatório"
                        OnClick="btnGerarEncaminhamentoRemanej_Click" ValidationGroup="EncaminhamentoRemanejado" />
                    <asp:Button ID="btnVoltar" runat="server" Text="Fechar" OnClientClick="$(&quot;#divEncaminhamentoRemanejado&quot;).dialog(&quot;close&quot;); return false;"
                        CausesValidation="False" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divSelecionarPeriodo" title="Selecione o período" class="hide">
        <fieldset>
            <asp:Label ID="lblMessagePeriodo" runat="server"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="Periodo" />
            <asp:Label ID="lblFormatoPeriodo" runat="server" Text="Período *" AssociatedControlID="_ddlPeriodo"
                EnableViewState="False"></asp:Label>
            <asp:DropDownList ID="_ddlPeriodo" runat="server" DataSourceID="odsPeriodo" DataTextField="cap_descricao" Visible="false"
                DataValueField="TodasChavesPrimarias" OnDataBound="_ddlPeriodo_DataBound" SkinID="text20C">
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="_rfvPeriodo" runat="server" ErrorMessage="Período é obrigatório."
                ControlToValidate="_ddlPeriodo" InitialValue="-1;-1" ValidationGroup="Periodo">*</asp:RequiredFieldValidator>
            <asp:ObjectDataSource ID="odsPeriodo" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_CalendarioPeriodo"
                OldValuesParameterFormatString="original_{0}" SelectMethod="SelecionaPeriodoPorCalendarioEntidade"
                TypeName="MSTech.GestaoEscolar.BLL.ACA_CalendarioPeriodoBO">
                <SelectParameters>
                    <asp:ControlParameter ControlID="UCComboCalendario1" DefaultValue="" Name="idCalendarioAnual"
                        PropertyName="Valor" Type="Int32" />
                    <asp:ControlParameter ControlID="__Page" DbType="Guid" DefaultValue="" Name="idEntidade"
                        PropertyName="__SessionWEB.__UsuarioWEB.Usuario.ent_id" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <div class="right">
                <asp:Button ID="_btnGerarRelatorio1" runat="server" Text="Gerar relatório" OnClick="_btnGerarRelatorio1_Click"
                    ValidationGroup="Periodo" />
                <asp:Button ID="_btnSelecionarPeriodoFechar" runat="server" Text="Fechar" OnClientClick="$('#divSelecionarPeriodo').dialog('close'); return false;"
                    CausesValidation="False" />
            </div>
        </fieldset>
    </div>
    <asp:UpdatePanel ID="updMensagem" runat="server">
        <ContentTemplate>
            <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div style="width: 40%; float: left; clear: none;" class="area-selecao-documento-aluno">
        <fieldset id="fdsDocumentos" runat="server" style="margin-right: 10px;">
            <legend>Relatórios de alunos</legend>
            <div id="_divRelatorio" class="divRelatorio" runat="server">
                <asp:RadioButtonList ID="_rdbRelatorios" runat="server" DataSourceID="odsDocumentos"
                    AutoPostBack="true" DataTextField="rda_nomeDocumento" DataValueField="rlt_id"
                    OnSelectedIndexChanged="_rdbRelatorios_SelectedIndexChanged">
                </asp:RadioButtonList>
                <asp:ObjectDataSource ID="odsDocumentos" runat="server" SelectMethod="ListarDocumentosAluno"
                    TypeName="MSTech.GestaoEscolar.BLL.CFG_RelatorioDocumentoAlunoBO" OldValuesParameterFormatString="original_{0}"
                    OnSelected="odsDocumentos_Selected">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="__Page" DbType="Guid" Name="idEntidade" PropertyName="__SessionWEB.__UsuarioWEB.Usuario.ent_id" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
        </fieldset>
    </div>
    <asp:UpdatePanel ID="uppPesquisa" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 60%; float: left; clear: none;" class="divPesquisa area-filtro-documento-aluno">
                <fieldset id="fdsPesquisa" runat="server" style="margin-left: 10px;">
                    <legend>Parâmetros de busca</legend>
                    <uc10:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" Visible="false" />
                    <div id="_divPesquisa" class="divPesquisa" runat="server">
                        <asp:Label ID="lblAvisoMensagem" runat="server"></asp:Label>
                        <!-- FiltrosPadrao -->
                        <asp:CheckBox ID="ChkEmitirAnterior" runat="server" Text="Emitir relatórios de anos anteriores" AutoPostBack="true"
                            OnCheckedChanged="ChkEmitirAnterior_CheckedChanged" />
                        <uc3:UCComboUAEscola ID="UCComboUAEscola" runat="server" CarregarEscolaAutomatico="true"
                            MostrarMessageSelecioneEscola="true" MostrarMessageSelecioneUA="true" OnIndexChangedUA="UCComboUAEscola_IndexChangedUA"
                            OnIndexChangedUnidadeEscola="UCComboUAEscola_IndexChangedUnidadeEscola" />
                        <div id="msgCertConcCurso" class="msgInformacao" runat="server" style="margin-top: -30px; width: 180px;"
                            visible="false">
                            Verifique se os alunos possuem data de nascimento, naturalidade e nacionalidade
                            cadastrados.
                        </div>
                        <uc2:UCComboCursoCurriculo ID="UCComboCursoCurriculo1" MostrarMensagemSelecione="true"
                            runat="server" />
                        <uc7:UCComboCurriculoPeriodo ID="UCComboCurriculoPeriodo1" MostrarMensagemSelecione="true"
                            runat="server" />
                        <uc5:UCComboCalendario ID="UCComboCalendario1" runat="server" MostrarMensagemSelecione="true" />
                        <uc4:UCComboTurma ID="UCComboTurma1" runat="server" MostrarMessageSelecione="true" />
                        <asp:CheckBox runat="server" ID="chkBuscaAvancada" Text="Busca avançada" OnCheckedChanged="chkBuscaAvancada_CheckedChanged" AutoPostBack="true" />
                        <div id="divBuscaAvancadaAluno" runat="server" class="divBuscaAvancadaAluno">
                            <uc6:UCCamposBuscaAluno ID="UCCamposBuscaAluno1" runat="server" />
                        </div>


                        <!-- FiltrosDeclaracaoSolicitacaoVaga -->
                        <div id="divDeclaracaoSolicitacaoVaga" runat="server" visible="false">

                            <asp:Label ID="_lblNomeAluno" runat="server" Text="Nome do aluno *" AssociatedControlID="_txtNomeAluno"></asp:Label>
                            <asp:TextBox ID="_txtNomeAluno" runat="server" SkinID="text30C"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="_rfvNomeAluno" runat="server" ControlToValidate="_txtNomeAluno"
                                ErrorMessage="Nome do aluno é obrigatório.">*</asp:RequiredFieldValidator>
                            <br />
                            <asp:Label ID="_lblDataNasc" runat="server" Text="Data de nascimento *" AssociatedControlID="_txtDataNasc"></asp:Label>
                            <asp:TextBox ID="_txtDataNasc" runat="server" MaxLength="10" SkinID="Data"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="_rfvDataNasc" runat="server" ControlToValidate="_txtDataNasc"
                                ErrorMessage="Data de nascimento é obrigatória.">*</asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cvDataNasc" runat="server" ControlToValidate="_txtDataNasc"
                                Display="Dynamic" ErrorMessage="" OnServerValidate="ValidarData_ServerValidate">* </asp:CustomValidator>
                            <br />
                            <asp:Label ID="_lblEscolaOrigem" runat="server" Text="Escola de origem" AssociatedControlID="_txtEscolaOrigem"></asp:Label>
                            <asp:TextBox ID="_txtEscolaOrigem" runat="server" SkinID="text60C"></asp:TextBox>
                            <br />
                            <asp:Label ID="_lblValidade" runat="server" Text="Validade *" AssociatedControlID="_ddlValidade"></asp:Label>
                            <asp:DropDownList ID="_ddlValidade" runat="server" AutoPostBack="true" SkinID="text30C"
                                OnSelectedIndexChanged="_ddlValidade_SelectedIndexChanged">
                                <asp:ListItem Value="-1" Selected="True">-- Selecione uma validade --</asp:ListItem>
                                <asp:ListItem Value="1">Horas</asp:ListItem>
                                <asp:ListItem Value="2">Dias</asp:ListItem>
                                <asp:ListItem Value="3">Data</asp:ListItem>
                            </asp:DropDownList>
                            <asp:CompareValidator ID="_cpvValidade" runat="server" ControlToValidate="_ddlValidade"
                                ValueToCompare="0" Operator="GreaterThan" ErrorMessage="Validade é obrigatória."
                                Display="Dynamic">*</asp:CompareValidator>
                            <asp:Panel ID="_pnlValidade" runat="server" Visible="false">
                                <asp:Label ID="_lblValor" runat="server"></asp:Label>
                                <asp:TextBox ID="_txtHoras" runat="server" MaxLength="10" SkinID="Hora" Visible="false"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="_revHoras" runat="server" ControlToValidate="_txtHoras"
                                    ValidationExpression="^([0-1][0-9]|[2][0-3])(:([0-5][0-9])){1,2}$" Display="Dynamic"
                                    EnableClientScript="false" ErrorMessage="Hora(s) não pode ser superior a 23:59.">*</asp:RegularExpressionValidator>
                                <asp:TextBox ID="_txtDias" runat="server" MaxLength="10" SkinID="Numerico" Visible="false"></asp:TextBox>
                                <asp:TextBox ID="_txtData" runat="server" MaxLength="10" SkinID="Data" Visible="false"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="_rfvValorValidade" runat="server" Display="Dynamic">*</asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="_cvValorData" runat="server" ControlToValidate="_txtData"
                                    Display="Dynamic" ErrorMessage="" Visible="false">*</asp:CustomValidator>
                            </asp:Panel>
                        </div>
                        <!-- FiltrosDeclaracaoSolicitacaoComparecimento -->
                        <div id="divDeclaracaoSolicitacaoComparecimento" runat="server" visible="false">
                            <asp:Label ID="lblNomeAluno" runat="server" Text="Aluno *" AssociatedControlID="txtNomeAluno"></asp:Label>
                            <asp:TextBox ID="txtNomeAluno" runat="server" Enabled="False" SkinID="text60C" MaxLength="200"
                                ValidationGroup="comparecimento"></asp:TextBox>
                            <asp:ImageButton ID="btnBuscaAluno" runat="server" SkinID="btPesquisar" CausesValidation="false"
                                OnClick="btnBuscaAluno_Click" />
                            <asp:RequiredFieldValidator ID="rfv_nomeAluno" runat="server" ControlToValidate="txtNomeAluno"
                                ErrorMessage="Aluno é obrigatório." Display="Dynamic">*</asp:RequiredFieldValidator>
                            <asp:Label ID="Label3" runat="server" Text="Nome do pai" AssociatedControlID="txtNomePai"></asp:Label>
                            <asp:TextBox ID="txtNomePai" runat="server" Enabled="False" SkinID="text60C" MaxLength="200"></asp:TextBox>
                            <asp:Label ID="Label4" runat="server" Text="Nome da mãe" AssociatedControlID="txtNomeMae"></asp:Label>
                            <asp:TextBox ID="txtNomeMae" runat="server" Enabled="False" SkinID="text60C" MaxLength="200"></asp:TextBox>
                            <asp:Label ID="Label5" runat="server" Text="Nome do responsável *" AssociatedControlID="txtNomeResp"></asp:Label>
                            <asp:TextBox ID="txtNomeResp" runat="server" Enabled="true" SkinID="text60C" MaxLength="200"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNomeResp"
                                ErrorMessage="Nome do responsável é obrigatório.">*</asp:RequiredFieldValidator>
                            <br />
                            <asp:Label ID="Label6" runat="server" Text="Comparecer em:" AssociatedControlID="Label6"></asp:Label>
                            <br />
                            <div>
                                <div style="float: left;">
                                    <asp:Label ID="Label7" runat="server" Text="Data *" AssociatedControlID="txtdata"></asp:Label>
                                    <asp:TextBox ID="txtData" runat="server" Enabled="true" MaxLength="10" SkinID="Data"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="_rfvData" runat="server" ControlToValidate="txtData"
                                        ErrorMessage="Data é obrigatório.">*</asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="cpvData" runat="server" ControlToValidate="txtData" Display="Dynamic"
                                        ErrorMessage="Data de comparecimento inválida ou inexistente (DD/MM/AAAA)." Operator="DataTypeCheck"
                                        Type="Date" SetFocusOnError="true">*</asp:CompareValidator>
                                </div>
                                <div style="float: left;">
                                    <asp:Label ID="Label8" runat="server" Text="Hora *" AssociatedControlID="ddlHoraComparecimento"></asp:Label>
                                    <asp:DropDownList ID="ddlHoraComparecimento" runat="server" Enabled="true">
                                    </asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator4" runat="server" ControlToValidate="ddlHoraComparecimento"
                                        ValueToCompare="-1" Operator="GreaterThan" Type="Integer" ErrorMessage="Hora é obrigatório."
                                        Display="Dynamic">*</asp:CompareValidator>
                                    &nbsp;
                                </div>
                                <div style="float: left;">
                                    <asp:Label ID="Label9" runat="server" Text="Minuto *" AssociatedControlID="ddlMinutos"></asp:Label>
                                    <asp:DropDownList ID="ddlMinutoComparecimento" runat="server" Enabled="true">
                                    </asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator5" runat="server" ValueToCompare="-1" Operator="GreaterThan"
                                        Type="Integer" ErrorMessage="Minuto é obrigatório." Display="Dynamic" ControlToValidate="ddlMinutoComparecimento">*</asp:CompareValidator>
                                </div>
                            </div>
                        </div>
                        <!-- FiltrosConviteReuniao -->
                        <div id="divConviteReuniao" runat="server" visible="false">
                            <div>
                                <asp:Label ID="Label10" runat="server" Text="Data *" AssociatedControlID="txtDataReuniao"></asp:Label>
                                <asp:TextBox ID="txtDataReuniao" runat="server" Enabled="true" MaxLength="10" SkinID="Data"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtDataReuniao"
                                    ErrorMessage="Data é obrigatório.">*</asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompareValidator6" runat="server" ControlToValidate="txtDataReuniao"
                                    Display="Dynamic" ErrorMessage="Data é inválida ou inexistente (DD/MM/AAAA)."
                                    Operator="DataTypeCheck" Type="Date" SetFocusOnError="true">*</asp:CompareValidator>
                                <br />
                            </div>
                            <div style="float: left">
                                <asp:Label ID="Labelinicio" runat="server" Text="Início" Font-Bold="true"></asp:Label>
                                <fieldset id="Fieldset1" runat="server" style="width: 120px">
                                    <div style="float: left; width: 60px">
                                        <asp:Label ID="lblHora" runat="server" Text="Hora *" AssociatedControlID="ddlHora"></asp:Label>
                                        <asp:DropDownList ID="ddlHora" runat="server" Enabled="true">
                                        </asp:DropDownList>
                                        <asp:CompareValidator ID="_cpvValidadeHora" runat="server" ControlToValidate="ddlHora"
                                            ValueToCompare="-1" Operator="GreaterThan" Type="Integer" ErrorMessage="Hora de início é obrigatório."
                                            Display="Dynamic">*</asp:CompareValidator>
                                    </div>
                                    <div style="float: left;">
                                        <asp:Label ID="lblMinutos" runat="server" Text="Minuto *" AssociatedControlID="ddlMinutos"></asp:Label>
                                        <asp:DropDownList ID="ddlMinutos" runat="server" Enabled="true">
                                        </asp:DropDownList>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ValueToCompare="-1" Operator="GreaterThan"
                                            Type="Integer" ErrorMessage="Minuto de início é obrigatório." Display="Dynamic"
                                            ControlToValidate="ddlminutos">*</asp:CompareValidator>
                                    </div>
                                </fieldset>
                            </div>
                            <div style="float: left; margin-left: 30px">
                                <asp:Label ID="lblTermino" runat="server" Text="Término" Font-Bold="true"></asp:Label>
                                <fieldset id="Fieldset2" runat="server" style="width: 120px; white-space: nowrap">
                                    <div style="float: left; width: 60px">
                                        <asp:Label ID="lblHoraFinal" runat="server" Text="Hora *" AssociatedControlID="ddlHoraFinal"></asp:Label>
                                        <asp:DropDownList ID="ddlHoraFinal" runat="server" Enabled="true">
                                        </asp:DropDownList>
                                        <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="ddlHoraFinal"
                                            ValueToCompare="-1" Operator="GreaterThan" Type="Integer" ErrorMessage="Hora de término é obrigatório."
                                            Display="Dynamic">*</asp:CompareValidator>
                                        &nbsp;
                                    </div>
                                    <div style="float: left;">
                                        <asp:Label ID="lblMinutosFinal" runat="server" Text="Minuto *" AssociatedControlID="ddlMinutosFinal"></asp:Label>
                                        <asp:DropDownList ID="ddlMinutosFinal" runat="server" Enabled="true">
                                        </asp:DropDownList>
                                        <asp:CompareValidator ID="CompareValidator3" runat="server" ValueToCompare="-1" Operator="GreaterThan"
                                            Type="Integer" ErrorMessage="Minuto de término é obrigatório." Display="Dynamic"
                                            ControlToValidate="ddlMinutosFinal">*</asp:CompareValidator>
                                    </div>
                                </fieldset>
                            </div>
                            <div style="width: 500px; white-space: nowrap">
                                <asp:Label ID="lblAssunto" runat="server" Text="Assuntos *" AssociatedControlID="txtAssunto"></asp:Label>
                                <asp:TextBox ID="txtAssunto" runat="server" TextMode="MultiLine" ValidationGroup="Convite"
                                    Width="500px"> </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvAssunto" runat="server" ControlToValidate="txtAssunto"
                                    ErrorMessage="Assunto é obrigatório.">*</asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="clear">
                        </div>
                    </div>
                    <div class="right">
                        <asp:Button ID="btnGerarRelatorioDeclaracaoSolicitacaoVaga" runat="server" Text="Gerar relatório"
                            OnClick="btnGerarRelatorioDeclaracaoSolicitacaoVaga_Click" Visible="false" />
                        <asp:Button ID="btnGerarRelatorioConviteReuniao" runat="server" Text="Gerar relatório"
                            OnClick="btnGerarRelatorioConviteReuniao_Click" Visible="false" />
                        <asp:Button ID="btnGerarRelatorioDeclaracaoSolicitacaoComparecimento" runat="server"
                            Text="Gerar relatório" OnClick="btnGerarRelatorioDeclaracaoSolicitacaoComparecimento_Click"
                            Visible="false" />
                        <asp:Button ID="btnGerarDeclaracaoTrabalho" runat="server" Text="Gerar relatório"
                            Visible="false" OnClick="btnGerarDeclaracaoTrabalho_Click" />
                        <asp:Button ID="btnGerarCertidaoEscolariedadeAnterior1972" runat="server"
                            Text="Gerar relatório" Visible="False"
                            OnClick="btnGerarCertidaoEscolariedadeAnterior1972_Click" />
                        <asp:Button ID="_btnPesquisar" runat="server" Text="Pesquisar" OnClick="_btnPesquisar_Click" />
                    </div>
                </fieldset>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="_btnPesquisar" />
        </Triggers>
    </asp:UpdatePanel>
    <div class="clear">
    </div>
    <asp:UpdatePanel ID="_updResultado" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset id="fdsResultados" runat="server">
                <legend>Resultados</legend>
                <div class="right area-botoes-top">
                    <asp:Button ID="btnGerarRelatorioCima" runat="server" Text="Gerar relatório" OnClick="_btnGerarRelatorio_Click" />
                </div>
                <div class="area-form">
                    <br />
                    <br />
                    <div id="DivSelecionaTodos" runat="server">
                        <div style="float: left; width: 50%">
                            <asp:CheckBox ID="_chkTodos" SkinID="chkTodos" Text="Selecionar todos os alunos" todososcursospeja='0'
                                runat="server" />
                        </div>
                        <div align="right" id="divQtdPaginacao" runat="server">
                            <asp:Label ID="_lblPag" runat="server" Text="Itens por página"></asp:Label>
                            <asp:DropDownList ID="_ddlQtPaginado" runat="server" AutoPostBack="True" OnSelectedIndexChanged="_ddlQtPaginado_SelectedIndexChanged">
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>20</asp:ListItem>
                                <asp:ListItem>50</asp:ListItem>
                                <asp:ListItem>100</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <br />                
                    <asp:GridView ID="_grvDocumentoAluno" runat="server" AllowPaging="True" AllowCustomPaging="true" AutoGenerateColumns="False"
                        BorderStyle="None" DataKeyNames="alu_id,tur_id,cal_id,esc_id,mtu_id,EscolaUniDestino,GrupamentoDestino,pes_nome,tur_escolaUnidade"
                        EmptyDataText="A pesquisa não encontrou resultados."
                        OnRowDataBound="_grvDocumentoAluno_RowDataBound" OnDataBound="_grvDocumentoAluno_DataBound"
                        AllowSorting="True" OnPageIndexChanging="_grvDocumentoAluno_PageIndexChanging" OnSorting="_grvDocumentoAluno_Sorting"
                        EnableModelValidation="True" SkinID="GridResponsive">
                        <Columns>
                            <%--0--%>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="_chkSelecionar" runat="server" alu_id='<%# Eval("alu_id") %>' cal_id='<%# Eval("cal_id") %>'
                                        tur_id='<%# Eval("tur_id") %>' esc_id='<%# Eval("esc_id") %>' cursopeja='<%# Eval("CursoPeja") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--1--%>
                            <asp:TemplateField HeaderText="Matricula estadual" SortExpression="alc_matriculaEstadual">
                                <ItemTemplate>
                                    <asp:Label ID="lblMatriculaEstadual" runat="server" Text='<%# Bind("alc_matriculaEstadual") %>'
                                        CssClass="wrap100px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--2--%>
                            <asp:TemplateField HeaderText="<%$ Resources:Mensagens,MSG_NUMEROMATRICULA %>" SortExpression="alc_matricula">
                                <ItemTemplate>
                                    <asp:Label ID="lblMatricula" runat="server" Text='<%# Bind("alc_matricula") %>' CssClass="wrap100px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--3--%>
                            <asp:BoundField HeaderText="Número chamada" Visible="false" />
                            <%--4--%>
                            <asp:TemplateField HeaderText="Nome" SortExpression="pes_nome">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("pes_nome") %>' CssClass="wrap150px"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="_lblAlterar" runat="server" Text='<%# Bind("pes_nome") %>' CssClass="wrap150px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--5--%>
                            <asp:TemplateField HeaderText="Escola" SortExpression="tur_escolaUnidade">
                                <ItemTemplate>
                                    <asp:Label ID="lblEscola" runat="server" Text='<%# Bind("tur_escolaUnidade") %>'
                                        CssClass="wrap150px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--6--%>
                            <asp:BoundField HeaderText="Data de nascimento" Visible="false" />
                            <%--7--%>
                            <asp:BoundField DataField="tur_codigo" HeaderText="Turma" SortExpression="tur_codigo" />
                            <%--8--%>
                            <asp:TemplateField HeaderText="Curso" SortExpression="tur_curso">
                                <ItemTemplate>
                                    <asp:Label ID="lblCurso" runat="server" Text='<%# Bind("tur_curso") %>' CssClass="wrap150px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--9--%>
                            <asp:TemplateField HeaderText="Calendário" SortExpression="tur_calendario">
                                <ItemTemplate>
                                    <asp:Label ID="lblCalendario" runat="server" Text='<%# Bind("tur_calendario") %>'
                                        CssClass="wrap100px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--10--%>
                            <asp:BoundField DataField="alu_situacao" HeaderText="Situação" SortExpression="alu_situacao" Visible="false" />
                        </Columns>
                    </asp:GridView>
                    <uc8:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="_grvDocumentoAluno"
                        class="clTotalReg" />
                </div>
                <div class="right area-botoes-bottom">
                    <asp:Button ID="_btnGerarRelatorio" runat="server" Text="Gerar relatório" OnClick="_btnGerarRelatorio_Click" />
                </div>
            </fieldset>


        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="_btnGerarRelatorio" />
            <asp:AsyncPostBackTrigger ControlID="btnGerarRelatorioCima" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
