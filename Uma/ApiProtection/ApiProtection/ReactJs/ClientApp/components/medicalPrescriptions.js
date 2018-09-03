import React, { Component } from "react";
import { Paper, Typography, CircularProgress } from 'material-ui';
import { withStyles } from 'material-ui/styles';
import { PrescriptionService } from '../services';
import { UserStore } from '../stores';
import AppDispatcher from '../appDispatcher';
import Constants from '../constants';

const styles = theme => ({
    margin: {
        margin: theme.spacing.unit,
    },
    padding: {
        padding: theme.spacing.unit
    }
});
class MedicalPrescriptions extends Component {
    constructor(props) {
        super(props);
        this.refreshData = this.refreshData.bind(this);
        this.state = {
            isLoading: true
        };
    }

    refreshData() {
        var self = this;
        PrescriptionService.getPrescriptions().then(function (result) {
            console.log(result);
        }).catch(function () {

        });
    }

    render() {
        var self = this;
        const { classes } = self.props;
        return (<div>
            <Paper className={classes.padding}>
                <Typography variant="headline" component="h3">
                    Prescriptions
                </Typography>
            </Paper>
        </div>);
    }

    componentDidMount() {
        var self = this;
        UserStore.addLoginListener(function () {
            self.refreshData();
        });

        if (UserStore.getUser()['id_token']) {
            self.refreshData();
        }
    }
}

export default withStyles(styles)(MedicalPrescriptions);