import React, { Component } from 'react';

class RuleForm extends Component {
    state = {
        isLoading: false,
        connId: ''
    }
    import = async () => {
        
    }

    render() {
        return (
        <div>
            <form className="ui form">
                <div className="field">
                    <label>Rule name</label>
                    <input type="text" name="rule-name" placeholder="Rule name" />
                </div>
                <div className="field">
                    <label>Text</label>
                    <input type="text" name="text" placeholder="Text" />
                </div>
                <div className="field">
                    <label>Group name</label>
                    <input type="text" name="group-name" placeholder="Group name" />
                </div>
            </form>
        </div>
        )
    }
}

export default RuleForm