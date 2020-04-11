import { HubConnectionBuilder } from '@aspnet/signalr';
import userManager from './UserManager'

interface IEvent {
    id: string
    object: any
}

class HubConnector {
    private static instance: HubConnector
    public hubConnection: any

    private _hubAddress = "https://localhost:5001/hub/"
    private _actions: { [id: string] : Function } = {}
    private _errorActions: { [id: string] : Function } = {}

    _executedCommandCompletedActions: Array<any> = []
    _executedCommandErrorActions: Array<any> = []

    init = async () => {
        if (!HubConnector.instance) {
            HubConnector.instance = this;

            this.hubConnection = new HubConnectionBuilder()
                .withUrl(this._hubAddress)
                // .withAutomaticReconnect()
                .build()

            await this._connect()
          }

          return HubConnector.instance;
    }

    async _connect() {
        await this.hubConnection.start()
        await this.hubConnection.invoke('registerClient', userManager.getUserFromStorage().id)
        console.log("Connected to hub")

        this.hubConnection.on('command-completed', (event: IEvent) => {
            this._executedCommandCompletedActions.push(event)
            this._consumeCompleteActions()
        });

        this.hubConnection.on('command-error', (event: IEvent) => {
            this._executedCommandErrorActions.push(event)
            this._consumeErrorActions()
        });
    }

    _consumeCompleteActions = () => {
        this._executedCommandCompletedActions.forEach((event: IEvent) => {
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

    waitForEventResult(eventId: string, action: Function) {
        this._actions[eventId] = action;
    }

    waitForEventErrorResult(eventId: string, action: Function) {
        this._errorActions[eventId] = action;
        this._consumeErrorActions();
    }
}

const instance = async () => await new HubConnector().init();

export default instance();