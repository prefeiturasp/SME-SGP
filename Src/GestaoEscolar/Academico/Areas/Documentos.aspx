<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Documentos.aspx.cs" Inherits="GestaoEscolar.Academico.Areas.Documentos" %>

<%@ PreviousPageType VirtualPath="~/Academico/Areas/Busca.aspx" %>

<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="UCComboUAEscola" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoNivelEnsino.ascx" TagName="UCComboTipoNivelEnsino"
    TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMensagem" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="lblMensagem" runat="server" EnableViewState="false"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary" runat="server" ValidationGroup="Documento" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="pnlArea" runat="server" GroupingText="<%$ Resources:Academico, Areas.Documentos.pnlArea.GroupingText %>">
        <div class="area-form">
            <asp:Panel ID="pnlDiretriz" runat="server" GroupingText="<%$ Resources:Academico, Areas.Documentos.pnlDiretriz.GroupingText %>">
                <uc1:UCComboUAEscola ID="UCComboUAEscola" runat="server" CarregarEscolaAutomatico="true"
                    MostrarMessageSelecioneEscola="true" MostrarMessageSelecioneUA="true" ValidationGroup="Documento" />
                <br />
                <br />
                <asp:UpdatePanel ID="updDocumentos" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblMensagemDocumentos" runat="server" EnableViewState="false"></asp:Label>
                        <div id="divDocumentos" runat="server">
                            <asp:Repeater ID="rptDocumentos" runat="server" OnItemDataBound="rptDocumentos_ItemDataBound" OnItemCommand="rptDocumentos_ItemCommand">
                                <HeaderTemplate>
                                    <table class="grid grid-responsive-list" cellspacing="0" style="width: 100%">
                                        <thead>
                                            <tr class="gridHeader">
                                                <th class="hide"></th>
                                                <th style="text-align: left;"><asp:Label ID="lblDescricao" runat="server" Text="<%$ Resources:Academico, Areas.Documentos.lblDescricao.Text %>"></asp:Label></th>
                                                <th></th>
                                                <th class="hide"></th>
                                                <th style="text-align: left;"><asp:Label ID="lblNivelEnsino" runat="server" Text="<%$ Resources:Academico, Areas.Documentos.lblNivelEnsino.Text %>"></asp:Label></th>
                                                <th></th>
                                                <th style="text-align: left;"><asp:Label ID="lblLinkArquivo" runat="server" Text="<%$ Resources:Academico, Areas.Documentos.lblLinkArquivo.Text %>"></asp:Label></th>
                                                <th></th>
                                                <th></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr class="gridRow">
                                        <td class="hide">
                                            <asp:Label ID="lblId" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                            <asp:Label ID="lblTadId" runat="server" Text='<%# Bind("tad_id") %>'></asp:Label>
                                            <asp:Label ID="lblAarId" runat="server" Text='<%# Bind("aar_id") %>'></asp:Label>
                                            <asp:Label ID="lblEscId" runat="server" Text='<%# Bind("esc_id") %>'></asp:Label>
                                            <asp:Label ID="lblUniId" runat="server" Text='<%# Bind("uni_id") %>'></asp:Label>
                                            <asp:Label ID="lblUadIdSuperior" runat="server" Text='<%# Bind("uad_idSuperior") %>'></asp:Label>
                                            <asp:Label ID="lblArqNome" runat="server" Text='<%# Bind("arq_nome") %>'></asp:Label>
                                        </td>
                                        <td style="width:20%">
                                            <asp:TextBox ID="txtDescricao" runat="server" MaxLength="200" SkinID="text20C" Text='<%# Bind("aar_descricao") %>'></asp:TextBox>
                                            <asp:Label ID="lblErroDescricao" runat="server" ForeColor="Red" Text="*" Visible="false"></asp:Label>
                                        </td>
                                        <td style="width:15%">
                                            <asp:RadioButtonList ID="rblLinkArquivo" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                                OnSelectedIndexChanged="rblLinkArquivo_SelectedIndexChanged" ValidationGroup="Documento">
                                                <asp:ListItem Text="<%$ Resources:Academico, Areas.Documentos.rblLinkArquivo.Link.Text %>" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Academico, Areas.Documentos.rblLinkArquivo.Arquivo.Text %>" Value="1"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td class="hide">
                                            <asp:Label ID="lblArqId" runat="server" Text='<%# Bind("arq_id") %>'></asp:Label>
                                        </td>
                                        <td style="width:10%">
                                            <div style="padding-bottom:12px;">
                                                <uc2:UCComboTipoNivelEnsino EnableTheming="false" ID="UCComboTipoNivelEnsino" runat="server" ValidationGroup="Documento" MostrarMessageTodos="true" Titulo=""/>
                                            </div>
                                        </td>
                                        <td style="width:10%">
                                            <asp:CheckBox runat="server" ID="chkPPP" ValidationGroup="Documento" Text="PPP" style="padding-top:5px;"/>
                                        </td>
                                        <td style="width:35%">
                                            <asp:HyperLink ID="hplDocumento" runat="server" Text="" ToolTip="<%$ Resources:Academico, Areas.Documentos.hplDocumento.ToolTip %>" Visible="false"></asp:HyperLink>
                                            <asp:TextBox ID="txtLink" runat="server" CssClass="text30C" Visible="false" Text='<%# Bind("aar_link") %>' />
                                            <asp:Button ID="btnUpload" runat="server" Text="<%$ Resources:Academico, Areas.Documentos.btnUpload.Text %>" Visible="false" CommandName="Upload" />
                                            <asp:Label ID="lblErroLink" runat="server" ForeColor="Red" Text="*" Visible="false"></asp:Label>
                                        </td>
                                        <td style="width:5%" align="center" id="colunaRepeaterAdicionar" runat="server" class="grid-responsive-item-inline">
                                            <asp:ImageButton ID="btnAdicionar" runat="server" SkinID="btNovo" ToolTip="<%$ Resources:Academico, Areas.Documentos.btnAdicionar.ToolTip %>"
                                                CommandName="Adicionar" ValidationGroup="Documento" Visible="False" />
                                        </td>
                                        <td style="width:5%" align="center" id="colunaExcluir" runat="server" class="grid-responsive-item-inline">
                                            <asp:ImageButton ID="btnExcluir" runat="server" SkinID="btExcluir" ToolTip="<%$ Resources:Academico, Areas.Documentos.btnExcluir.ToolTip %>"
                                                CommandName="Excluir" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="gridAlternatingRow">
                                        <td class="hide">
                                            <asp:Label ID="lblId" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                            <asp:Label ID="lblTadId" runat="server" Text='<%# Bind("tad_id") %>'></asp:Label>
                                            <asp:Label ID="lblAarId" runat="server" Text='<%# Bind("aar_id") %>'></asp:Label>
                                            <asp:Label ID="lblEscId" runat="server" Text='<%# Bind("esc_id") %>'></asp:Label>
                                            <asp:Label ID="lblUniId" runat="server" Text='<%# Bind("uni_id") %>'></asp:Label>
                                            <asp:Label ID="lblUadIdSuperior" runat="server" Text='<%# Bind("uad_idSuperior") %>'></asp:Label>
                                            <asp:Label ID="lblArqNome" runat="server" Text='<%# Bind("arq_nome") %>'></asp:Label>
                                        </td>
                                        <td style="width:20%">
                                            <asp:TextBox ID="txtDescricao" runat="server" MaxLength="200" SkinID="text20C" Text='<%# Bind("aar_descricao") %>'></asp:TextBox>
                                            <asp:Label ID="lblErroDescricao" runat="server" ForeColor="Red" Text="*" Visible="false"></asp:Label>
                                        </td>
                                        <td style="width:15%">
                                            <asp:RadioButtonList ID="rblLinkArquivo" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                                OnSelectedIndexChanged="rblLinkArquivo_SelectedIndexChanged" ValidationGroup="Documento">
                                                <asp:ListItem Text="<%$ Resources:Academico, Areas.Documentos.rblLinkArquivo.Link.Text %>" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Academico, Areas.Documentos.rblLinkArquivo.Arquivo.Text %>" Value="1"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td class="hide">
                                            <asp:Label ID="lblArqId" runat="server" Text='<%# Bind("arq_id") %>'></asp:Label>
                                        </td>
                                        <td style="width:10%">
                                            <div style="padding-bottom:12px;">
                                                <uc2:UCComboTipoNivelEnsino EnableTheming="false" ID="UCComboTipoNivelEnsino" runat="server" ValidationGroup="Documento" MostrarMessageTodos="true" Titulo=""/>
                                            </div>
                                        </td>
                                        <td style="width:10%">
                                            <asp:CheckBox runat="server" ID="chkPPP" ValidationGroup="Documento" Text="PPP" style="padding-top:5px;"/>
                                        </td>
                                        <td style="width:35%">
                                            <asp:HyperLink ID="hplDocumento" runat="server" Text="" ToolTip="<%$ Resources:Academico, Areas.Documentos.hplDocumento.ToolTip %>" Visible="false"></asp:HyperLink>
                                            <asp:TextBox ID="txtLink" runat="server" CssClass="text30C" Visible="false" Text='<%# Bind("aar_link") %>' />
                                            <asp:Button ID="btnUpload" runat="server" Text="<%$ Resources:Academico, Areas.Documentos.btnUpload.Text %>" Visible="false" CommandName="Upload" />
                                            <asp:Label ID="lblErroLink" runat="server" ForeColor="Red" Text="*" Visible="false"></asp:Label>
                                        </td>
                                        <td style="width:5%" align="center" id="colunaRepeaterAdicionar" runat="server">
                                            <asp:ImageButton ID="btnAdicionar" runat="server" SkinID="btNovo" ToolTip="<%$ Resources:Academico, Areas.Documentos.btnAdicionar.ToolTip %>"
                                                CommandName="Adicionar" ValidationGroup="Documento" Visible="False" />
                                        </td>
                                        <td style="width:5%" align="center" id="colunaExcluir" runat="server">
                                            <asp:ImageButton ID="btnExcluir" runat="server" SkinID="btExcluir" ToolTip="<%$ Resources:Academico, Areas.Documentos.btnExcluir.ToolTip %>"
                                                CommandName="Excluir" />
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                                <FooterTemplate>
                                    </tbody>
                                        </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnAdicionarArquivo" />
                    </Triggers>
                </asp:UpdatePanel>
            </asp:Panel>
        </div>
        <div class="right area-botoes-bottom">
            <asp:Button ID="btnSalvar" runat="server" Text="<%$ Resources:Academico, Areas.Documentos.btnSalvar.Text %>" OnClick="btnSalvar_Click"
                ValidationGroup="Documento" />
            <asp:Button ID="btnCancelar" runat="server" Text="<%$ Resources:Academico, Areas.Documentos.btnCancelar.Text %>" OnClick="btnCancelar_Click"
                CausesValidation="false" />
        </div>
    </asp:Panel>

    <div id="divUpload" title="Incluir documento" class="hide">
        <asp:UpdatePanel ID="upnUpload" runat="server">
            <ContentTemplate>
                <fieldset>
                    <asp:Label ID="lblArquivo" runat="server" Text="<%$ Resources:Academico, Areas.Documentos.lblArquivo.Text %>" AssociatedControlID="fupArquivo"></asp:Label>
                    <asp:FileUpload ID="fupArquivo" runat="server" ToolTip="<%$ Resources:Academico, Areas.Documentos.fupArquivo.ToolTip %>" CssClass="text60C" />
                    <div class="right">
                        <asp:Button ID="btnAdicionarArquivo" runat="server" Text="<%$ Resources:Academico, Areas.Documentos.btnAdicionarArquivo.Text %>" OnClick="btnAdicionarArquivo_Click" />
                        <asp:Button ID="btnCancelarAdicionarArquivo" runat="server" Text="<%$ Resources:Academico, Areas.Documentos.btnCancelarAdicionarArquivo.Text %>" CausesValidation="False"
                            OnClientClick="$('#divUpload').dialog('close');return false;" />
                    </div>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
