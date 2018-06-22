import React, { Component } from "react";
import { Grid, Paper, Button, Typography } from 'material-ui';
import { FormControl, FormHelperText } from 'material-ui/Form';
import { withStyles } from 'material-ui/styles';
import Input, { InputLabel } from 'material-ui/Input';
var AppDispatcher = require('../appDispatcher');
import Constants from '../constants';

const styles = theme => ({
    margin: {
        margin: theme.spacing.unit,
    },
    padding: {
        padding: theme.spacing.unit
    }
});

class Login extends Component {
    constructor(props) {
        super(props);
        this.handleChangeProperty = this.handleChangeProperty.bind(this);
        this.handleLocalAuthenticate = this.handleLocalAuthenticate.bind(this);
        this.handleExternalAuthenticate = this.handleExternalAuthenticate.bind(this);
        this.handleExternalAuthenticateWithSession = this.handleExternalAuthenticateWithSession.bind(this);
        this.externalAuthentication = this.externalAuthentication.bind(this);
        this.state = {
            login: '',
            password: '',
            isAuthenticateLoading: false
        };
    }

    handleChangeProperty(e) {
        this.setState({
            [e.target.name] : e.target.value
        });
    }

    handleLocalAuthenticate(e) {
        e.preventDefault();
        var self = this;
        self.setState({
            isAuthenticateLoading: true
        });
    }

    handleExternalAuthenticate() {
        var self = this;
        self.setState({
            isAuthenticateLoading: true
        });
        self.externalAuthentication(false);
    }

    handleExternalAuthenticateWithSession() {
        var self = this;
        self.setState({
            isAuthenticateLoading: true
        });
        self.externalAuthentication(true);
    }

    externalAuthentication(withSession) {
        const clientId = 'Website';
        const callbackUrl = 'http://localhost:64950/callback';
        var self = this;
        var getParameterByName = function (name, url) {
            if (!url) url = window.location.href;
            name = name.replace(/[\[\]]/g, "\\$&");
            var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, " "));
        };
        var self = this;
        self.setState({
            isLoading: true
        });
        var url = "http://localhost:60000/authorization?scope=openid role profile&state=75BCNvRlEGHpQRCT&redirect_uri="
            + callbackUrl
            + "&response_type=id_token token&client_id=" + clientId + "&nonce=nonce&response_mode=query";
        var w = window.open(url, '_blank');
        var interval = setInterval(function () {
            if (w.closed) {
                clearInterval(interval);
                return;
            }

            var href = w.location.href;
            var accessToken = getParameterByName('access_token', href);
            var idToken = getParameterByName('id_token', href);
            if (!idToken && !accessToken) {
                return;
            }

            var user = {
                id_token: idToken,
                access_token: accessToken,
                with_session: withSession,
                with_local_account: false
            };
            if (withSession) {
                var sessionState = getParameterByName('session_state', href);
                if (sessionState) {
                    sessionState = sessionState.replace(' ', '+');
                    user['session_state'] = sessionState;
                }
            }

            AppDispatcher.dispatch({
                actionName: Constants.events.USER_LOGGED_IN,
                data: user
            });
            self.props.history.push('/');
            self.setState({
                isLoading: false
            });
            clearInterval(interval);
            w.close();
        });
    }

    render() {
        var self = this;
        const { classes } = self.props;
        return (<div>
            <Grid container spacing={16}>
                <Grid item xs={12} sm={12}>
                    <Paper className={classes.padding}>
                        <Typography variant="headline" component="h3">
                            Authenticate
                        </Typography>
                    </Paper>
                </Grid>
                <Grid item xs={12} sm={6}>
                    <Paper className={classes.padding}>
                        {self.state.isAuthenticateLoading ? (<span>Loading ...</span>) : (
                            <form onSubmit={self.handleLocalAuthenticate}>
                                {/* Login */}
                                <FormControl fullWidth={true} className={classes.margin}>
                                    <InputLabel>Login</InputLabel>
                                    <Input value={self.state.login} name="login" onChange={self.handleChangeProperty} />
                                    <FormHelperText>Enter your login</FormHelperText>
                                </FormControl>
                                {/* Password */}
                                <FormControl fullWidth={true} className={classes.margin}>
                                    <InputLabel>Password</InputLabel>
                                    <Input value={self.state.password} name="password" type="password" onChange={self.handleChangeProperty} />
                                    <FormHelperText>Enter your password</FormHelperText>
                                </FormControl>
                                <Button type="submit" color="primary" variant="raised">Submit</Button>
                            </form>
                        )}
                    </Paper>
                </Grid>
                <Grid item xs={12} sm={6}>
                    <Paper className={classes.padding}>
                        {self.state.isAuthenticateLoading ? (<span>Loading ...</span>) : (
                            <div>
                                <Button color="primary" variant="raised" onClick={self.handleExternalAuthenticate}>External authentication without session</Button>
                                <Button color="primary" variant="raised" onClick={self.handleExternalAuthenticateWithSession}>External authentication with session</Button>
                            </div>
                        )}
                    </Paper>
                </Grid>
            </Grid>
        </div>);
    }
}

export default withStyles(styles)(Login);