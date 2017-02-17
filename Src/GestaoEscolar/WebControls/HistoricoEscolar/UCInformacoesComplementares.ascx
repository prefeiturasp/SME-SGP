<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCInformacoesComplementares.ascx.cs"
    Inherits="GestaoEscolar.WebControls.HistoricoEscolar.UCInformacoesComplementares" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>


<%--Mensagens--%>
<asp:UpdatePanel ID="updMessage" runat="server">
    <ContentTemplate>
        <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
    </ContentTemplate>
</asp:UpdatePanel>

<fieldset>
    <legend>
        <asp:Label runat="server" ID="lblLegend" EnableViewState="false" Text="<%$ Resources:UserControl, UCInformacoesComplementares.lblLegend.Text %>"></asp:Label>
    </legend>
    <%--Observações--%>
    <div id="divObservacao">
        <asp:UpdatePanel ID="updObservacao" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Button runat="server" ID="btnObservacao" Text="<%$ Resources:UserControl, UCInformacoesComplementares.btnObservacao.Text %>"
                    OnClick="btnObservacao_Click" />
                <asp:GridView runat="server" ID="grvObservacao" EmptyDataText="<%$ Resources:UserControl, UCInformacoesComplementares.grvObservacao.EmptyDataText%>"
                    AutoGenerateColumns="false" DataSourceID="odsObservacao" OnRowCommand="grvObservacao_RowCommand" DataKeyNames="alu_id, aho_id"
                    OnRowDataBound="grvObservacao_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="<%$ Resources:UserControl, UCInformacoesComplementares.grvObservacao.HeaderTextColumnObservacao%>">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lnkAlterar" Text='<%# Bind("aho_observacao") %>'
                                    ToolTip='<%# Bind("aho_observacao") %>' CommandName="Alterar"></asp:LinkButton>
                                <asp:Label runat="server" ID="lblAlterar" Text='<%# Bind("aho_observacao") %>'
                                    ToolTip='<%# Bind("aho_observacao") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:UserControl, UCInformacoesComplementares.grvObservacao.HeaderTextColumnExcluir%>">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnExcluir" runat="server" CommandName="Deletar" SkinID="btExcluir"
                                    CausesValidation="False" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource runat="server" ID="odsObservacao" SelectMethod="SelecionaPorAluno"
                    TypeName="MSTech.GestaoEscolar.BLL.ACA_AlunoHistoricoObservacaoBO"></asp:ObjectDataSource>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <%--Certificado--%>
    <div runat="server" id="divCertificado">
        <asp:UpdatePanel ID="updCertificado" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset>
                    <legend>
                        <asp:Label ID="lblCertificados" runat="server" EnableViewState="false" Text="<%$ Resources:UserControl, UCInformacoesComplementares.lblCertificados.Text %>" CssClass=""></asp:Label>
                    </legend>
                    <asp:Label ID="lblMensagemApareceHistorico" runat="server" EnableViewState="false" />
                    <asp:Button runat="server" ID="btnCertificado" Text="<%$ Resources:UserControl, UCInformacoesComplementares.btnCertificado.Text %>" OnClick="btnCertificado_Click" />
                    <asp:GridView runat="server" ID="grvCertificado" EmptyDataText="<%$ Resources:UserControl, UCInformacoesComplementares.grvCertificadoo.EmptyDataText %>"
                        AutoGenerateColumns="false" DataSourceID="odsCertificado" OnRowCommand="grvCertificado_RowCommand" DataKeyNames="alu_id, alh_id"
                        OnRowDataBound="grvCertificado_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ Resources:UserControl, UCInformacoesComplementares.grvCertificado.HeaderTextColumnAnoHistorico %>">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="lnkAlterar" Text='<%# Bind("AnoSerie") %>' CommandName="Alterar"></asp:LinkButton>
                                    <asp:Label runat="server" ID="lblAlterar" Text='<%# Bind("AnoSerie") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ahc_numero" HeaderText="<%$ Resources:UserControl, UCInformacoesComplementares.grvCertificado.HeaderTextColumnNumero %>" />
                            <asp:BoundField DataField="ahc_folha" HeaderText="<%$ Resources:UserControl, UCInformacoesComplementares.grvCertificado.HeaderTextColumnFolha %>" />
                            <asp:BoundField DataField="ahc_livro" HeaderText="<%$ Resources:UserControl, UCInformacoesComplementares.grvCertificado.HeaderTextColumnLivro %>" />
                            <asp:BoundField DataField="ahc_gdae" HeaderText="<%$ Resources:UserControl, UCInformacoesComplementares.grvCertificado.HeaderTextColumnGDAE %>" />
                            <asp:TemplateField HeaderText="<%$ Resources:UserControl, UCInformacoesComplementares.grvCertificado.HeaderTextColumnExcluir %>">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnExcluir" runat="server" CommandName="Deletar" SkinID="btExcluir"
                                        CausesValidation="False" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource runat="server" ID="odsCertificado" SelectMethod="SelecionaPorAluno"
                        TypeName="MSTech.GestaoEscolar.BLL.ACA_AlunoHistoricoCertificadoBO"></asp:ObjectDataSource>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div class="right">
        <asp:Button ID="btnVisualizarHistorico" runat="server" Text="<%$ Resources:UserControl, UCInformacoesComplementares.btnVisualizarHistorico.Text %>" CausesValidation="false"
            OnClick="btnVisualizarHistorico_Click" />
        <asp:Button ID="btnVoltar" runat="server" Text="<%$ Resources:UserControl, UCInformacoesComplementares.btnVoltar.Text %>" CausesValidation="false"
            OnClick="btnVoltar_Click" />
    </div>
