import { HubConnectionBuilder } from '@aspnet/signalr';
import { v4 as uuidv4 } from 'uuid';

class HubConnector {
    _hubAddress = "https://localhost:5001/hub/"
    _actions = {}
    _userId = uuidv4()

    init = async () => {
        if (!HubConnector.instance) {
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
        await this.HubConnection.invoke('registerClient', this._userId);
        console.log("Connected to hub");

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
}

const instance = async () => await new HubConnector().init();

export default instance();