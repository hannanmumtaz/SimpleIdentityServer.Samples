var AppDispatcher = require('../appDispatcher');
var EventEmitter = require('events').EventEmitter;
var $ = require('jquery');
import Constants from '../constants';

const localStorageName = 'sid-webauthentication';

function loadUser(user) {
    window.localStorage.setItem(localStorageName, JSON.stringify(user));
}

function resetUser() {
    window.localStorage.removeItem(localStorageName);
}

function getUser() {
    var json = window.localStorage.getItem(localStorageName);
    if (!json) {
        return {};
    }

    return JSON.parse(json);
}

var UserStore = $.extend({}, EventEmitter.prototype, {
    getUser() {
        return getUser();
    },
    getUserInfo() {
        var user = getUser();
        if (!user['id_token']) {
            return null;
        }

        var idToken = user['id_token'];
        return JSON.parse(window.atob(idToken.split('.')[1]));
    },
    emitLogin: function () {
        this.emit('login');
    },
    emitLogout: function () {
        this.emit('logout');
    },
    addLoginListener: function (callback) {
        this.on('login', callback);
    },
    addLogoutListener: function (callback) {
        this.on('logout', callback);
    },
    removeLoginListener: function (callback) {
        this.removeListener('login', callback);
    },
    removeLogoutistener: function (callback) {
        this.removeListener('logout', callback);
    }
});

AppDispatcher.register(function (payload) {
    switch (payload.actionName) {
        case Constants.events.USER_LOGGED_IN:
            loadUser(payload.data);
            UserStore.emitLogin();
            break;
        case Constants.events.USER_LOGGED_OUT:
            resetUser();
            UserStore.emitLogout();
            break;
        default:
            return true;
    }

    return true;

});

module.exports = UserStore;