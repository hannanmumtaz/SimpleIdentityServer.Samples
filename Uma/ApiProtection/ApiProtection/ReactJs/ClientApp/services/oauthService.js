import $ from 'jquery';
import Constants from '../constants';
import { UserStore } from '../stores';

module.exports = {
    getUmaProtectionAccessToken: function () {
        return new Promise(function (resolve, reject) {
            var data = { client_id: Constants.clientId, client_secret: Constants.clientSecret, scope: 'uma_protection', grant_type: 'client_credentials' };
            const searchParams = Object.keys(data).map((key) => {
                return encodeURIComponent(key) + '=' + encodeURIComponent(data[key]);
            }).join('&');
            $.get(Constants.authWellKnownConfiguration).then(function (r) {
                $.ajax(r['token_endpoint'], {
                    type: 'POST',
                    contentType: 'application/x-www-form-urlencoded',
                    data: searchParams
                }).then(function (data) {
                    resolve(data);
                }).fail(function (e) {
                    reject(e);
                });
            }).fail(function () {
                reject(e);
            });
        });
    },
    getAccessTokenWithUmaGrantType: function (ticket) {
        return new Promise(function (resolve, reject) {
            var user = UserStore.getUser();
            var data = { client_id: Constants.clientId, client_secret: Constants.clientSecret, ticket: ticket, grant_type: 'uma_ticket', claim_token: user['id_token'], claim_token_format: 'http://openid.net/specs/openid-connect-core-1_0.html#IDToken' };
            const searchParams = Object.keys(data).map((key) => {
                return encodeURIComponent(key) + '=' + encodeURIComponent(data[key]);
            }).join('&');
            $.get(Constants.authWellKnownConfiguration).then(function (r) {
                $.ajax(r['token_endpoint'], {
                    type: 'POST',
                    contentType: 'application/x-www-form-urlencoded',
                    data: searchParams
                }).then(function (data) {
                    resolve(data);
                }).fail(function (e) {
                    reject(e);
                });
            }).fail(function () {
                reject(e);
            });
        });
    },
    getPermissionTicket: function (resourceSetId, scopes, accessToken) {
        return new Promise(function (resolve, reject) {
            var data = JSON.stringify({ resource_set_id: resourceSetId, scopes: scopes });            
            $.get(Constants.authWellKnownConfiguration).then(function (r) {
                $.ajax(r['permission_endpoint'], {
                    type: 'POST',
                    contentType: 'application/json',
                    headers: {
                        'Authorization': 'Bearer ' + accessToken
                    },
                    data: data
                }).then(function (data) {
                    resolve(data);
                }).fail(function (e) {
                    reject(e);
                });
            }).fail(function () {
                reject(e);
            });
        });
    }
};