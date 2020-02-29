import React, { Component } from 'react'
import Select from './Select'
import * as fns from 'date-fns'

export default class DateTimeRange extends Component {
    static getDefaultRangeDate() {
        const currentDate = new Date()
        return {
            startDate: fns.startOfMonth(currentDate),
            endDate: fns.endOfMonth(currentDate)
        }
    }

    componentDidMount() {
        this.dateChanged('currentMonth')
    }

    dateChanged = (type) => {
        if (this.props.onChange) {
            const currentDate = new Date()
            switch (type) {
                case 'currentMonth':
                    this.props.onChange(fns.startOfMonth(currentDate), fns.endOfMonth(currentDate))
                    break;
                case 'lastMonth':
                    const lastMonthDate = fns.addMonths(currentDate, -1)
                    this.props.onChange(fns.startOfMonth(lastMonthDate), fns.endOfMonth(lastMonthDate))
                    break;
                case 'currentYear':
                    this.props.onChange(fns.startOfYear(currentDate), fns.endOfYear(currentDate))
                    break;
                case 'lastYear':
                    const lastYearDate = fns.addYears(currentDate, -1)
                    this.props.onChange(fns.startOfYear(lastYearDate), fns.endOfYear(lastYearDate))
                    break;
                case 'all':
                    this.props.onChange(new Date(1970, 1, 1), fns.endOfYear(currentDate))
                    break;
                default:
                    throw Error('Invalid type')
            }
        }
    }

    render() {
        return (
            <Select onChange={this.dateChanged} options={[
                { id: 'currentMonth', name: 'Current month' },
                { id: 'lastMonth', name: 'Last month' },
                { id: 'currentYear', name: 'Current year' },
                { id: 'lastYear', name: 'Last year' },
                { id: 'all', name: 'All' },
            ]} />
        );
    }
}