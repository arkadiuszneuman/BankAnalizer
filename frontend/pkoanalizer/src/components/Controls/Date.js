import React, { Component } from 'react';

export default class Date extends Component {
    componentDidMount() {
        window.$('#' + this.props.id).calendar({
            type: 'date',
            startCalendar: window.$(this.props.startCalendar),
            endCalendar: window.$(this.props.endCalendar),
            onChange: date => this.handleChange(date),
            initialDate: this.props?.value
        })
    }

    handleChange = date => {
        if (this.props.onChange) {
            this.props.onChange(date)
        }
    }

    render() {
        const placeholder = this.props.placeholder ?? 'Date'
        const id = this.props.id
        return (
            <div>
                <div className="ui calendar" id={id}>
                    <div className="ui input left icon">
                        <i className="calendar icon"></i>
                        <input type="text" placeholder={placeholder} />
                    </div>
                </div>
            </div>
        );
    }
}