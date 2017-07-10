<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCBoletimAngular.ascx.cs" Inherits="AreaAluno.WebControls.BoletimCompletoAluno.UCBoletimAngular" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <style type="text/css">
        @media print {
            .notprint {
                visibility: hidden;
            }
        }
    </style>
</head>
<body runat="server">
    <script src="../../includes/Angular/angular.js" type="text/javascript"></script>
    <script src="../../includes/Angular/module.js" type="text/javascript"></script>
    <script src="../../includes/Angular/Boletim.controller.js" type="text/javascript"></script>

    <script type="text/javascript">
       var params =
            {
                AluIds: '<%= alu_ids %>',
                MtuIds: '<%= mtu_ids %>',
                TpcId: <%= tpc_id %>
                };
				
        var site = '';
        var core = '<%= MSTech.GestaoEscolar.Web.WebProject.ApplicationWEB.UrlCoreSSO %>';
        var api = '<%= MSTech.GestaoEscolar.Web.WebProject.ApplicationWEB.UrlGestaoAcademicaWebApi %>';
        var msgDocenciaCompartilhada = '<%= GetGlobalResourceObject("WebControls", "BoletimCompletoAluno.UCDadosBoletimAluno.MensagemCompartilhada") %>';
        var msgAlunoSemDadosPeriodo = '<%= GetGlobalResourceObject("WebControls", "BoletimCompletoAluno.UCBoletimAngular.MensagemAlunoSemDadosPeriodo") %>';
        var msgSemEventoLiberado = '<%= infantil ? GetGlobalResourceObject("WebControls", "BoletimCompletoAluno.UCBoletimAngular.MensagemSemEventoLiberadoInfantil") :
                                        GetGlobalResourceObject("WebControls", "BoletimCompletoAluno.UCBoletimAngular.MensagemSemEventoLiberado") %>';

        var Usuario = '<%= Usuario %>';
        var Entidade = '<%= Entidade %>';
        var Grupo = '<%= Grupo %>';
        var Token = '<%= Token %>';
   
    </script>

    <form id="form1">
        <div id="divBoletinsAlunos">
            <div ng-app="app">
                <div ng-controller="BoletimController" ng-cloak>
                    <div class="loader" ng-if="!mensagemErro && !mensagemAlerta && listBoletins.length == 0">
                        <img class="imgLoader" src="../../App_Themes/IntranetSME/images/ajax-loader.gif" style="border-width: 0px;">
                    </div>                                       

                    <div class="divBoletinsAnteriores notprint" ng-if="listBoletins.length > 0 || mensagemAlerta">
                        <div class="divPeriodos">
                            <asp:Repeater ID="rptPeriodos" runat="server">
                                <HeaderTemplate>
                                    <span><%= MSTech.GestaoEscolar.BLL.GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id) %> </span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <input type="button" class="btn" value="<%# Container.ItemIndex + 1 %>" ng-disabled="(params.TpcId == <%# Eval("tpc_id") %>)" ng-click="trocarTpc(<%# Eval("tpc_id") %>)" />
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        <div ng-if="BoletimLiberado" class="divImprimir">
                            <input class="btnImprimir" type="button" value="Imprimir" ng-click="imprimir()" style="float: right;" />
                        </div>
                    </div>

                    <div ng-if="mensagemErro" class="summary" style="background: #fff url(/App_Themes/IntranetSME/images/error.png) no-repeat 45px 50%;">
                        {{mensagemErro}}
                    </div>

                    <div ng-if="mensagemAlerta" class="summary" style="background: #fff url(/App_Themes/IntranetSME/images/icoInformacoes.png) no-repeat 45px 50%; display: flex;">
                        {{mensagemAlerta}}
                    </div>

                    <div style="page-break-after: always; display:inline-block;" ng-repeat="boletim in listBoletins">
                        <div id="divBoletim" ng-show="listBoletins.length > 0 || mensagemErro.length > 0">
                            <fieldset class="fieldsetBoletim">

                                <div class="imgLogo">
                                    <img id="logoInstituicao" title="SME-SP" src="{{getLogoSystem()}}" alt="SME-SP" style="border-width: 0px;">
                                </div>

                                <span class="nomeBoletim">
                                    <h1><%= VS_nomeBoletim %> - {{boletim.ava_nome}} - {{boletim.cal_ano}}</h1>
                                    <h3><%= GetGlobalResourceObject("WebControls", "BoletimCompletoAluno.UCBoletimCompletoAluno.lblLegend.Titulo").ToString().ToUpper() %><br />
                                        {{boletim.uad_nome}}</h3>
                                    <h2>{{boletim.esc_nome}}</h2>
                                </span>

                                <div class="acertoPrint">
                                    <div class="divDados">
                                        <div>
                                            <span class="txtnegrito">Nome do aluno:</span>
                                            <span>{{boletim.pes_nome}}</span>
                                            <br/>
                                            <span class="txtnegrito">
                                                <asp:Literal runat="server" Text="<%$ Resources: UserControl, BoletimCompletoAluno.UCBoletimCompletoAluno.lblCodigo.Text %>" /></span>
                                            <span>{{boletim.alc_matricula}}</span>

                                            <span class="txtnegrito">{{boletim.tci_nome}}</span>

                                            <span class="txtnegrito">Ano/Turma:</span>
                                            <span>{{boletim.tur_codigo}}</span>
                                        </div>
                                    </div>
                                    <img alt="Foto do aluno" class="imgFoto" src="{{getPhotoStudent(boletim.arq_idFoto)}}" style="border-width: 0px;" />
                                </div>

                                <div ng-class="boletim.cicloClass">
                                    <div class="acertoPrint">
                                        <div class="divPerfil" ng-if="!boletim.fechamentoPorImportacao && !boletim.ensinoInfantil" style="display: {{boletim.displayPerfilAluno}}">
                                            <div>
                                                <div>
                                                    <span class="itemBoletim">Perfil do aluno <span>- Dados do Conselho de Classe</span></span>
                                                </div>
                                                <div>
                                                    <div class="boxAvaliacao semBorda">
                                                        <table>
                                                            <thead>
                                                                <tr>
                                                                    <th>
                                                                        <span>
                                                                            <asp:Literal runat="server" Text="<%$ Resources:Mensagens, MSG_DESEMPENHOAPRENDIZADO %>" /></span>
                                                                    </th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                <tr ng-repeat="desempenho in boletim.desempenho">
                                                                    <td>
                                                                        <p>{{desempenho}}</p>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                    <div class="boxAvaliacao">
                                                        <table>
                                                            <thead>
                                                                <tr>
                                                                    <th>Recomendações ao Aluno</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                <tr ng-repeat="recomendacaoAlu in boletim.recomendacaoAluno">
                                                                    <td>
                                                                        <p>{{recomendacaoAlu}}</p>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                    <!--boxAvaliacao-->
                                                </div>
                                            </div>
                                        </div>
                                        <!--divPerfil-->

                                        <div class="divProposta" ng-if="!boletim.fechamentoPorImportacao && boletim.tci_exibirBoletim && !boletim.ensinoInfantil" style="display: {{boletim.displayPerfilAluno}}">
                                            <div>
                                                <div class="semDivisao">
                                                    <span class="itemBoletim">Aluno <span>- Compromisso de estudo</span></span>
                                                </div>
                                                <div>
                                                    <div class="boxAvaliacao semBorda">
                                                        <span>O que tenho feito?</span>
                                                        <br>
                                                        <span>{{boletim.cpe_atividadeFeita}}</span>
                                                    </div>
                                                    <div class="boxAvaliacao semBorda">
                                                        <span>O que pretendo fazer?</span>
                                                        <br>
                                                        <span>{{boletim.cpe_atividadePretendeFazer}}</span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!--divPerfil-->

                                    </div>
                                    <!--acertoPrint-->
                                </div>
                                <!--cicloUm-->

                                <div ng-class="boletim.cicloClass">
                                    <div class="acertoPrint">
                                        <div class="divRecomendacoes" ng-if="!boletim.fechamentoPorImportacao" style="display: {{boletim.displayRecomendacoes}}">
                                            <div class="semDivisao">
                                                <span class="itemBoletim">Recomendações aos Pais/Responsáveis</span>
                                            </div>
                                            <div class="boxAvaliacao">

                                                <table>
                                                    <thead>
                                                        <tr>
                                                            <th>Recomendações aos Pais/Responsáveis</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <tr ng-repeat="recomendacaoResp in boletim.recomendacaoResponsavel">
                                                            <td>
                                                                <p>{{recomendacaoResp}}</p>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>

                                            </div>
                                        </div>
                                    </div>
                                    <!--acertoPrint-->
                                </div>
                                <!--cicloUm-->

                                <div>
                                    <span></span>
                                    <div>
                                        <span></span>
                                        <div style="overflow-x: auto;">
                                            <table rules="none" style="width: 100%; display: {{boletim.displayResultados}}" class="boletimDefault">
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <div class="divDados">
                                                                <table class="tblBoletim tblBoletimDetalhes" width="100%" rules="none">
                                                                    <tbody>
                                                                        <tr class="linhaImpar">
                                                                            <th class="linhaConceitoGlobal" colspan="3"><span>Boletim Escolar</span>
                                                                            </th>
                                                                        </tr>
                                                                        <tr class="linhaPar">
                                                                            <td colspan="3">
                                                                                <strong>Escola: </strong>
                                                                                {{boletim.esc_nome}}
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="linhaImpar">
                                                                            <td colspan="2">
                                                                                <strong>Nome: </strong>
                                                                                {{boletim.pes_nome}}
                                                                            </td>
                                                                            <td>
                                                                                <strong>Número: </strong>
                                                                                {{boletim.mtu_numeroChamada}}
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="linhaPar">
                                                                            <td style="width: 70%">
                                                                                <strong>
                                                                                    <%=MSTech.GestaoEscolar.BLL.GestaoEscolarUtilBO.nomePadraoCurso(__SessionWEB.__UsuarioWEB.Usuario.ent_id)%>: </strong>
                                                                                {{boletim.cur_nome}}
                                                                            </td>
                                                                            <td style="width: 15%">
                                                                                <strong>Turma: </strong>
                                                                                {{boletim.tur_codigo}}
                                                                            </td>
                                                                            <td style="width: 15%">
                                                                                <strong>Ano: </strong>
                                                                                {{boletim.cal_ano}}
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <!---->

                                                    <tr>
                                                        <td>
                                                            <div ng-class="boletim.cicloClass">
                                                                <div class="divResultados">
                                                                    <div>
                                                                        <span class="itemBoletim"><span ng-if="!boletim.ensinoInfantil" class="span-uppercase">{{boletim.nomeNota}}s e </span> Faltas</span>
                                                                    </div>
                                                                    <table class="tblBoletim" rules="none" ng-if="!boletim.ensinoInfantil">
                                                                        <thead>
                                                                            <tr>
                                                                                <th rowspan="2" class="nomePeriodo colPrincipal">
                                                                                    <span>
                                                                                        <asp:Literal runat="server" Text="<%$ Resources:Mensagens, MSG_DISCIPLINA %>" /></span>
                                                                                </th>

                                                                                <th class="nomePeriodo colBimestre" colspan="2" ng-repeat="periodo in boletim.periodos" title="{{periodo.MatriculaPeriodo}}">
                                                                                    <span>{{periodo.tpc_nome}}</span>
                                                                                </th>

                                                                                <th rowspan="2" class="nomePeriodo">Síntese Final</th>
                                                                                <th rowspan="2" class="nomePeriodo">Total de Ausências</th>
                                                                                <th rowspan="2" class="nomePeriodo" ng-if="boletim.exibeCompensacaoAusencia">Total de Compensações</th>
                                                                                <th rowspan="2" class="nomePeriodo" ng-if="boletim.exibeCompensacaoAusencia">Frequência Final(%)</th>
                                                                            </tr>
                                                                            <tr>
                                                                                <th class="nomePeriodoColunas" ng-repeat-start="periodo in boletim.periodos">{{boletim.nomeNota}}</th>
                                                                                <th class="nomePeriodoColunas" ng-repeat-end>Faltas</th>
                                                                            </tr>
                                                                        </thead>
                                                                        <tbody>
                                                                            <tr class="trDisciplina linhaImpar" ng-repeat="(indexMat, materia) in boletim.matter" ng-class="checkParImpar(indexMat)" ng-if="!materia.enriquecimentoCurricular||!materia.recuperacao||!materia.aee">
                                                                                <td class="nomeDisciplina">{{materia.Disciplina}}</td>

                                                                                <td class="nota" ng-repeat="(indexAval, avaliacao) in materia.avaliacao"
                                                                                    ng-if="indexAval%2 == 0 || indexMat == 0 || !materia.tipoComponenteRegencia"
                                                                                    rowspan="{{ indexAval%2 == 0 || !materia.tipoComponenteRegencia ? 1 : boletim.QtComponenteRegencia }}">{{ indexAval%2 == 0 ? avaliacao.conceito : avaliacao.faltas}}
                                                                                </td>

                                                                                <td class="nota">{{materia.MediaFinal}}
                                                                                </td>

                                                                                <td class="nota" ng-if="indexMat == 0 || !materia.tipoComponenteRegencia"
                                                                                    rowspan="{{ materia.tipoComponenteRegencia ? boletim.QtComponenteRegencia : 1 }}">{{materia.totalFaltas}}
                                                                                </td>
                                                                                <td class="nota" ng-if="boletim.exibeCompensacaoAusencia && (indexMat == 0 || !materia.tipoComponenteRegencia)"
                                                                                    rowspan="{{ materia.tipoComponenteRegencia ? boletim.QtComponenteRegencia : 1 }}">{{materia.ausenciasCompensadas}}
                                                                                </td>
                                                                                <td class="nota" ng-if="boletim.exibeCompensacaoAusencia && (indexMat == 0 || boletim.QtComponenteRegencia == 0)"
                                                                                    rowspan="{{ boletim.QtComponenteRegencia > 0 ? boletim.QtComponentes : 1 }}">{{materia.FrequenciaFinalAjustada}}
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="trDisciplina" ng-if="(!boletim.parecerConclusivo || boletim.parecerConclusivo.length > 0)"
                                                                                 style="display: {{boletim.displayParecerConclusivo;}}">
                                                                                <td class="nota tdParecerConclusivo" colspan="20">Parecer conclusivo: {{ boletim.parecerConclusivo }}
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>

                                                                    <div>
                                                                        <table class="tblBoletim" rules="none" ng-if="boletim.showCurricularEnrichment && !boletim.ensinoInfantil">
                                                                            <thead>
                                                                                <tr>
                                                                                    <th rowspan="2" class="nomePeriodo colPrincipal">
                                                                                        <span>
                                                                                            <asp:Literal runat="server" Text="<%$ Resources:UserControl, UCDadosBoletim.lblEnriquecimento.Text %>" /></span>
                                                                                    </th>

                                                                                    <th class="nomePeriodo colBimestre" ng-repeat="periodo in boletim.periodos" title="{{periodo.MatriculaPeriodo}}">
                                                                                        <span>{{periodo.tpc_nome}}</span>
                                                                                    </th>

                                                                                    <th rowspan="2" class="nomePeriodo">Total de Ausências</th>
                                                                                    <th rowspan="2" class="nomePeriodo" ng-if="boletim.exibeCompensacaoAusencia">Total de Compensações</th>
                                                                                    <th rowspan="2" class="nomePeriodo" ng-if="boletim.exibeCompensacaoAusencia">Parecer Final</th>

                                                                                </tr>
                                                                                <tr>
                                                                                    <th class="nomePeriodoColunas" ng-repeat="periodo in boletim.periodos">Faltas</th>
                                                                                </tr>
                                                                            </thead>
                                                                            <tbody>

                                                                                <tr class="trDisciplina" ng-repeat="(indexMat, materia) in boletim.enrichment" ng-class="checkParImpar(indexMat)" ng-if="materia.enriquecimentoCurricular||materia.recuperacao||materia.aee">
                                                                                    <td class="nomeDisciplina">{{materia.Disciplina}}</td>
                                                                                    <td class="nota" ng-repeat="(indexAval, avaliacao) in materia.notas">{{avaliacao.nota.numeroFaltas != null ? avaliacao.nota.numeroFaltas : "-"}}</td>
                                                                                    <td class="nota">{{materia.totalFaltas}}</td>
                                                                                    <td class="nota" ng-if="boletim.exibeCompensacaoAusencia">{{materia.ausenciasCompensadas}}</td>
                                                                                    <td class="nota">{{materia.parecerFinal}}</td>
                                                                                </tr>

                                                                            </tbody>
                                                                            <!--tbody-->
                                                                        </table>
                                                                        <!--table-->
                                                                    </div>

                                                                    <div>
                                                                        <table class="tblBoletim" rules="none" ng-if="boletim.ensinoInfantil">
                                                                            <thead>
                                                                                <tr>
                                                                                    <th rowspan="2" class="nomePeriodo colPrincipal">
                                                                                    </th>

                                                                                    <th class="nomePeriodo colBimestre" ng-repeat="periodo in boletim.periodos" title="{{periodo.MatriculaPeriodo}}">
                                                                                        <span>{{periodo.tpc_nome}}</span>
                                                                                    </th>

                                                                                    <th rowspan="2" class="nomePeriodo">Total de Aulas</th>
                                                                                    <th rowspan="2" class="nomePeriodo">Total de Ausências</th>
                                                                                    <th rowspan="2" class="nomePeriodo">Frequência Final (%)</th>

                                                                                </tr>
                                                                                <tr>
                                                                                    <th class="nomePeriodoColunas" ng-repeat="periodo in boletim.periodos">Faltas</th>
                                                                                </tr>
                                                                            </thead>
                                                                            <tbody>

                                                                                <tr class="trDisciplina" ng-repeat="(indexMat, materia) in boletim.matter" ng-class="checkParImpar(indexMat)" >
                                                                                    <td class="nomeDisciplina">{{boletim.tur_codigo}}</td>
                                                                                    <td class="nota" ng-repeat="(indexAval, avaliacao) in materia.notas">{{avaliacao.nota.numeroFaltas != null ? avaliacao.nota.numeroFaltas : "-"}}</td>
                                                                                    <td class="nota">{{materia.totalAulas}}</td>
                                                                                    <td class="nota">{{materia.totalFaltas}}</td>
                                                                                    <td class="nota">{{materia.FrequenciaFinalAjustada}}</td>
                                                                                </tr>

                                                                            </tbody>
                                                                            <!--tbody-->
                                                                        </table>
                                                                        <!--table-->
                                                                    </div>

                                                                    <div>
                                                                        <table class="tblBoletim" rules="none" ng-if="boletim.showRecuperacao">
                                                                            <thead>
                                                                                <tr>
                                                                                    <th rowspan="2" class="nomePeriodo colPrincipal">
                                                                                        <span>
                                                                                            <asp:Literal runat="server" Text="<%$ Resources:UserControl, UCDadosBoletim.lblRecuperacaoTitulo.Text %>" /></span>
                                                                                    </th>

                                                                                    <th class="nomePeriodo colBimestre" ng-repeat="periodo in boletim.periodos" title="{{periodo.MatriculaPeriodo}}">
                                                                                        <span>{{periodo.tpc_nome}}</span>
                                                                                    </th>

                                                                                    <th rowspan="2" class="nomePeriodo">Total de Ausências</th>
                                                                                    <th rowspan="2" class="nomePeriodo" ng-if="boletim.exibeCompensacaoAusencia">Parecer Final</th>

                                                                                </tr>
                                                                                <tr>
                                                                                    <th class="nomePeriodoColunas" ng-repeat="periodo in boletim.periodos">Faltas</th>
                                                                                </tr>
                                                                            </thead>
                                                                            <tbody>

                                                                                <tr class="trDisciplina" ng-repeat="(indexRec, materia) in boletim.recovery" ng-class="checkParImpar(indexRec)" ng-if="materia.enriquecimentoCurricular||materia.recuperacao||materia.aee">
                                                                                    <td class="nomeDisciplina">{{materia.Disciplina}}</td>
                                                                                    <td class="nota" ng-repeat="(indexAval, avaliacao) in materia.notas">{{avaliacao.nota.numeroFaltas != null ? avaliacao.nota.numeroFaltas : "-"}}</td>
                                                                                    <td class="nota">{{materia.totalFaltas}}</td>
                                                                                    <td class="nota" ng-if="boletim.exibeCompensacaoAusencia">{{materia.parecerFinal}}</td>
                                                                                </tr>

                                                                            </tbody>
                                                                            <!--tbody-->
                                                                        </table>
                                                                        <!--table-->
                                                                    </div>

                                                                    <div>
                                                                        <table class="tblBoletim" rules="none" ng-if="boletim.showAEE">
                                                                            <thead>
                                                                                <tr>
                                                                                    <th rowspan="2" class="nomePeriodo colPrincipal">
                                                                                        <span>
                                                                                            <asp:Literal runat="server" Text="<%$ Resources:UserControl, UCDadosBoletim.lblAEETitulo.Text %>" /></span>
                                                                                    </th>

                                                                                    <th class="nomePeriodo colBimestre" ng-repeat="periodo in boletim.periodos" title="{{periodo.MatriculaPeriodo}}">
                                                                                        <span>{{periodo.tpc_nome}}</span>
                                                                                    </th>

                                                                                    <th rowspan="2" class="nomePeriodo">Total de Ausências</th>
                                                                                    <th rowspan="2" class="nomePeriodo" ng-if="boletim.exibeCompensacaoAusencia">Parecer Final</th>

                                                                                </tr>
                                                                                <tr>
                                                                                    <th class="nomePeriodoColunas" ng-repeat="periodo in boletim.periodos">Faltas</th>
                                                                                </tr>
                                                                            </thead>
                                                                            <tbody>

                                                                                <tr class="trDisciplina" ng-repeat="(indexAee, materia) in boletim.aee" ng-class="checkParImpar(indexAee)" ng-if="materia.enriquecimentoCurricular||materia.recuperacao||materia.aee">
                                                                                    <td class="nomeDisciplina">{{materia.Disciplina}}</td>
                                                                                    <td class="nota" ng-repeat="(indexAval, avaliacao) in materia.notas">{{avaliacao.nota.numeroFaltas != null ? avaliacao.nota.numeroFaltas : "-"}}</td>
                                                                                    <td class="nota">{{materia.totalFaltas}}</td>
                                                                                    <td class="nota" ng-if="boletim.exibeCompensacaoAusencia">{{materia.parecerFinal}}</td>
                                                                                </tr>

                                                                            </tbody>
                                                                            <!--tbody-->
                                                                        </table>
                                                                        <!--table-->
                                                                    </div>
                                                                </div>
                                                                <!--divResultados-->
                                                                <div ng-if="boletim.docenciaCompartilhada != null && boletim.docenciaCompartilhada.length > 0">
                                                                    <div style="display: block" ng-repeat="msg in boletim.docenciaCompartilhada">{{msg}}</div>
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <!---->

                                                    <%--</tbody>--%>
                                                    <!--tbody-->
                                            </table>
                                            <!--table-->
                                        </div>
                                        <!--overflow-x:auto;-->
                                    </div>
                                </div>
                                <div ng-if="boletim.justificativaAbonoFalta.length > 0" class="rodapeBoletimJustificativaAbono">
                                    <span>
                                        {{boletim.justificativaAbonoFalta}}
                                    </span>
                                </div>
                                <div ng-if="!boletim.ensinoInfantil" id="divRodape" runat="server" class="rodapeBoletim">
                                    <div ng-if="boletim.possuiFreqExterna" id="divFreqExterna" runat="server" style="text-align:left;">
                                        <span>
                                            <span>
                                                <asp:Literal ID="lblFreqExterna" runat="server"></asp:Literal></span>
                                        </span>
                                    </div>
                                    <span>
                                        <span>
                                            <asp:Literal ID="lblRodape" runat="server" /></span>
                                    </span>
                                </div>
                                <div ng-if="boletim.ensinoInfantil" id="divRodapeInfantil" runat="server" class="rodapeBoletim">
                                    <div ng-if="boletim.possuiFreqExterna" id="divFreqExternaInfantil" runat="server" style="text-align:left;">
                                        <span>
                                            <span>
                                                <asp:Literal ID="lblFreqExternaInfantil" runat="server"></asp:Literal></span>
                                        </span>
                                    </div>
                                    <span>
                                        <span>
                                            <asp:Literal ID="lblRodapeInfantil" runat="server" /></span>
                                    </span>
                                </div>

                            </fieldset>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>