import React, { Component } from 'react';
import {
    NavLink
  } from "react-router-dom";

class Menu extends Component {
  render() {

    return (
          <div className="ui secondary vertical menu">
              <div className="item">
                  <div className="header">Transactions analizer</div>
                  <NavLink to="/" exact={true} activeClassName="active" className="item">Main</NavLink>
                  <NavLink to="/rules" activeClassName="active" className="item">Rules</NavLink>
                  <NavLink to="/charts" activeClassName="active" className="item">Charts</NavLink>
              </div>
          </div>
    )
  }
}

export default Menu