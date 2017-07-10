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
            <asp:Label ID="lblDisciplina" runat="server" Text="<%$ Resources:Classe, RelatorioRecuperacaoParalela.Cadastro.lblDisciplina.Text %>" AssociatedControlID="ddlDisciplina"></asp:Label>
            <asp:DropDownList ID="ddlDisciplina" runat="server" AppendDataBoundItems="True" AutoPostBack="true" 
                DataTextField="tud_nome" DataValueField="tur_tud_id" SkinID="text60C" OnSelectedIndexChanged="ddlDisciplina_SelectedIndexChanged">
            </asp:DropDownList>
            <uc:UCCRelatorioAtendimento ID="UCCRelatorioAtendimento" runat="server" MostrarMensagemSelecione="true" />
            <uc:UCCPeriodoCalendario ID="UCCPeriodoCalendario" runat="server" MostrarMensagemSelecione="true" Visible="false" />
            <div class="right">
                <asp:Button ID="btnVoltar" runat="server" Text="<%$ Resources:Classe, RelatorioRecuperacaoParalela.Cadastro.btnVoltar.Text %>" CausesValidation="false" OnClick="btnVoltar_Click" />
                <asp:Button ID="btnLimparBusca" runat="server" Text="<%$ Resources:Padrao, Padrao.LimparPesquisa.Text %>" OnClick="btnLimparBusca_Click" Visible="false" />
                <asp:Button ID="btnNovo" runat="server" Text="<%$ Resources:Classe, RelatorioRecuperacaoParalela.Cadastro.btnNovo.Text %>" CausesValidation="false" OnClick="btnNovo_Click" />
            </div>
        </asp:Panel>
        <fieldset id="fdsLancamento" runat="server">
            <legend><asp:Literal runat="server" ID="litLancamento" Text="<%$ Resources:Classe, RelatorioRecuperacaoParalela.Cadastro.litLancamento.Text %>"></asp:Literal></legend>
            <!-- Utilizado em relatórios que permitem mais de um lançamento -->
            <asp:GridView ID="grvLancamentos" runat="server" AutoGenerateColumns="false"
                EmptyDataText="<%$ Resources:Classe, RelatorioRecuperacaoParalela.Cadastro.grvLancamentos.EmptyDataText %>"
                DataKeyNames="reap_id" OnRowCommand="grvLancamentos_RowCommand" OnRowDataBound="grvLancamentos_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="reap_descricao" HeaderText="<%$ Resources:Classe, RelatorioRecuperacaoParalela.Cadastro.grvLancamentos.ColunaDescricao %>"></asp:BoundField>
                    <asp:TemplateField HeaderText="Detalhar" HeaderStyle-Width="100">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnDetalhar" runat="server" CommandName="Detalhar" SkinID="btDetalhar" CausesValidation="False" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Alterar" HeaderStyle-Width="100">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnAlterar" runat="server" CommandName="Alterar" SkinID="btEditar" CausesValidation="False" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Excluir" HeaderStyle-Width="100">
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
                </div><br />
                <uc:UCLancamentoRelatorioAtendimento ID="UCLancamentoRelatorioAtendimento" runat="server" />        
                <div class="right">
                    <asp:Button ID="btnSalvarBaixo" runat="server" Text="<%$ Resources:Classe, RelatorioRecuperacaoParalela.Cadastro.btnSalvar.Text %>" CausesValidation="false" OnClick="btnSalvar_Click" />
                    <asp:Button ID="btnCancelarBaixo" runat="server" Text="<%$ Resources:Classe, RelatorioRecuperacaoParalela.Cadastro.btnCancelar.Text %>" CausesValidation="false" OnClick="btnCancelar_Click" />
                </div>
            </asp:Panel><br />
            <div class="right">
                <asp:Button ID="btnVoltarBaixo" runat="server" Text="<%$ Resources:Classe, RelatorioRecuperacaoParalela.Cadastro.btnVoltar.Text %>" CausesValidation="false" OnClick="btnVoltar_Click" />
            </div>
        </fieldset>
    </div>
</asp:Content>
