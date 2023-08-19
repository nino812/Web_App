myApp.directive('clientTypeFilter', function () {
    return {
        restrict: 'E',
        scope: {
            ngModel: '='
        },
        template: `
            <select ng-model="ngModel">
                <option value="">All</option>
                <option value="Individual">Individual</option>
                <option value="Organization">Organization</option>
            </select>
        `
    };
});
myApp.controller('clientController', ['$scope', '$http', '$uibModal', '$timeout', function ($scope, $http, $uibModal, $timeout) {

    $scope.clients = [];
    $scope.reservations = [];

    $scope.clientName = '';
    $scope.clientType = '';
    $scope.clientTypeReport = '';
    var ClientType = {
        Individual: 0,
        Organization: 1
    };
    function calculateAge(birthDate) {
        if (!birthDate) {
            return age=100;
        }
        var today = new Date();
        var birthDateObj = new Date(birthDate);
        var age = today.getFullYear() - birthDateObj.getFullYear();
        var monthDiff = today.getMonth() - birthDateObj.getMonth();

        if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDateObj.getDate())) {
            age--;
        }

        return age;
    }

    // Add a function to format the birthdate
    function formatBirthDate(birthdate) {
        if (!birthdate) return "N/A"; // Handle null birthdate case
        var dateObj = new Date(birthdate);
        return dateObj.toLocaleDateString(); // Format the date to a localized string
    }
    $scope.GetClients = function () {
        $http.get('/api/client/GetAllClients')
            .then(function (response) {
                $scope.clients = response.data.map(function (client) {

                    client.Type = client.Type === ClientType.Individual ? "Individual" : "Organization";

                    // Format the birthdate
                    client.BirthDate = formatBirthDate(client.BirthDate);

                    return client;
                });

                console.log("Success");
            })
            .catch(function (error) {
                console.log('Error retrieving clients:', error);
            });

    };
    $scope.GetClients();

    $scope.Search = function () {

        $http.get('/api/client/GetFilteredClients', {
            params: {
                nameFilter: $scope.clientName,
                typeFilter: $scope.clientType
            }
        })
            .then(function (response) {

                $scope.clients = response.data.map(function (client) {

                    client.Type = client.Type === ClientType.Individual ? "Individual" : "Organization";

                    // Format the birthdate
                    client.BirthDate = formatBirthDate(client.BirthDate);

                    return client;
                });
            })
            .catch(function (error) {
                console.log('Error retrieving filtered clients:', error);
            });

    };

    $scope.AddClient = function (client) {

        var age = calculateAge(client.BirthDate);
        if (age >= 18) {
            $http.post('/api/client/AddClient', client)
                .then(function (response) {
                    $scope.successMessage = "Client added successfully!";
                    $timeout(function () {
                        $scope.successMessage = "";
                    }, 5000);
                   // $scope.GetDevices();
                    $scope.GetClients();
                })
                .catch(function (error) {
                    console.log('Error adding client:', error);
                });
        }
        else {
            $scope.errorMessage = "The client cannot be under 18";
            $timeout(function () {
                $scope.errorMessage = "";
            }, 3000);
        }
    };
    $scope.UpdateClient = function (client) {
        var age = calculateAge(client.BirthDate);
        if (age >= 18) {
            var updateClient = {
                Id: client.Id,
                Name: client.Name,
                Type: client.Type,
                BirthDate: client.clientBirthDate
            };
            updateClient.BirthDate = formatBirthDate(client.BirthDate);

            $http.put('/api/client/UpdateClient', updateClient)
                .then(function (response) {
                    $scope.successMessage = "Client updated successfully!";
                    $timeout(function () {
                        $scope.successMessage = "";
                    }, 5000);
                    $scope.GetClients();
                    
                })
                .catch(function (error) {
                    console.log('Error updating client:', error);
                });
        } else {
            $scope.errorMessage = "The client cannot be under 18";
            $timeout(function () {
                $scope.errorMessage = "";
            }, 3000);
        }
    };
    $scope.openmodel = function (client) {
        var modalInstance = $uibModal.open({
            templateUrl: "../../modal/clientModal.html",
            controller: "ClientModalController",
            size: 'm',
            resolve: {
                client: function () {
                    return client ? angular.copy(client) : {};
                }
            }

        });
        modalInstance.result.then(function (response) {
            if (client) {
                console.log('Response received:', response);

                $scope.UpdateClient(response);
            }
            else $scope.AddClient(response);

        }, function () { });

    };
    // Add a function to get the clients report
    $scope.clientsReport = [];
    $scope.GetClientsReport = function () {
        console.log("entered the function");
        $http.get('/api/client/GetClientsReport', {
            params: {
                typeFilter: $scope.clientTypeReport
            }
        })
            .then(function (response) {
                $scope.clientsReport = response.data;
            })
            .catch(function (error) {
                console.log('Error retrieving clients report:', error);
            });
    };

    // Initialize clients report

    $scope.GetClientsReport();

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

    // Call the GetAllReservations function to populate reservations when the page loads
    $scope.GetAllReservations();

    $scope.isPhoneNumberReserved = function (clientId) {
        var currentDate = new Date(); // Get the current date and time
       // console.log(formatBirthDate(currentDate));
        console.log($scope.reservations);

        for (var i = 0; i < $scope.reservations.length; i++) {
            if ($scope.reservations[i].Client === clientId && formatBirthDate($scope.reservations[i].BED) <= formatBirthDate(currentDate) && (formatBirthDate($scope.reservations[i].EED) >= formatBirthDate(currentDate) || $scope.reservations[i].EED === null)) {
                return true; // Client's phone number is reserved
            }
        }
        return false; // Client's phone number is not reserved
    };
    //$scope.isPhoneNumberReserved = function (clientId) {
    //    $http.get('/api/reservation/EffectiveReservation', { params: { Client: clientId } })
    //        .then(function (response) {
    //            // Check if there's an effective reservation for the client
    //            if (response.data.length > 0) {
    //                return true; // Client's phone number is reserved
    //            } else {
    //                return false; // Client's phone number is not reserved
    //            }
    //        })
    //        .catch(function (error) {
    //            console.log('Error checking effective reservation:', error);
    //            return false; // Error occurred, consider as not reserved
    //        });
   // };

    $scope.UnReserve = function (client) {
        var confirmUnreserve = confirm("Are you sure you want to unreserve the phone registration?");
        if (confirmUnreserve) {
            $http.post('/api/reservation/UnReserve', client)
                .then(function (response) {
                    alert("Phone registration successfully unreserved.");
                    // You might want to refresh your reservations list after unreserving
                     $scope.GetAllReservations();
                })
                .catch(function (error) {
                    console.log('Error during unreserving:', error);
                    alert('An error occurred while unreserving the phone registration.');
                });
        }
    };

   
    $scope.reservePhoneNumber = function (clientId) {
        var modalInstance = $uibModal.open({
            templateUrl: '../../modal/phoneNumberModal.html', // The template for the modal dialog
            controller: 'phoneNumberModalController', // The controller for the modal dialog
            resolve: {
                clientId: function () {
                    return clientId; // Pass the clientId to the modal controller
                }
            }
        });
        modalInstance.result.then(function (phoneNumberId) {
            // Add a new entry to the PhoneNumberReservation table in the database
            console.log('Response received:', phoneNumberId , clientId);

            var reservationData = {
                Client: clientId,
                PhoneNumber: phoneNumberId,
                BED: new Date(), // Current date and time as the BED
                EED: null // Set EED to null initially
            };
            console.log(reservationData);
            $http.post('/api/reservation/AddPhoneNumberReservation', reservationData)
                .then(function (response) {
                    // Reservation successful, refresh the reservations list
                    $scope.successMessage = "Phone number reserved successfully!";
                    $timeout(function () {
                        $scope.successMessage = "";
                    }, 5000);
                    $scope.GetAllReservations();
                })
                .catch(function (error) {
                    console.log('Error reserving phone number:', error);
                });
        });
    };

    
}]);

