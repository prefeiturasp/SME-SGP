<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="WebControls_PessoaDocumento_UCGridPessoaDocumento" CodeBehind="UCGridDocumento.ascx.cs" %>
<%@ Register Src="../Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc1" %>
<asp:UpdatePanel ID="updGridDocumentos" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <fieldset>
            <legend>Cadastro de documentos</legend>
            <div>
                <asp:Label ID="_lblMessage" runat="server"></asp:Label>
            </div>
            <asp:Repeater ID="rptDocumento" runat="server" OnItemDataBound="rptDocumento_ItemDataBound">
                <HeaderTemplate>
                    <div class="tipo-documento">
                </HeaderTemplate>
                <ItemTemplate>
                    <div class="linha-documento">
                        <div id="tipoDocumento" runat="server" class="div-input-inline">
                            <asp:Label ID="lblTipoDoc" runat="server" Text="Tipo de documento" AssociatedControlID="ddlTipoDoc"></asp:Label>
                            <asp:DropDownList ID="ddlTipoDoc" runat="server" AppendDataBoundItems="True" DataSourceID="odsTipoDocumentacao"
                                DataTextField="tdo_nome" DataValueField="tdo_id" SkinID="text20C" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlTipoDoc_SelectedIndexChanged">
                                <asp:ListItem Value="00000000-0000-0000-0000-000000000000">-- Selecione o tipo --</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div id="NumDoc" runat="server" class="div-input-inline">
                            <asp:Label ID="lblNumDoc" runat="server" Text="Número" AssociatedControlID="tbNumDoc"></asp:Label>
                            <asp:TextBox ID="tbNumDoc" runat="server" SkinID="text10C" MaxLength="50" Text='<%# Bind("numero") %>' OnTextChanged="tbNumDoc_TextChanged" AutoPostBack="true"></asp:TextBox>
                        </div>

                        <div id="Categoria" runat="server" class="div-input-inline">
                            <asp:Label ID="lblCategoria" runat="server" Text="Categoria" AssociatedControlID="tbCategoria"></asp:Label>
                            <asp:TextBox ID="tbCategoria" runat="server" SkinID="text10C" MaxLength="1000" Text='<%# Bind("categoria") %>'></asp:TextBox>
                        </div>

                        <div id="Classificacao" runat="server" class="div-input-inline">
                            <asp:Label ID="lblClassificacao" runat="server" Text="Classificação" AssociatedControlID="tbClassificacao"></asp:Label>
                            <asp:TextBox ID="tbClassificacao" runat="server" SkinID="text10C" MaxLength="1000" Text='<%# Bind("classificacao") %>'></asp:TextBox>
                        </div>

                        <div id="Csm" runat="server" class="div-input-inline">
                            <asp:Label ID="lblCsm" runat="server" Text="CSM" AssociatedControlID="tbCsm"></asp:Label>
                            <asp:TextBox ID="tbCsm" runat="server" SkinID="text10C" MaxLength="1000" Text='<%# Bind("csm") %>'></asp:TextBox>
                        </div>

                        <div id="DtEmissao" runat="server" class="div-input-inline">
                            <asp:Label ID="lblDtEmissao" runat="server" Text="Data de emissão" AssociatedControlID="tbDtEmissao"></asp:Label>
                            <asp:TextBox ID="tbDtEmissao" runat="server" Width="115" SkinID="Data" Text='<%# Bind("dataemissao") %>'></asp:TextBox>
                        </div>

                        <div id="DataEntrada" runat="server" class="div-input-inline">
                            <asp:Label ID="lblDataEntrada" runat="server" Text="Data de entrada" AssociatedControlID="tbDataEntrada"></asp:Label>
                            <asp:TextBox ID="tbDataEntrada" runat="server" Width="115" SkinID="Data" MaxLength="1000" Text='<%# Bind("dataEntrada") %>'></asp:TextBox>
                        </div>

                        <div id="DataExpedicao" runat="server" class="div-input-inline">
                            <asp:Label ID="lblDataExpedicao" runat="server" Text="Data de expedição" AssociatedControlID="tbDataExpedicao"></asp:Label>
                            <asp:TextBox ID="tbDataExpedicao" runat="server" Width="115" SkinID="Data" Text='<%# Bind("dataexpedicao") %>'></asp:TextBox>
                        </div>

                        <div id="DataValidade" runat="server" class="div-input-inline">
                            <asp:Label ID="lblDataValidade" runat="server" Text="Data de validade" AssociatedControlID="tbDataValidade"></asp:Label>
                            <asp:TextBox ID="tbDataValidade" runat="server" Width="115" SkinID="Data" MaxLength="1000" Text='<%# Bind("dataValidade") %>'></asp:TextBox>
                        </div>

                        <div id="OrgEmissor" runat="server" class="div-input-inline">
                            <asp:Label ID="lblOrgEmissor" runat="server" Text="Orgão emissor" AssociatedControlID="tbOrgEmissor"></asp:Label>
                            <asp:TextBox ID="tbOrgEmissor" runat="server" SkinID="text10C" MaxLength="200" Text='<%# Bind("orgaoemissao") %>'></asp:TextBox>
                        </div>

                        <div id="InfCompl" runat="server" class="div-input-inline">
                            <asp:Label ID="lblInfCompl" runat="server" Text="Info. complementares" AssociatedControlID="tbInfCompl"></asp:Label>
                            <asp:TextBox ID="tbInfCompl" runat="server" SkinID="text10C" Width="200" MaxLength="1000" Text='<%# Bind("info") %>'></asp:TextBox>
                        </div>

                        <div id="Pais" runat="server" class="div-input-inline">
                            <asp:Label ID="lbllPais" runat="server" Text="País" AssociatedControlID="ddlPais"></asp:Label>
                            <asp:DropDownList ID="ddlPais" runat="server" AppendDataBoundItems="True" DataSourceID="odsPais"
                                DataTextField="pai_nome" DataValueField="pai_id" SkinID="text20C" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlPais_SelectedIndexChanged">
                                <asp:ListItem Value="">-- Selecione um País --</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="lblPai_idOrigem" runat="server" Text='<%#Bind("pai_idOrigem") %>'
                                Visible="false"></asp:Label>
                            <asp:Label ID="lblPai_IdAntigo" runat="server" Text='<%#Bind("pai_idAntigo") %>'
                                Visible="false"></asp:Label>
                        </div>

                        <div id="UF" runat="server" class="div-input-inline">
                            <asp:Label ID="lbllUF" runat="server" Text="Unidade federativa" AssociatedControlID="ddlUF"></asp:Label>
                            <asp:DropDownList ID="ddlUF" runat="server" AppendDataBoundItems="True"
                                DataTextField="unf_nome" DataValueField="unf_id" SkinID="text20C" Enabled="false">
                                <asp:ListItem Value="">-- Selecione uma UF --</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="lblUnf_idEmissao" runat="server" Text='<%#Bind("unf_idEmissao") %>'
                                Visible="false"></asp:Label>
                            <asp:Label ID="lblUnf_idAntigo" runat="server" Text='<%#Bind("unf_idAntigo") %>'
                                Visible="false"></asp:Label>
                        </div>

                        <div id="RegiaoMilitar" runat="server" class="div-input-inline">
                            <asp:Label ID="lblRegiaoMilitar" runat="server" Text="Região militar" AssociatedControlID="tbRegiaoMilitar"></asp:Label>
                            <asp:TextBox ID="tbRegiaoMilitar" runat="server" SkinID="text10C" MaxLength="1000" Text='<%# Bind("regiaoMilitar") %>'></asp:TextBox>
                        </div>

                        <div id="NumeroRA" runat="server" class="div-input-inline">
                            <asp:Label ID="lblNumeroRA" runat="server" Text="Registro de alistamento" AssociatedControlID="tbNumeroRA"></asp:Label>
                            <asp:TextBox ID="tbNumeroRA" runat="server" SkinID="text10C" Width="200" MaxLength="1000" Text='<%# Bind("numeroRA") %>'></asp:TextBox>
                        </div>

                        <div id="Secao" runat="server" class="div-input-inline">
                            <asp:Label ID="lblSecao" runat="server" Text="Seção" AssociatedControlID="tbSecao"></asp:Label>
                            <asp:TextBox ID="tbSecao" runat="server" SkinID="text10C" MaxLength="1000" Text='<%# Bind("secao") %>'></asp:TextBox>
                        </div>

                        <div id="Serie" runat="server" class="div-input-inline">
                            <asp:Label ID="lblSerie" runat="server" Text="Série" AssociatedControlID="tbSerie"></asp:Label>
                            <asp:TextBox ID="tbSerie" runat="server" SkinID="text10C" MaxLength="1000" Text='<%# Bind("serie") %>'></asp:TextBox>
                        </div>

                        <div id="TipoGuarda" runat="server" class="div-input-inline">
                            <asp:Label ID="lblTipoGuarda" runat="server" Text="Tipo de guarda" AssociatedControlID="tbSerie"></asp:Label>
                            <asp:TextBox ID="tbTipoGuarda" runat="server" SkinID="text10C" Width="200" MaxLength="1000" Text='<%# Bind("tipoGuarda") %>'></asp:TextBox>
                        </div>

                        <div id="Via" runat="server" class="div-input-inline">
                            <asp:Label ID="lblVia" runat="server" Text="Via" AssociatedControlID="tbVia"></asp:Label>
                            <asp:TextBox ID="tbVia" runat="server" SkinID="text10C" MaxLength="1000" Text='<%# Bind("via") %>'></asp:TextBox>
                        </div>

                        <div id="Zona" runat="server" class="div-input-inline">
                            <asp:Label ID="lblZona" runat="server" Text="Zona" AssociatedControlID="tbZona"></asp:Label>
                            <asp:TextBox ID="tbZona" runat="server" SkinID="text10C" MaxLength="1000" Text='<%# Bind("zona") %>'></asp:TextBox>
                        </div>

                        <div id="LimparDocumento" runat="server" class="div-input-inline">
                            <asp:ImageButton ID="btnLimparDocumento" runat="server" SkinID="btLimpar" ToolTip="Limpar documento"
                                CssClass="tbNovoEndereco_incremental" OnClick="btnLimparDocumento_Click" CausesValidation="false"
                                TabIndex="10" />
                            <headerstyle cssclass="center" />
                            <itemstyle horizontalalign="Center" />
                            <headerstyle cssclass="center" />
                            <itemstyle horizontalalign="Center" />
                        </div>

                        <div id="tdo_id" runat="server" class="div-input-inline">
                            <asp:Label ID="lbltdo_id" runat="server" Visible="false" Text='<%#Bind("tdo_id") %>'></asp:Label>
                        </div>

                        <div id="Banco" runat="server" class="div-input-inline">
                            <asp:Label ID="lblBanco" runat="server" Visible="false" Text='<%#Bind("banco") %>'></asp:Label>
                        </div>
                    </div>
                </ItemTemplate>
                <FooterTemplate>
                    </div>
                </FooterTemplate>
            </asp:Repeater>
            <div class="right">
                <asp:Button ID="btAdicionar" runat="server" Text="Adicionar documento" CausesValidation="false" CommandArgument="Add" OnClick="btAdicionar_Click" />
            </div>

            <asp:ObjectDataSource ID="odsTipoDocumentacao" runat="server" DataObjectTypeName="MSTech.CoreSSO.Entities.SYS_TipoDocumentacao"
                DeleteMethod="Delete" OldValuesParameterFormatString="original_{0}" SelectMethod="GetSelect"
                TypeName="MSTech.CoreSSO.BLL.SYS_TipoDocumentacaoBO" UpdateMethod="Save">
                <SelectParameters>
                    <asp:Parameter DbType="Guid" DefaultValue="00000000-0000-0000-0000-000000000000"
                        Name="tdo_id" />
                    <asp:Parameter DefaultValue="" Name="tdo_nome" Type="String" />
                    <asp:Parameter DefaultValue="" Name="tdo_sigla" Type="String" />
                    <asp:Parameter DefaultValue="0" Name="tdo_situacao" Type="Byte" />
                    <asp:Parameter DefaultValue="false" Name="paginado" Type="Boolean" />
                    <asp:Parameter DefaultValue="1" Name="currentPage" Type="Int32" />
                    <asp:Parameter DefaultValue="1" Name="pageSize" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:ObjectDataSource ID="odsPais" runat="server" DataObjectTypeName="MSTech.CoreSSO.Entities.END_Pais"
                DeleteMethod="Delete" OldValuesParameterFormatString="original_{0}" SelectMethod="GetSelect"
                TypeName="MSTech.CoreSSO.BLL.END_PaisBO" UpdateMethod="Save">
                <SelectParameters>
                    <asp:Parameter DbType="Guid" DefaultValue="00000000-0000-0000-0000-000000000000"
                        Name="pai_id" />
                    <asp:Parameter DefaultValue="" Name="pai_nome" Type="String" />
                    <asp:Parameter DefaultValue="" Name="pai_sigla" Type="String" />
                    <asp:Parameter DefaultValue="0" Name="pai_situacao" Type="Byte" />
                    <asp:Parameter DefaultValue="false" Name="paginado" Type="Boolean" />
                    <asp:Parameter DefaultValue="1" Name="currentPage" Type="Int32" />
                    <asp:Parameter DefaultValue="1" Name="pageSize" Type="Int32" />
                </SelectParameters>
                <DeleteParameters>
                    <asp:Parameter Name="entity" Type="Object" />
                    <asp:Parameter Name="banco" Type="Object" />
                </DeleteParameters>
            </asp:ObjectDataSource>
        </fieldset>
    </ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript">
    //ADICIONADO POR CONTA DO UPDATEPANEL. PRECISA SER ADICIONADO NA PÁGINA POR CONTA DE POSTBACK.
    Sys.Application.add_load(function () {

        //mascara de data 'dd/mm/yyyy'
        $(".mskData").setMask({ mask: '39/19/2999', selectCharsOnFocus: false, autoTab: false });
        $(".mskData").datepicker({ dateFormat: 'dd/mm/yy' });

        console.log("entrou", $(".mskData"));
    });
</script>
