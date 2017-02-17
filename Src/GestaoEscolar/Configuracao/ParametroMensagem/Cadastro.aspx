<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Configuracao.ParametroMensagem.Cadastro" 
ValidateRequest="false"%>

<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="_updMessage" runat="server" UpdateMode="Conditional" EnableViewState="False">
        <ContentTemplate>
            <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="_ValidationSummary1" runat="server" ValidationGroup='<%=validationGroup %>' />
    <fieldset>
        <legend>Parâmetros de mensagens do sistema</legend>
        <uc1:UCCamposObrigatorios ID="_UCCamposObrigatorios1" runat="server" />
        <asp:Button ID="_btnNovo" runat="server" Text="Incluir novo parâmetro de mensagem"
            CausesValidation="false" OnClick="_btnNovo_Click" />
        <asp:UpdatePanel ID="_updParametro" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <uc2:UCLoader ID="_UCLoader1" runat="server" AssociatedUpdatePanelID="_updParametro" />
                <asp:GridView ID="grvParametro" runat="server" AutoGenerateColumns="False"
                    EmptyDataText="Não existem parâmetros de mensagem cadastrados." DataKeyNames="pms_id, pms_situacao, IsNew"
                    OnDataBinding="grvParametro_DataBinding" OnRowDataBound="grvParametro_RowDataBound"
                    OnRowEditing="grvParametro_RowEditing" OnRowUpdating="grvParametro_RowUpdating"
                    OnRowDeleting="grvParametro_RowDeleting" OnRowCancelingEdit="grvParametro_RowCancelingEdit">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbl1" runat="server" Text="Chave *" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="_lblChave" runat="server" 
                                    Text='<%# Bind("pms_chave") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="_txtChave" runat="server" Text='<%# Bind("pms_chave") %>' MaxLength="100"
                                    SkinID="text30C"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="_rfvChave" runat="server" ErrorMessage="Chave é obrigatório."
                                    ControlToValidate="_txtChave" ValidationGroup='<%=validationGroup %>'>*</asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <HeaderStyle Width="300px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Descrição">
                            <ItemTemplate>
                                <asp:Label ID="_lblDescricao" runat="server" Text='<%# Bind("pms_descricao") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="_txtDescricao" runat="server" Text='<%# Bind("pms_descricao") %>'
                                    MaxLength="200" SkinID="text15C"></asp:TextBox>
                            </EditItemTemplate>
                            <HeaderStyle Width="250px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Valor *">
                            <HeaderTemplate>
                                <asp:Label ID="lbl1" runat="server" Text="Valor *" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="_lblValor" runat="server" Text='<%# Bind("pms_valor") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="_txtValor" runat="server" 
                                    Text='<%# Bind("pms_valor") %>' MaxLength="2000"
                                    TextMode="MultiLine" Rows="5" Columns="10"
                                    SkinID="text30C" Width="220px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="_rfvValor" runat="server" ErrorMessage="Valor é obrigatório."
                                    ControlToValidate="_txtValor" ValidationGroup='<%=validationGroup %>'>*</asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <HeaderStyle Width="250px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Editar" HeaderStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:ImageButton ID="_imgEditar" runat="server" CommandName="Edit" SkinID="btEditar"
                                    ToolTip="Editar parâmetro de mensagem" CausesValidation="false" />
                                <asp:ImageButton ID="_imgCancelar" runat="server" CommandName="Cancel" SkinID="btDesfazer"
                                    ToolTip="Cancelar edição" CausesValidation="false" Visible="false" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Salvar" HeaderStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:ImageButton ID="_imgSalvar" runat="server" CommandName="Update" SkinID="btConfirmar"
                                    ToolTip="Salvar parâmetro de mensagem" ValidationGroup='<%=validationGroup %>' Visible="false" />
                                <asp:ImageButton ID="_imgCancelarParametro" runat="server" CommandName="Cancel"
                                    SkinID="btCancelar" ToolTip="Cancelar novo parâmetro de mensagem" CausesValidation="false"
                                    Visible="false" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Excluir" HeaderStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:ImageButton ID="_imgExcluir" runat="server" CommandName="Delete" SkinID="btExcluir"
                                    ToolTip="Excluir parâmetro de mensagem" CausesValidation="false" />
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
