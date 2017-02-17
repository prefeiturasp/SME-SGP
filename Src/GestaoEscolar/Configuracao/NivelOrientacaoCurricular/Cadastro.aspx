<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Configuracao.NivelOrientacaoCurricular.Cadastro" %>

<%@ PreviousPageType VirtualPath="~/Configuracao/NivelOrientacaoCurricular/Busca.aspx" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboCalendario.ascx" TagName="UCComboCalendario" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMsg" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMensagem" runat="server" EnableViewState="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>    
    <fieldset>
        <legend>Cadastro de nível de orientação curricular</legend>
        <br /><br />
        <asp:Label ID="lblInformacao" runat="server"></asp:Label>
        <div class="right">
            <asp:Button ID="btnLimparPesquisa" runat="server" Text="Limpar pesquisa" OnClick="btnLimparPesquisa_Click"
                CausesValidation="false" />
        </div>
    </fieldset>
    <fieldset>
        <legend>Resultados</legend>        
        <asp:UpdatePanel ID="updNiveis" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label ID="lblObservacao" runat="server" Visible="false"></asp:Label>
                <asp:Button ID="btnIncluir" runat="server" Text="Incluir novo nível de orientação curricular" 
                    OnClick="btnIncluir_Click" CausesValidation="false" />
                <asp:Label ID="lblMsgGrid" runat="server" CssClass="summary" Visible="false"></asp:Label>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="ValidationNivel" />
                <asp:GridView ID="grvNiveis" runat="server" AutoGenerateColumns="false" DataKeyNames="nvl_id,nvl_situacao,nvl_ordem,ocr_id" 
                    OnRowCommand="grvNiveis_RowCommand" OnRowEditing="grvNiveis_RowEditing" OnRowDataBound="grvNiveis_RowDataBound"
                    OnRowCancelingEdit="grvNiveis_RowCancelingEdit" OnRowUpdating="grvNiveis_RowUpdating">
                    <Columns>
                        <asp:TemplateField HeaderText="Nome" HeaderStyle-Width="80%">
                            <ItemTemplate>
                                <asp:Label ID="lblNomeNivel" runat="server" Text='<%# Bind("nvl_nome") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtNomeNivel" MaxLength="100" runat="server" Text='<%# Bind("nvl_nome") %>' SkinID="text60C"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvNome" runat="server" ControlToValidate="txtNomeNivel"
                                    ErrorMessage="Nome do nível é obrigatório." Display="Dynamic" ValidationGroup="ValidationNivel">*</asp:RequiredFieldValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nome no plural" HeaderStyle-Width="80%">
                            <ItemTemplate>
                                <asp:Label ID="lblNomePlural" runat="server" Text='<%# Bind("nvl_nomePlural") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtNomePlural" MaxLength="100" runat="server" Text='<%# Bind("nvl_nomePlural") %>' SkinID="text60C"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvNomePlural" runat="server" ControlToValidate="txtNomePlural"
                                    ErrorMessage="Nome do nível no plural é obrigatório." Display="Dynamic" ValidationGroup="ValidationNivel">*</asp:RequiredFieldValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ordem">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnSubir" runat="server" CausesValidation="false" CommandName="Subir"
                                    Height="16" Width="16" />
                                <asp:ImageButton ID="btnDescer" runat="server" CausesValidation="false" CommandName="Descer"
                                    Height="16" Width="16" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" /> 
                        </asp:TemplateField>                
                        <asp:TemplateField HeaderText="Editar/Salvar" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="70px">
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="btnAlterar" CausesValidation="false" CommandName="Edit"
                                    SkinID="btEditar" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton ID="btnSalvar" runat="server" CommandName="Update" SkinID="btConfirmar"
                                    ToolTip="Salvar" ValidationGroup="ValidationNivel" />
                                <asp:ImageButton ID="btnCancelar" runat="server" CommandName="Cancel" SkinID="btCancelar"
                                    ToolTip="Cancelar" CausesValidation="false" />
                            </EditItemTemplate>
                            <HeaderStyle CssClass="center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Excluir">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnExcluir" SkinID="btExcluir" runat="server" CommandName="Deletar" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:Button ID="btnIncluir2" runat="server" Text="Incluir novo nível de orientação curricular" 
                    OnClick="btnIncluir_Click" CausesValidation="false" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
</asp:Content>
