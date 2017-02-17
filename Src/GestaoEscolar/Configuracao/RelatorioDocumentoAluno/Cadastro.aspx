<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Configuracao.RelatorioDocumentoAluno.Cadastro" %>
<%@ Register src="../../WebControls/Mensagens/UCCamposObrigatorios.ascx" tagname="UCCamposObrigatorios" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Conditional" EnableViewState="False">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="DocumentoAluno" />
    <fieldset>
        <legend>Cadastro de documentos do aluno</legend>
        <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" />
        <asp:Button ID="btnNovo" runat="server" Text="Incluir novo documento do aluno" OnClick="btnNovo_Click" />
        <asp:UpdatePanel ID="updDocumentos" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:GridView ID="grvDocumentos" runat="server" AutoGenerateColumns="False" EmptyDataText="Não existem documentos cadastrados."
                    OnDataBinding="grvDocumentos_DataBinding" OnRowCancelingEdit="grvDocumentos_RowCancelingEdit"
                    OnRowDeleting="grvDocumentos_RowDeleting" OnRowEditing="grvDocumentos_RowEditing"
                    OnRowUpdating="grvDocumentos_RowUpdating" 
                    DataKeyNames="rlt_id,rda_id,rda_situacao">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblHeader" runat="server" Text="Relatório *" />
                            </HeaderTemplate>
                           <ItemTemplate>
                                <asp:Label ID="lblRelatorio" runat="server" Text='<%# Bind("rlt_nome") %>'></asp:Label>
                            </ItemTemplate>
                             <EditItemTemplate>
                                 <asp:DropDownList ID="ddlRelatorios" runat="server">
                                 </asp:DropDownList>
                                 <asp:CompareValidator ID="cpvRelatorios" runat="server" ErrorMessage="Relatório é obrigatório."
                                     ControlToValidate="ddlRelatorios" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic"
                                     ValidationGroup="DocumentoAluno">*</asp:CompareValidator>
                             </EditItemTemplate>
                            <HeaderStyle Width="450px" />
                        </asp:TemplateField>
                         <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblHeader" runat="server" Text="Nome do documento *" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblNome" runat="server" Text='<%# Bind("rda_nomeDocumento") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtNome" runat="server" Text='<%# Bind("rda_nomeDocumento") %>' SkinID="text60C"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvNome" runat="server" ErrorMessage="Nome do documento é obrigatório."
                                    ControlToValidate="txtNome" ValidationGroup="DocumentoAluno">*</asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <HeaderStyle Width="550px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblHeader" runat="server" Text="Ordem do documento *" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblOrdem" runat="server" Text='<%# Bind("rda_ordem") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtOrdem" runat="server" Text='<%# Bind("rda_ordem") %>' SkinID="text15C"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvOrdem" runat="server" ErrorMessage="Ordem do documento é obrigatório."
                                    ControlToValidate="txtOrdem" ValidationGroup="DocumentoAluno">*</asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <HeaderStyle Width="160px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Editar" HeaderStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEditar" runat="server" CommandName="Edit" SkinID="btEditar"
                                    ToolTip="Editar documento" CausesValidation="false" />
                                <asp:ImageButton ID="imgCancelar" runat="server" CommandName="Cancel" SkinID="btDesfazer"
                                    ToolTip="Cancelar edição" CausesValidation="false" Visible="false" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Salvar" HeaderStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgSalvar" runat="server" CommandName="Update" SkinID="btConfirmar"
                                    ToolTip="Salvar novo documento" ValidationGroup="DocumentoAluno" Visible="false" />
                                <asp:ImageButton ID="imgCancelarDoc" runat="server" CommandName="Cancel" SkinID="btCancelar"
                                    ToolTip="Cancelar novo documento" CausesValidation="false" Visible="false" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Excluir" HeaderStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgExcluir" runat="server" CommandName="Delete" SkinID="btExcluir"
                                    ToolTip="Excluir documento" CausesValidation="false" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
</asp:Content>
