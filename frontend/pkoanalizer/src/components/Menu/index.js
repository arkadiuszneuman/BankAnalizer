import React, { Component } from 'react'
import { NavLink } from "react-router-dom"
import userManager from '../../helpers/api/UserManager'


class Menu extends Component {
  render() {
    const user = userManager.getUserFromStorage()

    return (
          <div className="ui secondary vertical menu">
              <div className="item">
                  <div className="header">Hello {user?.firstName}</div>
                  <NavLink to="/" exact={true} activeClassName="active" className="item">Main</NavLink>
                  <NavLink to="/rules" activeClassName="active" className="item">Rules</NavLink>
                  <NavLink to="/charts" activeClassName="active" className="item">Charts</NavLink>
                  <NavLink to="/users-connections" activeClassName="active" className="item">Users connections</NavLink>
                  <NavLink to="/register" activeClassName="active" className="item">Register</NavLink>
                  <NavLink to="/login" activeClassName="active" className="item">Login</NavLink>
              </div>
          </div>
    )
  }
}

export default Menu