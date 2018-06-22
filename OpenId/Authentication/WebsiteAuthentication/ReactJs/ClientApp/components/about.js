import React, { Component } from "react";
import { Paper, Typography } from 'material-ui';
import { withStyles } from 'material-ui/styles';

const styles = theme => ({
    margin: {
        margin: theme.spacing.unit,
    },
    padding: {
        padding: theme.spacing.unit
    }
});
class About extends Component {
    render() {
        var self = this;
        const { classes } = self.props;
        return (<div>
            <Paper className={classes.padding}>
                <Typography variant="headline" component="h3">
                    About
                </Typography>
            </Paper>
        </div>);
    }
}

export default withStyles(styles)(About);