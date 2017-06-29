<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Configuracao.GraficoAtendimento.Cadastro" %>

<%@ PreviousPageType VirtualPath="~/Configuracao/GraficoAtendimento/Busca.aspx" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboQuestionario.ascx" TagName="UCComboQuestionario"
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoDisciplina.ascx" TagName="UCComboTipoDisciplina"
    TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/UCComboRelatorioAtendimento.ascx" TagPrefix="uc1" TagName="UCComboRelatorioAtendimento" %>
<%@ Register Src="~/WebControls/Combos/UCComboRacaCor.ascx" TagPrefix="uc1" TagName="UCComboRacaCor" %>
<%@ Register Src="~/WebControls/Combos/UCComboSexo.ascx" TagPrefix="uc1" TagName="UCComboSexo" %>
<%@ Register Src="~/WebControls/Combos/ComboTipoDeficiencia.ascx" TagPrefix="uc1" TagName="ComboTipoDeficiencia" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="updMessage" UpdateMode="Always">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="vsRelatorioAtendimento" runat="server" ValidationGroup="vgRelatorioAtendimento" />
    <fieldset>
        <legend>
            <asp:Label runat="server" ID="lblLegend" Text="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.lblLegend.Text %>" />
        </legend>
        <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios3" runat="server" />
        <asp:UpdatePanel runat="server" ID="updCadastro" UpdateMode="Always">
            <ContentTemplate>

                <asp:RequiredFieldValidator ID="rfvTitulo" runat="server" ControlToValidate="txtTitulo" ValidationGroup="vgRelatorioAtendimento"
                    Display="Dynamic" ErrorMessage="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.rfvTitulo.ErrorMessage %>" Text="*" />
                <asp:Label ID="lblTipo" runat="server" Text="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.lblTipo.Text %>" AssociatedControlID="ddlTipo"></asp:Label>
                <asp:DropDownList ID="ddlTipo" runat="server"
                    OnSelectedIndexChanged="ddlTipo_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Text="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.ddlTipo.msgSelecione %>" Value="0"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources:Enumerador, CLS_RelatorioAtendimentoBO.CLS_RelatorioAtendimentoTipo.AEE %>" Value="1"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources:Enumerador, CLS_RelatorioAtendimentoBO.CLS_RelatorioAtendimentoTipo.NAAPA %>" Value="2"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources:Enumerador, CLS_RelatorioAtendimentoBO.CLS_RelatorioAtendimentoTipo.RP %>" Value="3"></asp:ListItem>
                </asp:DropDownList>

                <uc1:UCComboRelatorioAtendimento runat="server" ID="UCComboRelatorioAtendimento" />
                <br />
                <br />
                <asp:Label ID="lblTitulo" runat="server" Text="Título do gráfico" AssociatedControlID="txtTitulo" />
                <asp:TextBox ID="txtTitulo" runat="server" SkinID="text60C" MaxLength="200"></asp:TextBox>
                <div runat="server" id="divPeriodicidade">
                    <asp:Label ID="lblPeriodicidade" runat="server" Text="Eixo de agrupamento: " AssociatedControlID="ddlEixoAgrupamento"></asp:Label>
                    <asp:DropDownList ID="ddlEixoAgrupamento" runat="server">
                        <asp:ListItem Text="-- Selecione um eixo de agrupamento --" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Curso" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Ciclo" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Período do curso" Value="3"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:CompareValidator ID="cpvPeriodicidade" runat="server" ErrorMessage="Eixo de agrupamento é obrigatório."
                        ControlToValidate="ddlEixoAgrupamento" Operator="GreaterThan" ValueToCompare="0"
                        Display="Dynamic" ValidationGroup="vgRelatorioAtendimento">*</asp:CompareValidator>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <br />
        </div>
        <asp:UpdatePanel ID="updFiltro" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <fieldset>
                    <legend>
                        <asp:Label runat="server" ID="Label1" Text="Filtros personalizados" />
                    </legend>

                    <asp:Label ID="lblFiltroFixo" runat="server" Text="Tipo de filtro: " AssociatedControlID="ddlFiltroFixo"></asp:Label>
                    <asp:DropDownList ID="ddlFiltroFixo" runat="server" AutoPostBack="true">
                        <asp:ListItem Text="-- Selecione um tipo de filtro --" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Período do preenchimento do relatório" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Raça/Cor" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Faixa de idade" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Sexo" Value="4"></asp:ListItem>
                    </asp:DropDownList>

                    <div runat="server" id="divRacaCor" visible="false">
                        <uc1:UCComboRacaCor runat="server" ID="UCComboRacaCor" />
                    </div>
                    <div runat="server" id="divSexo" visible="false">
                        <uc1:UCComboSexo runat="server" ID="UCComboSexo" />
                    </div>
                    <div runat="server" id="divIdade" visible="false">
                        <asp:Label ID="Label2" runat="server" Text="Idade mínima" AssociatedControlID="txtIdadeInicial" />
                        <asp:TextBox ID="txtIdadeInicial" runat="server" SkinID="text20C" MaxLength="2"></asp:TextBox>

                        <asp:Label ID="Label3" runat="server" Text="Idade máxima" AssociatedControlID="txtIdadeFinal" />
                        <asp:TextBox ID="txtIdadeFinal" runat="server" SkinID="text20C" MaxLength="2"></asp:TextBox>
                    </div>
                    <div runat="server" id="divDataPreenchimento" visible="false">
                        <asp:Label ID="Label4" runat="server" Text="Preenchimento de relatório de:" AssociatedControlID="txtDtInicial" />
                        <asp:TextBox ID="txtDtInicial" runat="server" SkinID="text30C" MaxLength="10"></asp:TextBox>

                        <asp:Label ID="Label5" runat="server" Text="Até:" AssociatedControlID="txtDtFinal" />
                        <asp:TextBox ID="txtDtFinal" runat="server" SkinID="text30C" MaxLength="10"></asp:TextBox>
                    </div>
                    <div runat="server" id="divDetalhamentoDeficiencia" visible="false">

                        <!-- Tipo de deficiencia -->
                        <uc1:ComboTipoDeficiencia runat="server" ID="ComboTipoDeficiencia" />
                        <!-- Detalhamento -->
                        <asp:UpdatePanel ID="updDetalhe" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <fieldset>
                                    <legend>
                                        <asp:Label runat="server" ID="lblLegendGrupo" Text="Detalhamento da deficiência" />
                                    </legend>
                                    <asp:Panel runat="server" ID="pnlGrupo" Style="overflow: scroll; height: 500px;">
                                        <asp:GridView runat="server" ID="gvDetalhe" AutoGenerateColumns="false" AllowPaging="false" AllowSorting="false"
                                            EmptyDataText="Não existe detalhamento para essa deficiência." DataKeyNames="dfd_id">
                                            <Columns>
                                                <asp:BoundField HeaderText="Detalhamento" DataField="dfd_nome" />
                                                <asp:TemplateField HeaderText="Selecionar" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" ID="chkSelecionar" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                </fieldset>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updQuestionario" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <fieldset>
                    <legend>
                        <asp:Label runat="server" ID="lblLegendQuestionario" Text="Filtros personalizados" />
                    </legend>
                    <div runat="server" id="divAdicionarQuestionario">
                        <fieldset>
                            <asp:ValidationSummary ID="vsQuestionario" runat="server" ValidationGroup="vgQuestionario" />
                            <table>
                                <tr>
                                    <td>
                                        <uc2:UCComboQuestionario runat="server" ID="UCComboQuestionario" ValidationGroup="vgQuestionario" Obrigatorio="True" MostrarMessageSelecione="True" PermiteEditar="True" />
                                    </td>

                                    <td>
                                        <asp:ImageButton runat="server" ID="btnAddQuestionario" ToolTip="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.btnAdicionarQuestionario.Text %>" ValidationGroup="vgQuestionario"
                                            SkinID="btNovo" OnClick="btnAdicionarQuestionario_Click" Style="padding-left: 5px; padding-top: 25px" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <asp:GridView runat="server" ID="gvQuestionario" AutoGenerateColumns="false" AllowPaging="false" AllowSorting="false"
                        DataKeyNames="qst_id, raq_id, raq_ordem, IsNew, emUso" EmptyDataText="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.gvQuestionario.EmptyDataText %>"
                        OnDataBound="gvQuestionario_DataBound" OnRowDataBound="gvQuestionario_RowDataBound" OnRowCommand="gvQuestionario_RowCommand">
                        <Columns>
                            <asp:BoundField HeaderText="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.gvQuestionario.HeaderTitulo %>" DataField="qst_titulo" />
                            <asp:TemplateField HeaderText="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.gvQuestionario.HeaderOrdem %>">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("sdq_ordem") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="_btnSubir" runat="server" CausesValidation="false" CommandName="Subir"
                                        Height="16" Width="16" />
                                    <asp:ImageButton ID="_btnDescer" runat="server" CausesValidation="false" CommandName="Descer"
                                        Height="16" Width="16" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="center" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.grvQuestoes.HeaderExcluir %>">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnExcluir" SkinID="btExcluir" runat="server" CommandName="Excluir" CausesValidation="false"
                                        ToolTip="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.gvQuestionario.btnExcluir.ToolTip %>" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:CheckBox ID="ckbBloqueado" runat="server" Visible="False" Text="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.ckbBloqueado.Text %>" />
        <div class="right">
            <asp:Button ID="bntSalvar" runat="server" Text="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.bntSalvar.Text %>" OnClick="bntSalvar_Click" ValidationGroup="vgRelatorioAtendimento" />
            <asp:Button ID="btnCancelar" runat="server" Text="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.btnCancelar.Text %>" CausesValidation="false"
                OnClick="btnCancelar_Click" />
        </div>
    </fieldset>
</asp:Content>
