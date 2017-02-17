<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCSelecaoDisciplinaCompartilhada.ascx.cs" Inherits="GestaoEscolar.WebControls.ControleTurma.UCSelecaoDisciplinaCompartilhada" %>
<%@ Register src="~/WebControls/Combos/UCComboTurmaDisciplinaRelacionada.ascx" tagname="UCComboTurmaDisciplinaRelacionada" tagprefix="uc10" %>

<%-- Disciplinas compartilhadas --%>
<div id="divDisciplinasCompartilhadas" class="hide">
    <fieldset>
        <asp:UpdatePanel ID="updDisciplinasCompartilhadas" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div style="min-height:70px">
                    <uc10:UCComboTurmaDisciplinaRelacionada ID="UCComboTurmaDisciplinaRelacionada" runat="server" Obrigatorio="true" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="right">
            <asp:Button ID="btnSelecionar" runat="server" Text="Selecionar" OnClick="btnSelecionar_Click" />
        </div>
    </fieldset>
</div>