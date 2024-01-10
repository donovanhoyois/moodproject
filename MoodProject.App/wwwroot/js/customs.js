﻿(function () {
    window.BootstrapManager = {
        closeModal: (modalId) => {
            $(modalId).modal('hide');
        },
        openModal: (modalId) => {
            console.log(modalId)
            $(modalId).modal('show');
        }
    }
    window.FileManager = {
        download: (base64Content, fileName) => {
            const byteCharacters = atob(base64Content);
            const byteNumbers = new Array(byteCharacters.length);

            for (let i = 0; i < byteCharacters.length; i++) {
                byteNumbers[i] = byteCharacters.charCodeAt(i);
            }

            const byteArray = new Uint8Array(byteNumbers);
            const blob = new Blob([byteArray], { type: "application/octet-stream" });

            const link = document.createElement("a");
            link.href = URL.createObjectURL(blob);
            link.download = fileName;
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        }
    }
})();