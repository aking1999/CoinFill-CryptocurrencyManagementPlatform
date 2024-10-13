function copyToClipboard() {
    const input = document.getElementById('address')

    if (isOS()) {
        let range = document.createRange()
        range.selectNodeContents(input)
        let selection = window.getSelection()
        selection.removeAllRanges()
        selection.addRange(range);
        input.setSelectionRange(0, 999999);
    } else {
        input.select()
    }

    document.execCommand("copy")
    toastr['success']('', 'Copied to clipboard successfully.')
    input.blur()
}

function isOS() {
    return navigator.userAgent.match(/ipad|iphone/i)
}