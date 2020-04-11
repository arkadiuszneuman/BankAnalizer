import React, { Component } from 'react';
import TransactionView from "./components/TransactionView"
import Menu from "./components/Menu"
import Rules from "./components/Rules/RulesTable"
import Charts from './components/Charts'
import UsersConnections from './components/UsersConnections'
import LoginPage from './components/Authentication/LoginPage'
import RegisterPage from './components/Authentication/RegisterPage'
import PrivateRoute from './components/Authentication/PrivateRoute'
import {
  BrowserRouter as Router,
  Switch,
  Route,
  Redirect
} from "react-router-dom";
import hubConnector from './helpers/api/HubConnector'

export default class App extends Component {

  componentDidMount = async () => {
    await hubConnector.init()
  }

  render() {
    return (
      <Router>
        
        {/* <Menu static={false} /> */}
        <div className="pusher">
          <div className="container">
            <div className="menu">
              <Menu static={true} />
            </div>
            <div className="content">
              <Switch>
                <PrivateRoute exact path="/" component={TransactionView} />
                <PrivateRoute path="/rules" component={Rules} />
                <PrivateRoute path="/charts" component={Charts} />
                <PrivateRoute path="/users-connections" component={UsersConnections} />
                <Route path="/login" component={LoginPage} />
                <Route path="/register" component={RegisterPage} />
                <Redirect from="*" to="/" />  
              </Switch>
            </div>
          </div>
        </div>
        
      </Router>
    );
  }
}
