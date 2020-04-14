declare const $: any

class Toast {
    private show(type: string, message: string, title?: string) {
        $('body')
            .toast({
                title: title,
                class: type,
                message: message
            })
    }

    public showError(message: string) {
        this.show('error', message, 'Error occured!')
    }
}

export default new Toast()