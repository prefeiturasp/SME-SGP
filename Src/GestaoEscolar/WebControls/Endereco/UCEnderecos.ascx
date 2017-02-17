<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebControls_Endereco_UCEnderecos"
    CodeBehind="UCEnderecos.ascx.cs" %>
<%@ Register Src="../Combos/UCComboZona.ascx" TagName="UCComboZona" TagPrefix="uc2" %>
<%@ Register Src="../Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc1" %>
<style type="text/css">
    .someinput::-ms-clear {
        width: 0;
        height: 0;
    }
</style>
<asp:UpdatePanel ID="updCadastroEndereco" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
        <div runat="server" id="divAdaptado" visible="false" style="float:left;">
            <asp:CheckBox ID="_chkAdaptado" runat="server" Text="<%$ Resources:Academico, Escola.Cadastro._chkAdaptado.Text %>" />
        </div>
        <asp:Repeater ID="rptEndereco" runat="server" OnItemDataBound="rptEndereco_ItemDataBound">
            <HeaderTemplate>
                <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" Visible='<%#_VS_Obrigatorio && VS_MostraMensagem %>' />
                <asp:ValidationSummary ID="valEndereco" runat="server" ValidationGroup='<%#_ValidationGroup%>' Visible='<%#_VisibleValSummary%>' />
                <asp:ValidationSummary ID="valLocalizacaoGeografica" runat="server" ValidationGroup="LocalizacaoGeografica" />
                <br />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Panel ID="pnlEndereco" runat="server" CssClass="tbEndereco">
                    <asp:CheckBox ID="chkPrincipal" runat="server" Text="Principal" Visible="false" SkinID="chkPrincipal"/>
                    <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
                    <asp:Label ID="LabelCEP" runat="server" Text='<%#_VS_Obrigatorio ? "CEP (somente números) *" : "CEP (somente números)" %>'
                        AssociatedControlID="txtCEP"> </asp:Label>
                    <asp:TextBox ID="txtCEP" runat="server" MaxLength="8" Width="160" SkinID="CepIncremental"
                        AutoPostBack="True" Text='<%#Bind("end_cep")%>' OnTextChanged="txtCEP_TextChanged"> </asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvCEP" runat="server" ValidationGroup='<%#_ValidationGroup %>'
                        ControlToValidate="txtCEP" Display="Dynamic" Enabled='<%#_VS_Obrigatorio %>'
                        ErrorMessage="CEP é obrigatório.">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revCEP" runat="server" ValidationGroup='<%#_ValidationGroup%>'
                        ControlToValidate="txtCEP" Display="Dynamic" ErrorMessage="CEP inválido." ValidationExpression="^([0-9]){8}$">*</asp:RegularExpressionValidator>
                    <asp:ImageButton ID="btnLimparEndereco" runat="server" Visible="False" SkinID="btLimpar"
                        ToolTip="Limpar campos do endereço" CssClass="tbNovoEndereco_incremental" OnClick="btnLimparEndereco_Click"
                        CausesValidation="false" TabIndex="10" />
                    <asp:Label ID="LabelLogradouro" runat="server" Text='<%#_VS_Obrigatorio ? "Endereço *" : "Endereço" %>'
                        AssociatedControlID="txtLogradouro"></asp:Label>
                    <asp:TextBox ID="txtLogradouro" runat="server" MaxLength="200" ToolTip="Digite para buscar o endereço"
                        Text='<%#Bind("end_logradouro") %>' Width="510" CssClass="tbLogradouro_incremental">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvLogradouro" runat="server" ValidationGroup='<%#_ValidationGroup%>'
                        Enabled='<%#_VS_Obrigatorio %>' ControlToValidate="txtLogradouro" ErrorMessage="Endereço é obrigatório."
                        Display="Dynamic">*</asp:RequiredFieldValidator>
                    <table>
                        <tr id="trNumeroCompl" runat="server">
                            <td>
                                <asp:Label ID="LabelNumero" runat="server" Text='<%#_VS_Obrigatorio? "Número *" : "Número" %>'
                                    AssociatedControlID="txtNumero"> </asp:Label>
                                <asp:TextBox ID="txtNumero" runat="server" Text='<%#Bind("numero") %>' MaxLength="20"> </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvNumero" runat="server" ValidationGroup='<%#_ValidationGroup %>'
                                    Enabled='<%#_VS_Obrigatorio %>' ControlToValidate="txtNumero" ErrorMessage="Número do endereço é obrigatório."
                                    Display="Dynamic">*</asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:Label ID="LabelComplemento" runat="server" Text="Complemento" AssociatedControlID="txtComplemento">
                                </asp:Label>
                                <asp:TextBox ID="txtComplemento" runat="server" MaxLength="100" Text='<%#Bind("complemento") %>'
                                    Width="240"> </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td id="tdDistrito" runat="server" visible="<%# VS_MostraDistrito %>">
                                <asp:Label ID="LabelDistrito" runat="server" Text="Distrito" AssociatedControlID="txtDistrito"></asp:Label>
                                <asp:TextBox ID="txtDistrito" runat="server" MaxLength="100" Text='<%#Bind("end_distrito")%>'
                                    CssClass="text30C tbDistrito_incremental"> </asp:TextBox>
                            </td>
                            <td>
                                <uc2:UCComboZona ID="UCComboZona1" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LabelBairro" runat="server" Text='<%#_VS_Obrigatorio ? "Bairro *" : "Bairro" %>'
                                    AssociatedControlID="txtBairro"></asp:Label>
                                <asp:TextBox ID="txtBairro" runat="server" MaxLength="100" Text='<%#Bind("end_bairro")%>'
                                    CssClass="text30C tbBairro_incremental"> </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvBairro" runat="server" ControlToValidate="txtBairro"
                                    ValidationGroup='<%#_ValidationGroup%>' Enabled='<%#_VS_Obrigatorio %>' Display="Dynamic"
                                    ErrorMessage="Bairro é obrigatório.">*</asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <input id="txtCid_id" runat="server" type="hidden" class="tbCid_id_incremental" value='<%#Bind("cid_id") %>' />
                                <asp:Label ID="LabelCidade" runat="server" ValidationGroup='<%#_ValidationGroup%>'
                                    Text='<%#_VS_Obrigatorio ? "Cidade *" : "Cidade" %>' AssociatedControlID="txtCidade">
                                </asp:Label>
                                <asp:TextBox ID="txtCidade" runat="server" MaxLength="200" Text='<%#Bind("cid_nome")%>'
                                    CssClass="text30C tbCidade_incremental"> </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvCidade" runat="server" ControlToValidate="txtCidade"
                                    ValidationGroup='<%#_ValidationGroup%>' Enabled='<%#_VS_Obrigatorio %>' Display="Dynamic"
                                    ErrorMessage="Cidade é obrigatório.">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <div runat="server" id="divLatLongitude" visible="false">
                            <tr>
                                <td>
                                    <asp:Label ID="lblLatitude" runat="server" Text="Latitude" EnableViewState="false"
                                        AssociatedControlID="txtLatitude"></asp:Label>
                                    <asp:TextBox ID="txtLatitude" runat="server" CssClass="maskCoordenadas"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="rfvLatitude" runat="server" Display="Dynamic" ControlToValidate="txtLatitude"
                                        ErrorMessage="Latitude é obrigatório." ValidationGroup="LocalizacaoGeografica">*</asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revLatitude" runat="server" Display="Dynamic" Enabled="false"
                                        ControlToValidate="txtLatitude" ErrorMessage="Latitude deve ser um número com no máximo dois caracteres para a parte inteira e pelo menos seis caracteres decimais."
                                        ValidationExpression="^[-+]?[0-9][0-9]?\.[0-9]{6}[0-9]*$" ValidationGroup="LocalizacaoGeografica">*</asp:RegularExpressionValidator>
                                    <asp:CustomValidator ID="cvLatitude" runat="server" Display="Dynamic" ErrorMessage="Latitude deve ser menor que 5.272222 (graus) e maior que -33.750833 (graus)."
                                        OnServerValidate="ValidarLatitude_ServerValidate" ValidationGroup="LocalizacaoGeografica">*</asp:CustomValidator>--%>
                                </td><td>
                                    <asp:Label ID="lblLongitude" runat="server" Text="Longitude" EnableViewState="false"
                                        AssociatedControlID="txtLongitude"></asp:Label>
                                    <asp:TextBox ID="txtLongitude" runat="server" CssClass="maskCoordenadas"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="rfvLongitude" runat="server" Display="Dynamic" ControlToValidate="txtLongitude"
                                        ErrorMessage="Longitude é obrigatório." ValidationGroup="LocalizacaoGeografica">*</asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revLongitude" runat="server" Display="Dynamic" Enabled="false"
                                        ControlToValidate="txtLongitude" ErrorMessage="Longitude deve ser um número com no máximo dois caracteres para a parte inteira e pelo menos seis caracteres decimais."
                                        ValidationExpression="^[-+]?[0-9][0-9]?\.[0-9]{6}[0-9]*$" ValidationGroup="LocalizacaoGeografica">*</asp:RegularExpressionValidator>
                                    <asp:CustomValidator ID="cvLongitude" runat="server" Display="Dynamic" ErrorMessage="Longitude deve ser menor que -28.85 (graus) e maior que -73.992222 (graus)."
                                        OnServerValidate="ValidarLongitude_ServerValidate" ValidationGroup="LocalizacaoGeografica">*</asp:CustomValidator>--%>
                                </td>
                            </tr>
                        </div>
                    </table>
                    <div class="right">
                        <asp:Button ID="btnLocalizarPorEndereco" runat="server" Text="Localizar por endereço"
                            CausesValidation="false" />
                        <asp:Button ID="btnLocalizarPorCoordenadas" runat="server" Text="Localizar por coordenadas"
                            CausesValidation="false" />
                        <asp:Button ID="btnExcluir" runat="server" Text="Excluir" CausesValidation="false"
                            Visible="false" OnClick="btnExcluir_Click" />
                    </div>
                    <asp:Label ID="lblBanco" runat="server" Visible="false" Text='<%#Bind("banco") %>'></asp:Label>
                    <asp:Label ID="lblID" runat="server" Visible="false" Text='<%#Bind("id") %>'></asp:Label>
                    <asp:Label ID="lblPrdID" runat="server" Visible="false" Text='<%#Bind("prd_id") %>'></asp:Label>
                    <asp:Label ID="lblPedID" runat="server" Visible="false" Text='<%#Bind("ped_id") %>'></asp:Label>
                    <asp:Label ID="lblUepID" runat="server" Visible="false" Text='<%#Bind("uep_id") %>'></asp:Label>
                    <input id="txtEnd_id" runat="server" type="hidden" value='<%#Bind("end_id") %>' class="tbEnd_id_incremental" />
                </asp:Panel>
            </ItemTemplate>
        </asp:Repeater>
        <div class="botoes">
            <asp:Button ID="btnNovo" runat="server" Text="Adicionar endereço" OnClick="btnNovo_Click"
                CausesValidation="false" Visible="false" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<div id="divLocalizacaoGeografica" title="Localização geográfica" class="hide">
    <div runat="server" id="map_canvas" style="float: left; width: 600px; height: 400px;"></div>
</div>
