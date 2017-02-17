<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="WebControls_FiltroEscolas_UCFiltroEscolas" Codebehind="UCFiltroEscolas.ascx.cs" %>
    
<%@ Register Src="../Combos/UCComboUnidadeEscola.ascx" TagName="UCComboUnidadeEscola"
    TagPrefix="uc1" %>
<%@ Register Src="../Combos/UCComboUnidadeAdministrativa.ascx" TagName="UCComboUnidadeAdministrativa"
    TagPrefix="uc2" %>
 
<uc2:UCComboUnidadeAdministrativa ID="UCComboUnidadeAdministrativa1" runat="server" />
<asp:CompareValidator ID="cvUnidadeAdministrativa" runat="server" ErrorMessage="Unidade administrativa é obrigatório."
    ControlToValidate="UCComboUnidadeAdministrativa1:_ddlUA" Operator="NotEqual"
    ValueToCompare="00000000-0000-0000-0000-000000000000" Display="Dynamic">*</asp:CompareValidator>
<uc1:UCComboUnidadeEscola ID="UCComboUnidadeEscola1" runat="server" />
<asp:CompareValidator ID="cvEscola" runat="server" ErrorMessage="Escola é obrigatório."
    ControlToValidate="UCComboUnidadeEscola1:_ddlUnidadeEscola" Operator="NotEqual"
    ValueToCompare="-1;-1" Display="Dynamic">*</asp:CompareValidator>
