<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCAgendamento.ascx.cs" Inherits="GestaoEscolar.WebControls.Sondagem.UCAgendamento" %>

<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="UCComboUAEscola"
    TagPrefix="uc2" %>

<asp:UpdatePanel runat="server" ID="updMessage" UpdateMode="Always">
    <ContentTemplate>
        <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
    </ContentTemplate>
</asp:UpdatePanel>
<fieldset>
    <legend><asp:Label runat="server" ID="lblLegend" Text="<%$ Resources:Academico, Sondagem.Agendamento.lblLegend.Text %>" /></legend>
    <asp:UpdatePanel runat="server" ID="updAgendamento">
        <ContentTemplate>
            <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios3" runat="server" />
            <asp:Label ID="lblSondagem" runat="server" Text="<%$ Resources:Academico, Sondagem.Agendamento.lblSondagem.Text %>" AssociatedControlID="txtSondagem"></asp:Label>
            <asp:TextBox ID="txtSondagem" runat="server" Enabled="false"></asp:TextBox>
            <asp:Label ID="lblDescricao" runat="server" Text="<%$ Resources:Academico, Sondagem.Agendamento.lblDescricao.Text %>" AssociatedControlID="txtDescricao"></asp:Label>
            <asp:TextBox ID="txtDescricao" runat="server" TextMode="MultiLine" SkinID="limite4000" Enabled="false"></asp:TextBox>
            <div><br /></div>
            <fieldset>
                <legend><asp:Label ID="lblLegendDatas" runat="server" Text="<%$ Resources:Academico, Sondagem.Agendamento.lblLegendDatas.Text %>"></asp:Label></legend>
                <asp:Button runat="server" ID="btnAdicionarAgendamento" Text="<%$ Resources:Academico, Sondagem.Agendamento.btnAdicionarAgendamento.Text %>"
                    OnClick="btnAdicionarAgendamento_Click" CausesValidation="false" />
                <asp:GridView runat="server" ID="grvAgendamentos" AutoGenerateColumns="false" AllowPaging="false" AllowSorting="false"
                    DataKeyNames="sda_id, sda_inicio, sda_fim, sda_dataInicio, sda_dataFim, sda_idRetificada, esc_id, uni_id, sda_situacao" 
                    EmptyDataText="<%$ Resources:Academico, Sondagem.Agendamento.grvAgendamentos.EmptyDataText %>"
                    OnRowDataBound="grvAgendamentos_RowDataBound" OnRowCommand="grvAgendamentos_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="sda_inicio" HeaderText="<%$ Resources:Academico, Sondagem.Agendamento.grvAgendamentos.HeaderDataInicio %>" />
                        <asp:BoundField DataField="sda_fim" HeaderText="<%$ Resources:Academico, Sondagem.Agendamento.grvAgendamentos.HeaderDataFim %>" />
                        <asp:BoundField DataField="periodos" HeaderText="<%$ Resources:Academico, Sondagem.Agendamento.grvAgendamentos.HeaderPeriodos %>" />
                        <asp:TemplateField HeaderText="<%$ Resources:Academico, Sondagem.Agendamento.grvAgendamentos.HeaderRetificar %>">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnRetificar" runat="server" SkinID="btDetalhar" CommandName="Retificar" CausesValidation="false"
                                    ToolTip="<%$ Resources:Academico, Sondagem.Agendamento.grvAgendamentos.btnRetificar.ToolTip %>" />
                                <asp:Label runat="server" ID="lblRetificado" Text="<%$ Resources:Academico, Sondagem.Agendamento.grvAgendamentos.lblRetificado.Text %>" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Academico, Sondagem.Agendamento.grvAgendamentos.HeaderAlterar %>">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnAlterar" runat="server" SkinID="btEditar" CommandName="Alterar" CausesValidation="false"
                                    ToolTip="<%$ Resources:Academico, Sondagem.Agendamento.grvAgendamentos.btnAlterar.ToolTip %>" />
                                <asp:Label ID="lblCancelado" runat="server" Text="<%$ Resources:Academico, Sondagem.Agendamento.grvAgendamentos.lblCancelado.Text %>" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Academico, Sondagem.Agendamento.grvAgendamentos.HeaderCancelarAgendamento %>">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnCancelarAgendamento" SkinID="btCancelarAgendamento" runat="server" CommandName="Cancelar" CausesValidation="false"
                                    ToolTip="<%$ Resources:Academico, Sondagem.Agendamento.grvAgendamentos.btnCancelarAgendamento.ToolTip %>" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Academico, Sondagem.Agendamento.grvAgendamentos.HeaderExcluir %>">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnExcluir" SkinID="btExcluir" runat="server" CommandName="Excluir" CausesValidation="false"
                                    ToolTip="<%$ Resources:Academico, Sondagem.Agendamento.grvAgendamentos.btnExcluir.ToolTip %>" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </fieldset>
            <div class="right">
                <asp:Button ID="bntSalvar" runat="server" Text="<%$ Resources:Academico, Sondagem.Agendamento.bntSalvar.Text %>" OnClick="bntSalvar_Click" />
                <asp:Button ID="btnCancelar" runat="server" Text="<%$ Resources:Academico, Sondagem.Agendamento.btnCancelar.Text %>" CausesValidation="false"
                    OnClick="btnCancelar_Click" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</fieldset>
