<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Academico_RecursosHumanos_Cargo_Cadastro" Title="Cadastro" CodeBehind="Cadastro.aspx.cs" %>

<%@ PreviousPageType VirtualPath="~/Academico/RecursosHumanos/Cargo/Busca.aspx" %>
<%@ Register Src="../../../WebControls/Combos/UCComboTipoVinculo.ascx" TagName="UCComboTipoVinculo"
    TagPrefix="uc2" %>
<%@ Register Src="../../../WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc3" %>
<%@ Register Src="../../../WebControls/Combos/UCComboParametroGrupoPerfil.ascx" TagName="UCComboParametroGrupoPerfil"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="_updCadastroCargo" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Cargo" />
            <fieldset id="fdsCargos" runat="server" >
                <legend>Cadastro de cargos</legend>
                <uc3:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" />
                <uc2:UCComboTipoVinculo ID="_UCComboTipoVinculo" runat="server" />
                <asp:Label ID="_lblNome" runat="server" Text="Nome do cargo *" AssociatedControlID="_txtNome"></asp:Label>
                <asp:TextBox ID="_txtNome" runat="server" SkinID="text60C" MaxLength="100"></asp:TextBox>
                <asp:RequiredFieldValidator ID="_rfvNome" runat="server" ErrorMessage="Nome do cargo é obrigatório."
                    ControlToValidate="_txtNome" ValidationGroup="Cargo">*</asp:RequiredFieldValidator>
                <asp:Label ID="_lblCodigo" runat="server" Text="Código do cargo" AssociatedControlID="_txtCodigo"></asp:Label>
                <asp:TextBox ID="_txtCodigo" runat="server" SkinID="text10C" MaxLength="20"></asp:TextBox>
                <asp:Label ID="_lblDescricao" runat="server" Text="Descrição" AssociatedControlID="_txtDescricao"></asp:Label>
                <asp:TextBox ID="_txtDescricao" runat="server" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox>
                <asp:Label ID="_lblCodIntegracao" runat="server" Text="Código de integração" AssociatedControlID="_txtCodIntegracao"></asp:Label>
                <asp:TextBox ID="_txtCodIntegracao" runat="server" SkinID="text20C" MaxLength="20"></asp:TextBox>
                <uc1:UCComboParametroGrupoPerfil ID="UCComboParametroGrupoPerfil1" runat="server" />
                <asp:Label ID="Label1" runat="server" Text="Tipo de cargo" AssociatedControlID="ddlTipo"></asp:Label>
                <asp:DropDownList ID="ddlTipo" runat="server">
                    <asp:ListItem Value="1" Text="Comum" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="2" Text="Cargo base"></asp:ListItem>
                    <asp:ListItem Value="3" Text="Atribuição esporádica"></asp:ListItem>
                    <asp:ListItem Value="4" Text="Indireto"></asp:ListItem>
                </asp:DropDownList>
                <asp:CheckBox ID="ckbControleIntegracao" runat="server" Text="<%$ Resources:Academico, RecursosHumanos.Cargo.Cadastro.ckbControladoPelaIntegracao.Text %>" Enabled="false"/>
                <asp:CheckBox ID="_ckbBloqueado" runat="server" Text="Bloqueado" />
                <asp:CheckBox ID="_ckbCargoDocente" runat="server" Text="Cargo docente" OnCheckedChanged="_cbkCargoDocente_CheckedChanged"
                    AutoPostBack="true" />
                <div id="divCargoDocente" runat="server">
                    <asp:Label ID="_lblMaxAulaDia" runat="server" Text="Máximo de aulas por dia *" AssociatedControlID="_txtMaxAulaDia"></asp:Label>
                    <asp:TextBox ID="_txtMaxAulaDia" runat="server" MaxLength="2" CssClass="numeric"
                        SkinID="Numerico"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="_rfvMaxAulaDia" runat="server" ErrorMessage="Máximo de aulas por dia é obrigatório."
                        ControlToValidate="_txtMaxAulaDia" ValidationGroup="Cargo">*</asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="_cpvMaxAulaDia" runat="server" Text="*" ErrorMessage="Máximo de aulas por dia não pode ser maior do que 24."
                        ControlToValidate="_txtMaxAulaDia" Type="Integer" Operator="LessThanEqual" ValueToCompare="24"
                        Display="Dynamic" ValidationGroup="Cargo">*</asp:CompareValidator>
                    <asp:RegularExpressionValidator ID="_revMaxAulaDia" runat="server" ControlToValidate="_txtMaxAulaDia"
                        Display="Dynamic" ErrorMessage="Máximo de aulas por dia inválido." ValidationExpression="^([0-9]){1,10}$">*</asp:RegularExpressionValidator>
                    <asp:Label ID="_lblMaxAulaSemana" runat="server" Text="Máximo de aulas por semana *"
                        AssociatedControlID="_txtMaxAulaSemana"></asp:Label>
                    <asp:TextBox ID="_txtMaxAulaSemana" runat="server" MaxLength="3" CssClass="numeric"
                        SkinID="Numerico"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="_rfvMaxAulaSemana" runat="server" ErrorMessage="Máximo de aulas por semana é obrigatório."
                        ControlToValidate="_txtMaxAulaSemana" ValidationGroup="Cargo">*</asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="_cpvMaxAulaSemana" runat="server" Text="*" ErrorMessage="Máximo de aulas por semana não pode ser maior do que 168."
                        ControlToValidate="_txtMaxAulaSemana" Type="Integer" Operator="LessThanEqual"
                        ValueToCompare="168" Display="Dynamic" ValidationGroup="Cargo">*</asp:CompareValidator>
                    <asp:RegularExpressionValidator ID="_revMaxAulaSemana" runat="server" ControlToValidate="_txtMaxAulaSemana"
                        Display="Dynamic" ErrorMessage="Máximo de aulas por semana inválido." ValidationExpression="^([0-9]){1,10}$"
                        ValidationGroup="Cargo">*</asp:RegularExpressionValidator>
                    <asp:CheckBox ID="_ckbEspecialista" runat="server" OnCheckedChanged="_cbkCargoEspecialista_CheckedChanged" Text="<%$ Resources:Academico, RecursosHumanos.Cargo.Cadastro._ckbEspecialista.Text %>"
                    AutoPostBack="true"/>
                </div>
                <div id="divDisciplinas" runat="server">
                        <fieldset id="fsDisciplinas" runat="server">
                            <legend><asp:Label runat="server" ID="lblLegend" Text="<%$ Resources:Mensagens, MSG_DISCIPLINA_PLURAL %>"></asp:Label></legend>
                            <div style="overflow: auto; height: 150px;">
                                <asp:CheckBoxList ID="cblDisciplinasPossiveis" runat="server" DataSourceID="odsDisciplinas"
                                    DataTextField="tne_tds_nome" DataValueField="tds_id">
                                </asp:CheckBoxList>
                            </div>
                             <asp:ObjectDataSource ID="odsDisciplinas" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_TipoDisciplina"
                                SelectMethod="SelecionaTipoDisciplina" TypeName="MSTech.GestaoEscolar.BLL.ACA_TipoDisciplinaBO">                        
                                 <SelectParameters>
                                     <asp:Parameter Name="ent_id" DbType="Guid" />
                                     <asp:Parameter Name="AppMinutosCacheLongo" DbType="Int32" />
                                 </SelectParameters>
                            </asp:ObjectDataSource>
                        </fieldset>
                </div>
                <div class="right">
                    <asp:Button ID="_btnSalvar" runat="server" Text="Salvar" OnClick="_btnSalvar_Click"
                        ValidationGroup="Cargo" />
                    <asp:Button ID="_btnCancelar" runat="server" Text="Cancelar" CausesValidation="false"
                        OnClick="_btnCancelar_Click" />
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
