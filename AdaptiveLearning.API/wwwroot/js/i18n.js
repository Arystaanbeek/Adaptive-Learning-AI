// Локализация: ru, en, kz
const TRANSLATIONS = {
  ru: {
    // Navbar
    'nav.courses':    'Курсы',
    'nav.dashboard':  'Дашборд',
    'nav.admin':      'Панель админа',
    'nav.login':      'Войти',
    'nav.register':   'Регистрация',
    'nav.logout':     'Выйти',
    // Главная
    'hero.title':     'Платформа которая учится вместе с тобой',
    'hero.subtitle':  'Система определяет твой стиль восприятия и подбирает материал',
    'hero.start':     'Начать бесплатно',
    'hero.catalog':   'Смотреть курсы',
    // Каталог
    'courses.title':  'Каталог курсов',
    'courses.welcome':'Привет',
    'courses.available':'Доступно курсов',
    'courses.enroll': 'Записаться',
    'courses.continue':'Продолжить',
    'courses.empty':  'Курсов не найдено',
    'courses.modules':'модулей',
    // Фильтры
    'filter.title':   'Фильтры',
    'filter.search':  'Поиск по курсам...',
    'filter.category':'Категория',
    'filter.level':   'Уровень',
    'filter.type':    'Тип контента',
    'filter.all':     'Все',
    'filter.reset':   'Сбросить фильтры',
    'filter.allcat':  'Все категории',
    'filter.alllvl':  'Все уровни',
    // Уровни
    'level.Beginner':    'Начальный',
    'level.Intermediate':'Средний',
    'level.Advanced':    'Продвинутый',
    // Дашборд
    'dash.title':     'Мой дашборд',
    'dash.courses':   'Курсов записано',
    'dash.completed': 'Завершено',
    'dash.avgscore':  'Средний балл',
    'dash.tests':     'Тестов сдано',
    'dash.mycourses': 'Мои курсы',
    'dash.scores':    'Динамика баллов',
    'dash.weak':      'Темы для повторения',
    'dash.style':     'Стиль восприятия',
    'dash.prediction':'Прогноз AI модели',
    'dash.recs':      'Рекомендации',
    'dash.progress':  'Прогресс',
    'dash.continue':  'Продолжить →',
    // Авторизация
    'auth.login':     'Вход в аккаунт',
    'auth.register':  'Создать аккаунт',
    'auth.email':     'Email',
    'auth.password':  'Пароль',
    'auth.name':      'Имя',
    'auth.submit_login':   'Войти',
    'auth.submit_register':'Зарегистрироваться',
    'auth.no_account':'Нет аккаунта?',
    'auth.has_account':'Уже есть аккаунт?',
    // Курс
    'course.ai_helper':   'AI помощник',
    'course.ai_greeting': 'Привет! Я твой AI помощник. Задавай вопросы по теме курса — объясню понятно.',
    'course.ask':         'Задай вопрос...',
    'course.simpler':     'Проще',
    'course.example':     'Пример',
    'course.practice':    'Практика',
    'course.progress':    'Прогресс',
    'course.modules':     'Модули курса',
    'course.complete':    'Отметить как просмотрено',
    'course.submit_quiz': 'Сдать тест',
  },

  en: {
    'nav.courses':    'Courses',
    'nav.dashboard':  'Dashboard',
    'nav.admin':      'Admin Panel',
    'nav.login':      'Sign In',
    'nav.register':   'Register',
    'nav.logout':     'Sign Out',
    'hero.title':     'Platform that learns with you',
    'hero.subtitle':  'System identifies your learning style and adapts content',
    'hero.start':     'Start for free',
    'hero.catalog':   'Browse courses',
    'courses.title':  'Course Catalog',
    'courses.welcome':'Hello',
    'courses.available':'Available courses',
    'courses.enroll': 'Enroll',
    'courses.continue':'Continue',
    'courses.empty':  'No courses found',
    'courses.modules':'modules',
    'filter.title':   'Filters',
    'filter.search':  'Search courses...',
    'filter.category':'Category',
    'filter.level':   'Level',
    'filter.type':    'Content type',
    'filter.all':     'All',
    'filter.reset':   'Reset filters',
    'filter.allcat':  'All categories',
    'filter.alllvl':  'All levels',
    'level.Beginner':    'Beginner',
    'level.Intermediate':'Intermediate',
    'level.Advanced':    'Advanced',
    'dash.title':     'My Dashboard',
    'dash.courses':   'Enrolled courses',
    'dash.completed': 'Completed',
    'dash.avgscore':  'Average score',
    'dash.tests':     'Tests taken',
    'dash.mycourses': 'My Courses',
    'dash.scores':    'Score dynamics',
    'dash.weak':      'Topics to review',
    'dash.style':     'Learning style',
    'dash.prediction':'AI Prediction',
    'dash.recs':      'Recommendations',
    'dash.progress':  'Progress',
    'dash.continue':  'Continue →',
    'auth.login':     'Sign In',
    'auth.register':  'Create Account',
    'auth.email':     'Email',
    'auth.password':  'Password',
    'auth.name':      'Name',
    'auth.submit_login':   'Sign In',
    'auth.submit_register':'Register',
    'auth.no_account': 'No account?',
    'auth.has_account':'Already have an account?',
    'course.ai_helper':   'AI Assistant',
    'course.ai_greeting': 'Hello! I am your AI assistant. Ask me anything about the course.',
    'course.ask':         'Ask a question...',
    'course.simpler':     'Simpler',
    'course.example':     'Example',
    'course.practice':    'Practice',
    'course.progress':    'Progress',
    'course.modules':     'Course modules',
    'course.complete':    'Mark as viewed',
    'course.submit_quiz': 'Submit quiz',
  },

  kz: {
    'nav.courses':    'Курстар',
    'nav.dashboard':  'Бақылау тақтасы',
    'nav.admin':      'Әкімші панелі',
    'nav.login':      'Кіру',
    'nav.register':   'Тіркелу',
    'nav.logout':     'Шығу',
    'hero.title':     'Сізбен бірге оқитын платформа',
    'hero.subtitle':  'Жүйе сіздің қабылдау стиліңізді анықтайды',
    'hero.start':     'Тегін бастау',
    'hero.catalog':   'Курстарды қарау',
    'courses.title':  'Курстар каталогы',
    'courses.welcome':'Сәлем',
    'courses.available':'Қолжетімді курстар',
    'courses.enroll': 'Жазылу',
    'courses.continue':'Жалғастыру',
    'courses.empty':  'Курстар табылмады',
    'courses.modules':'модуль',
    'filter.title':   'Сүзгілер',
    'filter.search':  'Курстарды іздеу...',
    'filter.category':'Санат',
    'filter.level':   'Деңгей',
    'filter.type':    'Мазмұн түрі',
    'filter.all':     'Барлығы',
    'filter.reset':   'Сүзгілерді тазалау',
    'filter.allcat':  'Барлық санаттар',
    'filter.alllvl':  'Барлық деңгейлер',
    'level.Beginner':    'Бастапқы',
    'level.Intermediate':'Орта',
    'level.Advanced':    'Жоғары',
    'dash.title':     'Менің тақтам',
    'dash.courses':   'Жазылған курстар',
    'dash.completed': 'Аяқталған',
    'dash.avgscore':  'Орташа балл',
    'dash.tests':     'Тапсырылған тесттер',
    'dash.mycourses': 'Менің курстарым',
    'dash.scores':    'Балл динамикасы',
    'dash.weak':      'Қайталау тақырыптары',
    'dash.style':     'Қабылдау стилі',
    'dash.prediction':'AI болжамы',
    'dash.recs':      'Ұсыныстар',
    'dash.progress':  'Прогресс',
    'dash.continue':  'Жалғастыру →',
    'auth.login':     'Аккаунтқа кіру',
    'auth.register':  'Аккаунт жасау',
    'auth.email':     'Email',
    'auth.password':  'Құпия сөз',
    'auth.name':      'Аты',
    'auth.submit_login':   'Кіру',
    'auth.submit_register':'Тіркелу',
    'auth.no_account': 'Аккаунт жоқ па?',
    'auth.has_account':'Аккаунт бар ма?',
    'course.ai_helper':   'AI көмекші',
    'course.ai_greeting': 'Сәлем! Мен сіздің AI көмекшіңізмін. Курс тақырыбы бойынша сұрақтар қойыңыз.',
    'course.ask':         'Сұрақ қою...',
    'course.simpler':     'Қарапайымырақ',
    'course.example':     'Мысал',
    'course.practice':    'Тәжірибе',
    'course.progress':    'Прогресс',
    'course.modules':     'Курс модульдері',
    'course.complete':    'Қаралды деп белгілеу',
    'course.submit_quiz': 'Тестті тапсыру',
  }
};

