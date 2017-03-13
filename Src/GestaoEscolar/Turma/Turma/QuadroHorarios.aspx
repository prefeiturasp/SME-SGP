<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="QuadroHorarios.aspx.cs" Inherits="GestaoEscolar.Turma.Turma.QuadroHorarios" %>

<%@ Register Src="~/WebControls/Calendario/UCCalendario.ascx" TagName="UCCalendario" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoHorario.ascx" TagName="UCComboTipoHorario" TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCTurmaDisciplina.ascx" TagName="UCCTurmaDisciplina" TagPrefix="uc3" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMensagem" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="lblMensagem" runat="server" EnableViewState="false"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <fieldset>
        <legend>
            <asp:Literal ID="litLegendaQuadro" runat="server" Text="<%$ Resources:Turma, Turma.QuadroHorarios.litLegendaQuadro.Text %>"></asp:Literal>
        </legend>
        <asp:Label ID="lblDados" runat="server"></asp:Label>
        <uc1:UCCalendario runat="server" ID="UCCalendario" />
        <asp:HiddenField ID="hdnMinTime" runat="server" Value="" />
        <asp:HiddenField ID="hdnMaxTime" runat="server" Value="" />
        <asp:HiddenField ID="hdnDiasSemana" runat="server" ClientIDMode="Static" EnableViewState="true" />
        <div class="right">
            <asp:Button ID="btnCancelar" runat="server" Text="<%$ Resources:Turma, Turma.QuadroHorarios.btnCancelar.Text %>" OnClick="btnCancelar_Click" CausesValidation="false" />
            <asp:Button ID="btnVoltar" runat="server" Text="<%$ Resources:Turma, Turma.QuadroHorarios.btnVoltar.Text %>" OnClick="btnCancelar_Click" CausesValidation="false" />
        </div>
    </fieldset>
   
    <div id="divAtribuirDisciplina" runat="server" ClientIDMode="Static" title="<%$ Resources:Turma, Turma.QuadroHorarios.divAtribuirDisciplina.title %>" class="hide divAtribuirDisciplina">
        <asp:UpdatePanel ID="updAtribuirDisciplina" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset>
                    <asp:HiddenField ID="hdnHorario" runat="server" ClientIDMode="Static" EnableViewState="true" />

                    <asp:Label ID="lblDiaSemana" runat="server" Text="<%$ Resources:Turma, Turma.QuadroHorarios.lblDiaSemana.Text %>" AssociatedControlID="txtDiaSemana"></asp:Label>
                    <asp:TextBox ID="txtDiaSemana" runat="server" ClientIDMode="Static" Enabled="false" CssClass="text20C"></asp:TextBox>
                    <div class="clear"></div>
                    <div style="display: inline-block; float: left;">
                        <div style="display: inline-block">
                            <asp:Label ID="lblHoraInicial" runat="server" Text="<%$ Resources:Turma, Turma.QuadroHorarios.lblHoraInicial.Text %>" AssociatedControlID="txtHoraInicial"></asp:Label>
                            <asp:TextBox ID="txtHoraInicial" runat="server" CssClass="text10C maskHora txtHoraInicial" Enabled="false"></asp:TextBox>
                        </div>
                        <div style="display: inline-block" class="clear"></div>
                        <div style="display: inline-block">
                            <asp:Label ID="lblHoraFinal" runat="server" Text="<%$ Resources:Turma, Turma.QuadroHorarios.lblHoraFinal.Text %>" AssociatedControlID="txtHoraFinal"></asp:Label>
                            <asp:TextBox ID="txtHoraFinal" runat="server" CssClass="text10C maskHora txtHoraFinal" Enabled="false"></asp:TextBox>
                        </div>
                    </div>
                    <div class="ddlTipoHorario">
                        <uc2:UCComboTipoHorario ID="UCComboTipoHorario" runat="server" PermiteEditar="false" />
                    </div>
                    <div class="ddlTurmaDisciplina">
                        <uc3:UCCTurmaDisciplina ID="UCCTurmaDisciplina" runat="server" MostrarMensagemSelecione="true"  PermiteEditar="false" />
                    </div>
                    <div class="right">
                        <asp:Button ID="btnCancelarAtribuicao" runat="server" Text="<%$ Resources:Turma, Turma.QuadroHorarios.btnCancelarAtribuicao.Text %>" OnClientClick='$(".divAtribuirDisciplina").dialog("close"); return false;' ClientIDMode="Static" />
                        <asp:Button ID="btnFecharAtribuicao" runat="server" Text="<%$ Resources:Turma, Turma.QuadroHorarios.btnFecharAtribuicao.Text %>" OnClientClick='$(".divAtribuirDisciplina").dialog("close"); return false;' ClientIDMode="Static" />
                    </div>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
