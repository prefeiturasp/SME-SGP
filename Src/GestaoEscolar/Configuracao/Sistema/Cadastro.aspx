<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Configuracao_Sistema_Cadastro" CodeBehind="Cadastro.aspx.cs" %>

<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Conditional" EnableViewState="False">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Config" />
    <fieldset>
        <legend>Configurações do sistema</legend>
        <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" />
        <asp:Button ID="btnNovo" runat="server" Text="Incluir nova configuração do sistema"
            CausesValidation="false" OnClick="btnNovo_Click" />
        <asp:UpdatePanel ID="updConfig" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:UCLoader ID="UCLoader1" runat="server" AssociatedUpdatePanelID="updConfig" />
                <asp:GridView ID="grvConfig" runat="server" AutoGenerateColumns="False" DataKeyNames="cfg_id, cfg_situacao, IsNew"
                    EmptyDataText="Não existem configurações cadastradas." OnRowEditing="grvConfig_RowEditing"
                    OnDataBinding="grvConfig_DataBinding" OnRowDeleting="grvConfig_RowDeleting" OnRowUpdating="grvConfig_RowUpdating"
                    OnRowDataBound="grvConfig_RowDataBound" OnRowCancelingEdit="grvConfig_RowCancelingEdit">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbl" runat="server" Text="Chave *" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblChave" runat="server" Text='<%# Bind("cfg_chave") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtChave" runat="server" Text='<%# Bind("cfg_chave") %>' SkinID="text30C"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvChave" runat="server" ErrorMessage="Chave é obrigatório."
                                    ControlToValidate="txtChave" ValidationGroup="Config">*</asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <HeaderStyle Width="320px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Descrição">
                            <ItemTemplate>
                                <asp:Label ID="lblDescricao" runat="server" Text='<%# Bind("cfg_descricao") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDescricao" runat="server" Text='<%# Bind("cfg_descricao") %>'
                                    SkinID="text30C"></asp:TextBox>
                            </EditItemTemplate>
                            <HeaderStyle Width="320px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbl" runat="server" Text="Valor *" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblValor" runat="server" Text='<%# Bind("cfg_valor") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtValor" runat="server" Text='<%# Bind("cfg_valor") %>' SkinID="text30C"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvValor" runat="server" ErrorMessage="Valor é obrigatório."
                                    ControlToValidate="txtValor" ValidationGroup="Config">*</asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <HeaderStyle Width="320px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Editar" HeaderStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEditar" runat="server" CommandName="Edit" SkinID="btEditar"
                                    ToolTip="Editar configuração" CausesValidation="false" />
                                <asp:ImageButton ID="imgCancelar" runat="server" CommandName="Cancel" SkinID="btDesfazer"
                                    ToolTip="Cancelar edição" CausesValidation="false" Visible="false" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Salvar" HeaderStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgSalvar" runat="server" CommandName="Update" SkinID="btConfirmar"
                                    ToolTip="Salvar nova configuração" ValidationGroup="Config" Visible="false" />
                                <asp:ImageButton ID="imgCancelarConfg" runat="server" CommandName="Cancel" SkinID="btCancelar"
                                    ToolTip="Cancelar nova configuração" CausesValidation="false" Visible="false" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Excluir" HeaderStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgExcluir" runat="server" CommandName="Delete" SkinID="btExcluir"
                                    ToolTip="Excluir configuração" CausesValidation="false" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnNovo" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </fieldset>
    <fieldset>
        <legend>Atualiza cache do menu</legend>
        <asp:Button ID="btnAtualizaCacheMenu" runat="server" Text="Atualizar cache do menu" OnClick="btnAtualizaCacheMenu_Click" />
    </fieldset>
</asp:Content>
