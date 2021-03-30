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
            //����������� ������
            sequence_ = NULL;
            size_ = 0;
        }

        // Copy-constructor.
        Points2D(const Points2D& rhs)
        {
            //������� �����
            size_ = rhs.size();

            //��������� ���'���
            if (this->sequence_ != NULL)
                delete[]this->sequence_;

            //�������� ���'���
            this->sequence_=new std::array<Object, 2>[size_];

            //������� �������� �����
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
            //������� ������� ��������
            Points2D tmp(*this);

            //������� ��������� ��������� ��'����
            size_ = rhs.size();

            //��������� ���'���
            if (sequence_ != NULL)
                delete[]sequence_;

            //������ �������� ���'���
            sequence_ = new std::array<Object, 2>[size_];

            //������� �������� � �������� ��'���
            for (int i = 0; i < size_; ++i)
            {
                this->sequence_[i][0] = rhs[i][0];
                this->sequence_[i][1] = rhs[i][1];
            }

            //��������� ���'��� � ���������� ��'����
            if (rhs.sequence_ != NULL)
                delete[]rhs.sequence_;

            //������� ���������
            rhs.size_ = tmp.size();

            //�������� ���'��� �� ��������� ��'���
            rhs.sequence_ = new std::array<Object, 2>[rhs.size_];

            //�� �������� �������� � ��������� ��'���
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
            //������� ������� ��������
            Points2D tmp(*this);

            //������� ��������� ��������� ��'����
            size_ = rhs.size();

            //��������� ���'���
            if (sequence_ != NULL)
                delete[]sequence_;

            //������ �������� ���'���
            sequence_ = new std::array<Object, 2>[size_];

            //������� �������� � �������� ��'���
            for (int i = 0; i < size_; ++i)
            {
                this->sequence_[i][0] = rhs[i][0];
                this->sequence_[i][1] = rhs[i][1];
            }

            //��������� ���'��� � ���������� ��'����
            if (rhs.sequence_ != NULL)
                delete[]rhs.sequence_;

            //������� ���������
            rhs.size_ = tmp.size();

            //�������� ���'��� �� ��������� ��'���
            rhs.sequence_ = new std::array<Object, 2>[rhs.size_];

            //�� �������� �������� � ��������� ��'���
            for (int i = 0; i < rhs.size_; ++i)
            {
                rhs.sequence_[i][0]= tmp.sequence_[i][0];
                rhs.sequence_[i][1] = tmp.sequence_[i][1];
            }

            //��������� �������� ��'���
            return *this;
        }

        ~Points2D()
        {
            //��������� ���'���
            if (sequence_ != NULL)
                delete[]sequence_;
        }

        // End of big-five.

        // One parameter constructor.
        Points2D(const std::array<Object, 2>* item,int size=1) 
        {
            //�������� ���'���
            sequence_ = new std::array<Object, 2>[size];

            //������� �������� ���������� ������ � ����������� �����
            for (int i = 0; i < size; ++i)
            {
                sequence_[i][0] = item[i][0];
                sequence_[i][1] = item[i][1];
            }

            //��������� ���������
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

            //��������� ���'��
            if (this->sequence_ != NULL)
                delete[]this->sequence_;

            //������ �������� ���'���
            this->sequence_ = new std::array<Object, 2>[size_of_sequence];

            //��������� ���������
            this->size_ = size_of_sequence;

            //����� ����� � �����������
            Object token;
            for (int i = 0; input_stream >> token; ++i)
            {
                //��������� �������� �
                this->sequence_[i][0] = token;
                input_stream >> token;
                //��������� �������� �
                this->sequence_[i][1] = token;
            }

        }

        size_t size() const 
        {
            //��������� ����� ����������� �����
            return size_;
        }

        // @location: an index to a location in the given sequence.
        // @returns the point at @location.
        // const version.
        // abort() if out-of-range.
        const std::array<Object, 2>& operator[](size_t location) const
        {
            //���������� �� �� ������ �� �� ��� �����������
            if (location >= size_ || location < 0)
                throw std::out_of_range("out_of_range");

            //�� ��������� ������� �� ������� ��������
            return sequence_[location];
        }

        //  @c1: A sequence.
        //  @c2: A second sequence.
        //  @return their sum. If the sequences are not of the same size, append the
        //    result with the remaining part of the larger sequence.
        friend Points2D operator+(const Points2D& c1, const Points2D& c2)
        {
            //�������� ����� ���� ����������� �� ����������� �������� ������ �� ��������� ��'����
            int size = (c1.size() > c2.size()) ? c1.size() : c2.size();

            //�������� ���'��� �� �����
            std::array<double, 2>* summ = new std::array<double, 2>[size];

            //��������� �� ��������
            for (int i = 0; i < size; ++i)
            {
                summ[i][0] = 0;
                summ[i][1] = 0;
            }

            //������ �������� ����� �����������
            for (int i = 0; i < c1.size(); ++i)
            {
                summ[i][0] += c1[i][0];
                summ[i][1] += c1[i][1];
            }

            //�� �����
            for (int i = 0; i < c2.size(); ++i)
            {
                summ[i][0] += c2[i][0];
                summ[i][1] += c2[i][1];
            }

            //������� ��'��� �� ���� ��������� ����
            Points2D<double> s(summ, size);

            //��������� ���'��
            delete[]summ;

            return s;
        }

        // Overloading the << operator.
        friend std::ostream& operator<<(std::ostream& out, const Points2D& some_points2d)
        {
            //� ���� �������� ���������� ��� �� ����� �� � � �����������
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
