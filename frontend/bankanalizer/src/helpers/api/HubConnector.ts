import { HubConnectionBuilder, HubConnection } from '@microsoft/signalr';
import userManager from './UserManager'

interface IEvent {
    id: string
    object: any
}

class HubConnector {
    private static instance: HubConnector
    public hubConnection!: HubConnection;

    private hubAddress = "https://localhost:5001/hub/"
    private actions: { [id: string] : Function } = {}
    private errorActions: { [id: string] : Function } = {}

    private executedCommandCompletedActions: Array<IEvent> = []
    private executedCommandErrorActions: Array<IEvent> = []

    init = async () => {
        if (!HubConnector.instance) {
            HubConnector.instance = this;

            this.hubConnection = new HubConnectionBuilder()
                .withUrl(this.hubAddress)
                .withAutomaticReconnect({
                    nextRetryDelayInMilliseconds(): number | null {
                        return 1000
                    }
                })
                .build()

            await this.connect()
          }

          return HubConnector.instance;
    }

    private async connect() {
        await this.hubConnection.start()
        await this.hubConnection.invoke('registerClient', userManager.getUserFromStorage().id)
        console.log("Connected to hub")

        this.hubConnection.on('command-completed', (event: IEvent) => {
            this.executedCommandCompletedActions.push(event)
            this.consumeCompleteActions()
        });

        this.hubConnection.on('command-error', (event: IEvent) => {
            this.executedCommandErrorActions.push(event)
            this.consumeErrorActions()
        });
    }

    private consumeCompleteActions = () => {
        this.executedCommandCompletedActions.forEach((event: IEvent) => {
            const action = this.actions[event.id];
            if (action) {
                delete this.actions[event.id];
                this.executedCommandCompletedActions = this.executedCommandCompletedActions.filter(e => e !== event)
                action(event.object);
            }
        })
    }

    private consumeErrorActions = () => {
        this.executedCommandErrorActions.forEach(event => {
            const action = this.errorActions[event.id]
            if (action) {
                delete this.errorActions[event.id]
                this.executedCommandErrorActions = this.executedCommandErrorActions.filter(e => e !== event)
                action()
            }
        })
    }

    public waitForEventResult(eventId: string, action: Function) {
        this.actions[eventId] = action;
    }

    public waitForEventErrorResult(eventId: string, action: Function) {
        this.errorActions[eventId] = action;
        this.consumeErrorActions();
    }
}

const instance = async () => await new HubConnector().init();

export default instance();