// Получить текущий язык
function getLang() {
  return localStorage.getItem('lang') || 'ru';
}

// Установить язык
function setLang(lang) {
  localStorage.setItem('lang', lang);
  applyTranslations();
  updateLangButtons();
}

// Перевести ключ
function t(key) {
  const lang = getLang();
  return TRANSLATIONS[lang]?.[key] || TRANSLATIONS['ru']?.[key] || key;
}

// Применить переводы ко всем элементам с data-i18n
function applyTranslations() {
  document.querySelectorAll('[data-i18n]').forEach(el => {
    const key = el.getAttribute('data-i18n');
    const val = t(key);
    if (el.tagName === 'INPUT' || el.tagName === 'TEXTAREA') {
      el.placeholder = val;
    } else {
      el.textContent = val;
    }
  });
}

// Обновить кнопки языка
function updateLangButtons() {
  const lang = getLang();
  document.querySelectorAll('.lang-btn').forEach(btn => {
    btn.classList.toggle('active', btn.dataset.lang === lang);
    btn.classList.toggle('btn-primary', btn.dataset.lang === lang);
    btn.classList.toggle('btn-outline-secondary', btn.dataset.lang !== lang);
  });
}

// Виджет переключения языка
function renderLangSwitcher(containerId) {
  const el = document.getElementById(containerId);
  if (!el) return;
  el.innerHTML = `
    <div class="btn-group btn-group-sm" role="group">
      <button class="btn lang-btn" data-lang="ru" onclick="setLang('ru')">RU</button>
      <button class="btn lang-btn" data-lang="en" onclick="setLang('en')">EN</button>
      <button class="btn lang-btn" data-lang="kz" onclick="setLang('kz')">KZ</button>
    </div>`;
  updateLangButtons();
}

// Автоматически применять при загрузке
document.addEventListener('DOMContentLoaded', () => {
  applyTranslations();
  updateLangButtons();
});
