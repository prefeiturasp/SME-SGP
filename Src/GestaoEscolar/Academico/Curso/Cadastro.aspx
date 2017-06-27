<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Academico_Curso_Cadastro" CodeBehind="Cadastro.aspx.cs" %>

<%@ PreviousPageType VirtualPath="~/Academico/Curso/Busca.aspx" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoNivelEnsino.ascx" TagName="UCComboTipoNivelEnsino"
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoModalidadeEnsino.ascx" TagName="UCComboTipoModalidadeEnsino"
    TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoDisciplina.ascx" TagName="UCComboTipoDisciplina"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc6" %>
<%@ Register Src="~/WebControls/Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoCiclo.ascx" TagPrefix="uc7" TagName="UCComboTipoCiclo" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoCurriculoPeriodo.ascx" TagPrefix="uc1" TagName="UCComboTipoCurriculoPeriodo" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="divPeriodo" runat="server" title="Cadastro de períodos" class="hide divPeriodo">
        <asp:UpdatePanel ID="_updCadastroPeriodo" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:UCLoader ID="UCLoader1" runat="server" AssociatedUpdatePanelID="_updCadastroPeriodo" />
                <asp:Label ID="_lblMessagePeriodo" runat="server" EnableViewState="False"></asp:Label>
                <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="Periodo" />
                <fieldset>
                    <uc6:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" />
                    <asp:Label ID="LabelDescricaoPeriodo" runat="server" Text=" " AssociatedControlID="_txtDescricaoPeriodo"></asp:Label>
                    <asp:TextBox ID="_txtDescricaoPeriodo" runat="server" MaxLength="200" Width="200px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="_rfvDescricaoPeriodo" runat="server" ControlToValidate="_txtDescricaoPeriodo"
                        ValidationGroup="Periodo" Display="Dynamic" ErrorMessage="Descrição é obrigatório.">*</asp:RequiredFieldValidator>
                    <asp:CheckBox ID="chkCrpConcluiNivelEnsino" runat="server" Text="Conclui nível de ensino" />
                    <asp:Label ID="_lblOrdemPeriodo" runat="server" Text="Ordem do período *" AssociatedControlID="_txtOrdemPeriodo"></asp:Label>
                    <asp:TextBox ID="_txtOrdemPeriodo" runat="server" MaxLength="3" SkinID="Numerico"
                        CssClass="numeric"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="_rfvOrdemPeriodo" runat="server" ControlToValidate="_txtOrdemPeriodo"
                        ValidationGroup="Periodo" Display="Dynamic" ErrorMessage="Ordem do período é obrigatório.">*</asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="cpvOrdemPeriodo" runat="server" Text="*" ErrorMessage="Ordem do período não pode ser maior do que 999."
                        ControlToValidate="_txtOrdemPeriodo" Type="Integer" Operator="LessThanEqual"
                        ValueToCompare="999" Display="Dynamic" ValidationGroup="Periodo">*</asp:CompareValidator>
                    <asp:Label ID="_lblIdadeIdealInicio" runat="server" Text="Idade mínima ideal *" AssociatedControlID="_txtIdadeIdealAnoInicio"></asp:Label>
                    <asp:TextBox ID="_txtIdadeIdealAnoInicio" runat="server" MaxLength="3" SkinID="Numerico"
                        CssClass="numeric"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="_rfvIdadeIdealAnoInicio" runat="server" ControlToValidate="_txtIdadeIdealAnoInicio"
                        ValidationGroup="Periodo" Display="Dynamic" ErrorMessage="Ano(s) da idade mínima ideal é obrigatório.">*</asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="cpvIdadeIdealAnoInicio" runat="server" Text="*" ErrorMessage="Ano(s) da idade mínima ideal não pode ser maior do que 999."
                        ControlToValidate="_txtIdadeIdealAnoInicio" Type="Integer" Operator="LessThanEqual"
                        ValueToCompare="999" Display="Dynamic" ValidationGroup="Periodo">*</asp:CompareValidator>
                    <asp:Label ID="_lblIdadeIdealAnoInicio" runat="server" Text="ano(s)"></asp:Label>
                    <asp:TextBox ID="_txtIdadeIdealMesInicio" runat="server" MaxLength="2" SkinID="Numerico"
                        CssClass="numeric"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="_rfvIdadeIdealMesInicio" runat="server" ControlToValidate="_txtIdadeIdealMesInicio"
                        ValidationGroup="Periodo" Display="Dynamic" ErrorMessage="Mês(es) da idade mínima ideal é obrigatório.">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="_revIdadeIdealMesInicio" runat="server" ControlToValidate="_txtIdadeIdealMesInicio"
                        ValidationGroup="Periodo" Display="Dynamic" ErrorMessage="Mês(es) da idade mínima ideal não pode ser maior do que 11."
                        ValidationExpression="^(0[0-9]|[\d]|1[0,1])$">*</asp:RegularExpressionValidator>
                    <asp:Label ID="_lblIdadeIdelaMesInicio" runat="server" Text="mês(es)"></asp:Label>
                    <asp:Label ID="_lblIdadeIdealFim" runat="server" Text="Idade máxima ideal *" AssociatedControlID="_txtIdadeIdealAnoFim"></asp:Label>
                    <asp:TextBox ID="_txtIdadeIdealAnoFim" runat="server" MaxLength="3" SkinID="Numerico"
                        CssClass="numeric"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="_rfvIdadeIdealAnoFim" runat="server" ControlToValidate="_txtIdadeIdealAnoFim"
                        ValidationGroup="Periodo" Display="Dynamic" ErrorMessage="Ano(s) da idade máxima ideal é obrigatório.">*</asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="cpvIdadeIdealAnoFim" runat="server" Text="*" ErrorMessage="Ano(s) da idade máxima ideal não pode ser maior do que 999."
                        ControlToValidate="_txtIdadeIdealAnoFim" Type="Integer" Operator="LessThanEqual"
                        ValueToCompare="999" Display="Dynamic" ValidationGroup="Periodo">*</asp:CompareValidator>
                    <asp:Label ID="_lblIdadeIdealMesFim" runat="server" Text="ano(s)"></asp:Label>
                    <asp:TextBox ID="_txtIdadeIdealMesFim" runat="server" MaxLength="2" SkinID="Numerico"
                        CssClass="numeric"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="_rfvIdadeIdealMesFim" runat="server" ControlToValidate="_txtIdadeIdealMesFim"
                        ValidationGroup="Periodo" Display="Dynamic" ErrorMessage="Mês(es) da idade máxima ideal é obrigatório.">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="_revIdadeIdealMesFim" runat="server" ControlToValidate="_txtIdadeIdealMesFim"
                        ValidationGroup="Periodo" Display="Dynamic" ErrorMessage="Mês(es) da idade máxima ideal não pode ser maior do que 11."
                        ValidationExpression="^(0[0-9]|[\d]|1[0,1])$">*</asp:RegularExpressionValidator>
                    <asp:Label ID="_lblIdadeIdelaMesFim" runat="server" Text="mês(es)"></asp:Label>

                    <uc7:UCComboTipoCiclo runat="server" ID="UCComboTipoCiclo" Obrigatorio="false" Titulo="Ciclo" />

                    <uc1:UCComboTipoCurriculoPeriodo runat="server" ID="UCComboTipoCurriculoPeriodo" />




                    <asp:Label ID="LabelQtdeDiasSemana" runat="server" Text="Quantidade de dias da semana que possui aula *"
                        AssociatedControlID="_txtQtdeDiasSemana"></asp:Label>
                    <asp:TextBox ID="_txtQtdeDiasSemana" runat="server" MaxLength="1" SkinID="Numerico"
                        CssClass="numeric"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="_rfvQtdeDiasSemana" runat="server" ControlToValidate="_txtQtdeDiasSemana"
                        ValidationGroup="Periodo" Display="Dynamic" ErrorMessage="Quantidade de dias da semana que possui aula é obrigatório.">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="_revQtdeDiasSemana" runat="server" ControlToValidate="_txtQtdeDiasSemana"
                        ValidationGroup="Periodo" Display="Dynamic" ErrorMessage="Quantidade de dias da semana que possui aula deve conter apenas números de 1(um) à 7(sete)."
                        ValidationExpression="^([1-7]){1}$">*</asp:RegularExpressionValidator>
                    <asp:Label ID="LabelControleTempo" runat="server" Text="Controle de horas/aulas *"
                        AssociatedControlID="_ddlControleTempo"></asp:Label>
                    <asp:DropDownList ID="_ddlControleTempo" runat="server" AppendDataBoundItems="True"
                        AutoPostBack="True" OnSelectedIndexChanged="_ddlControleTempo_SelectedIndexChanged">
                        <asp:ListItem Value="-1">-- Selecione um controle de horas/aulas --</asp:ListItem>
                        <asp:ListItem Value="1">Tempos de aula</asp:ListItem>
                        <asp:ListItem Value="2">Horas</asp:ListItem>
                    </asp:DropDownList>
                    <asp:CompareValidator ID="_cpvControleTempo" runat="server" ErrorMessage="Controle de horas/aulas é obrigatório."
                        ControlToValidate="_ddlControleTempo" Operator="GreaterThan" ValueToCompare="0"
                        Display="Dynamic" ValidationGroup="Periodo">*</asp:CompareValidator>
                    <div id="divTemposAula" runat="server">
                        <asp:Label ID="LabelQtdeTemposDia" runat="server" Text="Quantidade de tempos de aula de um dia *"
                            AssociatedControlID="_txtQtdeTemposDia"></asp:Label>
                        <asp:TextBox ID="_txtQtdeTemposDia" runat="server" MaxLength="3" SkinID="Numerico"
                            CssClass="numeric"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="_rfvQtdeTemposDia" runat="server" ControlToValidate="_txtQtdeTemposDia"
                            ValidationGroup="Periodo" Display="Dynamic" ErrorMessage="Quantidade de tempos de aula de um dia é obrigatório.">*</asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="_cpvQtdeTemposDia" runat="server" Text="*" ErrorMessage="Quantidade de tempos de aula de um dia não pode ser maior do que 255."
                            ControlToValidate="_txtQtdeTemposDia" Type="Integer" Operator="LessThanEqual"
                            ValueToCompare="255" Display="Dynamic" ValidationGroup="Periodo">*</asp:CompareValidator>
                    </div>
                    <div id="divHoras" runat="server">
                        <asp:Label ID="_lblCargaHorariaHora" runat="server" Text="Carga horária total do dia *"
                            AssociatedControlID="_txtCargaHorariaHora"></asp:Label>
                        <asp:TextBox ID="_txtCargaHorariaHora" runat="server" MaxLength="2" SkinID="Numerico"
                            CssClass="numeric"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="_rfvCargaHorariaHora" runat="server" ControlToValidate="_txtCargaHorariaHora"
                            ValidationGroup="Periodo" Display="Dynamic" ErrorMessage="Hora(s) da carga horária total do dia é obrigatório.">*</asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cpvCargaHorariaHora" runat="server" Text="*" ErrorMessage="Hora(s) da carga horária total do dia não pode ser maior do que 23."
                            ControlToValidate="_txtCargaHorariaHora" Type="Integer" Operator="LessThanEqual"
                            ValueToCompare="23" Display="Dynamic" ValidationGroup="Periodo">*</asp:CompareValidator>
                        <asp:Label ID="_lblCargaHorariaDiaHora" runat="server" Text="hora(s)"></asp:Label>
                        <asp:TextBox ID="_txtCargaHorariaMinuto" runat="server" MaxLength="2" SkinID="Numerico"
                            CssClass="numeric"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="_rfvCargaHorariaMinuto" runat="server" ControlToValidate="_txtCargaHorariaMinuto"
                            ValidationGroup="Periodo" Display="Dynamic" ErrorMessage="Minuto(s) da carga horária total do dia é obrigatório.">*</asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cpvCargaHorariaMinuto" runat="server" Text="*" ErrorMessage="Minuto(s) da carga horária total do dia não pode ser maior do que 59."
                            ControlToValidate="_txtCargaHorariaMinuto" Type="Integer" Operator="LessThanEqual"
                            ValueToCompare="59" Display="Dynamic" ValidationGroup="Periodo">*</asp:CompareValidator>
                        <asp:Label ID="Label1" runat="server" Text="minuto(s)"></asp:Label>
                    </div>
                    <asp:Label ID="_lblEletivasAlunos" runat="server" Text="<%$ Resources:Academico, Curso.Cadastro._lblEletivasAlunos.Text %>"
                        AssociatedControlID="_txtEletivasAlunos"></asp:Label>
                    <asp:TextBox ID="_txtEletivasAlunos" runat="server" MaxLength="3" SkinID="Numerico"
                        CssClass="numeric"></asp:TextBox>
                    <asp:CompareValidator ID="cpvEletivasAlunos" runat="server" Text="*"
                        ControlToValidate="_txtEletivasAlunos" Type="Integer" Operator="LessThanEqual"
                        ValueToCompare="255" Display="Dynamic" ValidationGroup="Periodo">*</asp:CompareValidator>
                    <asp:CheckBox ID="chkTurmaAvaliacao" runat="server" Text="Turmas por avaliação" AutoPostBack="true"
                        Visible="false" OnCheckedChanged="chkTurmaAvaliacao_CheckedChanged" />
                    <div id="divTurmaAvaliacao" runat="server" visible="false">
                        <asp:Label ID="LabelNomeAvaliacao" runat="server" Text="Nome da avaliação *" AssociatedControlID="_txtNomeAvaliacao"></asp:Label>
                        <asp:TextBox ID="_txtNomeAvaliacao" runat="server" MaxLength="100" Width="200px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="_rfvNomeAvaliacao" runat="server" ControlToValidate="_txtNomeAvaliacao"
                            ValidationGroup="Periodo" Display="Dynamic" ErrorMessage="Nome da avaliação é obrigatório.">*</asp:RequiredFieldValidator>
                    </div>
                    <div runat="server" id="divFundoFrente">
                        <asp:Label ID="lblFundoFrente" runat="server" AssociatedControlID="txtFundoFrente"
                            Text="<%$ Resources:Academico, Curso.Cadastro.lblFundoFrente.Text %>"></asp:Label>
                        <asp:TextBox ID="txtFundoFrente" runat="server" SkinID="text60C" MaxLength="260"></asp:TextBox>
                    </div>
                    <div class="right">
                        <asp:Button ID="_btnCancelarPeriodo" runat="server" CausesValidation="False" Text="Cancelar"
                            OnClientClick="$('.divPeriodo').dialog('close');" />
                    </div>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div runat="server" id="divDisp">
        <div id="divDisciplina" class="hide divDisciplina" runat="server">
            <asp:UpdatePanel ID="_updCadastroDisciplina" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc1:UCLoader ID="UCLoader2" runat="server" AssociatedUpdatePanelID="_updCadastroDisciplina" />
                    <asp:Label ID="_lblMessageDisciplina" runat="server" EnableViewState="False"></asp:Label>
                    <asp:ValidationSummary ID="ValidationSummary4" runat="server" ValidationGroup="Disciplina" />
                    <fieldset>
                        <uc6:UCCamposObrigatorios ID="UCCamposObrigatorios2" runat="server" />
                        <uc1:UCComboTipoDisciplina ID="UCComboTipoDisciplina1" runat="server" />
                        <asp:Label ID="LabelNomeDisciplina" runat="server" Text="<%$ Resources:Academico, Curso.Cadastro.LabelNomeDisciplina.Text %>" AssociatedControlID="_txtNomeDisciplina"></asp:Label>
                        <asp:TextBox ID="_txtNomeDisciplina" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="_rfvNomeDisciplina" runat="server" ControlToValidate="_txtNomeDisciplina"
                            ValidationGroup="Disciplina" Display="Dynamic" ErrorMessage="<%$ Resources:Academico, Curso.Cadastro._rfvNomeDisciplina.ErrorMessage %>">*</asp:RequiredFieldValidator>
                        <asp:Label ID="LabelNomeAbreviadoDisciplina" runat="server" Text="Nome abreviado"
                            AssociatedControlID="_txtNomeAbreviadoDisciplina"></asp:Label>
                        <asp:TextBox ID="_txtNomeAbreviadoDisciplina" runat="server" MaxLength="20" SkinID="text20C"></asp:TextBox>
                        <asp:Label ID="LabelCodigoDisciplina" runat="server" Text="Código" AssociatedControlID="_txtCodigoDisciplina"></asp:Label>
                        <asp:TextBox ID="_txtCodigoDisciplina" runat="server" MaxLength="10" SkinID="text10C"></asp:TextBox>
                        <asp:Label ID="LabelCargaHorariaExtraClasse" runat="server" Text="<%$ Resources:Academico, Curso.Cadastro.LabelCargaHorariaExtraClasse.Text %>" AssociatedControlID="_txtCargaHorariaExtraClasse"></asp:Label>
                        <asp:TextBox ID="_txtCargaHorariaExtraClasse" runat="server" SkinID="Decimal" MaxLength="8"></asp:TextBox>                        
                        <div class="right">
                            <asp:Button ID="_btnCancelarDisciplina" runat="server" CausesValidation="False" Text="Cancelar"
                                OnClientClick="$('.divDisciplina').dialog('close');" />
                        </div>
                    </fieldset>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <div id="divDisciplinaPeriodo" runat="server" class="hide divDisciplinaPeriodo">
        <asp:UpdatePanel ID="_updCadastroDisciplinaPeriodo" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:UCLoader ID="UCLoader3" runat="server" AssociatedUpdatePanelID="_updCadastroDisciplinaPeriodo" />
                <asp:Label ID="_lblMessageDisciplinaPeriodo" runat="server" EnableViewState="False"></asp:Label>
                <asp:ValidationSummary ID="ValidationSummary6" runat="server" ValidationGroup="DisciplinaPeriodo" />
                <fieldset>
                    <asp:Label ID="_lblDisciplina" runat="server"></asp:Label>
                </fieldset>
                <fieldset>
                    <asp:Label ID="LabelTipoDisciplina" runat="server" Text="<%$ Resources:Academico, Curso.Cadastro.LabelTipoDisciplina.Text %>"
                        AssociatedControlID="_ddlTipoDisciplina"></asp:Label>
                    <asp:DropDownList ID="_ddlTipoDisciplina" runat="server" AppendDataBoundItems="True"
                        AutoPostBack="True" OnSelectedIndexChanged="_ddlTipoDisciplina_SelectedIndexChanged">
                        <asp:ListItem Value="-1" Text="-- Selecione um tipo --" />
                        <asp:ListItem Value="1" Text="Obrigatória" />
                        <asp:ListItem Value="3" Text="Optativa" />
                        <asp:ListItem Value="4" Text="Eletiva" />
                        <asp:ListItem Value="5" Text="<%$ Resources:Academico, Curso.Cadastro._ddlTipoDisciplina.valor5 %>" />
                        <asp:ListItem Value="6" Text="Docente da turma e docente específico - obrigatória" />
                        <asp:ListItem Value="7" Text="Docente da turma e docente específico - eletiva" />
                        <asp:ListItem Value="8" Text="Depende da disponibilidade de professor - obrigatória" />
                        <asp:ListItem Value="9" Text="Depende da disponibilidade de professor - eletiva" />
                    </asp:DropDownList>
                    <asp:CompareValidator ID="_cpvTipoDisciplinaTemposAula" runat="server" ErrorMessage="<%$ Resources:Academico, Curso.Cadastro._cpvTipoDisciplinaTemposAula.ErrorMessage %>"
                        ControlToValidate="_ddlTipoDisciplina" Operator="GreaterThan" ValueToCompare="0"
                        Display="Dynamic" ValidationGroup="DisciplinaPeriodo">*</asp:CompareValidator>
                    <br />
                    <br />
                    <fieldset id="fdsEletivas" runat="server">
                        <legend>
                            <asp:Label runat="server" ID="lblLegend" Text="<%$ Resources:Academico, Curso.Cadastro.lblLegend.Text %>"></asp:Label></legend>
                        <asp:Label ID="LabelNomeGrupoEletivas" runat="server" Text="Nome do grupo de eletivas"
                            AssociatedControlID="_txtNomeGrupoEletivas"></asp:Label>
                        <asp:TextBox ID="_txtNomeGrupoEletivas" runat="server" MaxLength="200" Width="200px"></asp:TextBox>
                        <asp:CheckBoxList ID="chkDisciplinasEletivas" runat="server">
                        </asp:CheckBoxList>
                    </fieldset>
                    <div id="divCargaHoraria" runat="server">
                        <asp:Label ID="LabelCargaHorariaSemanal" runat="server" Text="Carga horária semanal"
                            AssociatedControlID="_txtCargaHorariaSemanal"></asp:Label>
                        <asp:TextBox ID="_txtCargaHorariaSemanal" runat="server" MaxLength="3" SkinID="Numerico"
                            CssClass="numeric"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="_rfvCargaHorariaSemanal" runat="server" ControlToValidate="_txtCargaHorariaSemanal"
                            ErrorMessage="Carga horária semanal é obrigatório." ValidationGroup="DisciplinaPeriodo"
                            Display="Dynamic" Visible="false">*</asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cpvCargaHorariaSemanal" runat="server" Text="*" ErrorMessage="Carga horária semanal não pode ser maior do que 168."
                            ControlToValidate="_txtCargaHorariaSemanal" Type="Integer" Operator="LessThanEqual"
                            ValueToCompare="168" Display="Dynamic" ValidationGroup="DisciplinaPeriodo">*</asp:CompareValidator>
                        <asp:Label ID="LabelCargaHorariaAnual" runat="server" Text="Carga horária anual"
                            AssociatedControlID="_txtCargaHorariaAnual"></asp:Label>
                        <asp:TextBox ID="_txtCargaHorariaAnual" runat="server" MaxLength="3" SkinID="Numerico"
                            CssClass="numeric"></asp:TextBox>
                    </div>
                    <asp:Label ID="LabelEmenta" runat="server" Text="Ementa" AssociatedControlID="_txtEmenta"></asp:Label>
                    <asp:TextBox ID="_txtEmenta" runat="server" SkinID="text60C" TextMode="MultiLine"></asp:TextBox>
                    <div class="right">
                        <asp:Button ID="_btnCancelarDisciplinaPeriodo" runat="server" CausesValidation="False"
                            Text="Cancelar" OnClientClick="$('.divDisciplinaPeriodo').dialog('close');" />
                    </div>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div runat="server" id="divEletivas">
        <div id="divEletivasAlunos" class="hide divEletivasAlunos" runat="server" title="<%$ Resources:Academico, Curso.Cadastro.divEletivasAlunos.Title %>">
            <div id="divDadosEletivasAlunos" runat="server">
                <asp:UpdatePanel ID="updCadastroEletivasAlunos" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <uc1:UCLoader ID="UCLoader7" runat="server" AssociatedUpdatePanelID="updCadastroEletivasAlunos" />
                        <asp:Label ID="lblMessageEletivasAlunos" runat="server" EnableViewState="False"></asp:Label>
                        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="EletivasAlunos" />
                        <fieldset>
                            <uc6:UCCamposObrigatorios ID="UCCamposObrigatorios3" runat="server" />
                            <asp:Label ID="LabelNomeDisciplinaEletivasAlunos" runat="server" Text="Título informal para divulgação *"
                                AssociatedControlID="txtNomeDisciplinaEletivasAlunos"></asp:Label>
                            <asp:TextBox ID="txtNomeDisciplinaEletivasAlunos" runat="server" MaxLength="40" SkinID="text30C"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvNomeDisciplinaEletivasAlunos" runat="server" ControlToValidate="txtNomeDisciplinaEletivasAlunos"
                                ValidationGroup="EletivasAlunos" Display="Dynamic" ErrorMessage="<%$ Resources:Academico, Curso.Cadastro.rfvNomeDisciplinaEletivasAlunos.ErrorMessage %>">*</asp:RequiredFieldValidator>
                            <asp:Label ID="lblNomeDocumentacoes" runat="server" AssociatedControlID="txtNomeDocumentacoes"
                                Text="<%$ Resources:Academico, Curso.Cadastro.lblNomeDocumentacoes.Text %>"></asp:Label>
                            <asp:TextBox ID="txtNomeDocumentacoes" runat="server" MaxLength="40" SkinID="text30C"></asp:TextBox>
                            <asp:Label ID="lblSigla" runat="server" Text="Código *" AssociatedControlID="txtSigla">
                            </asp:Label>
                            <asp:TextBox ID="txtSigla" runat="server" MaxLength="10" SkinID="text10C">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvSigla" ControlToValidate="txtSigla" ValidationGroup="EletivasAlunos"
                                runat="server" ErrorMessage="Código é obrigatório.">*
                            </asp:RequiredFieldValidator>
                            <asp:Label ID="LabelObjetivosEletivasAlunos" runat="server" Text="Objetivos" AssociatedControlID="txtObjetivosEletivasAlunos"></asp:Label>
                            <asp:TextBox ID="txtObjetivosEletivasAlunos" runat="server" SkinID="text60C" TextMode="MultiLine"></asp:TextBox>
                            <asp:Label ID="LabelHabilidadesEletivasAlunos" runat="server" AssociatedControlID="txtHabilidadesEletivasAlunos"
                                Text="Competências e Habilidades"></asp:Label>
                            <asp:TextBox ID="txtHabilidadesEletivasAlunos" runat="server" SkinID="text60C" TextMode="MultiLine"></asp:TextBox>
                            <asp:Label ID="LabelMetodologiasEletivasAlunos" runat="server" AssociatedControlID="txtMetodologiasEletivasAlunos"
                                Text="Atividades e Metodologias"></asp:Label>
                            <asp:TextBox ID="txtMetodologiasEletivasAlunos" runat="server" SkinID="text60C" TextMode="MultiLine"></asp:TextBox>
                            <asp:Label ID="LabelQtdeTemposAulaEletivasAlunos" runat="server" Text="Quantidade de tempos de aula por semana *"
                                AssociatedControlID="txtQtdeTemposAulaEletivasAlunos"></asp:Label>
                            <asp:TextBox ID="txtQtdeTemposAulaEletivasAlunos" runat="server" MaxLength="3" SkinID="Numerico"
                                CssClass="numeric"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvQtdeTemposAulaEletivasAlunos" runat="server" ControlToValidate="txtQtdeTemposAulaEletivasAlunos"
                                ValidationGroup="EletivasAlunos" Display="Dynamic" ErrorMessage="Quantidade de tempos de aula por semana é obrigatório.">*</asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cpvQtdeTemposAulaEletivasAlunos" runat="server" Text="*"
                                ErrorMessage="Quantidade de tempos de aula por semana não pode ser maior do que 255."
                                ControlToValidate="txtQtdeTemposAulaEletivasAlunos" Type="Integer" Operator="LessThanEqual"
                                ValueToCompare="255" Display="Dynamic" ValidationGroup="EletivasAlunos">*</asp:CompareValidator>
                            <asp:Panel ID="pnlPeriodosCursoEletivasAlunos" runat="server" GroupingText="Períodos do curso">
                                <asp:CheckBoxList ID="chkPeriodosCursoEletivasAlunos" runat="server">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:Label ID="LabelSituacaoEletivasAlunos" runat="server" Text="Situação *" AssociatedControlID="ddlSituacaoEletivasAlunos"></asp:Label>
                            <asp:DropDownList ID="ddlSituacaoEletivasAlunos" runat="server" AppendDataBoundItems="True">
                                <asp:ListItem Value="-1">-- Selecione uma situação --</asp:ListItem>
                                <asp:ListItem Value="1">Ativo</asp:ListItem>
                                <asp:ListItem Value="4">Inativo</asp:ListItem>
                            </asp:DropDownList>
                            <asp:CompareValidator ID="cvSituacaoEletivasAlunos" runat="server" ErrorMessage="Situação é obrigatório."
                                ControlToValidate="ddlSituacaoEletivasAlunos" Operator="GreaterThan" ValueToCompare="0"
                                Display="Dynamic" Text="*" ValidationGroup="EletivasAlunos"></asp:CompareValidator>
                            <br />
                            <fieldset id="fdsMacroCampo" runat="server">
                                <legend>Macro-campos</legend>
                                <asp:Repeater ID="rptCampos" runat="server">
                                    <HeaderTemplate>
                                        <div></div>
                                        <div class="checkboxlist-columns">
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnId" runat="server" Value='<%# Eval("tea_id") %>' />
                                        <asp:CheckBox ID="ckbCampo" runat="server" Text='<%# Eval("tea_nome") %>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </div>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </fieldset>
                            <div class="right">
                                <asp:Button ID="btnCancelarEletivasAlunos" runat="server" CausesValidation="False"
                                    Text="Cancelar" OnClientClick="$('.divEletivasAlunos').dialog('close');" />
                            </div>
                        </fieldset>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="_updMessage" runat="server">
        <ContentTemplate>
            <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Curso" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="divTabs">
        <ul class="hide">
            <li><a id="aDadosCurso" runat="server" href="#divTabs-0">Dados do curso</a></li>
            <li><a href="#divTabs-1">
                <asp:Label runat="server" ID="lblTab1" Text="<%$ Resources:Mensagens, MSG_DISCIPLINA_PLURAL %>"></asp:Label></a></li>
            <li><a id="aDisciplinasEletivasAlunos" runat="server" href="#divTabs-2">
                <asp:Label runat="server" ID="lblTab2" Text="<%$ Resources:Academico, Curso.Cadastro.lblTab2.Text %>"></asp:Label></a></li>
        </ul>
        <div id="divTabs-0">
            <fieldset>
                <asp:UpdatePanel ID="_updDadosCurso" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <uc1:UCLoader ID="UCLoader4" runat="server" AssociatedUpdatePanelID="_updDadosCurso" />
                        <uc6:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" />
                        <uc2:UCComboTipoNivelEnsino ID="UCComboTipoNivelEnsino1" runat="server" />
                        <uc3:UCComboTipoModalidadeEnsino ID="UCComboTipoModalidadeEnsino1" runat="server" />
                        <asp:Label ID="LabelCodigo" runat="server" Text="Código do curso" AssociatedControlID="_txtCodigo"></asp:Label>
                        <asp:TextBox ID="_txtCodigo" runat="server" MaxLength="10" Width="200px"></asp:TextBox>
                        <asp:Label ID="LabelNome" runat="server" Text="Nome do curso *" AssociatedControlID="_txtNome"></asp:Label>
                        <asp:TextBox ID="_txtNome" runat="server" MaxLength="200" Width="200px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="_rfvNomeCurso" runat="server" ControlToValidate="_txtNome"
                            ValidationGroup="Curso" Display="Dynamic">*</asp:RequiredFieldValidator>
                        <asp:Label ID="LabelNomeAbreviado" runat="server" Text="Nome abreviado" AssociatedControlID="_txtNomeAbreviado"></asp:Label>
                        <asp:TextBox ID="_txtNomeAbreviado" runat="server" MaxLength="20" Width="200px"></asp:TextBox>
                        <asp:CheckBox ID="_chkConcluiNivelEnsino" Text="Conclui nível de ensino" runat="server" />
                        <uc2:UCComboTipoNivelEnsino ID="UCComboTipoNivelEnsino2" runat="server" />
                        <asp:Label ID="LabelRegimeMatricula" runat="server" Text="Regime de matrícula *"
                            AssociatedControlID="_ddlRegimeMatricula"></asp:Label>
                        <asp:DropDownList ID="_ddlRegimeMatricula" runat="server" AppendDataBoundItems="True"
                            AutoPostBack="True" OnSelectedIndexChanged="_ddlRegimeMatricula_SelectedIndexChanged">
                            <asp:ListItem Value="-1">-- Selecione um regime --</asp:ListItem>
                            <asp:ListItem Value="1">Seriado</asp:ListItem>
                            <asp:ListItem Value="2">Por Créditos</asp:ListItem>
                            <asp:ListItem Value="3">Seriado, por avaliações</asp:ListItem>
                        </asp:DropDownList>
                        <asp:CompareValidator ID="_cpvRegimeMatricula" runat="server" ErrorMessage="Regime de matrícula é obrigatório."
                            ControlToValidate="_ddlRegimeMatricula" Operator="GreaterThan" ValueToCompare="0"
                            Display="Dynamic" ValidationGroup="Curso">*</asp:CompareValidator>
                        <div id="divQtdeAvaliacoes" runat="server" visible="false">
                            <asp:Label ID="LabelQtdeAvaliacoes" runat="server" Text="Quantidade de avaliações que o aluno deverá cursar para progredir *"
                                AssociatedControlID="_txtQtdeAvaliacoes"></asp:Label>
                            <asp:TextBox ID="_txtQtdeAvaliacoes" runat="server" MaxLength="3" SkinID="Numerico"
                                CssClass="numeric"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="_rfvQtdeAvaliacoes" runat="server" ControlToValidate="_txtQtdeAvaliacoes"
                                ValidationGroup="Curso" Display="Dynamic" ErrorMessage="Quantidade de avaliações que o aluno deverá cursar para progredir é obrigatório e deve ser um número inteiro maior que 0 (zero).">*</asp:RequiredFieldValidator>
                        </div>
                        <asp:Label ID="LabelPeriodosNormal" runat="server" Text="Quantidade normal de períodos *"
                            AssociatedControlID="_txtPeriodosNormal"></asp:Label>
                        <asp:TextBox ID="_txtPeriodosNormal" runat="server" MaxLength="3" SkinID="Numerico"
                            CssClass="numeric"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="_rfvPeriodosNormal" runat="server" ControlToValidate="_txtPeriodosNormal"
                            ValidationGroup="Curso" Display="Dynamic">*</asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="_cpvPeriodosNormal" runat="server" Text="*" ControlToValidate="_txtPeriodosNormal"
                            Type="Integer" Operator="LessThanEqual" ValueToCompare="255" Display="Dynamic"
                            ValidationGroup="Curso">*</asp:CompareValidator>
                        <asp:Label ID="LabelDiasLetivos" runat="server" Text="Quantidade de dias letivos *"
                            AssociatedControlID="_txtDiasLetivos"></asp:Label>
                        <asp:TextBox ID="_txtDiasLetivos" runat="server" MaxLength="3" SkinID="Numerico"
                            CssClass="numeric"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="_rfvDiasLetivos" runat="server" ControlToValidate="_txtDiasLetivos"
                            ValidationGroup="Curso" Display="Dynamic" ErrorMessage="Quantidade de dias letivos é obrigatório e deve ser um número inteiro maior que 0 (zero).">*</asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="_cpvDiasLetivos" runat="server" Text="*" ControlToValidate="_txtDiasLetivos"
                            ErrorMessage="Quantidade de dias letivos não pode ser maior do que 366." Type="Integer"
                            Operator="LessThanEqual" ValueToCompare="366" Display="Dynamic" ValidationGroup="Curso">*</asp:CompareValidator>
                        <asp:CheckBox ID="chkEfetivacaoSemestral" runat="server" Text="Permitir fechamento/efetivação semestral" />
                        <asp:Label ID="lblCargaHoraria" runat="server" Text="Carga horária"
                            AssociatedControlID="txtCargaHoraria"></asp:Label>
                        <asp:TextBox ID="txtCargaHoraria" runat="server" SkinID="Decimal" MaxLength="8"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="revCargaHoraria" runat="server" ControlToValidate="txtCargaHoraria"
                            ValidationExpression="[0-9]+(\,[0-9][0-9]?)?"
                            ErrorMessage="Carga horária não está no formato correto."
                            ValidationGroup="Curso">*</asp:RegularExpressionValidator>                        
                        <asp:Label ID="LabelVigenciaIni" runat="server" Text="Vigência inicial *" AssociatedControlID="_txtVigenciaIni"></asp:Label>
                        <asp:TextBox ID="_txtVigenciaIni" runat="server" MaxLength="10" SkinID="Data"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="_rfvVigenciaIni" runat="server" ControlToValidate="_txtVigenciaIni"
                            ValidationGroup="Curso" Display="Dynamic" ErrorMessage="Vigência inicial é obrigatório.">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="_revVigenciaIni" runat="server" ControlToValidate="_txtVigenciaIni"
                            ValidationExpression="(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d"
                            ErrorMessage="Data de vigência inicial não está no formato dd/mm/aaaa ou é inexistente."
                            ValidationGroup="Curso">*</asp:RegularExpressionValidator>
                        <asp:Label ID="LabelVigenciaFim" runat="server" Text="Vigência final" AssociatedControlID="_txtVigenciaFim"></asp:Label>
                        <asp:TextBox ID="_txtVigenciaFim" runat="server" MaxLength="10" SkinID="Data"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="_revVigenciaFim" runat="server" ControlToValidate="_txtVigenciaFim"
                            ValidationExpression="(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d"
                            ErrorMessage="Data de vigência final não está no formato dd/mm/aaaa ou é inexistente."
                            ValidationGroup="Curso">*</asp:RegularExpressionValidator>
                        <asp:CompareValidator ID="_cpvDataVigencia" runat="server" ErrorMessage="Data de vigência final deve ser maior ou igual a data de vigência inicial."
                            ControlToCompare="_txtVigenciaIni" ControlToValidate="_txtVigenciaFim" Operator="GreaterThanEqual"
                            Type="Date" ValidationGroup="Curso">*</asp:CompareValidator>
                        <asp:Label ID="LabelSituacao" runat="server" Text="Situação *" AssociatedControlID="_ddlCursoSituacao"></asp:Label><asp:DropDownList
                            ID="_ddlCursoSituacao" runat="server" AppendDataBoundItems="True">
                            <asp:ListItem Value="-1">-- Selecione uma situação --</asp:ListItem>
                            <asp:ListItem Value="1">Ativo</asp:ListItem>
                            <asp:ListItem Value="4">Desativado</asp:ListItem>
                            <asp:ListItem Value="5">Em ativação</asp:ListItem>
                            <asp:ListItem Value="6">Em desativação</asp:ListItem>
                        </asp:DropDownList>
                        <asp:CompareValidator ID="_cpvCursoSituacao" runat="server" ErrorMessage="Situação é obrigatório."
                            ControlToValidate="_ddlCursoSituacao" Operator="GreaterThan" ValueToCompare="0"
                            Display="Dynamic" ValidationGroup="Curso">*</asp:CompareValidator>
                        <asp:CheckBox ID="chkExclusivoDeficiente" runat="server" />
                        <asp:CheckBox ID="chknaoCausaConflitoSR" runat="server" />
                        <br />
                        <asp:Panel ID="pnlCursosRelacionados" runat="server" GroupingText="Cursos equivalentes"
                            Visible="false">
                            <div></div>
                            <div class="checkboxlist-columns">
                            <asp:CheckBoxList ID="cklCursoEquivalente" runat="server" DataTextField="cur_nome"
                                DataValueField="cur_crr_id" RepeatColumns="0" RepeatDirection="Horizontal" RepeatLayout="Flow" />
                                </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </fieldset>
        </div>
        <div id="divTabs-1" class="hide">
            <asp:UpdatePanel ID="_updGridPeriodo" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc1:UCLoader ID="UCLoader5" runat="server" AssociatedUpdatePanelID="_updGridPeriodo" />
                    <fieldset>
                        <asp:GridView ID="_grvCurriculo" runat="server" OnRowCommand="_grvCurriculo_RowCommand"
                                OnRowDataBound="_grvCurriculo_RowDataBound">
                        </asp:GridView>
                        <br />
                        <br />
                        <div style="font-weight: bold;">
                            Legenda:
                        </div>
                        <div id="divLegenda" runat="server" style="border-style: solid; border-width: thin; width: 550px;">
                            <asp:Label ID="LabelLegenda" runat="server"></asp:Label><br />
                            <br />
                            <table style="border-collapse: separate !important; border-spacing: 2px !important;">
                                <tr>
                                    <td bgcolor="DodgerBlue" height="15px" width="25px"></td>
                                    <td>Obrigatória
                                    </td>
                                    <td bgcolor="LimeGreen" height="15px" width="25px"></td>
                                    <td>Docente da turma e docente específico - obrigatória
                                    </td>
                                </tr>
                                <tr>
                                    <td bgcolor="red" height="15px" width="25px"></td>
                                    <td>Optativa
                                    </td>
                                    <td bgcolor="DarkGoldenrod" height="15px" width="25px"></td>
                                    <td>Docente da turma e docente específico - eletiva
                                    </td>
                                </tr>
                                <tr>
                                    <td bgcolor="DarkCyan" height="15px" width="25px"></td>
                                    <td>Eletiva
                                    </td>
                                    <td bgcolor="Purple" height="15px" width="25px"></td>
                                    <td>Depende da disponibilidade de professor - obrigatória
                                    </td>
                                </tr>
                                <tr>
                                    <td bgcolor="fuchsia" height="15px" width="25px"></td>
                                    <td>
                                        <asp:Label runat="server" ID="lblLegend2" Text="<%$ Resources:Academico, Curso.Cadastro.lblLegend2.Text %>"></asp:Label>
                                        &nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td bgcolor="Salmon" height="15px" width="25px"></td>
                                    <td>Depende da disponibilidade de professor - eletiva
                                    </td>
                                </tr>
                                <tr>
                                    <td bgcolor="DarkOrange" height="15px" width="25px"></td>
                                    <td>Regência
                                    </td>
                                    <td bgcolor="Maroon" height="15px" width="25px"></td>
                                    <td>Componente da Regência
                                    </td>
                                </tr>
                                <tr>
                                    <td bgcolor="DarkGreen" height="15px" width="25px"></td>
                                    <td>Docente específico – Complementação da regência
                                    </td>
                                    <td bgcolor="Sienna" height="15px" width="25px"></td>
                                    <td>Disciplina multisseriada
                                    </td>
                                </tr>
                                <tr>
                                    <td bgcolor="DarkKhaki" height="15px" width="25px"></td>
                                    <td>Disciplina multisseriada do aluno</td>
                                    <td bgcolor="DarkBlue" height="15px" width="25px"></td>
                                    <td>Docência compartilhada</td>
                                </tr>
                            </table>
                        </div>
                    </fieldset>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="divTabs-2">
            <div id="divGridEletivasAlunos" runat="server">
                <asp:UpdatePanel ID="updEletivasAlunos" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <uc1:UCLoader ID="UCLoader6" runat="server" AssociatedUpdatePanelID="updEletivasAlunos" />
                        <fieldset>
                            <asp:GridView ID="grvEletivasAlunos" runat="server" AutoGenerateColumns="False" OnRowCommand="grvEletivasAlunos_RowCommand"
                                OnRowDataBound="grvEletivasAlunos_RowDataBound" EmptyDataText="<%$ Resources:Academico, Curso.Cadastro.grvEletivasAlunos.EmptyDataText %>">
                                <Columns>
                                    <asp:TemplateField HeaderText="<%$ Resources:Mensagens, MSG_DISCIPLINA_PLURAL %>">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnAlterar" runat="server" CommandName="Alterar"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Períodos do curso">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPeriodos" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Situação">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSituacao" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </fieldset>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <fieldset>
        <div class="right">
            <asp:Button ID="_btnCancelar" runat="server" Text="Cancelar" CausesValidation="False"
                OnClick="_btnCancelar_Click" />
        </div>
    </fieldset>
    &nbsp;<input id="txtSelectedTab" type="hidden" runat="server" />
</asp:Content>
