<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebControls_Pessoa_UCCadastroPessoa"
    CodeBehind="UCCadastroPessoa.ascx.cs" %>
<%@ Register Src="../Combos/UCComboEstadoCivil.ascx" TagName="UCComboEstadoCivil"
    TagPrefix="uc1" %>
<%@ Register Src="../Combos/UCComboSexo.ascx" TagName="UCComboSexo" TagPrefix="uc2" %>
<%@ Register Src="../Combos/ComboNacionalidade.ascx" TagName="ComboNacionalidade"
    TagPrefix="uc3" %>
<%@ Register Src="../Combos/UCComboRacaCor.ascx" TagName="UCComboRacaCor" TagPrefix="uc4" %>
<%@ Register Src="../Combos/UCComboTipoEscolaridade.ascx" TagName="UCComboTipoEscolaridade"
    TagPrefix="uc5" %>
<%@ Register Src="../Combos/ComboTipoDeficiencia.ascx" TagName="ComboTipoDeficiencia"
    TagPrefix="uc6" %>
<%@ Register Src="../AlunoDeficiente/UCAlunoDeficiente.ascx" TagName="UCAlunoDeficiente"
    TagPrefix="uc7" %>
<%@ Register Src="../Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc8" %>
<asp:UpdatePanel ID="_updCadastroPessoa" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <uc8:UCLoader ID="UCLoader1" runat="server" AssociatedUpdatePanelID="_updCadastroPessoa" />
        <asp:Label ID="lblMensagem" runat="server" Text="" EnableViewState="false"></asp:Label>
        <table class="table-padding">
            <tr>
                <td colspan="3">
                    <asp:Label ID="lblNome" runat="server" Text="Nome *" AssociatedControlID="txtNome"></asp:Label>
                    <asp:TextBox ID="txtNome" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="_rfvNome" runat="server" ControlToValidate="txtNome"
                        ValidationGroup="Pessoa" ErrorMessage="Nome é obrigatório." Display="Dynamic">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr runat="server" id="trNomeSocial" visible="false">
                <td colspan="3">
                    <asp:Label ID="lblNomeSocial" runat="server" Text="<%$ Resources:UserControl, Pessoa.UCCadastroPessoa.lblNomeSocial.Text %>" AssociatedControlID="txtNomeSocial"></asp:Label>
                    <asp:TextBox ID="txtNomeSocial" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%--<asp:Label ID="LabelNomeAbreviado" runat="server" Text="Nome abreviado" AssociatedControlID="txtNomeAbreviado"></asp:Label>--%>
                    <asp:Label ID="LabelNomeAbreviado" runat="server" Text="<%$ Resources:UserControl, Pessoa.UCCadastroPessoa.LabelNomeAbreviado.Text %>" AssociatedControlID="txtNomeAbreviado"></asp:Label>
                    <asp:TextBox ID="txtNomeAbreviado" runat="server" MaxLength="50" Width="235px"></asp:TextBox>
                </td>
                <td colspan="2">
                    <asp:Label ID="LabelDataNasc" runat="server" Text="Data de nascimento" AssociatedControlID="txtDataNasc"></asp:Label>
                    <asp:TextBox ID="txtDataNasc" runat="server" MaxLength="10" SkinID="DataSemCalendario"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="_rfvDataNasc" runat="server" ControlToValidate="txtDataNasc"
                        ValidationGroup="Pessoa" ErrorMessage="Data de nascimento é obrigatório." Display="Dynamic"
                        Visible="false">*</asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="cvDataNascimento" runat="server" ControlToValidate="txtDataNasc"
                        ValidationGroup="Pessoa" Display="Dynamic" ErrorMessage="" OnServerValidate="ValidarData_ServerValidate">* </asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <uc2:UCComboSexo ID="UCComboSexo1" Obrigatorio="false" ValidationGroup="Pessoa" runat="server" />
                </td>
                <td colspan="2">
                    <uc4:UCComboRacaCor ID="UCComboRacaCor1" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <uc1:UCComboEstadoCivil ID="UCComboEstadoCivil1" runat="server" />
                </td>
                <td>
                    <uc3:ComboNacionalidade ID="ComboNacionalidade1" runat="server" MostrarMensagemSelecione="true" />
                </td>
                <td valign="bottom">
                    <asp:CheckBox ID="chkNaturalizado" runat="server" Text="Naturalizado" />
                    <input id="_txtCid_id" runat="server" type="hidden" class="tbCid_idNaturalidade_incremental" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelNaturalidade" runat="server" Text="Naturalidade" AssociatedControlID="txtNaturalidade"></asp:Label>
                    <asp:TextBox ID="txtNaturalidade" runat="server" MaxLength="200" Width="210px" CssClass="tbNaturalidade_incremental"></asp:TextBox>
                    <asp:ImageButton ID="btnCadastraCidade" runat="server" SkinID="btNovo" CausesValidation="false"
                        ToolTip="Cadastrar nova cidade" OnClick="btnCadastraCidade_Click" Style="vertical-align: middle" />
                </td>
                <td colspan="2">
                    <uc6:ComboTipoDeficiencia ID="ComboTipoDeficiencia1" runat="server" MostrarMensagemSelecione="true" />
                </td>
            </tr>
            <tr id="trDeficientes" runat="server" visible="false" style="vertical-align: top;">
                <td colspan="3">
                    <uc7:UCAlunoDeficiente ID="UCAlunoDeficiente1" runat="server" />
                </td>
            </tr>
            <tr>
                <td id="tdFoto" runat="server">
                    <asp:UpdatePanel ID="upnFoto" runat="server" UpdateMode="Conditional">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnCapturaFoto" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:Label ID="LabelFoto" runat="server" Text="Foto" AssociatedControlID="iptFoto"></asp:Label>
                            <input type="file" id="iptFoto" name="Foto" runat="server" visible="true" style="width: 235px;" />
                            <br />
                            <asp:Image ID="imgFoto" runat="server" Visible="false" />
                            <asp:Label ID="lblDataFoto" runat="server" Visible="false"></asp:Label>
                            <br />                            
                            <asp:Label ID="lblMensagemErroFoto" runat="server" EnableViewState="false"></asp:Label>
                            <asp:CheckBox ID="chbExcluirImagem" runat="server" Visible="false" Text="Excluir foto" />
                            <asp:Button ID="btnExcluir" runat="server" Visible="false" Text="Excluir foto" OnClick="btnExcluir_Click" />
                            <asp:Button ID="btnCapturaFoto" runat="server" Text="Capturar foto" Visible="false"
                                OnClick="btnCapturaFoto_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td id="tdTipoEscolaridade" runat="server" colspan="2" valign="top">
                    <uc5:UCComboTipoEscolaridade ID="UCComboTipoEscolaridade1" runat="server" />
                </td>
            </tr>
        </table>
        <div id="divPai" runat="server">
            <asp:Label ID="LabelPai" runat="server" Text="Pai" AssociatedControlID="txtPai"></asp:Label>
            <input id="_txtPes_idFiliacaoPai" runat="server" type="hidden" />
            <asp:TextBox ID="txtPai" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvPai" runat="server" ControlToValidate="txtPai"
                ErrorMessage="Nome do pai é obrigatório." Display="Dynamic" Visible="false" ValidationGroup="Pessoa">*</asp:RequiredFieldValidator>
            <asp:ImageButton ID="_btnPai" runat="server" CausesValidation="False" SkinID="btPesquisar"
                OnClick="_btnPai_Click" />
            <asp:ImageButton ID="_btnLimparPai" runat="server" SkinID="btLimpar" CausesValidation="false"
                OnClick="_btnLimparPai_Click" Visible="false" />
            <table>
                <tr>
                    <td width="280">
                        <asp:Label ID="lblCPFPai" runat="server" AssociatedControlID="txtCPFPai" Text="CPF do Pai"></asp:Label>
                        <asp:TextBox ID="txtCPFPai" runat="server" MaxLength="11" SkinID="Numerico" Width="150"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblRGPai" runat="server" AssociatedControlID="txtRGPai" Text="RG do Pai"></asp:Label>
                        <asp:TextBox ID="txtRGPai" runat="server" MaxLength="50" SkinID="text10C" Width="150"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divMae" runat="server">
            <asp:Label ID="LabelMae" runat="server" Text="Mãe" AssociatedControlID="txtMae"></asp:Label>
            <input id="_txtPes_idFiliacaoMae" runat="server" type="hidden" />
            <asp:TextBox ID="txtMae" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvMae" runat="server" ControlToValidate="txtMae"
                ErrorMessage="Nome da mãe é obrigatório." Display="Dynamic" Visible="false" ValidationGroup="Pessoa">*</asp:RequiredFieldValidator>
            <asp:ImageButton ID="_btnMae" runat="server" CausesValidation="False" SkinID="btPesquisar"
                OnClick="_btnMae_Click" />
            <asp:ImageButton ID="_btnLimparMae" runat="server" SkinID="btLimpar" CausesValidation="false"
                OnClick="_btnLimparMae_Click" Visible="false" />
            <table>
                <tr>
                    <td width="280">
                        <asp:Label ID="lblCPFMae" runat="server" AssociatedControlID="txtCPFMae" Text="CPF da Mãe"></asp:Label>
                        <asp:TextBox ID="txtCPFMae" runat="server" MaxLength="11" SkinID="Numerico" Width="150"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblRGMae" runat="server" AssociatedControlID="txtRGMae" Text="RG da Mãe"></asp:Label>
                        <asp:TextBox ID="txtRGMae" runat="server" MaxLength="50" SkinID="text10C" Width="150"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
