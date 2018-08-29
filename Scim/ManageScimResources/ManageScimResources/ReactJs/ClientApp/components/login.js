import React, { Component } from "react";
import { Grid, Paper, Button, Typography, Table, TableHead, TableBody, TableRow, TableCell } from 'material-ui';
import { FormControl, FormHelperText } from 'material-ui/Form';
import { withStyles } from 'material-ui/styles';
import { WebsiteService } from '../services';
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
        this.handleExternalAuthenticateWithSession = this.handleExternalAuthenticateWithSession.bind(this);
        this.state = {
            isAuthenticateLoading: false
        };
    }

    handleChangeProperty(e) {
        this.setState({
            [e.target.name] : e.target.value
        });
    }

    handleExternalAuthenticateWithSession() {
        var self = this;
        self.setState({
            isAuthenticateLoading: true
        });
        self.externalAuthentication(true, false);
    }

    externalAuthentication(withSession, withSms) {
        const clientId = 'ResourceManagerClientId';
        const callbackUrl = 'http://localhost:64950/callback';
        const stateValue = '75BCNvRlEGHpQRCT';
        const nonceValue = 'nonce';
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
        var url = "http://localhost:60000/authorization?scope=openid role profile scim&state="+stateValue+"&redirect_uri="
            + callbackUrl
            + "&response_type=id_token token&client_id=" + clientId + "&nonce=" + nonceValue +"&response_mode=query";
		if (withSms) {
			url += "&amr_values=sms";
		}
		
        var w = window.open(url, '_blank');
        var interval = setInterval(function () {
            if (w.closed) {
                clearInterval(interval);
                return;
            }

            var href = w.location.href;
            var accessToken = getParameterByName('access_token', href);
            var idToken = getParameterByName('id_token', href);
            var state = getParameterByName('state', href);
            if (!idToken && !accessToken) {
                return;
            }

            if (state !== stateValue) {
                return;
            }
            
            var payload = JSON.parse(window.atob(idToken.split('.')[1]));
            if (payload.nonce !== nonceValue) {
                return;
            }

            var user = {
                id_token: idToken,
                access_token: accessToken,
                with_session: withSession
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
                <Grid item xs={12} sm={4}>
                    <Paper className={classes.padding}>
                        {self.state.isAuthenticateLoading ? (<span>Loading ...</span>) : (
                            <div>
                                <Typography variant="subheading">
                                    Implicit workflow (with session)
                                </Typography>
                                <Table>
                                    <TableBody>
                                        <TableRow>
                                            <TableCell>Grant-Type</TableCell>
                                            <TableCell>Implicit workflow</TableCell>
                                        </TableRow>
                                        <TableRow>
                                            <TableCell>Session expiration</TableCell>
                                            <TableCell>The user is automatically disconnected when the cookie of the OPENID server has expired or is invalid</TableCell>
                                        </TableRow>
                                        <TableRow>
                                            <TableCell>Disconnect</TableCell>
                                            <TableCell>The user is redirected to the /end_session web page to remove the cookie</TableCell>
                                        </TableRow>
                                    </TableBody>
                                </Table>
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