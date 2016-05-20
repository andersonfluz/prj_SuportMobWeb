angular.module('HelpMeApp', []).controller('PainelBordoCtrl',
    function ($scope, $http) {
        
        $http({
            url: '/api/painelbordo',
            method: "GET",
            params: { parametro: 0 }
        }).success(function (data) {
                 $scope.chamados = data;
        });

        $http({
            url: '/api/painelbordo',
            method: "GET",
            params: { parametro: 1 }
        }).then(function (response) {
            $scope.chamadoshoje = response.data;
        });

        $http({
            url: '/api/painelbordo',
            method: "GET",
            params: { parametro: 2 }
        }).success(function (data) {
            $scope.chamadoshojetecnico = data;
        });
        
        $scope.atualizar = function () {
            $http.get('/api/painelbordo/ChamadosHoje')
             .success(function (data) {
                 $scope.chamados = data;
             });
        };
    });