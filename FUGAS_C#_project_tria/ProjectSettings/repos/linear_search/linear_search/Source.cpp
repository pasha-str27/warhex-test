#include <iostream>
#include <vector>
#include <Windows.h>
#include <time.h>

int count_compare;//������� ��������

//������� ������� ������
int linear_search(std::vector<int>elements, int element)
{
	//����������� �� ��� ���������
	for (int i = 0; i < elements.size(); ++i)
	{
		//��������� �������� �� �������� �������� �������
		++count_compare;
		if (elements[i] == element)//���� ������� �������, ��������� ���� ���������� �����
			return i;
	}
	//���� �� ������� �������, ��������� -1
	return -1;
}

int main()
{
	SetConsoleOutputCP(1251);//��� ������ ��������
	srand(time(NULL));//��� ������� ��������� ����� �����

	std::vector<int>elements;//������ ��� ��������� �������� ��� ������
	int value_for_search = rand() % 700-200;//�������� ����� ��� ������

	//�������� ���������� ���������
	std::cout << "ʳ������ ��������\t������� ��������\n";
	std::cout << "-----------------------------------------------\n";

	//��� ������ ������� �� 100 �� 1000 � ������ 100
	for (int i = 100; i <= 1000; i += 100)
	{
		//�������� �������� ������� �� ���������� �� �������� � ������
		for (int j = 0; j < i; ++j)
			elements.push_back(rand() % 500);

		//��������� �������� ��������
		count_compare = 0;

		//������ ������� � ������
		linear_search(elements, value_for_search);

		//�������� �� ����� ����� ������� �� ������� ��������, �������� ��� ����������� ��������
		std::cout << i << "\t\t\t\t" << count_compare << std::endl;

		//������� �����
		elements.clear();
	}

	//�������� ���������� �� �����
	system("pause");
	return 0;
}