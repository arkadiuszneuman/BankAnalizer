import React from 'react';
import Importer from "./components/Importer"
import Exporter from "./components/Exporter"
import TransactionView from "./components/TransactionView"
import Menu from "./components/Menu"
import Rules from "./components/Rules/RulesTable"
import Charts from './components/Charts'
import {
  BrowserRouter as Router,
  Switch,
  Route,
  Redirect
} from "react-router-dom";
import LoginPage from './components/Authentication/LoginPage'
import RegisterPage from './components/Authentication/RegisterPage'
import PrivateRoute from './components/Authentication/PrivateRoute'


function App() {
  return (
    <Router>
      <Menu></Menu>
      <Switch>
        <Route exact path="/">
          <Importer />
          <Exporter />
          <TransactionView />
        </Route>
        <Route path="/rules">
          <Rules />
        </Route>
        <PrivateRoute path="/charts" component={Charts} />
        <Route path="/login" component={LoginPage} />
        <Route path="/register" component={RegisterPage} />
        <Redirect from="*" to="/" />  
      </Switch>
    </Router>
  );
}

export default App;
