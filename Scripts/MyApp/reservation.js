myApp.controller('reservationController', ['$scope', '$http', '$uibModal', function ($scope, $http, $uibModal) {

    $scope.reservations = [];
    $scope.clients = [];
    $scope.phoneNumbers = [];
    $scope.ClientFilter = '';
    $scope.PhoneNumberFilter = '';

    //function formatBirthDate(birthdate) {
    //    if (!birthdate) return "N/A"; // Handle null birthdate case
    //    var dateObj = new Date(birthdate);
    //    return dateObj.toLocaleDateString(); // Format the date to a localized string
    //} 
    $scope.GetAllReservations = function () {
        $http.get('/api/reservation/GetAllReservations')
            .then(function (response) {
                $scope.reservations = response.data;
                console.log("Success in retrieving reservations");
            })
            .catch(function (error) {
                console.log('Error retrieving reservations:', error);
            });
    };
    $scope.GetAllReservations();
  
    $scope.getClientName = function (ClientId) {
        const client = $scope.clients.find(client => client.Id === ClientId);
        return client ? client.Name : '';
    };
    $scope.FilterReservations = function () {


        $http.get('/api/reservation/FilterReservations', { params: { ClientFilter: $scope.ClientFilter, PhoneNumberFilter: $scope.PhoneNumberFilter } })
            .then(function (response) {
                console.log(response.data);
                $scope.reservations = response.data;
            })
            .catch(function (error) {
                console.log('Error filtering phones:', error);
            });
    };
    // Function to get all clients
    $scope.GetAllClients = function () {
        $http.get('/api/client/GetAllClients')
            .then(function (response) {
                $scope.clients = response.data;
            })
            .catch(function (error) {
                console.log('Error retrieving clients:', error);
            });
    };
    $scope.GetAllClients();

    // Function to get all phone numbers
    $scope.GetAllPhoneNumbers = function () {
        $http.get('/api/phone/GetAllPhones')
            .then(function (response) {
                $scope.phoneNumbers = response.data;
            })
            .catch(function (error) {
                console.log('Error retrieving phone numbers:', error);
            });
    };
    $scope.GetAllPhoneNumbers();




   
}]);

// Define the client-selector directive

myApp.directive('clientSelector', ['$http', function ($http) {
    return {
        restrict: 'E',
        scope: {
            ngModel: '=', // Two-way binding with ngModel for selected device
        },
        template: `
     
      <select ng-model="ngModel" ng-options="client.Id as client.Name for client in clients">
        <option value="">All</option>
      </select>
    `,
        link: function (scope) {
            // Fetch available devices from the server
            function getData() {
                $http.get('/api/client/GetAllClients')
                    .then(function (response) {
                        scope.clients = response.data;
                    })
                    .catch(function (error) {
                        console.log('Error retrieving clients:', error);
                    });
            }

            getData(); // Fetch the devices immediately
        },
    };

}]);

myApp.directive('phoneSelector', ['$http', function ($http) {
    return {
        restrict: 'E',
        scope: {
            ngModel: '=', // Two-way binding with ngModel for selected device
        },
        template: `
     
      <select ng-model="ngModel" ng-options="phone.Id as phone.Number for phone in phones">
        <option value="">All</option>
      </select>
    `,
        link: function (scope) {
            // Fetch available devices from the server
            function getData() {
                $http.get('/api/phone/GetAllPhones')
                    .then(function (response) {
                        scope.phones = response.data;
                    })
                    .catch(function (error) {
                        console.log('Error retrieving phones:', error);
                    });
            }

            getData(); // Fetch the devices immediately
        },
    };

}]);


