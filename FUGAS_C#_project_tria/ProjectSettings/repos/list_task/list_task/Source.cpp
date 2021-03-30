#include <iostream>
#include <Windows.h>

struct date//структура для дати
{
    int year;
    int mounth;
    int day;
    int god;
    int minut;
};

bool operator>(const date& a1, const date& a2)//порівняння двох дат
{
    if (a1.year > a2.year)
        return true;
    if(a1.year < a2.year)
        return false;
    if (a1.mounth > a2.mounth)
        return true;
    if (a1.mounth < a2.mounth)
        return false;
    if (a1.day > a2.day)
        return true;
    if (a1.day < a2.day)
        return false;
    if (a1.minut > a2.minut)
        return true;
    if (a1.minut < a2.minut)
        return false;
}

bool operator==(const date& a1, const date& a2)//порівняння на рівність двох дат
{
    return a1.year== a2.year&& a1.mounth == a2.mounth && a1.day == a2.day&&a1.god == a2.god && a1.minut == a2.minut;
}

struct node //структура вузла
{
    node* next;
    int car_number;
    std::string surname;
    date start_time;
    date end_time;
    float tarif;
};

void print_node(const node* p)//вивід на екран значень вузла
{
    std::cout << "| " << p->car_number << " | " << p->surname << " | " << p->start_time.year << "-" << p->start_time.mounth << "-" << p->start_time.day << ", "
        << p->start_time.god << ":" << p->start_time.minut << " | " << p->end_time.year << "-" << p->end_time.mounth << "-" << p->end_time.day << ", "
        << p->end_time.god << ":" << p->end_time.minut << " | " << p->tarif << " |\n";
}

class list//клас списку
{
    node* lst;

public:
    list():lst(NULL) {}//конструктор

    ~list() //деструктор
    {
        clear();
    }

    //додавання нового вузла
    void add(node &element)
    {
        node* p = new node();
        if (p != NULL) 
        {
            p->car_number=element.car_number;
            p->surname= element.surname;
            p->start_time= element.start_time;
            p->end_time= element.end_time;
            p->tarif= element.tarif;

            p->next = lst;
            lst = p;
        }
    }

    //сортування бульбашками за номером авто
    void sort_by_car_number()
    {
        node* t, * m, * a, * b;
        if (lst == NULL)
            return;

        for (bool go = true; go; ) 
        {
            go = false;
            a = t = lst;
            b = lst->next;

            while (b != NULL)
            {
                if (a->car_number > b->car_number)
                {
                    if (t == a)
                        lst = b;
                    else
                        t->next = b;

                    a->next = b->next;
                    b->next = a;

                    m = a, a = b, b = m;
                    go = true;
                }
                t = a;
                a = a->next;
                b = b->next;
            }
        }
    }

    //сортування бульбашками за початком стоянки
    void sort_by_starttime()
    {
        node* t, * m, * a, * b;
        if (lst == NULL)
            return;

        for (bool go = true; go; )
        {
            go = false;
            a = t = lst;
            b = lst->next;

            while (b != NULL)
            {
                if (a->start_time > b->start_time)
                {
                    if (t == a)
                        lst = b;
                    else
                        t->next = b;

                    a->next = b->next;
                    b->next = a;

                    m = a, a = b, b = m;
                    go = true;
                }
                t = a;
                a = a->next;
                b = b->next;
            }
        }
    }

    //сортування бульбашками за прізвищем
    void sort_by_surname()
    {
        node* t, * m, * a, * b;
        if (lst == NULL)
            return;

        for (bool go = true; go; )
        {
            go = false;
            a = t = lst;
            b = lst->next;

            while (b != NULL)
            {
                if (a->surname > b->surname)
                {
                    if (t == a)
                        lst = b;
                    else
                        t->next = b;

                    a->next = b->next;
                    b->next = a;

                    m = a, a = b, b = m;
                    go = true;
                }
                t = a;
                a = a->next;
                b = b->next;
            }
        }
    }

    //пошук за прізвищем
    void find_by_surname(std::string surname)
    {
        for (const node* p = lst; p != NULL; p = p->next)
            if (p->surname == surname)//якщо знайдено
            {
                print_node(p);//то виводимо значення
                return;
            }
        std::cout << "Запис не знайдено!\n";
    }

    //пошук за номером авто
    void find_by_carnumber(int car_number)
    {
        for (const node* p = lst; p != NULL; p = p->next)
            if (p->car_number == car_number)
            {
                print_node(p);
                return;
            }
        std::cout << "Запис не знайдено!\n";
    }

