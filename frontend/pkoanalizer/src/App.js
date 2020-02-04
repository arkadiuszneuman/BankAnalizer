import React from 'react';
import Importer from "./components/Importer"
import TransactionList from "./components/Transactions"
import Menu from "./components/Menu"
import Rules from "./components/Rules/RulesTable"
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
          <TransactionList />
        </Route>
        <Route path="/rules">
          <Rules />
        </Route>
        </Switch>
    </Router>
  );
}

export default App;
