import React, { Component } from 'react'
import DateControl from './DateControl'
import Select from './SelectControl'
import TimeRange from './TimeRangeControl'

interface IProps {
    onChange: (dateFrom: Date, dateTo: Date) => void
}

interface IState {
    currentRangeType: string,
    dateFrom: Date,
    dateTo: Date
}

export default class DateTimeRangeControl extends Component<IProps, IState> {
    state = {
        currentRangeType: 'type',
        dateFrom: new Date(),
        dateTo: new Date()
    }

    rangeChanged = (newVal: string) => {
        this.setState({currentRangeType: newVal})
    }

    timeRangeChanged = (dateFrom: Date, dateTo: Date) => {
        this.setState({dateFrom: dateFrom, dateTo: dateTo})
        if (this.props.onChange) {
            this.props.onChange(dateFrom, dateTo)
        }
    }

    startDateChanged = (dateFrom: Date) => {
        this.timeRangeChanged(dateFrom, this.state.dateTo)
    }

    endDateChanged = (dateTo: Date) => {
        this.timeRangeChanged(this.state.dateFrom, dateTo)
    }

    render() {
        const currentRangeType = this.state.currentRangeType

        return (
            <div>
                <div className="ui form">
                    <div className="two fields">
                        <div className="field">
                            <label>Range type</label>
                            <Select 
                                onChange={this.rangeChanged}
                                options={[
                                    { id: 'type', name: 'Date type' },
                                    { id: 'range', name: 'Date range' }
                                ]} />
                        </div>
                        {currentRangeType === 'type' &&
                            <div className="field">
                                <label>Time range</label>
                                <TimeRange onChange={this.timeRangeChanged} />
                            </div>
                        }
                        {currentRangeType === 'range' &&
                            <div className="two fields">
                                <div className="field">
                                    <label>Start date</label>
                                    <DateControl 
                                        id="rangestart"
                                        placeholder="Date from" 
                                        endCalendar="#rangeend" 
                                        value={this.state.dateFrom}
                                        onChange={this.startDateChanged} />
                                </div>
                                <div className="field">
                                    <label>End date</label>
                                    <DateControl 
                                        id="rangeend" 
                                        placeholder="Date to" 
                                        startCalendar="#rangestart" 
                                        value={this.state.dateTo}
                                        onChange={this.endDateChanged} />
                                </div>
                            </div>
                        }
                    </div>
                </div>
            </div>
        );
    }
}