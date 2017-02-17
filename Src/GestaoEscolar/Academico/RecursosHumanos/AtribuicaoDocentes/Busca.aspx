<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Academico.RecursosHumanos.AtribuicaoDocentes.Busca" %>

<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="UCComboUAEscola"
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCalendario.ascx" TagName="UCCCalendario"
    TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCursoCurriculo.ascx" TagName="UCCCursoCurriculo"
    TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCurriculoPeriodo.ascx" TagName="UCCCurriculoPeriodo"
    TagPrefix="uc5" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCTurma.ascx" TagName="UCCTurma"
    TagPrefix="uc6" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc10" %>
<%@ Register Src="../../../WebControls/Combos/UCComboDocente.ascx" TagName="UCComboDocente" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/BuscaDocente/UCBuscaDocenteEscola.ascx" TagName="UCBuscaDocenteEscola" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Mensagens/UCConfirmacaoOperacao.ascx" TagPrefix="uc13" TagName="UCConfirmacaoOperacao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc13:UCConfirmacaoOperacao runat="server" ID="UCConfirmacaoOperacao" />
    <asp:UpdatePanel ID="_updBuscaTurmas" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:ValidationSummary ID="vsConsultaTurmas" runat="server" ValidationGroup='<%=validationGroup %>' />
            <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <fieldset id="fdsPesquisa" runat="server">
                <legend>Atribuição de docentes</legend>
                <asp:Label ID="lblInfoDocenciaCompartilhada" runat="server" EnableViewState="true"></asp:Label>
                <uc10:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" />
                <div id="divEscola" runat="server">
                    <uc2:UCComboUAEscola ID="UCComboUAEscola1" runat="server" CarregarEscolaAutomatico="true"
                        ObrigatorioEscola="true" ObrigatorioUA="true" MostrarMessageSelecioneEscola="true"
                        MostrarMessageSelecioneUA="true" ValidationGroup='<%=validationGroup %>' />
                </div>
                <div id="divComboDocete" runat="server">
                    <asp:Label ID="lblNomeDocente" runat="server" Text="<%$ Resources:Academico, RecursosHumanos.AtribuicaoDocentes.Busca.lblNomeDocente.Text %>" AssociatedControlID="txtNomeDocente"></asp:Label>
                    <asp:TextBox ID="txtNomeDocente" runat="server" Enabled="False" SkinID="text60CSearch" MaxLength="200"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvNomeDocente" runat="server" ControlToValidate="txtNomeDocente"
                        ErrorMessage="<%$ Resources:Academico, RecursosHumanos.AtribuicaoDocentes.Busca.rfvNomeDocente.ErrorMessage %>" Display="Dynamic" ValidationGroup='<%=validationGroup %>'>*</asp:RequiredFieldValidator>
                    <asp:ImageButton ID="btnBuscaDocente" runat="server" OnClick="btnBuscaDocente_Click"
                        SkinID="btPesquisar" CausesValidation="false" />
                    <asp:HiddenField ID="hdnDocente" runat="server" Value="-1;-1;-1;-1" />
                </div>
                <div id="divComboDoceteIndividual" runat="server">
                    <uc1:UCComboDocente ID="UCComboDocente2" runat="server" />
                </div>
                <uc3:UCCCalendario ID="UCCCalendario1" runat="server" Obrigatorio="true" SelecionarAnoCorrente="true"
                    PermiteEditar="false" MostrarMensagemSelecione="true" ValidationGroup='<%=validationGroup %>' />
                <uc4:UCCCursoCurriculo ID="UCCCursoCurriculo1" runat="server" Obrigatorio="true"
                    TrazerComboCarregado="true" PermiteEditar="false" MostrarMessageSelecione="true"
                    ValidationGroup='<%=validationGroup %>' />
                <uc5:UCCCurriculoPeriodo ID="UCCCurriculoPeriodo1" runat="server" Obrigatorio="true"
                    TrazerComboCarregado="true" PermiteEditar="false" MostrarMessageSelecione="true"
                    ValidationGroup='<%=validationGroup %>' />
                <uc6:UCCTurma ID="UCCTurma1" runat="server" Obrigatorio="true" TrazerComboCarregado="true"
                    MostrarMessageSelecione="true" PermiteEditar="false" ValidationGroup='<%=validationGroup %>' />
            </fieldset>
            <fieldset id="fdsResultado" runat="server" visible="false" class="fdsResultado">
                <legend>
                    <asp:Label ID="_lblLegenda" runat="server"></asp:Label>
                </legend>
                <div class="area-form">
                    <asp:GridView ID="_dgvTurma" runat="server" AutoGenerateColumns="False" AllowPaging="false"
                        DataKeyNames="tud_id,tdt_id,doc_id,tud_tipo,tud_cargaHorariaSemanal,tdt_posicao,tdr_id,tud_nome,duplaRegencia" DataSourceID="_odsTurma"
                        EmptyDataText="A pesquisa não encontrou resultados."
                        OnRowDataBound="_dgvTurma_RowDataBound" OnDataBound="_dgvTurma_DataBound" AllowSorting="true">
                        <Columns>
                            <asp:BoundField DataField="tud_nome" SortExpression="tud_nome" HeaderStyle-HorizontalAlign="Left" />
                            <asp:TemplateField HeaderText="Docentes">
                                <ItemTemplate>
                                    <asp:Label ID="lblDocentes" runat="server" Text=""></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Qtde. aulas semanais" SortExpression="tud_cargaHorariaSemanal"
                                HeaderStyle-HorizontalAlign="Center" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="_lbltud_cargaHorariaSemanal" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center"></ItemStyle>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="<%$ Resources:Academico, RecursosHumanos.AtribuicaoDocentes.Busca._dgvTurma.HeaderText.Titular %>">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkTitular" runat="server" ></asp:CheckBox>
                                </ItemTemplate>
                                <HeaderStyle CssClass="center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center"></ItemStyle>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="<%$ Resources:Academico, RecursosHumanos.AtribuicaoDocentes.Busca._dgvTurma.HeaderText.SegundoTitular %>">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSegundoTitular" runat="server"></asp:CheckBox>
                                </ItemTemplate>
                                <HeaderStyle CssClass="center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center"></ItemStyle>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Projetos">
                                <ItemTemplate>
                                    <asp:CheckBox ID="_chkProjeto" runat="server" AutoPostBack="true" OnCheckedChanged="_chkProjeto_CheckedChanged"></asp:CheckBox>
                                </ItemTemplate>
                                <HeaderStyle CssClass="center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Docência Compartilhada">
                                <ItemTemplate>
                                    <asp:CheckBox ID="_chkCompartilhada" runat="server" AutoPostBack="true" OnCheckedChanged="_chkCompartilhada_CheckedChanged"></asp:CheckBox>
                                </ItemTemplate>
                                <HeaderStyle CssClass="center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Substituição">
                                <ItemTemplate>
                                    <asp:CheckBox ID="_chkSubstituto" runat="server"></asp:CheckBox>
                                </ItemTemplate>
                                <HeaderStyle CssClass="center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Academico, RecursosHumanos.AtribuicaoDocentes.Busca._dgvTurma.ColunaVigenciaInicio %>">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtVigenciaInicio" runat="server" SkinID="Data"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle CssClass="center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Academico, RecursosHumanos.AtribuicaoDocentes.Busca._dgvTurma.ColunaVigenciaFim %>">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtVigenciaFim" runat="server" SkinID="Data"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle CssClass="center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:GridView>
                    <asp:ObjectDataSource ID="_odsTurma" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.TUR_TurmaDocente"
                        SelectMethod="SelecionaAtribuicaoDocentes" TypeName="MSTech.GestaoEscolar.BLL.TUR_TurmaDocenteBO"></asp:ObjectDataSource>
                    <asp:Label ID="_lblMsgRegencia" runat="server" Text="" Visible="false"></asp:Label>
                </div>
                <div class="right area-botoes-bottom">
                    <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CausesValidation="false"
                        OnClick="btnCancelar_Click" />
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="divBuscaDocente" title="Busca de docentes" class="hide">
        <asp:UpdatePanel ID="uppBuscaDocente" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <uc1:UCBuscaDocenteEscola ID="UCBuscaDocenteEscola1" runat="server" OnReturnValues="UCBuscaDocenteEscola1_ReturnValues" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
