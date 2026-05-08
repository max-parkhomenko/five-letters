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

using Spectre.Console;

public class Program
{
        public static string[] words = {
    "арбуз",
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
    "облак",
    "океан",
    "олень",
    "орехи",
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
    };
    public static void Main()
    {
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
            Console.WriteLine("Хорошо, вот правила: ");
            Console.WriteLine("Игра загадывает слово из 5 букв, у тебя есть 6 попыток чтобы его угадать.");
            Console.WriteLine("На каждой попытке игра подсвечивает каждую букву из введенного слова определенным цветом: ");
            AnsiConsole.MarkupLine("1. Буква есть в загаданном слове на той же позиции ({0})", "[black on green]зеленый цвет[/]");
            AnsiConsole.MarkupLine("2. Буква есть в загаданном слове, но на другой позиции ({0})", "[black on yellow]желтый цвет[/]");
            AnsiConsole.MarkupLine("3. Буквы нет в загаданном слове ({0})", "[black on red]красный цвет[/]");
            Console.WriteLine("Например, загадано слово \"мерка\". Вы ввели слово \"груша\"");
            Console.Write("Тогда будет выведено: ");
            AnsiConsole.MarkupLine("[black on red]г[/][black on yellow]р[/][black on red]у[/][black on red]ш[/][black on green]а[/]");
        }
        Console.WriteLine("Начнем игру...");
        string hiddenWord = words[Random.Shared.Next(0, words.Length - 1)];
        Console.Clear();
        for (int i = 0; i < 6; i++)
        {
            thisTry:
            Console.WriteLine("\tПопытка номер {0}", i + 1);
            Console.Write("Введите слово: ");
            string? word = Console.ReadLine();
            if (word == null)
            {
                Console.WriteLine("Ошибка!");
                return;
            }
            if (word.Length != 5)
            {
                Console.WriteLine("Требуется ввести слово из 5 букв!");
                goto thisTry;
            }
            Dictionary<char, int> not_single_chars = [];
            List<char> alreadyUsedChars = [];
            foreach (char c in word)
            {
                int cIndex = word.IndexOf(c);
                if (hiddenWord[cIndex] == c && !alreadyUsedChars.Contains(c))
                {
                    AnsiConsole.Markup("[black on green]{0}[/]", c);
                    alreadyUsedChars.Add(c);
                }
                else if (hiddenWord.Contains(c))
                {
                    if (not_single_chars.ContainsKey(c))
                    {
                        if (alreadyUsedChars.Contains(c))
                        {
                            AnsiConsole.Markup("[black on red]{0}[/]", c);
                        }
                        else {
                            AnsiConsole.Markup("[black on yellow]{0}[/]", c);
                        }
                        if (--not_single_chars[c] == 0)
                        {
                            not_single_chars.Remove(c);
                        }
                        continue;
                    }
                    if (alreadyUsedChars.Contains(c))
                    {
                        AnsiConsole.Markup("[black on red]{0}[/]", c);
                    }
                    else {
                        AnsiConsole.Markup("[black on yellow]{0}[/]", c);
                    }
                    int counts = hiddenWord.Count(ch => ch == c);
                    if (counts > 1)
                    {
                        not_single_chars.Add(c, counts - 1);
                    }
                }
                else
                {
                    AnsiConsole.Markup("[black on red]{0}[/]", c);
                }
            }
            Console.WriteLine();
            if (hiddenWord == word)
            {
                Console.WriteLine("Поздравляю с победой! Перезапустите игру, чтобы сыграть ещё!");
                return;
            }
        }
        Console.WriteLine("К сожалению, вы проиграли. Загаданное слово было: {0}", hiddenWord);
        Console.WriteLine("Перезапустите программу чтобы сыграть ещё!");
    }
}