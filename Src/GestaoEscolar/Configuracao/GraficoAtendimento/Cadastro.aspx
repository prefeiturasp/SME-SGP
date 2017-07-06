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
            <asp:Label runat="server" ID="lblLegend" Text="Gráfico de atendimento" />
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
                <div>
                    <asp:Label ID="lblTipoGrafico" runat="server" Text="Tipo de gráfico *" AssociatedControlID="ddlTipoGrafico"></asp:Label>
                    <asp:DropDownList ID="ddlTipoGrafico" runat="server">
                        <asp:ListItem Text="-- Selecione um tipo de gráfico --" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Barra" Value="1"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:CompareValidator ID="cpvTipoGrafico" runat="server" ErrorMessage="Tipo de gráfico é obrigatório."
                        ControlToValidate="ddlTipoGrafico" Operator="GreaterThan" ValueToCompare="0"
                        Display="Dynamic" ValidationGroup="vgRelatorioAtendimento">*</asp:CompareValidator>
                </div>
                <div runat="server" id="divPeriodicidade">
                    <asp:Label ID="lblPeriodicidade" runat="server" Text="Eixo de agrupamento *" AssociatedControlID="ddlEixoAgrupamento"></asp:Label>
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
        <asp:UpdatePanel ID="updFiltro" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset>
                    <legend>
                        <asp:Label runat="server" ID="Label1" Text="Filtros fixos" />
                    </legend>

                    <asp:Label ID="lblFiltroFixo" runat="server" Text="Tipo de filtro: " AssociatedControlID="ddlFiltroFixo"></asp:Label>
                    <asp:DropDownList ID="ddlFiltroFixo" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltroFixo_SelectedIndexChanged">
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
                        <asp:TextBox ID="txtDtInicial" runat="server" CssClass="maskData" SkinID="Data"></asp:TextBox>

                        <asp:Label ID="Label5" runat="server" Text="Até:" AssociatedControlID="txtDtFinal" />
                        <asp:TextBox ID="txtDtFinal" runat="server" CssClass="maskData" SkinID="Data"></asp:TextBox>
                    </div>
                    <div runat="server" id="divDetalhamentoDeficiencia" visible="false">

                        <!-- Tipo de deficiencia -->
                        <uc1:ComboTipoDeficiencia runat="server" ID="ComboTipoDeficiencia" />
                        <!-- Detalhamento -->

                        <div runat="server" id="divDetalhes" visible="false">
                            <asp:CheckBoxList ID="cklDetalhes" runat="server" RepeatDirection="Horizontal" DataTextField="dfd_nome" DataValueField="dfd_id">
                            </asp:CheckBoxList>
                        </div>
                                                
                    </div>
                    <div id="divBotoesFiltro" runat="server" class="right" visible="false">
                        <asp:Button ID="btnAdicionarFiltro" runat="server" Text="Adicionar filtro"
                            OnClick="btnAdicionarFiltro_Click" />
                        <asp:Button ID="btnCancelarFiltro" runat="server" Text="Cancelar filtro" CausesValidation="false"
                            OnClick="btnCancelarFiltro_Click" />
                    </div>
                    <asp:GridView runat="server" ID="gvFiltroFixo" AutoGenerateColumns="false" AllowPaging="false" AllowSorting="false"
                        DataKeyNames="gra_id, gff_tipoFiltro, IsNew" EmptyDataText="Nenhum filtro fixo ligado ao gráfico."
                        OnRowDataBound="gvFiltroFixo_RowDataBound" OnRowCommand="gvFiltroFixo_RowCommand">
                        <Columns>
                            <asp:BoundField HeaderText="Tipo de filtro" DataField="gff_tituloFiltro" />
                            <asp:BoundField HeaderText="Valor" DataField="gff_valorDetalhado" />
                            <asp:TemplateField HeaderText="Excluir">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnExcluir" SkinID="btExcluir" runat="server" CommandName="Excluir" CausesValidation="false"
                                        ToolTip="Excluir filtro fixo." />
                                </ItemTemplate>
                                <HeaderStyle CssClass="center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
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
                                        <uc2:UCComboQuestionario runat="server" ID="UCComboQuestionario" ValidationGroup="vgQuestionario" Obrigatorio="True" MostrarMessageSelecione="True" PermiteEditar="false"/>
                                    </td>
                                    <td Style="padding-left: 5px">
                                        <asp:Label ID="Label6" runat="server" Text="Pergunta" AssociatedControlID="ddlPergunta"></asp:Label>
                                        <asp:DropDownList ID="ddlPergunta" runat="server" OnSelectedIndexChanged="ddlPergunta_SelectedIndexChanged" AutoPostBack="true" Enabled="false">
                                            <asp:ListItem Text="-- Selecione uma pergunta --" Value="-1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td Style="padding-left: 5px">
                                        <asp:Label ID="Label7" runat="server" Text="Resposta" AssociatedControlID="ddlResposta"></asp:Label>
                                        <asp:DropDownList ID="ddlResposta" runat="server" Enabled="false">
                                            <asp:ListItem Text="-- Selecione uma resposta --" Value="-1"></asp:ListItem>
                                        </asp:DropDownList>
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
                        DataKeyNames="qst_id, qtc_id, qtr_id, IsNew, emUso" EmptyDataText="Nenhum filtro personalizado ligado ao gráfico."
                        OnRowDataBound="gvQuestionario_RowDataBound" OnRowCommand="gvQuestionario_RowCommand">
                        <Columns>
                            <asp:BoundField HeaderText="Questionário" DataField="qst_titulo" />
                            <asp:BoundField HeaderText="Pergunta" DataField="qtc_texto" />
                            <asp:BoundField HeaderText="Resposta" DataField="qtr_texto" />                            
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