    //пошук за початком стоянки
    void find_by_starttime(date start_time)
    {
        for (const node* p = lst; p != NULL; p = p->next)
            if (p->start_time == start_time)
            {
                print_node(p);
                return;
            }
        std::cout << "Запис не знайдено!\n";
    }

    //видалення всіх
    void clear() 
    {
        node* t;
        while (lst != NULL) {
            t = lst;
            lst = lst->next;
            delete t;
        }
    }

    //виведення на екран всіх вузлів
    void print()
    {
        std::cout << "| Номер авто | Прізвище | дата початку стоянки | дата кінця стоянки | тариф за годину |\n";
        for (const node* p = lst; p != NULL; p = p->next)
            print_node(p);
        std::cout << std::endl;
    }
};


int main()
{
    //зміна кодування для виводу кирилиці
    SetConsoleCP(1251);
    SetConsoleOutputCP(1251);

    list list_;//об'єкт списку

    int q = -1;
    while (q != 0)
    {
        system("cls");
        //вибираємо бажану дію
        std::cout << "1-додати новий запис" << std::endl;
        std::cout << "2-вивести всі записи" << std::endl;
        std::cout << "3-відсортувати за номером авто" << std::endl;
        std::cout << "4-відсортувати за прізвищем" << std::endl;
        std::cout << "5-відсортувати за початком стоянки" << std::endl;
        std::cout << "6-знайти за номером авто" << std::endl;
        std::cout << "7-знайти за прізвищем" << std::endl;
        std::cout << "8-знайти за початком стоянки" << std::endl;
        std::cout << "0-вихід" << std::endl;
        std::cin >> q;

        switch (q)//виконуємо дію відповідно до обраного пункту
        {
        case 1://додавання нового елемента
        {
            std::cout << "введіть елемент:\n";
            node new_element;
            std::cout << "номер авто: ";
            std::cin >> new_element.car_number;
            std::cout << "прізвище: ";
            std::cin >> new_element.surname;
            std::cout << "тариф: ";
            std::cin >> new_element.tarif;
            std::cout << "Початок стоянки: ";
            std::cout << "\tрік: ";
            std::cin >> new_element.start_time.year;
            std::cout << "\tмісяць: ";
            std::cin >> new_element.start_time.mounth;
            std::cout << "\tдень: ";
            std::cin >> new_element.start_time.day;
            std::cout << "\tгодина: ";
            std::cin >> new_element.start_time.god;
            std::cout << "\tхвилина: ";
            std::cin >> new_element.start_time.minut;
            std::cout << "кінець стоянки: ";
            std::cout << "\tрік: ";
            std::cin >> new_element.end_time.year;
            std::cout << "\tмісяць: ";
            std::cin >> new_element.end_time.mounth;
            std::cout << "\tдень: ";
            std::cin >> new_element.end_time.day;
            std::cout << "\tгодина: ";
            std::cin >> new_element.end_time.god;
            std::cout << "\tхвилина: ";
            std::cin >> new_element.end_time.minut;
            list_.add(new_element);
            break;
        }
        case 2://вивід елементів
        {
            list_.print();
            break;
        }
        case 3://сортування за номером авто
        {
            list_.sort_by_car_number();
            break;
        }
        case 4://сортування за прізвищем
        {
            list_.sort_by_surname();
            break;
        }
        case 5://сортування за початком стоянки
        {
            list_.sort_by_starttime();
            break;
        }
        case 6://пошук за номером авто
        {
            int number;
            std::cout << "Номер авто: ";
            std::cin >> number;
            list_.find_by_carnumber(number);
            break;
        }
        case 7://пршук за прізвищем
        {
            std::string surname;
            std::cout << "Прізвище: ";
            std::cin >> surname;
            list_.find_by_surname(surname);
            break;
        }
        case 8://пошук за початком стоянки
        {
            date start_time;
            std::cout << "Початок стоянки: ";
            std::cout << "\tрік: ";
            std::cin >> start_time.year;
            std::cout << "\tмісяць: ";
            std::cin >> start_time.mounth;
            std::cout << "\tдень: ";
            std::cin >> start_time.day;
            std::cout << "\tгодина: ";
            std::cin >> start_time.god;
            std::cout << "\tхвилина: ";
            std::cin >> start_time.minut;
            list_.find_by_starttime(start_time);
            break;
        }
        default:
            break;
        }
        system("pause");
    }
    return 0;
}