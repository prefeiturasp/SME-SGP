<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Classe.CompensacaoAusencia.Cadastro" %>

<%@ PreviousPageType VirtualPath="~/Classe/CompensacaoAusencia/Busca.aspx" %>
<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagPrefix="uc1" TagName="UCComboUAEscola" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCursoCurriculo.ascx" TagPrefix="uc2" TagName="UCCCursoCurriculo" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCPeriodoCalendario.ascx" TagPrefix="uc3" TagName="UCCPeriodoCalendario" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCalendario.ascx"  TagPrefix="uc5" TagName="UCComboCalendario"%>
<%@ Register src="~/WebControls/Combos/Novos/UCCTurmaDisciplina.ascx" tagname="UCCTurmaDisciplina" tagprefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMensagem" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary" runat="server" ValidationGroup="CompAusencia" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <fieldset id="fdsEscola" runat="server">
        <legend>Cadastro de compensação de ausência</legend>
        <asp:UpdatePanel ID="uppPesquisa" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="divPesquisa" runat="server" class="area-form">
                    <uc1:UCComboUAEscola runat="server" ID="UCComboUAEscola" AsteriscoObg="true" ObrigatorioEscola="true" ObrigatorioUA="true"
                        CarregarEscolaAutomatico="true" MostraApenasAtivas="true" MostrarMessageSelecioneEscola="true" MostrarMessageSelecioneUA="true"
                        ValidationGroup="CompAusencia" ValidationGroupEscola="CompAusencia" ValidationGroupUa="CompAusencia" />
                    <uc5:UCComboCalendario ID="UCComboCalendario" runat="server" MostrarMensagemSelecione="true" ValidationGroup="CompAusencia"
                            Obrigatorio="true" PermiteEditar="false" />
                    <uc2:UCCCursoCurriculo runat="server" ID="UCCCursoCurriculo" Obrigatorio="true" MostrarMensagemSelecione="true" PermiteEditar="false"
                        ValidationGroup="CompAusencia" />
                    <asp:Label ID="lblTurma" Text="Turma *" runat="server" AssociatedControlID="ddlTurma"></asp:Label>
                    <asp:DropDownList ID="ddlTurma" runat="server" AppendDataBoundItems="True"
                        AutoPostBack="true" DataTextField="tur_esc_nome" DataValueField="tur_id"
                        SkinID="text60C" OnSelectedIndexChanged="ddlTurma_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:CompareValidator ID="cpvTurma" runat="server" ErrorMessage="Turma é obrigatório."
                        ControlToValidate="ddlTurma" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic" ValidationGroup="CompAusencia"
                        Visible="true">*</asp:CompareValidator>
                    <uc4:UCCTurmaDisciplina ID="UCCTurmaDisciplina1" runat="server" 
                        Obrigatorio="true" PermiteEditar="false" MostrarMensagemSelecione="true"
                        ValidationGroup="CompAusencia" VS_MostraFilhosRegencia="false" VS_MostraExperiencia="true" VS_MostraTerritorio="false" />
                    <uc3:UCCPeriodoCalendario runat="server" ID="UCCPeriodoCalendario" MostrarMensagemSelecione="true" Obrigatorio="true" PermiteEditar="false"
                        ValidationGroup="CompAusencia" />
                    <asp:Label ID="lblQtAulas" runat="server" Text="Quantidade de aulas compensadas *" AssociatedControlID="txtQtAulas"></asp:Label>
                    <asp:TextBox runat="server" ID="txtQtAulas" MaxLength="2" SkinID="Numerico" CssClass="numeric"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvQtAulas" runat="server" ErrorMessage="Quantidade de aulas compensadas é obrigatório."
                        Display="Dynamic" ControlToValidate="txtQtAulas" ValidationGroup="CompAusencia">*</asp:RequiredFieldValidator>
                    <asp:Label ID="lblAtividades" runat="server" Text="Atividades desenvolvidas *" AssociatedControlID="txtAtividades"></asp:Label>
                    <asp:TextBox runat="server" ID="txtAtividades" TextMode="MultiLine" Width="500px" Height="150px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvAtividades" runat="server" ErrorMessage="Atividades desenvolvidas é obrigatório."
                        Display="Dynamic" ControlToValidate="txtAtividades" ValidationGroup="CompAusencia">*</asp:RequiredFieldValidator>
                    <fieldset id="fdsAlunos" runat="server" visible="false">
                        <legend>Adicionar alunos</legend>
                        <asp:Repeater ID="rptAlunos" runat="server">
                            <HeaderTemplate>
                                <div></div>
                                <div class="checkboxlist-columns">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnId" runat="server" Value='<%# Eval("TudAluMtuMtd_id") %>' />
                                <asp:CheckBox ID="ckbAluno" runat="server" Text='<%# Eval("MatAluAusenc") %>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                </div>
                            </FooterTemplate>
                        </asp:Repeater>
                    </fieldset>
                </div>
                <div class="right area-botoes-bottom">
                    <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClientClick="window.scrollTo(0,0);" OnClick="btnSalvar_Click"
                        ValidationGroup="CompAusencia" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click"
                        CausesValidation="false" Style="margin-bottom: 0px" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
</asp:Content>
