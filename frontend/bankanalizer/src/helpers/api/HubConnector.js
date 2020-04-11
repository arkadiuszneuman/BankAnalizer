import { HubConnectionBuilder } from '@aspnet/signalr';
import userManager from './UserManager'

class HubConnector {
    _hubAddress = "https://localhost:5001/hub/"
    _actions = {}
    _errorActions = {}

    _executedCommandCompletedActions = []
    _executedCommandErrorActions = []

    init = async () => {
        if (!HubConnector.instance) {
            HubConnector.instance = this;

            this.HubConnection = new HubConnectionBuilder()
                .withUrl(this._hubAddress)
                // .withAutomaticReconnect()
                .build()

            await this._connect()
          }

          return HubConnector.instance;
    }

    async _connect() {
        await this.HubConnection.start()
        await this.HubConnection.invoke('registerClient', userManager.getUserFromStorage().id)
        console.log("Connected to hub")

        this.HubConnection.on('command-completed', (event) => {
            this._executedCommandCompletedActions.push(event)
            this._consumeCompleteActions()
        });

        this.HubConnection.on('command-error', (event) => {
            this._executedCommandErrorActions.push(event)
            this._consumeErrorActions()
        });
    }

    _consumeCompleteActions = () => {
        this._executedCommandCompletedActions.forEach(event => {
            const action = this._actions[event.id];
            if (action) {
                delete this._actions[event.id];
                this._executedCommandCompletedActions = this._executedCommandCompletedActions.filter(e => e !== event)
                action(event.object);
            }
        })
    }

    _consumeErrorActions = () => {
        this._executedCommandErrorActions.forEach(event => {
            const action = this._errorActions[event.id]
            if (action) {
                delete this._errorActions[event.id]
                this._executedCommandErrorActions = this._executedCommandErrorActions.filter(e => e !== event)
                action()
            }
        })
    }

    waitForEventResult(eventId, action) {
        this._actions[eventId] = action;
    }

    waitForEventErrorResult(eventId, action) {
        this._errorActions[eventId] = action;
        this._consumeErrorActions();
    }
}

const instance = async () => await new HubConnector().init();

export default instance();