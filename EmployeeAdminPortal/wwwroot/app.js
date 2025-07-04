const API_BASE = "https://localhost:7017";  // Use the exact base URL from Swagger

async function login() {
    const username = document.getElementById("username").value;
    const password = document.getElementById("password").value;

    const res = await fetch(`${API_BASE}/api/Auth/login`, { // Capital A in Auth
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ username, password })
    });

    if (res.ok) {
        const data = await res.json();
        jwtToken = data.token;
        localStorage.setItem("jwtToken", jwtToken);
        document.getElementById("loginStatus").innerText = "✅ Login successful.";
    } else {
        const error = await res.text();
        document.getElementById("loginStatus").innerText = "❌ Login failed: " + error;
    }
}

async function getSecureData() {
    if (!jwtToken) {
        alert("Please login or provide a JWT token.");
        return;
    }

    const res = await fetch(`${API_BASE}/api/Auth/securedata`, {
        method: "GET",
        headers: {
            Authorization: "Bearer " + jwtToken
        }
    });

    if (res.status === 401) {
        document.getElementById("secureData").innerText = "❌ Unauthorized.";
    } else {
        const data = await res.text();
        document.getElementById("secureData").innerText = data;
    }
}
