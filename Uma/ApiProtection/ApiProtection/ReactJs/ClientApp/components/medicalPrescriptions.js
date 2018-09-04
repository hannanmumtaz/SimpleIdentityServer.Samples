import React, { Component } from "react";
import { Paper, Typography, CircularProgress, List, ListItem, ListItemText, ListItemAvatar } from 'material-ui';
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
            isLoading: true,
            medicalPrescriptions: []
        };
    }

    refreshData() {
        var self = this;
        self.setState({
            isLoading: true
        });
        PrescriptionService.getPrescriptions().then(function (result) {
            self.setState({
                isLoading: false,
                medicalPrescriptions: result.prescriptions
            });
        }).catch(function () {
            self.setState({
                isLoading: false,
                medicalPrescriptions: []
            });
        });
    }

    render() {
        var self = this;
        var medicalPrescriptions = [];
        if (self.state.medicalPrescriptions) {
            self.state.medicalPrescriptions.forEach(function (medicalPrescription) {
                medicalPrescriptions.push(
                    <ListItem>
                        <ListItemAvatar><b>{medicalPrescription['doctor_subject']}</b></ListItemAvatar>
                        <ListItemText>
                            {medicalPrescription['description']}
                        </ListItemText>
                    </ListItem>
                );
            });
        }

        const { classes } = self.props;
        return (<div>
            <Paper className={classes.padding}>
                <Typography variant="headline" component="h3">
                    Prescriptions
                </Typography>
                {self.state.isLoading ? (<CircularProgress />) : (
                    <List>
                        {medicalPrescriptions}
                    </List>
                )}
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