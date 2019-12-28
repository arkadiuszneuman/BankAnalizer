import React, { Component } from 'react';
import { HubConnectionBuilder } from '@aspnet/signalr';

class Importer extends Component {
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

  import = () => {
    this.setState({isLoading: true})

    fetch("https://localhost:5001/api/import", { 
      method: 'get', 
      headers: new Headers({
        'connectionId': this.state.connId
      })
    })
      .then(response => response.json())
      .then(json => console.log(json));
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