<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Configuracao_ParametroIntegracao_Cadastro" CodeBehind="Cadastro.aspx.cs" %>

<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="_updMessage" runat="server" UpdateMode="Conditional" EnableViewState="False">
        <ContentTemplate>
            <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="_ValidationSummary1" runat="server" ValidationGroup="Integracao" />
    <fieldset>
        <legend>Parâmetros de integração</legend>
        <uc1:UCCamposObrigatorios ID="_UCCamposObrigatorios1" runat="server" />
        <asp:Button ID="_btnNovo" runat="server" Text="Incluir novo parâmetro de integração"
            CausesValidation="false" OnClick="_btnNovo_Click" />
        <asp:UpdatePanel ID="_updParametroIntegracao" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:UCLoader ID="_UCLoader1" runat="server" AssociatedUpdatePanelID="_updParametroIntegracao" />
                <asp:GridView ID="_grvParametroIntegracao" runat="server" AutoGenerateColumns="False"
                    EmptyDataText="Não existem parâmetros de integração cadastrados." DataKeyNames="pri_id, pri_situacao, IsNew"
                    OnDataBinding="_grvParametroIntegracao_DataBinding" OnRowDataBound="_grvParametroIntegracao_RowDataBound"
                    OnRowEditing="_grvParametroIntegracao_RowEditing" OnRowUpdating="_grvParametroIntegracao_RowUpdating"
                    OnRowDeleting="_grvParametroIntegracao_RowDeleting" OnRowCancelingEdit="_grvParametroIntegracao_RowCancelingEdit">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbl1" runat="server" Text="Chave *" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="_lblChave" runat="server" Text='<%# Bind("pri_chave") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="_txtChave" runat="server" Text='<%# Bind("pri_chave") %>' MaxLength="100"
                                    SkinID="text30C"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="_rfvChave" runat="server" ErrorMessage="Chave é obrigatório."
                                    ControlToValidate="_txtChave" ValidationGroup="Integracao">*</asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <HeaderStyle Width="300px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Descrição">
                            <ItemTemplate>
                                <asp:Label ID="_lblDescricao" runat="server" Text='<%# Bind("pri_descricao") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="_txtDescricao" runat="server" Text='<%# Bind("pri_descricao") %>'
                                    MaxLength="200" SkinID="text30C"></asp:TextBox>
                            </EditItemTemplate>
                            <HeaderStyle Width="300px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Valor *">
                            <HeaderTemplate>
                                <asp:Label ID="lbl1" runat="server" Text="Valor *" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="_lblValor" runat="server" Text='<%# Bind("pri_valor") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="_txtValor" runat="server" Text='<%# Bind("pri_valor") %>' MaxLength="1000"
                                    SkinID="text30C"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="_rfvValor" runat="server" ErrorMessage="Valor é obrigatório."
                                    ControlToValidate="_txtValor" ValidationGroup="Integracao">*</asp:RequiredFieldValidator>
                                <asp:DropDownList ID="_ddlValor" runat="server" Visible="false">
                                    <asp:ListItem Value="1" Text="Sim"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Não"></asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <HeaderStyle Width="300px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Editar" HeaderStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:ImageButton ID="_imgEditar" runat="server" CommandName="Edit" SkinID="btEditar"
                                    ToolTip="Editar parâmetro de integração" CausesValidation="false" />
                                <asp:ImageButton ID="_imgCancelar" runat="server" CommandName="Cancel" SkinID="btDesfazer"
                                    ToolTip="Cancelar edição" CausesValidation="false" Visible="false" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Salvar" HeaderStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:ImageButton ID="_imgSalvar" runat="server" CommandName="Update" SkinID="btConfirmar"
                                    ToolTip="Salvar novo parâmetro de integração" ValidationGroup="Integracao" Visible="false" />
                                <asp:ImageButton ID="_imgCancelarParametroIntegracao" runat="server" CommandName="Cancel"
                                    SkinID="btCancelar" ToolTip="Cancelar novo parâmetro de integração" CausesValidation="false"
                                    Visible="false" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Excluir" HeaderStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:ImageButton ID="_imgExcluir" runat="server" CommandName="Delete" SkinID="btExcluir"
                                    ToolTip="Excluir parâmetro de integração" CausesValidation="false" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="_btnNovo" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </fieldset>
</asp:Content>
