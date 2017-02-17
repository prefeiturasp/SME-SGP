<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Configuracao_TipoDisciplina_Cadastro" CodeBehind="Cadastro.aspx.cs" %>


<%@ PreviousPageType VirtualPath="~/Configuracao/TipoDisciplina/Busca.aspx" %>
<%@ Register Src="../../WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc36" %>
<%@ Register Src="../../WebControls/Combos/UCComboTipoNivelEnsino.ascx" TagName="UCComboTipoNivelEnsino"
    TagPrefix="uc2" %>
<%@ Register src="../../WebControls/Combos/UCComboAreaConhecimento.ascx" tagname="UCComboAreaConhecimento" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server"/> 
    <fieldset>
        <legend>
            <asp:Label runat="server" ID="lblLegend" Text="<%$ Resources:Configuracao, TipoDisciplina.Cadastro.lblLegend.Text %>"></asp:Label></legend>
        <uc36:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" />
        <asp:Label ID="_lblTipoDisciplina" runat="server" Text="<%$ Resources:Configuracao, TipoDisciplina.Cadastro._lblTipoDisciplina.Text %>" AssociatedControlID="_txtTipoDisciplina"></asp:Label>
        
        <asp:TextBox ID="_txtTipoDisciplina" runat="server" MaxLength="100" SkinID="text60C" Enabled="false"></asp:TextBox>
        <asp:RequiredFieldValidator ID="_rfvTipoDisciplina" runat="server" ControlToValidate="_txtTipoDisciplina"
            Display="Dynamic">*</asp:RequiredFieldValidator>

        <uc2:UCComboTipoNivelEnsino ID="UCComboTipoNivelEnsino1" runat="server" PermiteEditar="false" />
        <asp:Label ID="_lblBase" runat="server" Text="Base *" AssociatedControlID="_ddlBase"></asp:Label>
        <asp:DropDownList ID="_ddlBase" runat="server" Enabled="false">
            <asp:ListItem Text="-- Selecione uma base --" Value="-1"></asp:ListItem>
            <asp:ListItem Text="Nacional " Value="1"></asp:ListItem>
            <asp:ListItem Text="Diversificada" Value="2"></asp:ListItem>
        </asp:DropDownList>
        <asp:CompareValidator ID="_cvBase" runat="server" ErrorMessage="Base é obrigatório."
            ControlToValidate="_ddlBase" Operator="GreaterThan" ValueToCompare="0" Display="Dynamic">*</asp:CompareValidator>

        <asp:CheckBox ID="_ckbBloqueado" runat="server" Text="Bloqueado" Visible="false" Enabled="false" />

        <asp:CheckBox ID="ckbAlunoEspecial" runat="server" Text="Separar lançamentos de alunos especiais"
            AutoPostBack="true" OnCheckedChanged="ckbAlunoEspecial_CheckedChanged" Enabled="false" />

        <div id="divDisciplinaEspecial" runat="Server">
            <asp:UpdatePanel ID="_updGridDeficiencia" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="lblNomeDisciplinaEspecial" runat="server" Text="<%$ Resources:Configuracao, TipoDisciplina.Cadastro.lblNomeDisciplinaEspecial.Text %>" AssociatedControlID="txtNomeDisciplinaEspecial"></asp:Label>
                    <asp:TextBox ID="txtNomeDisciplinaEspecial" runat="server" MaxLength="100" SkinID="text60C" Enabled="false"></asp:TextBox>
                     <br />
                     <br />
                     <fieldset id="_fdsDeficiencias" runat="server">
                          <asp:Label ID="lblMessageDeficiencia" runat="server" EnableViewState="False"></asp:Label>
                          
                          <asp:Panel ID="pnlDeficiencias" runat="server">
                              <asp:CheckBoxList ID="chkDeficiencias" runat="server" Enabled="false">
                              </asp:CheckBoxList>
                          </asp:Panel>
                     </fieldset>
                </ContentTemplate>
             </asp:UpdatePanel>   
        </div>

        <uc1:UCComboAreaConhecimento ID="UCComboAreaConhecimento1" runat="server" PermiteEditar="false" />

        <asp:Label ID="lblQtdeDisciplinaRelacionada" runat="server" Text="<%$ Resources:Configuracao, TipoDisciplina.Cadastro.lblQtdeDisciplinaRelacionada.Text %>" AssociatedControlID="txtQtdeDisciplinaRelacionada"></asp:Label>      
        <asp:TextBox ID="txtQtdeDisciplinaRelacionada" runat="server" MaxLength="3" SkinID="Numerico" Enabled="false"></asp:TextBox>

        <div class="right">
            <asp:Button ID="_btnCancelar" runat="server" Text="Voltar" CausesValidation="false"
                OnClick="_btnCancelar_Click" />
        </div>
    </fieldset>
</asp:Content>
