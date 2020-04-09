import React, { Component } from 'react'
import Date from './Date'
import Select from './Select'
import TimeRange from './TimeRange'

export default class DateTimeRange extends Component {
    state = {
        currentRangeType: 'type',
        dateFrom: new Date(),
        dateTo: new Date()
    }

    rangeChanged = newVal => {
        this.setState({currentRangeType: newVal})
    }

    timeRangeChanged = (dateFrom, dateTo) => {
        this.setState({dateFrom: dateFrom, dateTo: dateTo})
        if (this.props.onChange) {
            this.props.onChange(dateFrom, dateTo)
        }
    }

    startDateChanged = (dateFrom) => {
        this.timeRangeChanged(dateFrom, this.state.dateTo)
    }

    endDateChanged = (dateTo) => {
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
                                    <Date 
                                        id="rangestart"
                                        placeholder="Date from" 
                                        endCalendar="#rangeend" 
                                        value={this.state.dateFrom}
                                        onChange={this.startDateChanged} />
                                </div>
                                <div className="field">
                                    <label>End date</label>
                                    <Date 
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