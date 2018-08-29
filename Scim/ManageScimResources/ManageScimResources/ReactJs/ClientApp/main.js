import React from "react";
import { render } from 'react-dom';
import { AppContainer } from 'react-hot-loader';
import { BrowserRouter } from 'react-router-dom';
import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider';
import { createMuiTheme } from 'material-ui/styles';
import lightBlue from 'material-ui/colors/lightBlue';
import * as RoutesModule from './routes';

let routes = RoutesModule.routes;

const theme = createMuiTheme({
    palette: {
        primary: lightBlue
    }
});

class Main extends React.Component {
    render() {
        const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
        return (
            <MuiThemeProvider theme={theme}>
                <AppContainer>
                    <BrowserRouter children={routes} basename={baseUrl} />
                </AppContainer>
            </MuiThemeProvider>);
    }
}

render(<Main />, document.getElementById('react-app'));