<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Configuracao.Questionario.Cadastro"
    ValidateRequest="false" %>

<%@ Register Src="../../WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc1" %>
<%@ Register Src="../../WebControls/Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="_updMessage" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Questionario" />
    <fieldset>
        <legend>Questionário</legend>
        <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" />
        <asp:UpdatePanel ID="_updQuestionario" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:UCLoader ID="UCLoader1" runat="server" />
                <asp:Label ID="_lblTitulo" runat="server" Text="Título do questionário *"></asp:Label>
                <asp:TextBox ID="_txtTitulo" runat="server" CssClass="wrap150px" SkinID="text30C"></asp:TextBox>
                <asp:RequiredFieldValidator ID="_rfvTitulo" runat="server" ErrorMessage="Título é obrigatório."
                    ControlToValidate="_txtTitulo" ValidationGroup="Questionario">*</asp:RequiredFieldValidator>
                <asp:GridView ID="_grvQuestionario" runat="server" AutoGenerateColumns="False"
                    EmptyDataText="Não existem ? cadastrados." OnDataBinding="_grvQuestionario_DataBinding"
                    OnRowDataBound="_grvQuestionario_RowDataBound" OnRowEditing="_grvQuestionario_RowEditing"
                    OnRowUpdating="_grvQuestionario_RowUpdating" OnRowDeleting="_grvQuestionario_RowDeleting"
                    OnRowCancelingEdit="_grvQuestionario_RowCancelingEdit">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbl1" runat="server" Text="Texto"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                &nbsp;
                                <asp:Label ID="_lblTexto" runat="server" Text='<%#Bind("Texto")%>' CssClass="wrap150px"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                &nbsp;
                                <asp:TextBox ID="_txtTexto" runat="server" CssClass="wrap150px" SkinID="text30C"></asp:TextBox>
                            </EditItemTemplate>
                            <HeaderStyle />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbl1" runat="server" Text="Tipo de conteúdo"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                &nbsp;
                                <asp:Label ID="_lblTipoConteudo" runat="server" Text='<%#Bind("qtc_tipo")%>' CssClass="wrap150px"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                &nbsp;
                                <asp:DropDownList ID="_ddlTipoConteudo" runat="server" SelectedValue='<%#Bind("qtc_tipo")%>' CssClass="wrap150px">
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
                                <asp:Label ID="lbl1" runat="server" Text="Tipo de resposta"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                &nbsp;
                                <asp:Label ID="_lblTipoResposta" runat="server" Text='<%#Bind("TipoResposta")%>' CssClass="wrap150px"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                &nbsp;
                                <asp:RadioButtonList ID="_rblTipoResposta" runat="server" Text='<%#Bind("trp_id")%>' CssClass="wrap150px">
                                    <asp:ListItem Text="Múltipla seleção" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Seleção única" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Texto aberto" Value="3"></asp:ListItem>
                                </asp:RadioButtonList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbl1" runat="server" Text="Respostas"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%-- Repeater de respostas --%>
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
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="_btnNovo" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:Button ID="_btnNovo" runat="server" CausesValidation="False" Text="Incluir novo ?"
            OnClick="_btnNovo_Click" />
    </fieldset>
</asp:Content>
