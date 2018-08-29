import React, { Component } from "react";
import { Paper, Typography, Button, CircularProgress } from 'material-ui';
import { FormControl, FormHelperText } from 'material-ui/Form';
import Input, { InputLabel } from 'material-ui/Input';
import { withStyles } from 'material-ui/styles';
import { ScimService } from '../services';

const styles = theme => ({
    margin: {
        margin: theme.spacing.unit,
    },
    padding: {
        padding: theme.spacing.unit
    }
});
class Profile extends Component {
    constructor(props) {
        super(props);
        this.refreshData = this.refreshData.bind(this);
        this.handleSave = this.handleSave.bind(this);
        this.handePropertyChange = this.handePropertyChange.bind(this);
        this.state = {
            isLoading: true,
            age: 0
        };
    }

    refreshData() {
        var self = this;
        self.setState({
            isLoading: true
        });
        ScimService.getCurrentUserInformation().then(function (r) {
            self.setState({
                isLoading: false,
                age: r.age
            });
        }).catch(function (e) {
            self.setState({
                isLoading: false,
                age: 0
            });
        });
    }

    handleSave() {
        var self = this;
        self.setState({
            isLoading: true
        });
        var request = { schemas: ["urn:ietf:params:scim:schemas:core:2.0:User"], age: self.state.age };
        ScimService.updateUser(request).then(function () {
            self.setState({
                isLoading: false
            });
        }).catch(function () {
            self.setState({
                isLoading: false
            });
        });
    }

    handePropertyChange(e) {
        var self = this;
        self.setState({
            [e.target.name] : e.target.value
        });
    }

    render() {
        var self = this;
        const { classes } = self.props;
        return (<div>
            <Paper className={classes.padding}>
                <Typography variant="headline" component="h3">
                    Profile
                </Typography>
                {self.state.isLoading ? (<CircularProgress />): (
                    <div>
                        <FormControl fullWidth={true} style={{ margin: "5px" }}>
                            <InputLabel>Age</InputLabel>
                            <Input type="number" value={self.state.age} name="age" onChange={self.handePropertyChange}/>
                            <FormHelperText>Enter the age</FormHelperText>
                        </FormControl>
                        <Button variant="raised" color="primary" onClick={self.handleSave}>Save</Button>
                    </div>
                )}
            </Paper>
        </div>);
    }

    componentDidMount() {
        this.refreshData();
    }
}

export default withStyles(styles)(Profile);