var myApp = angular.module("myApp", ['ui.bootstrap']);
myApp.controller('deviceController', ['$scope', '$http', '$uibModal', '$timeout', function ($scope, $http, $uibModal,$timeout) {
    
    $scope.devices = [];
  
    $scope.GetDevices = function () {
        $http.get('/api/device/GetAllDevices')
            .then(function (response) {
                $scope.devices = response.data;
                console.log("Success");
            })
            .catch(function (error) {
                console.log('Error retrieving devices:', error);
            });
           
    };
    $scope.GetDevices();
    $scope.Search = function () {
        $http.get('/api/device/GetFilteredDevices', { params: { keyword: $scope.devname } })
            .then(function (response) {
                $scope.devices = response.data;
            })
            .catch(function (error) {
                console.log('Error retrieving filtered devices:', error);

            });

    };
    $scope.AddDevice = function (device) {
        var device = {
          
            Name: device.Name
        };
        $http.post('/api/device/AddDevice',device)
            .then(function (response) {
              // $scope.showSuccessModal('Client added successfully.');
                $scope.successMessage = "Device added successfully!";
                $timeout(function () {
                    $scope.successMessage = "";
                }, 5000);
               $scope.GetDevices();
            })
            .catch(function (error) {
                $scope.errorMessage = "An error occurred. Please try again.";
                $timeout(function () {
                    $scope.errorMessage = "";
                }, 5000);
            });
    };
    $scope.UpdateDevice = function (device) {
        var updatedevice = {
            Id: device.Id,
            Name: device.Name
        };
        $http.put('/api/device/UpdateDevice', updatedevice)
            .then(function (response) {
                $scope.successMessage = "Device updated successfully!";
                $timeout(function () {
                    $scope.successMessage = "";
                }, 5000);
                $scope.GetDevices();
            })
            .catch(function (error) {
                $scope.errorMessage = "An error occurred. Please try again.";
                $timeout(function () {
                    $scope.errorMessage = "";
                }, 5000);
            });
    };
    $scope.delete = function (nb) {


        $scope.devices.splice(nb, 1);
    };


    $scope.openmodel = function (device) {
        var modalInstance = $uibModal.open({
            templateUrl: "../../modal/modal.html",
            controller: "DeviceModalContentCtrl",
            size: 'sm',
            resolve: {
             
                device: function () {
                    return device ? angular.copy(device) : {};
                }
            }
        });
        modalInstance.result.then(function (response) {
            
            if (device) {

                console.log('Response received:', response);

                    $scope.UpdateDevice(response);
            } else {

                    $scope.AddDevice(response);
                }
            }
        , function () { });

    };
    //$scope.showSuccessModal = function (message) {
    //    var modalInstance = $uibModal.open({
    //        templateUrl: '../../modal/success.html',
    //        controller: 'SuccessModalController',
    //        resolve: {
    //            successMessage: function () {
    //                return message;
    //            }
    //        }
    //    });

    //    modalInstance.result.then(function () {
    //        // Optional: Perform actions after modal is closed
    //    });
    //};

}]);
//myApp.controller('SuccessModalController', ['$scope', '$uibModalInstance', 'successMessage', function ($scope, $uibModalInstance, successMessage) {
//    $scope.successMessage = successMessage;

//    $scope.ok = function () {
//        $uibModalInstance.close();
//    };
//}]);

myApp.controller('DeviceModalContentCtrl', ['$scope', '$uibModalInstance', 'device', function ($scope, $uibModalInstance, device) {
    console.log('Received device object:', device);

    $scope.device = device ? angular.copy(device) : {};
    console.log('Copied device object to $scope.device:', $scope.device);

    $scope.ok = function () {
        var response = {
            Id: device.Id,
            Name: $scope.deviceName
        };
      
        console.log('Response to be sent:', response);

        $uibModalInstance.close(response);
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };



}]);
