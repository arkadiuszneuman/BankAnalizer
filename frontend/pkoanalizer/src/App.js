import React from 'react';
import Importer from "./components/Importer"
import TransactionView from "./components/TransactionView"
import Menu from "./components/Menu"
import Rules from "./components/Rules/RulesTable"
import Charts from './components/Charts'
import {
  BrowserRouter as Router,
  Switch,
  Route,
} from "react-router-dom";


function App() {
  return (
    <Router>
      <Menu></Menu>
      <Switch>
        <Route exact path="/">
          <Importer />
          <TransactionView />
        </Route>
        <Route path="/rules">
          <Rules />
        </Route>
        <Route path="/charts">
          <Charts />
        </Route>
      </Switch>
    </Router>
  );
}

export default App;
