import React, { Component } from "react";
import { Route, Redirect } from 'react-router-dom';

import Layout from './layout';
import { Login, About, Profile } from './components';

export const routes = (<Layout>
    <Route exact path='/' component={About} />
    <Route exact path='/about' component={About} />
    <Route exact path='/login' component={Login} />
    <Route exact path='/profile' component={Profile} />
</Layout>);