myApp.controller('phoneNumberModalController', ['$scope', '$uibModalInstance', 'clientId', '$http', function ($scope, $uibModalInstance, clientId, $http) {
    console.log('Received client object:', clientId);

    // $scope.phoneNumbers = [];
    //$scope.selectedPhoneNumberId = null;

    // Function to get all available phone numbers
    //$scope.getAllPhoneNumbers = function () {
    //    $http.get('/api/phone/GetAllPhones')
    //        .then(function (response) {
    //            $scope.phoneNumbers = response.data;
    //        })
    //        .catch(function (error) {
    //            console.log('Error retrieving phone numbers:', error);
    //        });
    //};

    // Function to save the selected phone number and close the modal
    $scope.ok = function () {
        console.log('Response to be sent:', $scope.PhoneNumberFilter);

        if ($scope.PhoneNumberFilter) {
            $uibModalInstance.close($scope.PhoneNumberFilter);
        }
    };

    // Function to cancel and close the modal
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

    // Initialize the modal by fetching available phone numbers
    //$scope.getAllPhoneNumbers();
}]);
myApp.controller('ClientModalController', ['$scope', '$uibModalInstance', 'client', function ($scope, $uibModalInstance, client) {
    console.log('Received client object:', client);
    $scope.client = client ? angular.copy(client) : {};
    console.log('Copied device object to $scope.client:', $scope.client);

    $scope.ok = function () {
        var response = {
            Id: client.Id,
            Name: $scope.clientName,
            Type: $scope.clientType,
            BirthDate: $scope.clientBirthDate
        };
        console.log('Response to be sent:', response);
        $uibModalInstance.close(response);

    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };




}]);