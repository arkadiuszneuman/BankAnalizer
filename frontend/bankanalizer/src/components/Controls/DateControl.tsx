import React, { Component } from 'react';

declare const $: any

interface IProps {
    id: string,
    value: Date,
    startCalendar?: string,
    endCalendar?: string,
    placeholder?: string

    onChange: (date: Date) => void
}

export default class DateControl extends Component<IProps, {}> {
    componentDidMount() {
        $('#' + this.props.id).calendar({
            type: 'date',
            startCalendar: $(this.props.startCalendar),
            endCalendar: $(this.props.endCalendar),
            onChange: (date: Date) => this.handleChange(date),
            initialDate: this.props?.value
        })
    }

    handleChange = (date: Date) => {
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