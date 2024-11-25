
qrCodePopupAction = (row) => {
    var html =
        `<span data-bs-toggle1="modal" data-bs-target1="#sort_modal">
                    <a href="javascript:;" onclick="javascript:doDisplayQrCode('${row.qrCodeImageUrl}')"
                        class="mb-1 mt-1 me-1 btn btn-sm btn-success" data-bs-toggle="tooltip" data-bs-placement="bottom"
                        title="View QR Code" data-bs-original-title="View QR Code" aria-label="View QR Code">
                        <i class="fas fa-qrcode"></i>
                    </a>
                </span>`;
    return html;
}

doDisplayQrCode = (qrImageUrl) => {
    $('#qrcode-view').modal('toggle');
    $('.qr-code-preview').attr('src', qrImageUrl);
}

$(function () {
    //$(".download-qrcode-btn").click(function () {
    //    var ImageSrc = $(".qr-code-preview").attr("src");
    //    downloadImage(ImageSrc);
    //});
    $("#qrcode-view").on('shown.bs.modal', function (e) {
        $(".download-qrcode-btn").attr("href", $(".qr-code-preview").attr("src"));
    });
    $(".print-qr-code-btn").click(function () {
        var ImageSrc = $(".qr-code-preview").attr("src");
        event.preventDefault();
        var myWindow = window.open('', 'popupWindow', 'width=500,height=500');
        myWindow.document.write("<img src='" + ImageSrc + "' style='max-width:100%;height:auto;'>");
        myWindow.document.write("<script>document.querySelector('img').addEventListener('load',function(){window.print();window.close()})</" + "script>");
        myWindow.document.close();
        myWindow.focus();
    });
})

function downloadImage(url) {
    fetch(url, {
        mode: 'no-cors',
    })
    .then(response => response.blob())
    .then(blob => {
        let blobUrl = window.URL.createObjectURL(blob);
        let a = document.createElement('a');
        a.download = url.replace(/^.*[\\\/]/, '');
        a.href = blobUrl;
        document.body.appendChild(a);
        a.click();
        a.remove();
    })
}
