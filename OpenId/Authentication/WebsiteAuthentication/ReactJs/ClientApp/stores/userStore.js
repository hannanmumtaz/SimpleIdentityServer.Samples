var AppDispatcher = require('../appDispatcher');
var EventEmitter = require('events').EventEmitter;
var $ = require('jquery');
import Constants from '../constants';

var _user = {};
const localStorageName = 'sid-webauthentication';

function loadUser(user) {
    _user = user;
    window.localStorage.setItem(localStorageName, JSON.stringify(_user));
}

function resetUser() {
    _user = {};
    window.localStorage.removeItem(localStorageName);
}

function restoreUser() {
    var json = window.localStorage.getItem(localStorageName);
    if (!localStorageName) {
        return;
    }

    _user = JSON.parse(json);
}

restoreUser();

var UserStore = $.extend({}, EventEmitter.prototype, {
    getUser() {
        return _user;
    },
    getUserInfo() {
        if (!_user['id_token']) {
            return null;
        }

        var idToken = _user['id_token'];
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