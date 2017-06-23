<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Classe.RelatorioRecuperacaoParalela.Cadastro" %>

<%@ Register Src="~/WebControls/Combos/UCComboRelatorioAtendimento.ascx" TagName="UCCRelatorioAtendimento" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCPeriodoCalendario.ascx" TagName="UCCPeriodoCalendario" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/LancamentoRelatorioAtendimento/UCLancamentoRelatorioAtendimento.ascx" TagName="UCLancamentoRelatorioAtendimento" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="rel-atendimento">
        <asp:UpdatePanel ID="updMensagem" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:Label ID="lblMensagem" runat="server"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Panel ID="pnlFiltros" runat="server" GroupingText="<%$ Resources:Classe, RelatorioRecuperacaoParalela.Cadastro.pnlFiltros.GroupingText %>">
            <!-- Exibe as disciplinas eletivas relacionadas -->
            <asp:Label ID="lblDisciplina" runat="server" Text="<%$ Resources:Mensagens, MSG_DISCIPLINA %>" AssociatedControlID="ddlDisciplina"></asp:Label>
            <asp:DropDownList ID="ddlDisciplina" runat="server" AppendDataBoundItems="True" AutoPostBack="true" 
                DataTextField="tds_nome" DataValueField="tds_id" SkinID="text60C" OnSelectedIndexChanged="ddlDisciplina_SelectedIndexChanged">
            </asp:DropDownList>
            <uc:UCCRelatorioAtendimento ID="UCCRelatorioAtendimento" runat="server" MostrarMensagemSelecione="true" />
            <uc:UCCPeriodoCalendario ID="UCCPeriodoCalendario" runat="server" MostrarMensagemSelecione="true" Visible="false" />
            <div class="right" runat="server" visible="false" id="divBotoes">
                <asp:Button ID="btnNovo" runat="server" Text="<%$ Resources:Classe, RelatorioRecuperacaoParalela.Cadastro.btnNovo.Text %>" CausesValidation="false" OnClick="btnNovo_Click" />
            </div>
        </asp:Panel>
        <asp:UpdatePanel ID="updLancamento" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
            <ContentTemplate>
                <!-- Utilizado em relatórios que permitem mais de um lançamento -->
                <asp:GridView ID="grvLancamentos" runat="server" AutoGenerateColumns="false"
                    EmptyDataText="<%$ Resources:Classe, RelatorioRecuperacaoParalela.Cadastro.grvLancamentos.EmptyDataText %>"
                    DataKeyNames="reap_id" OnRowCommand="grvLancamentos_RowCommand" OnRowDataBound="grvLancamentos_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="reap_descricao" HeaderText=""></asp:BoundField>
                        <asp:TemplateField HeaderText="Alterar">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnAlterar" runat="server" CommandName="Alterar" SkinID="btEditar" CausesValidation="False" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Excluir">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnExcluir" runat="server" CommandName="Deletar" SkinID="btExcluir" CausesValidation="False" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>    
                <asp:Panel id="pnlLancamento" runat="server" Visible="false">
                    <div class="right area-botoes-top" style="border-bottom-right-radius: 0px; border-bottom-left-radius: 0px;">
                        <asp:Button ID="btnSalvar" runat="server" Text="<%$ Resources:Classe, RelatorioRecuperacaoParalela.Cadastro.btnSalvar.Text %>" CausesValidation="false" OnClick="btnSalvar_Click" />
                        <asp:Button ID="btnCancelar" runat="server" Text="<%$ Resources:Classe, RelatorioRecuperacaoParalela.Cadastro.btnCancelar.Text %>" CausesValidation="false" OnClick="btnCancelar_Click" />
                    </div>
                    <uc:UCLancamentoRelatorioAtendimento ID="UCLancamentoRelatorioAtendimento" runat="server" />        
                    <div class="right">
                        <asp:Button ID="btnSalvarBaixo" runat="server" Text="<%$ Resources:Classe, RelatorioRecuperacaoParalela.Cadastro.btnSalvar.Text %>" CausesValidation="false" OnClick="btnSalvar_Click" />
                        <asp:Button ID="btnCancelarBaixo" runat="server" Text="<%$ Resources:Classe, RelatorioRecuperacaoParalela.Cadastro.btnCancelar.Text %>" CausesValidation="false" OnClick="btnCancelar_Click" />
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
