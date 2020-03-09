import React from 'react';
import TransactionView from "./components/TransactionView"
import Menu from "./components/Menu"
import Rules from "./components/Rules/RulesTable"
import Charts from './components/Charts'
import LoginPage from './components/Authentication/LoginPage'
import RegisterPage from './components/Authentication/RegisterPage'
import PrivateRoute from './components/Authentication/PrivateRoute'
import {
  BrowserRouter as Router,
  Switch,
  Route,
  Redirect
} from "react-router-dom";

export default function App() {
  return (
    <Router>
      <Menu></Menu>
      <Switch>
        <PrivateRoute exact path="/" component={TransactionView} />
        <PrivateRoute path="/rules" component={Rules} />
        <PrivateRoute path="/charts" component={Charts} />
        <Route path="/login" component={LoginPage} />
        <Route path="/register" component={RegisterPage} />
        <Redirect from="*" to="/" />  
      </Switch>
    </Router>
  );
}
