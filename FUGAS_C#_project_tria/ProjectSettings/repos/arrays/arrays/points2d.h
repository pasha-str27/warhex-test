// --> YOUR NAME here
// Few comments describing the class Points2D

#ifndef HOMEWORK2_POINTS2D_H_
#define HOMEWORK2_POINTS2D_H_

#include <array>
#include <iostream>
#include <cstddef>
#include <string>
#include <sstream>

namespace teaching_project {
    // Place comments that provide a brief explanation of this class,
    // and its sample usage.
    template<typename Object>
    class Points2D 
    {
    public:
        // Default "big five" -- you have to alter them for your assignment.
        // That means that you will remove the "= default" statement.
        //  and you will provide an implementation for it.

        // Zero-parameter constructor. 
        // Set size to 0.
        Points2D()
        {
            //ініціалізація нулями
            sequence_ = NULL;
            size_ = 0;
        }

        // Copy-constructor.
        Points2D(const Points2D& rhs)
        {
            //копіюємо розмір
            size_ = rhs.size();

            //видаляємо пам'ять
            if (this->sequence_ != NULL)
                delete[]this->sequence_;

            //виділяємо пам'ять
            this->sequence_=new std::array<Object, 2>[size_];

            //копіюємо значення точок
            for (int i = 0; i < size_; ++i)
            {
                sequence_[i][0] = rhs[i][0];
                sequence_[i][1] = rhs[i][1];
            }
        }

        // Copy-assignment. If you have already written
        // the copy-constructor and the move-constructor
        // you can just use:
        // {
        // Points2D copy = rhs; 
        // std::swap(*this, copy);
        // return *this;
        // }
        Points2D& operator=(const Points2D& rhs)
        {
             Points2D copy = rhs; 
             std::swap(*this, copy);
             return *this;
        }

        // Move-constructor. 
        Points2D(Points2D&& rhs)
        {
            //копіюємо поточне значення
            Points2D tmp(*this);

            //змінюємо розмірність поточного об'єкта
            size_ = rhs.size();

            //звільняємо пам'ять
            if (sequence_ != NULL)
                delete[]sequence_;

            //наново виділяємо пам'ять
            sequence_ = new std::array<Object, 2>[size_];

            //копіюємо значення в поточний об'єкт
            for (int i = 0; i < size_; ++i)
            {
                this->sequence_[i][0] = rhs[i][0];
                this->sequence_[i][1] = rhs[i][1];
            }

            //звільняємо пам'ять з переданого об'єкта
            if (rhs.sequence_ != NULL)
                delete[]rhs.sequence_;

            //копіюємо розмірність
            rhs.size_ = tmp.size();

            //виділяємо пам'ять під переданий об'єкт
            rhs.sequence_ = new std::array<Object, 2>[rhs.size_];

            //та копіюємло елементи в переданий об'єкт
            for (int i = 0; i < rhs.size_; ++i)
            {
                rhs.sequence_[i][0] = tmp.sequence_[i][0];
                rhs.sequence_[i][1] = tmp.sequence_[i][1];
            }
        }

        // Move-assignment.
        // Just use std::swap() for all variables.
        Points2D& operator=(Points2D&& rhs)
        {
            //копіюємо поточне значення
            Points2D tmp(*this);

            //змінюємо розмірність поточного об'єкта
            size_ = rhs.size();

            //звільняємо пам'ять
            if (sequence_ != NULL)
                delete[]sequence_;

            //наново виділяємо пам'ять
            sequence_ = new std::array<Object, 2>[size_];

            //копіюємо значення в поточний об'єкт
            for (int i = 0; i < size_; ++i)
            {
                this->sequence_[i][0] = rhs[i][0];
                this->sequence_[i][1] = rhs[i][1];
            }

            //звільняємо пам'ять з переданого об'єкта
            if (rhs.sequence_ != NULL)
                delete[]rhs.sequence_;

            //копіюємо розмірність
            rhs.size_ = tmp.size();

            //виділяємо пам'ять під переданий об'єкт
            rhs.sequence_ = new std::array<Object, 2>[rhs.size_];

            //та копіюємло елементи в переданий об'єкт
            for (int i = 0; i < rhs.size_; ++i)
            {
                rhs.sequence_[i][0]= tmp.sequence_[i][0];
                rhs.sequence_[i][1] = tmp.sequence_[i][1];
            }

            //повертаємо поточний об'єкт
            return *this;
        }

