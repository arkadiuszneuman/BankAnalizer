import { HubConnectionBuilder } from '@aspnet/signalr';
import userManager from './UserManager'

class HubConnector {
    _hubAddress = "https://localhost:5001/hub/"
    _actions = {}

    init = async () => {
        if (!HubConnector.instance) {
            HubConnector.instance = this;

            this.HubConnection = new HubConnectionBuilder()
                .withUrl(this._hubAddress)
                // .withAutomaticReconnect()
                .build()

            await this._connect();
          }

          return HubConnector.instance;
    }

    async _connect() {
        await this.HubConnection.start();
        await this.HubConnection.invoke('registerClient', userManager.getUserFromStorage().id);
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