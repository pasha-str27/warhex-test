#define _CRT_SECURE_NO_DEPRECATE
#include <stdio.h>
#include <string.h>
#include <malloc.h>

//Дано файл, що містить випадкову послідовність 0 і 1. 
//Написати програму, яка відсортує файл таким чином,
//щоб усі 0 були раніше 1. Використати сортування вставкою.

int read_file(char*file_name)
{
	FILE* fin=fopen("file.txt", "r");
	char tmp='1';
	int count0=0;
	int count1 = 0;
	while (!feof(fin))
	{
		fscanf(fin, "%c", &tmp);
		if (tmp == '1')
			count1++;
		else
			count0++;
	}
	if(fin)
		fclose(fin);

	FILE* fout = fopen("file.txt", "w");
	for (int i = 0; i < count0; ++i)
	{
		fprintf(fout, "%d", 0);
	}
	for (int i = 0; i < count1; ++i)
	{
		fprintf(fout, "%d", 1);
	}

	if (fout)
		fclose(fout);

	return 0;
}

void write_to_file(char* a, int size/*, char* file_name*/)
{
	FILE* fout = fopen("file.txt"/*strcat(file_name, ".txt")*/, "w");
	
	for (int i = 0; i < size; ++i)
	{
		//printf("%c", a[i]);
		fprintf(fout,"%c", a[i]);
		//putc(a[i], fout);
	}

		//fprintf(fout, "%s",a[i]);
	if (fout)
		fclose(fout);
}

//сортування методом вставки
void sort(char* a, int size)
{


	for (int i = 1; i < size; i++) 
	{
		// і починаємо переміщення елементів зліва від нього
// поки елемент в пам'яті не стане меншим ніж попередній 
		for (int j = i; j > 0 && a[j - 1] > a[j]; j--)
		{
			char tmp = a[j - 1];
			a[j - 1] = a[j];// і поставимо записаний елемент на його нове місце 
			a[j] = tmp;
		}
	}
	//кінець сортування
}

int main()
{
	int size = 1;

	char* a = (char*)malloc(size * sizeof(char));;

	char file_name[25];

	printf("file name: ");
	fgets(file_name, 25, stdin);


	size=read_file(file_name);

	printf("%d", strlen(a));

	sort(a, size);

	free(a);

	system("pause");
	return 0;
}