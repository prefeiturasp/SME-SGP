<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Turma.TurmaMultisseriada.Cadastro" %>

<%@ PreviousPageType VirtualPath="~/Turma/TurmaMultisseriada/Busca.aspx" %>
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
<%@ Register Src="~/WebControls/Combos/UCComboCurriculoPeriodo.ascx" TagName="UCComboCurriculoPeriodo"
    TagPrefix="uc8" %>
<%@ Register Src="~/WebControls/Combos/UCComboDocente.ascx" TagName="UCComboDocente"
    TagPrefix="uc9" %>
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
    <asp:Panel ID="pnlTurma" runat="server" GroupingText="Cadastro de turmas multisseriadas">
        <asp:UpdatePanel ID="upnTurma" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:UCComboUAEscola ID="uccFiltroEscola" runat="server" CarregarEscolaAutomatico="true"
                    ObrigatorioEscola="true" ObrigatorioUA="true" MostrarMessageSelecioneEscola="true"
                    ValidationGroup='<%=validationGroup %>' PermiteAlterarCombos="false" />
                <uc5:UCComboCalendario ID="uccCalendario" runat="server" ValidationGroup='<%=validationGroup %>'
                    MostrarMensagemSelecione="true" Obrigatorio="true" PermiteEditar="false" />
                
                <div id="divDiciplinas" runat="server" visible="false">
                <br /><br />
                <fieldset>
                <legend><asp:Label ID="lblLegendDisciplinas" runat="server"></asp:Label></legend>
                <asp:UpdatePanel ID="updDiciplinas" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblMessageDisciplina" runat="server" EnableViewState="False"></asp:Label>
                        <asp:ValidationSummary ID="summaryDisciplina" runat="server" />
                        <asp:GridView ID="grvDiciplinas" runat="server" AutoGenerateColumns="False" DataKeyNames="IsNew"
                            OnRowDataBound="grvDiciplinas_RowDataBound" OnSelectedIndexChanging="grvDiciplinas_SelectedIndexChanging">
                            <Columns> 
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCursoCurriculo" runat="server" Text='<%#Bind("cur_nome") %>' />
                                        <uc2:UCComboCursoCurriculo ID="uccCursoCurriculo" runat="server" MostrarMessageSelecione="true"
                                             SkinIDCombo="text30C" PermiteEditar="false"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCurriculoPeriodo" runat="server" Text='<%#Bind("crp_descricao") %>' />
                                        <uc8:UCComboCurriculoPeriodo ID="uccCurriculoPeriodo" runat="server" MostrarMessageSelecione="true"
                                             PermiteEditar="false" SkinIDCombo="text30C" CancelSelect="true"/> 
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDisciplina" runat="server" Text='<%#Bind("dis_nome") %>' />
                                            <uc4:UCComboDisciplina ID="uccDisciplina" runat="server" MostrarMensagemSelecione="true"
                                             PermiteEditar="false" SkinIDCombo="text30C" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </fieldset>
            </div>
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
                <asp:Label ID="lblAulasSemanais" runat="server" Text="Quantidade de aulas semanais *" AssociatedControlID="txtAulasSemanais"></asp:Label>
                <asp:TextBox ID="txtAulasSemanais" runat="server" SkinID="Numerico" MaxLength="3" Enabled="false"></asp:TextBox>
                <asp:CompareValidator ID="cvAulasSemanais" runat="server" ErrorMessage="Quantidade de aulas semanais não pode ter valor zero."
                    ControlToValidate="txtAulasSemanais" Operator="GreaterThan" ValueToCompare="0"
                    ValidationGroup='<%=validationGroup %>' Display="Dynamic">*
                </asp:CompareValidator>
                <asp:RequiredFieldValidator ID="rfvAulasSemanais" ControlToValidate="txtAulasSemanais"
                    Display="Dynamic" ValidationGroup='<%=validationGroup %>' runat="server" ErrorMessage="Quantidade de aulas semanais é obrigatório.">*
                </asp:RequiredFieldValidator>
                <br /><br />
                <div id="divDocente" runat="server" style="padding-bottom: 30px;">
                    <asp:Label ID="lblDocente" runat="server" Text="Docente *"></asp:Label><br />
                    <asp:Repeater ID="rptDocentes" runat="server" OnItemDataBound="rptDocentes_ItemDataBound">
                        <ItemTemplate>
                            <div style="width:60%;">
                                <asp:Label ID="lblposicao" runat="server" Text='<%#Bind("posicao") %>' Visible="false"></asp:Label>
                                <uc12:UCControleVigenciaDocentes ID="UCControleVigenciaDocentes" runat="server" PermiteEditar="false" />
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
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
