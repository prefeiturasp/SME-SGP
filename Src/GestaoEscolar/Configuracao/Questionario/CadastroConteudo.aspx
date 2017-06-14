<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="CadastroConteudo.aspx.cs" Inherits="GestaoEscolar.Configuracao.Questionario.CadastroConteudo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="_updConteudos" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset>
                    <legend>Conteúdos</legend>
                    <asp:GridView ID="_grvConteudos" runat="server" AutoGenerateColumns="False"
                        EmptyDataText="Não existem conteúdos cadastrados." OnDataBinding="_grvConteudos_DataBinding"
                        OnRowDataBound="_grvConteudos_RowDataBound" OnRowEditing="_grvConteudos_RowEditing"
                        OnRowUpdating="_grvConteudos_RowUpdating" OnRowDeleting="_grvConteudos_RowDeleting"
                        OnRowCancelingEdit="_grvConteudos_RowCancelingEdit">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTexto" runat="server" Text="Texto"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    &nbsp;
                                <asp:Label ID="_lblTexto" runat="server" Text="" CssClass="wrap150px"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    &nbsp;
                                <asp:TextBox ID="_txtTexto" runat="server" CssClass="wrap150px" SkinID="text30C"></asp:TextBox>
                                </EditItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTipoConteudo" runat="server" Text="Tipo de conteúdo"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    &nbsp;
                                <asp:Label ID="_lblTipoConteudo" runat="server" Text="" CssClass="wrap150px"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    &nbsp;
                                <asp:DropDownList ID="_ddlTipoConteudo" runat="server" CssClass="wrap150px">
                                    <asp:ListItem Text="-- Selecione --" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Título 1" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Título 2" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Texto" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Pergunta" Value="4"></asp:ListItem>
                                </asp:DropDownList>
                                </EditItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTipoResposta" runat="server" Text="Tipo de resposta"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    &nbsp;
                                <asp:Label ID="_lblTipoResposta" runat="server" Text="" CssClass="wrap150px"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    &nbsp;
                                <asp:DropDownList ID="_ddlTipoResposta" runat="server" CssClass="wrap150px" Enabled="false">
                                    <asp:ListItem Text="-- Selecione --" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Múltipla seleção" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Seleção única" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Texto aberto" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblRespostas" runat="server" Text="Respostas"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:UpdatePanel ID="_updRespostas" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>                                            
                                                <%-- Repeater de respostas --%>                                            
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:Button ID="_btnNovaResposta" runat="server" CausesValidation="False" Text="Incluir nova resposta"
                                        OnClick="_btnNovaResposta_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Editar" HeaderStyle-CssClass="center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="_imgEditar" runat="server" CommandName="Edit" SkinID="btEditar"
                                        ToolTip="Editar" CausesValidation="false" />
                                    <asp:ImageButton ID="_imgCancelar" runat="server" CommandName="Cancel" SkinID="btDesfazer"
                                        ToolTip="Cancelar edição" CausesValidation="false" Visible="false" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Salvar" HeaderStyle-CssClass="center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="_imgSalvar" runat="server" CommandName="Update" SkinID="btConfirmar"
                                        ToolTip="Salvar" ValidationGroup='Questionario'
                                        Visible="false" />
                                    <asp:ImageButton ID="_imgCancelar" runat="server" CommandName="Cancel" SkinID="btCancelar"
                                        ToolTip="Cancelar" CausesValidation="false"
                                        Visible="false" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Excluir" HeaderStyle-CssClass="center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="_imgExcluir" runat="server" CommandName="Delete" SkinID="btExcluir"
                                        ToolTip="Excluir" CausesValidation="false" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </fieldset>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="_btnNovoConteudo" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:Button ID="_btnNovoConteudo" runat="server" CausesValidation="False" Text="Incluir novo conteúdo"
            OnClick="_btnNovoConteudo_Click" />
</asp:Content>