</fieldset>

<%--Pop-up cadastro de observacao--%>
<div class="hide divCadastroObservacao" id="divCadastroObservacao" title="<%$ Resources:UserControl, UCInformacoesComplementares.divCadastroObservacao.Title %>" runat="server">
    <asp:UpdatePanel ID="updCadastroObservacao" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Label ID="lblMessageObservacao" runat="server" EnableViewState="false"></asp:Label>
            <asp:ValidationSummary ID="vlsObservacao" ValidationGroup="vlgObservacao" runat="server" />

            <asp:Label runat="server" ID="lblObservacao" Text="<%$ Resources:UserControl, UCInformacoesComplementares.lblObservacao.Text %>"
                AssociatedControlID="txtObservacaoHtml"></asp:Label>
            <CKEditor:CKEditorControl ID="txtObservacaoHtml" BasePath="/includes/ckeditor/" runat="server"></CKEditor:CKEditorControl>
            <asp:RequiredFieldValidator runat="server" ID="rfvObservacao" ControlToValidate="txtObservacaoHtml"
                ValidationGroup="vlgObservacao" ErrorMessage="<%$ Resources:UserControl, UCInformacoesComplementares.rfvObservacao.ErrorMessage %>">
            </asp:RequiredFieldValidator>
            <div runat="server" id="divObsPadrao">
                <asp:Panel runat="server" GroupingText="<%$ Resources:UserControl, UCInformacoesComplementares.pnlObservacao.Text %>" ID="pnlObservacao">
                    <asp:UpdatePanel ID="updObservacoesPadroes" runat="server">
                        <ContentTemplate>
                            <asp:Repeater ID="rptObservacoesPadroes" runat="server" OnItemDataBound="rptObservacoesPadroes_ItemDataBound">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnConfirmaObservacoesPadroes" runat="server" SkinID="btConfirmar"
                                        Visible='<%# (!string.IsNullOrEmpty(Eval("hop_id").ToString()))? true: false %>' />
                                    <asp:Label ID="lblObservacoesPadroes" runat="server" Text='<%# Eval("hop_descricao").ToString() %>'
                                        Visible='<%# (!string.IsNullOrEmpty(Eval("hop_id").ToString()))? true: false %>'></asp:Label><br />
                                </ItemTemplate>
                            </asp:Repeater>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </div>
            <div class="right">
                <asp:Button ID="btnSalvarObservacao" runat="server" Text="<%$ Resources:UserControl, UCInformacoesComplementares.btnSalvarObservacao.Text %>" CausesValidation="true"
                    OnClick="btnSalvarObservacao_Click" ValidationGroup="vlgObservacao" />
                <asp:Button ID="btnCancelarObservacao" runat="server" Text="<%$ Resources:UserControl, UCInformacoesComplementares.btnCancelarObservacao.Text %>" CausesValidation="false"
                    OnClientClick="$('.divCadastroObservacao').dialog('close');" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<%--Pop-up cadastro de certificados--%>
