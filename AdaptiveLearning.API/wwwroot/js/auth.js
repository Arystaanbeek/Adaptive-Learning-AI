const API = 'http://localhost:5000/api';

function saveAuth(data) {
    localStorage.setItem('token',  data.token);
    localStorage.setItem('name',   data.name);
    localStorage.setItem('email',  data.email);
    localStorage.setItem('role',   data.role);
    localStorage.setItem('userId', data.userId);
}
function getToken() { return localStorage.getItem('token'); }
function getRole()  { return localStorage.getItem('role'); }
function getName()  { return localStorage.getItem('name'); }
function getUserId(){ return localStorage.getItem('userId'); }

function requireAuth() {
    if (!getToken()) { window.location.href = '/login.html'; return false; }
    return true;
}
function requireAdmin() {
    if (!requireAuth()) return false;
    if (getRole() !== 'Admin') { window.location.href = '/index.html'; return false; }
    return true;
}
function logout() { localStorage.clear(); window.location.href = '/login.html'; }

async function authFetch(url, options = {}) {
    const token = getToken();
    options.headers = { ...options.headers, 'Content-Type': 'application/json' };
    if (token) options.headers['Authorization'] = 'Bearer ' + token;
    const res = await fetch(API + url, options);
    if (res.status === 401) { logout(); return null; }
    return res;
}

function updateNavbar() {
    const name  = getName();
    const role  = getRole();
    const token = getToken();
    const navUser  = document.getElementById('nav-user');
    const navAdmin = document.getElementById('nav-admin');
    if (!token) {
        if (navUser)  navUser.innerHTML  = `<a class="nav-link" href="/login.html">Войти</a><a class="nav-link" href="/register.html">Регистрация</a>`;
        if (navAdmin) navAdmin.style.display = 'none';
        return;
    }
    if (navUser) navUser.innerHTML = `<span class="navbar-text me-3 text-light"><i class="bi bi-person-circle me-1"></i>${name}</span><button class="btn btn-outline-light btn-sm" onclick="logout()">Выйти</button>`;
    if (navAdmin) navAdmin.style.display = role === 'Admin' ? 'flex' : 'none';
}
