import React, { Component } from "react";
import { withRouter, Link } from 'react-router-dom';
import { AppBar, Toolbar, Typography, Button, Snackbar } from 'material-ui';
import { UserStore } from './stores';
import moment from 'moment';
import Constants from './constants';
import AppDispatcher from './appDispatcher';

class Layout extends Component {
    constructor(props) {
        super(props);
        this._appDispatcher = null;
        this._sessionFrame = null;
        this._interval = null;
        this._checkSession = false;
        this.navigate = this.navigate.bind(this);
        this.handleUserLogin = this.handleUserLogin.bind(this);
        this.handleUserLogout = this.handleUserLogout.bind(this);
        this.handleClickLogout = this.handleClickLogout.bind(this);
        this.handleCheckSession = this.handleCheckSession.bind(this);
        this.handleSnackbarClose = this.handleSnackbarClose.bind(this);
        this.displayMessage = this.displayMessage.bind(this);
        this.handleMessage = this.handleMessage.bind(this);
        this.state = {
            checkSession : false,
            isLoggedIn: false,
            snackbarMessage: '',
            isSnackbarOpened: false,
            role: ''
        };
    }

    navigate(href) {
        this.props.history.push(href);
    }

    /**
    * Snackbar is closed.
    */
    handleSnackbarClose(message) {
        this.setState({
            isSnackbarOpened: false
        });
    }

    /**
    * Display the message in the snackbar.
    */
    displayMessage(message) {
        this.setState({
            isSnackbarOpened: true,
            snackbarMessage: message
        });
    }

    handleUserLogin() {
        var self = this;
        var user = UserStore.getUser();
        var userInfo = UserStore.getUserInfo();
        var withSession = user['with_session'];
        this.setState({
            isLoggedIn: true,
            role: userInfo['role']
        });
        if (!withSession) {
            // CHECK THE ACCESS TOKEN VALIDITY.
            self._interval = setInterval(function () {
                user = UserStore.getUser();
                if (!user['access_token']) {
                    clearInterval(self._interval);
                    self._interval = null;
                    AppDispatcher.dispatch({
                        actionName: Constants.events.USER_LOGGED_OUT
                    });
                    return;
                }

                var accessToken = user['access_token'];
                var accessTokenPayload = JSON.parse(window.atob(accessToken.split('.')[1]));
                var expirationTime = moment.unix(accessTokenPayload['exp']);
                var now = moment();
                if (expirationTime < now) {
                    clearInterval(self._interval);
                    self._interval = null;
                    AppDispatcher.dispatch({
                        actionName: Constants.events.USER_LOGGED_OUT
                    });
                }
            }, 3*1000);
            return;
        }

        // CHECK THE SESSION.
        this.setState({
            checkSession: true
        });
    }

    handleUserLogout() {
        var self = this;
        if(self._interval !== null) {
            clearInterval(self._interval);
            self._interval = null;
        }

        self.setState({
            isLoggedIn: false,
            checkSession: false
        });
        self._checkSession = false;
        window.removeEventListener("message", self.handleMessage, false);
    }

    handleClickLogout() {
        var user = UserStore.getUser();
        var withSession = user['with_session'];
        if (!withSession)
        {
            AppDispatcher.dispatch({
                actionName: Constants.events.USER_LOGGED_OUT
            });
            return;
        }   

        var url = "http://localhost:60000/end_session?post_logout_redirect_uri=http://localhost:64950/end_session&id_token_hint="+ user['id_token'];
        var w = window.open(url, '_blank');
        var interval = setInterval(function() {
            if (w.closed) {
                clearInterval(interval);
                return;
            }

            var href = w.location.href;
            if (href === "http://localhost:64950/end_session") {                
                clearInterval(interval);
                w.close();
                AppDispatcher.dispatch({
                    actionName: Constants.events.USER_LOGGED_OUT
                });
            }
        }); 
    }

    handleCheckSession() {
        // CHECK THE SESSION
        var self = this;
        if (self._checkSession) {
            return;
        }

        self._checkSession = true;
        window.addEventListener("message", self.handleMessage, false);
        var originUrl = window.location.protocol + "//" + window.location.host;
        self._interval = setInterval(function() { 
            var session = UserStore.getUser();
            var message = "ResourceManagerClientId ";
            if (session) {
                message += session['session_state'];
            } else {
                session += "tmp";
            }
            
            var win = self._sessionFrame.contentWindow;
            win.postMessage(message, "http://localhost:60000");
        }, 3*1000);
    }

    handleMessage(e) {
        var self = this;
        if (e.data === 'error' || e.data === 'changed') {
            console.log(e.data);
            AppDispatcher.dispatch({
                actionName: Constants.events.USER_LOGGED_OUT
            });
            self.props.history.push('/');
        }
    }    

    render() {
        var self = this;
        return (<div>
            <AppBar>
                <Toolbar>
                    <Typography variant="title" color="inherit">Website Authentication</Typography>
                    <Button color="inherit" onClick={() => self.navigate('/about')}>About</Button>
                    {self.state.isLoggedIn && self.state.role === 'patient' && (
                        <Button color="inherit" onClick={() => self.navigate('/prescriptions')}>My prescriptions</Button>                        
                    )}
                    {self.state.isLoggedIn && self.state.role === 'doctor' && (
                        <Button color="inherit" onClick={() => self.navigate('/prescriptions/assign')}>Assign prescription</Button>
                    )}
                    {!self.state.isLoggedIn  && (
                        <Button color="inherit" onClick={() => self.navigate('/login')}>Login</Button>                        
                    )}
                    {self.state.isLoggedIn && (
                        <Button color="inherit" onClick={() => self.handleClickLogout()}>Logout</Button>                        
                    )}
                </Toolbar>
            </AppBar>
            <div style={{marginTop: "100px"}}>
                {this.props.children}
            </div>
            { this.state.checkSession && (<div>
                    <iframe ref={(elt) => { self._sessionFrame = elt; self.handleCheckSession(); }} id="session-frame" src="http://localhost:60000/check_session" style={{display: "none"}} /> 
                </div>
            )}
            <Snackbar anchorOrigin={{ vertical: 'bottom', horizontal: 'center' }} open={self.state.isSnackbarOpened} onClose={self.handleSnackbarClose} message={<span>{self.state.snackbarMessage}</span>} />
        </div>);
    }

    componentDidMount() {
        var self = this;
        self._appDispatcher = AppDispatcher.register(function (payload) {
            switch (payload.actionName) {
                case Constants.events.DISPLAY_MESSAGE:
                    self.displayMessage(payload.data);
                    break;
            }
        });
        var authenticatedUser = UserStore.getUser();
        if (authenticatedUser && authenticatedUser['id_token']) {
            self.handleUserLogin();
        }

        UserStore.addLoginListener(self.handleUserLogin);
        UserStore.addLogoutListener(self.handleUserLogout);
    }

    componentWillUnmount() {
        AppDispatcher.unregister(this._appDispatcher);
    }
}

export default withRouter(Layout);