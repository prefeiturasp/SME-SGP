<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Academico_Turma_Cadastro" CodeBehind="Cadastro.aspx.cs" %>

<%@ PreviousPageType VirtualPath="~/Turma/Turma/Busca.aspx" %>
<%@ Register Src="~/WebControls/Combos/UCComboCalendario.ascx" TagName="_UCComboCalendario"
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboCursoCurriculo.ascx" TagName="_UCComboCursoCurriculo"
    TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/UCComboTurno.ascx" TagName="_UCComboTurno"
    TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Combos/UCComboCurriculoPeriodo.ascx" TagName="_UCComboCurriculoPeriodo"
    TagPrefix="uc5" %>
<%@ Register Src="../../WebControls/Combos/UCComboFormatoAvaliacao.ascx" TagName="UCComboFormatoAvaliacao"
    TagPrefix="uc11" %>
<%@ Register Src="../../WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc36" %>
<%@ Register Src="~/WebControls/TurmaDisciplina/UCGridDisciplina.ascx" TagName="UCGridDisciplina"
    TagPrefix="uc13" %>
<%@ Register Src="~/WebControls/TurmaDisciplina/UCRepeaterDisciplina.ascx" TagName="UCRepeaterDisciplina"
    TagPrefix="uc14" %>
<%@ Register Src="../../WebControls/Combos/UCComboUAEscola.ascx" TagName="UCComboUAEscola"
    TagPrefix="uc6" %>
<%@ Register Src="~/WebControls/ControleVigenciaDocentes/UCControleVigenciaDocentes.ascx"
    TagName="UCControleVigenciaDocentes" TagPrefix="uc8" %>
<%@ Register Src="~/WebControls/TurmaDisciplina/UCGridDisciplinaRegencia.ascx" TagPrefix="uc11"
    TagName="UCGridDisciplinaRegencia" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao" 
    TagPrefix="uc9" %>
