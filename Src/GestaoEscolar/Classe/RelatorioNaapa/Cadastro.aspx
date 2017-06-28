<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Classe.RelatorioNaapa.Cadastro" %>

<%@ PreviousPageType VirtualPath="~/Classe/RelatorioNaapa/Busca.aspx" %>

<%@ Register Src="~/WebControls/Combos/UCComboRelatorioAtendimento.ascx" TagName="UCCRelatorioAtendimento" TagPrefix="uc" %>
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
        <asp:Panel ID="pnlFiltros" runat="server" GroupingText="<%$ Resources:Classe, RelatorioNaapa.Cadastro.pnlFiltros.GroupingText %>">
            <uc:UCCRelatorioAtendimento ID="UCCRelatorioAtendimento" runat="server" MostrarMensagemSelecione="true" />
            <div class="right">
                <asp:Button ID="btnVoltar" runat="server" Text="<%$ Resources:Padrao, Padrao.Voltar.Text %>" CausesValidation="false" OnClick="btnVoltar_Click" />
                <asp:Button ID="btnLimparBusca" runat="server" Text="<%$ Resources:Padrao, Padrao.LimparPesquisa.Text %>" OnClick="btnLimparBusca_Click" Visible="false" />
                <asp:Button ID="btnNovo" runat="server" Text="<%$ Resources:Classe, RelatorioNaapa.Cadastro.btnNovo.Text %>" CausesValidation="false" OnClick="btnNovo_Click" />
            </div>
        </asp:Panel>
        <fieldset id="fdsLancamento" runat="server">
            <legend><asp:Literal runat="server" ID="litLancamento" Text="<%$ Resources:Classe, RelatorioNaapa.Cadastro.pnlFiltros.GroupingText %>"></asp:Literal></legend>
            <asp:GridView ID="grvLancamentos" runat="server" AutoGenerateColumns="false"
                EmptyDataText="<%$ Resources:Classe, RelatorioNaapa.Cadastro.grvLancamentos.EmptyDataText %>"
                DataKeyNames="reap_id" OnRowCommand="grvLancamentos_RowCommand" OnRowDataBound="grvLancamentos_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="reap_descricao" HeaderText="<%$ Resources:Classe, RelatorioNaapa.Cadastro.grvLancamentos.ColunaDescricao %>"></asp:BoundField>
                    <asp:TemplateField HeaderText="<%$ Resources:Padrao, Padrao.Detalhar.Text %>" HeaderStyle-Width="100">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnDetalhar" runat="server" CommandName="Detalhar" SkinID="btDetalhar" CausesValidation="False" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Padrao, Padrao.Alterar.Text %>" HeaderStyle-Width="100">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnAlterar" runat="server" CommandName="Alterar" SkinID="btEditar" CausesValidation="False" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Padrao, Padrao.Excluir.Text %>" HeaderStyle-Width="100">
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
                    <asp:Button ID="btnSalvar" runat="server" Text="<%$ Resources:Padrao, Padrao.Salvar.Text %>" CausesValidation="false" OnClick="btnSalvar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="<%$ Resources:Padrao, Padrao.Cancelar.Text %>" CausesValidation="false" OnClick="btnCancelar_Click" />
                </div><br />
                <uc:UCLancamentoRelatorioAtendimento ID="UCLancamentoRelatorioAtendimento" runat="server" />        
                <div class="right">
                    <asp:Button ID="btnSalvarBaixo" runat="server" Text="<%$ Resources:Padrao, Padrao.Salvar.Text %>" CausesValidation="false" OnClick="btnSalvar_Click" />
                    <asp:Button ID="btnCancelarBaixo" runat="server" Text="<%$ Resources:Padrao, Padrao.Cancelar.Text %>" CausesValidation="false" OnClick="btnCancelar_Click" />
                </div>
            </asp:Panel><br />
            <div class="right">
                <asp:Button ID="btnVoltarBaixo" runat="server" Text="<%$ Resources:Padrao, Padrao.Voltar.Text %>" CausesValidation="false" OnClick="btnVoltar_Click" />
            </div>
        </fieldset>
    </div>
</asp:Content>
