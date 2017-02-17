<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Turma.TurmaEletivaAluno.Cadastro" %>

<%@ PreviousPageType VirtualPath="~/Turma/TurmaEletivaAluno/Busca.aspx" %>
<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="UCComboUAEscola"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboCursoCurriculo.ascx" TagName="UCComboCursoCurriculo"
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboDisciplina.ascx" TagName="UCComboDisciplina"
    TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Combos/UCComboCalendario.ascx" TagName="UCComboCalendario"
    TagPrefix="uc5" %>
<%@ Register Src="~/WebControls/Combos/UCComboFormatoAvaliacao.ascx" TagName="UCComboFormatoAvaliacao"
    TagPrefix="uc6" %>
<%@ Register Src="~/WebControls/Combos/UCComboTurno.ascx" TagName="UCComboTurno"
    TagPrefix="uc7" %>
<%@ Register Src="~/WebControls/Combos/UCComboDocente.ascx" TagName="UCComboDocente"
    TagPrefix="uc8" %>
<%@ Register Src="~/WebControls/Mensagens/UCConfirmacaoOperacao.ascx" TagName="UCConfirmacaoOperacao"
    TagPrefix="uc15" %>
<%@ Register Src="~/WebControls/ControleVigenciaDocentes/UCControleVigenciaDocentes.ascx"
    TagName="UCControleVigenciaDocentes" TagPrefix="uc12" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="summary" runat="server" ValidationGroup='<%=validationGroup %>' />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="pnlTurma" runat="server" GroupingText="Cadastro de turmas de eletivas">
        <asp:UpdatePanel ID="upnTurma" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:UCComboUAEscola ID="uccFiltroEscola" runat="server" CarregarEscolaAutomatico="true"
                    ObrigatorioEscola="true" ObrigatorioUA="true" MostrarMessageSelecioneEscola="true"
                    ValidationGroup='<%=validationGroup %>' PermiteAlterarCombos="false" />
                <uc2:UCComboCursoCurriculo ID="uccCursoCurriculo" runat="server" MostrarMessageSelecione="true"
                    ValidationGroup='<%=validationGroup %>' PermiteEditar="false" />
                <uc5:UCComboCalendario ID="uccCalendario" runat="server" ValidationGroup='<%=validationGroup %>'
                    MostrarMensagemSelecione="true" Obrigatorio="true" PermiteEditar="false" />
                <uc4:UCComboDisciplina ID="uccDisciplina" runat="server" MostrarMensagemSelecione="true"
                    ValidationGroup='<%=validationGroup %>' PermiteEditar="false" />
                <uc6:UCComboFormatoAvaliacao ID="uccFormatoAvaliacao" runat="server" _MostrarMessageSelecione="true"
                    CancelaSelect="true" Obrigatorio="true" ValidationGroup='<%=validationGroup %>' PermiteEditar="false" />
                <asp:Label ID="lblCodigoTurma" runat="server" Text="Código da turma *" AssociatedControlID="txtCodigoTurma">
                </asp:Label>
                <asp:TextBox ID="txtCodigoTurma" runat="server" MaxLength="20" SkinID="text30C" Enabled="false"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvCodigoTurma" ControlToValidate="txtCodigoTurma"
                    ValidationGroup='<%=validationGroup %>' runat="server" ErrorMessage="Código da turma é obrigatório.">*
                </asp:RequiredFieldValidator>
                <asp:Label ID="LabelCodigoInep" runat="server" Text="Código INEP" AssociatedControlID="txtCodigoInep"></asp:Label>
                <asp:TextBox ID="txtCodigoInep" runat="server" MaxLength="10" SkinID="text10C" Enabled="false"></asp:TextBox>
                <asp:Label ID="lblCapacidade" runat="server" Text="Capacidade *" AssociatedControlID="txtCapacidade"></asp:Label>
                <asp:TextBox ID="txtCapacidade" runat="server" SkinID="Numerico" MaxLength="9" Enabled="false"></asp:TextBox>
                <asp:CompareValidator ID="cvCapacidade" runat="server" ErrorMessage="Capacidade não pode ter valor zero."
                    ControlToValidate="txtCapacidade" Operator="GreaterThan" ValueToCompare="0" ValidationGroup='<%=validationGroup %>'
                    Display="Dynamic">*
                </asp:CompareValidator>
                <asp:RequiredFieldValidator ID="rfvCapacidade" ControlToValidate="txtCapacidade"
                    Display="Dynamic" ValidationGroup='<%=validationGroup %>' runat="server" ErrorMessage="Capacidade é obrigatório.">*</asp:RequiredFieldValidator>
                <asp:Label ID="Label1" runat="server" Text="Quantidade mínima de alunos matriculados"
                    AssociatedControlID="txtMinimoMatriculados"></asp:Label>
                <asp:TextBox ID="txtMinimoMatriculados" runat="server" SkinID="Numerico" MaxLength="9" Enabled="false"></asp:TextBox>
                <asp:CustomValidator ID="cvVagasCapacidade" runat="server" ValidationGroup='<%=validationGroup %>'
                    ControlToValidate="txtMinimoMatriculados" Text="*" ErrorMessage="Quantidade mínima de alunos matriculados deve ser menor ou igual a capacidade."
                    OnServerValidate="ValidarCapacidade_ServerValidate"></asp:CustomValidator>
                <asp:Label ID="Label2" runat="server" Text="Quantidade de aulas semanais *" AssociatedControlID="txtAulasSemanais"></asp:Label>
                <asp:TextBox ID="txtAulasSemanais" runat="server" SkinID="Numerico" MaxLength="3" Enabled="false"></asp:TextBox>
                <asp:CompareValidator ID="cvAulasSemanais" runat="server" ErrorMessage="Quantidade de aulas semanais não pode ter valor zero."
                    ControlToValidate="txtAulasSemanais" Operator="GreaterThan" ValueToCompare="0"
                    ValidationGroup='<%=validationGroup %>' Display="Dynamic">*
                </asp:CompareValidator>
                <asp:RequiredFieldValidator ID="rfvAulasSemanais" ControlToValidate="txtAulasSemanais"
                    Display="Dynamic" ValidationGroup='<%=validationGroup %>' runat="server" ErrorMessage="Quantidade de aulas semanais é obrigatório.">*
                </asp:RequiredFieldValidator>
                <div id="divPeriodosCurso" runat="server" visible="false">
                    <asp:Label ID="lblPeriodoCursos" runat="server" Text="Período do curso *" AssociatedControlID="chkPeriodosCurso">
                    </asp:Label>
                    <asp:Label ID="lblSemPeriodoCurso" runat="server" Visible="false">
                    </asp:Label>
                    <asp:CheckBoxList ID="chkPeriodosCurso" runat="server" DataTextField="crp_descricao"
                        DataValueField="crp_id" RepeatDirection="Horizontal" Enabled="false">
                    </asp:CheckBoxList>
                </div>
                <br />
                <div id="divPeriodosCalendario" class="divPeriodosCalendario" runat="server" visible="false"
                    title="Devem ser selecionados exatamente 2 períodos consecutivos do calendário."
                    style="display: inline-block;">
                    <asp:Label ID="Label3" runat="server" Text="Período do calendário *" AssociatedControlID="chkPeriodosCalendario">
                    </asp:Label>
                    <asp:Label ID="lblSemPeriodoCalendario" runat="server" Visible="false">
                    </asp:Label>
                    <asp:CheckBoxList ID="chkPeriodosCalendario" runat="server" DataTextField="tpc_nome"
                        DataValueField="tpc_id" RepeatDirection="Horizontal" Enabled="false">
                    </asp:CheckBoxList>
                </div>
                <div id="divDocente" runat="server">
                    <asp:Label ID="lblDocente" runat="server" Text="Docente *" AssociatedControlID="UCControleVigenciaDocentes"></asp:Label>
                    <uc12:UCControleVigenciaDocentes ID="UCControleVigenciaDocentes" runat="server" PermiteEditar="false" />
                </div>
                <uc7:UCComboTurno ID="uccTurno" runat="server" ValidationGroup='<%=validationGroup %>'
                    CancelSelect="false" Obrigatorio="true" MostrarPorTipoTurno="true" PermiteEditar="false" />
                <asp:CheckBox ID="chkRodizio" runat="server" Text="Turma que participa de rodízio" Enabled="false" />
                <asp:Label ID="lblSituacao" runat="server" Text="Situação *" AssociatedControlID="ddlSituacao"></asp:Label>
                <asp:DropDownList ID="ddlSituacao" runat="server" Enabled="false">
                    <asp:ListItem Value="0" Text="--Selecione uma situação--"></asp:ListItem>
                    <asp:ListItem Value="1" Text="Ativa"></asp:ListItem>
                    <asp:ListItem Value="5" Text="Encerrada"></asp:ListItem>
                </asp:DropDownList>
                <asp:CompareValidator ID="cvSituacao" runat="server" ErrorMessage="Situação é obrigatório."
                    ControlToValidate="ddlSituacao" Operator="GreaterThan" ValueToCompare="0" Display="Dynamic"
                    ValidationGroup='<%=validationGroup %>'>*</asp:CompareValidator>
            </ContentTemplate>
        </asp:UpdatePanel>
        <!-- Dialog confirmação padrão -->
        <uc15:UCConfirmacaoOperacao ID="UCConfirmacaoOperacao1" runat="server" />
        <div class="right">
            <asp:Button ID="btnCancelar" runat="server" Text="Voltar" OnClick="btnCancelar_Click"
                CausesValidation="false" />
        </div>
    </asp:Panel>
</asp:Content>