        ~Points2D()
        {
            //звільняємо пам'ять
            if (sequence_ != NULL)
                delete[]sequence_;
        }

        // End of big-five.

        // One parameter constructor.
        Points2D(const std::array<Object, 2>* item,int size=1) 
        {
            //виділяємо пам'ять
            sequence_ = new std::array<Object, 2>[size];

            //копіюємо елементи переданого масиву в послідовність точок
            for (int i = 0; i < size; ++i)
            {
                sequence_[i][0] = item[i][0];
                sequence_[i][1] = item[i][1];
            }

            //присвоєння розмірності
            size_ = size;
        }

        // Read a chain from standard input.
        void ReadPoints2D()
        {
            // Part of code included (without error checking).
            std::string input_line;
            std::getline(std::cin, input_line);
            std::stringstream input_stream(input_line);
            if (input_line.empty()) return;
            // Read size of sequence (an integer).
            int size_of_sequence;
            input_stream >> size_of_sequence;
            // Allocate space for sequence.

            //звільнення пам'яті
            if (this->sequence_ != NULL)
                delete[]this->sequence_;

            //наново виділяємо пам'ять
            this->sequence_ = new std::array<Object, 2>[size_of_sequence];

            //присвоєння розмірності
            this->size_ = size_of_sequence;

            //запис даних в послідовність
            Object token;
            for (int i = 0; input_stream >> token; ++i)
            {
                //присвоєння значення х
                this->sequence_[i][0] = token;
                input_stream >> token;
                //присвоєння значення у
                this->sequence_[i][1] = token;
            }

        }

        size_t size() const 
        {
            //повертаємо розмір послідовності точок
            return size_;
        }

        // @location: an index to a location in the given sequence.
        // @returns the point at @location.
        // const version.
        // abort() if out-of-range.
        const std::array<Object, 2>& operator[](size_t location) const
        {
            //перевіряємо чи не вийшли ми за межі послідовності
            if (location >= size_ || location < 0)
                throw std::out_of_range("out_of_range");

            //та повертаємо елемент із заданим індексом
            return sequence_[location];
        }

        //  @c1: A sequence.
        //  @c2: A second sequence.
        //  @return their sum. If the sequences are not of the same size, append the
        //    result with the remaining part of the larger sequence.
        friend Points2D operator+(const Points2D& c1, const Points2D& c2)
        {
            //вибираємо розмір нової послідовності як максимальне значення розміру із переданих об'єктів
            int size = (c1.size() > c2.size()) ? c1.size() : c2.size();

            //виділяємо пам'ять під масив
            std::array<double, 2>* summ = new std::array<double, 2>[size];

            //занулюємо всі значення
            for (int i = 0; i < size; ++i)
            {
                summ[i][0] = 0;
                summ[i][1] = 0;
            }

            //додаємо значення першої послідовності
            for (int i = 0; i < c1.size(); ++i)
            {
                summ[i][0] += c1[i][0];
                summ[i][1] += c1[i][1];
            }

            //та другої
            for (int i = 0; i < c2.size(); ++i)
            {
                summ[i][0] += c2[i][0];
                summ[i][1] += c2[i][1];
            }

            //формуємо об'єкт та потім повертаємо його
            Points2D<double> s(summ, size);

            //звільнення пам'яті
            delete[]summ;

            return s;
        }

        // Overloading the << operator.
        friend std::ostream& operator<<(std::ostream& out, const Points2D& some_points2d)
        {
            //в циклі виводимо інформацію про всі точки що є в послідовності
            for(int i=0; i<some_points2d.size();++i)
                out << "(" << some_points2d[i][0] << "," << some_points2d[i][1] << ") ";
            out << "\n";
            return out;
        }

    private:
        // Sequence of points. 
        std::array<Object, 2>* sequence_;
        // Size of the sequence.
        size_t size_;
    };

}  // namespace teaching_project
#endif // HOMEWORK2_POINTS2D_H_
