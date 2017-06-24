<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Configuracao.DeficienciaDetalhe.Cadastro" 
   %>

<%@ PreviousPageType VirtualPath="~/Configuracao/DeficienciaDetalhe/Busca.aspx" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoDeficiencia.ascx" TagPrefix="uc1" TagName="UCComboTipoDeficiencia" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function ExibirValidacao(vs, grupo) {
            if (!Page_ClientValidate(grupo)) {
                var vsummary = document.getElementById(vs);
                if (vsummary != null) {
                    setTimeout(function () { vsummary.scrollIntoView(true) }, 500);
                }
            }
        }
        function AdicionarDetalhe_Click() {
            ExibirValidacao('<%=vsDetalhe.ClientID%>', 'vgDetalhe');
        }

        function AdicionarFilha_Click() {
            ExibirValidacao('<%=vsFilha.ClientID%>', 'vgFilha');
            }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="DeficienciaDetalhe" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updCadastroQualidade" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset>
                <legend>Cadastro de detalhamento de deficiência</legend>
                <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios4" runat="server" />
                <uc1:UCComboTipoDeficiencia runat="server" ID="UCComboTipoDeficiencia" _MostrarMessageTodas="False" />
                <div>
                    <br />
                </div>
                <div id="divDetalhe" runat="server">
                    <fieldset>
                        <legend>
                            <asp:Label runat="server" ID="lblLegendDetalhe" Text="Detalhamento" /></legend>
                        <asp:Button runat="server" ID="btnNovoDetalhe" Text="Incluir novo detalhe"
                            OnClick="btnNovoDetalhe_Click" CausesValidation="false" />
                        <div>
                            <br />
                        </div>
                        <div id="divInserirDetalhe" visible="false" runat="server">
                            <asp:UpdatePanel runat="server" ID="updMessagePopUpDetalhe" UpdateMode="Always">
                                <ContentTemplate>
                                    <asp:Label ID="lblMessagePopUpDetalhe" runat="server" EnableViewState="False"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdatePanel runat="server" ID="updPopUpDetalhe" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Panel runat="server" DefaultButton="btnAdicionarDetalhe">
                                        <asp:ValidationSummary ID="vsDetalhe" runat="server" ValidationGroup="vgDetalhe" />
                                        <fieldset>
                                            <legend>
                                                <asp:Label runat="server" ID="lblTituloPopUpDetalhe" Text="Adicionar detalhe" />
                                            </legend>
                                            <asp:Label runat="server" ID="lblCampoDetalhe" Text="Detalhe" AssociatedControlID="txtItemDetalhe" />
                                            <asp:TextBox runat="server" ID="txtItemDetalhe" SkinID="text60C" MaxLength="100" ValidationGroup="vgDetalhe" />
                                            <asp:RequiredFieldValidator ID="revtxtItemDetalhe" runat="server" ControlToValidate="txtItemDetalhe" ValidationGroup="vgDetalhe"
                                                Display="Dynamic" ErrorMessage="Descrição do detalhe é obrigatória." Text="*" />
                                            <div class="right">
                                                <asp:Button ID="btnAdicionarDetalhe" runat="server" Text="Adicionar detalhe"
                                                    OnClick="btnAdicionarDetalhe_Click" ValidationGroup="vgDetalhe" OnClientClick="AdicionarDetalhe_Click();" />
                                                <asp:Button ID="btnCancelarItemDetalhe" runat="server" Text="Cancelar detalhe" CausesValidation="false"
                                                    OnClick="btnCancelarItemDetalhe_Click" />
                                            </div>
                                        </fieldset>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <asp:GridView runat="server" ID="grvDetalhes" AutoGenerateColumns="false" AllowPaging="false" AllowSorting="true"
                            DataKeyNames="tde_id, dfd_id, dfd_nome" EmptyDataText="Nenhum detalhamento cadastrado."
                            OnRowDataBound="grv_RowDataBound" OnRowCommand="grvDetalhes_RowCommand" OnSorting="grvDetalhes_Sorting">
                            <Columns>
                                <asp:BoundField DataField="dfd_nome" HeaderText="Detalhe" SortExpression="dfd_nome"/>
                                <asp:TemplateField HeaderText="<%$ Resources:Academico, Sondagem.Cadastro.grvQuestoes.HeaderExcluir %>">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnExcluir" SkinID="btExcluir" runat="server" CommandName="Excluir" CausesValidation="false"
                                            ToolTip="Excluir" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </fieldset>
                </div>

                <div id="divFilha" runat="server" visible="false">
                    <fieldset>
                        <legend>
                            <asp:Label runat="server" ID="lblDefRelacionada" Text="Deficiências relacionadas" /></legend>
                        <asp:Button runat="server" ID="btnNovaFilha" Text="Nova deficiência relacionada"
                            OnClick="btnNovaFilha_Click" CausesValidation="false" />
                        <div>
                            <br />
                        </div>
                                <div id="divInserirFilha" visible="false" runat="server">
                                    <asp:UpdatePanel runat="server" ID="upFilha" UpdateMode="Always">
                                        <ContentTemplate>
                                            <asp:Label ID="lblPopupFilha" runat="server" EnableViewState="False"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>                                   
                                    
                                    <asp:UpdatePanel runat="server" ID="updPopUpFilha" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel runat="server" ID="panelFilha" DefaultButton="btnFilha">
                                                <asp:ValidationSummary ID="vsFilha" runat="server" ValidationGroup="vgFilha" />
                                                <fieldset>
                                                    <legend>
                                                        <asp:Label runat="server" ID="Label3" Text="Adicionar deficiência relacionada" />
                                                    </legend>
                                                    <uc1:UCComboTipoDeficiencia runat="server" ID="UCComboTipoDeficienciaFilha" _MostrarMessageTodas="False" />

                                                    <div class="right">
                                                        <asp:Button ID="btnFilha" runat="server" Text="Adicionar deficiência relacionada"
                                                            OnClick="btnAdicionarFilha_Click" ValidationGroup="vgFilha" OnClientClick="AdicionarFilha_Click();" />
                                                        <asp:Button ID="btnCancelarFilha" runat="server" Text="Cancelar deficiência relacionada" CausesValidation="false"
                                                            OnClick="btnCancelarItemFilha_Click" />
                                                    </div>
                                                </fieldset>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>

                        <asp:GridView runat="server" ID="gdvDeficienciaFilha" AutoGenerateColumns="false" AllowPaging="false" AllowSorting="false"
                            DataKeyNames="tde_id,tde_idFilha, tde_nomeFilha" EmptyDataText="Nenhuma deficiência relacionada cadastrada."
                            OnRowDataBound="grvFilha_RowDataBound" OnRowCommand="grvFilha_RowCommand">
                            <columns>
                                <asp:BoundField DataField="tde_nomeFilha" HeaderText="Deficiência relacionada" />
                                <asp:TemplateField HeaderText="Excluir">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnExcluirFilha" SkinID="btExcluir" runat="server" CommandName="Excluir" CausesValidation="false"
                                            ToolTip="Excluir" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                            </columns>
                        </asp:GridView>
        </fieldset>
                </div>

                <div align="right">
                    <asp:Button ID="bntSalvar" runat="server" Text="Salvar" ValidationGroup="DeficienciaDetalhe" OnClick="bntSalvar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CausesValidation="false"
                        OnClick="btnCancelar_Click" />
                </div>
        </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
