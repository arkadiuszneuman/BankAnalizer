import userManager from '../api/UserManager'

export interface IResponse {
    ok: boolean,
    status: number,
    response: any
}

class ApiConnector {
    private apiAddress = "https://localhost:5001/api/"

    private executeMethod =  async (methodName: string, methodType: string, body?: any, headers?: any ) => {
        let finalHeaders = {
            'Content-Type': 'application/json',
            ...headers
        }

        finalHeaders = this.addAuthorizationHeaderToHeaders(finalHeaders);

        const result = await fetch(this.apiAddress + methodName, { 
            method: methodType, 
            headers: finalHeaders,
            body: JSON.stringify(body)
        })

        if (result.status === 401) {
            window.location.href = '/login'
        }

        return result
    }

    private addAuthorizationHeaderToHeaders = (headers: { [id: string] : string }) => {
        const user = userManager.getUserFromStorage()
        if (user == null) {
            return headers
        }

        return {
            'Authorization': 'Bearer ' + user.token,
            ...headers
        }
    }
    
    private executeMethodAndParseResult = async (methodName: string, methodType: string, body?: any, headers?: { [id: string] : string }) : Promise<IResponse> => {
        const result = await this.executeMethod(methodName, methodType, body, headers)

        if (!result.ok)
            throw result;

        var bodyAsText = await result.text()

        return {
            ok: result.ok,
            status: result.status,
            response: bodyAsText === '' ? null : JSON.parse(bodyAsText)
        }
    }

    private prepareGetQuery = (params?: { [id: string] : any }) => {
        let query = '?';
        let isFirstQuery = true;

        for (var param in params) {
            let paramValue = params[param]

            if (paramValue instanceof Array) {
                if (paramValue.length > 0) {
                    if (query.length !== 0) {
                        query += '&'
                    }

                    query += paramValue.map(p => param + '=' + p).join('&')
                }
                continue
            }

            if (paramValue instanceof Date) {
                paramValue = params[param].toISOString()
            }

            if (!paramValue)
                continue;

            if (isFirstQuery) {
                isFirstQuery = false
            } else {
                query += '&'
            }
            query += param + "=" + paramValue
        }

        if (query === '?')
            query = ''

        return query
    }

    public get = async (methodName: string, params?: { [id: string] : string }) => {
        let query = this.prepareGetQuery(params)
        return await this.executeMethodAndParseResult(methodName + query, 'get')
    }

    public getFile = async (methodName: string, params: { [id: string] : string }) => {
        let query = this.prepareGetQuery(params)
        let response = await this.executeMethod(methodName + query, 'get')
        return response.blob()
    }

    public put = async (methodName: string) => {
        return await this.executeMethodAndParseResult(methodName, 'put')
    }

    public post = async (methodName: string, body?: any, headers?: { [id: string] : string }) => {
        return await this.executeMethodAndParseResult(methodName, 'post', body, headers)
    }

    public delete = async (methodName: string, body: any) => {
        return await this.executeMethodAndParseResult(methodName, 'delete', body)
    }

    public uploadFile =  async (methodName: string, file: any, headers: { [id: string] : string }) => {
        const finalHeaders = {
            ...headers
        }

        var result = await fetch(this.apiAddress + methodName, { 
            method: 'post', 
            headers: this.addAuthorizationHeaderToHeaders(finalHeaders),
            body: file
        })

        if (result.status === 401) {
            window.location.href = '/login'
        }

        var bodyAsText = await result.text()
        if (bodyAsText === '')
            return null

        return JSON.parse(bodyAsText)
    }
}

export default new ApiConnector()