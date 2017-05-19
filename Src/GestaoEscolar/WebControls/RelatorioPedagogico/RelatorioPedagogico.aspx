<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RelatorioPedagogico.aspx.cs" Inherits="GestaoEscolar.WebControls.RelatorioPedagogico.RelatorioPedagogico" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../App_Themes/IntranetSMEBootStrap/bootstrap.css" rel="stylesheet" media="screen">
    <link href="../../App_Themes/IntranetSMEBootStrap/custom.css" rel="stylesheet" media="screen">

    <style>
        html {
            overflow: scroll;
            overflow-x: hidden;
        }

        ::-webkit-scrollbar {
            width: 0px; /* remove scrollbar space */
            background: transparent; /* optional: just make scrollbar invisible */
        }
        /* optional: show position indicator in red */
        ::-webkit-scrollbar-thumb {
            background: #FF0000;
        }
    </style>
</head>
<body>

    <script src="../../Includes/jquery-2.0.3.min.js" type="text/javascript"></script>
    <script src="../../Includes/bootstrap/bootstrap.min.js" type="text/javascript"></script>

    <script src="../../Includes/Angular/angular.js" type="text/javascript"></script>
    <script src="../../Includes/Angular/module.js" type="text/javascript"></script>
    <script src="../../Includes/Angular/boletim.controller.js" type="text/javascript"></script>
    <script src="../../Includes/Angular/calendario.controller.js" type="text/javascript"></script>
    <script src="../../Includes/Angular/sondagem.controller.js" type="text/javascript"></script>
    <script src="../../Includes/Angular/anotacao.controller.js" type="text/javascript"></script>
    <script src="../../Includes/Angular/justificativaFalta.controller.js" type="text/javascript"></script>
    <script src="../../Includes/Angular/movimentacao.controller.js" type="text/javascript"></script>
    <script src="../../Includes/Angular/matriculaTurma.controller.js" type="text/javascript"></script>
    <script src="../../Includes/scrolling.js" type="text/javascript"></script>
    <script type="text/javascript">
        var params =
            {
                alu_id : <%= alu_id %>,
                mtu_id : <%= mtu_id %>,
                AluIds: '<%= alu_id %>',
                MtuIds: '<%= mtu_id %>',
                TpcId: <%= tpc_id %>,
                ano: <%= ano %>,
                exibicaoNome: <%= (int)MSTech.GestaoEscolar.BLL.eExibicaoNomePessoa.NomeSocial %>
                };

        var site = '';
        var core = '<%= MSTech.GestaoEscolar.Web.WebProject.ApplicationWEB.UrlCoreSSO %>';
            var api = '<%= MSTech.GestaoEscolar.Web.WebProject.ApplicationWEB.UrlGestaoAcademicaWebApi %>';
        var msgDocenciaCompartilhada = '<%= GetGlobalResourceObject("WebControls", "BoletimCompletoAluno.UCDadosBoletimAluno.MensagemCompartilhada") %>';
        var msgAlunoSemDadosPeriodo = '<%= GetGlobalResourceObject("WebControls", "BoletimCompletoAluno.UCBoletimAngular.MensagemAlunoSemDadosPeriodo") %>';
    </script>

    <form id="form1" runat="server">
        <div ng-app="app">
            <div ng-controller="CalendarioController" ng-cloak>
                <div class="loader" ng-if="!mensagemErro && !mensagemAlerta && listCalendario.length == 0">
                    <img class="imgLoader" src="../../App_Themes/IntranetSMEBootStrap/images/ajax-loader.gif" style="border-width: 0px;">
                </div>
                <aside class="nav-aside" style="top: 5px; z-index: 1">
                    <div class="dropdown pull-left">
                        <button class="btn btn-default dropdown-toggle dropdown-ico" type="button" id="dropdownMenu1"
                            data-toggle="dropdown" aria-haspopup="true" aria-expanded="true" ng-disabled="listCalendario.length <= 1">
                            <i class="material-icons md-color md-18">&#xE878;</i><span>{{params.ano}}</span>
                            <span class="caret"></span>
                        </button>
                        <ul class="dropdown-menu" aria-labelledby="dropdownMenu1" ng-if="listCalendario.length > 1">
                            <li ng-repeat="calendario in listCalendario" ng-if="calendario.cal_ano!=params.ano">
                                <a href="#" ng-click="trocarAno(calendario.cal_ano, calendario.mtu_id, calendario.tpc_id)">{{calendario.cal_ano}}</a>
                            </li>
                        </ul>
                    </div>
                </aside>
            </div>

            <div ng-controller="matriculaTurmaController" ng-cloak>

                <aside class="nav-aside" style="top: 60px; z-index: 0;">

                    <!-- Header relatorio -->
                    <header class="info-header">
                        <div class="foto-aluno">
                            <img class="imgFoto" src="{{getPhotoStudent(matricula.arq_idFoto)}}" alt="Foto do aluno" />
                        </div>
                        <div class="info-aluno">
                            <h2 class="info-nome">{{matricula.pes_nome}} <small>{{matricula.alc_matricula}}</small>
                            </h2>
                            <span class="info-turma">{{matricula.tci_nome}} / {{matricula.tur_codigo}}
                            </span>
                            <h2 class="info-escola">{{matricula.esc_nome}} -  {{matricula.uad_nome}}
                            </h2>
                        </div>
                    </header>

                    <a href="#nav-list-collapse" class="btn btn-primary btn-nav-toggle" role="button" data-toggle="collapse" aria-expanded="false" aria-controls="nav-list-collapse"><i class="material-icons">&#xE5D2;</i></a>
                    <div class="collapse" id="nav-list-collapse">
                        <ul class="nav-list" id="nav-list">
                            <li>
                                <a href="#area-notas-faltas">Notas e faltas</a>
                            </li>
                            <li>
                                <a href="#area-obs-conselho">Observações do conselho de classe</a>
                            </li>
                            <li>
                                <a href="#area-com-estudos">Compromisso de estudo</a>
                            </li>
                            <li>
                                <a href="#area-sondagem">Resultados de sondagem</a>
                            </li>
                            <li>
                                <a href="#area-obs-individuais">Observações individuais</a>
                            </li>
                            <li>
                                <a href="#area-just-faltas">Justificativa de faltas</a>
                            </li>
                            <li>
                                <a href="#area-cons-mov">Remanejamento e Reclassificacao</a>
                            </li>
                        </ul>
                    </div>
                </aside>

            </div>

            <!--Conteúdo-->
            <div role="main" id="acontent">
                <button title="Ir para o topo" class="btn btn-primary btn-float btn-float-3" id="btn-top"><i class="material-icons">&#xE5D8;</i></button>
                <button title="Voltar" class="btn btn-primary btn-float btn-float-2"><i class="material-icons">&#xE166;</i></button>
                <button title="Imprimir" class="btn btn-primary btn-float"><i class="material-icons">&#xE8AD;</i></button>

                <!-- Boletim -->
                <div ng-controller="BoletimRelPedagogicoController" ng-cloak>
                    <section id="area-notas-faltas" class="section-area" ng-repeat="boletim in listBoletins | limitTo : 1">
                        <h3 ng-if="!boletim.ensinoInfantil"><i class="material-icons pull-left">&#xE5CC;</i>{{boletim.nomeNota}}s e faltas</h3>
                        <div class="conteudo">

                            <table class="table table-responsive-list" ng-if="!boletim.ensinoInfantil">
                                <thead>
                                    <tr>
                                        <th rowspan="2" style="vertical-align: middle">Disciplina</th>
                                        <th class="text-center" ng-repeat="periodo in boletim.periodos">{{periodo.tpc_nome}}</th>
                                        <th rowspan="2" class="text-center" style="vertical-align: middle">Síntese Final</th>
                                        <th rowspan="2" class="text-center" style="vertical-align: middle">Total de Ausências</th>
                                        <th rowspan="2" class="text-center" style="vertical-align: middle">Frequência Final(%)</th>
                                    </tr>
                                    <tr>
                                        <th class="text-center" ng-repeat="periodo in boletim.periodos">{{boletim.nomeNota}}</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr ng-repeat="(indexMat, materia) in boletim.matter" ng-class="checkParImpar(indexMat)" ng-if="!materia.enriquecimentoCurricular||!materia.recuperacao">
                                        <td data-header="Disciplina"><strong>{{materia.Disciplina}}</strong></td>
                                        <td class="text-center" ng-repeat="(indexAval, avaliacao) in materia.avaliacao"
                                            ng-if="indexAval%2 == 0"
                                            rowspan="{{ indexAval%2 == 0 || !materia.tipoComponenteRegencia ? 1 : boletim.QtComponenteRegencia }}">{{avaliacao.conceito}}
                                        </td>
                                        <td class="text-center cel-destaque" data-header="Final - Nota"><strong>{{materia.MediaFinal}}</strong></td>
                                        <td class="text-center cel-destaque" data-header="Final - Faltas" ng-if="indexMat == 0 || !materia.tipoComponenteRegencia" rowspan="{{ materia.tipoComponenteRegencia ? boletim.QtComponenteRegencia : 1 }}"><strong>{{materia.totalFaltas}}</strong></td>
                                        <td class="text-center cel-destaque" data-header="Final - Frequencia" ng-if="boletim.exibeCompensacaoAusencia && (indexMat == 0 || boletim.QtComponenteRegencia == 0)" rowspan="{{ boletim.QtComponenteRegencia > 0 ? boletim.QtComponentes : 1 }}"><strong>{{materia.FrequenciaFinalAjustada}}</strong></td>
                                    </tr>
                                </tbody>
                            </table>


                            <table class="table table-responsive-list" ng-if="(boletim.showCurricularEnrichment || boletim.showRecuperacao) && !boletim.ensinoInfantil">
                                <thead>
                                    <tr>
                                        <th rowspan="2" style="vertical-align: middle">Enriq. curricular / Projetos / Ativ. compl.</th>
                                        <th class="text-center" ng-repeat="periodo in boletim.periodos">{{periodo.tpc_nome}}</th>
                                        <th rowspan="2" class="text-center" style="vertical-align: middle">Parecer Final</th>
                                        <th rowspan="2" class="text-center" style="vertical-align: middle">Total de Ausências</th>
                                        <th rowspan="2" class="text-center" style="vertical-align: middle">Frequência Final(%)</th>
                                    </tr>
                                    <tr>
                                        <th class="text-center" ng-repeat="periodo in boletim.periodos">Faltas</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr ng-repeat="(indexMat, materia) in boletim.enrichment" ng-class="checkParImpar(indexMat)" ng-if="materia.enriquecimentoCurricular||materia.recuperacao">
                                        <td data-header="Disciplina">{{materia.Disciplina}}</td>
                                        <td class="text-center" ng-repeat="(indexAval, avaliacao) in materia.notas">{{avaliacao.nota.numeroFaltas != null ? avaliacao.nota.numeroFaltas : "-"}}</td>
                                        <td class="text-center" data-header="Parecer Final">{{materia.parecerFinal}}</td>
                                        <td class="text-center" data-header="Final - Faltas">{{materia.totalFaltas}}</td>
                                        <td class="text-center" data-header="Final - Frequencia">{{materia.FrequenciaFinalAjustada}}</td>
                                    </tr>
                                    <tr ng-repeat="(indexMat, materia) in boletim.recovery" ng-class="checkParImpar(indexMat)" ng-if="materia.enriquecimentoCurricular||materia.recuperacao">
                                        <td data-header="Disciplina">{{materia.Disciplina}}</td>
                                        <td class="text-center" ng-repeat="(indexAval, avaliacao) in materia.notas">{{avaliacao.nota.numeroFaltas != null ? avaliacao.nota.numeroFaltas : "-"}}</td>
                                        <td class="text-center" data-header="Parecer Final">{{materia.parecerFinal}}</td>
                                        <td class="text-center" data-header="Final - Faltas">{{materia.totalFaltas}}</td>
                                        <td class="text-center" data-header="Final - Frequencia">{{materia.FrequenciaFinalAjustada}}</td>
                                    </tr>
                                </tbody>
                            </table>

                        </div>
                    </section>

                    <!-- OBSERVACOES CONSELHO -->
                    <section id="area-obs-conselho" class="section-area">
                        <h3><i class="material-icons pull-left">&#xE5CC;</i>Observações do conselho de classe</h3>
                        <div class="conteudo">
                            <div class="sr-only">Conteúdo dividido por Accordion</div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="panel-group accordion" id="accordion" role="tablist" aria-multiselectable="true">
                                        <div class="panel panel-default">
                                            <div class="panel-heading" role="tab" id="headingOne">
                                                <h4 class="panel-title">
                                                    <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne">1º Bimestre
                                                    </a>
                                                </h4>
                                            </div>
                                            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne">
                                                <div class="panel-body">
                                                    Anim pariatur cliche reprehenderit, enim eiusmod high life accusamus terry richardson ad squid. 3 wolf moon officia aute, non cupidatat skateboard dolor brunch. Food truck quinoa nesciunt laborum eiusmod. Brunch 3 wolf moon tempor, sunt aliqua put a bird on it squid single-origin coffee nulla assumenda shoreditch et. Nihil anim keffiyeh helvetica, craft beer labore wes anderson cred nesciunt sapiente ea proident. Ad vegan excepteur butcher vice lomo. Leggings occaecat craft beer farm-to-table, raw denim aesthetic synth nesciunt you probably haven't heard of them accusamus labore sustainable VHS.
                                                </div>
                                            </div>
                                        </div>
                                        <div class="panel panel-default">
                                            <div class="panel-heading" role="tab" id="headingTwo">
                                                <h4 class="panel-title">
                                                    <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">2º Bimestre
                                                    </a>
                                                </h4>
                                            </div>
                                            <div id="collapseTwo" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingTwo">
                                                <div class="panel-body">
                                                    Anim pariatur cliche reprehenderit, enim eiusmod high life accusamus terry richardson ad squid. 3 wolf moon officia aute, non cupidatat skateboard dolor brunch. Food truck quinoa nesciunt laborum eiusmod. Brunch 3 wolf moon tempor, sunt aliqua put a bird on it squid single-origin coffee nulla assumenda shoreditch et. Nihil anim keffiyeh helvetica, craft beer labore wes anderson cred nesciunt sapiente ea proident. Ad vegan excepteur butcher vice lomo. Leggings occaecat craft beer farm-to-table, raw denim aesthetic synth nesciunt you probably haven't heard of them accusamus labore sustainable VHS.
                                                </div>
                                            </div>
                                        </div>
                                        <div class="panel panel-default">
                                            <div class="panel-heading" role="tab" id="headingThree">
                                                <h4 class="panel-title">
                                                    <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseThree" aria-expanded="false" aria-controls="collapseThree">3º Bimestre
                                                    </a>
                                                </h4>
                                            </div>
                                            <div id="collapseThree" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingThree">
                                                <div class="panel-body">
                                                    Anim pariatur cliche reprehenderit, enim eiusmod high life accusamus terry richardson ad squid. 3 wolf moon officia aute, non cupidatat skateboard dolor brunch. Food truck quinoa nesciunt laborum eiusmod. Brunch 3 wolf moon tempor, sunt aliqua put a bird on it squid single-origin coffee nulla assumenda shoreditch et. Nihil anim keffiyeh helvetica, craft beer labore wes anderson cred nesciunt sapiente ea proident. Ad vegan excepteur butcher vice lomo. Leggings occaecat craft beer farm-to-table, raw denim aesthetic synth nesciunt you probably haven't heard of them accusamus labore sustainable VHS.
                                                </div>
                                            </div>
                                        </div>
                                        <div class="panel panel-default">
                                            <div class="panel-heading" role="tab" id="headingFour">
                                                <h4 class="panel-title">
                                                    <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseFour" aria-expanded="false" aria-controls="collapseFour">4º Bimestre
                                                    </a>
                                                </h4>
                                            </div>
                                            <div id="collapseFour" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour">
                                                <div class="panel-body">
                                                    Anim pariatur cliche reprehenderit, enim eiusmod high life accusamus terry richardson ad squid. 3 wolf moon officia aute, non cupidatat skateboard dolor brunch. Food truck quinoa nesciunt laborum eiusmod. Brunch 3 wolf moon tempor, sunt aliqua put a bird on it squid single-origin coffee nulla assumenda shoreditch et. Nihil anim keffiyeh helvetica, craft beer labore wes anderson cred nesciunt sapiente ea proident. Ad vegan excepteur butcher vice lomo. Leggings occaecat craft beer farm-to-table, raw denim aesthetic synth nesciunt you probably haven't heard of them accusamus labore sustainable VHS.
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </section>


                    <!-- Compromisso de estudo -->
                    <section id="area-com-estudos" class="section-area" ng-if="!boletim.fechamentoPorImportacao && boletim.tci_exibirBoletim && !boletim.ensinoInfantil" style="display: {{boletim.displayPerfilAluno}}">
                        <h3><i class="material-icons pull-left">&#xE5CC;</i>Compromisso de estudo</h3>
                        <div class="conteudo">
                            <div class="sr-only">Conteúdo dividido por Accordion</div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="panel-group accordion" id="accordionComEst" role="tablist" aria-multiselectable="true">
                                        <div class="panel panel-default">
                                            <div class="panel-heading" role="tab" id="headingAtividadeFeita">
                                                <h4 class="panel-title">
                                                    <a role="button" data-toggle="collapse" data-parent="#accordionComEst" href="#collapseAtividadeFeita" aria-expanded="true" aria-controls="collapseAtividadeFeita">O que tenho feito?
                                                    </a>
                                                </h4>
                                            </div>
                                            <div id="collapseAtividadeFeita" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingAtividadeFeita">
                                                <div class="panel-body">
                                                    {{boletim.cpe_atividadeFeita}}
                                                </div>
                                            </div>
                                        </div>
                                        <div class="panel panel-default">
                                            <div class="panel-heading" role="tab" id="headingAtividadeFazer">
                                                <h4 class="panel-title">
                                                    <a role="button" data-toggle="collapse" data-parent="#accordionComEst" href="#collapseAtividadeFazer" aria-expanded="true" aria-controls="collapseAtividadeFazer">O que pretendo fazer?
                                                    </a>
                                                </h4>
                                            </div>
                                            <div id="collapseAtividadeFazer" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingAtividadeFazer">
                                                <div class="panel-body">
                                                    {{boletim.cpe_atividadePretendeFazer}}
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </section>
                </div>

                <!-- SONDAGEM -->
                <div ng-controller="SondagemController" ng-cloak>
                    <section id="area-sondagem" class="section-area" ng-show="listSondagens.length > 0">
                        <h3><i class="material-icons pull-left">&#xE5CC;</i>Resultados de sondagem</h3>
                        <div class="conteudo">
                            <div class="sr-only">Conteúdo dividido por Accordion</div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="panel-group accordion" id="accordion3" role="tablist" aria-multiselectable="true">
                                        <div class="panel panel-default" ng-repeat="sondagem in listSondagens">
                                            <div class="panel-heading" role="tab" id="{{'headingSondagem-' + sondagem.id}}">
                                                <h4 class="panel-title">
                                                    <a role="button" data-toggle="collapse" data-parent="#accordion3" href="{{'#collapseSondagem-' + sondagem.id}}" aria-expanded="true" aria-controls="{{'collapseSondagem-' + sondagem.id}}">{{sondagem.titulo}}
                                                    </a>
                                                </h4>
                                            </div>
                                            <div id="{{'collapseSondagem-' + sondagem.id}}" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="{{'headingSondagem-' + sondagem.id}}">
                                                <div class="panel-body">
                                                    <ul class="list">
                                                        <li ng-repeat="questao in sondagem.questoes">
                                                            <span class="list-header">{{questao.descricao}}</span>
                                                            <ul class="list sub">
                                                                <li ng-repeat="resposta in questao.respostas">
                                                                    <span class="list-item"><strong>{{resposta.subQuestao}}</strong>{{resposta.resposta}}</span>
                                                                </li>
                                                            </ul>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </section>
                </div>

                <div ng-controller="AnotacaoController" ng-cloak>
                    <!-- OBSERVACOES INDIVIDUAIS -->
                    <section id="area-obs-individuais" class="section-area">
                        <h3><i class="material-icons pull-left">&#xE5CC;</i>Observações individuais</h3>
                        <div class="conteudo">
                            <div class="obs-block" ng-repeat="anotacao in listAnotacoesDocente">
                                <p>
                                   {{anotacao.anotacao}}
                                </p>
                                <p class="text-autor">Observação do(a) professor(a) <strong>{{anotacao.nomeDocente}} | {{anotacao.nomeDisciplina}}</strong></p>
                            </div>
                            <div class="obs-block" ng-repeat="anotacao in listAnotacoesGestor">
                                <p>
                                   {{anotacao.anotacao}}
                                </p>
                                <p class="text-autor">Observação do(a) <strong>{{anotacao.funcaoGestor}}</strong></p>
                            </div>
                        </div>
                    </section>
                </div>

                <div ng-controller="JustificativaFaltaController" ng-cloak>
                    <!-- Justificativa de falta -->
                    <section id="area-just-faltas" class="section-area">
                        <h3><i class="material-icons pull-left">&#xE5CC;</i>Justificativa de faltas</h3>
                        <div class="conteudo">
                            <div class="obs-block" ng-repeat="justificativa in listJustificativa">
                                <p>
                                    <strong>{{justificativa.tipo}}</strong><span ng-if="justificativa.observacao"> - {{justificativa.observacao}}</span>
                                </p>
                                <p class="text-autor">Período: <strong>{{justificativa.dataInicio}} - {{justificativa.dataFim ? justificativa.dataFim : "*"}}</strong></p>
                            </div>
                        </div>
                    </section>
                </div>

                <div ng-controller="MovimentacaoController" ng-cloak>
                    <!-- Movimentações -->
                    <section id="area-cons-mov" class="section-area">
                        <h3><i class="material-icons pull-left">&#xE5CC;</i>Remanejamento e Reclassificacao</h3>
                        <div class="conteudo">
                            <div class="obs-block" ng-repeat="movimentacao in listMovimentacoes">
                                <h4>
                                    <strong>{{movimentacao.tipo}}</strong>
                                </h4>
                                <p ng-if="movimentacao.turmaAnterior">
                                    <strong>Origem: </strong>{{movimentacao.escolaAnterior + " | " + movimentacao.turmaAnterior}}
                                </p>
                                <p ng-if="movimentacao.turmaAtual">
                                    <strong>Destino: </strong>{{movimentacao.escolaAtual + " | " + movimentacao.turmaAtual}}
                                </p>
                                <p class="text-autor">Data de movimentação: <strong>{{movimentacao.dataRealizacao}}</strong></p>
                            </div>
                        </div>
                    </section>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
