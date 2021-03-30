#include <iostream>
#include <vector>
#include <Windows.h>
#include <time.h>

int count_compare;//кількість порівнянь

//функція лінійного пошуку
int linear_search(std::vector<int>elements, int element)
{
	//проходимося по усіх елементах
	for (int i = 0; i < elements.size(); ++i)
	{
		//порівнюємо елемерти та збільшуємо лічильник порінянь
		++count_compare;
		if (elements[i] == element)//якщо знайшли елемент, повертаємо його порядковий номер
			return i;
	}
	//якщо не знайшли елемент, повертаємо -1
	return -1;
}

int main()
{
	SetConsoleOutputCP(1251);//для виводу кирилиці
	srand(time(NULL));//для постійної генерації нових даних

	std::vector<int>elements;//вектор для зберігання елементів для пошуку
	int value_for_search = rand() % 700-200;//генеруємо змінну для пошуку

	//виводимо невеличкий заголовок
	std::cout << "Кількість елементів\tкількість порівнянь\n";
	std::cout << "-----------------------------------------------\n";

	//для векторі розміром від 100 до 1000 з кроком 100
	for (int i = 100; i <= 1000; i += 100)
	{
		//генеруємо значення вектора та добавляємо це значення в вектор
		for (int j = 0; j < i; ++j)
			elements.push_back(rand() % 500);

		//обнуляємо лічильник порівнянь
		count_compare = 0;

		//шукаємо елемент у векторі
		linear_search(elements, value_for_search);

		//виводимо на екран розмір вектора та кількість порівнянь, потрібних для знаходження елемента
		std::cout << i << "\t\t\t\t" << count_compare << std::endl;

		//очищаємо екран
		elements.clear();
	}

	//затримка результатів на екрані
	system("pause");
	return 0;
}