import React, { Component } from "react";
import { withRouter, Link } from 'react-router-dom';
import { AppBar, Toolbar, Typography, Button } from 'material-ui';
import { UserStore } from './stores';
import moment from 'moment';
import Constants from './constants';
var AppDispatcher = require('./appDispatcher');

class Layout extends Component {
    constructor(props) {
        super(props);
        this._sessionFrame = null;
        this._interval = null;
        this.navigate = this.navigate.bind(this);
        this.handleUserLogin = this.handleUserLogin.bind(this);
        this.handleUserLogout = this.handleUserLogout.bind(this);
        this.handleClickLogout = this.handleClickLogout.bind(this);
        this.handleCheckSession = this.handleCheckSession.bind(this);
        this.state = {
            checkSession : false,
            isLoggedIn: false
        };
    }

    navigate(href) {
        this.props.history.push(href);
    }

    handleUserLogin() {
        var self = this;
        var user = UserStore.getUser();
        var withSession = user['with_session'];
        this.setState({
            isLoggedIn: true
        });
        if (!withSession) {
            // CHECK THE ACCESS TOKEN VALIDITY.
            self._interval = setInterval(function () {
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
        this.setState({
            isLoggedIn: false
        });
    }

    handleClickLogout() {        
        AppDispatcher.dispatch({
            actionName: Constants.events.USER_LOGGED_OUT
        });
    }

    handleCheckSession() {
        // CHECK THE SESSION
        var self = this;
        if (self._interval !== null) {
            return;
        }

        var evt = window.addEventListener("message", function (e) {
            if (e.data !== 'unchanged') {
                AppDispatcher.dispatch({
                    actionName: Constants.events.USER_LOGGED_OUT
                });
                self._interval = null;
                self.props.history.push('/');
            }
        }, false);
        var originUrl = window.location.protocol + "//" + window.location.host;
        self._interval = setInterval(function() { 
            var session = UserStore.getUser();
            var message = "Website ";
            if (session) {
                message += session['session_state'];
            } else {
                session += "tmp";
            }
            
            var win = self._sessionFrame.contentWindow;
            win.postMessage(message, "http://localhost:60000");
        }, 3*1000);

    }

    render() {
        var self = this;
        return (<div>
            <AppBar>
                <Toolbar>
                    <Typography variant="title" color="inherit">Website Authentication</Typography>
                    <Button color="inherit" onClick={() => self.navigate('/about')}>About</Button>
                    {!self.state.isLoggedIn && (
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
        </div>);
    }

    componentDidMount() {
        var self = this;
        var authenticatedUser = UserStore.getUser();
        if (authenticatedUser && authenticatedUser['id_token']) {
            self.handleUserLogin();
        }

        UserStore.addLoginListener(self.handleUserLogin);
        UserStore.addLogoutListener(self.handleUserLogout);
    }

    componentWillUnmount() {
        var self = this;
        UserStore.removeLoginListener(self.handleUserLogin);
        UserStore.removeLogoutistener(self.handleUserLogout);
    }
}

export default withRouter(Layout);