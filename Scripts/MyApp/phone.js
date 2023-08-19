myApp.directive('deviceSelector', ['$http', function ($http) {
    return {
        restrict: 'E',
        scope: {
        ngModel: '=', // Two-way binding with ngModel for selected device
        },
        template: `
     
      <select ng-model="ngModel" ng-options="device.Id as device.Name for device in devices">
        <option value="">All</option>
      </select>
    `,
        link: function (scope) {
            // Fetch available devices from the server
            function getData() {
                $http.get('/api/device/GetAllDevices')
                    .then(function (response) {
                        scope.devices = response.data;
                    })
                    .catch(function (error) {
                        console.log('Error retrieving devices:', error);
                    });
            }

            getData(); // Fetch the devices immediately
        },
    };

}]);
myApp.controller('phoneController', ['$scope', '$http', '$uibModal', '$timeout', function ($scope, $http, $uibModal, $timeout) {
    $scope.phones = [];
    $scope.devices = [];
    $scope.NumberFilter = '';
    $scope.DeviceFilter = '';
    $scope.GetAllPhones = function () {
         $http.get('/api/phone/GetAllPhones')
            .then(function (response) {

                $scope.phones = response.data;
                console.log("Success in retrieving phones");
            })
            .catch(function (error) {
                console.log('Error retrieving phones:', error);
            });
    };
    $scope.GetAllPhones();

    // Function to get all devices
    $scope.GetAllDevices = function () {
        $http.get('/api/device/GetAllDevices')
            .then(function (response) {
                $scope.devices = response.data;
            })
            .catch(function (error) {
                console.log('Error retrieving devices:', error);
            });
    };
    $scope.GetAllDevices();
    $scope.getDeviceName = function (deviceId) {
        const device = $scope.devices.find(device => device.Id === deviceId);
        return device ? device.Name : '';
    };
    $scope.AddPhone = function (phone) {
      

        $http.post('/api/phone/AddPhone', phone)
            .then(function (response) {
                $scope.successMessage = "Phone added successfully!";
                $timeout(function () {
                    $scope.successMessage = "";
                }, 5000);
                $scope.GetAllPhones();
            })
            .catch(function (error) {
                console.log('Error adding phone:', error);
            });
    };
    $scope.UpdatePhone = function (phone) {
        var updatedPhone = {
            Id: phone.Id,
            Number: phone.Number,
            DeviceId: phone.DeviceId
        };
        if (updatedPhone.Number.length >= 8) {
            $http.put('/api/phone/UpdatePhone', updatedPhone)
                .then(function (response) {
                    $scope.successMessage = "Phone updated successfully!";
                    $timeout(function () {
                        $scope.successMessage = "";
                    }, 5000);
                    $scope.GetAllPhones();
                })
                .catch(function (error) {
                    console.log('Error updating phone:', error);
                });
        } else {
            $scope.errorMessage = "The phone number should be of 8 digits";
            $timeout(function () {
                $scope.errorMessage = "";
            }, 3000);
        }
        
    };
    $scope.FilterPhones = function () {
  

        $http.get('/api/phone/FilterPhones', { params: { numberFilter: $scope.NumberFilter,deviceFilter: $scope.DeviceFilter } })
            .then(function (response) {
                $scope.phones = response.data;
            })
            .catch(function (error) {
                console.log('Error filtering phones:', error);
            });
    };
    $scope.openmodel = function (phone,devices) {
        var modalInstance = $uibModal.open({
            templateUrl: "../../modal/phoneModal.html",
            controller: "ModalContentCtrl",
            size: 'sm',
            resolve: {

                phone: function () {
                    return phone ? angular.copy(phone) : {};
                },
                devices: function () {
                    return devices ? angular.copy(devices) : {};
                }
             
            }
        });
        modalInstance.result.then(function (response) {

            if (phone) {

                console.log('Response received:', response);

                $scope.UpdatePhone(response);
            } else {

                $scope.AddPhone(response);
            }
        }
            , function () { });

    };

    $scope.phoneReport = [];
    $scope.RegistrationFilter = '';
    $scope.DeviceFilter = '';
   
    $scope.GetPhoneReport = function () {
        console.log("entered the function");
        $http.get('/api/phone/GetPhoneReport', {
            params: {
                RegistrationFilter: $scope.RegistrationFilter,
                DeviceFilter: $scope.DeviceFilter,
            
            }
        })
            .then(function (response) {
                $scope.phoneReport = response.data;
                console.log(response.data);
            })
            .catch(function (error) {
                console.log('Error retrieving clients report:', error);
            });
    };

    // Initialize clients report

    $scope.GetPhoneReport();

}]);
myApp.controller('ModalContentCtrl', ['$scope', '$uibModalInstance', 'phone', 'devices', function ($scope, $uibModalInstance, phone,devices) {
    console.log('Received device object:', phone);

    $scope.phone = phone ? angular.copy(phone) : {};
    console.log('Copied device object to $scope.device:', $scope.phone);
    $scope.devices = devices;
    $scope.ok = function () {
        var response = {
            Id: phone.Id,
            Number: $scope.number,
            DeviceId: $scope.device,
            device : phone.device

        };

        console.log('Response to be sent:', response);

        $uibModalInstance.close(response);
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
}]);