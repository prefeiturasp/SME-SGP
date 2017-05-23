'use strict';

(function (angular) {
    'use strict';

    angular
        .module('app')
        .controller('matriculaTurmaController', matriculaTurmaController);

    matriculaTurmaController.$inject = ['$scope', '$timeout', '$http', '$location', '$filter', 'trocarAnoService'];

    function matriculaTurmaController($scope, $timeout, $http, $location, $filter, trocarAnoService) {

        this.reload = function () {
            initVars();
            getMatricula();
        };
        trocarAnoService.addSubscriber(this);

        function init() {
            configVariables();
        };

        function configVariables() {
            $scope.matriculaLoaded = false;
            $scope.baseUrl = $location.absUrl().split("/");
            $scope.site = $scope.baseUrl[0] + "//" + $scope.baseUrl[2]; // site;
            $scope.logos = core;
            $scope.api = api;

            initVars();
            getMatricula();
        };

        function initVars() {
            $scope.matriculaLoaded = false;
            $scope.matricula = {};
            $scope.params = params;
            $scope.mensagemErro = "";
            $scope.mensagemAlerta = "";
        };

        function getMatricula() {
            if (!$scope.params || !$scope.params.alu_id || !$scope.params.mtu_id) {
                $scope.mensagemErro = "Parâmetros inválidos";
            } else {
                var url = $scope.api + "/matriculaTurma?alu_id=" + $scope.params.alu_id + "&mtu_id=" + $scope.params.mtu_id;

                $http({
                    method: 'GET',
                    url: url
                }).then(function successCallback(response) {
                    if (response.data == null) {
                        $scope.mensagemErro = "Falha inesperada ao carregar dados da matrícula.";
                    }
                    else if (response.data && response.data.Status && response.data.Status == 1) {
                        $scope.mensagemErro = response.data.StatusDescription;
                    }
                    else {
                        try {
                            $scope.matricula = response.data;
                        }
                        catch (e) {
                            console.log(e);
                            $scope.mensagemErro = "Ocorreu um erro ao carregar dados da matrícula.";
                        }
                    }
                }, function errorCallback(response) {
                    if (response.status == 404)
                        $scope.mensagemErro = "Falha ao recuperar os dados - API indisponível";
                    else if (response.status == 500)
                        $scope.mensagemErro = "Falha ao recuperar os dados - erro na API";
                    else
                        $scope.mensagemErro = "Falha inesperada ao carregar dados da matrícula.";
                }).finally(function () {
                    $scope.matriculaLoaded = true;
                });
            }
        };

        $scope.getPhotoStudent = function __getPhotoStudent(id) {
            if (!id) return "../../App_Themes/IntranetSMEBootStrap/images/imgsAreaAluno/fotoAluno.png"
            else return $scope.site + "/WebControls/RelatorioPedagogico/imagem.ashx?idfoto=" + id;
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
