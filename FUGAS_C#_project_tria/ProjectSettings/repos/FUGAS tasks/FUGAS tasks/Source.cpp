//5. Напишіть програму numwrds, яка підраховує кількість слів у введеному рядку.
#include <iostream>
#include <string>
#include <Windows.h>
#include <string.h>
#include <fstream>
using namespace std;

int main(int argc, char** argv)
{
    SetConsoleOutputCP(1251);

    //чи потрібно користувати основним алгоритмом
    bool ask_words = true;

    //читання тексту з файлу
    bool read_file = false;

    //перевірка параметрів командного рядка
    if (argc > 1)
    {
        for (int i = 1; i < argc; ++i)
        {
            //виведення довідки про програму
            if (strcmp(argv[i], "-h") == 0)
                cout << "-v - версія програми\n-f - зчитати дані з файлу\n";

            //дозволяємо зчитувати дані з файлу
            if (strcmp(argv[i], "-f") == 0)
            {
                //перевірка на одночасне введення -с і -f
                for (int j = i + 1; j < argc; ++j)
                    if (strcmp(argv[j], "-c") == 0)
                    {
                        cout << "Помилка введення!" << endl;
                        return 1;
                    }

                read_file = true;
                break;
            }

            //виведення кількості слів в командному рядку
            if (strcmp(argv[i], "-c") == 0)
            {
                //перевірка на одночасне введення -с і -f
                for (int j = i + 1; j < argc; ++j)
                    if (strcmp(argv[j], "-f") == 0)
                    {
                        cout << "Помилка введення!" << endl;
                        return 1;
                    }
                cout << "кількість слів: " << argc - i - 1 << endl;
                ask_words = false;
            }

            //виведення довідки про програму
            if (strcmp(argv[i], "-v") == 0)
                std::cout << "Версія 0.99\n";
        }
    }

    //ввід рядка для роботи програми
    if (ask_words)
    {
        //зчитуємо текст з файлу або вводимо його
        string str;
        if (!read_file)
        {
            cout << "Введіть рядок: ";
            getline(cin, str);
        }
        else
        {
            ifstream read_data(argv[argc - 1]);
            string line;
            while (getline(read_data, line))
                str += line + "\n";
            read_data.close();
        }


        //кількість слів у рядку
        int counter = 0;

        //пошук кількості слів у рядку
        for (int i = 1; i <= str.size(); ++i)
            if (isalpha(str[i - 1]) && !isalpha(str[i]))
                ++counter;

        //звільнення пам'яті
        str.clear();

        //вивід кількості слів
        cout << "кількість слів: " << counter << endl;
    }

    return 0;
}
//not so good as it could be but OK
//some configuration reading
//one more nice code block
