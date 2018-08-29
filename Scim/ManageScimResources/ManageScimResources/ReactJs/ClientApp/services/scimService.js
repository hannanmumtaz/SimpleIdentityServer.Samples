import $ from 'jquery';
import { UserStore } from '../stores';

module.exports = {
    getCurrentUserInformation: function () {
        return new Promise(function (resolve, reject) {
            var user = UserStore.getUser();
            $.ajax({
                url: 'http://localhost:60001/Users/Me',
                method: 'GET',
                headers: {
                    "Authorization": "Bearer " + user['access_token']
                }
            }).then(function (data) {
                resolve(data);
            }).fail(function (e) {
                reject(e);
            });
        });
    },
    getSchemaInformation: function (schemaId) {
        return new Promise(function (resolve, reject) {
            $.ajax({
                url: 'http://localhost:60001/Schemas/' + schemaId,
                method: 'GET'
            }).then(function (data) {
                resolve(data);
            }).fail(function (e) {
                reject(e);
            });
        });
    },
    updateUser: function (request) {
        return new Promise(function (resolve, reject) {
            var data = JSON.stringify(request);
            var user = UserStore.getUser();
            $.ajax({
                url: 'http://localhost:60001/Users/Me',
                method: "PUT",
                data: data,
                contentType: 'application/json',
                headers: {
                    "Authorization": "Bearer " + user['access_token']
                }
            }).then(function (data) {
                resolve(data);
            }).fail(function (e) {
                reject(e);
            });
        });
    }
};