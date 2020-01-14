import React, { Component } from 'react';
import { HubConnectionBuilder } from '@aspnet/signalr';
import ApiConnector from '../../helpers/api/ApiConnector'

class Importer extends Component {
  connector = new ApiConnector()

  state = {
    isLoading: false,
    connId: ''
  }

  componentDidMount = () => {
    const hubConnection = new HubConnectionBuilder()
      .withUrl('https://localhost:5001/hub')
      .build()
    
    

    hubConnection
        .start()
        .then(() => {
          hubConnection.invoke('getConnectionId')
            .then(connectionId => {
                console.log("connectionID: " + connectionId);
                this.setState({connId: connectionId});
            })
        })
        .catch(err => console.log('Error while establishing connection :('))

    hubConnection.on('transaction-imported', (event) => {
      this.setState({isLoading: false})
    });
  }

  import = async () => {
    this.setState({isLoading: true})

    const result = await this.connector.get("transaction/import", { 'connectionId': this.state.connId });
    console.log(result);
  }

  render() {
    const loading = this.state.isLoading ? 'loading' : ''
    const classes = `ui button ${loading}`

    return (
      <div>
        <button onClick={this.import} className={classes}>Import</button>
      </div>
    )
  }
}

export default Importer