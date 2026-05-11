/*
Five-Letters Guessing Game
Copyright (C) 2026  Max Parkhomenko

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using System.Text.RegularExpressions;
using Spectre.Console;

public class Program
{
    private static bool displayKeyboard = true;
    public static string[] words = [
        "арбуз",
        "армия",
        "банан",
        "берег",
        "билет",
        "бочка",
        "ветка",
        "вишня",
        "весна",
        "волна",
        "город",
        "груша",
        "дверь",
        "диван",
        "дождь",
        "драка",
        "доска",
        "жираф",
        "замок",
        "зебра",
        "земля",
        "игрок",
        "индюк",
        "кабан",
        "капля",
        "карта",
        "каюта",
        "кисть",
        "книга",
        "ковер",
        "колос",
        "комар",
        "конец",
        "копье",
        "кошка",
        "крыло",
        "лампа",
        "лилия",
        "лимон",
        "лодка",
        "ложка",
        "магия",
        "маска",
        "месяц",
        "метро",
        "мечта",
        "мышка",
        "носок",
        "недуг",
        "обмен",
        "океан",
        "олень",
        "палец",
        "песня",
        "петух",
        "пирог",
        "пламя",
        "плечо",
        "полка",
        "птица",
        "пушка",
        "радар",
        "рукав",
        "рынок",
        "салат",
        "сапог",
        "свеча",
        "серия",
        "скала",
        "сквер",
        "скрип",
        "слеза",
        "слово",
        "снега",
        "сокол",
        "сосна",
        "спина",
        "стена",
        "столб",
        "сумка",
        "тайга",
        "танец",
        "топор",
        "трава",
        "труба",
        "тунец",
        "тыква",
        "уголь",
        "улица",
        "фрукт",
        "халат",
        "цапля",
        "чайка",
        "череп",
        "шапка",
        "школа",
        "шпион",
        "шприц",
        "щенок",
        "экран",
        "юноша",
        "ягода"
    ];
    public static void Main()
    {
        if (Console.WindowHeight < 18)
        {
            Console.WriteLine("Предупреждение: размер окна слишком мал, виртуальная клавиатура не будет отображена!");
            displayKeyboard = false;
        }
        Console.WriteLine("Добро пожаловать в игру \"Пять Букв\"!");
        Console.Write("Хотите расскажу правила? (да/нет): ");
        string? answer = Console.ReadLine();
        if (answer == null)
        {
            Console.WriteLine("Ошибка ввода-вывода!");
            return;
        }
        if (answer.Equals("да", StringComparison.OrdinalIgnoreCase))
        {
            #region Game Rules
            Console.WriteLine("Хорошо, вот правила: ");
            Console.WriteLine("Игра загадывает слово из 5 букв, у тебя есть 6 попыток чтобы его угадать.");
            Console.WriteLine("На каждой попытке игра подсвечивает каждую букву из введенного слова определенным цветом: ");
            AnsiConsole.MarkupLine("1. Буква есть в загаданном слове на той же позиции ({0})", "[black on green]зеленый цвет[/]");
            AnsiConsole.MarkupLine("2. Буква есть в загаданном слове, но на другой позиции ({0})", "[black on yellow]желтый цвет[/]");
            AnsiConsole.MarkupLine("3. Буквы нет в загаданном слове ({0})", "[black on red]красный цвет[/]");
            Console.WriteLine("Например, загадано слово \"мерка\". Вы ввели слово \"груша\"");
            Console.Write("Тогда будет выведено: ");
            AnsiConsole.MarkupLine("[black on red]г[/][black on yellow]р[/][black on red]у[/][black on red]ш[/][black on green]а[/]");
            Console.WriteLine("Также внизу программы есть виртуальная клавиатура, на которой каждая буква также подсвечивается.");
            Console.WriteLine("Нажмите любую клавишу, чтобы продолжить...");
            Console.ReadKey(true);
            #endregion
        }
        string hiddenWord = words[Random.Shared.Next(0, words.Length - 1)];
        bool win = false;
        Console.Clear();
        KeyColor[] kboard = new KeyColor[32];
        try
        {
            for (int i = 0; i < 6; i++)
            {
                Console.WriteLine("\tПопытка номер {0}", i + 1);
                Console.Write("Введите слово: ");
                string? word = Console.ReadLine()?.ToLower();
                #region Word Checks
                if (word == null)
                {
                    Console.WriteLine("Ошибка ввода-вывода!");
                    throw new IOException("IOException: word is null");
                }
                if (word.Length != 5)
                {
                    Console.WriteLine("Требуется ввести слово из 5 букв!");
                    i--;
                    continue;
                }
                if (!Regex.IsMatch(word, @"^[а-яё]+$"))
                {
                    Console.WriteLine("Требуется ввести русское слово, не содержащее спец. символов!");
                    i--;
                    continue;
                }
                #endregion
                Dictionary<char, int> charCount = CountChars(word.ToCharArray(), hiddenWord);
                List<char> alreadyYellowSingularChars = [];
                for (int cIndex = 0; cIndex < 5; cIndex++)
                {
                    char c = word[cIndex];
                    int iChar = GetKeyboardId(c);
                    if (hiddenWord[cIndex] == c)
                    {
                        AnsiConsole.Markup("[black on green]{0}[/]", c);
                        kboard[iChar] = KeyColor.Green;
                    }
                    else if (hiddenWord.Contains(c))
                    {
                        if (alreadyYellowSingularChars.Contains(c) || charCount[c] == 1)
                        {
                            AnsiConsole.Markup("[black on red]{0}[/]", c);
                            if (kboard[iChar] !=  KeyColor.Green)
                                kboard[iChar] = KeyColor.Red;
                            continue;
                        }
                        AnsiConsole.Markup("[black on yellow]{0}[/]", c);
                        if (kboard[iChar] !=  KeyColor.Green)
                            kboard[iChar] = KeyColor.Yellow;
                        if (hiddenWord.Count(ch => ch == c) == 1)
                        {
                            alreadyYellowSingularChars.Add(c);
                        }
                    }
                    else
                    {
                        AnsiConsole.Markup("[black on red]{0}[/]", c);
                        if (kboard[iChar] !=  KeyColor.Green)
                            kboard[iChar] = KeyColor.Red;
                    }
                }
                Console.WriteLine();
                DisplayVirtualKeyboard(kboard);
                if (hiddenWord == word)
                {
                    win = true;
                    break;
                }
            }

        }
        catch (Exception ex)
        {
            RemoveVirtualKeyboard();
            Console.WriteLine("Произошла ошибка! Сообщите разработчику: {0}", ex.Message);
        }
        finally
        {
            RemoveVirtualKeyboard();
            if (win)
            {
                Console.WriteLine("Поздравляю с победой! Перезапустите игру, чтобы сыграть ещё!");
            }
            else
            {
                Console.WriteLine("К сожалению, вы проиграли. Загаданное слово было: {0}", hiddenWord);
                Console.WriteLine("Перезапустите программу чтобы сыграть ещё!");
            }

            Console.Write("Нажмите любую клавишу для выхода...");
            Console.ReadKey(true);
        }
    }

    private static void DisplayVirtualKeyboard(KeyColor[] keyboardParameters) // 32 params
    {
        if (!displayKeyboard) return;
        (int left, int top) = Console.GetCursorPosition();
        (int newLeft, int newTop) = (left, Console.WindowHeight - 5);
        Console.SetCursorPosition(newLeft, newTop);

        byte row = 0;
        int currentKeyInRow = 0;
        char[][] kboard = [
            ['й', 'ц', 'у', 'к', 'е', 'н', 'г', 'ш', 'щ', 'з', 'х', 'ъ'],
            ['ф', 'ы', 'в', 'а', 'п', 'р', 'о', 'л', 'д', 'ж', 'э'],
            ['я', 'ч', 'с', 'м', 'и', 'т', 'ь', 'б', 'ю']
        ];
        for (int i = 0; i < 32; i++)
        {
            if (i < 12)
            {
                row = 0;
                currentKeyInRow = i;
            }
            else if (i < 23)
            {
                if (i == 12)
                {
                    Console.WriteLine();
                    Console.WriteLine();
                }
                row = 1;
                currentKeyInRow = i - 12;
            }
            else
            {
                if (i == 23)
                {
                    Console.WriteLine();
                    Console.WriteLine();
                }
                row = 2;
                currentKeyInRow = i - 23;
            }
            char key = kboard[row][currentKeyInRow];
            KeyColor color = keyboardParameters[i];

            switch (color)
            {
                case KeyColor.White:
                    Console.Write("{0}  ", key);
                    break;
                case KeyColor.Green:
                    AnsiConsole.Markup("[black on green]{0}[/]  ", key);
                    break;
                case KeyColor.Yellow:
                    AnsiConsole.Markup("[black on yellow]{0}[/]  ", key);
                    break;
                case KeyColor.Red:
                    AnsiConsole.Markup("[black on red]{0}[/]  ", key);
                    break;
            }
        }

        Console.SetCursorPosition(left, top);
    }

    private static void RemoveVirtualKeyboard()
    {
        (int left, int top) = Console.GetCursorPosition();
        (int newLeft, int newTop) = (left, Console.WindowHeight - 5);
        Console.SetCursorPosition(newLeft, newTop);

        Console.WriteLine("                                                            \n");
        Console.WriteLine("                                                            \n");
        Console.WriteLine("                                                            \n");
        
        Console.SetCursorPosition(left, top);
    }

    private static int GetKeyboardId(char ch)
    {
        const string keyboardAlphabet = "йцукенгшщзхъфывапролджэячсмитьбю";
        return keyboardAlphabet.IndexOf(ch);        
    }

    private static Dictionary<char, int> CountChars(char[] chars, string word)
    {
        Dictionary<char, int> result = new Dictionary<char, int>();
        chars = chars.Distinct().ToArray();
        foreach (char ch in chars)
        {
            int count = word.Count(c => c == ch);
            if (!result.TryAdd(ch, count))
            {
                result[ch] = count + 1;
            }
        }
        return result;
    }
}