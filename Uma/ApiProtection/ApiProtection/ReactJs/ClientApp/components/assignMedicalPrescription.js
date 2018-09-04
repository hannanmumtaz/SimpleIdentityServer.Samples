import React, { Component } from "react";
import { Paper, Typography, CircularProgress, Button, Select, MenuItem } from 'material-ui';
import Input, { InputLabel } from 'material-ui/Input';
import { FormControl, FormHelperText } from 'material-ui/Form';
import { withStyles } from 'material-ui/styles';
import { PrescriptionService } from '../services';
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
class AssignMedicalPrescription extends Component {
    constructor(props) {
        super(props);
        this.handleChangeProperty = this.handleChangeProperty.bind(this);
        this.addMedicalPrescription = this.addMedicalPrescription.bind(this);
        this.state = {
            isLoading: false,
            description: '',
            subject: 'patient1'
        };
    }

	/**
    * Change the property.
	**/
    handleChangeProperty(e) {
        var self = this;
        self.setState({
            [e.target.name]: e.target.value
        });
    }

    /**
     * Add medical prescription. 
     **/
    addMedicalPrescription() {
        var self = this;
        self.setState({
            isLoading: true
        });
        PrescriptionService.addPrescription({ description: self.state.description, patient_subject: self.state.subject }).then(function () {
            self.setState({
                isLoading: false
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: 'Prescription has been assigned'
            });
        }).catch(function () {
            self.setState({
                isLoading: false
            });
            AppDispatcher.dispatch({
                actionName: Constants.events.DISPLAY_MESSAGE,
                data: 'An error occured while trying to assign the medical prescription'
            });
        });
    }

    render() {
        var self = this;
        const { classes } = self.props;
        return (<div>
            <Paper className={classes.padding}>
                <Typography variant="headline" component="h3">
                    Assign prescription
                </Typography>
                {self.state.isLoading ? (<CircularProgress />) : (
                    <div>
                        <FormControl fullWidth={true}>
                            <InputLabel>Description</InputLabel>
                            <Input name="description" value={self.state.description} onChange={self.handleChangeProperty} />
                            <FormHelperText>Enter the prescription desciption</FormHelperText>
                        </FormControl>
                        <FormControl fullWidth={true}>
                            <InputLabel>Patient subject</InputLabel>
                            <Select value={self.state.subject} onChange={self.handleChangeProperty} name="subject">
                                <MenuItem value="patient1">patient1</MenuItem>
                                <MenuItem value="patient2">patient2</MenuItem>
                            </Select>
                            <FormHelperText>Enter the patient subject</FormHelperText>
                        </FormControl>
                        <Button variant="raised" color="primary" onClick={self.addMedicalPrescription}>Add prescription</Button>
                    </div>                    
                )}
            </Paper>
        </div>);
    }

    componentDidMount() {
    }
}

export default withStyles(styles)(AssignMedicalPrescription);