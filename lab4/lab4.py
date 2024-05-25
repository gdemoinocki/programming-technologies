import time


def kmp_search(text, pattern):
    occurrences = []  # Список для хранения индексов начала вхождений шаблона в текст
    for i in range(len(text) - len(pattern) + 1):  # Перебираем текст для поиска совпадений
        match = True  # Флаг, указывающий на совпадение шаблона с текстом в текущей позиции
        for j in range(len(pattern)):  # Перебираем символы шаблона
            if text[i + j] != pattern[j]:  # Если символы не совпадают, сбрасываем флаг и прерываем внутренний цикл
                match = False
                break
        if match:  # Если шаблон полностью совпал с частью текста
            occurrences.append(i)  # Добавляем индекс начала совпадения в список
    return occurrences  # Возвращаем список индексов вхождений


def naive_search(text, pattern):
    def compute_lps_array(pattern):
        lps = [0] * len(pattern)  # Массив для хранения длин наибольших префиксов, которые являются суффиксами
        length = 0  # Длина текущего наибольшего префикса
        i = 1
        while i < len(pattern):  # Построение массива lps
            if pattern[i] == pattern[length]:
                length += 1
                lps[i] = length
                i += 1
            else:
                if length != 0:
                    length = lps[length - 1]
                else:
                    lps[i] = 0
                    i += 1
        return lps

    occurrences = []  # Список для хранения индексов начала вхождений
    lps = compute_lps_array(pattern)  # Предварительный расчет массива lps
    i = j = 0  # Индексы для текста и шаблона
    while i < len(text):  # Поиск шаблона в тексте
        if pattern[j] == text[i]:  # Сравнение символов шаблона и текста
            i += 1
            j += 1
        if j == len(pattern):  # Если найдено полное совпадение шаблона
            occurrences.append(i - j)  # Записываем индекс начала совпадения
            j = lps[j - 1]  # Используем lps для определения следующей позиции шаблона
        elif i < len(text) and pattern[j] != text[i]:  # Если символы не совпадают
            if j != 0:
                j = lps[j - 1]  # Перемещаем индекс шаблона с использованием lps
            else:
                i += 1
    return occurrences  # Возвращаем список индексов вхождений


def read_book(file_path):
    with open(file_path, 'r', encoding='utf-8') as file:  # Открываем файл для чтения
        return file.read()  # Возвращаем содержимое файла


def search_and_measure(text, names):
    results = {}  # Словарь для хранения результатов поиска
    for name in names:  # Для каждого имени из списка
        for search_algorithm in (kmp_search, naive_search):  # Используем каждый алгоритм поиска
            start_time = time.time()  # Засекаем время начала поиска
            occurrences = search_algorithm(text, name)  # Выполняем поиск
            duration = time.time() - start_time  # Вычисляем продолжительность поиска
            print(f"{search_algorithm.__name__} найдено {len(occurrences)} упоминаний персонажа: '{name}' за {duration:.6f} секунд.")
            results[name] = len(occurrences)  # Сохраняем результаты поиска
    return results  # Возвращаем результаты


# Путь к файлу и запуск функций для демонстрации работы программы
file_path = 'war_and_peace.txt'
text = read_book(file_path)
names = ["Пьер", "Наташа", "Андрей", "Николай", "Элен"]  # Имена для поиска

search_and_measure(text, names)  # Выполнение поиска и измерение времени
