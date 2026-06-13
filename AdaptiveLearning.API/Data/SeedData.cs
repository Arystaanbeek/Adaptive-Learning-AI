using AdaptiveLearning.API.Models;

namespace AdaptiveLearning.API.Data;

// ─────────────────────────────────────────────────────────────────────────────
//  Каждый курс помечен тегом ContentFocus, который отражает доминирующий тип
//  контента.  Это поле используется RecommendController для сопоставления
//  слабых ContentType студента с нужным курсом.
//
//  Стили и их ContentType:
//    visual      → video   (видеокурсы с визуальным объяснением)
//    reading     → text    (глубокая текстовая теория)
//    interactive → quiz    (практика, задачи, квизы)
//    social      → text+video (бизнес/коммуникации — смешанный формат)
//    research    → text    (наука, данные, аналитика)
// ─────────────────────────────────────────────────────────────────────────────

public static class SeedData
{
    public static void Initialize(AppDbContext db)
    {
        if (db.Courses.Any()) return;

        // ══════════════════════════════════════════════════════════════════════
        //  VISUAL — 3 курса  (ContentType: video-heavy)
        // ══════════════════════════════════════════════════════════════════════
        var c_vis1 = new Course
        {
            Title = "Видеокурс: Основы Python",
            Description = "Весь Python через видео — от переменных до функций. Идеально для визуалов.",
            Category = "Программирование",
            Level = "Beginner",
            IsPublished = true,
            CreatedBy = 1,
            Modules = new List<Module> {
                new Module { Title="Что такое Python?", ContentType="video", OrderIndex=1, DurationMinutes=10,
                    ContentUrl="https://www.youtube.com/watch?v=kqtD5dpn9C8", Description="Введение в Python через видео." },
                new Module { Title="Переменные и типы — видео", ContentType="video", OrderIndex=2, DurationMinutes=14,
                    ContentUrl="https://www.youtube.com/watch?v=rfscVS0vtbw", Description="Типы данных визуально." },
                new Module { Title="Функции и циклы — видео", ContentType="video", OrderIndex=3, DurationMinutes=16,
                    ContentUrl="https://www.youtube.com/watch?v=DZwmZ8Usvnk", Description="Циклы и функции через скринкаст." },
                new Module { Title="Финальный тест", ContentType="quiz", OrderIndex=4, DurationMinutes=8, IsQuiz=true,
                    Questions=new List<QuizQuestion> {
                        new QuizQuestion { Question="Тип данных у 3.14?", OptionA="int", OptionB="float", OptionC="str", OptionD="bool", CorrectAnswer="B", OrderIndex=1 },
                        new QuizQuestion { Question="Вывод числа в Python?", OptionA="echo(x)", OptionB="console.log(x)", OptionC="print(x)", OptionD="printf(x)", CorrectAnswer="C", OrderIndex=2 },
                        new QuizQuestion { Question="Что выведет print(2**3)?", OptionA="6", OptionB="8", OptionC="5", OptionD="9", CorrectAnswer="B", OrderIndex=3 },
                    }
                },
            }
        };

        var c_vis2 = new Course
        {
            Title = "Видеокурс: HTML и CSS с нуля",
            Description = "Верстка через видеоуроки — Flexbox, Grid, анимации. Только смотри и повторяй.",
            Category = "Программирование",
            Level = "Beginner",
            IsPublished = true,
            CreatedBy = 1,
            Modules = new List<Module> {
                new Module { Title="HTML структура — видео", ContentType="video", OrderIndex=1, DurationMinutes=13,
                    ContentUrl="https://www.youtube.com/watch?v=qz0aGYrrlhU", Description="Основы HTML через видео." },
                new Module { Title="CSS Flexbox — видео", ContentType="video", OrderIndex=2, DurationMinutes=18,
                    ContentUrl="https://www.youtube.com/watch?v=fYq5PXgSsbE", Description="Flexbox визуально." },
                new Module { Title="CSS Grid — видео", ContentType="video", OrderIndex=3, DurationMinutes=15,
                    ContentUrl="https://www.youtube.com/watch?v=jV8B24rSN5o", Description="Grid Layout через скринкаст." },
                new Module { Title="Тест по вёрстке", ContentType="quiz", OrderIndex=4, DurationMinutes=8, IsQuiz=true,
                    Questions=new List<QuizQuestion> {
                        new QuizQuestion { Question="Тег заголовка 1-го уровня?", OptionA="<header>", OptionB="<h1>", OptionC="<title>", OptionD="<head>", CorrectAnswer="B", OrderIndex=1 },
                        new QuizQuestion { Question="CSS-свойство для цвета текста?", OptionA="text-color", OptionB="font-color", OptionC="color", OptionD="foreground", CorrectAnswer="C", OrderIndex=2 },
                        new QuizQuestion { Question="Flexbox: выравнивание по центру?", OptionA="align:center", OptionB="justify-content:center", OptionC="text-align:center", OptionD="flex-center:true", CorrectAnswer="B", OrderIndex=3 },
                    }
                },
            }
        };

        var c_vis3 = new Course
        {
            Title = "Видеокурс: Дизайн и Figma",
            Description = "UI/UX дизайн с нуля через видеоуроки. Figma, прототипирование, компоненты.",
            Category = "Дизайн",
            Level = "Beginner",
            IsPublished = true,
            CreatedBy = 1,
            Modules = new List<Module> {
                new Module { Title="Интерфейс Figma — обзор", ContentType="video", OrderIndex=1, DurationMinutes=12,
                    ContentUrl="https://www.youtube.com/watch?v=FTFaQWZBqQ8", Description="Знакомство с Figma." },
                new Module { Title="Фреймы и компоненты", ContentType="video", OrderIndex=2, DurationMinutes=16,
                    ContentUrl="https://www.youtube.com/watch?v=YZMy4ZTnXWk", Description="Компоненты и автолейаут." },
                new Module { Title="Прототипирование — видео", ContentType="video", OrderIndex=3, DurationMinutes=14,
                    ContentUrl="https://www.youtube.com/watch?v=lTIeZ2ahEkQ", Description="Создание прототипов." },
                new Module { Title="Тест по Figma", ContentType="quiz", OrderIndex=4, DurationMinutes=8, IsQuiz=true,
                    Questions=new List<QuizQuestion> {
                        new QuizQuestion { Question="Figma — это инструмент для?", OptionA="Программирования", OptionB="UI/UX дизайна", OptionC="Монтажа видео", OptionD="3D-моделирования", CorrectAnswer="B", OrderIndex=1 },
                        new QuizQuestion { Question="Компонент в Figma позволяет?", OptionA="Писать код", OptionB="Переиспользовать элементы", OptionC="Анимировать видео", OptionD="Хранить данные", CorrectAnswer="B", OrderIndex=2 },
                    }
                },
            }
        };

        // ══════════════════════════════════════════════════════════════════════
        //  READING — 3 курса  (ContentType: text-heavy)
        // ══════════════════════════════════════════════════════════════════════
        var c_read1 = new Course
        {
            Title = "Алгоритмы и структуры данных",
            Description = "Глубокое текстовое погружение в алгоритмы: сортировки, графы, динамическое программирование.",
            Category = "Программирование",
            Level = "Intermediate",
            IsPublished = true,
            CreatedBy = 1,
            Modules = new List<Module> {
                new Module { Title="Сложность алгоритмов — теория", ContentType="text", OrderIndex=1, DurationMinutes=25,
                    ContentText="<h4>Big-O нотация</h4><p>Описывает скорость роста времени выполнения алгоритма в зависимости от размера входных данных.</p><ul><li>O(1) — константное время</li><li>O(n) — линейное</li><li>O(n²) — квадратичное</li></ul><p>Пример: поиск в несортированном массиве — O(n), бинарный поиск — O(log n).</p>",
                    Description="Теория алгоритмической сложности." },
                new Module { Title="Сортировки — теория и анализ", ContentType="text", OrderIndex=2, DurationMinutes=30,
                    ContentText="<h4>Сортировка пузырьком</h4><p>Проходим по массиву, меняем соседние элементы если нужно. O(n²).</p><h4>Быстрая сортировка</h4><p>Выбираем пивот, делим массив. Средний случай O(n log n).</p><pre>def quicksort(arr):\n    if len(arr) <= 1: return arr\n    pivot = arr[len(arr)//2]\n    left  = [x for x in arr if x < pivot]\n    mid   = [x for x in arr if x == pivot]\n    right = [x for x in arr if x > pivot]\n    return quicksort(left) + mid + quicksort(right)</pre>",
                    Description="Сравнение алгоритмов сортировки." },
                new Module { Title="Графы и деревья", ContentType="text", OrderIndex=3, DurationMinutes=28,
                    ContentText="<h4>Граф</h4><p>G = (V, E) — вершины и рёбра. Бывают ориентированные и нет.</p><h4>Обходы</h4><ul><li>BFS — обход в ширину, очередь</li><li>DFS — обход в глубину, стек/рекурсия</li></ul><h4>Дерево</h4><p>Связный граф без циклов. BST: левый потомок < родитель < правый.</p>",
                    Description="Теория графов и деревьев." },
                new Module { Title="Тест по алгоритмам", ContentType="quiz", OrderIndex=4, DurationMinutes=10, IsQuiz=true,
                    Questions=new List<QuizQuestion> {
                        new QuizQuestion { Question="Сложность бинарного поиска?", OptionA="O(n)", OptionB="O(n²)", OptionC="O(log n)", OptionD="O(1)", CorrectAnswer="C", OrderIndex=1 },
                        new QuizQuestion { Question="BFS использует структуру:", OptionA="Стек", OptionB="Очередь", OptionC="Граф", OptionD="Дерево", CorrectAnswer="B", OrderIndex=2 },
                        new QuizQuestion { Question="Quicksort в среднем:", OptionA="O(n)", OptionB="O(n log n)", OptionC="O(n²)", OptionD="O(log n)", CorrectAnswer="B", OrderIndex=3 },
                    }
                },
            }
        };

        var c_read2 = new Course
        {
            Title = "Английский язык: продвинутая грамматика",
            Description = "Детальный разбор грамматических конструкций — тексты, схемы, объяснения.",
            Category = "Языки",
            Level = "Intermediate",
            IsPublished = true,
            CreatedBy = 1,
            Modules = new List<Module> {
                new Module { Title="Perfect Tenses — полный разбор", ContentType="text", OrderIndex=1, DurationMinutes=22,
                    ContentText="<h4>Present Perfect</h4><p>Связь прошлого с настоящим: <em>I have finished my work.</em></p><ul><li>already, just, yet, ever, never</li><li>Signal words: since, for</li></ul><h4>Past Perfect</h4><p>Действие до другого прошедшего: <em>She had left before he arrived.</em></p>",
                    Description="Perfect Tenses в деталях." },
                new Module { Title="Conditionals: 0, 1, 2, 3", ContentType="text", OrderIndex=2, DurationMinutes=25,
                    ContentText="<h4>Zero Conditional</h4><p>Факты: If + Present Simple, Present Simple. <em>If water reaches 100°C, it boils.</em></p><h4>First Conditional</h4><p>Реально: If + Present Simple, will + V. <em>If it rains, I will stay home.</em></p><h4>Second Conditional</h4><p>Нереально: If + Past Simple, would + V. <em>If I were rich, I would travel.</em></p><h4>Third Conditional</h4><p>Прошлое: If + Past Perfect, would have + V3.</p>",
                    Description="Условные предложения." },
                new Module { Title="Артикли и предлоги — систематика", ContentType="text", OrderIndex=3, DurationMinutes=20,
                    ContentText="<h4>Артикли</h4><p><b>a/an</b> — первое упоминание, счётные существительные.<br><b>the</b> — конкретный объект, известный обоим.<br><b>∅</b> — несчётные, множественное число в общем смысле.</p><h4>Предлоги времени</h4><ul><li>at — точное время (at 5pm)</li><li>on — день (on Monday)</li><li>in — период (in June, in 2024)</li></ul>",
                    Description="Артикли и предлоги систематически." },
                new Module { Title="Грамматический тест", ContentType="quiz", OrderIndex=4, DurationMinutes=10, IsQuiz=true,
                    Questions=new List<QuizQuestion> {
                        new QuizQuestion { Question="'She ___ already ___ the book.'", OptionA="has / read", OptionB="is / reading", OptionC="did / read", OptionD="have / read", CorrectAnswer="A", OrderIndex=1 },
                        new QuizQuestion { Question="Second Conditional: If I were you, I ___", OptionA="will go", OptionB="would go", OptionC="go", OptionD="would have gone", CorrectAnswer="B", OrderIndex=2 },
                        new QuizQuestion { Question="'___ sun rises in the east.'", OptionA="A", OptionB="An", OptionC="The", OptionD="—", CorrectAnswer="C", OrderIndex=3 },
                    }
                },
            }
        };

        var c_read3 = new Course
        {
            Title = "Основы экономики: теория и анализ",
            Description = "Микро- и макроэкономика через тексты и аналитические материалы.",
            Category = "Наука",
            Level = "Intermediate",
            IsPublished = true,
            CreatedBy = 1,
            Modules = new List<Module> {
                new Module { Title="Спрос и предложение", ContentType="text", OrderIndex=1, DurationMinutes=22,
                    ContentText="<h4>Закон спроса</h4><p>При росте цены объём спроса падает (при прочих равных). Кривая спроса — нисходящая.</p><h4>Закон предложения</h4><p>При росте цены объём предложения растёт. Кривая предложения — восходящая.</p><h4>Равновесие</h4><p>Точка пересечения — рыночная цена и объём. При дефиците цена растёт до равновесия.</p>",
                    Description="Рынок: спрос, предложение, равновесие." },
                new Module { Title="Макроэкономика: ВВП и инфляция", ContentType="text", OrderIndex=2, DurationMinutes=25,
                    ContentText="<h4>ВВП</h4><p>Валовый внутренний продукт — суммарная стоимость всех товаров и услуг, произведённых в стране за год.</p><p>ВВП = C + I + G + NX</p><h4>Инфляция</h4><p>Устойчивый рост общего уровня цен. Измеряется через ИПЦ (индекс потребительских цен). Умеренная (2–4%) полезна для экономики.</p>",
                    Description="Макроэкономические показатели." },
                new Module { Title="Тест по экономике", ContentType="quiz", OrderIndex=3, DurationMinutes=10, IsQuiz=true,
                    Questions=new List<QuizQuestion> {
                        new QuizQuestion { Question="При росте цены спрос:", OptionA="Растёт", OptionB="Падает", OptionC="Не меняется", OptionD="Зависит от товара", CorrectAnswer="B", OrderIndex=1 },
                        new QuizQuestion { Question="ВВП = C + I + G + ?", OptionA="NX", OptionB="CPI", OptionC="GDP", OptionD="T", CorrectAnswer="A", OrderIndex=2 },
                        new QuizQuestion { Question="Умеренная инфляция — это:", OptionA="Плохо", OptionB="Нейтрально", OptionC="Полезно", OptionD="Катастрофа", CorrectAnswer="C", OrderIndex=3 },
                    }
                },
            }
        };

        // ══════════════════════════════════════════════════════════════════════
        //  INTERACTIVE — 3 курса  (ContentType: quiz-heavy + практика)
        // ══════════════════════════════════════════════════════════════════════
        var c_int1 = new Course
        {
            Title = "Практический Python: задачи и квизы",
            Description = "Учишься делая — каждая тема сразу закрепляется практическими задачами и тестами.",
            Category = "Программирование",
            Level = "Beginner",
            IsPublished = true,
            CreatedBy = 1,
            Modules = new List<Module> {
                new Module { Title="Задачи на переменные", ContentType="quiz", OrderIndex=1, DurationMinutes=12, IsQuiz=true,
                    Questions=new List<QuizQuestion> {
                        new QuizQuestion { Question="x = 5; x += 3. Чему равно x?", OptionA="5", OptionB="3", OptionC="8", OptionD="15", CorrectAnswer="C", OrderIndex=1 },
                        new QuizQuestion { Question="type('hello') — результат?", OptionA="str", OptionB="int", OptionC="list", OptionD="bool", CorrectAnswer="A", OrderIndex=2 },
                        new QuizQuestion { Question="len([1,2,3,4]) — результат?", OptionA="3", OptionB="4", OptionC="5", OptionD="0", CorrectAnswer="B", OrderIndex=3 },
                    }
                },
                new Module { Title="Задачи на условия и циклы", ContentType="quiz", OrderIndex=2, DurationMinutes=15, IsQuiz=true,
                    Questions=new List<QuizQuestion> {
                        new QuizQuestion { Question="range(1,5) — сколько чисел?", OptionA="5", OptionB="4", OptionC="6", OptionD="3", CorrectAnswer="B", OrderIndex=1 },
                        new QuizQuestion { Question="Что выведет: for i in range(3): print(i)?", OptionA="1 2 3", OptionB="0 1 2", OptionC="0 1 2 3", OptionD="1 2", CorrectAnswer="B", OrderIndex=2 },
                        new QuizQuestion { Question="Оператор для 'не равно' в Python?", OptionA="<>", OptionB="!=", OptionC="=/=", OptionD="not=", CorrectAnswer="B", OrderIndex=3 },
                    }
                },
                new Module { Title="Теория: функции кратко", ContentType="text", OrderIndex=3, DurationMinutes=10,
                    ContentText="<h4>Функции</h4><p>Определяются через <code>def</code>. Принимают аргументы, возвращают значение через <code>return</code>.</p><pre>def add(a, b):\n    return a + b\n\nresult = add(3, 4)  # 7</pre>",
                    Description="Краткая теория по функциям." },
                new Module { Title="Финальный практический тест", ContentType="quiz", OrderIndex=4, DurationMinutes=12, IsQuiz=true,
                    Questions=new List<QuizQuestion> {
                        new QuizQuestion { Question="def f(x): return x*2. f(5) = ?", OptionA="10", OptionB="5", OptionC="25", OptionD="52", CorrectAnswer="A", OrderIndex=1 },
                        new QuizQuestion { Question="Список в Python создаётся через:", OptionA="{}", OptionB="()", OptionC="[]", OptionD="<>", CorrectAnswer="C", OrderIndex=2 },
                        new QuizQuestion { Question="dict['key'] — обращение к:", OptionA="Списку", OptionB="Строке", OptionC="Словарю", OptionD="Кортежу", CorrectAnswer="C", OrderIndex=3 },
                    }
                },
            }
        };

        var c_int2 = new Course
        {
            Title = "Практическая математика: задачи и тесты",
            Description = "Алгебра, геометрия и теория вероятностей через задачи — минимум теории, максимум практики.",
            Category = "Математика",
            Level = "Intermediate",
            IsPublished = true,
            CreatedBy = 1,
            Modules = new List<Module> {
                new Module { Title="Квадратные уравнения — задачи", ContentType="quiz", OrderIndex=1, DurationMinutes=15, IsQuiz=true,
                    Questions=new List<QuizQuestion> {
                        new QuizQuestion { Question="Реши: x² - 5x + 6 = 0", OptionA="x=2, x=3", OptionB="x=1, x=6", OptionC="x=-2, x=-3", OptionD="x=5, x=6", CorrectAnswer="A", OrderIndex=1 },
                        new QuizQuestion { Question="Дискриминант D = b² - 4ac при a=1,b=4,c=3?", OptionA="4", OptionB="28", OptionC="16", OptionD="8", CorrectAnswer="A", OrderIndex=2 },
                    }
                },
                new Module { Title="Вероятность и статистика — практика", ContentType="quiz", OrderIndex=2, DurationMinutes=15, IsQuiz=true,
                    Questions=new List<QuizQuestion> {
                        new QuizQuestion { Question="P(орёл) при бросании монеты?", OptionA="1", OptionB="0.5", OptionC="0.25", OptionD="2", CorrectAnswer="B", OrderIndex=1 },
                        new QuizQuestion { Question="Среднее {2, 4, 6, 8}?", OptionA="4", OptionB="5", OptionC="6", OptionD="20", CorrectAnswer="B", OrderIndex=2 },
                        new QuizQuestion { Question="Медиана {1, 3, 5, 7, 9}?", OptionA="3", OptionB="5", OptionC="7", OptionD="1", CorrectAnswer="B", OrderIndex=3 },
                    }
                },
                new Module { Title="Геометрия — задачи на площадь", ContentType="quiz", OrderIndex=3, DurationMinutes=12, IsQuiz=true,
                    Questions=new List<QuizQuestion> {
                        new QuizQuestion { Question="Площадь прямоугольника 4×7?", OptionA="11", OptionB="22", OptionC="28", OptionD="14", CorrectAnswer="C", OrderIndex=1 },
                        new QuizQuestion { Question="Площадь круга r=3 (π≈3.14)?", OptionA="9.42", OptionB="18.84", OptionC="28.26", OptionD="6.28", CorrectAnswer="C", OrderIndex=2 },
                    }
                },
                new Module { Title="Мегатест: все темы", ContentType="quiz", OrderIndex=4, DurationMinutes=15, IsQuiz=true,
                    Questions=new List<QuizQuestion> {
                        new QuizQuestion { Question="log₂(8) = ?", OptionA="2", OptionB="3", OptionC="4", OptionD="8", CorrectAnswer="B", OrderIndex=1 },
                        new QuizQuestion { Question="sin(90°) = ?", OptionA="0", OptionB="0.5", OptionC="1", OptionD="-1", CorrectAnswer="C", OrderIndex=2 },
                        new QuizQuestion { Question="Число Фибоначчи после 8?", OptionA="12", OptionB="13", OptionC="16", OptionD="21", CorrectAnswer="B", OrderIndex=3 },
                    }
                },
            }
        };

        var c_int3 = new Course
        {
            Title = "SQL на практике: запросы и задачи",
            Description = "Учишься SQL решая реальные задачи. Каждый урок — набор практических тестов.",
            Category = "Наука",
            Level = "Beginner",
            IsPublished = true,
            CreatedBy = 1,
            Modules = new List<Module> {
                new Module { Title="SELECT — практика", ContentType="quiz", OrderIndex=1, DurationMinutes=12, IsQuiz=true,
                    Questions=new List<QuizQuestion> {
                        new QuizQuestion { Question="Выбрать всё из таблицы users:", OptionA="GET * FROM users", OptionB="SELECT * FROM users", OptionC="FETCH users", OptionD="SELECT users.*", CorrectAnswer="B", OrderIndex=1 },
                        new QuizQuestion { Question="Фильтрация строк — оператор:", OptionA="FILTER", OptionB="HAVING", OptionC="WHERE", OptionD="LIMIT", CorrectAnswer="C", OrderIndex=2 },
                        new QuizQuestion { Question="Сортировка по убыванию:", OptionA="ORDER BY DESC", OptionB="ORDER BY age DESC", OptionC="SORT DESC age", OptionD="DESC BY age", CorrectAnswer="B", OrderIndex=3 },
                    }
                },
                new Module { Title="JOIN — теория", ContentType="text", OrderIndex=2, DurationMinutes=15,
                    ContentText="<h4>INNER JOIN</h4><p>Только совпадающие строки из обеих таблиц.</p><pre>SELECT u.name, o.amount\nFROM users u\nINNER JOIN orders o ON u.id = o.user_id;</pre><h4>LEFT JOIN</h4><p>Все строки из левой таблицы + совпадения из правой. Если нет совпадения — NULL.</p>",
                    Description="Типы JOIN." },
                new Module { Title="JOIN и агрегация — практика", ContentType="quiz", OrderIndex=3, DurationMinutes=15, IsQuiz=true,
                    Questions=new List<QuizQuestion> {
                        new QuizQuestion { Question="COUNT(*) считает:", OptionA="Сумму", OptionB="Среднее", OptionC="Количество строк", OptionD="Максимум", CorrectAnswer="C", OrderIndex=1 },
                        new QuizQuestion { Question="GROUP BY используется с:", OptionA="WHERE", OptionB="HAVING и агрегатами", OptionC="ORDER BY", OptionD="SELECT *", CorrectAnswer="B", OrderIndex=2 },
                        new QuizQuestion { Question="LEFT JOIN возвращает:", OptionA="Только совпадения", OptionB="Все из левой + совпадения", OptionC="Все из правой", OptionD="Декартово произведение", CorrectAnswer="B", OrderIndex=3 },
                    }
                },
            }
        };

        // ══════════════════════════════════════════════════════════════════════
        //  SOCIAL — 3 курса  (бизнес, коммуникации, менеджмент)
        // ══════════════════════════════════════════════════════════════════════
        var c_soc1 = new Course
        {
            Title = "Бизнес-коммуникации и презентации",
            Description = "Переговоры, публичные выступления, деловая переписка. Практические навыки бизнес-общения.",
            Category = "Бизнес",
            Level = "Beginner",
            IsPublished = true,
            CreatedBy = 1,
            Modules = new List<Module> {
                new Module { Title="Структура убедительной презентации", ContentType="video", OrderIndex=1, DurationMinutes=14,
                    ContentUrl="https://www.youtube.com/watch?v=Ks-_Mh1QhMc", Description="Как строить презентацию." },
                new Module { Title="Деловая переписка — принципы", ContentType="text", OrderIndex=2, DurationMinutes=18,
                    ContentText="<h4>Структура делового письма</h4><ol><li>Тема — конкретная и краткая</li><li>Приветствие — формальное</li><li>Суть — первый абзац</li><li>Детали — второй абзац</li><li>Призыв к действию</li><li>Подпись</li></ol><h4>Правила</h4><ul><li>Одно письмо = одна тема</li><li>Ответ в течение 24 часов</li><li>Не пиши заглавными — воспринимается как крик</li></ul>",
                    Description="Деловая переписка." },
                new Module { Title="Переговоры — тактики", ContentType="video", OrderIndex=3, DurationMinutes=16,
                    ContentUrl="https://www.youtube.com/watch?v=llct21xVSDs", Description="Тактики переговоров." },
                new Module { Title="Тест: Бизнес-коммуникации", ContentType="quiz", OrderIndex=4, DurationMinutes=8, IsQuiz=true,
                    Questions=new List<QuizQuestion> {
                        new QuizQuestion { Question="Первое правило деловой переписки?", OptionA="Длинные письма", OptionB="Одна тема на письмо", OptionC="Писать капсом", OptionD="Нет структуры", CorrectAnswer="B", OrderIndex=1 },
                        new QuizQuestion { Question="Цель убедительной презентации?", OptionA="Много слайдов", OptionB="Изменить мнение/действие аудитории", OptionC="Показать знания", OptionD="Долго говорить", CorrectAnswer="B", OrderIndex=2 },
                    }
                },
            }
        };

        var c_soc2 = new Course
        {
            Title = "Основы менеджмента и лидерства",
            Description = "Управление командой, мотивация, постановка задач. Для будущих тимлидов и менеджеров.",
            Category = "Бизнес",
            Level = "Intermediate",
            IsPublished = true,
            CreatedBy = 1,
            Modules = new List<Module> {
                new Module { Title="Стили лидерства — обзор", ContentType="video", OrderIndex=1, DurationMinutes=15,
                    ContentUrl="https://www.youtube.com/watch?v=oS_mOYNzd3E", Description="Типы лидерства." },
                new Module { Title="Постановка задач по SMART", ContentType="text", OrderIndex=2, DurationMinutes=20,
                    ContentText="<h4>SMART-цели</h4><ul><li><b>S</b> — Specific (конкретная)</li><li><b>M</b> — Measurable (измеримая)</li><li><b>A</b> — Achievable (достижимая)</li><li><b>R</b> — Relevant (значимая)</li><li><b>T</b> — Time-bound (с дедлайном)</li></ul><p>Плохая цель: «улучшить продажи».<br>SMART: «увеличить продажи на 15% за Q3 2025».</p>",
                    Description="SMART-цели в менеджменте." },
                new Module { Title="Мотивация команды", ContentType="video", OrderIndex=3, DurationMinutes=14,
                    ContentUrl="https://www.youtube.com/watch?v=u6XAPnuFjJc", Description="Теории мотивации." },
                new Module { Title="Тест по менеджменту", ContentType="quiz", OrderIndex=4, DurationMinutes=8, IsQuiz=true,
                    Questions=new List<QuizQuestion> {
                        new QuizQuestion { Question="T в SMART означает:", OptionA="Team", OptionB="True", OptionC="Time-bound", OptionD="Transparent", CorrectAnswer="C", OrderIndex=1 },
                        new QuizQuestion { Question="Теория Маслоу — это:", OptionA="Стили управления", OptionB="Иерархия потребностей", OptionC="Метод планирования", OptionD="Финансовая модель", CorrectAnswer="B", OrderIndex=2 },
                        new QuizQuestion { Question="Делегирование — это:", OptionA="Увольнение", OptionB="Передача задачи и ответственности", OptionC="Контроль всего лично", OptionD="Отчёт о работе", CorrectAnswer="B", OrderIndex=3 },
                    }
                },
            }
        };

        var c_soc3 = new Course
        {
            Title = "Маркетинг и продвижение",
            Description = "Digital-маркетинг, SMM, email-кампании и аналитика. Теория + реальные кейсы.",
            Category = "Бизнес",
            Level = "Beginner",
            IsPublished = true,
            CreatedBy = 1,
            Modules = new List<Module> {
                new Module { Title="Digital-маркетинг: введение", ContentType="video", OrderIndex=1, DurationMinutes=13,
                    ContentUrl="https://www.youtube.com/watch?v=bixR-KIJKYM", Description="Основы digital." },
                new Module { Title="SMM и контент-стратегия", ContentType="text", OrderIndex=2, DurationMinutes=20,
                    ContentText="<h4>Контент-стратегия</h4><p>Контент-план — расписание публикаций. Правило 80/20: 80% пользы, 20% продаж.</p><h4>Форматы</h4><ul><li>Карусели — высокий охват</li><li>Reels / видео — максимальный охват</li><li>Истории — вовлечённость</li><li>Посты с текстом — авторитет</li></ul><h4>Метрики</h4><p>Reach, Impressions, Engagement Rate = (лайки+комменты) / подписчики × 100%</p>",
                    Description="SMM стратегия." },
                new Module { Title="Email-маркетинг и воронки", ContentType="text", OrderIndex=3, DurationMinutes=18,
                    ContentText="<h4>Email воронка</h4><p>Awareness → Interest → Desire → Action (AIDA).</p><h4>Ключевые метрики</h4><ul><li>Open Rate — % открытий</li><li>CTR — % кликов</li><li>Conversion Rate — % покупок</li><li>Unsubscribe Rate — % отписок</li></ul><p>Хороший OR: 20–25%.</p>",
                    Description="Email-маркетинг." },
                new Module { Title="Тест по маркетингу", ContentType="quiz", OrderIndex=4, DurationMinutes=8, IsQuiz=true,
                    Questions=new List<QuizQuestion> {
                        new QuizQuestion { Question="AIDA — последний шаг:", OptionA="Awareness", OptionB="Interest", OptionC="Desire", OptionD="Action", CorrectAnswer="D", OrderIndex=1 },
                        new QuizQuestion { Question="CTR расшифровывается как:", OptionA="Content To Reader", OptionB="Click-Through Rate", OptionC="Customer Traffic Rate", OptionD="Creative Tag Reach", CorrectAnswer="B", OrderIndex=2 },
                        new QuizQuestion { Question="Правило 80/20 в SMM означает:", OptionA="80% рекламы, 20% пользы", OptionB="80% пользы, 20% рекламы", OptionC="80% видео, 20% текста", OptionD="80% подписчиков активны", CorrectAnswer="B", OrderIndex=3 },
                    }
                },
            }
        };

        // ══════════════════════════════════════════════════════════════════════
        //  RESEARCH — 3 курса  (наука, данные, аналитика — глубокие тексты)
        // ══════════════════════════════════════════════════════════════════════
        var c_res1 = new Course
        {
            Title = "Машинное обучение: теория и математика",
            Description = "Глубокое погружение в ML — математическая база, метрики, интерпретация моделей.",
            Category = "Наука",
            Level = "Advanced",
            IsPublished = true,
            CreatedBy = 1,
            Modules = new List<Module> {
                new Module { Title="Математика ML: линейная алгебра", ContentType="text", OrderIndex=1, DurationMinutes=30,
                    ContentText="<h4>Векторы и матрицы</h4><p>Вектор — упорядоченный набор чисел. Матрица — двумерный массив.</p><p>Скалярное произведение: <code>a·b = Σaᵢbᵢ</code></p><h4>Нормализация</h4><p>L2-норма: ‖x‖ = √(Σxᵢ²)</p><h4>SVD-разложение</h4><p>A = UΣVᵀ — основа рекомендательных систем и PCA.</p>",
                    Description="Линейная алгебра для ML." },
                new Module { Title="Теория вероятностей для ML", ContentType="text", OrderIndex=2, DurationMinutes=28,
                    ContentText="<h4>Теорема Байеса</h4><p>P(A|B) = P(B|A)·P(A) / P(B)</p><p>Основа Naive Bayes, байесовской оптимизации и многих других алгоритмов.</p><h4>Распределения</h4><ul><li>Нормальное — bell curve, CLT</li><li>Бернулли — бинарный исход</li><li>Пуассона — редкие события</li></ul>",
                    Description="Вероятностная теория." },
                new Module { Title="Метрики качества моделей", ContentType="text", OrderIndex=3, DurationMinutes=25,
                    ContentText="<h4>Классификация</h4><ul><li>Accuracy = (TP+TN)/(TP+TN+FP+FN)</li><li>Precision = TP/(TP+FP)</li><li>Recall = TP/(TP+FN)</li><li>F1 = 2·P·R/(P+R)</li><li>ROC-AUC — площадь под ROC-кривой</li></ul><h4>Регрессия</h4><ul><li>MAE = среднее |y - ŷ|</li><li>RMSE = √(среднее (y-ŷ)²)</li><li>R² — коэффициент детерминации</li></ul>",
                    Description="Метрики качества." },
                new Module { Title="Тест по теории ML", ContentType="quiz", OrderIndex=4, DurationMinutes=12, IsQuiz=true,
                    Questions=new List<QuizQuestion> {
                        new QuizQuestion { Question="ROC-AUC = 0.5 означает:", OptionA="Отличная модель", OptionB="Случайное угадывание", OptionC="Переобучение", OptionD="Идеальная модель", CorrectAnswer="B", OrderIndex=1 },
                        new QuizQuestion { Question="Recall (полнота) = TP / (TP + ?)", OptionA="FP", OptionB="TN", OptionC="FN", OptionD="TP", CorrectAnswer="C", OrderIndex=2 },
                        new QuizQuestion { Question="SVD используется в:", OptionA="Нейросетях", OptionB="Рекомендательных системах", OptionC="Сортировке", OptionD="Регрессии", CorrectAnswer="B", OrderIndex=3 },
                    }
                },
            }
        };

        var c_res2 = new Course
        {
            Title = "Анализ данных с Python и Pandas",
            Description = "Исследовательский анализ данных: загрузка, очистка, визуализация, статистика.",
            Category = "Наука",
            Level = "Intermediate",
            IsPublished = true,
            CreatedBy = 1,
            Modules = new List<Module> {
                new Module { Title="Pandas: загрузка и изучение данных", ContentType="text", OrderIndex=1, DurationMinutes=25,
                    ContentText="<h4>Основные операции</h4><pre>import pandas as pd\ndf = pd.read_csv('data.csv')\ndf.head()        # первые 5 строк\ndf.info()        # типы и пропуски\ndf.describe()    # статистика\ndf.shape         # (строки, столбцы)</pre><h4>Выборка данных</h4><pre>df[df['age'] > 25]          # фильтрация\ndf[['name', 'age']]         # столбцы\ndf.loc[0:5, 'salary']       # срез</pre>",
                    Description="Pandas для анализа данных." },
                new Module { Title="Очистка данных", ContentType="text", OrderIndex=2, DurationMinutes=22,
                    ContentText="<h4>Пропущенные значения</h4><pre>df.isnull().sum()          # подсчёт\ndf.dropna()                # удалить строки\ndf.fillna(df.mean())       # заполнить средним</pre><h4>Дубликаты</h4><pre>df.duplicated().sum()      # подсчёт\ndf.drop_duplicates()       # удалить</pre><h4>Типы данных</h4><pre>df['date'] = pd.to_datetime(df['date'])\ndf['cat']  = df['cat'].astype('category')</pre>",
                    Description="Очистка данных в Pandas." },
                new Module { Title="Статистика и группировка", ContentType="text", OrderIndex=3, DurationMinutes=20,
                    ContentText="<h4>Группировка</h4><pre>df.groupby('city')['salary'].mean()\ndf.groupby(['city','dept']).agg({'salary':'mean','age':'count'})</pre><h4>Корреляция</h4><pre>df.corr()                  # матрица корреляций\ndf['a'].corr(df['b'])      # корреляция пары</pre>",
                    Description="Статистический анализ." },
                new Module { Title="Тест по Pandas", ContentType="quiz", OrderIndex=4, DurationMinutes=10, IsQuiz=true,
                    Questions=new List<QuizQuestion> {
                        new QuizQuestion { Question="df.shape возвращает:", OptionA="Тип данных", OptionB="(строки, столбцы)", OptionC="Индексы", OptionD="Названия столбцов", CorrectAnswer="B", OrderIndex=1 },
                        new QuizQuestion { Question="Удалить дубликаты в pandas:", OptionA="df.remove_dups()", OptionB="df.drop_duplicates()", OptionC="df.unique()", OptionD="df.distinct()", CorrectAnswer="B", OrderIndex=2 },
                        new QuizQuestion { Question="groupby используется для:", OptionA="Сортировки", OptionB="Группировки и агрегации", OptionC="Фильтрации", OptionD="Слияния таблиц", CorrectAnswer="B", OrderIndex=3 },
                    }
                },
            }
        };

        var c_res3 = new Course
        {
            Title = "Статистика для исследователей",
            Description = "Описательная и инференциальная статистика, гипотезы, A/B-тесты — для тех кто любит разбираться глубоко.",
            Category = "Наука",
            Level = "Advanced",
            IsPublished = true,
            CreatedBy = 1,
            Modules = new List<Module> {
                new Module { Title="Описательная статистика", ContentType="text", OrderIndex=1, DurationMinutes=25,
                    ContentText="<h4>Меры центральной тенденции</h4><ul><li>Среднее: x̄ = Σxᵢ/n</li><li>Медиана: серединное значение</li><li>Мода: наиболее частое значение</li></ul><h4>Меры разброса</h4><ul><li>Дисперсия: σ² = Σ(xᵢ-x̄)²/n</li><li>Стандартное отклонение: σ</li><li>IQR: Q3 - Q1</li></ul>",
                    Description="Описательная статистика." },
                new Module { Title="Проверка гипотез", ContentType="text", OrderIndex=2, DurationMinutes=28,
                    ContentText="<h4>Нулевая гипотеза H₀</h4><p>Предположение, которое мы проверяем. Альтернативная H₁ — то, что хотим доказать.</p><h4>p-value</h4><p>Вероятность получить наблюдаемые данные при истинной H₀. Если p &lt; 0.05 — отвергаем H₀.</p><h4>t-тест</h4><p>Сравнение средних двух групп. t = (x̄₁ - x̄₂) / SE</p><h4>Ошибки</h4><ul><li>I рода (α): отвергли верную H₀</li><li>II рода (β): не отвергли ложную H₀</li></ul>",
                    Description="Статистические гипотезы." },
                new Module { Title="A/B-тестирование", ContentType="text", OrderIndex=3, DurationMinutes=22,
                    ContentText="<h4>Методология A/B-теста</h4><ol><li>Сформулируй гипотезу</li><li>Определи метрику (CR, ARPU, CTR)</li><li>Рассчитай необходимый размер выборки</li><li>Случайно раздели пользователей</li><li>Запусти тест на нужный срок</li><li>Проверь статзначимость</li></ol><h4>Размер выборки</h4><p>n = (z·σ/d)² где d — минимальный детектируемый эффект.</p>",
                    Description="A/B тестирование." },
                new Module { Title="Тест по статистике", ContentType="quiz", OrderIndex=4, DurationMinutes=10, IsQuiz=true,
                    Questions=new List<QuizQuestion> {
                        new QuizQuestion { Question="p-value < 0.05 означает:", OptionA="Принять H₀", OptionB="Отвергнуть H₀", OptionC="Тест не работает", OptionD="Нет разницы", CorrectAnswer="B", OrderIndex=1 },
                        new QuizQuestion { Question="Стандартное отклонение — это:", OptionA="Максимум - минимум", OptionB="√дисперсии", OptionC="Среднее/n", OptionD="Сумма отклонений", CorrectAnswer="B", OrderIndex=2 },
                        new QuizQuestion { Question="Ошибка I рода — это:", OptionA="Не отвергли ложную H₀", OptionB="Отвергли верную H₀", OptionC="Малая выборка", OptionD="Неверный тест", CorrectAnswer="B", OrderIndex=3 },
                    }
                },
            }
        };

        var courses = new List<Course> {
            // visual (3)
            c_vis1, c_vis2, c_vis3,
            // reading (3)
            c_read1, c_read2, c_read3,
            // interactive (3)
            c_int1, c_int2, c_int3,
            // social (3)
            c_soc1, c_soc2, c_soc3,
            // research (3)
            c_res1, c_res2, c_res3,
        };
        db.Courses.AddRange(courses);
        db.SaveChanges();

        // ══════════════════════════════════════════════════════════════════════
        //  СТУДЕНТЫ — 8 студентов с разными профилями и слабыми местами
        // ══════════════════════════════════════════════════════════════════════
        var rng = new Random(42);

        // (name, email, avgScore, dominantStyle, weakContentType, pred)
        // weakContentType — тип контента, где студент слаб: "video" | "text" | "quiz" | null
        var studentProfiles = new[]
        {
            // Алия — визуал, но плохо с квизами
            ("Алия Назарова",    "aliya@test.kz",   88.0, "visual",      "quiz",  "Distinction", 600, 100, 50),
            // Марат — визуал, слаб в теории (text)
            ("Марат Сейтов",     "marat@test.kz",   70.0, "visual",      "text",  "Pass",        500, 80, 100),
            // Дина — практик, всё ок
            ("Дина Ахметова",    "dina@test.kz",    92.0, "interactive", null,    "Distinction",  80, 100, 700),
            // Нурлан — исследователь, слаб в видео
            ("Нурлан Бекжанов",  "nurlan@test.kz",  45.0, "research",    "video", "Fail",         60, 800, 100),
            // Сания — социальный стиль, плохо с квизами
            ("Сания Жакупова",   "sania@test.kz",   38.0, "social",      "quiz",  "Withdrawn",   150, 150, 50),
            // Азамат — читатель, слаб в видео
            ("Азамат Токаев",    "azamat@test.kz",  75.0, "reading",     "video", "Pass",         80, 600, 150),
            // Камила — практик, слаб в теории
            ("Камила Абдрахман", "kamila@test.kz",  85.0, "interactive", "text",  "Distinction", 100, 60, 650),
            // Тимур — визуал, плохо с квизами
            ("Тимур Касымов",    "timur@test.kz",   60.0, "visual",      "quiz",  "Pass",        400, 200, 100),
        };

        // Индексы курсов по стилю — для записи на курс
        var styleToIndexes = new Dictionary<string, int[]>
        {
            ["visual"] = new[] { 0, 1 },   // c_vis1, c_vis2
            ["reading"] = new[] { 3, 4 },   // c_read1, c_read2
            ["interactive"] = new[] { 6, 7 },   // c_int1, c_int2
            ["social"] = new[] { 9, 10 },  // c_soc1, c_soc2
            ["research"] = new[] { 12, 13 }, // c_res1, c_res2
        };

        foreach (var (name, email, avgScore, dominantStyle, weakContentType, pred,
                       videoTime, textTime, quizTime) in studentProfiles)
        {
            var user = new User
            {
                Name = name,
                Email = email,
                Role = "Student",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("test123"),
            };
            db.Users.Add(user);
            db.SaveChanges();

            var totalTime = videoTime + textTime + quizTime + 1.0;
            db.LearningProfiles.Add(new LearningProfile
            {
                UserId = user.Id,
                DominantStyle = dominantStyle,
                AvgScore = avgScore,
                PredictedResult = pred,
                RatioVideo = videoTime / totalTime,
                RatioText = textTime / totalTime,
                RatioQuiz = quizTime / totalTime,
                WeakTopics = weakContentType != null ? $"[\"{weakContentType}\"]" : "[]",
                UpdatedAt = DateTime.UtcNow,
            });

            // Записываем на 2 курса своего стиля
            var courseIndexes = styleToIndexes[dominantStyle];
            foreach (var idx in courseIndexes)
            {
                db.Enrollments.Add(new Enrollment
                {
                    UserId = user.Id,
                    CourseId = courses[idx].Id,
                    Progress = rng.Next(15, 85),
                });
            }

            // Генерируем результаты тестов:
            // — на сильных ContentType — высокие баллы
            // — на слабых ContentType — низкие баллы
            foreach (var course in courses.Take(6)) // первые 6 курсов для разнообразия
            {
                foreach (var module in course.Modules)
                {
                    // Пропускаем ~70% модулей — не все студенты всё прошли
                    if (rng.Next(10) < 7) continue;

                    bool isWeak = weakContentType != null && module.ContentType == weakContentType;
                    int baseScore;
                    if (module.ContentType == "video")
                        baseScore = isWeak ? rng.Next(25, 50) : (int)(videoTime > 300 ? avgScore + rng.Next(-5, 10) : avgScore - 10);
                    else if (module.ContentType == "text")
                        baseScore = isWeak ? rng.Next(20, 45) : (int)(textTime > 300 ? avgScore + rng.Next(-5, 10) : avgScore - 8);
                    else if (module.ContentType == "quiz")
                        baseScore = isWeak ? rng.Next(15, 40) : (int)(quizTime > 300 ? avgScore + rng.Next(-5, 12) : avgScore - 5);
                    else
                        baseScore = (int)(avgScore + rng.Next(-8, 8));

                    baseScore = Math.Clamp(baseScore, 0, 100);

                    db.TestResults.Add(new TestResult
                    {
                        UserId = user.Id,
                        ModuleId = module.Id,
                        Score = module.ContentType == "video" ? -1 : baseScore,
                        TimeSpentSec = module.ContentType == "video" ? videoTime / 2 :
                                       module.ContentType == "text" ? textTime / 3 : quizTime / 3,
                    });
                }
            }
        }

        db.SaveChanges();
        Console.WriteLine("✅ Seed: 15 курсов (3×5 стилей) + 8 студентов с профилями слабых мест");
    }
}