import React, { Component } from 'react'
import { NavLink } from "react-router-dom"
import userManager from '../../helpers/api/UserManager'

declare const $: any

interface IProps {
  static: boolean
}

class Menu extends Component<IProps, {}> {
  componentDidMount() {
    // window.$('.ui.sidebar').sidebar('setting', 'transition', 'push').sidebar('toggle')
    $('a.sidebar-toggle').click(function () {
      $('#sidebar').sidebar('toggle')
    })
  }

  render() {
    const user = userManager.getUserFromStorage()
    const staticMenu = this.props.static 

    function menu() {
      return (
        <div className="item">
          <div className="header">Hello {user?.firstName}</div>
          <NavLink to="/" exact={true} activeClassName="active" className="item">Main</NavLink>
          <NavLink to="/rules" activeClassName="active" className="item">Rules</NavLink>
          <NavLink to="/charts" activeClassName="active" className="item">Charts</NavLink>
          <NavLink to="/users-connections" activeClassName="active" className="item">Users connections</NavLink>
          <NavLink to="/register" activeClassName="active" className="item">Register</NavLink>
          <NavLink to="/login" activeClassName="active" className="item">Login</NavLink>
        </div>
      )
    }

    return (
      <div>
        {!staticMenu &&
          <div id="sidebar" className="ui secondary vertical menu sidebar">
            {menu()}
          </div>
        }
        {staticMenu &&
        <div>
          <div className="ui vertical left fixed menu">
            {menu()}
          </div>
        
          {/* <div className="ui sidebar visible fixed main menu screen tablet mobile only">
            <a className="launch icon item sidebar-toggle">
              <i className="sidebar icon"></i>
            </a>
          </div> */}
          </div>
        }
      </div>
    )
  }
}

export default Menu