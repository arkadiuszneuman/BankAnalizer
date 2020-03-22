import React from 'react';
import { Route, Redirect } from 'react-router-dom';
import userManager from '../../helpers/api/UserManager'

const PrivateRoute = ({ component: Component, ...rest }) => (
    <Route {...rest} render={props => (
        userManager.getUserFromStorage()
            ? <Component {...props} />
            : <Redirect to={{ pathname: '/login', state: { from: props.location } }} />
    )} />
)

export default PrivateRoute