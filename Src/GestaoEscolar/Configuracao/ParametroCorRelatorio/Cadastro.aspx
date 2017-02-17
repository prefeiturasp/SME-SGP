<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Configuracao_ParametroCorRelatorio_Cadastro" CodeBehind="Cadastro.aspx.cs" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc1" %>
<%@ Register Src="../../WebControls/Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc2" %>
<%@ Register Src="../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMensagem" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="<%=validationGroup %>" />
    <fieldset>
        <%--<legend>Cadastro de cor para relatórios: <asp:Label ID="lblVariavel" runat="server" Font-Bold="True" Font-Size="Medium" Font-Overline="False" Font-Underline="True"></asp:Label></legend>--%>
        <legend>Cadastro de cor para relatórios </legend>
        <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" /> 
        <asp:Label ID="lblVariavel" runat="server" Font-Bold="True" Font-Size="Medium" 
            Font-Overline="False" Font-Underline="False"></asp:Label>
        <br /><br /><br /><br />
                      
        <asp:Button ID="btnNovaCor" runat="server" CausesValidation="False" Text="Incluir nova cor por relatório" OnClick="btnNovaCor_Click" 
            ToolTip="Incluir nova cor" />  
        <div style="text-align: right;">
            <asp:Button ID="btnVoltar" runat="server" CausesValidation="False" Text="Voltar" OnClick="btnVoltar_Click" 
                ToolTip="Voltar para página anterior" />  
        </div>
                <asp:GridView ID="grvCadastroCor" runat="server" AutoGenerateColumns="False"
                    DataKeyNames="rlt_id, cor_id, cor_situacao, cor_ordem, IsNew"
                    OnRowDataBound="grvCadastroCor_RowDataBound"
                    OnRowCommand="grvCadastroCor_RowCommand"
                    OnDataBound="grvCadastroCor_DataBound"
                    OnRowEditing="grvCadastroCor_RowEditing"
                    OnRowUpdating="grvCadastroCor_RowUpdating"
                    OnRowDeleting="grvCadastroCor_RowDeleting"
                    OnRowCancelingEdit="grvCadastroCor_RowCancelingEdit">
                    <Columns>                                                  
                        <%-- Cores do relatorio --%>
                        <asp:TemplateField HeaderText="Descrição *">
                            <HeaderTemplate>
                                <asp:Label ID="lblCorPaleta" runat="server" Text="Cor *" />
                            </HeaderTemplate> 
                            <ItemTemplate>        
                                <asp:TextBox ID="txtCorPaleta" runat="server" Text='<%# Bind("cor_corPaleta") %>' MaxLength="200" class="colorInput color {hash:true}">
                                </asp:TextBox>                
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtCorPaleta" runat="server" Text='<%# Bind("cor_corPaleta") %>' MaxLength="200" class="colorInput color {hash:true}">
                                </asp:TextBox>                                                              
                            </EditItemTemplate>
                            <HeaderStyle CssClass="Left" Width="200px" />
                        </asp:TemplateField>
                        <%-- Ordem --%>
                        <asp:TemplateField HeaderText="Ordem">
                            <ItemTemplate>                                
                                <asp:ImageButton ID="_btnSubir" runat="server" CausesValidation="false" CommandName="Subir" 
                                    Height="16" Width="16" />
                                <asp:ImageButton ID="_btnDescer" runat="server" CausesValidation="false" CommandName="Descer"
                                    Height="16" Width="16" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Editar" HeaderStyle-CssClass="center" Visible="true">
                            <ItemTemplate>
                                <asp:ImageButton ID="_imgEditar" runat="server" CommandName="Edit" SkinID="btEditar"
                                    ToolTip="Editar" CausesValidation="false" />
                                <asp:ImageButton ID="_imgCancelar" runat="server" CommandName="Cancel" SkinID="btDesfazer"
                                    ToolTip="Cancelar" CausesValidation="false" Visible="false" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center" Width="25px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Salvar" HeaderStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:ImageButton ID="_imgSalvar" runat="server" CommandName="Update" SkinID="btConfirmar"
                                    ToolTip="Salvar" ValidationGroup='<%=validationGroup %>'
                                    Visible="false" />
                                <asp:ImageButton ID="_imgCancelarParametro" runat="server" CommandName="Cancel" SkinID="btCancelar"
                                    ToolTip="Cancelar" CausesValidation="false"
                                    Visible="false" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center" Width="25px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Excluir" HeaderStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:ImageButton ID="_imgExcluir" runat="server" CommandName="Delete" SkinID="btExcluir"
                                    ToolTip="Excluir" CausesValidation="false" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center" Width="25px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            <uc3:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="grvCadastroCor" />                            
    </fieldset>
</asp:Content>
