<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebControls_Cargo_UCCadastroCargo"
    CodeBehind="UCCadastroCargo.ascx.cs" %>
<%@ Register Src="../Combos/UCComboCargo.ascx" TagName="UCComboCargo" TagPrefix="uc1" %>
<%@ Register Src="../Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc3" %>
<%@ Register Src="../Combos/UCComboCargaHoraria.ascx" TagName="UCComboCargaHoraria"
    TagPrefix="uc2" %>
<asp:UpdatePanel ID="updCadastroCargos" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="divCadastro" runat="server" visible="false">
            <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Cargo" />
            <fieldset>
                <uc3:UCCamposObrigatorios ID="UCCamposObrigatorios2" runat="server" />
                <uc1:UCComboCargo ID="UCComboCargo1" runat="server" />
                <fieldset id="fsDisciplinas" runat="server" visible="true">
                    <legend><asp:Label runat="server" ID="lblLegend"></asp:Label></legend>
                    <div>
                        <div style="overflow: auto; height: 150px;">
                            <asp:CheckBoxList ID="cblDisciplinasPossiveis" runat="server"
                                DataTextField="tne_tds_nome" DataValueField="tds_id">
                            </asp:CheckBoxList>
                        </div>
                    </div>
                    <asp:ObjectDataSource ID="odsDisciplinas" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_TipoDisciplina"
                        SelectMethod="SelecionaTipoDisciplina" TypeName="MSTech.GestaoEscolar.BLL.ACA_TipoDisciplinaBO">
                        <SelectParameters>
                             <asp:Parameter Name="ent_id" DbType="Guid" />
                            <asp:Parameter Name="AppMinutosCacheLongo" DbType="Int32" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </fieldset>
                <asp:Label ID="LabelMatricula" runat="server" Text="Matrícula" AssociatedControlID="txtMatricula"></asp:Label>
                <asp:TextBox ID="txtMatricula" runat="server" MaxLength="50" SkinID="text20C"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvMatricula" runat="server" ErrorMessage="Matrícula é obrigatório."
                    Display="Dynamic" ControlToValidate="txtMatricula" ValidationGroup="Cargo" Visible="False">*</asp:RequiredFieldValidator>
                <asp:CheckBox ID="ckbVinculoSede" runat="server" Text="Vínculo sede" />
                <asp:CheckBox ID="ckbVinculoExtra" runat="server" Text="Vínculo extra" />
                <asp:CheckBox ID="ckbComplementacaoCargaHoraria" runat="server" Text="Complementação de carga horária" />
                <asp:CheckBox ID="ckbColaboradorReadaptado" runat="server" Text="Readaptado" />                
                <asp:Label ID="LabelUA" runat="server" Text="Unidade administrativa *" AssociatedControlID="btnPesquisarUA"></asp:Label>
                <asp:TextBox ID="txtUA" runat="server" Enabled="False" MaxLength="200" SkinID="text30C"></asp:TextBox>
                <asp:ImageButton ID="btnPesquisarUA" runat="server" SkinID="btPesquisar" CausesValidation="False"
                    OnClick="btnPesquisarUA_Click" />
                <asp:RequiredFieldValidator ID="rfvUA" ControlToValidate="txtUA" ValidationGroup="Cargo"
                    runat="server" ErrorMessage="Unidade administrativa é obrigatório.">*</asp:RequiredFieldValidator>
                <asp:Label ID="LabelVigenciaIni" runat="server" Text="Vigência inicial *" AssociatedControlID="txtVigenciaIni"></asp:Label>
                <asp:TextBox ID="txtVigenciaIni" runat="server" MaxLength="10" SkinID="Data"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvVigenciaInicial" ControlToValidate="txtVigenciaIni"
                    ValidationGroup="Cargo" runat="server" ErrorMessage="Vigência inicial é obrigatório.">*</asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cvVigenciIni" runat="server" ControlToValidate="txtVigenciaIni"
                    ValidationGroup="Cargo" Display="Dynamic" ErrorMessage="" OnServerValidate="ValidarData_ServerValidate">* </asp:CustomValidator>
                <asp:Label ID="LabelVigenciaFinal" runat="server" Text="Vigência final" AssociatedControlID="txtVigenciaFim"></asp:Label>
                <asp:TextBox ID="txtVigenciaFim" runat="server" MaxLength="10" SkinID="Data"></asp:TextBox>
                <asp:CustomValidator ID="cvVigenciaFim" runat="server" ControlToValidate="txtVigenciaFim"
                    ValidationGroup="Cargo" Display="Dynamic" ErrorMessage="" OnServerValidate="ValidarData_ServerValidate">* </asp:CustomValidator>
                <asp:Label ID="LabelDataInicioMatricula" runat="server" Text="Data de início da matrícula"
                    AssociatedControlID="_txtDataInicioMatricula"></asp:Label>
                <asp:TextBox ID="_txtDataInicioMatricula" runat="server" MaxLength="10" SkinID="Data"></asp:TextBox>
                <uc2:UCComboCargaHoraria ID="UCComboCargaHoraria1" runat="server" ValidationGroup="Cargo" />
                <asp:Label ID="LabelCargoSituacao" runat="server" Text="Situação *" AssociatedControlID="ddlCargoSituacao"></asp:Label>
                <asp:DropDownList ID="ddlCargoSituacao" runat="server" AppendDataBoundItems="True"
                    SkinID="text30C">
                    <asp:ListItem Value="-1">-- Selecione uma situação --</asp:ListItem>
                    <asp:ListItem Value="1">Ativo</asp:ListItem>
                    <asp:ListItem Value="4">Designado</asp:ListItem>
                    <asp:ListItem Value="5">Afastado</asp:ListItem>
                    <asp:ListItem Value="6">Desativado</asp:ListItem>
                </asp:DropDownList>
                <asp:CompareValidator ID="cpvCargoSituacao" runat="server" ErrorMessage="Situação é obrigatório."
                    ControlToValidate="ddlCargoSituacao" Operator="GreaterThan" ValueToCompare="0"
                    Display="Dynamic" ValidationGroup="Cargo">*</asp:CompareValidator>
                <asp:Label ID="LabelObservacao" runat="server" Text="Observação" AssociatedControlID="txtObservacao"></asp:Label>
                <asp:TextBox ID="txtObservacao" runat="server" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox>
                <div class="right">
                    <asp:Button ID="_btnIncluir" runat="server" Text="Incluir" ValidationGroup="Cargo"
                        CausesValidation="true" OnClick="_btnIncluir_Click" />
                    <asp:Button ID="_btnCancelar" runat="server" Text="Cancelar" OnClientClick="$('#divCargos').dialog('close');"
                        CausesValidation="False" />
                </div>
            </fieldset>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
