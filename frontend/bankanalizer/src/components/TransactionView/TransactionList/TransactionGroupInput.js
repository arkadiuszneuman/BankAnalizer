import React, { Component } from 'react';

export default class TransactionGroupInput extends Component {
    state = {
        currentGroup: ''
    }

    addGroupByEnter = (event) => {
        if (event.key === 'Enter') {
            this.addGroup()
        }
    }

    addGroup = () => {
        const currentGroup = this.state.currentGroup
        this.setState({currentGroup: '' })
        if (this.props.onAddGroup)
            this.props.onAddGroup(currentGroup)
    }

    updateCurrentGroup = (event) => {
        this.setState({currentGroup: event.target.value })
    }

    render() {
        return (
            <div className="ui mini action input">
                <input type="text" placeholder="Group" 
                        value={this.state.currentGroup} 
                        onChange={this.updateCurrentGroup}
                        onKeyDown={this.addGroupByEnter} />
                <button className="ui mini teal icon button" onClick={this.addGroup}>
                    <i className="add icon"></i>
                </button>
            </div>
        )
    }
}