<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Configuracao.RelatorioAtendimento.Cadastro" %>

<%@ PreviousPageType VirtualPath="~/Configuracao/RelatorioAtendimento/Busca.aspx" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboQuestionario.ascx" TagName="UCComboQuestionario" 
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoDisciplina.ascx" TagName="UCComboTipoDisciplina" 
    TagPrefix="uc3" %>

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
                <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.lblTitulo.Text %>" AssociatedControlID="txtTitulo" />
                <asp:TextBox ID="txtTitulo" runat="server" SkinID="text60C" MaxLength="200"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvTitulo" runat="server" ControlToValidate="txtTitulo" ValidationGroup="vgRelatorioAtendimento"
                    Display="Dynamic" ErrorMessage="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.rfvTitulo.ErrorMessage %>" Text="*" />
                <asp:Label ID="lblPeriodicidade" runat="server" Text="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.lblPeriodicidade.Text %>" AssociatedControlID="ddlPeriodicidade"></asp:Label>
                <asp:DropDownList ID="ddlPeriodicidade" runat="server">
                    <asp:ListItem Text="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.ddlPeriodicidade.msgSelecione %>" Value="0"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources:Enumerador, CLS_RelatorioAtendimentoBO.CLS_RelatorioAtendimentoPeriodicidade.Periodico %>" Value="1"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources:Enumerador, CLS_RelatorioAtendimentoBO.CLS_RelatorioAtendimentoPeriodicidade.Encerramento %>" Value="2"></asp:ListItem>
                </asp:DropDownList>
                <asp:CompareValidator ID="cpvPeriodicidade" runat="server" ErrorMessage="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.cpvPeriodicidade.ErrorMessage %>"
                    ControlToValidate="ddlPeriodicidade" Operator="GreaterThan" ValueToCompare="0"
                    Display="Dynamic" ValidationGroup="vgRelatorioAtendimento">*</asp:CompareValidator>
                <asp:Label ID="lblTipo" runat="server" Text="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.lblTipo.Text %>" AssociatedControlID="ddlTipo"></asp:Label>
                <asp:DropDownList ID="ddlTipo" runat="server"  
                    OnSelectedIndexChanged="ddlTipo_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Text="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.ddlTipo.msgSelecione %>" Value="0"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources:Enumerador, CLS_RelatorioAtendimentoBO.CLS_RelatorioAtendimentoTipo.AEE %>" Value="1"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources:Enumerador, CLS_RelatorioAtendimentoBO.CLS_RelatorioAtendimentoTipo.NAAPA %>" Value="2"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources:Enumerador, CLS_RelatorioAtendimentoBO.CLS_RelatorioAtendimentoTipo.RP %>" Value="3"></asp:ListItem>
                </asp:DropDownList>
                <asp:CompareValidator ID="cpvTipo" runat="server" ErrorMessage="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.cpvTipo.ErrorMessage %>"
                    ControlToValidate="ddlTipo" Operator="GreaterThan" ValueToCompare="0"
                    Display="Dynamic" ValidationGroup="vgRelatorioAtendimento">*</asp:CompareValidator>
                <div runat="server" id="divRacaCor" visible="false">
                    <asp:CheckBox runat="server" ID="chkExibeRacaCor" Text="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.chkExibeRacaCor.Text %>" />
                </div>
                <div runat="server" id="divHipotese" visible="false">
                    <asp:CheckBox runat="server" ID="chkExibeHipotese" Text="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.chkExibeHipotese.Text %>" />
                </div>
                <div runat="server" id="divDisciplina" visible="false">
                    <uc3:UCComboTipoDisciplina runat="server" ID="UCComboTipoDisciplina" MostrarMessageSelecione="True" PermiteEditar="True" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <br />
        </div>
        <div>
            <fieldset>
                <legend>
                    <asp:Label ID="lblLegendAnexo" runat="server" Text="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.lblLegendAnexo.Text %>" />
                </legend>
                <div runat="server" id="divAddAnexo" visible="false">
                    <asp:ValidationSummary ID="vsAnexo" runat="server" ValidationGroup="vgAnexo" />
                    <asp:Label ID="lblTituloAnexo" runat="server" Text="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.lblTituloAnexo.Text %>" AssociatedControlID="txtTituloAnexo" />
                    <asp:TextBox ID="txtTituloAnexo" runat="server" SkinID="text60C" MaxLength="200"></asp:TextBox>
                    <table><tr>
                        <td>
                            <asp:Label ID="lblAnexo" runat="server" Text="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.lblAnexo.Text %>" AssociatedControlID="fupAnexo"></asp:Label>
                            <asp:FileUpload ID="fupAnexo" runat="server" ToolTip="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.fupAnexo.ToolTip %>" />
                        </td>
                        <td>
                            <asp:ImageButton runat="server" ID="btnAddAnexo" ToolTip="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.btnAddAnexo.Text %>" ValidationGroup="vgAnexo"
                                SkinID="btNovo" OnClick="btnAddAnexo_Click" style="padding-left: 5px; padding-top: 25px" />
                        </td>
                    </tr></table>
                </div>
                <div runat="server" id="divAnexoAdicionado" visible="false">
                    <asp:HyperLink runat="server" ID="hplAnexo"></asp:HyperLink>
                    <asp:ImageButton runat="server" ID="btnExcluirAnexo" ToolTip="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.btnExcluirAnexo.Text %>" CausesValidation="false"
                        SkinID="btExcluir" OnClick="btnExcluirAnexo_Click" />
                </div>
            </fieldset>
        </div>
        <asp:UpdatePanel ID="updQuestionario" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <fieldset>
                    <legend>
                        <asp:Label runat="server" ID="lblLegendQuestionario" Text="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.lblLegendQuestionario.Text %>" />
                    </legend>
                    <div runat="server" id="divAdicionarQuestionario">
                        <fieldset>
                            <asp:ValidationSummary ID="vsQuestionario" runat="server" ValidationGroup="vgQuestionario" />
                            <table><tr>
                                <td>
                                    <uc2:UCComboQuestionario runat="server" id="UCComboQuestionario" ValidationGroup="vgQuestionario" Obrigatorio="True" MostrarMessageSelecione="True" PermiteEditar="True" />
                                </td>
                                <td>
                                    <asp:ImageButton runat="server" ID="btnAddQuestionario" ToolTip="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.btnAdicionarQuestionario.Text %>" ValidationGroup="vgQuestionario"
                                        SkinID="btNovo" OnClick="btnAdicionarQuestionario_Click" style="padding-left: 5px; padding-top: 25px" />
                                </td>
                            </tr></table>
                        </fieldset>
                    </div>
                    <asp:GridView runat="server" ID="gvQuestionario" AutoGenerateColumns="false" AllowPaging="false" AllowSorting="false"
                        DataKeyNames="qst_id, raq_id, raq_ordem" EmptyDataText="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.gvQuestionario.EmptyDataText %>"
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
        <asp:UpdatePanel ID="updGrupo" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <fieldset>
                    <legend>
                        <asp:Label runat="server" ID="lblLegendGrupo" Text="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.lblLegendGrupo.Text %>" />
                    </legend>
                    <asp:Panel runat="server" id="pnlGrupo" style="overflow: scroll; height: 500px;">
                        <asp:GridView runat="server" ID="gvGrupo" AutoGenerateColumns="false" AllowPaging="false" AllowSorting="false"
                            EmptyDataText="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.gvGrupo.EmptyDataText %>" DataKeyNames="gru_id">
                            <Columns>
                                <asp:BoundField HeaderText="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.gvGrupo.HeaderNome %>" DataField="gru_nome" />
                                <asp:TemplateField HeaderText="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.gvGrupo.HeaderPermissaoConsulta %>" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chkpermissaoConsulta" Checked='<%# Eval("rag_permissaoConsulta") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.gvGrupo.HeaderPermissaoEdicao %>" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chkpermissaoEdicao" Checked='<%# Eval("rag_permissaoEdicao") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.gvGrupo.HeaderPermissaoExclusao %>" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chkpermissaoExclusao" Checked='<%# Eval("rag_permissaoExclusao") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.gvGrupo.HeaderPermissaoAprovacao %>" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chkpermissaoAprovacao" Checked='<%# Eval("rag_permissaoAprovacao") %>' />
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
        <asp:UpdatePanel ID="updCargo" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <fieldset>
                    <legend>
                        <asp:Label runat="server" ID="lblLegendCargo" Text="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.lblLegendCargo.Text %>" />
                    </legend>
                    <asp:Panel runat="server" id="pnlCargo" style="overflow: scroll; height: 500px;">
                        <asp:GridView runat="server" ID="gvCargo" AutoGenerateColumns="false" AllowPaging="false" AllowSorting="false"
                            EmptyDataText="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.gvCargo.EmptyDataText %>" DataKeyNames="crg_id">
                            <Columns>
                                <asp:BoundField HeaderText="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.gvCargo.HeaderDescricao %>" DataField="crg_descricao" />
                                <asp:TemplateField HeaderText="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.gvCargo.HeaderPermissaoConsulta %>" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chkpermissaoConsulta" Checked='<%# Eval("rac_permissaoConsulta") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.gvCargo.HeaderPermissaoEdicao %>" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chkpermissaoEdicao" Checked='<%# Eval("rac_permissaoEdicao") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.gvCargo.HeaderPermissaoExclusao %>" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chkpermissaoExclusao" Checked='<%# Eval("rac_permissaoExclusao") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.gvCargo.HeaderPermissaoAprovacao %>" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chkpermissaoAprovacao" Checked='<%# Eval("rac_permissaoAprovacao") %>' />
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
        <asp:CheckBox ID="ckbBloqueado" runat="server" Visible="False" Text="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.ckbBloqueado.Text %>" />
        <div class="right">
            <asp:Button ID="bntSalvar" runat="server" Text="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.bntSalvar.Text %>" OnClick="bntSalvar_Click" ValidationGroup="vgRelatorioAtendimento" />
            <asp:Button ID="btnCancelar" runat="server" Text="<%$ Resources:Configuracao, RelatorioAtendimento.Cadastro.btnCancelar.Text %>" CausesValidation="false"
                OnClick="btnCancelar_Click" />
        </div>
    </fieldset>
</asp:Content>
