<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Relatorios.RelatoriosCP.GraficoIndividualNotas.Busca" %>

<%@ Register Src="~/WebControls/Combos/Novos/UCCPeriodoCalendario.ascx" TagName="UCCPeriodoCalendario"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/BuscaDocente/UCBuscaDocenteTurma.ascx" TagName="UCBuscaDocenteTurma"
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/BuscaAluno/UCCamposBuscaAluno.ascx" TagName="UCCamposBuscaAluno"
    TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="updMensagem" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updPesquisa" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div>
                <fieldset id="fdsPesquisa" runat="server" style="margin-left: 10px;">
                    <legend id="lgdTitulo" runat="server"></legend>

                    <uc5:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" Visible="false" />
                    
                    <div class="divPesquisa">
                        <asp:Label ID="lblAvisoMensagem" runat="server"></asp:Label>
                        
                        <!-- FiltrosPadrao -->
                        <uc2:UCBuscaDocenteTurma runat="server" ID="UCBuscaDocenteTurma" Visible="false" />

                        <div id="divPeriodoCalendario" runat="server" visible="false">
                            <uc1:UCCPeriodoCalendario runat="server" ID="UCCPeriodoCalendario" MostrarMensagemSelecione="true" Obrigatorio="false" PermiteEditar="false" />
                        </div>

                        <uc3:UCCamposBuscaAluno ID="UCCamposBuscaAluno1" runat="server" />

                        <div class="clear"></div>
                    </div>

                    <div class="right">
                        <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click" />
                    </div>

                </fieldset>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnPesquisar" />
        </Triggers>
    </asp:UpdatePanel>
    <div class="clear">
    </div>
    <asp:UpdatePanel ID="updResultado" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset id="fdsResultados" runat="server">
                <legend>Resultados</legend>
                <div class="right area-botoes-top">
                    <asp:Button ID="btnGerarRelatorioCima" runat="server" Text="<%$ Resources:Relatorios, RelatoriosCP.GraficoIndividualNotas.Busca.btnGerarRel.Text %>" OnClick="btnGerarRelatorio_Click" />
                </div>
                <div class="area-form">
                    <br />
                    <br />
                    <div>
                        <div style="float: left; width: 50%">
                            <asp:CheckBox ID="chkTodos" SkinID="chkTodos" Text="Selecionar todos os alunos" todososcursospeja='0'
                                runat="server" />
                        </div>
                        <div align="right" id="divQtdPaginacao" runat="server">
                            <asp:Label ID="lblPag" runat="server" Text="Itens por página"></asp:Label>
                            <asp:DropDownList ID="ddlQtPaginado" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlQtPaginado_SelectedIndexChanged">
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>20</asp:ListItem>
                                <asp:ListItem>50</asp:ListItem>
                                <asp:ListItem>100</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <br />
                    <asp:GridView ID="grvDocumentoAluno" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        BorderStyle="None" DataKeyNames="alu_id,tur_id,cal_id,esc_id,mtu_id,EscolaUniDestino,GrupamentoDestino,pes_nome,tur_escolaUnidade"
                        EmptyDataText="A pesquisa não encontrou resultados." AllowCustomPaging="true"
                        OnRowDataBound="grvDocumentoAluno_RowDataBound" OnDataBound="grvDocumentoAluno_DataBound"
                        AllowSorting="True" OnPageIndexChanging="grvDocumentoAluno_PageIndexChanging" OnSorting="grvDocumentoAluno_Sorting"
                        EnableModelValidation="True" SkinID="GridResponsive">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelecionar" runat="server" alu_id='<%# Eval("alu_id") %>' cal_id='<%# Eval("cal_id") %>'
                                        tur_id='<%# Eval("tur_id") %>' esc_id='<%# Eval("esc_id") %>' cursopeja='<%# Eval("CursoPeja") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Matricula estadual" SortExpression="alc_matriculaEstadual">
                                <ItemTemplate>
                                    <asp:Label ID="lblMatriculaEstadual" runat="server" Text='<%# Bind("alc_matriculaEstadual") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Mensagens,MSG_NUMEROMATRICULA %>" SortExpression="alc_matricula">
                                <ItemTemplate>
                                    <asp:Label ID="lblMatricula" runat="server" Text='<%# Bind("alc_matricula") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nome" SortExpression="pes_nome">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtNome" runat="server" Text='<%# Bind("pes_nome") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAlterar" runat="server" Text='<%# Bind("pes_nome") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Escola" SortExpression="tur_escolaUnidade" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblEscola" runat="server" Text='<%# Bind("tur_escolaUnidade") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="tur_codigo" HeaderText="Turma" SortExpression="tur_codigo" />
                            <asp:TemplateField HeaderText="Curso" SortExpression="tur_curso" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblCurso" runat="server" Text='<%# Bind("tur_curso") %>' CssClass="wrap150px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Calendário" SortExpression="tur_calendario" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblCalendario" runat="server" Text='<%# Bind("tur_calendario") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="alu_situacao" HeaderText="Situação" SortExpression="alu_situacao" />

                        </Columns>
                    </asp:GridView>
                    <uc4:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="_grvDocumentoAluno"
                        class="clTotalReg" />
                </div>
                <div class="right">
                    <asp:Button ID="btnGerarRelatorio" runat="server" Text="<%$ Resources:Relatorios, RelatoriosCP.GraficoIndividualNotas.Busca.btnGerarRel.Text %>" OnClick="btnGerarRelatorio_Click" />
                </div>
            </fieldset>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnGerarRelatorio" />
            <asp:AsyncPostBackTrigger ControlID="btnGerarRelatorioCima" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

