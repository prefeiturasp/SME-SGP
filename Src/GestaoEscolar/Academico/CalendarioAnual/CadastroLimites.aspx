<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="CadastroLimites.aspx.cs" Inherits="GestaoEscolar.Academico.CalendarioAnual.CadastroLimites" %>
<%@ PreviousPageType VirtualPath="~/Academico/CalendarioAnual/Busca.aspx" %>

<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCTipoEvento.ascx" TagName="UCCTipoEvento" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCPeriodoCalendario.ascx" TagName="UCCPeriodoCalendario" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="UCComboUAEscola" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="pnlMensagem" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="summary" runat="server" ValidationGroup="<%= ValidationGroup %>" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <fieldset>
        <legend>Cadastro de limites para data de criação de eventos</legend>
        <asp:UpdatePanel ID="updCadastro" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <b>Calendário:</b>
                <asp:Literal ID="lblCalendarioAno" runat="server" />
                <br />
                <br />
                <uc:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" EnableViewState="false" />
                <div id="divCadastro" runat="server">
                    <uc:UCCTipoEvento ID="UCCTipoEvento" runat="server" MostrarMensagemSelecione="true" 
                        Obrigatorio="true" OnIndexChanged="UCCTipoEvento_IndexChanged"
                        ValidationGroup="<%= ValidationGroup %>" />

                    <uc:UCCPeriodoCalendario ID="UCCPeriodoCalendario" runat="server" MostrarMensagemSelecione="true" 
                        Obrigatorio="true" Visible="false" ValidationGroup="<%= ValidationGroup %>" />

                    <asp:Label ID="lblData" runat="server" Text="Período *" AssociatedControlID="txtDataInicio" />
                    <asp:TextBox ID="txtDataInicio" runat="server" SkinID="Data"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvDataInicio" runat="server" ControlToValidate="txtDataInicio"
                        Display="Dynamic" ErrorMessage="Data de início é obrigatório." Text="*" 
                        ValidationGroup="<%= ValidationGroup %>" />
                    <asp:CustomValidator ID="ctvDataInicioFormato" runat="server" ControlToValidate="txtDataInicio"
                        Display="Dynamic" ErrorMessage="Data de início não está no formato dd/mm/aaaa ou é inexistente."
                        OnServerValidate="ValidarData_ServerValidate" Text="*" ValidationGroup="<%= ValidationGroup %>" />
                    <asp:CompareValidator ID="cpvDataInicio" runat="server" ControlToValidate="txtDataInicio"
                        ErrorMessage="Data de início deve ser maior ou igual à data atual." Operator="GreaterThanEqual" 
                        Display="Dynamic" Type="Date" Text="*" ValidationGroup="<%= ValidationGroup %>" />
                    <asp:Label ID="lblSeparador" runat="server" EnableViewState="False" Text="a" />
                    <asp:TextBox ID="txtDataFim" runat="server" SkinID="Data"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvDataFim" runat="server" ControlToValidate="txtDataFim"
                        Display="Dynamic" ErrorMessage="Data de fim é obrigatório." Text="*" 
                        ValidationGroup="<%= ValidationGroup %>" />
                    <asp:CustomValidator ID="ctvDataFimFormato" runat="server" ControlToValidate="txtDataFim"
                        Display="Dynamic" ErrorMessage="Data de fim não está no formato dd/mm/aaaa ou é inexistente."
                        OnServerValidate="ValidarData_ServerValidate" Text="*" ValidationGroup="<%= ValidationGroup %>" />
                    <asp:CompareValidator ID="cpvDataFim" runat="server" ControlToValidate="txtDataFim"
                        ErrorMessage="Data de fim deve ser maior ou igual à data de início." Type="Date"
                        ControlToCompare="txtDataInicio" Operator="GreaterThanEqual" Display="Dynamic" Text="*" 
                        ValidationGroup="<%= ValidationGroup %>" />

                    <asp:Label ID="lblAlcance" runat="server" Text="Alcance *" AssociatedControlID="ddlAlcance" />
                    <asp:DropDownList ID="ddlAlcance" runat="server" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlAlcance_SelectedIndexChanged">
                        <asp:ListItem Value="-1">-- Selecione um alcance --</asp:ListItem>
                        <asp:ListItem Value="1">Toda a rede</asp:ListItem>
                        <asp:ListItem Value="3">DRE</asp:ListItem>
                        <asp:ListItem Value="2">Escola</asp:ListItem>
                    </asp:DropDownList>
                    <asp:CompareValidator ID="cvAlcance" runat="server" ErrorMessage="Alcance é obrigatório."
                        ControlToValidate="ddlAlcance" Operator="GreaterThan" ValueToCompare="0" Display="Dynamic" 
                        Text="*" ValidationGroup="<%= ValidationGroup %>" />
                    <uc:UCComboUAEscola ID="UCComboUAEscola" runat="server" MostrarMessageSelecioneUA="true"
                        MostrarMessageSelecioneEscola="true" CarregarEscolaAutomatico="true" ObrigatorioEscola="true" 
                        Visible="false" OnIndexChangedUA="UCComboUAEscola_IndexChangedUA" 
                        ValidationGroup="<%= ValidationGroup %>" />
                </div>
                <div class="right">
                    <asp:Button ID="btnSalvar" runat="server" Text="Incluir novo limite para data de criação de eventos" 
                        CausesValidation="true" OnClick="btnSalvar_Click" ValidationGroup="<%= ValidationGroup %>" />
                    <asp:Button ID="btnVoltar" runat="server" Text="Voltar" CausesValidation="False"
                        OnClick="btnVoltar_Click" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
    <fieldset>
        <legend>Listagem das datas-limite para criação de eventos</legend>
        <asp:UpdatePanel ID="updListagem" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div id="accordion">
                    <asp:Repeater ID="rptLimitesCalendario" runat="server">
                        <ItemTemplate>
                            <h3>
                                <a href="#">
                                    <%# Eval("TipoEventoBimestre")%>
                                </a>
                            </h3>
                            <div>
                                <asp:GridView ID="grvLimitesCalendario" runat="server" AutoGenerateColumns="False" DataSource='<%# Eval("Limites") %>'
                                    DataKeyNames="tev_id, evl_id" OnRowDataBound="grvLimitesCalendario_RowDataBound" 
                                    OnRowDeleting="grvLimitesCalendario_RowDeleting" EmptyDataText="Não existem itens cadastrados.">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Alcance">
                                            <ItemTemplate>
                                                <asp:Literal ID="ltlAlcance" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Vigência">
                                            <ItemTemplate>
                                                <asp:Literal ID="ltlVigencia" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Usuário">
                                            <ItemTemplate>
                                                <asp:Literal ID="ltlUsuario" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Excluir">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnExcluir" CausesValidation="False" runat="server" CommandName="Delete"
                                                    SkinID="btExcluir" ToolTip="Excluir o limite para criação de eventos" />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
</asp:Content>
