import $ from 'jquery';

module.exports = {
    /**
    * Authenticate the end-user with login & password.
    */
    authenticate: function (request) {
        return new Promise(function (resolve, reject) {
            var data = JSON.stringify(request);
            $.ajax({
                url: '/Home/Authenticate',
                method: 'POST',
                data: data,
                contentType: 'application/json'
            }).then(function (data) {
                resolve(data);
            }).fail(function (e) {
                reject(e);
            });
        });
    },
    /**
    * Send the confirmation code.
    */
    sendConfirmationCode: function (phoneNumber) {
        return new Promise(function (resolve, reject) {
            var data = JSON.stringify({ phone_number: phoneNumber });
            $.ajax({
                url: '/Home/SendConfirmationCode',
                method: 'POST',
                data: data,
                contentType: 'application/json'
            }).then(function (r) {
                resolve(r);
            }).fail(function (e) {
                reject(e);
            });
        });
    },
    /**
     * Validate the confirmation code.
     */
    validateConfirmationCode: function (phoneNumber, confirmationCode) {
        return new Promise(function (resolve, reject) {
            var data = JSON.stringify({ phone_number: phoneNumber, confirmation_code: confirmationCode });
            $.ajax({
                url: '/Home/ConfirmConfirmationCode',
                method: 'POST',
                data: data,
                contentType: 'application/json'
            }).then(function (r) {
                resolve(r);
            }).fail(function (e) {
                reject(e);
            });
        });
    }
};