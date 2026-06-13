const API_BASE = 'http://localhost:5000/api';

async function predict(studentData, studentName = 'Студент') {
    const res = await fetch(`${API_BASE}/predict?studentName=${encodeURIComponent(studentName)}`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(studentData)
    });
    if (!res.ok) {
        const err = await res.json();
        throw new Error(err.error || 'Ошибка сервера');
    }
    return res.json();
}

async function getHistory(limit = 50) {
    const res = await fetch(`${API_BASE}/history?limit=${limit}`);
    return res.json();
}

async function getStats() {
    const res = await fetch(`${API_BASE}/history/stats`);
    return res.json();
}

async function deleteHistory(id) {
    await fetch(`${API_BASE}/history/${id}`, { method: 'DELETE' });
}

async function checkHealth() {
    try {
        const res = await fetch(`${API_BASE}/predict/health`);
        return res.json();
    } catch {
        return { dotnet_api: 'error', ai_service: 'error' };
    }
}

// Цвета и иконки для классов
const CLASS_CONFIG = {
    'Pass':        { color: 'success', icon: 'bi-check-circle-fill',  label: 'Сдал',        bg: '#198754' },
    'Distinction': { color: 'primary', icon: 'bi-star-fill',          label: 'Отличник',    bg: '#0d6efd' },
    'Fail':        { color: 'danger',  icon: 'bi-x-circle-fill',      label: 'Не сдал',     bg: '#dc3545' },
    'Withdrawn':   { color: 'warning', icon: 'bi-exclamation-triangle-fill', label: 'Отчислен', bg: '#ffc107' },
};

const STYLE_CONFIG = {
    'reading':     { icon: 'bi-book',           label: 'Читатель',     color: 'purple' },
    'social':      { icon: 'bi-people',         label: 'Социальный',   color: 'info'   },
    'interactive': { icon: 'bi-controller',     label: 'Практик',      color: 'success'},
    'research':    { icon: 'bi-search',         label: 'Исследователь',color: 'warning'},
    'visual':      { icon: 'bi-eye',            label: 'Визуал',       color: 'danger' },
};