<div class="hide divCadastroCertificado" id="divCadastroCertificado" title="<%$ Resources:UserControl, UCInformacoesComplementares.divCadastroCertificado.Title %>" runat="server">
    <asp:UpdatePanel ID="updCadastroCertificado" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Label ID="lblMessageCertificado" runat="server" EnableViewState="false"></asp:Label>
            <asp:ValidationSummary ID="vlsCertificado" ValidationGroup="vlgCertificado" runat="server" />

            <asp:Label runat="server" ID="lblAnoHistorico" Text="<%$ Resources:UserControl, UCInformacoesComplementares.lblAnoHistorico.Text %>"
                AssociatedControlID="ddlAnoHistorico"></asp:Label>
            <asp:DropDownList runat="server" ID="ddlAnoHistorico" DataTextField="AnoSerie" DataValueField="alh_id" SkinID="text30C"></asp:DropDownList>
            <asp:CompareValidator ID="cpvAnoHistorico" runat="server" ErrorMessage="<%$ Resources:UserControl, UCInformacoesComplementares.cpvAnoHistorico.ErrorMessage %>"
                ControlToValidate="ddlAnoHistorico" Operator="GreaterThan" ValueToCompare="0"
                Display="Dynamic" ValidationGroup="vlgCertificado">*</asp:CompareValidator>
            
            <asp:Label runat="server" ID="lblNumero" Text="<%$ Resources:UserControl, UCInformacoesComplementares.lblNumero.Text %>"
                AssociatedControlID="txtNumero"></asp:Label>
            <asp:TextBox runat="server" ID="txtNumero" MaxLength="50"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ID="rfvNumero" ControlToValidate="txtNumero"
                ValidationGroup="vlgCertificado" ErrorMessage="<%$ Resources:UserControl, UCInformacoesComplementares.rfvNumero.ErrorMessage %>">*
            </asp:RequiredFieldValidator>

            <asp:Label runat="server" ID="lblFolha" Text="<%$ Resources:UserControl, UCInformacoesComplementares.lblFolha.Text %>"
                AssociatedControlID="txtFolha"></asp:Label>
            <asp:TextBox runat="server" ID="txtFolha" MaxLength="50"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ID="rfvFolha" ControlToValidate="txtFolha"
                ValidationGroup="vlgCertificado" ErrorMessage="<%$ Resources:UserControl, UCInformacoesComplementares.rfvFolha.ErrorMessage %>">*
            </asp:RequiredFieldValidator>


            <asp:Label runat="server" ID="lblLivro" Text="<%$ Resources:UserControl, UCInformacoesComplementares.lblLivro.Text %>"
                AssociatedControlID="txtLivro"></asp:Label>
            <asp:TextBox runat="server" ID="txtLivro" MaxLength="50"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ID="rfvLivro" ControlToValidate="txtLivro"
                ValidationGroup="vlgCertificado" ErrorMessage="<%$ Resources:UserControl, UCInformacoesComplementares.rfvLivro.ErrorMessage %>">*
            </asp:RequiredFieldValidator>

            <asp:Label runat="server" ID="lblGDAE" Text="<%$ Resources:UserControl, UCInformacoesComplementares.lblGdae.Text %>"
                AssociatedControlID="txtGDAE"></asp:Label>
            <asp:TextBox runat="server" ID="txtGDAE" MaxLength="50"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ID="rfvGDAE" ControlToValidate="txtGDAE"
                ValidationGroup="vlgCertificado" ErrorMessage="<%$ Resources:UserControl, UCInformacoesComplementares.rfvGDAE.ErrorMessage %>">*
            </asp:RequiredFieldValidator>

            <div class="right">
                <asp:Button ID="btnSalvarCertificado" runat="server" Text="<%$ Resources:UserControl, UCInformacoesComplementares.btnSalvarCertificado.Text %>"
                    OnClick="btnSalvarCertificado_Click" ValidationGroup="vlgCertificado" />
                <asp:Button ID="btnCancelarCertificado" runat="server" Text="<%$ Resources:UserControl, UCInformacoesComplementares.btnCancelarCertificado.Text %>" CausesValidation="false"
                    OnClientClick="$('.divCadastroCertificado').dialog('close');" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
