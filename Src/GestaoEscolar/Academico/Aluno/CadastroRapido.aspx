<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Academico_Aluno_CadastroRapido" Codebehind="CadastroRapido.aspx.cs" %>

<%@ Register Src="../../WebControls/Combos/UCComboEstadoCivil.ascx" TagName="UCComboEstadoCivil"
    TagPrefix="uc1" %>
<%@ Register Src="../../WebControls/Combos/UCComboSexo.ascx" TagName="UCComboSexo"
    TagPrefix="uc2" %>
<%@ Register Src="../../WebControls/Endereco/UCEnderecos.ascx" TagName="UCEnderecos"
    TagPrefix="uc3" %>
<%@ Register Src="../../WebControls/Combos/UCComboRacaCor.ascx" TagName="UCComboRacaCor"
    TagPrefix="uc4" %>
<%@ Register Src="../../WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc5" %>
<%@ Register Src="../../WebControls/FiltroEscolas/UCFiltroEscolas.ascx" TagName="UCFiltroEscolas"
    TagPrefix="uc6" %>
<%@ Register Src="../../WebControls/Combos/UCComboCursoCurriculo.ascx" TagName="UCComboCursoCurriculo"
    TagPrefix="uc7" %>
<%@ Register Src="../../WebControls/Combos/UCComboCurriculoPeriodo.ascx" TagName="UCComboCurriculoPeriodo"
    TagPrefix="uc8" %>
<%@ Register Src="../../WebControls/Contato/UCGridContato.ascx" TagName="UCGridContato"
    TagPrefix="uc9" %>
<%@ Register Src="../../WebControls/Combos/UCComboTipoDeficiencia.ascx" TagName="UCComboTipoDeficiencia"
    TagPrefix="uc10" %>
