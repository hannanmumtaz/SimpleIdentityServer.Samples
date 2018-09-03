import $ from 'jquery';
import Constants from '../constants';
import { UserStore } from '../stores';

module.exports = {
    addPrescription: function (request) {
        return new Promise(function (resolve, reject) {
            var content = JSON.stringify(request);
            $.ajax('/Prescriptions/AddPrescription', {
                type: 'POST',
                contentType: 'application/json',
                data: content,
                headers: {
                    Authorization: 'Bearer '+ UserStore.getUser()['id_token']
                }

            }).then(function (data) {
                resolve(data);
            }).fail(function (e) {
                reject(e);
            });
        });
    },
    getPrescriptions: function (request) {
        return new Promise(function (resolve, reject) {
            var content = JSON.stringify(request);
            $.ajax('/Prescriptions/GetMyPrescriptions', {
                type: 'GET',
                headers: {
                    Authorization: 'Bearer ' + UserStore.getUser()['id_token']
                }
            }).then(function (data) {
                resolve(data);
            }).fail(function (e) {
                reject(e);
            });
        });
    }
};