'use strict';

(function (angular) {


    angular
        .module('app')
        .requires.push('chart.js');

    angular
        .module('app')
        .requires.push('angular.filter');

    angular
        .module('app')
        .controller('SondagemController', SondagemController);

    SondagemController.$inject = ['$scope', '$timeout', '$http', '$location', '$filter', 'trocarAnoService', 'groupByFilter', '$q'];

    function SondagemController($scope, $timeout, $http, $location, $filter, trocarAnoService, groupByFilter, $q) {

        this.reload = function () {
            initVars();
            getSondagens();
        };
        trocarAnoService.addSubscriber(this);

        function init() {
            configVariables();

            $timeout(function () {
                [].forEach.call(document.querySelectorAll('#area-sondagem .panel-title a'), function (ctrl) {
                    angular.element(ctrl).addClass('collapsed');
                });

                [].forEach.call(document.querySelectorAll('#area-sondagem div.panel-collapse'), function (ctrl) {
                    angular.element(ctrl).removeClass('in');
                });
            }, 700);
        };

        $scope.getGraphLabels = _.memoize(function (id) {
            return $scope.graphLabels[id];
        });

        $scope.getGraphSeries = _.memoize(function (id) {
            return $scope.graphSeries[id];
        });

        $scope.getGraphData = _.memoize(function (id) {
            return $scope.graphData[id];
        });

        $scope.getGraphOptions = _.memoize(function (id) {
            return {
                responsive: true,
                scales: {
                    xAxes: [{
                        display: true,
                        scaleLabel: {
                            display: true,
                            labelString: 'Data de agendamento'
                        }
                        ,
                        ticks: {
                            maxRotation: 45,
                            minRotation: 45
                        }
                    }],
                    yAxes: [
                        {
                            id: 'yaxes',
                            type: 'linear',
                            display: true,
                            position: 'left',
                            scaleLabel: {
                                display: true,
                                labelString: 'Respostas'
                            },
                            ticks: {
                                userCallback: function (value, index, values) {
                                    if ($scope.respDic[id][value]) {
                                        return stringToArray($scope.respDic[id][value], 40);
                                    }
                                    else {
                                        return "";
                                    }
                                }
                                ,
                                beginAtZero: true
                                ,
                                stepSize: 50
                            }
                        }
                    ]
                },
                legend: {
                    display: true,
                    position: 'top'
                },
                tooltips: {
                    callbacks: {
                        label: function (tooltipItem, data) {
                            var resp = []
                            if (data.datasets[tooltipItem.datasetIndex].label && data.datasets[tooltipItem.datasetIndex].label != "") {
                                resp.push(data.datasets[tooltipItem.datasetIndex].label);
                            }

                            if ($scope.respDic[id][data.datasets[tooltipItem.datasetIndex].data[0]]) {
                                resp.push("Resposta: " + $scope.respDic[id][data.datasets[tooltipItem.datasetIndex].data[0]]);
                            }
                            else {
                                resp.push("Sem resposta");
                            }
                            return resp;
                        }
                    }
                }
            };
        });

        function configVariables() {
            $scope.sondagemLoaded = false;
            $scope.baseUrl = $location.absUrl().split("/");
            $scope.site = $scope.baseUrl[0] + "//" + $scope.baseUrl[2]; // site;
            $scope.logos = core;
            $scope.api = api;

            initVars();
            getSondagens();
        };

        function initVars() {
            $scope.sondagemLoaded = false;
            $scope.listSondagens = [];
            $scope.params = params;
            $scope.graph = {}
            $scope.mensagemErro = "";
            $scope.mensagemAlerta = "";
            $scope.graphLabels = {};
            $scope.graphSeries = {};
            $scope.graphData = {};
            $scope.graphDatasetOverride = []
            $scope.graphOptions = {};
            $scope.respDic = {};
            $scope.sondRespDic = {};
        };

        function getSondagens() {
            if (!$scope.params || !$scope.params.alu_id || !$scope.params.ano) {
                $scope.mensagemErro = "Parâmetros inválidos";
            } else {
                var url = $scope.api + "/sondagem?alu_id=" + $scope.params.alu_id + "&ano=" + $scope.params.ano;

                $http.defaults.headers.common.Authorization = 'Bearer ' + Token;

                $http({
                    method: 'GET',
                    url: url
                }).then(function successCallback(response) {
                    if (response.data == null) {
                        $scope.mensagemErro = "Falha inesperada ao carregar as sondagens.";
                    }
                    else if (response.data && response.data.Status && response.data.Status == 1) {
                        $scope.mensagemErro = response.data.StatusDescription;
                    }
                    else {
                        try {
                            modelSondagens(response.data.sondagens);
                        }
                        catch (e) {
                            console.log(e);
                            $scope.mensagemErro = "Ocorreu um erro ao carregar as sondagens.";
                        }
                    }
                }, function errorCallback(response) {
                    if (response.status == 401) {
                        RefreshToken();
                    }else if (response.status == 404)
                        $scope.mensagemErro = "Falha ao recuperar os dados - API indisponível";
                    else if (response.status == 500)
                        $scope.mensagemErro = "Falha ao recuperar os dados - erro na API";
                    else
                        $scope.mensagemErro = "Falha inesperada ao carregar as sondagens.";
                }).finally(function () {
                    $scope.sondagemLoaded = true;
                });
            }
        };

        function modelSondagens(list) {
            var graphLabels = {};
            var graphSeries = {};
            var graphData = {};

            for (var s = 0; s < list.length; s++) {
                var agendamentos = [];

                var aux = 50;
                $scope.respDic[list[s].id] = [];
                $scope.respDic[list[s].id][0] = "";
                $scope.sondRespDic[list[s].id] = []
                $scope.sondRespDic[list[s].id][0] = 0;

                 for (var r = 0; r < list[s].respostas.length; r++) {
                    $scope.respDic[list[s].id][aux] = list[s].respostas[r].descricao;
                    $scope.sondRespDic[list[s].id][list[s].respostas[r].id] = aux;
                    aux = aux + 50;
                }

                //graphLabels.push("");

                graphLabels[list[s].id] = [];
                graphSeries[list[s].id] = [];
                graphData[list[s].id] = {};

                for (var a = 0; a < list[s].agendamentos.length; a++) {
                    
                    
                    graphData[list[s].id][list[s].agendamentos[a].id] = [];

                    var agendamento = { id: list[s].agendamentos[a].id, dataInicio: list[s].agendamentos[a].dataInicio, dataFim: list[s].agendamentos[a].dataFim, respostas: [] }

                    if (graphLabels[list[s].id].indexOf(list[s].agendamentos[a].dataInicio) == -1) {
                        graphLabels[list[s].id].push(list[s].agendamentos[a].dataInicio);
                    }

                    for (var r = 0; r < list[s].agendamentos[a].respostasAluno.length; r++) {

                        var questao = $filter('filter')(list[s].questoes, { id: list[s].agendamentos[a].respostasAluno[r].idQuestao }, true);

                        if (questao.length) {

                            var subQuestao = $filter('filter')(list[s].subQuestoes, { id: list[s].agendamentos[a].respostasAluno[r].idSubQuestao }, true);
                            var resposta = $filter('filter')(list[s].respostas, { id: list[s].agendamentos[a].respostasAluno[r].idResposta }, true);

                            if (subQuestao.length) {

                                if (graphSeries[list[s].id].indexOf("Questão: " + questao[0].descricao + " | Subquestão: " + subQuestao[0].descricao) == -1) {
                                    graphSeries[list[s].id].push("Questão: " + questao[0].descricao + " | Subquestão: " + subQuestao[0].descricao);
                                }

                                if (resposta.length) {
                                    agendamento.respostas.push({ idQuestao: questao[0].id, idSubQ: subQuestao[0].id, subQuestao: subQuestao[0].descricao, idResposta: resposta[0].id, resposta: resposta[0].descricao })
                                    graphData[list[s].id][list[s].agendamentos[a].id].push({ questao: "Questão: " + questao[0].descricao + " | Subquestão: " + subQuestao[0].descricao, resposta: resposta[0].descricao, idResposta: resposta[0].id });
                                }
                                else {
                                    agendamento.respostas.push({ idQuestao: questao[0].id, idSubQ: subQuestao[0].id, subQuestao: subQuestao[0].descricao, idResposta: 0, resposta: "" })
                                    graphData[list[s].id][list[s].agendamentos[a].id].push({ questao: "Questão: " + questao[0].descricao + " | Subquestão: " + subQuestao[0].descricao, resposta: "", idResposta: 0 });
                                }
                            }
                            else {

                                if (graphSeries[list[s].id].indexOf("Questão: " + questao[0].descricao) == -1) {
                                    graphSeries[list[s].id].push("Questão: " + questao[0].descricao);
                                }

                                if (resposta.length) {
                                    agendamento.respostas.push({ idQuestao: questao[0].id, idSubQ: -1, subQuestao: questao[0].descricao, idResposta: resposta[0].id, resposta: resposta[0].descricao })
                                    graphData[list[s].id][list[s].agendamentos[a].id].push({ questao: "Questão: " + questao[0].descricao, resposta: resposta[0].descricao, idResposta: resposta[0].id });
                                }
                                else {
                                    agendamento.respostas.push({ idQuestao: questao[0].id, idSubQ: -1, subQuestao: questao[0].descricao, idResposta: 0, resposta: "" })
                                    graphData[list[s].id][list[s].agendamentos[a].id].push({ questao: "Questão: " + questao[0].descricao, resposta: "", idResposta: 0 });
                                }
                            }
                        } else {
                            var resposta = $filter('filter')(list[s].respostas, { id: list[s].agendamentos[a].respostasAluno[r].idResposta }, true);

                            if (graphSeries[list[s].id].indexOf("Sondagem: " + list[s].titulo) == -1) {
                                graphSeries[list[s].id].push("Sondagem: " + list[s].titulo);
                            }

                            if (resposta.length) {
                                agendamento.respostas.push({ idQuestao: -1, idSubQ: -1, subQuestao: "", idResposta: resposta[0].id, resposta: resposta[0].descricao })
                                graphData[list[s].id][list[s].agendamentos[a].id].push({ questao: "Sondagem: " + list[s].titulo, resposta: resposta[0].descricao, idResposta: resposta[0].id });
                            }
                            else {
                                agendamento.respostas.push({ idQuestao: -1, idSubQ: -1, subQuestao: "", idResposta: 0, resposta: "" })
                                graphData[list[s].id][list[s].agendamentos[a].id].push({ questao: "Sondagem: " + list[s].titulo, resposta: "", idResposta: 0 });
                            }
                        }
                    }

                    agendamentos.push(agendamento);
                }

                var respQ = []
                for (var i = 0; i < agendamentos.length; i++) {
                    for (var j = 0; j < agendamentos[i].respostas.length; j++) {
                        respQ.push({ id: agendamentos[i].id.toString() + ',' +  agendamentos[i].respostas[j].idQuestao.toString() + "," + agendamentos[i].respostas[j].idSubQ.toString(), idResposta: agendamentos[i].respostas[j].idResposta, resposta: agendamentos[i].respostas[j].resposta  })
                    }
                }

                var respQGroup = $filter('toArray')($filter('groupBy')(respQ, 'id'));

                var combResp = []
                if (respQGroup.length) {
                    for (var i = 0; i < respQGroup.length; i++) {
                        if (respQGroup[i].length > 1) {
                            var r = $filter('orderBy')(respQGroup[i], 'idResposta');
                            if (combResp.indexOf(r) == -1) {
                                combResp.push(r);
                            }
                        }
                    }
                }

                for (var c = 0; c < combResp.length; c++) {
                    var resposta = "";
                    var idResposta = "";
                    if (combResp[c].length > 1) {
                        for (var r = 0; r < combResp[c].length; r++) {
                            resposta = resposta + combResp[c][r].resposta + " | ";
                            idResposta = idResposta.toString() + "000" + combResp[c][r].idResposta.toString();
                        }
                        if ($scope.respDic[list[s].id].indexOf(resposta) == -1) {
                            $scope.respDic[list[s].id][aux] = resposta;
                            $scope.sondRespDic[list[s].id][parseInt(idResposta)] = aux;
                            aux = aux + 50;
                        }
                    }
                }

                if (!list[s].questoes.length) {
                    list[s].questoes = [];
                    list[s].questoes.push({ descricao: "" });
                    list[s].questoes[0]["agendamentos"] = [];
                    for (var a = 0; a < agendamentos.length; a++) {
                        var respostas = $filter('toArray')($filter('groupBy')(agendamentos[a].respostas, 'idSubQ'));
                        if (respostas.length) {
                            var resp = [];

                            for (var i = 0; i < respostas.length; i++) {
                                var subQ = { subQuestao: respostas[i][0].subQuestao, respostasAluno: [] };
                                for (var j = 0; j < respostas[i].length; j++) {
                                    subQ.respostasAluno.push({ resposta: respostas[i][j].resposta });
                                }
                                resp.push(subQ);
                            }

                            list[s].questoes[0]["agendamentos"].push({ dataInicio: agendamentos[a].dataInicio, dataFim: agendamentos[a].dataFim, respostas: resp });
                        }
                    }
                }
                else {
                    for (var q = 0; q < list[s].questoes.length; q++) {
                         list[s].questoes[q]["agendamentos"] = []
                        for (var a = 0; a < agendamentos.length; a++) {
                            var respostas =  $filter('toArray')($filter('groupBy')($filter('filter')(agendamentos[a].respostas, { idQuestao: list[s].questoes[q].id }, true), 'idSubQ'));
                            if (respostas.length) { 
                                
                                var resp = [];

                                for (var i = 0; i < respostas.length; i++) {
                                    var subQ = { subQuestao: respostas[i][0].subQuestao, respostasAluno: []};
                                    for (var j = 0; j < respostas[i].length; j++) {
                                        subQ.respostasAluno.push({ resposta: respostas[i][j].resposta });
                                    }
                                    resp.push(subQ);
                                }

                                list[s].questoes[q]["agendamentos"].push({ dataInicio: agendamentos[a].dataInicio, dataFim: agendamentos[a].dataFim, respostas: resp });
                            }
                        }
                    }
                }

                var graphDataAgroup = [];
                var resp = [];
                for (var a = 0; a < list[s].agendamentos.length; a++) {

                    var dadosAgrup = $filter('toArray')($filter('groupBy')(graphData[list[s].id][list[s].agendamentos[a].id], 'questao'), true);    
                    
                    for (var d = 0; d < dadosAgrup.length; d++) {
                        var idResposta = "";
                        if (dadosAgrup[d].length > 1) {
                            for (var v = 0; v < dadosAgrup[d].length; v++) {
                                idResposta = idResposta.toString() + "000" + dadosAgrup[d][v].idResposta.toString();
                            }
                            resp.push( { questao: dadosAgrup[d][0].questao, resp: $scope.sondRespDic[list[s].id][parseInt(idResposta)] });
                        }
                        else {
                            resp.push( { questao: dadosAgrup[d][0].questao, resp: $scope.sondRespDic[list[s].id][dadosAgrup[d][0].idResposta] });
                        }

                    }
                }

                for (var i = 0; i < graphSeries[list[s].id].length; i++) {
                    var respOrd = [];
                    var aux = $filter('filter')(resp, { questao: graphSeries[list[s].id][i] }, true);
                    for (var j = 0; j < aux.length; j++) {
                        respOrd.push(aux[j].resp);
                    }
                    graphDataAgroup.push(respOrd);
                } 

                graphData[list[s].id] = graphDataAgroup;

                if (graphLabels[list[s].id].length == 1) {
                    graphLabels[list[s].id].push("");
                }
            }

            $scope.graphLabels = graphLabels;
            $scope.graphSeries = graphSeries;
            $scope.graphData = graphData;
            $scope.graphDatasetOverride = [{ yAxisID: 'yaxes' }];
         
            $scope.listSondagens = list;
        }

        function stringToArray(string, arraySize) {
            var array = [];
            while (string.length > 0) {
                var end = arraySize;
                if (end > string.length) {
                    end = string.length
                }
                array.push(string.slice(0, end));
                string = string.slice(end, string.length);
            }
            return array;
        }

        function getToken() {
            var deferred = $q.defer();
            $http({
                method: "POST",
                url: "RelatorioPedagogico.aspx/CreateToken",
                dataType: 'json',
                data: '{ "usuario":  "' + Usuario + '", "entidade": "' + Entidade + '", "grupo": "' + Grupo + '" }',
                headers: {
                    "Content-Type": "application/json"
                }
            }).success(function (data) {
                deferred.resolve(data);
            });

            return deferred.promise;
        }

        function RefreshToken() {
            var promise = getToken();
            promise.then(function (data) {
                Token = data.d;
                initVars();
                getSondagens();
            });
        }

        $scope.safeApply = function __safeApply() {
            var $scope, fn, force = false;
            if (arguments.length === 1) {
                var arg = arguments[0];
                if (typeof arg === 'function') {
                    fn = arg;
                } else {
                    $scope = arg;
                }
            } else {
                $scope = arguments[0];
                fn = arguments[1];
                if (arguments.length === 3) {
                    force = !!arguments[2];
                }
            }
            $scope = $scope || this;
            fn = fn || function () { };

            if (force || !$scope.$$phase) {
                $scope.$apply ? $scope.$apply(fn) : $scope.apply(fn);
            } else {
                fn();
            }
        };

        init();
    }
})(angular);
