<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Documentos.DocumentoDocente.Busca" %>

<%@ Register Src="~/WebControls/BuscaDocente/UCBuscaDocenteTurma.ascx" TagPrefix="uc1" TagName="UCBuscaDocenteTurma" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios" TagPrefix="uc9" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCTurmaDisciplina.ascx" TagPrefix="uc2" TagName="UCComboTurmaDisciplina" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCPeriodoCalendario.ascx" TagPrefix="uc1" TagName="UCCPeriodoCalendario" %>
<%@ Register Src="~/WebControls/BuscaAluno/UCCamposBuscaAluno.ascx" TagPrefix="uc1" TagName="UCCamposBuscaAluno" %>
<%@ Register Src="~/WebControls/Combos/UCComboAtividadeAvaliativa.ascx" TagPrefix="uc1" TagName="UCComboAtividadeAvaliativa" %>
<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc8" %>
<%@ Register Src="~/WebControls/Busca/UCAluno.ascx" TagName="UCAluno" TagPrefix="uc10" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMensagem" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div style="width: 40%; float: left; clear: none;" class="area-selecao-documento">
        <fieldset>
            <legend>Relatórios do docente</legend>
            <div id="divRelatorio" class="divRelatorio" runat="server">
                <asp:RadioButtonList ID="rdbRelatorios" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rdbRelatorios_OnSelectedIndexChanged"
                    CausesValidation="false">
                </asp:RadioButtonList>
            </div>
        </fieldset>
    </div>
    <div style="width: 60%; float: left; clear: none;" class="area-parametros-documento">
    <asp:UpdatePanel ID="uppPesquisa" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            
                <fieldset id="fdsDocumentosEscola" runat="server" style="margin-left: 10px;">
                    <legend>Parâmetros de busca</legend>
                    <uc9:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" />
                    <div id="divPesquisa" class="divPesquisa area-form" runat="server">
                        <asp:Label ID="lblAvisoMensagem" runat="server"></asp:Label>
                        <asp:CheckBox ID="ChkEmitirAnterior" runat="server" Text="Emitir relatórios de anos anteriores" AutoPostBack="true"
                            OnCheckedChanged="ChkEmitirAnterior_CheckedChanged" Visible="false" />
                        <uc1:UCBuscaDocenteTurma runat="server" ID="UCBuscaDocenteTurma" Visible="false" />
                        <div runat="server" id="divPeriodoCalendario" visible="false">
                            <uc1:UCCPeriodoCalendario runat="server" ID="UCCPeriodoCalendario" MostrarMensagemSelecione="true" Obrigatorio="false" PermiteEditar="false" />
                        </div>
                        <div runat="server" id="divTurmaDisciplina" visible="false">
                            <uc2:UCComboTurmaDisciplina runat="server" ID="UCComboTurmaDisciplina" 
                                Obrigatorio="true" PermiteEditar="false" MostrarMensagemSelecione="true"
                                VS_MostraFilhosRegencia="false" />
                        </div>
                        <div runat="server" id="divAtividadeAvaliativa" visible="false">
                            <uc1:UCComboAtividadeAvaliativa runat="server" id="UCComboAtividadeAvaliativa" MostrarMessageSelecione="true" Obrigatorio="true" PermiteEditar="false" />
                        </div>
                        <div runat="server" id="divBuscaAluno" visible="false">
                            <uc1:UCCamposBuscaAluno runat="server" ID="UCCamposBuscaAluno" />
                        </div>
                        <div runat="server" id="divData" visible="false">
                            <div style="display: inline-block">
                                <asp:Label ID="lblDataInicio" runat="server" Text="Início da vigência" AssociatedControlID="txtDataInicio"></asp:Label>
                                <asp:TextBox runat="server" ID="txtDataInicio" SkinID="Data"></asp:TextBox>
                                <asp:CustomValidator ID="cvDataInicio" runat="server" ControlToValidate="txtDataInicio"
                                    Display="Dynamic" ErrorMessage="Data de início da vigência não está no formato dd/mm/aaaa ou é inexistente."
                                    OnServerValidate="ValidarData_ServerValidate">*</asp:CustomValidator>
                            </div>
                            <div style="display: inline-block">
                                <asp:Label ID="lblDataFim" runat="server" Text="Fim da vigência" AssociatedControlID="txtDataFim"></asp:Label>
                                <asp:TextBox runat="server" ID="txtDataFim" SkinID="Data"></asp:TextBox>
                                <asp:CustomValidator ID="cvDataFim" runat="server" ControlToValidate="txtDataFim"
                                    Display="Dynamic" ErrorMessage="Data de fim da vigência não está no formato dd/mm/aaaa ou é inexistente."
                                    OnServerValidate="ValidarData_ServerValidate">*</asp:CustomValidator>
                                <asp:CustomValidator ID="cvDatas" runat="server" Display="Dynamic"
                                    ErrorMessage="Data de fim da vigência deve ser maior ou igual a data de início da vigência."
                                    OnServerValidate="ValidarDataDocumento_ServerValidate">*</asp:CustomValidator>
                            </div>
                        </div>
                    </div>
                    <div class="right area-botoes-bottom">
                        <asp:Button ID="btnGerar" runat="server" Text="<%$ Resources:Documentos, DocumentosDocente.Busca.btnGerar.Text %>" OnClick="btnGerar_Click" CausesValidation="true" />
                        <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" Visible="false"  OnClick="btnPesquisar_Click" />
                    </div>
                </fieldset>
            
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnPesquisar" />
        </Triggers>
    </asp:UpdatePanel>
    </div>
        <div class="clear">
    </div>
    <asp:UpdatePanel ID="_updResultado" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset id="fdsResultados" runat="server" visible="false">
                <legend>Resultados</legend>
                <br />
                <div class="right area-botoes-bottom">
                    <asp:Button ID="btnGerarRelatorioCima" runat="server" Text="<%$ Resources:Documentos, DocumentosDocente.Busca.btnGerarRelatorioCima.Text %>" OnClick="btnGerar_Click" CausesValidation="true"/>
                </div>
                <br />
                <br />
                <div id="DivSelecionaTodos" runat="server">
                    <div style="float: left; width: 50%">
                        <asp:CheckBox ID="_chkTodos" SkinID="chkTodos" Text="Selecionar todos os alunos" todososcursospeja='0'
                            runat="server" OnCheckedChanged="_chkTodos_CheckedChanged" AutoPostBack="true"/>
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
                    BorderStyle="None" DataKeyNames="alu_id" EmptyDataText="A pesquisa não encontrou resultados."
                     OnDataBound="_grvDocumentoAluno_DataBound" OnSorting="_grvDocumentoAluno_Sorting"
                    AllowSorting="True" OnPageIndexChanging="grvDocumentoAluno_PageIndexChanging" 
                    EnableModelValidation="True" OnRowDataBound="_grvDocumentoAluno_RowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="_chkSelecionar" runat="server" AutoPostBack="true" OnCheckedChanged="_chkSelecionar_CheckedChanged"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Matricula estadual" SortExpression="alc_matriculaEstadual">
                            <ItemTemplate>
                                <asp:Label ID="lblMatriculaEstadual" runat="server" Text='<%# Bind("alc_matriculaEstadual") %>'
                                    CssClass="wrap100px"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Mensagens,MSG_NUMEROMATRICULA %>" SortExpression="alc_matricula">
                            <ItemTemplate>
                                <asp:Label ID="lblMatricula" runat="server" Text='<%# Bind("alc_matricula") %>' CssClass="wrap100px"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nome" SortExpression="pes_nome">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("pes_nome") %>' CssClass="wrap150px"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="_lblAlterar" runat="server" Text='<%# Bind("pes_nome") %>' CssClass="wrap150px"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Escola" SortExpression="tur_escolaUnidade">
                            <ItemTemplate>
                                <asp:Label ID="lblEscola" runat="server" Text='<%# Bind("tur_escolaUnidade") %>'
                                    CssClass="wrap150px"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="tur_codigo" HeaderText="Turma" SortExpression="tur_codigo" />
                        <asp:TemplateField HeaderText="Curso" SortExpression="tur_curso">
                            <ItemTemplate>
                                <asp:Label ID="lblCurso" runat="server" Text='<%# Bind("tur_curso") %>' CssClass="wrap150px"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Calendário" SortExpression="tur_calendario">
                            <ItemTemplate>
                                <asp:Label ID="lblCalendario" runat="server" Text='<%# Bind("tur_calendario") %>'
                                    CssClass="wrap100px"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="alu_situacao" HeaderText="Situação" SortExpression="alu_situacao" Visible="false" />
                    </Columns>
                </asp:GridView>
                <uc8:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="_grvDocumentoAluno"
                    class="clTotalReg" />
                <div class="right area-botoes-bottom">
                    <asp:Button ID="_btnGerarRelatorio" runat="server" Text="<%$ Resources:Documentos, DocumentosDocente.Busca._btnGerarRelatorio.Text %>" OnClick="btnGerar_Click" CausesValidation="true"/>
                </div>
            </fieldset>
        </ContentTemplate>
        <Triggers>
<%--            <asp:AsyncPostBackTrigger ControlID="btnPesquisarCertificadoHabilitacao" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="_btnGerarRelatorio" />
            <asp:AsyncPostBackTrigger ControlID="btnGerarRelatorioCima" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
