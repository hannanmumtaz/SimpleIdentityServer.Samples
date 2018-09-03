import React, { Component } from "react";
import { Paper, Typography } from 'material-ui';
import { withStyles } from 'material-ui/styles';
import { OAuthService } from '../services';
import { UserStore } from '../stores';

const styles = theme => ({
    margin: {
        margin: theme.spacing.unit,
    },
    padding: {
        padding: theme.spacing.unit
    }
});
class AssignMedicalPrescription extends Component {
    render() {
        var self = this;
        const { classes } = self.props;
        return (<div>
            <Paper className={classes.padding}>
                <Typography variant="headline" component="h3">
                    Assign prescription
                </Typography>
            </Paper>
        </div>);
    }

    componentDidMount() {
        var getUmaToken = function () {
            OAuthService.getUmaProtectionAccessToken().then(function (r) {
                OAuthService.getPermissionTicket("1", ["write"], r['access_token']).then(function (ticketResult) {
                    OAuthService.getAccessTokenWithUmaGrantType(ticketResult['ticket_id']).then(function (grantedToken) {
                        console.log(grantedToken);
                    });
                });
            });
        };

        UserStore.addLoginListener(function () {
            getUmaToken();
        });

        var user = UserStore.getUser();
        if (user && user['id_token']) {
            getUmaToken();
        }
    }
}

export default withStyles(styles)(AssignMedicalPrescription);