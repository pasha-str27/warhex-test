//5. �������� �������� numwrds, ��� �������� ������� ��� � ��������� �����.
#include <iostream>
#include <string>
#include <Windows.h>
#include <string.h>
#include <fstream>
using namespace std;

int main(int argc, char** argv)
{
    SetConsoleOutputCP(1251);

    //�� ������� ����������� �������� ����������
    bool ask_words = true;

    //������� ������ � �����
    bool read_file = false;

    //�������� ��������� ���������� �����
    if (argc > 1)
    {
        for (int i = 1; i < argc; ++i)
        {
            //��������� ������ ��� ��������
            if (strcmp(argv[i], "-h") == 0)
                cout << "-v - ����� ��������\n-f - ������� ��� � �����\n";

            //���������� ��������� ��� � �����
            if (strcmp(argv[i], "-f") == 0)
            {
                //�������� �� ��������� �������� -� � -f
                for (int j = i + 1; j < argc; ++j)
                    if (strcmp(argv[j], "-c") == 0)
                    {
                        cout << "������� ��������!" << endl;
                        return 1;
                    }

                read_file = true;
                break;
            }

            //��������� ������� ��� � ���������� �����
            if (strcmp(argv[i], "-c") == 0)
            {
                //�������� �� ��������� �������� -� � -f
                for (int j = i + 1; j < argc; ++j)
                    if (strcmp(argv[j], "-f") == 0)
                    {
                        cout << "������� ��������!" << endl;
                        return 1;
                    }
                cout << "������� ���: " << argc - i - 1 << endl;
                ask_words = false;
            }

            //��������� ������ ��� ��������
            if (strcmp(argv[i], "-v") == 0)
                std::cout << "����� 0.99\n";
        }
    }

    //��� ����� ��� ������ ��������
    if (ask_words)
    {
        //������� ����� � ����� ��� ������� ����
        string str;
        if (!read_file)
        {
            cout << "������ �����: ";
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


        //������� ��� � �����
        int counter = 0;

        //����� ������� ��� � �����
        for (int i = 1; i <= str.size(); ++i)
            if (isalpha(str[i - 1]) && !isalpha(str[i]))
                ++counter;

        //��������� ���'��
        str.clear();

        //���� ������� ���
        cout << "������� ���: " << counter << endl;
    }

    return 0;
}
//not so good as it could be but OK
//some configuration reading
//one more nice code block
