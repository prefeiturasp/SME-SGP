<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Academico.Sondagem.Cadastro" %>

<%@ PreviousPageType VirtualPath="~/Academico/Sondagem/Busca.aspx" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel runat="server" ID="updMessage" UpdateMode="Always">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel runat="server" ID="updCadastro" UpdateMode="Always">
        <ContentTemplate>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
            <fieldset>
                <legend><asp:Label runat="server" ID="lblLegend" Text="<%$ Resources:Academico, Sondagem.Cadastro.lblLegend.Text %>" /></legend>
                <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios3" runat="server" />
                <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:Academico, Sondagem.Cadastro.lblTitulo.Text %>" AssociatedControlID="txtTitulo" />
                <asp:TextBox ID="txtTitulo" runat="server" SkinID="text60C" MaxLength="200"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvTitulo" runat="server" ControlToValidate="txtTitulo"
                    Display="Dynamic" ErrorMessage="<%$ Resources:Academico, Sondagem.Cadastro.rfvTitulo.ErrorMessage %>" Text="*" />
                <asp:Label ID="lblDescricao" runat="server" Text="<%$ Resources:Academico, Sondagem.Cadastro.lblDescricao.Text %>" AssociatedControlID="txtDescricao" />
                <asp:TextBox ID="txtDescricao" runat="server" TextMode="MultiLine" SkinID="limite4000" MaxLength="4000"></asp:TextBox>
                <div><br /></div>
                <fieldset>
                    <legend><asp:Label runat="server" ID="lblLegendQuestoes" Text="<%$ Resources:Academico, Sondagem.Cadastro.lblLegendQuestoes.Text %>" /></legend>
                    <asp:Button runat="server" ID="btnAdicionarQuestao" Text="<%$ Resources:Academico, Sondagem.Cadastro.btnAdicionarQuestao.Text %>"
                        OnClick="btnAdicionarQuestao_Click" CausesValidation="false" />
                    <asp:GridView runat="server" ID="grvQuestoes" AutoGenerateColumns="false" AllowPaging="false" AllowSorting="false"
                        DataKeyNames="sdq_id, sdq_descricao, sdq_subQuestao" EmptyDataText="<%$ Resources:Academico, Sondagem.Cadastro.grvQuestoes.EmptyDataText %>"
                        OnDataBound="grv_DataBound" OnRowDataBound="grv_RowDataBound" OnRowCommand="grvQuestoes_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="sdq_ordem" HeaderText="<%$ Resources:Academico, Sondagem.Cadastro.grvQuestoes.HeaderNumero %>" />
                            <asp:TemplateField HeaderText="<%$ Resources:Academico, Sondagem.Cadastro.grvQuestoes.HeaderNome %>">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnAlterar" runat="server" CommandName="Alterar" Text='<%# Bind("sdq_descricao") %>' CausesValidation="false"></asp:LinkButton>
                                    <asp:Label ID="lblAlterar" runat="server" Text='<%# Bind("sdq_descricao") %>' Visible="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Academico, Sondagem.Cadastro.grvQuestoes.HeaderOrdem %>">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("sdq_ordem") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="_btnSubir" runat="server" CausesValidation="false" CommandName="Subir"
                                        Height="16" Width="16" />
                                    <asp:ImageButton ID="_btnDescer" runat="server" CausesValidation="false" CommandName="Descer"
                                        Height="16" Width="16" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="center" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Academico, Sondagem.Cadastro.grvQuestoes.HeaderExcluir %>">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnExcluir" SkinID="btExcluir" runat="server" CommandName="Excluir" CausesValidation="false"
                                        ToolTip="<%$ Resources:Academico, Sondagem.Cadastro.grvQuestoes.btnExcluir.ToolTip %>" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </fieldset>
                <fieldset>
                    <legend><asp:Label runat="server" ID="lblLegendSubQuestoes" Text="<%$ Resources:Academico, Sondagem.Cadastro.lblLegendSubQuestoes.Text %>" /></legend>
                    <asp:Button runat="server" ID="btnAdicionarSubQuestao" Text="<%$ Resources:Academico, Sondagem.Cadastro.btnAdicionarSubQuestao.Text %>"
                        OnClick="btnAdicionarSubQuestao_Click" CausesValidation="false" />
                    <asp:GridView runat="server" ID="grvSubQuestoes" AutoGenerateColumns="false" AllowPaging="false" AllowSorting="false"
                        DataKeyNames="sdq_id, sdq_descricao, sdq_subQuestao" EmptyDataText="<%$ Resources:Academico, Sondagem.Cadastro.grvSubQuestoes.EmptyDataText %>"
                        OnDataBound="grv_DataBound" OnRowDataBound="grv_RowDataBound" OnRowCommand="grvSubQuestoes_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="sdq_ordem" HeaderText="<%$ Resources:Academico, Sondagem.Cadastro.grvSubQuestoes.HeaderNumero %>" />
                            <asp:TemplateField HeaderText="<%$ Resources:Academico, Sondagem.Cadastro.grvSubQuestoes.HeaderNome %>">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnAlterar" runat="server" CommandName="Alterar" Text='<%# Bind("sdq_descricao") %>' CausesValidation="false"></asp:LinkButton>
                                    <asp:Label ID="lblAlterar" runat="server" Text='<%# Bind("sdq_descricao") %>' Visible="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Academico, Sondagem.Cadastro.grvSubQuestoes.HeaderOrdem %>">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("sdq_ordem") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="_btnSubir" runat="server" CausesValidation="false" CommandName="Subir"
                                        Height="16" Width="16" />
                                    <asp:ImageButton ID="_btnDescer" runat="server" CausesValidation="false" CommandName="Descer"
                                        Height="16" Width="16" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="center" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Academico, Sondagem.Cadastro.grvSubQuestoes.HeaderExcluir %>">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnExcluir" SkinID="btExcluir" runat="server" CommandName="Excluir" CausesValidation="false"
                                        ToolTip="<%$ Resources:Academico, Sondagem.Cadastro.grvSubQuestoes.btnExcluir.ToolTip %>" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </fieldset>
                <fieldset>
                    <legend><asp:Label runat="server" ID="lblLegendRespostas" Text="<%$ Resources:Academico, Sondagem.Cadastro.lblLegendRespostas.Text %>" /></legend>
                    <asp:Button runat="server" ID="btnAdicionarResposta" Text="<%$ Resources:Academico, Sondagem.Cadastro.btnAdicionarResposta.Text %>"
                        OnClick="btnAdicionarResposta_Click" CausesValidation="false" />
                    <asp:GridView runat="server" ID="grvRespostas" AutoGenerateColumns="false" AllowPaging="false" AllowSorting="false"
                        DataKeyNames="sdr_id, sdr_descricao, sdr_sigla" EmptyDataText="<%$ Resources:Academico, Sondagem.Cadastro.grvRespostas.EmptyDataText %>"
                        OnDataBound="grv_DataBound" OnRowDataBound="grv_RowDataBound" OnRowCommand="grvRespostas_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="sdr_ordem" HeaderText="<%$ Resources:Academico, Sondagem.Cadastro.grvRespostas.HeaderNumero %>" />
                            <asp:BoundField DataField="sdr_sigla" HeaderText="<%$ Resources:Academico, Sondagem.Cadastro.grvRespostas.HeaderSigla %>" />
                            <asp:TemplateField HeaderText="<%$ Resources:Academico, Sondagem.Cadastro.grvRespostas.HeaderNome %>">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnAlterar" runat="server" CommandName="Alterar" Text='<%# Bind("sdr_descricao") %>' CausesValidation="false"></asp:LinkButton>
                                    <asp:Label ID="lblAlterar" runat="server" Text='<%# Bind("sdr_descricao") %>' Visible="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Academico, Sondagem.Cadastro.grvRespostas.HeaderOrdem %>">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("sdr_ordem") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="_btnSubir" runat="server" CausesValidation="false" CommandName="Subir"
                                        Height="16" Width="16" />
                                    <asp:ImageButton ID="_btnDescer" runat="server" CausesValidation="false" CommandName="Descer"
                                        Height="16" Width="16" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="center" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Academico, Sondagem.Cadastro.grvRespostas.HeaderExcluir %>">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnExcluir" SkinID="btExcluir" runat="server" CommandName="Excluir" CausesValidation="false"
                                        ToolTip="<%$ Resources:Academico, Sondagem.Cadastro.grvRespostas.btnExcluir.ToolTip %>" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </fieldset>
                <asp:CheckBox ID="ckbBloqueado" runat="server" Visible="False" Text="Bloqueado" />
                <div class="right">
                    <asp:Button ID="bntSalvar" runat="server" Text="<%$ Resources:Academico, Sondagem.Cadastro.bntSalvar.Text %>" OnClick="bntSalvar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="<%$ Resources:Academico, Sondagem.Cadastro.btnCancelar.Text %>" CausesValidation="false"
                        OnClick="btnCancelar_Click" />
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="divInserir" class="hide">
        <asp:UpdatePanel runat="server" ID="updMessagePopUp" UpdateMode="Always">
            <ContentTemplate>
                <asp:Label ID="lblMessagePopUp" runat="server" EnableViewState="False"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel runat="server" ID="updPopUp" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset>
                    <legend><asp:Label runat="server" ID="lblTituloPopUp" /></legend>
                    <div runat="server" id="divSigla" visible="false">
                        <asp:Label runat="server" ID="lblSigla" Text="<%$ Resources:Academico, Sondagem.Cadastro.lblSigla.Text %>" AssociatedControlID="txtSigla" />
                        <asp:TextBox runat="server" ID="txtSigla" SkinID="text10C" MaxLength="20" />
                    </div>
                    <asp:Label runat="server" ID="lblCampo" Text="<%$ Resources:Academico, Sondagem.Cadastro.lblCampo.Text %>" AssociatedControlID="txtItem" />
                    <asp:TextBox runat="server" ID="txtItem" SkinID="text60C" MaxLength="250" />
                    <div class="right">
                            <asp:Button ID="btnAdicionar" runat="server" Text="<%$ Resources:Academico, Sondagem.Cadastro.bntAdicionar.Text %>" CausesValidation="false" 
                                OnClick="btnAdicionar_Click" />
                            <asp:Button ID="btnCancelarItem" runat="server" Text="<%$ Resources:Academico, Sondagem.Cadastro.btnCancelar.Text %>" CausesValidation="false"
                                OnClientClick="$('#divInserir').dialog('close');" />
                    </div>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>