<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros" 
    TagPrefix="uc10" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="uppMessage" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
    <div id="divTabs">
        <ul class="hide">
            <li><a href="#divTabs-1">Dados da turma</a></li>
            <li><a href="#divTabs-2">
                <asp:Literal runat="server" ID="litDisciplinas" Text="<%$ Resources:Mensagens, MSG_DISCIPLINA_PLURAL %>"></asp:Literal></a>
            </li>
            <li><a href="#divTabs-3" runat="server" id="tabDocenciaCompartilhada">
                <asp:Literal runat="server" ID="litDocenciaCompartilhada" Text="<%$ Resources:Turma, Turma.Cadastro.litDocenciaCompartilhada.Text %>"></asp:Literal></a>
            </li>
        </ul>
        <div id="divTabs-1">
            <asp:UpdatePanel ID="uppDadosTurma" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <fieldset id="fdsTurma" runat="server">
                        <uc36:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" />
                        <uc6:UCComboUAEscola ID="uccUAEscola" runat="server" ObrigatorioUA="true" ObrigatorioEscola="true"
                            CarregarEscolaAutomatico="true" FiltroEscolasControladas="true" MostrarMessageSelecioneUA="true"
                            MostrarMessageSelecioneEscola="true" OnIndexChangedUA="uccUAEscola_IndexChangedUnidadeEscola"
                            OnIndexChangedUnidadeEscola="uccUAEscola_IndexChangedUnidadeEscola" PermiteAlterarCombos="false" />
                        <uc3:_UCComboCursoCurriculo ID="uccCursoCurriculo" runat="server" MostrarMessageSelecione="true" PermiteEditar="false" />
                        <uc5:_UCComboCurriculoPeriodo ID="uccCurriculoPeriodo" runat="server" Obrigatorio="true"
                            _MostrarMessageSelecione="true" PermiteEditar="false" />
                        <uc2:_UCComboCalendario ID="uccCalendario" runat="server" MostrarMensagemSelecione="true" PermiteEditar="false" />
                        <uc11:UCComboFormatoAvaliacao ID="uccFormatoAvaliacao" runat="server" _MostrarMessageSelecione="true"
                            OnIndexChanged="uccFormatoAvaliacao_IndexChanged" PermiteEditar="false" />
                        <br />
                        <div id="divAvaliacao" class="divAvaliacao" runat="server" visible="false" title="Selecione uma ou mais avaliações consecutivas."
                            style="display: inline-block;">
                            <asp:Label ID="LabelAvaliacao" runat="server" Text="Avaliação *" AssociatedControlID="chkAvaliacao">
                            </asp:Label>
                            <asp:CheckBoxList ID="chkAvaliacao" runat="server" DataTextField="crp_nomeAvaliacao"
                                DataValueField="tca_id_numeroAvaliacao" RepeatDirection="Horizontal" Enabled="false">
                            </asp:CheckBoxList>
                        </div>
                        <asp:Label ID="lblCodigoTurma" runat="server" Text="Código da turma *  " AssociatedControlID="txtCodigoTurma"></asp:Label>
                        <asp:TextBox ID="txtCodigoTurma" runat="server" MaxLength="20" SkinID="text30C" Enabled="false"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvCodigoTurma" ControlToValidate="txtCodigoTurma"
                            runat="server" ErrorMessage="Código da turma é obrigatório.">*</asp:RequiredFieldValidator>
                        <asp:Label ID="LabelCodigoInep" runat="server" Text="Código INEP" AssociatedControlID="txtCodigoInep"></asp:Label>
                        <asp:TextBox ID="txtCodigoInep" runat="server" MaxLength="10" SkinID="text10C" Enabled="false"></asp:TextBox>
                        <asp:Label ID="lblCapacidade" runat="server" Text="Capacidade *" AssociatedControlID="txtCapacidade"></asp:Label>
                        <asp:TextBox ID="txtCapacidade" runat="server" SkinID="Numerico" MaxLength="9" Enabled="false"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvCapacidade" ControlToValidate="txtCapacidade"
                            runat="server" ErrorMessage="Capacidade é obrigatório.">*</asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cvCapacidade" runat="server" ControlToValidate="txtCapacidade"
                            Operator="DataTypeCheck" Text="*" Type="Integer" ErrorMessage="Capacidade não pode ser maior do que 999.999.999."></asp:CompareValidator>
                        <asp:Label ID="Label1" runat="server" Text="Quantidade mínima de alunos matriculados"
                            AssociatedControlID="txtMinimoMatriculados"></asp:Label>
                        <asp:TextBox ID="txtMinimoMatriculados" runat="server" SkinID="Numerico" MaxLength="9" Enabled="false"></asp:TextBox>
                        <asp:CompareValidator ID="cvMinimoMatriculados" runat="server" ControlToValidate="txtMinimoMatriculados"
                            Operator="DataTypeCheck" Text="*" Type="Integer" ErrorMessage="Quantidade mínima de alunos matriculados não pode ser maior do que 999.999.999."></asp:CompareValidator>
                        <asp:CustomValidator ID="cvVagasCapacidade" runat="server" ControlToValidate="txtMinimoMatriculados"
                            Text="*" ErrorMessage="Quantidade mínima de alunos matriculados deve ser menor ou igual a capacidade."
                            OnServerValidate="ValidarCapacidade_ServerValidate"></asp:CustomValidator>
                        <asp:Label ID="lblDuracao" runat="server" Text="Duração *" AssociatedControlID="ddlDuracao"></asp:Label>
                        <asp:DropDownList ID="ddlDuracao" runat="server" Enabled="false">
                            <asp:ListItem Value="0" Text="-- Selecione uma duração --"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Anual"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Semestral"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Condensada"></asp:ListItem>
                            <asp:ListItem Value="4" Text="Livre"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:CompareValidator ID="cvDuracao" runat="server" ErrorMessage="Duração é obrigatório."
                            ControlToValidate="ddlDuracao" Operator="GreaterThan" ValueToCompare="0" Display="Dynamic">*</asp:CompareValidator>
                        <uc4:_UCComboTurno ID="uccTurno" runat="server" MostrarPorTipoTurno="true" PermiteEditar="false" />
                        <asp:CheckBox ID="chkProfessorEspecialista" runat="server" Text="Turma de docente especialista"
                            OnCheckedChanged="chkProfessorEspecialista_CheckedChanged" AutoPostBack="true" Enabled="false" />
                        <asp:CheckBox ID="chkRodizio" runat="server" Text="Turma que participa de rodízio" Enabled="false" />
                        <asp:Label ID="lblSituacao" runat="server" Text="Situação *" AssociatedControlID="ddlSituacao"></asp:Label>
                        <asp:DropDownList ID="ddlSituacao" runat="server" OnSelectedIndexChanged="ddlSituacao_SelectedIndexChanged" AutoPostBack="true" Enabled="false">
                            <asp:ListItem Value="0" Text="--Selecione uma situação--"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Ativa"></asp:ListItem>
                            <asp:ListItem Value="5" Text="Encerrada"></asp:ListItem>
                            <asp:ListItem Value="7" Text="Extinta"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:CompareValidator ID="cvSituacao" runat="server" ErrorMessage="Situação é obrigatório."
                            ControlToValidate="ddlSituacao" Operator="GreaterThan" ValueToCompare="0" Display="Dynamic">*</asp:CompareValidator>
                        <div id="divDataEncerramento" runat="server">
                            <asp:Label ID="lblDataEncerramento" runat="server" Text="Data de encerramento" AssociatedControlID="txtDataEncerramento"></asp:Label>
                            <asp:TextBox ID="txtDataEncerramento" runat="server" SkinID="Data" Enabled="false"></asp:TextBox>                            
                            <asp:CustomValidator ID="cvDataEncerramento" runat="server" ControlToValidate="txtDataEncerramento" ErrorMessage="Data de encerramento não está no formato dd/mm/aaaa ou é inexistente." Display="Dynamic" OnServerValidate="ValidarData_ServerValidate">*</asp:CustomValidator>
                        </div>                        
                        <asp:Label ID="lblObservacao" runat="server" Text="Observação" AssociatedControlID="txtObservacao"></asp:Label>
                        <asp:TextBox ID="txtObservacao" runat="server" TextMode="MultiLine" Enabled="false"></asp:TextBox>

                    </fieldset>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="chkProfessorEspecialista" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div id="divTabs-2" class="hide">
            <asp:UpdatePanel ID="upnDisciplinas" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <fieldset id="fdsDisciplinas" runat="server" visible="false">
                        <!-- Mensagem de Aviso ao Docente -->
                        <asp:Label ID="lblMensagemAvisoDocente" runat="server" Text="" Visible="true"></asp:Label>
                        <uc36:UCCamposObrigatorios ID="UCCamposObrigatorios2" runat="server" />
                        <uc11:UCGridDisciplinaRegencia ID="UCGridDisciplinaRegencia" nomeTipoDisciplina="Regência"
                            runat="server" AssociatedUpdatePanelID="upnDisciplinas" Visible="false" />
                        <uc13:UCGridDisciplina ID="ucDiscComplementoRegencia" nomeTipoDisciplina="<%$ Resources:Turma, Turma.Cadastro.ucDiscComplementoRegencia.nomeTipoDisciplina %>"
                            runat="server" AssociatedUpdatePanelID="upnDisciplinas" Visible="false" />
                        <uc13:UCGridDisciplina ID="ucDiscMultisseriadaAluno" nomeTipoDisciplina="Componentes curriculares multisseriadas do aluno"
                            runat="server" AssociatedUpdatePanelID="upnDisciplinas" Visible="false" />
                        <uc13:UCGridDisciplina ID="ucDiscPrincipal" nomeTipoDisciplina="<%$ Resources:Turma, Turma.Cadastro.ucDiscPrincipal.nomeTipoDisciplina %>"
                            runat="server" AssociatedUpdatePanelID="upnDisciplinas" Visible="false" />
                        <uc13:UCGridDisciplina ID="ucDiscObrigatoria" nomeTipoDisciplina="<%$ Resources:Turma, Turma.Cadastro.ucDiscObrigatoria.nomeTipoDisciplina %>"
                            runat="server" AssociatedUpdatePanelID="upnDisciplinas" Visible="false" />
                        <uc14:UCRepeaterDisciplina ID="ucDiscOptativa" runat="server" nomeTipoDisciplina="<%$ Resources:Turma, Turma.Cadastro.ucDiscOptativa.nomeTipoDisciplina %>"
                            AssociatedUpdatePanelID="upnDisciplinas" Visible="false" />
                        <uc13:UCGridDisciplina ID="ucDiscDocenteTurmaObrigatoria" nomeTipoDisciplina="<%$ Resources:Turma, Turma.Cadastro.ucDiscDocenteTurmaObrigatoria.nomeTipoDisciplina %>"
                            runat="server" AssociatedUpdatePanelID="upnDisciplinas" Visible="false" />
                        <uc13:UCGridDisciplina ID="ucDiscDocenciaCompartilhada" nomeTipoDisciplina="<%$ Resources:Turma, Turma.Cadastro.ucDiscDocenciaCompartilhada.nomeTipoDisciplina %>"
                            runat="server" AssociatedUpdatePanelID="upnDisciplinas" Visible="false" />
                        <uc14:UCRepeaterDisciplina ID="ucDiscDisponibilidadeProfObrigatoria" nomeTipoDisciplina="<%$ Resources:Turma, Turma.Cadastro.ucDiscDisponibilidadeProfObrigatoria.nomeTipoDisciplina %>"
                            runat="server" AssociatedUpdatePanelID="upnDisciplinas" Visible="false" />
                        <uc14:UCRepeaterDisciplina ID="ucDiscEletiva" runat="server" AssociatedUpdatePanelID="upnDisciplinas"
                            nomeTipoDisciplina="<%$ Resources:Turma, Turma.Cadastro.ucDiscEletiva.nomeTipoDisciplina %>"
                            Visible="false" />
                        <uc14:UCRepeaterDisciplina ID="ucDiscDocenteTurmaEletiva" runat="server" nomeTipoDisciplina="<%$ Resources:Turma, Turma.Cadastro.ucDiscDocenteTurmaEletiva.nomeTipoDisciplina %>"
                            AssociatedUpdatePanelID="upnDisciplinas" Visible="false" />
                        <uc14:UCRepeaterDisciplina ID="ucDiscDisponibilidadeProfEletiva" nomeTipoDisciplina="<%$ Resources:Turma, Turma.Cadastro.ucDiscDisponibilidadeProfEletiva.nomeTipoDisciplina %>"
                            runat="server" AssociatedUpdatePanelID="upnDisciplinas" Visible="false" />
                        <!-- Mensagem de Atualização de Dados -->
                        <div align="right">
                            <asp:Label ID="lblMensagemAtualizacaoDados" runat="server" Text="" Visible="true"></asp:Label>
                        </div>
                    </fieldset>
                    <fieldset id="fdsSemDisciplinas" runat="server">
                        <asp:Label ID="lblSemDisciplinas" runat="server"></asp:Label>
                    </fieldset>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="divTabs-3" class="hide">
            <asp:UpdatePanel ID="upnDocenciaCompartilhada" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <fieldset id="fdsDocenciaCompartilhada" runat="server">
                        <asp:Label ID="lblMensagemSemDocenciaCompartilhada" runat="server" EnableViewState="True"></asp:Label>
                        <asp:Label ID="lblDisciplinasDocenciaCompartilhada" runat="server" Text="<%$ Resources:Mensagens, MSG_DISCIPLINA %>" AssociatedControlID="ddlDisciplinasDocenciaCompartilhada"></asp:Label>
                        <asp:DropDownList ID="ddlDisciplinasDocenciaCompartilhada" runat="server" OnSelectedIndexChanged="ddlDisciplinasDocenciaCompartilhada_SelectedIndexChanged" AutoPostBack="true" Enabled="false"></asp:DropDownList>
                        <br /><br />
                        <fieldset id="fdsDisciplinasDocenciaCompartilhada" runat="server">
                            <legend>
                                <asp:Literal runat="server" ID="litDisciplinasTurma" Text="<%$ Resources:Turma, Turma.Cadastro.litDocenciaCompartilhada.Text %>"></asp:Literal>
                            </legend>
                            <asp:GridView ID="_dgvTurma" runat="server" AutoGenerateColumns="False" AllowPaging="false"
                                DataKeyNames="tud_id,tud_tipo,tdr_id,relacionada" 
                                EmptyDataText="A pesquisa não encontrou resultados."
                                OnRowDataBound="_dgvTurma_RowDataBound" AllowSorting="false" >
                                <Columns>
                                    <asp:BoundField HeaderText="<%$ Resources:Mensagens, MSG_DISCIPLINA %>" DataField="tud_nome" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:TemplateField HeaderText="Docência Compartilhada">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="_chkCompartilhada" runat="server" Enabled="false"></asp:CheckBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="center"></ItemStyle>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:GridView>
                            <asp:Label id="_lblMsgRegencia" runat="server" Text="" Visible="false"></asp:Label>
                            <div align="right" class="divBtnCadastro">
                                <asp:Button ID="btnVisualizarHistorico" runat="server" Text="<%$ Resources:Turma, Turma.Cadastro.btnVisualizarHistorico.Text %>" OnClick="btnVisualizarHistorico_Click" CausesValidation="false" />
                            </div>
                        </fieldset>
                    </fieldset>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <fieldset>
            <div align="right" class="divBtnCadastro">
                <asp:Button ID="btnCancelar" runat="server" Text="Voltar" OnClick="btnCancelar_Click"
                    CausesValidation="false" />
            </div>
        </fieldset>
    </div>

    <div title="<%$ Resources:Turma, Turma.Cadastro.divHistoricoDocenciaCompartilhada.title %>" class="divHistoricoDocenciaCompartilhada" runat="server">
        <asp:UpdatePanel ID="upnHistoricoDocenciaCompartilhada" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset id="fdsHistorico" runat="server">
                    <legend>
                        <asp:Literal runat="server" ID="litHistoricoDocenciaCompartilhada" Text=""></asp:Literal>
                    </legend>
                    <uc9:UCComboQtdePaginacao ID="UCComboQtdePaginacaoHistorico" runat="server" OnIndexChanged="UCComboQtdePaginacaoHistorico_IndexChanged" />
                    <asp:GridView ID="gvHistorico" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                        EmptyDataText="A pesquisa não encontrou resultados." OnDataBound="gvHistorico_DataBound">
                        <Columns>
                            <asp:BoundField HeaderText="<%$ Resources:Mensagens, MSG_DISCIPLINA %>" DataField="tud_nome" HeaderStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="<%$ Resources:Turma, Turma.Cadastro.gvHistorico.vigenciaInicio %>" DataField="tdr_vigenciaInicio" 
                                HeaderStyle-CssClass="center" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField HeaderText="<%$ Resources:Turma, Turma.Cadastro.gvHistorico.vigenciaFim %>" DataField="tdr_vigenciaFim" 
                                HeaderStyle-CssClass="center" ItemStyle-HorizontalAlign="Center" />
                        </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource ID="odsHistorico" runat="server" OldValuesParameterFormatString="original_{0}"
                        SelectMethod="SelecionarHistoricoPorDisciplinaCompartilhada" TypeName="MSTech.GestaoEscolar.BLL.TUR_TurmaDisciplinaRelacionadaBO"></asp:ObjectDataSource>
                    <uc10:UCTotalRegistros ID="UCTotalRegistrosHistorico" runat="server" AssociatedGridViewID="gvHistorico" />
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <input id="txtSelectedTab" type="hidden" runat="server" />
</asp:Content>
