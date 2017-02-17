<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCBoletimCompletoAluno.ascx.cs" Inherits="GestaoEscolar.WebControls.BoletimCompletoAluno.UCBoletimCompletoAluno" %>

<%@ Register Src="UCDadosBoletim.ascx" TagName="UCDadosBoletim" TagPrefix="uc1" %>

<asp:Label ID="lblMensagem" runat="server" EnableViewState="False"></asp:Label>
<div class="right" runat="server" id="ImprimirDiv" visible='<%#ImprimirVisible %>'>
    <input id="btnImprimir" class="btnImprimir" type="button" value="Imprimir" />
</div>

<div id="divBoletim">
    <fieldset id="Fieldset1" runat="server" class="fieldsetBoletim">

        <div class="imgLogo">
            <asp:Image ID="logoInstituicao" runat="server" />
        </div>

        <asp:Label ID="lblLegend" runat="server" Text="" CssClass="nomeBoletim"></asp:Label>
        <%--<div>--%>
        <div class="divBoletinsAnteriores" id="DivBoletinsAnteriores" runat="server">
            <asp:Label ID="lblBoletinsAnteriores" runat="server" Text="Boletins anteriores"></asp:Label>

            <asp:Repeater ID="rptPeriodo" runat="server">
                <ItemTemplate>
                    <asp:Button ID="btnPeriodo" runat="server" Text='<%# Eval("RowAvaliacao") %>'
                        OnClick="btnPeriodo_Click" />
                    <asp:HiddenField ID="hdnPeriodo" runat="server" Value='<%# Eval("tpc_id") %>' />
                </ItemTemplate>
            </asp:Repeater>

        </div>

        <div class="acertoPrint">
            <div class="divDados">
                <div>
                    <asp:Label ID="lblNome" runat="server" Text="Nome do aluno:" CssClass="txtnegrito"></asp:Label>
                    <asp:Label ID="lblNomeAluno" runat="server" Text=""></asp:Label>
                    <br />

                    <asp:Label ID="lblCodigo" runat="server" Text="<%$ Resources:UserControl, BoletimCompletoAluno.UCBoletimCompletoAluno.lblCodigo.Text %>" CssClass="txtnegrito"></asp:Label>
                    <asp:Label ID="lblCodigoEOL" runat="server" Text=""></asp:Label>

                    <asp:Label ID="lblCicloAluno" runat="server" Text="" CssClass="txtnegrito"></asp:Label>

                    <asp:Label ID="lblTurma" runat="server" Text="Ano/Turma:" CssClass="txtnegrito"></asp:Label>
                    <asp:Label ID="Label5" runat="server" Text=""></asp:Label>
                </div>
            </div>
            <asp:Image runat="server" ID="imgFotoAluno" SkinID="imgFoto" />
        </div>

        <%--<h1 class="tituloBoletim"> 
            <asp:Label ID="lblAvaliacao" runat="server" Text="AVALIAÇÃO"></asp:Label>
        </h1>--%>

        <div id="divCiclo" runat="server" class="cicloTres">
            <div class="acertoPrint">
                <div id="divPerfil" runat="server" class="divPerfil">
                    <div>
                        <div>
                            <span class="itemBoletim">Perfil do aluno <span>- Dados do Conselho de Classe</span></span>
                        </div>
                        <div>
                            <%--QUALIDADES DO ALUNO--%>
                            <!--   <div class="duasDivisoes boxAvaliacao semBorda">
                            <asp:Repeater ID="rptQualidades" runat="server">
                                <HeaderTemplate>
                                    <table>
                                        <thead>
                                            <tr>
                                                        <th>Qualidades</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblQualidade" runat="server" Text='<%# Eval("qualidade").ToString() %>'></asp:Label>
                                            </td>
                                        </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                        </tbody>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>-->
                            <%--DESEMPENHO DO ALUNO--%>
                            <div class="boxAvaliacao semBorda">
                                <asp:Repeater ID="rptDesempenho" runat="server">
                                    <HeaderTemplate>
                                        <table>
                                            <thead>
                                                <tr>
                                                    <th>
                                                        <asp:Label ID="lblCabDesempenhoAprendizado" runat="server" Text="<%$ Resources:Mensagens, MSG_DESEMPENHOAPRENDIZADO %>"></asp:Label>
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblDesempenhoAprendizado" runat="server" Text='<%# Eval("desempenho").ToString() %>'></asp:Label>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>
                                    </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                            <%--RECOMENDAÇÕES AO ALUNO--%>
                            <div class="boxAvaliacao">
                                <asp:Repeater ID="rptRecomendacoes" runat="server">
                                    <HeaderTemplate>
                                        <table>
                                            <thead>
                                                <tr>
                                                    <th>Recomendações ao Aluno</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblRecomendacao" runat="server" Text='<%# Eval("recomendacao").ToString() %>'></asp:Label>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>
                                    </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="divProposta" id="DivProposta" runat="server">
                    <%--Aluno--%>
                    <div>
                        <div class="semDivisao">
                            <span class="itemBoletim">Aluno <span>- Compromisso de estudo</span></span>
                        </div>
                        <div class="boxAvaliacao semBorda">
                            <asp:Label ID="Label3" runat="server" Text="O que tenho feito?"></asp:Label>
                            <br />
                            <asp:Label ID="lblAtividadeFeita" runat="server"></asp:Label>
                        </div>
                        <div class="boxAvaliacao">
                            <asp:Label ID="Label6" runat="server" Text="O que pretendo fazer?"></asp:Label>
                            <br />
                            <asp:Label ID="lblAtividadePretendeFazer" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>

            <div class="acertoPrint">
                <div id="divRecomendacoes" runat="server" class="divRecomendacoes">
                    <%-- Recomendações às Mães, Pais e Responsáveis --%>
                    <div class="semDivisao">
                        <span class="itemBoletim">Recomendações aos Pais/Responsáveis</span>
                    </div>
                    <div class="boxAvaliacao">
                        <asp:Repeater ID="rptRecomendacoesAosResp" runat="server">
                            <HeaderTemplate>
                                <table>
                                    <thead>
                                        <tr>
                                            <th>Recomendações aos Pais/Responsáveis</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblRecomendacaoPais" runat="server" Text='<%# Eval("recomendacao").ToString() %>'></asp:Label>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </tbody>
                            </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>

            <uc1:UCDadosBoletim ID="UCDadosBoletim1" runat="server" />
        </div>

        <div class="rodapeBoletim" id="rodapeBoletim" runat="server">
            <span>
                <asp:Label runat="server" ID="lblRodapeBoletimCompleto" Text="<%$ Resources:Mensagens, MSG_RODAPEBOLETIMCOMPLETO %>"></asp:Label>
            </span>
        </div>
        <%--</div>--%>
    </fieldset>
</div>
