(function () {
    window.BootstrapManager = {
        closeModal: (modalId) => {
            $(modalId).modal('hide');
        }
    }
})();