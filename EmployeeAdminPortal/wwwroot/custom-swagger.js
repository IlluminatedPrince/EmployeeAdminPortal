console.log("✅ custom-swagger.js loaded!");

(function () {
    // Patch fetch to ensure "Bearer " is always prefixed
    const originalFetch = window.fetch;
    window.fetch = function (input, init) {
        if (init && init.headers && init.headers.Authorization) {
            const auth = init.headers.Authorization;
            if (!auth.toLowerCase().startsWith("bearer ")) {
                init.headers.Authorization = "Bearer " + auth;
                console.log("✅ Bearer token updated:", init.headers.Authorization);
            }
        }
        return originalFetch(input, init);
    };

    // Watch for Swagger UI authorize modal interaction
    window.addEventListener('load', () => {
        const observer = new MutationObserver(() => {
            const modalButton = document.querySelector('.modal-btn.auth');
            if (modalButton) {
                modalButton.addEventListener('click', () => {
                    setTimeout(() => {
                        const bearerInput = document.querySelector('input[placeholder="Bearer <JWT>"]');
                        if (bearerInput && bearerInput.value.trim()) {
                            alert("🔐 You are an authorized user!");
                        } else {
                            console.log("🚫 No token found.");
                        }
                    }, 500);
                });
            }
        });

        observer.observe(document.body, {
            childList: true,
            subtree: true
        });
    });
})();
