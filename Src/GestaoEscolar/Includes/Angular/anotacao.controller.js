'use strict';

(function (angular) {
    'use strict';

    angular
        .module('app')
        .controller('AnotacaoController', AnotacaoController);

    AnotacaoController.$inject = ['$scope', '$timeout', '$http', '$location', '$filter', 'trocarAnoService'];

    function AnotacaoController($scope, $timeout, $http, $location, $filter, trocarAnoService) {

        this.reload = function () {
            initVars();
            getAnotacoes();
        };
        trocarAnoService.addSubscriber(this);

        function init() {
            configVariables();
        };

        function configVariables() {
            $scope.anotacaoLoaded = false;
            $scope.baseUrl = $location.absUrl().split("/");
            $scope.site = $scope.baseUrl[0] + "//" + $scope.baseUrl[2]; // site;
            $scope.logos = core;
            $scope.api = api;

            initVars();
            getAnotacoes();
        };

        function initVars() {
            $scope.listAnotacoesDocente = [];
            $scope.listAnotacoesGestor = [];
            $scope.params = params;
            $scope.mensagemErro = "";
            $scope.mensagemAlerta = "";
            $scope.anotacaoLoaded = false;
        };

        function getAnotacoes() {
            if (!$scope.params || !$scope.params.alu_id || !$scope.params.ano) {
                $scope.mensagemErro = "Parâmetros inválidos";
            } else {
                var url = $scope.api + "/alunoAnotacao?alu_id=" + $scope.params.alu_id + "&ano=" + $scope.params.ano;

                $http({
                    method: 'GET',
                    url: url
                }).then(function successCallback(response) {
                    if (response.data == null) {
                        $scope.mensagemErro = "Falha inesperada ao carregar as anotações.";
                    }
                    else if (response.data && response.data.Status && response.data.Status == 1) {
                        $scope.mensagemErro = response.data.StatusDescription;
                    }
                    else {
                        try {
                            $scope.listAnotacoesDocente = response.data.anotacoesDocente;
                            $scope.listAnotacoesGestor = response.data.anotacoesGestor;
                        }
                        catch (e) {
                            console.log(e);
                            $scope.mensagemErro = "Ocorreu um erro ao carregar as anotações.";
                        }
                    }
                }, function errorCallback(response) {
                    if (response.status == 404)
                        $scope.mensagemErro = "Falha ao recuperar os dados - API indisponível";
                    else if (response.status == 500)
                        $scope.mensagemErro = "Falha ao recuperar os dados - erro na API";
                    else
                        $scope.mensagemErro = "Falha inesperada ao carregar as anotações.";
                }).finally(function () {
                    $scope.anotacaoLoaded = true;
                });
            }
        };

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