<%@ Register Src="../../WebControls/Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="updMensagem" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <fieldset>
        <legend>Cadastro rápido de alunos</legend>
        <uc5:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" />
        <asp:Label ID="lblMatriculaEstadual" runat="server" Text="Matrícula estadual *" AssociatedControlID="txtMatriculaEstadual"></asp:Label>
        <asp:TextBox ID="txtMatriculaEstadual" runat="server" MaxLength="50" SkinID="text30C"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvMatriculaEstadual" runat="server" ControlToValidate="txtMatriculaEstadual"
            Display="Dynamic">*</asp:RequiredFieldValidator>
        <uc10:UCComboTipoDeficiencia ID="UCComboTipoDeficiencia1" runat="server" />
        <asp:Label ID="LabelNome" runat="server" Text="Nome completo do aluno (sem abreviações) *"
            AssociatedControlID="txtNome"></asp:Label>
        <asp:TextBox ID="txtNome" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvNome" runat="server" ControlToValidate="txtNome"
            ErrorMessage="Nome completo do aluno (sem abreviações) é obrigatório." Display="Dynamic">*</asp:RequiredFieldValidator>
        <uc2:UCComboSexo ID="UCComboSexo1" runat="server" />
        <asp:CompareValidator ID="cvSexo" runat="server" ErrorMessage="Sexo é obrigatório."
            ControlToValidate="UCComboSexo1:_ddlSexo" Operator="GreaterThan" ValueToCompare="0"
            Display="Dynamic">*</asp:CompareValidator>
        <uc4:UCComboRacaCor ID="UCComboRacaCor1" runat="server" />
        <asp:CompareValidator ID="cvRacaCor" runat="server" ErrorMessage="Raça / cor é obrigatório."
            ControlToValidate="UCComboRacaCor1:_ddlRacaCor" Operator="GreaterThan" ValueToCompare="0"
            Display="Dynamic">*</asp:CompareValidator>
        <uc1:UCComboEstadoCivil ID="UCComboEstadoCivil1" runat="server" />
        <asp:CompareValidator ID="cvEstadoCivil" runat="server" ErrorMessage="Estado civil é obrigatório."
            ControlToValidate="UCComboEstadoCivil1:_ddlEstadoCivil" Operator="GreaterThan"
            ValueToCompare="0" Display="Dynamic">*</asp:CompareValidator>
        <asp:Label ID="LabelDataNasc" runat="server" Text="Data de nascimento do aluno *"
            AssociatedControlID="txtDataNasc"></asp:Label>
        <asp:TextBox ID="txtDataNasc" runat="server" MaxLength="10" SkinID="DataSemCalendario"></asp:TextBox>
        <asp:Label ID="lblFormatoData1" runat="server" Text="(DD/MM/AAAA)"></asp:Label>
        <asp:RequiredFieldValidator ID="_rfvData" runat="server" ControlToValidate="txtDataNasc"
            ErrorMessage="Data de nascimento do aluno é obrigatório." Display="Dynamic">*</asp:RequiredFieldValidator>
        <asp:CustomValidator ID="cvDataNascimento" runat="server" ControlToValidate="txtDataNasc"
            Display="Dynamic" ErrorMessage="" OnServerValidate="ValidarData_ServerValidate">* </asp:CustomValidator>
        <br />
        <asp:Label ID="_lblSituacao" runat="server" Text="Situação *"></asp:Label>
        <br />
        <asp:DropDownList ID="_ddlSituacao" runat="server" AutoPostBack="True" 
            onselectedindexchanged="_ddlSituacao_SelectedIndexChanged">
            <asp:ListItem Value="-1">-- Selecione uma situação --</asp:ListItem>
            <asp:ListItem Value="1">Ativo</asp:ListItem>
            <asp:ListItem Value="7">Em matrícula</asp:ListItem>
            <asp:ListItem Value="8">Excedente</asp:ListItem>
        </asp:DropDownList>
        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Situação é obrigatório."
            ControlToValidate="_ddlSituacao" Operator="GreaterThan" ValueToCompare="0" Display="Dynamic">*</asp:CompareValidator>
    </fieldset>
    <fieldset>
        <legend>Filiação</legend>
        <asp:Label ID="LabelMae" runat="server" Text="Nome completo da mãe (sem abreviações)"
            AssociatedControlID="txtMae"></asp:Label>
        <asp:TextBox ID="txtMae" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
        <asp:Label ID="LabelCPFMae" runat="server" Text="CPF da mãe" AssociatedControlID="txtCPFMae"></asp:Label>
        <asp:TextBox ID="txtCPFMae" runat="server" MaxLength="11" SkinID="Numerico" CssClass="numeric"></asp:TextBox>
        <asp:RegularExpressionValidator ID="revCPFMae" runat="server" ControlToValidate="txtCPFMae"
            Display="Dynamic" ErrorMessage="CPF da mãe inválido." ValidationExpression="^([0-9]){11}$">*</asp:RegularExpressionValidator>
        <asp:Label ID="LabelPai" runat="server" Text="Nome completo do pai (sem abreviações)"
            AssociatedControlID="txtPai"></asp:Label>
        <asp:TextBox ID="txtPai" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
        <asp:Label ID="LabelCPFPai" runat="server" Text="CPF do pai" AssociatedControlID="txtCPFPai"></asp:Label>
        <asp:TextBox ID="txtCPFPai" runat="server" MaxLength="11" SkinID="Numerico" CssClass="numeric"></asp:TextBox>
        <asp:RegularExpressionValidator ID="revCPFPai" runat="server" ControlToValidate="txtCPFPai"
            Display="Dynamic" ErrorMessage="CPF do pai inválido." ValidationExpression="^([0-9]){11}$">*</asp:RegularExpressionValidator>
    </fieldset>
    <fieldset>
        <legend>Endereço</legend>
        <uc3:UCEnderecos ID="UCEnderecos1" runat="server" />
    </fieldset>
    <fieldset>
        <legend>Dados da certidão de nascimento</legend>
        <asp:Label ID="LabelNaturalidade" runat="server" Text="Município de nascimento do aluno *"
            AssociatedControlID="txtNaturalidade"></asp:Label>
        <input id="_txtCid_id" runat="server" type="hidden" class="tbCid_idNaturalidade_incremental" />
        <asp:TextBox ID="txtNaturalidade" runat="server" MaxLength="200" CssClass="text30C tbNaturalidade_incremental"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvNaturalidade" runat="server" ControlToValidate="txtNaturalidade"
            ErrorMessage="Município de nascimento do aluno é obrigatório." Display="Dynamic">*</asp:RequiredFieldValidator>
        <asp:Label ID="LabelCidadeCertidao" runat="server" Text="Município da comarca da certidão de nascimento *"
            AssociatedControlID="txtCidadeCertidao"></asp:Label>
        <input id="_txtCid_idCertidao" runat="server" type="hidden" class="tbCid_idCertidao_incremental" />
        <asp:TextBox ID="txtCidadeCertidao" runat="server" MaxLength="200" CssClass="text30C tbCidadeCertidao_incremental"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvCidadeCertidao" runat="server" ControlToValidate="txtCidadeCertidao"
            ErrorMessage="Município da comarca da certidão de nascimento é obrigatório."
            Display="Dynamic">*</asp:RequiredFieldValidator>
        <asp:Label ID="LabelDistritoCertidao" runat="server" Text="Distrito da certidão de nascimento *"
            AssociatedControlID="txtDistritoCertidao"></asp:Label>
        <asp:TextBox ID="txtDistritoCertidao" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDistritoCertidao"
            ErrorMessage="Distrito da certidão de nascimento é obrigatório." Display="Dynamic">*</asp:RequiredFieldValidator>
        <asp:Label ID="Label4" runat="server" Text="Data de emissão da certidão de nascimento"
            AssociatedControlID="txtDataEmissao"></asp:Label>
        <asp:TextBox ID="txtDataEmissao" runat="server" SkinID="DataSemCalendario"></asp:TextBox>
        <asp:Label ID="lblFormatoData2" runat="server" Text="(DD/MM/AAAA)"></asp:Label>
        <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="txtDataEmissao"
            Display="Dynamic" ErrorMessage=""
            OnServerValidate="ValidarData_ServerValidate">* </asp:CustomValidator>
        <asp:Label ID="Label2" runat="server" Text="Folha" AssociatedControlID="txtFolha"></asp:Label>
        <asp:TextBox ID="txtFolha" runat="server" SkinID="text15C" MaxLength="20"></asp:TextBox>
        <asp:Label ID="Label3" runat="server" Text="Livro" AssociatedControlID="txtLivro"></asp:Label>
        <asp:TextBox ID="txtLivro" runat="server" SkinID="text15C" MaxLength="20"></asp:TextBox>
        <asp:Label ID="Label1" runat="server" Text="Número da certidão" AssociatedControlID="txtNumeroTermo"></asp:Label>
        <asp:TextBox ID="txtNumeroTermo" runat="server" SkinID="text15C" MaxLength="50"></asp:TextBox>
    </fieldset>
    <fieldset>
        <legend>Contatos</legend>
        <uc9:UCGridContato ID="UCGridContato1" runat="server" />
    </fieldset>
    <asp:UpdatePanel ID="updMatricula" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UCLoader ID="UCLoader1" runat="server" AssociatedUpdatePanelID="updMatricula" />
            <fieldset>
                <legend>Matrícula</legend>
                <asp:Label ID="lblMessageMatricula" runat="server" EnableViewState="False"></asp:Label>
                <uc6:UCFiltroEscolas ID="UCFiltroEscolas1" runat="server" />
                <uc7:UCComboCursoCurriculo ID="UCComboCursoCurriculo1" runat="server" />                
                <uc8:UCComboCurriculoPeriodo ID="UCComboCurriculoPeriodo1" runat="server" />
                <asp:CompareValidator ID="cvCurriculoPeriodo" runat="server" ControlToValidate="UCComboCurriculoPeriodo1:_ddlCurriculoPeriodo"
                    Operator="GreaterThan" ValueToCompare="0" Display="Dynamic">*</asp:CompareValidator>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
    <fieldset>
        <div class="right">
            <asp:Button ID="btnSalvarNovo" runat="server" Text="Salvar e incluir novo aluno"
                OnClick="_btnSalvar_Click" />
            <asp:Button ID="_btnSalvar" runat="server" Text="Salvar" OnClick="_btnSalvar_Click" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CausesValidation="false"
                OnClick="btnCancelar_Click" />
            <input id="txtSelectedTab" type="hidden" class="txtSelectedTab" runat="server" />
        </div>
    </fieldset>
</asp:Content>
