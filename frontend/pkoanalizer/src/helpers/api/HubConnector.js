import { HubConnectionBuilder } from '@aspnet/signalr';

class HubConnector {
    _hubAddress = "https://localhost:5001/hub/"
    _connectionId = ""
    _actions = {}

    constructor() {
        if(!HubConnector.instance){
            HubConnector.instance = this;

            this._hubConnection = new HubConnectionBuilder()
                .withUrl(this._hubAddress)
                .build()

            this._connect();
          }

          return HubConnector.instance;
    }

    async _connect() {
        await this._hubConnection.start();
        this._connectionId = await this._hubConnection.invoke('getConnectionId');
        console.log("connectionID: " + this._connectionId);

        this._hubConnection.on('command-completed', (event) => {
            const action = this._actions[event.id];
            if (action) {
                delete this._actions[event.id];
                action(event.object);
            }
        });
    }

    waitForEventResult(eventId, action) {
        this._actions[eventId] = action;
    }

    getConnectionId = () => this._connectionId;
}

const instance = new HubConnector();

export default instance;