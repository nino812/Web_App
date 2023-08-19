myApp.controller('loginController', ['$scope', '$http', '$window', function ($scope, $http, $window) {
    $scope.user = {};

    $scope.register = function () {
        $http.post('/api/login/registerUser', $scope.user)
            .then(function (response) {
                if (response.data === "User registered successfully.") {
                    // Redirect to the login page after successful registration
                    $window.location.href = '/Home/Login'; // Change to your login page URL
                } else {
                    // Show error message
                    alert('An error occurred while registering. Please try again.');
                }
            })
            .catch(function (error) {
                console.log('Error during registration:', error);
            });
    };
    $scope.register
    $scope.login = function () {

        console.log($scope.user);

        $http.post('/api/login/loginUser', $scope.user)

            .then(function (response) {
                //if (response.data.success) {
                //    console.log("returned");
                if (response.data === "User authenticated.") {

                    // Redirect to the home or device page on successful login
                    $window.location.href = '/Home/Device'; // Change to your desired page URL
                } else {
                //    // Show error modal for unsuccessful login
                    alert('Invalid username or password. Please try again.');
                }
            })
            .catch(function (error) {
                console.log('Error during login:', error);
            });
    };
}]);