<div id="divInserir" class="hide">
    <asp:UpdatePanel runat="server" ID="updMessagePopUp" UpdateMode="Always">
        <ContentTemplate>
            <asp:Label ID="lblMessagePopUp" runat="server" EnableViewState="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel runat="server" ID="updPopUp" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="periodo" />
            <fieldset>
                <legend><asp:Label runat="server" ID="lblTituloPopUp" Text="<%$ Resources:Academico, Sondagem.Agendamento.lblTituloPopUp.Text %>" /></legend>
                <div runat="server" id="divEscola" visible="false">
                    <uc2:UCComboUAEscola ID="UCComboUAEscola" runat="server" CarregarEscolaAutomatico="true"
                        MostrarMessageSelecioneEscola="true" MostrarMessageSelecioneUA="true" OnIndexChangedUA="UCComboUAEscola_IndexChangedUA"
                        ValidationGroup="periodo" ObrigatorioEscola="true" ObrigatorioUA="true" />
                </div>
                <asp:Label runat="server" ID="lblDataInicio" Text="<%$ Resources:Academico, Sondagem.Agendamento.lblDataInicio.Text %>" AssociatedControlID="txtDataInicio" />
                <asp:TextBox runat="server" ID="txtDataInicio" SkinID="Data" />
                <asp:RequiredFieldValidator ID="_rfvInicio" runat="server" ErrorMessage="<%$ Resources:Academico, Sondagem.Agendamento._rfvInicio.ErrorMessage %>"
                    Display="Dynamic" ControlToValidate="txtDataInicio" ValidationGroup="periodo">*</asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cvDataInicio" runat="server" ControlToValidate="txtDataInicio"
                    ValidationGroup="periodo" Display="Dynamic" ErrorMessage="<%$ Resources:Academico, Sondagem.Agendamento.cvDataInicio.ErrorMessage %>" 
                    OnServerValidate="ValidarData_ServerValidate">*</asp:CustomValidator>
                <asp:Label runat="server" ID="lblDataFim" Text="<%$ Resources:Academico, Sondagem.Agendamento.lblDataFim.Text %>" AssociatedControlID="txtDataFim" />
                <asp:TextBox runat="server" ID="txtDataFim" SkinID="Data" />
                <asp:RequiredFieldValidator ID="_rfvFim" runat="server" ErrorMessage="<%$ Resources:Academico, Sondagem.Agendamento._rfvFim.ErrorMessage %>"
                    Display="Dynamic" ControlToValidate="txtDataFim" ValidationGroup="periodo">*</asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cvDataFim" runat="server" ControlToValidate="txtDataFim"
                    ValidationGroup="periodo" Display="Dynamic" ErrorMessage="<%$ Resources:Academico, Sondagem.Agendamento.cvDataFim.ErrorMessage %>"
                    OnServerValidate="ValidarData_ServerValidate">*</asp:CustomValidator>
                <asp:CompareValidator ID="cpvDataFim2" runat="server" ControlToValidate="txtDataFim"
                    ValidationGroup="periodo" Display="Dynamic" Type="Date" Operator="GreaterThanEqual"
                    ControlToCompare="txtDataInicio" ErrorMessage="<%$ Resources:Academico, Sondagem.Agendamento.cpvDataFim2.ErrorMessage %>">*</asp:CompareValidator>
                <div><br /></div>
                <fieldset>
                    <legend><asp:Label runat="server" ID="lblLegendPeriodos" Text="<%$ Resources:Academico, Sondagem.Agendamento.lblLegendPeriodos.Text %>" /></legend>
                    <asp:CheckBox ID="ckbSelecionarTodosPeriodos" runat="server" Text="<%$ Resources:Academico, AgendamentoSondagem.Agendamento.ckbSelecionarTodosPeriodos.Text %>" OnCheckedChanged="ckbSelecionarTodosPeriodos_CheckedChanged" AutoPostBack="true"/>
                    <asp:Repeater runat="server" ID="rptNivelEnsino" OnItemDataBound="rptNivelEnsino_ItemDataBound">
                        <AlternatingItemTemplate>
                            <fieldset>
                                <legend><asp:Label runat="server" ID="lblNivelEnsino" /></legend>
                                <div></div>
                                <asp:HiddenField ID="hdnTneNome" runat="server" Value='<%# Eval("tne_nomeSimples") %>' />
                                <asp:Repeater ID="rptCampos" runat="server">
                                    <HeaderTemplate>
                                        <div class="checkboxlist-columns">
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnId" runat="server" Value='<%# Eval("tcp_id") %>' />
                                        <asp:HiddenField ID="hdnOrdem" runat="server" Value='<%# Eval("tcp_ordem") %>' />
                                        <asp:HiddenField ID="hdnTneOrdem" runat="server" Value='<%# Eval("tne_ordem") %>' />
                                        <asp:CheckBox ID="ckbCampo" runat="server" Text='<%# Eval("tcp_descricao") %>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </div> 
                                    </FooterTemplate>
                                </asp:Repeater>
                            </fieldset>
                        </AlternatingItemTemplate>
                        <ItemTemplate>
                            <fieldset>
                                <legend><asp:Label runat="server" ID="lblNivelEnsino" /></legend>
                                <div></div>
                                <asp:HiddenField ID="hdnTneNome" runat="server" Value='<%# Eval("tne_nomeSimples") %>' />
                                <asp:Repeater ID="rptCampos" runat="server">
                                    <HeaderTemplate>
                                        <div class="checkboxlist-columns">
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnId" runat="server" Value='<%# Eval("tcp_id") %>' />
                                        <asp:HiddenField ID="hdnOrdem" runat="server" Value='<%# Eval("tcp_ordem") %>' />
                                        <asp:HiddenField ID="hdnTneOrdem" runat="server" Value='<%# Eval("tne_ordem") %>' />
                                        <asp:CheckBox ID="ckbCampo" runat="server" Text='<%# Eval("tcp_descricao") %>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </div> 
                                    </FooterTemplate>
                                </asp:Repeater>
                            </fieldset>
                        </ItemTemplate>
                    </asp:Repeater>
                </fieldset>
                <div class="right">
                    <asp:Button ID="btnAdicionar" runat="server" Text="<%$ Resources:Academico, Sondagem.Agendamento.bntAdicionar.Text %>" ValidationGroup="periodo" 
                        OnClick="btnAdicionar_Click" />
                    <asp:Button ID="btnCancelarItem" runat="server" Text="<%$ Resources:Academico, Sondagem.Agendamento.btnCancelar.Text %>" CausesValidation="false"
                        OnClientClick="$('#divInserir').dialog('close');" />
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>