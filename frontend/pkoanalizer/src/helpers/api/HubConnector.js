import { HubConnectionBuilder } from '@aspnet/signalr';

class HubConnector {
    _hubAddress = "https://localhost:5001/hub/"
    _connectionId = ""
    _actions = {}
    UserId = '42A15FE3-905D-4718-9552-968C21BEDC66'

    Init = async () => {
        if (!HubConnector.instance){
            HubConnector.instance = this;

            this.HubConnection = new HubConnectionBuilder()
                .withUrl(this._hubAddress)
                .build()

            await this._connect();
          }

          return HubConnector.instance;
    }

    async _connect() {
        await this.HubConnection.start();
        this._connectionId = await this.HubConnection.invoke('getConnectionId');
        await this.HubConnection.invoke('registerClient', this.UserId, this._connectionId);
        console.log("connectionID: " + this._connectionId);

        this.HubConnection.on('command-completed', (event) => {
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

const instance = async () => await new HubConnector().Init();

export default instance();