<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Relatorios.ObjetoAprendizagem.Busca" %>

<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="UCComboUAEscola"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboAnoLetivo.ascx" TagName="UCComboAnoLetivo" 
    TagPrefix="uc3" %>

<%@ Register src="../../WebControls/Combos/UCComboTipoDisciplina.ascx" tagname="UCComboTipoDisciplina" tagprefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="updMensagem" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Resultados" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="uppPesquisa" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div>
                <fieldset id="fdsPesquisa" runat="server" style="margin-left: 10px;">
                    <legend><asp:Label runat="server" ID="lblLegend" Text="<%$ Resources:Relatorios, ObjetoAprendizagem.Busca.lblLegend.Text %>"></asp:Label></legend>
                    <uc2:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" Visible="false" />
                    <div id="_divPesquisa" class="divPesquisa area-form" runat="server">
                        <asp:Label ID="lblAvisoMensagem" runat="server"></asp:Label>
                        <!-- FiltrosPadrao -->
                        <uc3:UCComboAnoLetivo ID="UCComboAnoLetivo1" runat="server" Obrigatorio="true" _MostrarMessageSelecione="false" />
                        <uc1:UCComboUAEscola ID="UCComboUAEscola" runat="server" CarregarEscolaAutomatico="true"
                            MostrarMessageSelecioneEscola="true" MostrarMessageSelecioneUA="true" OnIndexChangedUA="UCComboUAEscola_IndexChangedUA"
                            OnIndexChangedUnidadeEscola="UCComboUAEscola_IndexChangedUnidadeEscola" ValidationGroup="Resultados" 
                            ObrigatorioEscola="false" ObrigatorioUA="false" />
                        <uc4:UCComboTipoDisciplina ID="UCComboTipoDisciplina1" runat="server" Obrigatorio="true" ValidationGroup="Resultados" _MostrarMessageSelecione="true" />
                        <div>
                            <br />
                            <asp:UpdatePanel ID="updCiclos" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <fieldset>
                                        <legend>Ciclos <span class="asteriscoObrigatorio">*</span></legend>
                                        <div></div>
                                        <asp:Repeater ID="rptCampos" runat="server">
                                            <HeaderTemplate>
                                                <div class="checkboxlist-columns">
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnId" runat="server" Value='<%# Eval("tci_id") %>' />
                                                <asp:CheckBox ID="ckbCampo" runat="server" Text='<%# Eval("tci_nome") %>' />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </div> 
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </fieldset>
                                    <asp:CustomValidator ID="cvCiclos" runat="server" ValidationGroup="Resultados" Display="None" Text="*"
                                        OnServerValidate="cvCiclos_ServerValidate" ErrorMessage="<%$ Resources:Relatorios, ObjetoAprendizagem.Busca.cvCiclos.ErrorMessage %>" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <asp:Label runat="server" ID="lblTipoRel" Text="<%$ Resources:Relatorios, ObjetoAprendizagem.Busca.lblTipoRel.Text %>"></asp:Label>
                        <asp:DropDownList runat="server" ID="ddlTipoRel">
                            <asp:ListItem Text="Percentual" Value="0" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Valor absoluto" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="right area-botoes-bottom">
                        <asp:Button ID="btnGerarRel" runat="server" Text="<%$ Resources:Relatorios, ObjetoAprendizagem.Busca.btnGerarRel.Text %>" OnClick="btnGerarRel_Click"
                            ValidationGroup="Resultados" />
                    </div>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
