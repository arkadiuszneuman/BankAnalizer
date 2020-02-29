import React, { Component } from 'react';

export default class Date extends Component {
    state = {
        value: ''
    }

    componentDidMount = () => {
        window.$('.ui.dropdown').dropdown()
    }

    handleChange = event => {
        const value = event.target.value
        if (this.props.onChange) {
            this.props.onChange(value)
        }

        this.setState({value: value});
    }

    render() {
        const options = this.props.options ?? []
        return (
            <select className="ui dropdown" value={this.state.value} onChange={this.handleChange}>
                {options.map(option => 
                    <option key={option.id} value={option.id}>{option.name}</option>
                )}
            </select>

        );
    }
}