import apiConnector, { IResponse } from './ApiConnector'
import hubConnector from './HubConnector'
import toast from '../../components/Controls/Toast'

class CqrsApiConnector {
    private handleCqrsResult = async (result: IResponse) => {
        if (result.status === 202) {
            try {
                return await hubConnector.handleCommandResult(result)
            } catch (err) {
                toast.showError(err.errorMessage)
                throw err
            }
        }

        return result.response
    }

    public get = async (methodName: string, params?: { [id: string] : any }) => {
        const result = await apiConnector.get(methodName, params)
        return this.handleCqrsResult(result)
    }

    public getFile = (methodName: string, params: { [id: string] : string }) => {
        return apiConnector.getFile(methodName, params)
    }

    public put = async (methodName: string) => {
        const result = await apiConnector.put(methodName)
        return this.handleCqrsResult(result)
    }

    public post = async (methodName: string, body?: any, headers?: { [id: string] : string }) => {
        const result = await apiConnector.post(methodName, body, headers)
        return this.handleCqrsResult(result)
    }

    public delete = async (methodName: string, body?: any) => {
        const result = await apiConnector.post(methodName, body)
        return this.handleCqrsResult(result)
    }

    public uploadFile =  (methodName: string, file: any, headers: { [id: string] : string }) => {
        return apiConnector.uploadFile(methodName, file, headers)
    }
}

export default new CqrsApiConnector()